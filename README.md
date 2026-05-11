# MSSQL CDC → Kafka → Elasticsearch Pipeline (.NET Clean Architecture + CQRS)

This project implements a data streaming pipeline that captures database
changes from MSSQL using CDC, streams them through Kafka using Debezium,
processes them via a .NET application built with Clean Architecture and CQRS,
and indexes them in Elasticsearch for Query APIs.

Traditional systems rely on batch jobs to synchronize data between services.
This introduces delays and heavy database load.

This project demonstrates a real‑time data pipeline using:

- SQL Server Change Data Capture (CDC)
- Debezium for change event streaming
- Kafka for event transport
- .NET application implementing Clean Architecture + CQRS
- Elasticsearch for indexing and querying

# Prerequisites

This project requires tools as prerequisites, by running the docker compose file, the following images are added:

- debezium/connect
- kibana/kibana
- logstash/logstash
- provectuslabs/kafka-ui
- confluentinc/cp-kafka
- elasticsearch/elasticsearch
- confluentinc/cp-zookeeper

# Data Flow Explanation

This system uses the Transactional Outbox Pattern combined with Change Data Capture (CDC) to propagate domain changes from SQL Server to Elasticsearch in near real‑time.

The .NET application produces events by writing them to an Outbox table, while Logstash is responsible for consuming events from Kafka and indexing them into Elasticsearch.

## 1. Application Layer (.NET – Clean Architecture + CQRS)

The process begins in the .NET application, which follows Clean Architecture and CQRS principles.

When a command modifies a domain aggregate:

The domain state is persisted to the database.
An event describing the change is written to an Outbox table within the same database transaction.
This guarantees atomicity between the business operation and the event creation.

```
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity);

        var outboxMessages = new List<OutboxMessage>();

        foreach (var entity in domainEntities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredOn = domainEvent.OccurredOn,
                    Type = domainEvent.GetType().Name,
                    Content = JsonSerializer.Serialize(
                        domainEvent,
                        domainEvent.GetType()
                    )
                };

                outboxMessages.Add(outboxMessage);
            }

            entity.ClearDomainEvents();
        }

        await OutboxMessages.AddRangeAsync(outboxMessages, cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }
```

Important:

The .NET application does not publish events directly to Kafka and does not consume events to update Elasticsearch. Its only responsibility is writing events to the Outbox table.

## 2. SQL Server Change Data Capture (CDC)
CDC is enabled on the Outbox table in SQL Server.

CDC monitors the transaction log and captures row-level changes made to the Outbox table.

Whenever a new event record is inserted, CDC records the change without requiring triggers or polling logic inside the application.

To activate cdc:

```
EXEC sys.sp_cdc_enable_db;

EXEC sys.sp_cdc_enable_table
@source_schema = 'dbo',
@source_name   = 'OutboxMessages',
@role_name     = NULL;

```
Note:

SQLSERVER AGENT MUST BE RUNNIG!

## 3. Debezium Connector
Debezium connects to SQL Server and reads the captured CDC changes.

Debezium converts the database change events into structured JSON messages and publishes them to Kafka topics.

debezium connector use http protocol, to create a connector use the following api and body, note that change the config based on your system:

```
curl --request POST \
  --url http://localhost:8083/connectors \
  --header 'content-type: application/json' \
  --data '{
  "name": "sqlserver-outbox-connector",
  "config": {
    "connector.class": "io.debezium.connector.sqlserver.SqlServerConnector",

    "database.hostname": "host.docker.internal",
    "database.port": "1433",
    "database.user": "sa",
    "database.password": "123",
    "database.names": "Mssql-Debezium-Kafka-Elk",
    "database.encrypt": "false",

    "topic.prefix": "inventory",
    "table.include.list": "dbo.OutboxMessages",

    "schema.history.internal.kafka.bootstrap.servers": "kafka:29092",
    "schema.history.internal.kafka.topic": "schema-changes.inventory",

    "tasks.max": "1",
    "include.schema.changes": "false",

    "transforms": "outbox",
    "transforms.outbox.type": "io.debezium.transforms.outbox.EventRouter",

    "transforms.outbox.route.by.field": "Type",
    "transforms.outbox.route.topic.replacement": "outbox.event.${routedByValue}",

    "transforms.outbox.table.field.event.id": "Id",
    "transforms.outbox.table.field.event.key": "Id",
    "transforms.outbox.table.field.event.payload": "Content",

    "transforms.outbox.table.fields.additional.placement": "Type:header:eventType"
  }
}
'
```

## 4. Apache Kafka
Kafka acts as the event streaming backbone of the system.

Debezium publishes events to Kafka topics.
Events are stored durably and can be replayed if needed.
Consumers can process events independently from the producing application.
Kafka therefore decouples the event producer (.NET application) from the event consumer (Logstash).

## 5. Logstash Processing (Kafka → Elasticsearch)
Logstash is responsible for consuming Kafka events and transforming them before indexing them into Elasticsearch.

### Kafka Input
Logstash subscribes to all Outbox event topics.

```
input {
  kafka {
    bootstrap_servers => "kafka:29092"
    topics_pattern => "outbox\.event.*"
    group_id => "logstash-group"
    codec => json
    auto_offset_reset => "earliest"
    decorate_events => false
  }
}
```
This configuration allows Logstash to automatically consume events from any topic that matches the outbox.event.* pattern.

### Event Transformation
Logstash processes the incoming messages using filters.

```
filter {
  # Parse the JSON string inside "payload"
  json {
    source => "payload"
  }

  # Lowercase index name for Elasticsearch
  mutate {
    lowercase => ["AggregateType"]
  }

  # Remove noisy fields
  mutate {
    remove_field => [
      "payload",
      "schema",
      "event",
      "@version",
      "@timestamp"
    ]
  }
}
```
During this stage Logstash:

- Extracts the domain data from the payload field
- Normalizes the aggregate type for use as an Elasticsearch index
- Removes unnecessary metadata produced by Debezium and Kafka

### Elasticsearch Output
After transformation, Logstash sends the events to Elasticsearch.
```
output {
  elasticsearch {
    hosts => ["http://elasticsearch:9200"]
    index => "%{AggregateType}"
    document_id => "%{AggregateId}"
    action => "%{Operation}"
  }

  stdout { codec => rubydebug }
}
```
Indexing behavior:

- Index Name: derived from AggregateType
- Document ID: AggregateId ensures idempotent updates
- Action: determined by Operation (create, update, delete)
This allows Elasticsearch to maintain a search‑optimized projection of the domain data.

## 6. Elasticsearch and Kibana
Elasticsearch stores the processed documents and makes them available for fast search and analytics.

Kibana provides visualization capabilities, allowing dashboards and queries to be built on top of the indexed data.


# Summary
- .NET application writes domain events to the Outbox table.
- SQL Server CDC captures Outbox changes.
- Debezium streams those changes to Kafka topics.
- Logstash consumes Kafka events and transforms them.
- Logstash indexes the events into Elasticsearch.
- Kibana visualizes the indexed data.

The .NET application never writes to Elasticsearch directly. The entire read‑model pipeline is handled by Debezium, Kafka, and Logstash.

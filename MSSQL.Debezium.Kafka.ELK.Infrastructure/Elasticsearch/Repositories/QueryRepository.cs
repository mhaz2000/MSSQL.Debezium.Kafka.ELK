using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Queries;

namespace MSSQL.Debezium.Kafka.ELK.Infrastructure.Elasticsearch.Repositories;
    public class QueryRepository<T> : IQueryRepository<T> where T : class
{
    private readonly ElasticsearchClient _elastic;
    private readonly string _indexName;

    public QueryRepository(ElasticsearchClient elastic, string indexName)
    {
        _elastic = elastic;
        _indexName = indexName;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _elastic.GetAsync<T>(
            id.ToString(),
            g => g.Index(_indexName),
            ct);

        return response.Found ? response.Source : null;
    }

    public async Task<(IReadOnlyList<T> data, long count)> GetAllAsync(
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var from = (page - 1) * pageSize;

        var response = await _elastic.SearchAsync<T>(s => s
            .Index(_indexName)
            .From(from)
            .Size(pageSize)
            .Query(q => q.MatchAll(new MatchAllQuery())),
            ct);


        return (response.Documents.ToList(), response.Total);
    }
}

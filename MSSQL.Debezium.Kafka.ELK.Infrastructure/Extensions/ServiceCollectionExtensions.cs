using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Queries;
using MSSQL.Debezium.Kafka.ELK.Infrastructure.Elasticsearch;
using MSSQL.Debezium.Kafka.ELK.Infrastructure.Elasticsearch.Repositories;
using MSSQL.Debezium.Kafka.ELK.Infrastructure.Persistence;
using MSSQL.Debezium.Kafka.ELK.Infrastructure.Persistence.Repositories;

namespace MSSQL.Debezium.Kafka.ELK.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<ProductCommandRepository>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<ProductQueryRepository>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Sql"),
                b => b.MigrationsAssembly("MSSQL.Debezium.Kafka.ELK.Infrastructure"));
        });

        services.AddSingleton(ElasticsearchInitializer.CreateClient(configuration["ELK:ElasticUrl"]!));

        return services;
    }
}
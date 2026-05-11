using MediatR;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs;

namespace MSSQL.Debezium.Kafka.ELK.Application.Queries.Products;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;

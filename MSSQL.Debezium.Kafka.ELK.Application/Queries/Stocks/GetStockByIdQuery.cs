using MediatR;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs;

namespace MSSQL.Debezium.Kafka.ELK.Application.Queries.Stocks;
public record GetStockByIdQuery(Guid Id) : IRequest<StockDto?>;

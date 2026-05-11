using MediatR;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs.Base;

namespace MSSQL.Debezium.Kafka.ELK.Application.Queries.Stocks;

public record GetStocksQuery(
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResult<StockDto>>;

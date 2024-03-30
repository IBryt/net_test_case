using Dapper;
using WebAPI.Data;
using WebAPI.Domain.Queries.Transactions;
using WebAPI.Interfaces;

namespace WebAPI.Domain.QueryHandlers.Transactions;

public class GetTransactionIdsHandler : IQueryHandler<GetTransactionIds, IEnumerable<string>>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetTransactionIdsHandler(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }


    public async Task<IEnumerable<string>> Execute(GetTransactionIds query)
    {
        using var conn = await _connectionFactory.GetDbConnection();

        var sql = "SELECT TransactionId FROM Transactions WHERE TransactionId = ANY(@Ids)";

        return await conn.QueryAsync<string>(sql, new { Ids = query.Ids.ToArray() });
    }
}

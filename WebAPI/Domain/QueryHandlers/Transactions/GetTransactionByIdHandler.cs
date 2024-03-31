using Dapper;
using WebAPI.Data;
using WebAPI.Domain.DTOs.Transactions;
using WebAPI.Domain.Queries.Transactions;
using WebAPI.Interfaces;

namespace WebAPI.Domain.QueryHandlers.Transactions;

public class GetTransactionByIdHandler : IQueryHandler<GetTransactionById, TransactionDTO>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetTransactionByIdHandler(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<TransactionDTO> Execute(GetTransactionById query)
    {
        using var conn = await _connectionFactory.GetDbConnection();

        var sql = "SELECT * FROM Transactions WHERE TransactionId = @Id";

        return await conn.QueryFirstOrDefaultAsync<TransactionDTO>(sql, new { Id = query.Id });

    }
}

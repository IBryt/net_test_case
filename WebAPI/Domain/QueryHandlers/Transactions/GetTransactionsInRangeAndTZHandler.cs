using Dapper;
using WebAPI.Data;
using WebAPI.Domain.DTOs.Transactions;
using WebAPI.Domain.Queries.Transactions;
using WebAPI.Interfaces;

namespace WebAPI.Domain.QueryHandlers.Transactions
{
    public class GetTransactionsInRangeAndTZHandler : IQueryHandler<GetTransactionsInRangeAndTZ, IEnumerable<TransactionDTO>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetTransactionsInRangeAndTZHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TransactionDTO>> Execute(GetTransactionsInRangeAndTZ query)
        {
            using var conn = await _connectionFactory.GetDbConnection();

            //TransactionId, Name, Email, Amount, TransactionDate, ClientLocation, TimeZone
            var sql = @"
                SELECT *
                FROM Transactions
                WHERE TransactionDate >= @FromDate 
                    AND TransactionDate < @ToDate 
                    AND TimeZone = @TimeZone";

            return await conn.QueryAsync<TransactionDTO>(sql, new
            {
                FromDate = query.From.ToDateTime(new TimeOnly()),
                ToDate = query.To.ToDateTime(new TimeOnly()),
                TimeZone = query.TimeZone
            });
        }
    }
}

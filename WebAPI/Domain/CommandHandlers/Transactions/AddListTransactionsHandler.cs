using Dapper;
using WebAPI.Data;
using WebAPI.Domain.Commands.Transactions;
using WebAPI.Interfaces;

namespace WebAPI.Domain.CommandHandlers.Transactions;

public class AddListTransactionsHandler : ICommandHandler<AddListTransactions>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AddListTransactionsHandler(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Execute(AddListTransactions commands)
    {
        using (var conn = await _connectionFactory.GetDbConnection())
        {
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    foreach (var command in commands.Transactions)
                    {
                        await conn.ExecuteAsync(@"
                            INSERT INTO Transactions (TransactionId, Name, Email, Amount, TransactionDate, ClientLocation, TimeZone) 
                            VALUES (@TransactionId, @Name, @Email, @Amount, @TransactionDate, @ClientLocation, @TimeZone);
                        ", command, transaction: transaction);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}

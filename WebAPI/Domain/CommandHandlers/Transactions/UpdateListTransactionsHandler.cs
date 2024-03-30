using Dapper;
using WebAPI.Data;
using WebAPI.Domain.Commands.Transactions;
using WebAPI.Interfaces;

namespace WebAPI.Domain.CommandHandlers.Transactions;

public class UpdateListTransactionsHandler : ICommandHandler<UpdateListTransactions>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UpdateListTransactionsHandler(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Execute(UpdateListTransactions command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        using (var conn = await _connectionFactory.GetDbConnection())
        {
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    foreach (var transactionData in command.Transactions)
                    {
                        await conn.ExecuteAsync(@"
                                UPDATE Transactions 
                                SET Name = @Name,
                                    Email = @Email, 
                                    Amount = @Amount, 
                                    TransactionDate = @TransactionDate, 
                                    ClientLocation = @ClientLocation, 
                                    TimeZone = @TimeZone 
                                WHERE TransactionId = @TransactionId;
                            ", transactionData, transaction: transaction);
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
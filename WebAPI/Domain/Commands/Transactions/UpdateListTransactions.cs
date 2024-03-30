using WebAPI.Domain.DTOs.Transactions;

namespace WebAPI.Domain.Commands.Transactions;

public class UpdateListTransactions
{
    public IEnumerable<TransactionDTO> Transactions { get; set; }
}

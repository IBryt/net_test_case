using WebAPI.Domain.DTOs.Transactions;

namespace WebAPI.Domain.Commands.Transactions;

public class AddListTransactions
{
    public IEnumerable<TransactionDTO> Transactions { get; set; }
}

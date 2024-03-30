using WebAPI.Domain.DTOs.Transactions;

namespace WebAPI.Interfaces.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDTO>> ReadTransactionsFromCsvFile(IFormFile file);
    Task AddAndUpdate(IEnumerable<TransactionDTO> transactions);
    Task<TransactionDTO> GetTransactionById(string id);
    MemoryStream ExportTransactionsToExcel(TransactionDTO transaction);
}

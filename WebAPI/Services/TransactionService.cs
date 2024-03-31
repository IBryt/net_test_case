using OfficeOpenXml;
using System.Globalization;
using System.Text.RegularExpressions;
using WebAPI.Domain.Commands;
using WebAPI.Domain.Commands.Transactions;
using WebAPI.Domain.DTOs.Transactions;
using WebAPI.Domain.Queries;
using WebAPI.Domain.Queries.Transactions;
using WebAPI.Extensions;
using WebAPI.Interfaces.Services;

namespace WebAPI.Services;

public class TransactionService : ITransactionService
{
    private readonly ICommandsProcessor _commandsProcessor;
    private readonly IQueriesProcessor _queriesProcessor;

    public TransactionService(ICommandsProcessor commandsProcessor, IQueriesProcessor queriesProcessor)
    {
        _commandsProcessor = commandsProcessor;
        _queriesProcessor = queriesProcessor;
    }

    public async Task AddAndUpdate(IEnumerable<TransactionDTO> transactions)
    {
        var ids = transactions.Select(t => t.TransactionId).ToList();
        var existingTransactionIds = await _queriesProcessor.Process<GetTransactionIds, IEnumerable<string>>(new GetTransactionIds { Ids = ids });

        var transactionsToAdd = transactions.Where(t => !existingTransactionIds.Contains(t.TransactionId)).ToList();
        var transactionsToUpdate = transactions.Where(t => existingTransactionIds.Contains(t.TransactionId)).ToList();

        if (transactionsToAdd.Any())
        {
            await _commandsProcessor.Process(new AddListTransactions { Transactions = transactionsToAdd });
        }

        if (transactionsToUpdate.Any())
        {
            await _commandsProcessor.Process(new UpdateListTransactions { Transactions = transactionsToUpdate });
        }
    }

    public MemoryStream ExportTransactionsToExcel(TransactionDTO transaction)
    {
        using var package = new ExcelPackage();

        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Transactions");

        worksheet.Column(1).Width = 30;
        worksheet.Column(2).Width = 30;
        worksheet.Column(3).Width = 30;
        worksheet.Column(4).Width = 10;
        worksheet.Column(5).Width = 20;
        worksheet.Column(6).Width = 40;

        worksheet.Cells[1, 1].Value = "TransactionId";
        worksheet.Cells[1, 2].Value = "Name";
        worksheet.Cells[1, 3].Value = "Email";
        worksheet.Cells[1, 4].Value = "Amount";
        worksheet.Cells[1, 5].Value = "TransactionDate";
        worksheet.Cells[1, 6].Value = "ClientLocation";

        worksheet.Cells[2, 1].Value = transaction.TransactionId;
        worksheet.Cells[2, 2].Value = transaction.Name;
        worksheet.Cells[2, 3].Value = transaction.Email;
        worksheet.Cells[2, 4].Value = "$" + transaction.Amount.ToString("0.00", CultureInfo.InvariantCulture);
        worksheet.Cells[2, 5].Value = transaction.TransactionDate.ToString("yyyy-MM-dd HH:mm:ss");
        worksheet.Cells[2, 6].Value = transaction.ClientLocation;

        MemoryStream stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public async Task<TransactionDTO> GetTransactionById(string id)
    {
        return await _queriesProcessor.Process<GetTransactionById, TransactionDTO>(new GetTransactionById { Id = id });
    }

    public async Task<IEnumerable<TransactionDTO>> ReadTransactionsFromCsvFile(IFormFile file)
    {
        List<TransactionDTO> transactions = new List<TransactionDTO>();

        using var reader = new StreamReader(file.OpenReadStream());

        string? line;

        string pattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

        await reader.ReadLineAsync();

        while ((line = await reader.ReadLineAsync()) != null)
        {

            string[] parts = Regex.Split(line, pattern);

            var transactionDate = DateTime.Parse(parts[4], CultureInfo.InvariantCulture);
            transactionDate = DateTime.SpecifyKind(transactionDate, DateTimeKind.Utc);
            var clientLocation = parts[5].Trim('"');

            var transaction = new TransactionDTO
            {
                TransactionId = parts[0],
                Name = parts[1],
                Email = parts[2],
                Amount = decimal.Parse(parts[3].Replace("$", ""), CultureInfo.InvariantCulture),
                TransactionDate = transactionDate,
                ClientLocation = clientLocation,
                TimeZone = clientLocation.GetTimeZone(transactionDate)
            };
            transactions.Add(transaction);
        }

        return transactions
            .GroupBy(t => t.TransactionId)
            .Select(g => g.First())
            .ToList();
    }

}

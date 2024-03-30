using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{

    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {

        _transactionService = transactionService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadCsvFile(IFormFile file)
    {

        if (file == null || file.Length == 0)
            return BadRequest("The file was not uploaded.");

        if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Invalid file format. Only files with the .csv extension are allowed.");

        if (file.Length > 1024 * 1024) // 1 MB in bytes
            return BadRequest("The file size exceeds the maximum allowed size (1 MB).");

        var transactions = await _transactionService.ReadTransactionsFromCsvFile(file);

        await _transactionService.AddAndUpdate(transactions);

        return Ok("The file was successfully uploaded.");
    }


    [HttpGet("export")]
    public async Task<IActionResult> DownloadExcel(string id)
    {
        var transaction = await _transactionService.GetTransactionById(id);
        if (transaction == null)
        {
            return BadRequest($"Failed to found id={id}");
        }

        var stream = _transactionService.ExportTransactionsToExcel(transaction);

        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{id}.xlsx");
    }
}

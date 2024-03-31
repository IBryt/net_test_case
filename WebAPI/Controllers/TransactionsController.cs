using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WebAPI.Interfaces.Services;

namespace WebAPI.Controllers;

/// <summary>
/// Controller for managing transactions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{

    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Uploads a CSV file with transactions.
    /// </summary>
    /// <param name="file">The CSV file to upload. The file must have the .csv extension.</param>
    /// <returns>A response indicating the success or failure of the upload.</returns>
    [HttpPost("upload")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        return Ok();
    }


    /// <summary>
    /// Downloads transaction by id as an Excel file.
    /// </summary>
    /// <param name="id">The ID of the transaction to download.</param>
    /// <returns>The Excel file containing the transaction data.</returns>
    [HttpGet("export/{id}")]
    [Produces("application/json", "multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadTransactionByIdExcel([DefaultValue("T-4-270.54545454545456_3.04")] string id)
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

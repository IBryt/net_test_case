namespace WebAPI.Domain.Queries.Transactions;

public class GetTransactionsInRangeAndTZ
{
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public string TimeZone { get; set; }
}

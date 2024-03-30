namespace WebAPI.Domain.Queries.Exceptions;

public class UnknownQueryException : Exception
{
    public UnknownQueryException(string message) : base(message) { }
}

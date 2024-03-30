namespace WebAPI.Domain.Commands.Exceptions;

public class UnknownCommandException : Exception
{
    public UnknownCommandException(string message) : base(message) { }
}

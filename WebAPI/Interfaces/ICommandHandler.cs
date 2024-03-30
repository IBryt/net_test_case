namespace WebAPI.Interfaces;

public interface ICommandHandler<TCommand>
{
    Task Execute(TCommand command);
}


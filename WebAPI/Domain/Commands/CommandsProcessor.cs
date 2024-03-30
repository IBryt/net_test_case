using WebAPI.Domain.Commands.Exceptions;
using WebAPI.Interfaces;

namespace WebAPI.Domain.Commands
{
    public class CommandsProcessor : ICommandsProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandsProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Process<TCommand>(TCommand command)
        {
            var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();

            if (handler == null)
                throw new UnknownCommandException($"Handler for command \"{command.GetType().Name}\" not found.");

            return handler.Execute(command);
        }
    }

    public interface ICommandsProcessor
    {
        Task Process<TCommand>(TCommand command);
    }
}

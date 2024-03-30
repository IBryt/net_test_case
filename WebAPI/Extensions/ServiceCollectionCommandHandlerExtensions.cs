using WebAPI.Interfaces;

namespace WebAPI.Extensions;

public static class ServiceCollectionCommandHandlerExtensions
{
    public static void AddCommandHandler<TCommandHandler, TCommand>(this IServiceCollection @this)
        where TCommandHandler : class, ICommandHandler<TCommand>
        where TCommand : class
    {
        @this.AddScoped(typeof(ICommandHandler<TCommand>), typeof(TCommandHandler));
    }
}

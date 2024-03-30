namespace WebAPI.Interfaces;

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult> Execute(TQuery query);
}

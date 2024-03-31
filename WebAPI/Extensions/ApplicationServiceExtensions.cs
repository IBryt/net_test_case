using OfficeOpenXml;
using WebAPI.Domain.CommandHandlers.Transactions;
using WebAPI.Domain.Commands;
using WebAPI.Domain.Commands.Transactions;
using WebAPI.Domain.DTOs.Transactions;
using WebAPI.Domain.Queries;
using WebAPI.Domain.Queries.Transactions;
using WebAPI.Domain.QueryHandlers.Transactions;
using WebAPI.Interfaces.Services;
using WebAPI.Services;

namespace WebAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        services.AddScoped<ITransactionService, TransactionService>();

        services.AddCommandHandler<AddListTransactionsHandler, AddListTransactions>();
        services.AddCommandHandler<UpdateListTransactionsHandler, UpdateListTransactions>();

        services.AddQueryHandler<GetTransactionIdsHandler, GetTransactionIds, IEnumerable<string>>();
        services.AddQueryHandler<GetTransactionByIdHandler, GetTransactionById, TransactionDTO>();
        services.AddQueryHandler<GetTransactionsInRangeAndTZHandler, GetTransactionsInRangeAndTZ, IEnumerable<TransactionDTO>>();

        services.AddScoped<ICommandsProcessor, CommandsProcessor>();
        services.AddScoped<IQueriesProcessor, QueriesProcessor>();

        return services;
    }
}
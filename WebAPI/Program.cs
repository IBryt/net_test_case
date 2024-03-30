using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebAPI.Data;
using WebAPI.Domain.CommandHandlers.Transactions;
using WebAPI.Domain.Commands;
using WebAPI.Domain.Commands.Transactions;
using WebAPI.Domain.DTOs.Transactions;
using WebAPI.Domain.Queries;
using WebAPI.Domain.Queries.Transactions;
using WebAPI.Domain.QueryHandlers.Transactions;
using WebAPI.Extensions;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
           options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IDbConnectionFactory>(
    new PostgresDBConnectionFactory(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCommandHandler<AddListTransactionsHandler, AddListTransactions>();
builder.Services.AddCommandHandler<UpdateListTransactionsHandler, UpdateListTransactions>();

builder.Services.AddQueryHandler<GetTransactionIdsHandler, GetTransactionIds, IEnumerable<string>>();
builder.Services.AddQueryHandler<GetTransactionByIdHandler, GetTransactionById, TransactionDTO>();


builder.Services.AddScoped<ICommandsProcessor, CommandsProcessor>();
builder.Services.AddScoped<IQueriesProcessor, QueriesProcessor>();

var app = builder.Build();

app.Migration();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

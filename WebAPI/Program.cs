using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

builder.Services.DBRegistration(builder.Configuration);

var app = builder.Build();

app.Migration();

if(builder.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
    
app.MapControllers();

app.Run();

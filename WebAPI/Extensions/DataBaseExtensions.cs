﻿using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

namespace WebAPI.Extensions;

public static class DataBaseExtensions
{
    public static void Migration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DataContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured during migration");
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    public static void DBRegistration(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<DataContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton<IDbConnectionFactory>(
            new PostgresDBConnectionFactory(configuration.GetConnectionString("DefaultConnection")));
    }
}

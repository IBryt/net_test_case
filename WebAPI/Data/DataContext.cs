using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Tables;

namespace WebAPI.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            entity.SetTableName(tableName?.ToLowerInvariant());

            foreach (var property in entity.GetProperties())
            {
                var propertyName = property.Name;
                property.SetColumnName(propertyName?.ToLowerInvariant());
            }
        }
    }
}



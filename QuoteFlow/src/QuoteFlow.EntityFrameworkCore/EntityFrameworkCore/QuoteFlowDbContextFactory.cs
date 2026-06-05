using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace QuoteFlow.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class QuoteFlowDbContextFactory : IDesignTimeDbContextFactory<QuoteFlowDbContext>
{
    public QuoteFlowDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        QuoteFlowEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<QuoteFlowDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new QuoteFlowDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../QuoteFlow.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}

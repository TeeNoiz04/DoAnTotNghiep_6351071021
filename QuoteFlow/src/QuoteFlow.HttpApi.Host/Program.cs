using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace QuoteFlow;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Async(c => c.File(
                path: $"Logs/logs.txt",
                rollingInterval: RollingInterval.Day,   // Creates a new file every day.
                retainedFileCountLimit: 30,             // Keeps only 30 log files.
                shared: true,                           // Allows shared access for multiple processes.
                flushToDiskInterval: TimeSpan.FromSeconds(5)
            ))
            .WriteTo.Async(c => c.Console())
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting QuoteFlow.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(args);
            // Set default to 30MB for all endpoints
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 30 * 1024 * 1024; // 30MB default
            });

            // Configure Kestrel (ONLY ONCE)
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = null; // Allow per-endpoint override
                serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
                serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
            });

            builder.Host
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration
#if DEBUG
                        .MinimumLevel.Debug()
#else
                        .MinimumLevel.Information()
#endif
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                        .MinimumLevel.Override("Volo.Abp.Swashbuckle", LogEventLevel.Debug)
                        .MinimumLevel.Override("Volo.Abp.Quartz", LogEventLevel.Information)
                        .MinimumLevel.Override("Quartz", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Async(c => c.File(
                            path: $"Logs/logs.txt",
                            rollingInterval: RollingInterval.Day,   // Creates a new file every day.
                            retainedFileCountLimit: 30,             // Keeps only 30 log files.
                            shared: true,                           // Allows shared access for multiple processes.
                            flushToDiskInterval: TimeSpan.FromSeconds(5)
                        ))
                        .WriteTo.Async(c => c.Console())
                        .WriteTo.Async(c => c.AbpStudio(services));
                });
            await builder.AddApplicationAsync<QuoteFlowHttpApiHostModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

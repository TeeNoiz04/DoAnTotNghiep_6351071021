using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace QuoteFlow.Data;

/* This is used if database provider does't define
 * IQuoteFlowDbSchemaMigrator implementation.
 */
public class NullQuoteFlowDbSchemaMigrator : IQuoteFlowDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}

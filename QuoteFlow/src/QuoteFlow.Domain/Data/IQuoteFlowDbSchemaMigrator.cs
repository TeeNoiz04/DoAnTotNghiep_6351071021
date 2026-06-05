using System.Threading.Tasks;

namespace QuoteFlow.Data;

public interface IQuoteFlowDbSchemaMigrator
{
    Task MigrateAsync();
}

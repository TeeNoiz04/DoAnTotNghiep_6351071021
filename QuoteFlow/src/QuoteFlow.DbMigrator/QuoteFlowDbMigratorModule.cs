using QuoteFlow.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace QuoteFlow.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(QuoteFlowEntityFrameworkCoreModule),
    typeof(QuoteFlowApplicationContractsModule)
)]
public class QuoteFlowDbMigratorModule : AbpModule
{
}

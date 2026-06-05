using QuoteFlow.Localization;
using Volo.Abp.Application.Services;

namespace QuoteFlow;

/* Inherit your application services from this class.
 */
public abstract class QuoteFlowAppService : ApplicationService
{
    protected QuoteFlowAppService()
    {
        LocalizationResource = typeof(QuoteFlowResource);
    }
}

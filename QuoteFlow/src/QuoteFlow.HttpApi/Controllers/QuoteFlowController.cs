using QuoteFlow.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class QuoteFlowController : AbpControllerBase
{
    protected QuoteFlowController()
    {
        LocalizationResource = typeof(QuoteFlowResource);
    }
}

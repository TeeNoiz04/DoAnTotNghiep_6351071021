using QuoteFlow.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace QuoteFlow;

[Dependency(ReplaceServices = true)]
public class QuoteFlowBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<QuoteFlowResource> _localizer;

    public QuoteFlowBrandingProvider(IStringLocalizer<QuoteFlowResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
    public override string? LogoUrl => _localizer["/Themes/LeptonX/Layouts/Image/logo-mevn-2.svg"];
    public override string? LogoReverseUrl => _localizer["/Themes/LeptonX/Layouts/Image/logo-mevn-2.svg"];
}

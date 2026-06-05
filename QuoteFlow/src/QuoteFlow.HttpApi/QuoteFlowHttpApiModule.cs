using Localization.Resources.AbpUi;
using QuoteFlow.Localization;
using Volo.Abp.Account;
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Gdpr;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.FileManagement;

namespace QuoteFlow;

[DependsOn(
   typeof(QuoteFlowApplicationContractsModule),
   typeof(AbpPermissionManagementHttpApiModule),
   typeof(AbpSettingManagementHttpApiModule),
   typeof(AbpIdentityHttpApiModule),
   typeof(AbpAccountAdminHttpApiModule),
   typeof(TextTemplateManagementHttpApiModule),
   typeof(AbpAuditLoggingHttpApiModule),
   typeof(AbpOpenIddictProHttpApiModule),
   typeof(LanguageManagementHttpApiModule),
   typeof(FileManagementHttpApiModule),
   typeof(AbpGdprHttpApiModule),
   typeof(AbpAccountPublicHttpApiModule),
   typeof(AbpFeatureManagementHttpApiModule)
   )]
public class QuoteFlowHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<QuoteFlowResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}

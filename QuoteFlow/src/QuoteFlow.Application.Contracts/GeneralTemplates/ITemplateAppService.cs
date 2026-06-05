using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.GeneralTemplates;
public interface ITemplateAppService : IApplicationService
{
    Task<IRemoteStreamContent> GetTemplateMaterialNewRegistrationAsync();
    Task<IRemoteStreamContent> GetTemplateMaterialUpdatePriceAsync();
    Task<IRemoteStreamContent> GetTemplateMaterialUpdateWithoutPriceAsync();
    Task<IRemoteStreamContent> GetTemplateMaterialUpdateInventoryPlanAsync();
    Task<IRemoteStreamContent> GetTemplateStockTracingDeliveryAsync();
    Task<IRemoteStreamContent> GetTemplateStockTracingReceiptAsync();
    Task<IRemoteStreamContent> GetTemplateStockTracingInventoryAsync();
    Task<IRemoteStreamContent> GetTemplateMaterialAsync(string typeTemplate);
    Task<IRemoteStreamContent> GetPriceOfferTemplateAsync(string templateType);
    Task<IRemoteStreamContent> GetPriceOfferAddMoreItemsTemplateAsync();
    Task<IRemoteStreamContent> GetPriceOfferChangeItemPropertiesTemplateAsync();
    Task<IRemoteStreamContent> GetStockImportTemplateAsync();
    Task<IRemoteStreamContent> GetStockImportPriorityTemplateAsync();
    Task<IRemoteStreamContent> GetTemplateStockManagementAsync(string typeTemplate);
    Task<IRemoteStreamContent> GetTemplatePSIAsync(string typeTemplate);
    Task<IRemoteStreamContent> GetCargoTemplateAsync();
    Task<IRemoteStreamContent> GetSOTemplateAsync();
    Task<IRemoteStreamContent> GetPOTemplateAsync();
    Task<IRemoteStreamContent> GetDPOTemplateAsync();
    Task<IRemoteStreamContent> GetGKRTemplateAsync();
    Task<IRemoteStreamContent> GetSpecialInputPriceTemplateAsync();
    Task<IRemoteStreamContent> GetGICTemplateAsync(string gicType);
    Task<IRemoteStreamContent> GetBatchRequestImportTemplateAsync();
}

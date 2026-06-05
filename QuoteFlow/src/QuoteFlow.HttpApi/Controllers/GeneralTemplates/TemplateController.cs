using Asp.Versioning;
using QuoteFlow.GeneralTemplates;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.GeneralTemplates;

[RemoteService]
[Area("app")]
[ControllerName("Template")]
[Route("api/app/templates")]
public class TemplateController : AbpController, ITemplateAppService
{
    protected ITemplateAppService _templateAppService;

    public TemplateController(ITemplateAppService templateAppService)
    {
        _templateAppService = templateAppService;
    }

    [HttpGet]
    [Route("material-new-registration")]
    public Task<IRemoteStreamContent> GetTemplateMaterialNewRegistrationAsync()
    {
        return _templateAppService.GetTemplateMaterialNewRegistrationAsync();
    }

    [HttpGet]
    [Route("material-update-inventory-plan")]
    public Task<IRemoteStreamContent> GetTemplateMaterialUpdateInventoryPlanAsync()
    {
        return _templateAppService.GetTemplateMaterialUpdateInventoryPlanAsync();
    }

    [HttpGet]
    [Route("material-update-price")]
    public Task<IRemoteStreamContent> GetTemplateMaterialUpdatePriceAsync()
    {
        return _templateAppService.GetTemplateMaterialUpdatePriceAsync();
    }
    [HttpGet]
    [Route("material-update-without-price")]

    public Task<IRemoteStreamContent> GetTemplateMaterialUpdateWithoutPriceAsync()
    {
        return _templateAppService.GetTemplateMaterialUpdateWithoutPriceAsync();
    }
    [HttpGet]
    [Route("stock-tracing-delivery")]
    public Task<IRemoteStreamContent> GetTemplateStockTracingDeliveryAsync()
    {
        return _templateAppService.GetTemplateStockTracingDeliveryAsync();
    }
    [HttpGet]
    [Route("stock-tracing-inventory")]
    public Task<IRemoteStreamContent> GetTemplateStockTracingInventoryAsync()
    {
        return _templateAppService.GetTemplateStockTracingInventoryAsync();
    }
    [HttpGet]
    [Route("stock-tracing-receipt")]
    public Task<IRemoteStreamContent> GetTemplateStockTracingReceiptAsync()
    {
        return _templateAppService.GetTemplateStockTracingReceiptAsync();
    }
    [HttpGet]
    [Route("material")]
    public Task<IRemoteStreamContent> GetTemplateMaterialAsync(string typeTemplate)
    {
        return _templateAppService.GetTemplateMaterialAsync(typeTemplate);
    }
    [HttpGet]
    [Route("price-offers/import-template")]
    public Task<IRemoteStreamContent> GetPriceOfferTemplateAsync(string templateType)
    {
        return _templateAppService.GetPriceOfferTemplateAsync(templateType);
    }
    [HttpGet]
    [Route("price-offers/add-more-items-template")]
    public Task<IRemoteStreamContent> GetPriceOfferAddMoreItemsTemplateAsync()
    {
        return _templateAppService.GetPriceOfferAddMoreItemsTemplateAsync();
    }
    [HttpGet]
    [Route("price-offers/change-item-properties-template")]
    public Task<IRemoteStreamContent> GetPriceOfferChangeItemPropertiesTemplateAsync()
    {
        return _templateAppService.GetPriceOfferChangeItemPropertiesTemplateAsync();
    }
    [HttpGet]
    [Route("stock-import-template")]
    public Task<IRemoteStreamContent> GetStockImportTemplateAsync()
    {
        return _templateAppService.GetStockImportTemplateAsync();
    }
    [HttpGet]
    [Route("stock-import-priority-template")]
    public Task<IRemoteStreamContent> GetStockImportPriorityTemplateAsync()
    {
        return _templateAppService.GetStockImportPriorityTemplateAsync();
    }
    [HttpGet]
    [Route("stock-management-template")]
    public Task<IRemoteStreamContent> GetTemplateStockManagementAsync(string typeTemplate)
    {
        return _templateAppService.GetTemplateStockManagementAsync(typeTemplate);
    }
    [HttpGet]
    [Route("psi-template")]
    public Task<IRemoteStreamContent> GetTemplatePSIAsync(string typeTemplate)
    {
        return _templateAppService.GetTemplatePSIAsync(typeTemplate);
    }
    [HttpGet]
    [Route("cargo-template")]
    public Task<IRemoteStreamContent> GetCargoTemplateAsync()
    {
        return _templateAppService.GetCargoTemplateAsync();
    }
    [HttpGet]
    [Route("so-template")]
    public Task<IRemoteStreamContent> GetSOTemplateAsync()
    {
        return _templateAppService.GetSOTemplateAsync();
    }
    [HttpGet]
    [Route("po-template")]
    public Task<IRemoteStreamContent> GetPOTemplateAsync()
    {
        return _templateAppService.GetPOTemplateAsync();
    }
    [HttpGet]
    [Route("dpo-template")]
    public Task<IRemoteStreamContent> GetDPOTemplateAsync()
    {
        return _templateAppService.GetDPOTemplateAsync();
    }
    [HttpGet]
    [Route("gkr-template")]
    public Task<IRemoteStreamContent> GetGKRTemplateAsync()
    {
        return _templateAppService.GetGKRTemplateAsync();
    }
    [HttpGet]
    [Route("special-input-price")]
    public Task<IRemoteStreamContent> GetSpecialInputPriceTemplateAsync()
    {
        return _templateAppService.GetSpecialInputPriceTemplateAsync();
    }

    [HttpGet]
    [Route("gic-template")]
    public Task<IRemoteStreamContent> GetGICTemplateAsync(string gicType)
    {
        return _templateAppService.GetGICTemplateAsync(gicType);
    }
    [HttpGet]
    [Route("batch-request")]
    public Task<IRemoteStreamContent> GetBatchRequestImportTemplateAsync()
    {
        return _templateAppService.GetBatchRequestImportTemplateAsync();
    }
}


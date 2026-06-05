using QuoteFlow.GICs;
using QuoteFlow.Materials;
using QuoteFlow.PriceOffers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.FileManagement.Files;

namespace QuoteFlow.GeneralTemplates;
[RemoteService(IsEnabled = false)]
public class TemplateAppService : QuoteFlowAppService, ITemplateAppService
{
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    private readonly FileDescriptorAppService _fileDescriptorAppService;

    public TemplateAppService(FileDescriptorAppService fileDescriptorAppService, IRepository<FileDescriptor, Guid> fileDescriptorRepository)
    {
        _fileDescriptorAppService = fileDescriptorAppService;
        _fileDescriptorRepository = fileDescriptorRepository;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplateMaterialNewRegistrationAsync()
    {
        var fileName = "Template_MaterialNewRegistration.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");


        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplateMaterialUpdateInventoryPlanAsync()
    {
        var fileName = "Template_MaterialUpdateInventoryPlan.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");


        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplateMaterialUpdatePriceAsync()
    {
        var fileName = "Template_MaterialUpdatePrice.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplateMaterialUpdateWithoutPriceAsync()
    {
        var fileName = "Template_MaterialUpdateWithoutPrice.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");


        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }
    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplateStockTracingDeliveryAsync()
    {
        var fileName = "Template_StockTracing_Delivery.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");


        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }
    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplateStockTracingInventoryAsync()
    {
        var fileName = "Template_StockTracing_Inventory.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");


        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }
    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplateStockTracingReceiptAsync()
    {
        var fileName = "Template_StockTracing_Receipt.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");


        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplateMaterialAsync(string typeTemplate)
    {
        var fileName = string.Empty;
        fileName = typeTemplate.ToUpper() switch
        {
            MaterialImportType.M1U => "Template_M1U.xlsx",
            MaterialImportType.M2U => "Template_M2U.xlsx",
            MaterialImportType.M3U => "Template_M3U.xlsx",
            MaterialImportType.M4U => "Template_M4U.xlsx",
            MaterialImportType.M5U => "Template_M5U.xlsx",
            MaterialImportType.M6U => "Template_M6U.xlsx",
            MaterialImportType.M7U => "Template_M7U.xlsx",
            MaterialImportType.M8U => "Template_M8U.xlsx",
            MaterialImportType.STOCK_INVENTORY => "Template_StockInventory.xlsx",
            MaterialImportType.STOCK_TRANSFER => "Template_StockTransfer.xlsx",
            _ => throw new UserFriendlyException("Invalid template type specified."),
        };
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetPriceOfferTemplateAsync(string templateType)
    {
        var fileName = string.Empty;
        switch (templateType.ToUpper())
        {
            case PriceOfferTypes.PriceOfferPP:
                fileName = "Template_SPO.PP.xlsx";
                break;

            case PriceOfferTypes.PriceOfferDS:
                fileName = "Template_SPO.DS.xlsx";
                break;

            case PriceOfferTypes.PriceOfferAP:
                fileName = "Template_SPO.AP.xlsx";
                break;
            case PriceOfferTypes.PriceOfferNB:
                fileName = "Template_SPO.NB.xlsx";
                break;
        }

        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetPriceOfferAddMoreItemsTemplateAsync()
    {
        var fileName = "Template_SPO.AddMoreItems.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetPriceOfferChangeItemPropertiesTemplateAsync()
    {
        var fileName = "Template_SPO.ChangeItemProperties.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetStockImportTemplateAsync()
    {
        //template invoice
        var fileName = "Template_StockImport.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetStockImportPriorityTemplateAsync()
    {
        var fileName = "Template_StockImportPriority.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplateStockManagementAsync(string typeTemplate)
    {
        var fileName = string.Empty;
        fileName = typeTemplate.ToUpper() switch
        {
            MaterialImportType.STOCK_INVENTORY => "Template_StockInventory.xlsx",
            MaterialImportType.STOCK_TRANSFER => "Template_StockTransfer.xlsx",
            _ => throw new UserFriendlyException("Invalid template type specified."),
        };
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetTemplatePSIAsync(string typeTemplate)
    {
        var fileName = string.Empty;
        fileName = typeTemplate.ToUpper() switch
        {
            "PSI_FA" => "Template_PSI_Import_FA.xlsx",
            "PSI_LVS" => "Template_PSI_Import_LVS.xlsx",
            _ => throw new UserFriendlyException("Invalid template type specified."),
        };
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);

        return fileWithContent;
    }
    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetCargoTemplateAsync()
    {
        var fileName = "Template_Cargo.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetSOTemplateAsync()
    {
        var fileName = "Template_SaleOrder.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }
    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetPOTemplateAsync()
    {
        var fileName = "Template_PurchaseOrder.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }
    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetDPOTemplateAsync()
    {
        var fileName = "Template_DistributorPurchaseOrder.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetGKRTemplateAsync()
    {
        var fileName = "Template_GoodsKeepingRequest.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetGICTemplateAsync(string gicType)
    {
        var fileName = string.Empty;

        fileName = gicType switch
        {
            GICTypeCodes.Internal => "Template_GIC_Import_IU.xlsx",
            GICTypeCodes.GivingSponsor => "Template_GIC_Import_FOC.xlsx",
            GICTypeCodes.Warranty => "Template_GIC_Import_WR.xlsx",
            GICTypeCodes.WriteOff => "Template_GIC_Import_WO.xlsx",
            _ => throw new UserFriendlyException($"Invalid GIC type specified, valid types are: {string.Join(", ", GICTypeCodes.AllTypes)}"),
        };
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetSpecialInputPriceTemplateAsync()
    {
        var fileName = "Template_SpecialInputPrice.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetBatchRequestImportTemplateAsync()
    {
        var fileName = "Template_BatchRequest.xlsx";
        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("The excel template does not exist in the system.");

        var token = await _fileDescriptorAppService.GetDownloadTokenAsync(fileDescriptor.Id);
        var fileWithContent = await _fileDescriptorAppService.DownloadAsync(fileDescriptor.Id, token.Token);
        return fileWithContent;
    }
}

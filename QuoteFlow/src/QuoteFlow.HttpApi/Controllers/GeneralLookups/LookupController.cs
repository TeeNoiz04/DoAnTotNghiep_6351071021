using Asp.Versioning;
using QuoteFlow.AddMoreItemHistories;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.Customers;
using QuoteFlow.GeneralLookups;
using QuoteFlow.Shared;
using QuoteFlow.SpecialInputPrices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.GeneralLookups;

[RemoteService]
[Area("app")]
[ControllerName("Lookup")]
[Route("api/app/lookups")]
public class LookupController : AbpController, ILookupsAppService
{
    protected ILookupsAppService _lookupsAppService;

    public LookupController(ILookupsAppService lookupsAppService)
    {
        _lookupsAppService = lookupsAppService;
    }

    [HttpGet]
    [Route("eu-industries")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetEUIndustryLookupAsync()
    {
        return _lookupsAppService.GetEUIndustryLookupAsync();
    }

    [HttpGet]
    [Route("customer-types")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetCustomerTypeLookupAsync()
    {
        return _lookupsAppService.GetCustomerTypeLookupAsync();
    }

    [HttpGet]
    [Route("key-account-classes")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountClassLookupAsync()
    {
        return _lookupsAppService.GetKeyAccountClassLookupAsync();
    }

    [HttpGet]
    [Route("key-account-types")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountTypeLookupAsync()
    {
        return _lookupsAppService.GetKeyAccountTypeLookupAsync();
    }

    [HttpGet]
    [Route("key-account-evaluations/financial")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountEvaluationFinancialLookupAsync()
    {
        return _lookupsAppService.GetKeyAccountEvaluationFinancialLookupAsync();
    }

    [HttpGet]
    [Route("key-account-evaluations/product")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountEvaluationProductLookupAsync()
    {
        return _lookupsAppService.GetKeyAccountEvaluationProductLookupAsync();
    }

    [HttpGet]
    [Route("material-groups")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetMaterialGroupLookupAsync()
    {
        return _lookupsAppService.GetMaterialGroupLookupAsync();
    }

    [HttpGet]
    [Route("material-types")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetMaterialTypeLookupAsync()
    {
        return _lookupsAppService.GetMaterialTypeLookupAsync();
    }

    [HttpGet]
    [Route("ware-house")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetWareHouseLookupAsync()
    {
        return _lookupsAppService.GetWareHouseLookupAsync();
    }

    [HttpGet]
    [Route("spo-types")]
    public virtual Task<List<string>> GetSpoTypeLookupAsync()
    {
        return _lookupsAppService.GetSpoTypeLookupAsync();
    }

    [HttpGet]
    [Route("vendors")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetVendorLookupAsync()
    {
        return _lookupsAppService.GetVendorLookupAsync();
    }

    [HttpGet]
    [Route("locations")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetLocationLookupAsync()
    {
        return _lookupsAppService.GetLocationLookupAsync();
    }

    [HttpGet]
    [Route("project-types")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetProjectTypeLookupAsync()
    {
        return _lookupsAppService.GetProjectTypeLookupAsync();
    }

    [HttpGet]
    [Route("buyers")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetBuyerLookupAsync([FromQuery] bool byPassBuyerCheck)
    {
        return _lookupsAppService.GetBuyerLookupAsync(byPassBuyerCheck);
    }

    [HttpGet]
    [Route("users")]
    public virtual Task<List<UserLookupDto>> GetUserLookupAsync()
    {
        return _lookupsAppService.GetUserLookupAsync();
    }

    [HttpGet]
    [Route("buyer-types")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetBuyerTypeLookupAsync(GetBuyerTypeLookupInput input)
    {
        return _lookupsAppService.GetBuyerTypeLookupAsync(input);
    }

    [HttpGet]
    [Route("nationality")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetNationalityLookupAsync()
    {
        return _lookupsAppService.GetNationalityLookupAsync();
    }

    [HttpGet]
    [Route("stock-foc")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetFOCStockCategoryLookupAsync()
    {
        return _lookupsAppService.GetFOCStockCategoryLookupAsync();
    }

    [HttpGet]
    [Route("stock-category")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetStockCategoryLookupAsync()
    {
        return _lookupsAppService.GetStockCategoryLookupAsync();
    }

    [HttpGet]
    [Route("stock-category/damaged")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetDamagedStockCategoryLookupAsync()
    {
        return _lookupsAppService.GetDamagedStockCategoryLookupAsync();
    }

    [HttpGet]
    [Route("main-stock-category")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetMainStockCategoryLookupAsync()
    {
        return _lookupsAppService.GetMainStockCategoryLookupAsync();
    }

    [HttpGet]
    [Route("stock-category-with-available-amount")]
    public virtual Task<ListResultDto<StockCategoryLookupDto<Guid>>> GetStockCategoryLookupWithAvailableAmountAsync([FromQuery] string materialCode, [FromQuery] bool? damagedStock)
    {
        return _lookupsAppService.GetStockCategoryLookupWithAvailableAmountAsync(materialCode, damagedStock);
    }


    [HttpGet]
    [Route("currency-category")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetCurrencyCategoryLookupAsync()
    {
        return _lookupsAppService.GetCurrencyCategoryLookupAsync();
    }
    [HttpGet]
    [Route("key-account")]
    public virtual Task<ListResultDto<KeyAccountLookupDto<Guid>>> GetKeyAccountLookupAsync(Guid? buyerId, string? materialType)
    {
        return _lookupsAppService.GetKeyAccountLookupAsync(buyerId, materialType);
    }
    [HttpGet]
    [Route("product")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetProductCategoryLookupAsync()
    {
        return _lookupsAppService.GetProductCategoryLookupAsync();
    }
    [HttpGet]
    [Route("financial")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetFinancialCategoryLookupAsync()
    {
        return _lookupsAppService.GetFinancialCategoryLookupAsync();
    }
    [HttpGet]
    [Route("{keyAccountTypeCode}/key-account-classify-child")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountClassChildLookupAsync(string keyAccountTypeCode, bool hiddenNA)
    {
        return _lookupsAppService.GetKeyAccountClassChildLookupAsync(keyAccountTypeCode, hiddenNA);
    }
    [HttpGet]
    [Route("special-input-prices")]
    public virtual Task<ListResultDto<SpecialInputPriceLookupDto<Guid>>> GetSpecialInputPriceLookupAsync(string? materialType)
    {
        return _lookupsAppService.GetSpecialInputPriceLookupAsync(materialType);
    }
    [HttpGet]
    [Route("supplier")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetSupplierLookupAsync()
    {
        return _lookupsAppService.GetSupplierLookupAsync();
    }
    [HttpGet]
    [Route("supplierBU")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetSupplierBULookupAsync()
    {
        return _lookupsAppService.GetSupplierBULookupAsync();
    }
    [HttpGet]
    [Route("buyers/{buyerTypeId}")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetBuyerLookupByBuyerTypeAsync(Guid buyerTypeId)
    {
        return _lookupsAppService.GetBuyerLookupByBuyerTypeAsync(buyerTypeId);
    }
    [HttpGet]
    [Route("supplierBU/{supplierId}")]
    public virtual Task<ListResultDto<SupplierBULookupDto<Guid>>> GetSupplierBUBySupplierLookupAsync(Guid supplierId)
    {
        return _lookupsAppService.GetSupplierBUBySupplierLookupAsync(supplierId);
    }

    [HttpGet]
    [Route("supplier/by-material-type/{materialType}")]
    public virtual Task<ListResultDto<LookupDto<Guid>>> GetSupplierByMaterialTypeLookupAsync(string materialType)
    {
        return _lookupsAppService.GetSupplierByMaterialTypeLookupAsync(materialType);
    }

    [HttpGet]
    [Route("supplierBU/{supplierId}/material-type/{materialType}")]
    public virtual Task<ListResultDto<SupplierBULookupDto<Guid>>> GetSupplierBUBySupplierAndMaterialTypeLookupAsync(Guid supplierId, string materialType)
    {
        return _lookupsAppService.GetSupplierBUBySupplierAndMaterialTypeLookupAsync(supplierId, materialType);
    }

    [HttpGet]
    [Route("account-no")]
    public virtual Task<ListResultDto<AccountCodeLookupDto>> GetAccountNoAsync([FromQuery][Required] string materialCode)
    {
        return _lookupsAppService.GetAccountNoAsync(materialCode);
    }
    [HttpGet]
    [Route("user-by-username")]
    public Task<List<UserLookupDto>> GetListUserLookup(string name)
    {
        return _lookupsAppService.GetListUserLookup(name);
    }
    [HttpGet]
    [Route("all-user")]
    public Task<List<UserLookupDto>> GetListAllUserLookup()
    {
        return _lookupsAppService.GetListAllUserLookup();
    }
    [HttpGet]
    [Route("sale-pic-by-materialtype")]
    public Task<List<UserLookupDto>> GetListUserPICLookup(GetSalePICLookupInput input)
    {
        return _lookupsAppService.GetListUserPICLookup(input);
    }
    [HttpGet]
    [Route("get-list-pic")]
    public Task<List<UserLookupDto>> GetAllSalePICLookupAsync()
    {
        return _lookupsAppService.GetAllSalePICLookupAsync();
    }
    [HttpGet]
    [Route("fiscal-year")]
    public Task<ListResultDto<int>> GetYearLookupAsync()
    {
        return _lookupsAppService.GetYearLookupAsync();
    }
    [HttpGet]
    [Route("fiscal-year-distributor-target")]
    public Task<ListResultDto<int?>> GetFiscalYearOfDistributorTargetLookupAsync()
    {
        return _lookupsAppService.GetFiscalYearOfDistributorTargetLookupAsync();
    }

    [HttpGet]
    [Route("gic-types")]
    public virtual Task<ListResultDto<GicTypeLookupDto<Guid>>> GetGicTypesLookupAsync()
    {
        return _lookupsAppService.GetGicTypesLookupAsync();
    }
    [HttpGet]
    [Route("material-group-psi")]
    public Task<ListResultDto<LookupDto<Guid>>> GetMaterialGroupPSILookupAsync(string materialType)
    {
        return _lookupsAppService.GetMaterialGroupPSILookupAsync(materialType);
    }
    [HttpGet]
    [Route("buyer-not-in-material-group")]
    public Task<ListResultDto<LookupDto<Guid>>> GetBuyersNotAssignedToMaterialGroupAsync()
    {
        return _lookupsAppService.GetBuyersNotAssignedToMaterialGroupAsync();
    }
    [HttpGet]
    [Route("material-group-by-type")]
    public Task<ListResultDto<LookupDto<Guid>>> GetMaterialGroupByTypeAsync(string type)
    {
        return _lookupsAppService.GetMaterialGroupByTypeAsync(type);
    }
    [HttpGet]
    [Route("fiscal-year-key-account")]
    public Task<ListResultDto<int?>> GetYearLookupKeyAccountAsync()
    {
        return _lookupsAppService.GetYearLookupKeyAccountAsync();
    }
    [HttpGet]
    [Route("supplier-po")]
    public Task<List<SupplierPOLookupDto>> GetSupplierPOLookupAsync(string? materialType, string? currency, string? createSource, bool? epa)
    {
        return _lookupsAppService.GetSupplierPOLookupAsync(materialType, currency, createSource, epa);
    }

    [HttpGet]
    [Route("workflow-levels")]
    public Task<ListResultDto<short?>> GetLevelLookupWorkflowAsync(string type)
    {
        return _lookupsAppService.GetLevelLookupWorkflowAsync(type);
    }
    [HttpGet]
    [Route("workflow-conditions")]
    public Task<ListResultDto<string?>> GetConditionLookupWorkflowAsync(string type)
    {
        return _lookupsAppService.GetConditionLookupWorkflowAsync(type);
    }
    [HttpGet]
    [Route("fy-psi")]
    public Task<ListResultDto<int?>> GetYearDistinctPSIAsync()
    {
        return _lookupsAppService.GetYearDistinctPSIAsync();
    }
    [HttpGet]
    [Route("history-dpo/{id}")]
    public Task<List<ApprovalHistoryDto>> GetDPOApprovalHistoriesAsync(Guid id)
    {
        return _lookupsAppService.GetDPOApprovalHistoriesAsync(id);
    }
    [HttpGet]
    [Route("history-so/{id}")]
    public Task<List<ApprovalHistoryDto>> GetSOHistoriesAsync(Guid id)
    {
        return _lookupsAppService.GetSOHistoriesAsync(id);
    }

    [HttpGet]
    [Route("history-add-more/{id}")]
    public Task<List<AddMoreItemHistoryDto>> AddMoreItemHistoryAsync(Guid id)
    {
        return _lookupsAppService.AddMoreItemHistoryAsync(id);
    }
    [HttpGet]
    [Route("katype-by-spotype")]
    public Task<List<string?>> GetKAbySPOAsync(string spoType)
    {
        return _lookupsAppService.GetKAbySPOAsync(spoType);
    }
    [HttpGet]
    [Route("customer-taxcode")]
    public Task<ListResultDto<LookupDto<Guid>>> GetCustomerTaxCodeLookupAsync(GetCustomersInput input)
    {
        return _lookupsAppService.GetCustomerTaxCodeLookupAsync(input);
    }
}
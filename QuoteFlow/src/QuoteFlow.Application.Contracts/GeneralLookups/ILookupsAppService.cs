using QuoteFlow.AddMoreItemHistories;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.Customers;
using QuoteFlow.Shared;
using QuoteFlow.SpecialInputPrices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.GeneralLookups;

public interface ILookupsAppService : IApplicationService
{
    Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountClassLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetCustomerTypeLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountTypeLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountEvaluationFinancialLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountEvaluationProductLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetMaterialGroupLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetMaterialTypeLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetWareHouseLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetVendorLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetLocationLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetProjectTypeLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetEUIndustryLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetBuyerLookupAsync(bool byPassBuyerCheck);
    Task<ListResultDto<LookupDto<Guid>>> GetDamagedStockCategoryLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetBuyerLookupByBuyerTypeAsync(Guid buyerTypeId);
    Task<List<UserLookupDto>> GetUserLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetBuyerTypeLookupAsync(GetBuyerTypeLookupInput input);
    Task<ListResultDto<LookupDto<Guid>>> GetNationalityLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetStockCategoryLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetMainStockCategoryLookupAsync();
    Task<ListResultDto<StockCategoryLookupDto<Guid>>> GetStockCategoryLookupWithAvailableAmountAsync(string materialCode, bool? damagedStock);
    Task<ListResultDto<LookupDto<Guid>>> GetCurrencyCategoryLookupAsync();
    Task<ListResultDto<KeyAccountLookupDto<Guid>>> GetKeyAccountLookupAsync(Guid? buyerId, string? materialType);
    Task<ListResultDto<LookupDto<Guid>>> GetProductCategoryLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetFinancialCategoryLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountClassChildLookupAsync(string keyAccountTypeCode, bool hiddenNA);
    Task<ListResultDto<SpecialInputPriceLookupDto<Guid>>> GetSpecialInputPriceLookupAsync(string? materialType);
    Task<ListResultDto<LookupDto<Guid>>> GetSupplierLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetSupplierBULookupAsync();
    Task<List<SupplierPOLookupDto>> GetSupplierPOLookupAsync(string? materialType, string? currency, string? createSource, bool? epa);
    Task<ListResultDto<SupplierBULookupDto<Guid>>> GetSupplierBUBySupplierLookupAsync(Guid supplierId);

    Task<ListResultDto<LookupDto<Guid>>> GetSupplierByMaterialTypeLookupAsync(string materialType);

    Task<ListResultDto<SupplierBULookupDto<Guid>>> GetSupplierBUBySupplierAndMaterialTypeLookupAsync(Guid supplierId, string materialType);

    Task<ListResultDto<AccountCodeLookupDto>> GetAccountNoAsync(string materialCode);
    Task<List<Shared.UserLookupDto>> GetListUserLookup(string name);
    Task<List<Shared.UserLookupDto>> GetListAllUserLookup();
    Task<List<Shared.UserLookupDto>> GetListUserPICLookup(GetSalePICLookupInput input);
    Task<List<UserLookupDto>> GetAllSalePICLookupAsync();
    Task<ListResultDto<int>> GetYearLookupAsync();
    Task<ListResultDto<int?>> GetFiscalYearOfDistributorTargetLookupAsync();
    Task<ListResultDto<GicTypeLookupDto<Guid>>> GetGicTypesLookupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetMaterialGroupPSILookupAsync(string materialType);
    Task<ListResultDto<LookupDto<Guid>>> GetBuyersNotAssignedToMaterialGroupAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetMaterialGroupByTypeAsync(string type);
    Task<ListResultDto<int?>> GetYearLookupKeyAccountAsync();
    Task<ListResultDto<LookupDto<Guid>>> GetFOCStockCategoryLookupAsync();
    Task<List<ApprovalHistoryDto>> GetDPOApprovalHistoriesAsync(Guid id);
    Task<List<ApprovalHistoryDto>> GetSOHistoriesAsync(Guid id);
    Task<List<AddMoreItemHistoryDto>> AddMoreItemHistoryAsync(Guid id);


    Task<ListResultDto<short?>> GetLevelLookupWorkflowAsync(string type);
    Task<ListResultDto<string?>> GetConditionLookupWorkflowAsync(string type);
    Task<ListResultDto<int?>> GetYearDistinctPSIAsync();
    Task<List<string>> GetSpoTypeLookupAsync();
    Task<List<string?>> GetKAbySPOAsync(string spoType);

    Task<ListResultDto<LookupDto<Guid>>> GetCustomerTaxCodeLookupAsync(GetCustomersInput input);
}

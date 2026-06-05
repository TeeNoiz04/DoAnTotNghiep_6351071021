using QuoteFlow.ApprovalRoutes;
using QuoteFlow.HistoryTrackings;
using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.Materials.MaterialImport.MaterialFactory;
using QuoteFlow.Materials.MaterialImport.MaterialSAP;
using QuoteFlow.Materials.MaterialImport.MaterialStatus;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.Materials;

public interface IMaterialsAppService : IApplicationService
{

    Task<PagedResultDto<MaterialDto>> GetListAsync(GetMaterialsInput input);

    Task<PagedResultDto<MaterialApprovalRequestDto>> GetListApprovalAsync(GetMaterialsApprovalInput input);
    Task<PagedResultDto<MaterialApprovalRequestDto>> GetListMyApprovalAsync(GetMaterialsApprovalInput input);
    Task<PagedResultDto<MaterialApprovalRequestDetailDto>> GetListByApprovalIdAsync(Guid id);

    Task<MaterialApprovalRequestDto> GetMaterialApprovalDetailAsync(Guid approvalId);

    Task<MaterialDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<MaterialDto> CreateAsync(MaterialCreateDto input);

    Task<MaterialDto> UpdateAsync(Guid id, MaterialUpdateDto input);

    //Task<MaterialApprovalRequestDto> SubmitAsync(Guid id, MaterialApprovalRequestSubmitDto input);
    Task<MaterialApprovalRequestDto> PerformActionAsync(Guid id, ActionDto input);

    Task<MaterialApprovalRequestDto> ImportMaterialUpdatePriceAsync(ExcelValidationResult<MaterialUpdatePriceImportDto> data, string? note = null);
    Task<ExcelValidationResult<MaterialUpdatePriceImportDto>> ValidateAndParseUpdatePriceAsync(IRemoteStreamContent file);
    Task<MaterialApprovalRequestDto> ImportMaterialNewRegistrationAsync(ExcelValidationResult<MaterialNewRegistrationImportDto> data, string? note = null);
    Task<ExcelValidationResult<MaterialNewRegistrationImportDto>> ValidateAndParseNewRegistrationAsync(IRemoteStreamContent file);
    Task<ExcelValidationResult<MaterialUpdateWithoutPriceImportDto>> ValidateAndParseUpdateWithoutPriceAsync(IRemoteStreamContent file);
    Task<MaterialApprovalRequestDto> ImportMaterialUpdateWithoutPriceAsync(ExcelValidationResult<MaterialUpdateWithoutPriceImportDto> data, string? note = null);
    Task<ExcelValidationResult<MaterialUpdateInventoryPlanImportDto>> ValidateAndParseUpdateInventoryPlanAsync(IRemoteStreamContent file);
    Task<MaterialApprovalRequestDto> MaterialUpdateInventoryPlanAsync(ExcelValidationResult<MaterialUpdateInventoryPlanImportDto> data, string? note);

    Task<ExcelValidationResult<MaterialSAPUpdateExcelDto>> ValidateAndParseSAPAsync(IRemoteStreamContent file);
    Task UpdateMaterialSAPAsync(ExcelValidationResult<MaterialSAPUpdateExcelDto> data, string? note);

    Task<ExcelValidationResult<MaterialFactoryUpdateExcelDto>> ValidateAndParseFactoryAsync(IRemoteStreamContent file);
    Task UpdateMaterialFactoryAsync(ExcelValidationResult<MaterialFactoryUpdateExcelDto> data, string? note);

    Task<ExcelValidationResult<MaterialStatusUpdateExcelDto>> ValidateAndParseStatusAsync(IRemoteStreamContent file);
    Task<MaterialApprovalRequestDto> UpdateMaterialStatusAsync(ExcelValidationResult<MaterialStatusUpdateExcelDto> data, string? note);
    Task<List<ApproverDto>> GetListApproversAsync(Guid materialId);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetMaterialsInput input);
    Task<List<HistoryTrackingDto>> GetListMaterialHistoryAsync(string golfaCode);
    Task<IRemoteStreamContent> GetMaterialHistoryAsExcelAsync(string golfaCode);

}
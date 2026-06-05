using Asp.Versioning;
using QuoteFlow.ApprovalRoutes;
using QuoteFlow.HistoryTrackings;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.Materials.MaterialImport.MaterialFactory;
using QuoteFlow.Materials.MaterialImport.MaterialSAP;
using QuoteFlow.Materials.MaterialImport.MaterialStatus;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.Materials;

[RemoteService]
[Area("app")]
[ControllerName("Material")]
[Route("api/app/materials")]

public class MaterialController : AbpController, IMaterialsAppService
{
    protected IMaterialsAppService _materialsAppService;

    //private List<MaterialDto> _materialData;
    public MaterialController(IMaterialsAppService materialsAppService)
    {
        _materialsAppService = materialsAppService;


        //foreach (var item in _materialApprovalData)
        //{
        //    int randomDetailCount = Random.Shared.Next(1, 11);
        //    var details = seeder.GenerateApprovalListDetail(randomDetailCount, item.Id, seed);
        //    _materialApprovalDetailData.AddRange(details);
        //}
        //_materialData = seeder.Generate(count, seed);


    }

    [HttpGet]
    public virtual Task<PagedResultDto<MaterialDto>> GetListAsync(GetMaterialsInput input)
    {
        return _materialsAppService.GetListAsync(input);

        //// Simulate in-memory data retrieval
        //var filteredData = _materialData;
        //var pagedResult = filteredData
        //    .Skip(input.SkipCount)
        //    .Take(input.MaxResultCount)
        //    .ToList();
        //var totalCount = filteredData.Count;
        //return Task.FromResult(new PagedResultDto<MaterialDto>(totalCount, pagedResult));
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<MaterialDto> GetAsync(Guid id)
    {
        return _materialsAppService.GetAsync(id);

        // Simulate in-memory data retrieval
        //var material = _materialData.FirstOrDefault(m => m.Id == id)
        //    ?? throw new EntityNotFoundException(typeof(MaterialDto), id);

        //return Task.FromResult(material);
    }

    [HttpPost]
    public virtual Task<MaterialDto> CreateAsync(MaterialCreateDto input)
    {
        return _materialsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<MaterialDto> UpdateAsync(Guid id, MaterialUpdateDto input)
    {
        return _materialsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _materialsAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("approval")]
    public Task<PagedResultDto<MaterialApprovalRequestDto>> GetListApprovalAsync(GetMaterialsApprovalInput input)
    {
        return _materialsAppService.GetListApprovalAsync(input);
    }

    [HttpGet]
    [Route("my-approval-detail/{approvalId}")]
    public Task<MaterialApprovalRequestDto> GetMaterialApprovalDetailAsync(Guid approvalId)
    {
        return _materialsAppService.GetMaterialApprovalDetailAsync(approvalId);
    }

    [HttpPost]
    [Route("validate-and-parse/update-price")]
    public Task<ExcelValidationResult<MaterialUpdatePriceImportDto>> ValidateAndParseUpdatePriceAsync(IRemoteStreamContent file)
    {
        return _materialsAppService.ValidateAndParseUpdatePriceAsync(file);
    }

    [HttpPost]
    [Route("validate-and-parse/new-registration")]
    public Task<ExcelValidationResult<MaterialNewRegistrationImportDto>> ValidateAndParseNewRegistrationAsync(IRemoteStreamContent file)
    {
        return _materialsAppService.ValidateAndParseNewRegistrationAsync(file);
    }

    [HttpPost]
    [Route("validate-and-parse/update-without-price")]
    public Task<ExcelValidationResult<MaterialUpdateWithoutPriceImportDto>> ValidateAndParseUpdateWithoutPriceAsync(IRemoteStreamContent file)
    {
        return _materialsAppService.ValidateAndParseUpdateWithoutPriceAsync(file);
    }
    [HttpPost]
    [Route("import/update-without-price")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = 150 * 1024 * 1024)]
    public Task<MaterialApprovalRequestDto> ImportMaterialUpdateWithoutPriceAsync(ExcelValidationResult<MaterialUpdateWithoutPriceImportDto> data, string? note = null)
    {
        return _materialsAppService.ImportMaterialUpdateWithoutPriceAsync(data, note);
    }

    [HttpPost]
    [Route("validate-and-parse/update-inventory-plan")]
    public Task<ExcelValidationResult<MaterialUpdateInventoryPlanImportDto>> ValidateAndParseUpdateInventoryPlanAsync(IRemoteStreamContent file)
    {
        return _materialsAppService.ValidateAndParseUpdateInventoryPlanAsync(file);
    }

    [HttpPost]
    [Route("import/update-inventory-plan")]
    public Task<MaterialApprovalRequestDto> MaterialUpdateInventoryPlanAsync(ExcelValidationResult<MaterialUpdateInventoryPlanImportDto> data, string? note = null)
    {
        return _materialsAppService.MaterialUpdateInventoryPlanAsync(data, note);
    }

    [HttpPost]
    [Route("import/material-new-registration")]
    public Task<MaterialApprovalRequestDto> ImportMaterialNewRegistrationAsync(ExcelValidationResult<MaterialNewRegistrationImportDto> data, string? note = null)
    {
        return _materialsAppService.ImportMaterialNewRegistrationAsync(data, note);
    }

    [HttpPost]
    [Route("import/material-update-price")]
    public Task<MaterialApprovalRequestDto> ImportMaterialUpdatePriceAsync(ExcelValidationResult<MaterialUpdatePriceImportDto> data, string? note = null)
    {
        return _materialsAppService.ImportMaterialUpdatePriceAsync(data, note);
    }

    //[HttpPost]
    //[Route("submit")]
    //public Task<MaterialApprovalRequestDto> SubmitAsync(Guid id, MaterialApprovalRequestSubmitDto input)
    //{
    //    return _materialsAppService.SubmitAsync(id, input);
    //}

    [HttpPost]
    [Route("perform-action")]
    public Task<MaterialApprovalRequestDto> PerformActionAsync(Guid id, ActionDto input)
    {
        return _materialsAppService.PerformActionAsync(id, input);
    }

    [HttpGet]
    [Route("my-approval")]
    public Task<PagedResultDto<MaterialApprovalRequestDto>> GetListMyApprovalAsync(GetMaterialsApprovalInput input)
    {
        return _materialsAppService.GetListMyApprovalAsync(input);
    }

    [HttpPost]
    [Route("validate-and-parse/sap")]
    public Task<ExcelValidationResult<MaterialSAPUpdateExcelDto>> ValidateAndParseSAPAsync(IRemoteStreamContent file)
    {
        return _materialsAppService.ValidateAndParseSAPAsync(file);
    }

    [HttpPost]
    [Route("import/sap")]
    public Task UpdateMaterialSAPAsync(ExcelValidationResult<MaterialSAPUpdateExcelDto> data, string? note)
    {
        return _materialsAppService.UpdateMaterialSAPAsync(data, note);
    }

    [HttpPost]
    [Route("validate-and-parse/factory")]
    public Task<ExcelValidationResult<MaterialFactoryUpdateExcelDto>> ValidateAndParseFactoryAsync(IRemoteStreamContent file)
    {
        return _materialsAppService.ValidateAndParseFactoryAsync(file);
    }

    [HttpPost]
    [Route("import/factory")]
    public Task UpdateMaterialFactoryAsync(ExcelValidationResult<MaterialFactoryUpdateExcelDto> data, string? note)
    {
        return _materialsAppService.UpdateMaterialFactoryAsync(data, note);
    }

    [HttpPost]
    [Route("validate-and-parse/status")]
    public Task<ExcelValidationResult<MaterialStatusUpdateExcelDto>> ValidateAndParseStatusAsync(IRemoteStreamContent file)
    {
        return _materialsAppService.ValidateAndParseStatusAsync(file);
    }

    [HttpPost]
    [Route("import/status")]
    public Task<MaterialApprovalRequestDto> UpdateMaterialStatusAsync(ExcelValidationResult<MaterialStatusUpdateExcelDto> data, string? note)
    {
        return _materialsAppService.UpdateMaterialStatusAsync(data, note);
    }

    [HttpGet]
    [Route("{id}/approval-details")]
    public Task<PagedResultDto<MaterialApprovalRequestDetailDto>> GetListByApprovalIdAsync(Guid id)
    {
        return _materialsAppService.GetListByApprovalIdAsync(id);
    }

    [HttpGet]
    [Route("{materialId}/approvers")]
    public Task<List<ApproverDto>> GetListApproversAsync(Guid materialId)
    {
        return _materialsAppService.GetListApproversAsync(materialId);
    }
    [HttpGet]
    [Route("export")]
    public Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetMaterialsInput input)
    {
        return _materialsAppService.GetListAsExcelFileAsync(input);
    }
    [HttpGet]
    [Route("history-tracking")]
    public Task<List<HistoryTrackingDto>> GetListMaterialHistoryAsync([FromQuery] string golfaCode)
    {
        return _materialsAppService.GetListMaterialHistoryAsync(golfaCode);
    }
    [HttpGet]
    [Route("history-tracking/export")]
    public Task<IRemoteStreamContent> GetMaterialHistoryAsExcelAsync([FromQuery] string golfaCode)
    {
        return _materialsAppService.GetMaterialHistoryAsExcelAsync(golfaCode);
    }
}
using Asp.Versioning;
using QuoteFlow.ApprovalRoutes;
using QuoteFlow.PSIs;
using QuoteFlow.Seeders;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.PSIs;

[RemoteService]
[Area("app")]
[ControllerName("PSI")]
[Route("api/app/psi")]
public class PSIController : AbpController, IPSIsAppService
{
    protected IPSIsAppService _pSIsAppService;
    protected readonly List<PSIDto> _pSIData;

    public PSIController(IPSIsAppService pSIsAppService)
    {
        _pSIsAppService = pSIsAppService;

        var seeder = new PSISeeder();
        var seed = 1234;
        var count = 1000;
        _pSIData = seeder.Generate(count, seed);
    }
    [HttpGet]
    public virtual Task<PagedResultDto<PSIDto>> GetListAsync(GetPSIsInput input)
    {
        return _pSIsAppService.GetListAsync(input);
    }
    [HttpGet]
    [Route("pending")]
    public virtual Task<PagedResultDto<PSIDto>> GetListPendingAsync(GetPSIsInput input)
    {
        return _pSIsAppService.GetListPendingAsync(input);
    }
    [HttpGet]
    [Route("{id}")]
    public Task<PSIDto> GetAsync(Guid id)
    {
        return _pSIsAppService.GetAsync(id);
    }


    [HttpPost]
    public Task<PSIDto> CreateAsync(PSICreateDto input)
    {
        return _pSIsAppService.CreateAsync(input);
    }
    [HttpPut]
    [Route("{id}")]
    public Task<PSIDto> UpdateAsync(Guid id, PSIUpdateDto input)
    {
        return _pSIsAppService.UpdateAsync(id, input);
        //var systemCategory = _pSIData.FirstOrDefault(c => c.Id == id)
        //     ?? throw new EntityNotFoundException(typeof(SystemCategoryDto), id);
        //return Task.FromResult(systemCategory);
    }
    [HttpDelete]
    [Route("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _pSIsAppService.DeleteAsync(id);
    }
    [HttpGet]
    [Route("as-excel-file")]
    public Task<IRemoteStreamContent> GetListAsExcelFileAsync(PSIExcelDownloadDto input)
    {
        return _pSIsAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _pSIsAppService.GetDownloadTokenAsync();
    }
    [HttpGet]
    [Route("psi-report")]
    public Task<PagedResultDto<PSIReportDto>> GetListPSIReportAsync(GetPSIReportsInput input)
    {
        //return _pSIsAppService.GetListPSIReportAsync(input);
        var allReports = PSISeeder.GeneratePSIReports(500, 1001);

        var paged = allReports
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        var result = new PagedResultDto<PSIReportDto>
        {
            TotalCount = allReports.Count,
            Items = paged
        };

        return Task.FromResult(result);
    }
    [HttpGet]
    [Route("psi-report/export")]
    public Task<IRemoteStreamContent> GetListAsExcelAsync(GetPSIReportsInput input)
    {
        return _pSIsAppService.GetListAsExcelAsync(input);
    }
    [HttpGet]
    [Route("detail/{id}/psi-data")]
    public async Task<PSIDto> GetDetailAsync(Guid id)
    {
        return await _pSIsAppService.GetDetailAsync(id);
    }
    [HttpGet]
    [Route("psi-report-data")]
    public Task<List<PSIExportDataDto>> GetListPSIReportDataAsync(PSIByProductExportInput input)
    {
        return _pSIsAppService.GetListPSIReportDataAsync(input);
    }
    [HttpPost]
    [Route("validate-psi-fa")]
    public Task<ExcelValidationResult<PSIImportDto>> ValidateAndParseFAAsync([FromForm] IRemoteStreamContent file, [FromForm] GetPSIImportsInput input)
    {
        return _pSIsAppService.ValidateAndParseFAAsync(file, input);
    }

    [HttpPost]
    [Route("import-psi-fa")]
    public Task ImportFAAsync(ExcelValidationResult<PSIImportDto> validationResult)
    {
        return _pSIsAppService.ImportFAAsync(validationResult);
    }

    [HttpPost]
    [Route("validate-psi-lvs")]
    public Task<ExcelValidationResult<PSIImportDto>> ValidateAndParseLVSAsync([FromForm] IRemoteStreamContent file, [FromForm] GetPSIImportsInput input)
    {
        return _pSIsAppService.ValidateAndParseLVSAsync(file, input);
    }
    [HttpPost]
    [Route("import-psi-lvs")]
    public Task ImportLVSAsync(ExcelValidationResult<PSIImportDto> validationResult)
    {
        return _pSIsAppService.ImportLVSAsync(validationResult);
    }
    [HttpGet]
    [Route("approvers")]
    public Task<List<ApproverDto>> GetListApproversAsync(Guid pSI_Id)
    {
        return _pSIsAppService.GetListApproversAsync(pSI_Id);
    }
    [HttpPost]
    [Route("perform-actions")]
    public Task<PSIDto> PerformActionAsync(Guid id, ActionDto input)
    {
        return _pSIsAppService.PerformActionAsync(id, input);
    }
    [HttpPost]
    [Route("validation-psi")]
    public Task ValidationPSIAsync(string materialType, string? currency, decimal? total)
    {
        return _pSIsAppService.ValidationPSIAsync(materialType, currency, total);
    }

    [HttpPost]
    [Route("psi-by-product-export")]
    public Task<IRemoteStreamContent> GetPSIByProductExportAsync(PSIByProductExportInput input)
    {
        return _pSIsAppService.GetPSIByProductExportAsync(input);
    }
}

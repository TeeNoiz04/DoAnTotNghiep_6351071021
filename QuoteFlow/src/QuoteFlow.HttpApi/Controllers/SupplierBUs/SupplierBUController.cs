using Asp.Versioning;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SupplierBUs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.SupplierBUs;

[RemoteService]
[Area("app")]
[ControllerName("SupplierBU")]
[Route("api/app/supplier-bUs")]

public class SupplierBUController : AbpController, ISupplierBUsAppService
{
    protected ISupplierBUsAppService _supplierBUsAppService;

    public SupplierBUController(ISupplierBUsAppService supplierBUsAppService)
    {
        _supplierBUsAppService = supplierBUsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<SupplierBUDto>> GetListAsync(GetSupplierBUsInput input)
    {
        return _supplierBUsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<SupplierBUDto> GetAsync(Guid id)
    {
        return _supplierBUsAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<SupplierBUDto> CreateAsync(SupplierBUCreateDto input)
    {
        return _supplierBUsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<SupplierBUDto> UpdateAsync(Guid id, SupplierBUUpdateDto input)
    {
        return _supplierBUsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _supplierBUsAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(SupplierBUExcelDownloadDto input)
    {
        return _supplierBUsAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _supplierBUsAppService.GetDownloadTokenAsync();
    }
    [HttpPost]
    [Route("validater-and-parse")]

    public Task<ExcelValidationResult<SupplierBUImportDto>> ValidateAndParseSupplierBUAsync(IRemoteStreamContent file)
    {
        return _supplierBUsAppService.ValidateAndParseSupplierBUAsync(file);
    }
    [HttpPost]
    [Route("import")]
    public Task<List<SupplierBUDto>> ImportSupplierBUAsync(ExcelValidationResult<SupplierBUImportDto> dataImport)
    {
        return _supplierBUsAppService.ImportSupplierBUAsync(dataImport);
    }
    [HttpPost]
    [Route("change-active")]
    public Task ChangeDeactiveSupplierBUAsync(List<Guid> ids)
    {
        return _supplierBUsAppService.ChangeDeactiveSupplierBUAsync(ids);
    }
}
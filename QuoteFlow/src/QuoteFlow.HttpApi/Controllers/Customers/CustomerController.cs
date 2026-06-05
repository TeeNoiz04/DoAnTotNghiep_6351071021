using Asp.Versioning;
using QuoteFlow.Customers;
using QuoteFlow.Shared.Excels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.Customers;

[RemoteService]
[Area("app")]
[ControllerName("Customer")]
[Route("api/app/customers")]

public class CustomerController : AbpController, ICustomersAppService
{
    protected ICustomersAppService _customersAppService;

    public CustomerController(ICustomersAppService customersAppService)
    {
        _customersAppService = customersAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomersInput input)
    {
        return _customersAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<CustomerDto> GetAsync(Guid id)
    {
        return _customersAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<CustomerDto> CreateAsync(CustomerCreateDto input)
    {
        return _customersAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<CustomerDto> UpdateAsync(Guid id, CustomerUpdateDto input)
    {
        return _customersAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _customersAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(CustomerExcelDownloadDto input)
    {
        return _customersAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _customersAppService.GetDownloadTokenAsync();
    }
    [HttpPost]
    [Route("validater-and-parse")]
    public Task<ExcelValidationResult<CustomerImportDto>> ValidateAndParseCustomerAsync(IRemoteStreamContent file)
    {
        return _customersAppService.ValidateAndParseCustomerAsync(file);
    }
    [HttpPost]
    [Route("import")]
    public Task<List<CustomerDto>> ImportCustomerAsync(ExcelValidationResult<CustomerImportDto> dataImport)
    {
        return _customersAppService.ImportCustomerAsync(dataImport);
    }
}
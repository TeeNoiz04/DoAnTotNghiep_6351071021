using Asp.Versioning;
using QuoteFlow.Suppliers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.Suppliers;

[RemoteService]
[Area("app")]
[ControllerName("Supplier")]
[Route("api/app/suppliers")]
public class SupplierController : AbpController, ISupplierAppService
{
    protected ISupplierAppService _supplierAppService;
    public SupplierController(ISupplierAppService supplierAppService)
    {
        _supplierAppService = supplierAppService;
    }
    [HttpPost]
    public virtual Task<SupplierDto> CreateAsync(SupplierCreateDto input)
    {
        return _supplierAppService.CreateAsync(input);
    }
    [HttpDelete]
    [Route("{id}")]

    public virtual Task DeleteAsync(Guid id)
    {
        return _supplierAppService.DeleteAsync(id);
    }
    [HttpGet]
    [Route("{id}")]

    public Task<SupplierDto> GetAsync(Guid id)
    {
        return _supplierAppService.GetAsync(id);
    }
    [HttpGet]
    public Task<PagedResultDto<SupplierDto>> GetListAsync(GetSuppliersInput input)
    {
        return _supplierAppService.GetListAsync(input);
    }
    [HttpPut]
    [Route("{id}")]
    public Task<SupplierDto> UpdateAsync(Guid id, SupplierUpdateDto input)
    {
        return _supplierAppService.UpdateAsync(id, input);
    }
}

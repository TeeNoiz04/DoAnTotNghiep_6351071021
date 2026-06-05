using Asp.Versioning;
using QuoteFlow.CustomerPICs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.CustomerPICs;

[RemoteService]
[Area("app")]
[ControllerName("CustomerPIC")]
[Route("api/app/customer-pICs")]

public class CustomerPICController : AbpController, ICustomerPICsAppService
{
    protected ICustomerPICsAppService _customerPICsAppService;

    public CustomerPICController(ICustomerPICsAppService customerPICsAppService)
    {
        _customerPICsAppService = customerPICsAppService;
    }

    [HttpPost]
    public virtual Task<CustomerPICDto> CreateAsync(CustomerPICCreateDto input)
    {
        return _customerPICsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<CustomerPICDto> UpdateAsync(Guid id, CustomerPICUpdateDto input)
    {
        return _customerPICsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _customerPICsAppService.DeleteAsync(id);
    }
}
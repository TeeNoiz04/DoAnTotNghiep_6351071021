using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace QuoteFlow.CustomerPICs;

[RemoteService(IsEnabled = false)]
public class CustomerPICsAppService : QuoteFlowAppService, ICustomerPICsAppService
{

    protected ICustomerPICRepository _customerPICRepository;
    protected CustomerPICManager _customerPICManager;

    public CustomerPICsAppService(ICustomerPICRepository customerPICRepository, CustomerPICManager customerPICManager)
    {

        _customerPICRepository = customerPICRepository;
        _customerPICManager = customerPICManager;

    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await _customerPICRepository.DeleteAsync(id);
    }

    public virtual async Task<CustomerPICDto> CreateAsync(CustomerPICCreateDto input)
    {

        var customerPIC = await _customerPICManager.CreateAsync(
        input.KeyAccountId, input.PICName, input.PICPhone, input.PICEmail, input.PICJobTitle, input.Remark
        );

        return ObjectMapper.Map<CustomerPIC, CustomerPICDto>(customerPIC);
    }


    public virtual async Task<CustomerPICDto> UpdateAsync(Guid id, CustomerPICUpdateDto input)
    {

        var customerPIC = await _customerPICManager.UpdateAsync(
        id,
        input.KeyAccountId, input.PICName, input.PICPhone, input.PICEmail, input.PICJobTitle, input.Remark, input.ConcurrencyStamp
        );

        return ObjectMapper.Map<CustomerPIC, CustomerPICDto>(customerPIC);
    }
}
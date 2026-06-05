using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace QuoteFlow.CustomerPICs;

public interface ICustomerPICsAppService : IApplicationService
{

    Task DeleteAsync(Guid id);

    Task<CustomerPICDto> CreateAsync(CustomerPICCreateDto input);

    Task<CustomerPICDto> UpdateAsync(Guid id, CustomerPICUpdateDto input);
}
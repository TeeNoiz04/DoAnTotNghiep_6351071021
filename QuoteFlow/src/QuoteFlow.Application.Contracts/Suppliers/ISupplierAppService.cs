using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.Suppliers;

public interface ISupplierAppService : IApplicationService
{
    Task<PagedResultDto<SupplierDto>> GetListAsync(GetSuppliersInput input);

    Task<SupplierDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<SupplierDto> CreateAsync(SupplierCreateDto input);

    Task<SupplierDto> UpdateAsync(Guid id, SupplierUpdateDto input);
}

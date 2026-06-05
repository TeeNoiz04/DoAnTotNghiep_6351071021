using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.SaleOrdersSapImports;

public interface ISaleOrdersSapImportsAppService : IApplicationService
{

    Task<PagedResultDto<SaleOrdersSapImportDto>> GetListAsync(GetSaleOrdersSapImportsInput input);

    Task<SaleOrdersSapImportDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<SaleOrdersSapImportDto> CreateAsync(SaleOrdersSapImportCreateDto input);

    Task<SaleOrdersSapImportDto> UpdateAsync(Guid id, SaleOrdersSapImportUpdateDto input);
}
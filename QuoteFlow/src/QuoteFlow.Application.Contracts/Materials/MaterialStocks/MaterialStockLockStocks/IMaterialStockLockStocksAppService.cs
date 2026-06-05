using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;

public interface IMaterialStockLockStocksAppService : IApplicationService
{

    Task<PagedResultDto<MaterialStockLockStockDto>> GetListAsync(GetMaterialStockLockStocksInput input);

    Task<MaterialStockLockStockDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<MaterialStockLockStockDto> CreateAsync(MaterialStockLockStockCreateDto input);

    Task<MaterialStockLockStockDto> UpdateAsync(Guid id, MaterialStockLockStockUpdateDto input);
}
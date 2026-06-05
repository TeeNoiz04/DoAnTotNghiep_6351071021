using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.StockCategories;

public interface IStockCategoriesAppService : IApplicationService
{

    Task<PagedResultDto<StockCategoryDto>> GetListAsync(GetStockCategoriesInput input);

    Task<StockCategoryDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<StockCategoryDto> CreateAsync(StockCategoryCreateDto input);

    Task<StockCategoryDto> UpdateAsync(Guid id, StockCategoryUpdateDto input);
}
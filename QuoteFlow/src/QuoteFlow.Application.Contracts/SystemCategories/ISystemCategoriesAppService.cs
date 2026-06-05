using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.SystemCategories;

public interface ISystemCategoriesAppService : IApplicationService
{

    Task<PagedResultDto<SystemCategoryDto>> GetListAsync(GetSystemCategoriesInput input);

    Task<SystemCategoryDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<SystemCategoryDto> CreateAsync(SystemCategoryCreateDto input);

    Task<SystemCategoryDto> UpdateAsync(Guid id, SystemCategoryUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(SystemCategoryExcelDownloadDto input);

    Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

}
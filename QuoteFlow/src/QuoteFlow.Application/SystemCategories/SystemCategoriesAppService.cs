using QuoteFlow.SystemCategories.ParameterObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;

namespace QuoteFlow.SystemCategories;

[RemoteService(IsEnabled = false)]

public class SystemCategoriesAppService : QuoteFlowAppService, ISystemCategoriesAppService
{
    protected IDistributedCache<SystemCategoryDownloadTokenCacheItem, string> _downloadTokenCache;
    protected ISystemCategoryRepository _systemCategoryRepository;
    protected SystemCategoryManager _systemCategoryManager;

    public SystemCategoriesAppService(ISystemCategoryRepository systemCategoryRepository, SystemCategoryManager systemCategoryManager, IDistributedCache<SystemCategoryDownloadTokenCacheItem, string> downloadTokenCache)
    {
        _downloadTokenCache = downloadTokenCache;
        _systemCategoryRepository = systemCategoryRepository;
        _systemCategoryManager = systemCategoryManager;

    }

    public virtual async Task<PagedResultDto<SystemCategoryDto>> GetListAsync(GetSystemCategoriesInput input)
    {
        var filterParams = ObjectMapper.Map<GetSystemCategoriesInput, SystemCategoryFilterParams>(input);
        var totalCount = await _systemCategoryRepository.GetCountAsync(filterParams);
        var items = await _systemCategoryRepository.GetListAsync(filterParams, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<SystemCategoryDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<SystemCategory>, List<SystemCategoryDto>>(items)
        };
    }

    public virtual async Task<SystemCategoryDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<SystemCategory, SystemCategoryDto>(await _systemCategoryRepository.GetAsync(id));
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await _systemCategoryRepository.DeleteAsync(id);
    }


    public virtual async Task<SystemCategoryDto> CreateAsync(SystemCategoryCreateDto input)
    {
        var createParams = ObjectMapper.Map<SystemCategoryCreateDto, SystemCategoryCreateParams>(input);
        var systemCategory = await _systemCategoryManager.CreateAsync(createParams);

        return ObjectMapper.Map<SystemCategory, SystemCategoryDto>(systemCategory);
    }


    public virtual async Task<SystemCategoryDto> UpdateAsync(Guid id, SystemCategoryUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<SystemCategoryUpdateDto, SystemCategoryUpdateParams>(input);

        var systemCategory = await _systemCategoryManager.UpdateAsync(
        id,
        updateParams, input.ConcurrencyStamp
        );

        return ObjectMapper.Map<SystemCategory, SystemCategoryDto>(systemCategory);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(SystemCategoryExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }
        var filterParams = ObjectMapper.Map<SystemCategoryExcelDownloadDto, SystemCategoryFilterParams>(input);
        var items = await _systemCategoryRepository.GetListAsync(filterParams);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<SystemCategory>, List<SystemCategoryExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "SystemCategories.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new SystemCategoryDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new QuoteFlow.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}
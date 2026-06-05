using QuoteFlow.Permissions;
using QuoteFlow.StockCategories.ParameterObjects;
using QuoteFlow.SystemCategories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace QuoteFlow.StockCategories;
[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.MasterDatas.StorageLocation)]
public class StockCategoriesAppService : QuoteFlowAppService, IStockCategoriesAppService
{
    protected IDistributedCache<SystemCategoryDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IStockCategoryRepository _stockCategoryRepository;
    protected StockCategoryManager _stockCategoryManager;

    public StockCategoriesAppService(IStockCategoryRepository stockCategoryRepository, IDistributedCache<SystemCategoryDownloadTokenCacheItem, string> downloadTokenCache, StockCategoryManager stockCategoryManager)
    {
        _downloadTokenCache = downloadTokenCache;
        _stockCategoryRepository = stockCategoryRepository;
        _stockCategoryManager = stockCategoryManager;
    }

    public virtual async Task<PagedResultDto<StockCategoryDto>> GetListAsync(GetStockCategoriesInput input)
    {
        var filterParams = ObjectMapper.Map<GetStockCategoriesInput, StockCategoryFilterParams>(input);
        var totalCount = await _stockCategoryRepository.GetCountAsync(filterParams);
        var items = await _stockCategoryRepository.GetListAsync(filterParams);

        return new PagedResultDto<StockCategoryDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<StockCategory>, List<StockCategoryDto>>(items)
        };
    }

    public virtual async Task<StockCategoryDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<StockCategory, StockCategoryDto>(await _stockCategoryRepository.GetAsync(id));
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        var mainStocks = await _stockCategoryRepository.GetListAsync(x => x.MainStock == true && x.Id != id);
        if (!mainStocks.Any())
        {
            throw new UserFriendlyException("Must have a Main Stock.");
        }
        await _stockCategoryRepository.DeleteAsync(id);
    }

    [UnitOfWork]
    public virtual async Task<StockCategoryDto> CreateAsync(StockCategoryCreateDto input)
    {
        var createParams = ObjectMapper.Map<StockCategoryCreateDto, StockCategoryCreateParams>(input);
        if (!string.IsNullOrWhiteSpace(createParams.SAPCode))
        {
            var existedSAP = await _stockCategoryRepository.AnyAsync(x => x.SAPCode == createParams.SAPCode);
            if (existedSAP)
            {
                throw new UserFriendlyException($"SAP Code '{createParams.SAPCode}' already exists.");
            }
        }
        var existedStockCode = await _stockCategoryRepository.AnyAsync(x => x.SAPCode == createParams.StockCode);
        if (existedStockCode)
        {
            throw new UserFriendlyException($"Stock Code '{createParams.StockCode}' already exists.");
        }

        if (createParams.MainStock == true)
        {
            var mainStocks = await _stockCategoryRepository.GetListAsync(x => x.MainStock == true);
            var mainStockIds = mainStocks.Select(x => x.Id).ToList();
            await RemoveMainStock(mainStockIds);
        }
        var systemCategory = await _stockCategoryManager.CreateAsync(createParams);

        return ObjectMapper.Map<StockCategory, StockCategoryDto>(systemCategory);
    }
    [UnitOfWork]
    public virtual async Task<StockCategoryDto> UpdateAsync(Guid id, StockCategoryUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<StockCategoryUpdateDto, StockCategoryUpdateParams>(input);
        if (!string.IsNullOrWhiteSpace(updateParams.SAPCode))
        {
            var existedSAP = await _stockCategoryRepository.AnyAsync(x => x.SAPCode == updateParams.SAPCode && x.Id != id);
            if (existedSAP)
            {
                throw new UserFriendlyException($"SAP Code '{updateParams.SAPCode}' already exists.");
            }
        }

        var mainStocks = await _stockCategoryRepository.GetListAsync(x => x.MainStock == true && x.Id != id);
        var mainStockIds = mainStocks.Select(x => x.Id).ToList();
        if (updateParams.MainStock == true)
        {
            if (mainStockIds.Any())
            {
                await RemoveMainStock(mainStockIds);
            }
        }
        else
        {
            if (!mainStockIds.Any())
            {
                throw new UserFriendlyException("Must have a Main Stock.");
            }
        }
        var systemCategory = await _stockCategoryManager.UpdateAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<StockCategory, StockCategoryDto>(systemCategory);
    }
    public virtual async Task RemoveMainStock(List<Guid> ids)
    {
        await _stockCategoryManager.RemoveMainStock(ids);
    }

}
using QuoteFlow.StockTracingDetails.ParameterObjects;
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

namespace QuoteFlow.StockTracingDetails;

[RemoteService(IsEnabled = false)]

public class StockTracingDetailsAppService : QuoteFlowAppService, IStockTracingDetailsAppService
{
    protected IDistributedCache<StockTracingDetailDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IStockTracingDetailRepository _stockTracingDetailRepository;
    protected StockTracingDetailManager _stockTracingDetailManager;

    public StockTracingDetailsAppService(IStockTracingDetailRepository stockTracingDetailRepository, StockTracingDetailManager stockTracingDetailManager, IDistributedCache<StockTracingDetailDownloadTokenCacheItem, string> downloadTokenCache)
    {
        _downloadTokenCache = downloadTokenCache;
        _stockTracingDetailRepository = stockTracingDetailRepository;
        _stockTracingDetailManager = stockTracingDetailManager;

    }

    private const int MAX_EXCEL_EXPORT_RECORDS = 10000;
    private const int MAX_SKIP_COUNT_RECORDS = 0;

    public virtual async Task<PagedResultDto<StockTracingDetailDto>> GetListAsync(GetStockTracingDetailsInput input)
    {
        var filterParams = ObjectMapper.Map<GetStockTracingDetailsInput, StockTracingDetailFilterParams>(input);

        var realTotal = await _stockTracingDetailRepository.GetCountAsync(filterParams);

        var cappedTotal = Math.Min(realTotal, MAX_EXCEL_EXPORT_RECORDS);

        var items = await _stockTracingDetailRepository.GetListAsync(
            filterParams,
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount
        );

        return new PagedResultDto<StockTracingDetailDto>
        {
            TotalCount = cappedTotal,
            Items = ObjectMapper.Map<List<StockTracingDetail>, List<StockTracingDetailDto>>(items)
        };
    }

    public virtual async Task<StockTracingDetailDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<StockTracingDetail, StockTracingDetailDto>(await _stockTracingDetailRepository.GetAsync(id));
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        await _stockTracingDetailRepository.DeleteAsync(id);
    }


    public virtual async Task<StockTracingDetailDto> CreateAsync(StockTracingDetailCreateDto input)
    {
        var createParams = ObjectMapper.Map<StockTracingDetailCreateDto, StockTracingDetailCreateParams>(input);
        var stockTracingDetail = await _stockTracingDetailManager.CreateAsync(createParams);

        return ObjectMapper.Map<StockTracingDetail, StockTracingDetailDto>(stockTracingDetail);
    }


    public virtual async Task<StockTracingDetailDto> UpdateAsync(Guid id, StockTracingDetailUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<StockTracingDetailUpdateDto, StockTracingDetailUpdateParams>(input);
        var stockTracingDetail = await _stockTracingDetailManager.UpdateAsync(id, updateParams, input.ConcurrencyStamp);

        return ObjectMapper.Map<StockTracingDetail, StockTracingDetailDto>(stockTracingDetail);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(StockTracingDetailExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }
        var filterParams = ObjectMapper.Map<StockTracingDetailExcelDownloadDto, StockTracingDetailFilterParams>(input);
        filterParams.MaxResultCount = MAX_EXCEL_EXPORT_RECORDS;
        filterParams.SkipCount = MAX_SKIP_COUNT_RECORDS;
        var items = await _stockTracingDetailRepository.GetListAsync(filterParams, "Desc", 10000, 0);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<StockTracingDetail>, List<StockTracingDetailExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "StockTracingDetails.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new StockTracingDetailDownloadTokenCacheItem { Token = token },
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
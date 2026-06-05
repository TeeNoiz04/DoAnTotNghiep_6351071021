using Asp.Versioning;
using QuoteFlow.Seeders;
using QuoteFlow.StockTracingDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.StockTracingDetails;

[RemoteService]
[Area("app")]
[ControllerName("StockTracingDetail")]
[Route("api/app/stock-tracing-details")]

public class StockTracingDetailController : AbpController, IStockTracingDetailsAppService
{
    protected IStockTracingDetailsAppService _stockTracingDetailsAppService;
    protected IMemoryCache _memCache;
    private readonly List<StockTracingDetailDto> _stockTracingDetailData = new();

    public StockTracingDetailController(IStockTracingDetailsAppService stockTracingDetailsAppService, IMemoryCache memCache)
    {
        _stockTracingDetailsAppService = stockTracingDetailsAppService;
        _memCache = memCache;

        InitializeData();

    }

    private void InitializeData()
    {
        if (_memCache.TryGetValue("StockTracingDetailData", out List<StockTracingDetailDto>? cachedData))
        {
            _stockTracingDetailData.AddRange(cachedData ?? []);
            return;
        }

        var seeder = new StockTracingSeeder();
        var seed = 1234;
        var count = 1000;
        var countDetail = 60;
        var stockTracings = seeder.GenerateStockTracings(count, seed); // Generate 1000 stock tracings with a fixed seed
        _stockTracingDetailData.AddRange(seeder.GenerateStockTracingDetails(stockTracings, countDetail, seed));

        _memCache.Set("StockTracingDetailData", _stockTracingDetailData);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<StockTracingDetailDto>> GetListAsync(GetStockTracingDetailsInput input)
    {
        return _stockTracingDetailsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<StockTracingDetailDto> GetAsync(Guid id)
    {
        return _stockTracingDetailsAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<StockTracingDetailDto> CreateAsync(StockTracingDetailCreateDto input)
    {
        return _stockTracingDetailsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<StockTracingDetailDto> UpdateAsync(Guid id, StockTracingDetailUpdateDto input)
    {
        return _stockTracingDetailsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _stockTracingDetailsAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(StockTracingDetailExcelDownloadDto input)
    {
        return _stockTracingDetailsAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _stockTracingDetailsAppService.GetDownloadTokenAsync();
    }

}
using Asp.Versioning;
using QuoteFlow.Seeders;
using QuoteFlow.Shared.Excels;
using QuoteFlow.StockTracings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.StockTracings;

[RemoteService]
[Area("app")]
[ControllerName("StockTracing")]
[Route("api/app/stock-tracings")]

public class StockTracingController : AbpController, IStockTracingsAppService
{
    protected IStockTracingsAppService _stockTracingsAppService;
    private readonly List<StockTracingDto> stockTracings;

    public StockTracingController(IStockTracingsAppService stockTracingsAppService)
    {
        _stockTracingsAppService = stockTracingsAppService;

        var seeder = new StockTracingSeeder();
        var seed = 1234;
        var count = 1000;
        stockTracings = seeder.GenerateStockTracings(count, seed); // Generate 1000 stock tracings with a fixed seed
    }

    [HttpGet]
    public virtual Task<PagedResultDto<StockTracingDto>> GetListAsync(GetStockTracingsInput input)
    {
        return _stockTracingsAppService.GetListAsync(input);

        // Simulate with in-memory data
        //var pagedResult = new PagedResultDto<StockTracingDto>
        //{
        //    TotalCount = stockTracings.Count,
        //    Items = stockTracings.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()
        //};
        //return Task.FromResult(pagedResult);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<StockTracingDto> GetAsync(Guid id)
    {
        return _stockTracingsAppService.GetAsync(id);

        // Simulate with in-memory data
        //var stockTracing = stockTracings.FirstOrDefault(d => d.Id == id)
        //    ?? throw new EntityNotFoundException(typeof(StockTracingDto), id);
        //return Task.FromResult(stockTracing);
    }

    [HttpPost]
    public virtual Task<StockTracingDto> CreateAsync(StockTracingCreateDto input)
    {
        return _stockTracingsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<StockTracingDto> UpdateAsync(Guid id, StockTracingUpdateDto input)
    {
        return _stockTracingsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _stockTracingsAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(StockTracingExcelDownloadDto input)
    {
        return _stockTracingsAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _stockTracingsAppService.GetDownloadTokenAsync();
    }

    [HttpPost]
    [Route("validation/delivery")]
    public Task<ExcelValidationResult<StockTracingDeliveryImportDto>> ValidateAndStockTracingDeliveryAsync(IRemoteStreamContent file, DateTime fromDate, DateTime toDate, string? note)
    {
        return _stockTracingsAppService.ValidateAndStockTracingDeliveryAsync(file, fromDate, toDate, note);
    }

    [HttpPost]
    [Route("import/delivery")]
    public Task<StockTracingDto> ImportStockTracingDeliveryAsync(ExcelValidationResult<StockTracingDeliveryImportDto> data, DateTime fromDate, DateTime toDate, string? note)
    {
        return _stockTracingsAppService.ImportStockTracingDeliveryAsync(data, fromDate, toDate, note);
    }

    [HttpPost]
    [Route("validation/inventory")]
    public Task<ExcelValidationResult<StockTracingInventoryImportDto>> ValidateAndStockTracingInventoryAsync(IRemoteStreamContent file, DateTime dateEntered, string? note)
    {
        return _stockTracingsAppService.ValidateAndStockTracingInventoryAsync(file, dateEntered, note);
    }
    [HttpPost]
    [Route("validation/receipt")]
    public Task<ExcelValidationResult<StockTracingReceiptImportDto>> ValidateAndStockTracingReceiptAsync(IRemoteStreamContent file, DateTime fromDate, DateTime toDate, string? note)
    {
        return _stockTracingsAppService.ValidateAndStockTracingReceiptAsync(file, fromDate, toDate, note);
    }

    [HttpPost]
    [Route("import/invetory")]
    public Task<StockTracingDto> ImportStockTracingInvantoryAsync(ExcelValidationResult<StockTracingInventoryImportDto> data, DateTime? entered, string? note)
    {
        return _stockTracingsAppService.ImportStockTracingInvantoryAsync(data, entered, note);
    }

    [HttpPost]
    [Route("import/receipt")]
    public Task<StockTracingDto> ImportStockTracingReceiptAsync(ExcelValidationResult<StockTracingReceiptImportDto> data, DateTime fromDate, DateTime toDate, string? note)
    {
        return _stockTracingsAppService.ImportStockTracingReceiptAsync(data, fromDate, toDate, note);
    }

}
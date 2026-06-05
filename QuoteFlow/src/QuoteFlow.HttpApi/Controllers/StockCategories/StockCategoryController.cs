using Asp.Versioning;
using QuoteFlow.StockCategories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.StockCategories;

[RemoteService]
[Area("app")]
[ControllerName("StockCategory")]
[Route("api/app/stock-categories")]

public class StockCategoryController : AbpController, IStockCategoriesAppService
{
    protected IStockCategoriesAppService stockCategoriesAppService;


    public StockCategoryController(IStockCategoriesAppService stockCategoriesAppService)
    {
        this.stockCategoriesAppService = stockCategoriesAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<StockCategoryDto>> GetListAsync(GetStockCategoriesInput input)
    {
        return stockCategoriesAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<StockCategoryDto> GetAsync(Guid id)
    {
        return stockCategoriesAppService.GetAsync(id);

    }

    [HttpPost]
    public virtual Task<StockCategoryDto> CreateAsync(StockCategoryCreateDto input)
    {
        return stockCategoriesAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<StockCategoryDto> UpdateAsync(Guid id, StockCategoryUpdateDto input)
    {
        return stockCategoriesAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return stockCategoriesAppService.DeleteAsync(id);
    }



}
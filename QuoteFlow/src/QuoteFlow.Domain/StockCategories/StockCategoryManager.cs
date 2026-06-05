using QuoteFlow.StockCategories.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.StockCategories;

public class StockCategoryManager : DomainService
{
    protected IStockCategoryRepository _stockCategoryRepository;

    public StockCategoryManager(IStockCategoryRepository stockCategoryRepository)
    {
        _stockCategoryRepository = stockCategoryRepository;
    }

    public virtual async Task<StockCategory> CreateAsync(
    StockCategoryCreateParams createParams)
    {

        var stockCategory = new StockCategory(
         GuidGenerator.Create(),
         createParams
         );

        return await _stockCategoryRepository.InsertAsync(stockCategory);
    }

    public virtual async Task<StockCategory> UpdateAsync(
        Guid id,
        StockCategoryUpdateParams updateParams
    )
    {
        var stockCategory = await _stockCategoryRepository.GetAsync(id);

        stockCategory.StockCode = updateParams.StockCode;
        stockCategory.StockName = updateParams.StockName;
        stockCategory.SAPCode = updateParams.SAPCode;
        stockCategory.MainStock = updateParams.MainStock;
        stockCategory.FOC = updateParams.FOC;
        stockCategory.DamagedStock = updateParams.DamagedStock;
        stockCategory.SortOrder = updateParams.SortOrder;
        stockCategory.IsDeactive = updateParams.IsDeactive;
        stockCategory.Note = updateParams.Note;

        stockCategory.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _stockCategoryRepository.UpdateAsync(stockCategory);
    }

    public virtual async Task RemoveMainStock(List<Guid> ids)
    {
        if (ids == null || !ids.Any())
            return;

        var stocks = await _stockCategoryRepository.GetListAsync(x => ids.Contains(x.Id));

        foreach (var stock in stocks)
        {
            stock.MainStock = false;
        }

        await _stockCategoryRepository.UpdateManyAsync(stocks);
    }


}
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.Shared.Excels;
using QuoteFlow.StockCategories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace QuoteFlow.StockManagements.Excels.StockTransfer.Validators;
public class StockTransferValidator : BaseExcelValidator<MaterialStockUploadDetailImportTransferDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly IStockCategoryRepository _stockCategoryRepository;
    public StockTransferValidator(ExcelValidationConfig config, IExcelRowValidator<MaterialStockUploadDetailImportTransferDto> rowValidator, ILogger<BaseExcelValidator<MaterialStockUploadDetailImportTransferDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _stockCategoryRepository = _provider.GetRequiredService<IStockCategoryRepository>();


    }

    protected override async Task PostValidateAsync(ExcelValidationResult<MaterialStockUploadDetailImportTransferDto> result)
    {
        var stock = await _stockCategoryRepository.GetListAsync(x => x.IsDeactive == false);
        foreach (var row in result.ListData)
        {
            var item = row.RowData;

            item.StorageDesc_Id = stock.Find(x => x.StockCode == item.StorageDestination)?.Id ?? null;
            item.StorageSrc_Id = stock.Find(x => x.StockCode == item.Storage)?.Id ?? null;
        }
    }
}
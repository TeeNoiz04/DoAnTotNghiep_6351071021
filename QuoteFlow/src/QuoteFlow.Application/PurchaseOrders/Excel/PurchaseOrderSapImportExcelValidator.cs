using QuoteFlow.PurchaseOrdersSapImports.Excel;
using QuoteFlow.Shared.Excels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace QuoteFlow.PurchaseOrders.Excel;
public class PurchaseOrderSapImportExcelValidator : BaseExcelValidator<PurchaseOrdersSapImportsExcelDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly IPurchaseOrderRepository _purchaseOrderRepository;
    public PurchaseOrderSapImportExcelValidator(ExcelValidationConfig config, IExcelRowValidator<PurchaseOrdersSapImportsExcelDto> rowValidator, ILogger<BaseExcelValidator<PurchaseOrdersSapImportsExcelDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _purchaseOrderRepository = _provider.GetRequiredService<IPurchaseOrderRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<PurchaseOrdersSapImportsExcelDto> result)
    {



    }

}

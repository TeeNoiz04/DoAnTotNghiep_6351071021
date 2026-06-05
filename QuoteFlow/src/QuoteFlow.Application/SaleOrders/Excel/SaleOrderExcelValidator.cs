using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SaleOrders.Excel;
public class SaleOrderExcelValidator : BaseExcelValidator<SaleOrderExcelDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly ISaleOrderRepository _saleOrderRepository;
    public SaleOrderExcelValidator(ExcelValidationConfig config, IExcelRowValidator<SaleOrderExcelDto> rowValidator, ILogger<BaseExcelValidator<SaleOrderExcelDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _saleOrderRepository = _provider.GetRequiredService<ISaleOrderRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<SaleOrderExcelDto> result)
    {
        var saleOrder = await _saleOrderRepository.GetListAsync(new(),
            x => new SaleOrderSupportInfo(x.Id)
            {
                Status = x.StatusCode,
                SONo = x.SONo
            });

        foreach (var row in result.ListData)
        {
            var existSONo = saleOrder.Any(x => x.SONo == row.RowData.SONo && x.Status == QuoteFlowStatuses.InProgress);
            if (!existSONo)
            {
                row.Errors.Add($"SO No = {row.RowData.SONo} in status = In_Progress is not exist.");
            }
        }

    }
    private class SaleOrderSupportInfo : Entity<Guid>
    {
        public string Status { get; set; } = null!;
        public string SONo { get; set; } = null!;

        public SaleOrderSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}

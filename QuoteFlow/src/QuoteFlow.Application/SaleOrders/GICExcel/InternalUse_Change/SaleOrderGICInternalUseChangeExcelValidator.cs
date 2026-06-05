using QuoteFlow.Materials;
using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SaleOrders.GICExcel.InternalUse_Change;

public class SaleOrderGICInternalUseChangeExcelValidator : BaseExcelValidator<SaleOrderGICInternalUseChangeExcelDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly ISaleOrderRepository _saleOrderRepository;
    protected readonly ISaleOrderDetailRepository _saleOrderDetailRepository;
    protected readonly IMaterialRepository _materialRepository;
    public SaleOrderGICInternalUseChangeExcelValidator(ExcelValidationConfig config, IExcelRowValidator<SaleOrderGICInternalUseChangeExcelDto> rowValidator, ILogger<BaseExcelValidator<SaleOrderGICInternalUseChangeExcelDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _saleOrderRepository = _provider.GetRequiredService<ISaleOrderRepository>();
        _saleOrderDetailRepository = _provider.GetRequiredService<ISaleOrderDetailRepository>();
        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<SaleOrderGICInternalUseChangeExcelDto> result)
    {
        var saleOrder = await _saleOrderRepository.GetListAsync(new(),
            x => new SaleOrderSupportInfo(x.Id)
            {
                Status = x.StatusCode,
                SONo = x.SONo,

            });

        var materials = await _materialRepository.GetListAsync(
                new(),
                x => new MaterialSupportInfo(x.Id)
                {
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    GolfaCode = x.GolfaCode,
                    Model = x.Model
                });

        var saleOrderDetail = await _saleOrderDetailRepository.GetListAsync(new(),
            x => new SaleOrderDetailSupportInfo(x.Id)
            {
                MaterialCode = x.GolfaCode,

                SOId = x.SaleOrderId,

            });

        foreach (var row in result.ListData)
        {


            var exitsMaterial = materials.Any(x => (string.Equals(x.GolfaCode, row.RowData.MaterialCode, StringComparison.OrdinalIgnoreCase)) &&
                (string.Equals(x.Model, row.RowData.ModelName, StringComparison.OrdinalIgnoreCase)));
            if (!exitsMaterial)
            {
                row.Errors.Add($"Material code '{row.RowData.MaterialCode}' with model '{row.RowData.ModelName}' is not exist.");
            }

            var so = saleOrder.FirstOrDefault(x => x.SONo == row.RowData.SONo);
            if (so is null)
            {
                row.Errors.Add($"SO No '{row.RowData.SONo}' was not found.");
            }
            else
            {
                var exitsMaterialCodeOfSO = saleOrderDetail.Any(x => x.SOId == so.Id && x.MaterialCode == row.RowData.MaterialCode);
                if (!exitsMaterialCodeOfSO)
                {
                    row.Errors.Add($"Material code '{row.RowData.MaterialCode}' with model '{row.RowData.ModelName}' was not found in the SO No '{row.RowData.SONo}'.");

                }
                var existSONo = saleOrder.Any(x => x.SONo == row.RowData.SONo && x.Status == QuoteFlowStatuses.Closed);
                if (!existSONo)
                {
                    row.Errors.Add($"SO No '{row.RowData.SONo}' must be closed.");
                }

            }

            if (row.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
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

    private class SaleOrderDetailSupportInfo : Entity<Guid>
    {
        public string MaterialCode { get; set; } = null!;

        public Guid SOId { get; set; }



        public SaleOrderDetailSupportInfo(Guid id)
        {
            Id = id;
        }
    }
    private class MaterialSupportInfo : Entity<Guid>
    {
        public string ConcurrencyStamp { get; set; } = null!;
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public decimal Standard_Price { get; set; }

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}

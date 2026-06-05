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

namespace QuoteFlow.SaleOrders.GICExcel.Warranty;

public class SaleOrderGICWarrantyExcelValidator : BaseExcelValidator<SaleOrderGICWarrantyExcelDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly ISaleOrderRepository _saleOrderRepository;
    protected readonly ISaleOrderDetailRepository _saleOrderDetailRepository;
    protected readonly IMaterialRepository _materialRepository;
    public SaleOrderGICWarrantyExcelValidator(ExcelValidationConfig config, IExcelRowValidator<SaleOrderGICWarrantyExcelDto> rowValidator, ILogger<BaseExcelValidator<SaleOrderGICWarrantyExcelDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _saleOrderRepository = _provider.GetRequiredService<ISaleOrderRepository>();
        _saleOrderDetailRepository = _provider.GetRequiredService<ISaleOrderDetailRepository>();
        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<SaleOrderGICWarrantyExcelDto> result)
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

                SOId = x.SaleOrderId

            });

        foreach (var row in result.ListData)
        {


            var exitsMaterial = materials.Any(x =>
                string.Equals(x.GolfaCode, row.RowData.MaterialCode, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.Model, row.RowData.ModelName, StringComparison.OrdinalIgnoreCase));
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
                var existSONo = saleOrder.FirstOrDefault(x => x.SONo == row.RowData.SONo && (x.Status == QuoteFlowStatuses.InProgress || x.Status == QuoteFlowStatuses.Closed));
                if (existSONo is null)
                {
                    row.Errors.Add($"SO No '{row.RowData.SONo}' must be in progress or closed.");
                }
                else if (existSONo.Status != QuoteFlowStatuses.Closed)
                {
                    // Validate required fields
                    ValidateRequiredFieldsToClose(row);
                }

            }
            if (row.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }

        }

    }

    private static void ValidateRequiredFieldsToClose(ExcelRowResult<SaleOrderGICWarrantyExcelDto> row)
    {
        if (string.IsNullOrWhiteSpace(row.RowData.SAPSONo))
            row.Errors.Add($"SAP SO No is required.");

        if (string.IsNullOrWhiteSpace(row.RowData.DONo))
            row.Errors.Add($"DO No is required.");

        if (string.IsNullOrWhiteSpace(row.RowData.BillingNo))
            row.Errors.Add($"Billing No is required.");


        var cost = row.RowData.GICSAPLandingCost;
        var quantity = row.RowData.SOQty;
        var amount = row.RowData.GICAmountSAPLandingCost;
        var rowIndex = row.RowIndex;

        // Tự thực hiện logic validation TÍCH (×)
        var tolerance = 0.01m; // Sử dụng decimal cho tolerance
        if (cost.HasValue && quantity.HasValue && amount.HasValue)
        {
            var expected = cost.Value * quantity.Value;
            if (Math.Abs(expected - amount.Value) > tolerance)
            {
                row.Errors.Add(
                    $"Amount in SAP Landing Cost(AD) must equal SAP Landing Cost(AC) × SO Qty(S). " +
                    $"Expected: {expected.ToString("N0")}, but got: {amount.Value.ToString("N0")}."
                );
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

using QuoteFlow.GICs;
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

namespace QuoteFlow.SaleOrders.GICExcel.InternalUse;

public class SaleOrderGICInternalUseExcelValidator : BaseExcelValidator<SaleOrderGICInternalUseExcelDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly ISaleOrderRepository _saleOrderRepository;
    protected readonly ISaleOrderDetailRepository _saleOrderDetailRepository;
    protected readonly IMaterialRepository _materialRepository;
    public SaleOrderGICInternalUseExcelValidator(ExcelValidationConfig config, IExcelRowValidator<SaleOrderGICInternalUseExcelDto> rowValidator, ILogger<BaseExcelValidator<SaleOrderGICInternalUseExcelDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _saleOrderRepository = _provider.GetRequiredService<ISaleOrderRepository>();
        _saleOrderDetailRepository = _provider.GetRequiredService<ISaleOrderDetailRepository>();
        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<SaleOrderGICInternalUseExcelDto> result)
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
            var exitsMaterial = materials.Any(x => x.GolfaCode == row.RowData.MaterialCode && x.Model == row.RowData.ModelName);
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
                var soWithValidStatus = saleOrder.FirstOrDefault(x => x.SONo == row.RowData.SONo && (x.Status == QuoteFlowStatuses.InProgress));
                if (soWithValidStatus is null)
                {
                    row.Errors.Add($"SO No '{row.RowData.SONo}' must be in progress.");
                }

                //check fields based on status & process
                else
                {

                    if (so.Status == QuoteFlowStatuses.InProgress)
                    {
                        // process : Reservation

                        if (row.RowData.GICProcess == GICProcessCodes.ReservationNo)
                        {
                            if (string.IsNullOrWhiteSpace(row.RowData.GICReservationNo))
                                row.Errors.Add($"AssetClass (ReservationNo) is required.");
                        }
                        // process : Asset/Tool
                        else if (row.RowData.GICProcess == GICProcessCodes.Asset || row.RowData.GICProcess == GICProcessCodes.Tool)
                        {
                            if (string.IsNullOrWhiteSpace(row.RowData.GICAssetClass))
                                row.Errors.Add($"AssetClass (AA) is required for Asset/Tool process.");

                            if (string.IsNullOrWhiteSpace(row.RowData.GICMainAssetCode))
                                row.Errors.Add($"MainAssetCode (AB) is required for Asset/Tool process.");

                            if (string.IsNullOrWhiteSpace(row.RowData.GICSubAssetCode))
                                row.Errors.Add($"SubAssetCode (AC) is required for Asset/Tool process.");

                            if (string.IsNullOrWhiteSpace(row.RowData.GICAssetName))
                                row.Errors.Add($"AssetName (AD) is required for Asset/Tool process.");
                        }



                        if (soWithValidStatus.Status == QuoteFlowStatuses.Closed)
                        {
                            // only read PRNo to update, don't validate required
                            // PRNo will be used to update GIC later
                        }

                    }
                }













                //var exitsMaterial = materials.Any(x => x.GolfaCode == row.RowData.MaterialCode && x.Model == row.RowData.ModelName);
                //if (!exitsMaterial)
                //{
                //    row.Errors.Add($"Material code '{row.RowData.MaterialCode}' with model '{row.RowData.ModelName}' is not exist.");
                //}

                //var so = saleOrder.FirstOrDefault(x => x.SONo == row.RowData.SONo);
                //if (so is null)
                //{
                //    row.Errors.Add($"SO No '{row.RowData.SONo}' was not found.");
                //}
                //else
                //{
                //    var exitsMaterialCodeOfSO = saleOrderDetail.Any(x => x.SOId == so.Id && x.MaterialCode == row.RowData.MaterialCode);
                //    if (!exitsMaterialCodeOfSO)
                //    {
                //        row.Errors.Add($"Material code '{row.RowData.MaterialCode}' with model '{row.RowData.ModelName}' was not found in the SO No '{row.RowData.SONo}'.");

                //    }
                //    var existSONo = saleOrder.Any(x => x.SONo == row.RowData.SONo && (x.Status == QuoteFlowStatuses.InProgress || x.Status == QuoteFlowStatuses.Closed));
                //    if (!existSONo)
                //    {
                //        row.Errors.Add($"SO No '{row.RowData.SONo}' must be in progress or closed.");
                //    }
                //}
                if (row.HasErrors)
                {
                    ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
                }

            }
            if (result.ListData.Select(x => x.RowData.GICProcess).Distinct().Count() > 1)
            {

                result.Errors.Add("SO GIC must have the same GIC Process");
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

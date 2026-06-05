using QuoteFlow.Materials;
using QuoteFlow.PurchaseOrderDetails;
using QuoteFlow.PurchaseOrders;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Cargos.Excels;
public class CargoValidator : BaseExcelValidator<CargoImportDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
    protected readonly IPurchaseOrderRepository _purchaseRepository;
    protected readonly IMaterialRepository _materialRepository;

    public CargoValidator(ExcelValidationConfig config, IExcelRowValidator<CargoImportDto> rowValidator, ILogger<BaseExcelValidator<CargoImportDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;

        _purchaseOrderDetailRepository = _provider.GetRequiredService<IPurchaseOrderDetailRepository>();
        _purchaseRepository = _provider.GetRequiredService<IPurchaseOrderRepository>();
        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<CargoImportDto> result, ExcelImportContext? context = null)
    {
        var PODetails = await _purchaseOrderDetailRepository.GetListAsync(
           new(),
           x => new PurchaseOrderDetailSupportInfo(x.Id)
           {
               PODetailCode = x.PODetailCode,
               PurchaseOrderId = x.PurchaseOrderId,
               Qty = x.Qty,
               MaterialCode = x.GolfaCode,
               Model = x.Model,
               StatusCode = x.StatusCode,
               IsDeleted = x.IsDeleted
           });

        //var materials = await _materialRepository.GetListAsync(
        //new(),
        //x => new MaterialSupportInfo(x.Id)
        //{
        //    MaterialType = x.MaterialType,
        //    GolfaCode = x.GolfaCode,
        //    Model = x.Model,
        //    SupplierCode = x.SupplierCode
        //});

        var pOs = await _purchaseRepository.GetListAsync(
            new(),
            x => new PurchaseOrderSupportInfo(x.Id)
            {
                PONo = x.PONo,
                SupplierCode = x.SupplierCode,
                MaterialType = x.MaterialType,
            }
        );

        var materialType = context?.GetData<string?>(ExcelImportContextKeys.Cargo.MaterialType)
            ?? throw new ArgumentNullException(nameof(context), "Material type is required");
        var supplierCode = context?.GetData<string?>(ExcelImportContextKeys.Cargo.SupplierCode)
            ?? throw new ArgumentNullException(nameof(context), "Supplier is required");

        // Chuyển đổi sang Dictionary để tối ưu tìm kiếm
        var poDictionary = pOs.ToDictionary(
            po => po.PONo ?? string.Empty,
            po => po,
            StringComparer.OrdinalIgnoreCase
        );

        // Dictionary cho PODetail với composite key
        var poDetailDictionary = PODetails
            .Where(x => x.PODetailCode != null && x.MaterialCode != null && x.Model != null && x.IsDeleted != true)
            .GroupBy(x => new PODetailKey(
                x.PODetailCode!,
                x.PurchaseOrderId ?? Guid.Empty,
                x.MaterialCode!,
                x.Model!
            ))
            .ToDictionary(
                g => g.Key,
                g => g.First()
            );

        var duplicatedCombos = result.ListData
            .Where(x =>
                !string.IsNullOrWhiteSpace(x.RowData.PODetailCode) &&
                !string.IsNullOrWhiteSpace(x.RowData.GolfaCode) &&
                !string.IsNullOrWhiteSpace(x.RowData.PORef))
            .GroupBy(x => new
            {
                POSEQ = x.RowData.PODetailCode!.Trim(),
                MaterialCode = x.RowData.GolfaCode!.Trim(),
                PORef = x.RowData.PORef
            })
            .Where(g => g.Count() > 1);

        foreach (var row in result.ListData)
        {
            var PODetailCode = row.RowData.PODetailCode;
            var poRef = row.RowData.PORef;

            // Sử dụng Dictionary thay vì FirstOrDefault
            if (!poDictionary.TryGetValue(poRef ?? string.Empty, out var po))
            {
                row.Errors.Add($"Purchase Order with PONo = {poRef} not found");
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
                continue;
            }

            if (po.MaterialType != materialType)
            {
                row.Errors.Add($"Selected material type ({materialType}) does not match with Purchase Order material type ({po.MaterialType})");
            }

            if (po.SupplierCode != supplierCode)
            {
                row.Errors.Add($"Selected supplier ({supplierCode}) does not match with Purchase Order supplier ({po.SupplierCode})");
            }

            if (!string.IsNullOrWhiteSpace(PODetailCode))
            {
                var key = new PODetailKey(
                    PODetailCode,
                    po.Id,
                    row.RowData.GolfaCode ?? string.Empty,
                    row.RowData.Model ?? string.Empty
                );

                // Sử dụng Dictionary thay vì FirstOrDefault
                if (poDetailDictionary.TryGetValue(key, out var PODetail))
                {
                    if (string.Equals(PODetail.StatusCode, QuoteFlowStatuses.Closed, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    row.RowData.PODetailId = PODetail.Id;

                    if (!int.TryParse(row.RowData.OrderQty, out var orderQty))
                    {
                        continue;
                    }

                    var poDetailQty = PODetail.Qty;
                    if (poDetailQty < orderQty)
                    {
                        row.Errors.Add($"Cargo order quantity ({orderQty:N0}) cannot exceed purchase order detail quantity ({poDetailQty:N0})");
                    }
                }
                else
                {
                    row.Errors.Add($"Cannot find Purchase Order Detail with Code = {PODetailCode}; Purchase Order No = {poRef}, Material Code = {row.RowData.GolfaCode}, Model = {row.RowData.Model}");
                }
            }

            if (row.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
        }

        foreach (var group in duplicatedCombos)
        {
            var key = group.Key;

            var baseRow = group.First();
            var baseRowNo = result.ListData.IndexOf(baseRow) + 1;

            foreach (var row in group.Skip(1))
            {
                var rowNo = result.ListData.IndexOf(row) + 1;
                row.Errors.Add(
                    $"Row {rowNo}: Combination of PO NO = {key.PORef}, PO SEQ='{key.POSEQ}', Material Code='{key.MaterialCode}' is duplicated with line {baseRowNo}."
                );
                if (row.HasErrors)
                {
                    ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
                }
            }
        }
    }

    // Struct để làm composite key cho Dictionary
    private struct PODetailKey : IEquatable<PODetailKey>
    {
        public string PODetailCode { get; }
        public Guid PurchaseOrderId { get; }
        public string MaterialCode { get; }
        public string Model { get; }

        public PODetailKey(string poDetailCode, Guid purchaseOrderId, string materialCode, string model)
        {
            PODetailCode = poDetailCode;
            PurchaseOrderId = purchaseOrderId;
            MaterialCode = materialCode;
            Model = model;
        }

        public bool Equals(PODetailKey other)
        {
            return string.Equals(PODetailCode, other.PODetailCode, StringComparison.OrdinalIgnoreCase) &&
                   PurchaseOrderId.Equals(other.PurchaseOrderId) &&
                   string.Equals(MaterialCode, other.MaterialCode, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Model, other.Model, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            return obj is PODetailKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                PODetailCode.ToLowerInvariant(),
                PurchaseOrderId,
                MaterialCode.ToLowerInvariant(),
                Model.ToLowerInvariant()
            );
        }
    }

    private class PurchaseOrderDetailSupportInfo : Entity<Guid>
    {
        public string? PODetailCode { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public int? Qty { get; set; }
        public string? MaterialCode { get; set; }
        public string? Model { get; set; }
        public string? StatusCode { get; set; }
        public bool? IsDeleted { get; set; }

        public PurchaseOrderDetailSupportInfo(Guid id)
        {
            Id = id;
        }
    }

    private class PurchaseOrderSupportInfo : Entity<Guid>
    {
        public string? PONo { get; set; }
        public string? SupplierCode { get; set; }
        public string? MaterialType { get; set; }

        public PurchaseOrderSupportInfo(Guid id)
        {
            Id = id;
        }
    }

    private class MaterialSupportInfo : Entity<Guid>
    {
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string? MaterialType { get; set; }
        public string? SupplierCode { get; set; }

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}

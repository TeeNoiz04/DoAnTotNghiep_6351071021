using QuoteFlow.Materials;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.Shared.Excels;
using QuoteFlow.StockCategories;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.StockManagements.Excels.StockTransfer.Validators;

public class StockTransferRowValidation : IExcelRowValidator<MaterialStockUploadDetailImportTransferDto>
{
    protected readonly IMaterialRepository _materialRepository;
    protected readonly IStockCategoryRepository _stockCategoryRepository;

    public StockTransferRowValidation(IMaterialRepository materialRepository, IStockCategoryRepository stockCategoryRepository)
    {
        _materialRepository = materialRepository;
        _stockCategoryRepository = stockCategoryRepository;
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var materialCode = ExcelParser.GetValue<string?>(rowData, "A");
        var model = ExcelParser.GetValue<string?>(rowData, "B");
        var storage = ExcelParser.GetValue<string?>(rowData, "C");
        var storageDestination = ExcelParser.GetValue<string?>(rowData, "D");
        var qty = ExcelParser.GetValue<decimal?>(rowData, "E");
        var remark = ExcelParser.GetValue<string?>(rowData, "F");

        if (string.IsNullOrWhiteSpace(materialCode))
            result.AddError("Material Code is required.");
        if (string.IsNullOrWhiteSpace(storage))
            result.AddError("Source Storage is required.");
        if (string.IsNullOrWhiteSpace(storageDestination))
            result.AddError("Destination Storage is required.");
        if (qty == null || qty <= 0)
            result.AddError("Qty must be greater than 0.");
        if (!string.IsNullOrWhiteSpace(materialCode) && !string.IsNullOrWhiteSpace(model))
        {
            var materialId = MaterialAsync(materialCode, model);
            if (materialId == Guid.Empty)
            {
                result.AddError($"Material Code = '{materialCode}' & Model Name = '{model}' is not existed");
            }
        }
        if (!string.IsNullOrWhiteSpace(storage))
        {
            var storageSource = _stockCategoryRepository.FirstOrDefaultAsync(x => x.StockCode == storage);

            if (storageSource.Result == null)
            {
                result.AddError($"Source Storage = '{storage}' is not existed. Please entered stock code");
            }

        }
        if (!string.IsNullOrWhiteSpace(storageDestination))
        {
            var storageDesc = _stockCategoryRepository.FirstOrDefaultAsync(x => x.StockCode == storageDestination);
            if (storageDesc.Result == null)
            {
                result.AddError($"Destination Storage = '{storageDestination}' is not existed. Please entered stock code");
            }
        }
        return result;
    }
    public Guid MaterialAsync(string code, string model)
    {
        var material = _materialRepository.GetListAsync(x => x.GolfaCode == code && x.Model == model);
        if (material.Result.Count == 0)
        {
            return Guid.Empty;
        }
        return material.Result[0].Id;
    }
    public MaterialStockUploadDetailImportTransferDto ParseRow(IDictionary<string, object> rowData)
    {
        return new MaterialStockUploadDetailImportTransferDto
        {
            RequestId = ExcelParser.InValidGuidSafe(rowData, "A", MaterialAsync(ExcelParser.GetValue<string?>(rowData, "A") ?? string.Empty, ExcelParser.GetValue<string?>(rowData, "B") ?? string.Empty), true),
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim() ?? string.Empty,
            Model = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            Storage = ExcelParser.GetValue<string?>(rowData, "C")?.Trim(),
            StorageDestination = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            Qty = ExcelParser.GetValue<decimal?>(rowData, "E"),
            Remark = ExcelParser.GetValue<string?>(rowData, "F")?.Trim()
        };
    }
}

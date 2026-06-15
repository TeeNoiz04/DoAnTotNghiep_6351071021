using QuoteFlow.Shared.Excels;
using QuoteFlow.Suppliers;
using QuoteFlow.SystemCategories;
using QuoteFlow.SystemCategories.ParameterObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SupplierBUs.Excels.Validations;
public class SupplierBUValidator : BaseExcelValidator<SupplierBUImportDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly ISupplierRepository _supplierRepository;
    protected readonly ISupplierBURepository _supplierBURepository;
    protected readonly ISystemCategoryRepository _systemCategoryRepository;
    public SupplierBUValidator(ExcelValidationConfig config, IExcelRowValidator<SupplierBUImportDto> rowValidator, ILogger<BaseExcelValidator<SupplierBUImportDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _supplierRepository = _provider.GetRequiredService<ISupplierRepository>();
        _supplierBURepository = _provider.GetRequiredService<ISupplierBURepository>();
        _systemCategoryRepository = _provider.GetRequiredService<ISystemCategoryRepository>();
    }
    protected override async Task PostValidateAsync(ExcelValidationResult<SupplierBUImportDto> result, ExcelImportContext? context = null)
    {
        var suppliers = await _supplierRepository.GetListAsync();
        var supplierBUs = await _supplierBURepository.GetListAsync();
        var filter = new SystemCategoryFilterParams();
        filter.CategoryType = SystemCategories.CategoryTypes.Currency;
        var currencyList = await _systemCategoryRepository.GetListAsync(
            filter,
            x => new SystemCategorySupportInfo(x.Id)
            {
                Code = x.Code,
                Description = x.Description,
                Value = x.Value
            });

        foreach (var supplierBU in result.ListData)
        {
            //if (supplierBU.RowData.SupplierShortenName is not null && supplierBU.RowData.SupplierCode is not null)
            //{
            var existSupplier = suppliers.FirstOrDefault(x => x.SupplierCode == supplierBU.RowData.SupplierCode && x.SAPCode == supplierBU.RowData.SAPCode);
            var currency = currencyList.FirstOrDefault(x => x.Code == supplierBU.RowData.Currency);
            if (existSupplier is not null)
            {
                supplierBU.RowData.SupplierID = existSupplier.Id;
            }
            else
            {
                supplierBU.Errors.Add($"Cannot find Supplier Code = {supplierBU.RowData.SupplierCode} and SAP Code = {supplierBU.RowData.SAPCode}");
            }
            //}
            if (supplierBU.RowData.SupplierBU is not null)
            {
                var existSupplierBU = supplierBUs.FirstOrDefault(x => x.SupplierBUCode == supplierBU.RowData.SupplierBU);
                if (existSupplierBU is not null)
                {
                    supplierBU.RowData.IsUpdate = true;
                    supplierBU.RowData.IdUpdate = existSupplierBU.Id;
                    supplierBU.RowData.ConcurrencyStamp = existSupplierBU.ConcurrencyStamp;
                }
                else
                {
                    //if (string.IsNullOrWhiteSpace(supplierBU.RowData.SupplierShortenName))
                    //{
                    //    supplierBU.Errors.Add($"Row {supplierBU.RowIndex} add new: Supplier Shorten Name (N) is required.");
                    //}
                    //if (string.IsNullOrWhiteSpace(supplierBU.RowData.SupplierCode))
                    //{
                    //    supplierBU.Errors.Add($"Row {supplierBU.RowIndex} add new: Supplier Code (M) is required.");
                    //}
                    supplierBU.RowData.IsUpdate = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(supplierBU.RowData.Currency) && currency is null)
            {
                supplierBU.Errors.Add($"Cannot find Currency = {supplierBU.RowData.Currency}");
            }
            if (supplierBU.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, supplierBU.RowIndex, supplierBU.Errors);
            }
        }

    }

    private class SystemCategorySupportInfo : Entity<Guid>
    {
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Value { get; set; } = null;

        public SystemCategorySupportInfo(Guid id)
        {
            Id = id;
        }
    }
}

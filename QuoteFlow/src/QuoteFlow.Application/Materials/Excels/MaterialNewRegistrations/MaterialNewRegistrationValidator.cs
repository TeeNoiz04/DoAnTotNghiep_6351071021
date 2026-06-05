using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using QuoteFlow.SupplierBUs;
using QuoteFlow.SystemCategories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.Excels.MaterialNewRegistrations;

public class MaterialNewRegistrationValidator : BaseExcelValidator<MaterialNewRegistrationImportDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly ISystemCategoryRepository _systemCategoryRepository;
    protected readonly IMaterialRepository _materialRepository;
    protected readonly IMaterialGroupRepository _materialGroupRepository;
    protected readonly IMaterialApprovalRequestDetailRepository _materialDetailRepository;
    protected readonly IMaterialApprovalRequestRepository _materialApprovalRequestRepository;
    protected readonly ISupplierBURepository _supplierBURepository;
    public MaterialNewRegistrationValidator(ExcelValidationConfig config, IExcelRowValidator<MaterialNewRegistrationImportDto> rowValidator, ILogger<BaseExcelValidator<MaterialNewRegistrationImportDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _systemCategoryRepository = _provider.GetRequiredService<ISystemCategoryRepository>();
        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
        _materialGroupRepository = _provider.GetRequiredService<IMaterialGroupRepository>();
        _materialDetailRepository = _provider.GetRequiredService<IMaterialApprovalRequestDetailRepository>();
        _materialApprovalRequestRepository = _provider.GetRequiredService<IMaterialApprovalRequestRepository>();
        _supplierBURepository = _provider.GetRequiredService<ISupplierBURepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<MaterialNewRegistrationImportDto> result)
    {
        if (result.ListData.Any(x => x.HasErrors))
        {
            return;
        }

        var materialDetails = await _materialDetailRepository.GetListAsync(
             new(),
            x => new MaterialDetailSupportInfo(x.Id)
            {
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                MaterialType = x.MaterialType,
                MaterialId = x.MaterialApprovalId,
                Standard_Price = x.Standard_Price
            });
        var materialApprovalRequests = await _materialApprovalRequestRepository.GetListAsync(
            new(),
            x => new MaterialApprovalRequestSupportInfo(x.Id)
            {
                Status = x.Status
            });
        var supplierBUs = await _supplierBURepository.GetListAsync(
            new(),
            x => new SupplierBUSupportInfo(x.Id)
            {
                SupplierBUCode = x.SupplierBUCode,
                SupplierCode = x.SupplierCode,
                Currency = x.Currency,
                MaterialType = x.MaterialType
            });
        var materials = await _materialRepository.GetListAsync(
            new(),
            x => new MaterialSupportInfo(x.Id)
            {
                Standard_Price = x.Standard_Price,
                ConcurrencyStamp = x.ConcurrencyStamp,
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                MaterialType = x.MaterialType
            });
        var systemCategories = await _systemCategoryRepository.GetListAsync(
            new(),
            x => new SystemCategorySupportInfo(x.Id)
            {
                Code = x.Code,
                Description = x.Description,
                Value = x.Value
            });
        var materialGroups = await _materialGroupRepository.GetListAsync(
            new(),
            x => new MaterialGroupsSupportInfo(x.Id)
            {
                Code = x.Code,
                MaterialType = x.MaterialType
            });

        var duplicateKeys = result.ListData
            .GroupBy(x => new { x.RowData.MaterialCode, x.RowData.ModelName })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();
        foreach (var row in result.ListData)
        {

            var code = row.RowData.MaterialCode;
            var model = row.RowData.ModelName;
            var materialType = row.RowData.MaterialType;
            var codeCurrency = row.RowData.InputCurrency;
            var codeMaterialGroup = row.RowData.MaterialGroup;

            var supplierBUCode = row.RowData.SupplierBU;
            var supplierCode = row.RowData.Supplier;
            //decimal standard = row.RowData.StandardPrice;

            var catergory = systemCategories.FirstOrDefault(x => x.Code.Equals(codeCurrency, StringComparison.OrdinalIgnoreCase));
            var material = materials.Any(x => x.GolfaCode.Equals(code, StringComparison.OrdinalIgnoreCase));

            var materialDetail = materialDetails.FirstOrDefault(x => x.GolfaCode.Equals(code, StringComparison.OrdinalIgnoreCase) && x.Model.Equals(model, StringComparison.OrdinalIgnoreCase));

            var supplierBU = supplierBUs.FirstOrDefault(x => x.SupplierBUCode.Equals(supplierBUCode, StringComparison.OrdinalIgnoreCase) && string.Equals(x.SupplierCode, supplierCode, StringComparison.OrdinalIgnoreCase));

            if (duplicateKeys.Contains(new { MaterialCode = code, ModelName = model }))
            {
                row.Errors.Add($"Duplicate Material Code = {code} & Model Name = {model} found in Excel.");
            }
            if (material)
            {
                row.Errors.Add($"Material Code = {code} & Model Name = {model} is exits");
            }
            //Caterogry currency
            if (catergory is null)
            {
                row.Errors.Add($"Cannot find Code Currency = {codeCurrency}");
            }
            else
            {
                row.RowData.InputCurrencyId = catergory.Id;
            }
            //Material Group - checked required
            if (!string.IsNullOrWhiteSpace(codeMaterialGroup))
            {
                var materialGroup = materialGroups.FirstOrDefault(x => x.Code == codeMaterialGroup);
                if (materialGroup is null)
                {
                    row.Errors.Add($"Cannot find Material Group = {codeMaterialGroup}");
                }
                else if (!materialType.Equals(materialGroup.MaterialType))
                {
                    row.Errors.Add($"Material Type of Material Group = {materialGroup.MaterialType} does not match Material Type = {materialType}");

                }
            }
            //Material Detail
            if (materialDetail is not null)
            {
                var approvalRequest = materialApprovalRequests.Any(x => x.Id == materialDetail.MaterialId && (x.Status == QuoteFlowStatuses.InProgress));
                if (approvalRequest)
                {
                    row.Errors.Add($"Material Detail with Golfa Code = {code} & Model Name = {model} is already submitted");
                }
            }
            //Supplier BU
            if (supplierBU is null)
            {
                row.Errors.Add($"Cannot find Supplier BU Code = {supplierBUCode} & Supplier Code = {supplierCode}");
            }
            else
            {
                if (!codeCurrency.Equals(supplierBU.Currency)) //currency of new.Material must same with Supplier BU
                {
                    row.Errors.Add($"Currency of Supplier BU = {supplierBU.Currency} does not match Currency = {codeCurrency}");
                }

                if (!supplierBU.MaterialType.Equals(materialType)) //material type of supplier BU must same with material type
                {
                    row.Errors.Add($"Material Type of Supplier BU = {supplierBU.MaterialType} does not match Material Type = {materialType}");
                }

                row.RowData.SupplierBUId = supplierBU.Id;
            }

            if (row.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
        }
    }


    private class MaterialSupportInfo : Entity<Guid>
    {
        public string ConcurrencyStamp { get; set; } = null!;
        public virtual decimal? Standard_Price { get; set; }
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string? MaterialType { get; set; }

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
    private class MaterialDetailSupportInfo : Entity<Guid>
    {
        //public string ConcurrencyStamp { get; set; } = null!;
        public virtual decimal? Standard_Price { get; set; }
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string? MaterialType { get; set; }
        public Guid? MaterialId { get; set; } = null;

        public MaterialDetailSupportInfo(Guid id)
        {
            Id = id;
        }
    }
    private class MaterialApprovalRequestSupportInfo : Entity<Guid>
    {
        public string? Status { get; set; }
        public MaterialApprovalRequestSupportInfo(Guid id)
        {
            Id = id;
        }
    }
    private class SupplierBUSupportInfo : Entity<Guid>
    {
        public string SupplierBUCode { get; set; } = null!;
        public string? SupplierCode { get; set; }
        public string? Currency { get; set; }
        public string? MaterialType { get; set; }
        public SupplierBUSupportInfo(Guid id)
        {
            Id = id;
        }
    }
    private class MaterialGroupsSupportInfo : Entity<Guid>
    {
        public string Code { get; set; } = null!;
        public string? MaterialType { get; set; }

        public MaterialGroupsSupportInfo(Guid id)
        {
            Id = id;
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

using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.Materials.Excels.MaterialUpdateWithoutPrices;

internal class MaterialUpdateWhitoutPriceExcelDtoConverter : ExcelDtoConverter<MaterialUpdateWithoutPriceImportDto, ExcelMaterialUpdateWithoutPrriceParams>
{
    public MaterialUpdateWhitoutPriceExcelDtoConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }
    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<MaterialUpdateWithoutPriceImportDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<ExcelMaterialUpdateWithoutPrriceParams?> MapToCreateParamsAsync(
        MaterialUpdateWithoutPriceImportDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<ExcelMaterialUpdateWithoutPrriceParams?>(createParams);
    }

    private ExcelMaterialUpdateWithoutPrriceParams ToCreateParams(MaterialUpdateWithoutPriceImportDto input, ExcelImportContext context)
    {
        return new ExcelMaterialUpdateWithoutPrriceParams
        {
            Id = input.Id,
            GolfaCode = input.MaterialCode!,
            Model = input.ModelName!,
            RegistrationDate = input.RegistrationDate,
            ValidFrom = input.ValidFrom,
            ValidTo = input.ValidTo,
            Spec1 = input.Spec1,
            Spec2 = input.Spec2,
            Spec3 = input.Spec3,
            Spec4 = input.Spec4,
            Description_EN = input.DescriptionEN,
            Description_VN = input.DescriptionVN,
            SupplierCode = input.Supplier,                 // L
            SupplierBUCode = input.SupplierBU,             // M
            SupplierBUId = input.SupplierBUId,
            Factory_Text = input.Factory,                  // N
            MaterialType = input.MaterialType,             // O
            Unit = input.Unit,                             // P
            Material_Group = input.MaterialGroup,        // MaterialGroupId
            SAPMatGroup = input.SAPGroup,                  // R
            ProductHierarchyDescription = input.ProductHierarchy_Description, // S
            CountryOfOrigin = input.CountryOfOrigin,       // T
            ReferenceLeadTime = input.ReferenceLeadTime,   // U
            WarrantyTime = input.WarrantyTime,             // V
            InventoryCategory = input.InventoryCategory,   // W
            CargoNote = input.CargoNote,                 // x
            Weight = input.Weight,                         // Y
            Size = input.Size,                             // Z
            QRCode = input.QRCode,                       // AA
            Maxlot = input.MaxLot,                         // AB
            StockWarning = input.StockWarning,             // AC
            StockQty = input.StockQty,                     // AD
            HS_Code = input.HSCode,                        // AE
            ConcurrencyStamp = input.ConcurrencyStamp!,
        };
    }
}

using QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.Materials.Excels.MaterialNewRegistrations;
public class MaterialNewRegistrationExcelDtoConverter : ExcelDtoConverter<MaterialNewRegistrationImportDto, MaterialApprovalRequestDetailCreateParams>
{
    public MaterialNewRegistrationExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator)
        : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => new[]
    {
        ExcelImportContextKeys.ParentEntityId
    };

    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<MaterialNewRegistrationImportDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<MaterialApprovalRequestDetailCreateParams?> MapToCreateParamsAsync(
        MaterialNewRegistrationImportDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<MaterialApprovalRequestDetailCreateParams?>(createParams);
    }

    private MaterialApprovalRequestDetailCreateParams ToCreateParams(MaterialNewRegistrationImportDto importDto, ExcelImportContext context)
    {
        return new MaterialApprovalRequestDetailCreateParams
        {
            MaterialApprovalId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
            GolfaCode = importDto.MaterialCode,
            Model = importDto.ModelName,
            RegistrationDate = importDto.RegistrationDate,
            ValidFrom = importDto.ValidFrom,
            ValidTo = importDto.ValidTo,
            SAP_Code = importDto.SAPCode,
            Spec1 = importDto.Spec1,
            Spec2 = importDto.Spec2,
            Spec3 = importDto.Spec3,
            Spec4 = importDto.Spec4,
            Description_EN = importDto.DescriptionEN,
            Description_VN = importDto.DescriptionVN,
            MaterialType = importDto.MaterialType,
            Unit = importDto.Unit,
            Material_SEC_Classification = importDto.MaterialSECClassification,
            Material_Group = importDto.MaterialGroup,
            SAPMatGroup = importDto.SAPGroup,
            Product_Hierarchy = importDto.ProductHierarchy,
            ProductHierarchyDescription = importDto.ProductHierarchy_Description,
            CountryOfOrigin = importDto.CountryOfOrigin,
            ReferenceLeadTime = importDto.ReferenceLeadTime,
            WarrantyTime = importDto.WarrantyTime,
            InventoryCategory = importDto.InventoryCategory,
            Maxlot = importDto.MaxLot,
            StockWarning = importDto.StockWarning,
            VAT = importDto.VAT,
            HS_Code = importDto.HSCode,
            SupplierBUId = importDto.SupplierBUId,
            SupplierBUCode = importDto.SupplierBU,
            //Factory = importDto.FactoryId,
            Factory_Text = importDto.Factory,
            Input_Price = importDto.InputPrice,
            InputCurrency = importDto.InputCurrency,
            INCOTERMS = importDto.Incoterms,
            EPA = importDto.EPA,
            ImportDuty = importDto.ImportDuty,
            AppliedExchangeRate = importDto.ExchangeRate,
            LandedCost = importDto.LandedCost,
            MaxSalesOfferPrice = importDto.MaxSaleOfferPrice,
            MaxMangerOfferPrice = importDto.MaxManagerOfferPrice,
            Standard_Price = importDto.StandardPrice,
            SellingPrice1 = importDto.SellingPrice1,
            SellingPrice2 = importDto.SellingPrice2,
            SellingPrice3 = importDto.SellingPrice3,
            SellingPrice4 = importDto.SellingPrice4,
            SellingPrice5 = importDto.SellingPrice5,
            SupplierCode = importDto.Supplier,
            CargoNote = importDto.CargoNote,
            Weight = importDto.Weight,
            Size = importDto.Size,
            QRCode = importDto.QRCode,
        };
    }
}

using QuoteFlow.Cargos.CargoDatas.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;

namespace QuoteFlow.Cargos.Excels;

public class CargoExcelDtoConverter : ExcelDtoConverter<CargoImportDto, CargoDataCreateParams>
{
    public CargoExcelDtoConverter(Volo.Abp.ObjectMapping.IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    //public CargoExcelDtoConverter(
    //    IObjectMapper objectMapper,
    //    IGuidGenerator guidGenerator)
    //    : base(objectMapper, guidGenerator)
    //{
    //}

    //protected override IEnumerable<string> RequiredValidationContextKeys => new[]
    //{
    //    ExcelImportContextKeys.ParentEntityId
    //};
    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<CargoImportDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<CargoDataCreateParams?> MapToCreateParamsAsync(
        CargoImportDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<CargoDataCreateParams?>(createParams);
    }

    private CargoDataCreateParams ToCreateParams(CargoImportDto importDto, ExcelImportContext context)
    {
        return new CargoDataCreateParams
        {
            //CargoId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
            //PODetailId = importDto.PODetailId,
            //PODetailCode = importDto.PODetailCode,
            GolfaCode = importDto.GolfaCode,
            Model = importDto.Model,
            PORef = importDto.PORef,
            PODetailCode = importDto.PODetailCode,
            InvoiceNo = importDto.InvoiceNo,
            SRNo = importDto.SRNo,
            Classification = importDto.Classification,
            Product = importDto.Product,
            //MaterialType = importDto.ProductCategory,
            Spec1 = importDto.Spec1,
            Spec2 = importDto.Spec2,
            Spec3 = importDto.Spec3,
            OrderQty = importDto.OrderQty,
            ExWorkQty = importDto.ExWorkQty,
            NonExWorkQty = importDto.NonExWorks,
            InSTCH = importDto.StockQuantity,
            Shipped = importDto.Shipped,
            WaitForShip = importDto.WaitForShip,
            ShipDate = importDto.ShipDate,
            OrderDate = importDto.Order,
            InSTCHDate = importDto.ShippingDate,
            ShipmentMethod = importDto.ShipmentMethod,
            ETA1 = importDto.ETA1,
            ETA2 = importDto.ETA2,
            MEVNRequest = importDto.MEVNRequest,
            STCReply = importDto.STCReply,
            EU = importDto.EU,
            MEVNAddedRequest = importDto.MEVNAddedRequest,
            //NPD = importDto.NPD,
            //PlannedShipment = importDto.PlannedShipmentItem,
            SODate = importDto.SODate,
            CellMarker = importDto.CellMarker,
            ShipmentForm = importDto.ShippingForm,
            MachineNumber = importDto.MachineNo,

            PODetailId = importDto.PODetailId
        };
    }
}

using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using System;

namespace QuoteFlow.Materials.ParameterObjects;

public class ExcelMaterialUpdateInventoryPlanUpdateParams
{
    public Guid? Id { get; set; }
    public string GolfaCode { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? InventoryCategory { get; set; }
    public int? StockWarning { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public ExcelMaterialUpdateInventoryPlanUpdateParams()
    {
    }
    public ExcelMaterialUpdateInventoryPlanUpdateParams(MaterialApprovalRequestDetail detail)
    {
        Id = detail.MaterialApprovalId;
        GolfaCode = detail.GolfaCode;
        Model = detail.Model;
        InventoryCategory = detail.InventoryCategory;
        StockWarning = detail.StockWarning;
    }
}

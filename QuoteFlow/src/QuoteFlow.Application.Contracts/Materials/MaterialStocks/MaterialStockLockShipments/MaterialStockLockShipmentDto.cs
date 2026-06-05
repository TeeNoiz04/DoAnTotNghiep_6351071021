using QuoteFlow.Shared;
using System;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;

public class MaterialStockLockShipmentDto : ExtendedAuditedEntityDto<Guid>
{
    public string GolfaCode { get; set; } = null!;
    public int? LockOnOrder { get; set; }
    public int? StockOnOrder { get; set; }
    public string? Note { get; set; }

}
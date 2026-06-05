using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;

public class MaterialStockLockShipmentUpdateDto
{
    [Required]
    [StringLength(MaterialStockLockShipmentConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    public int? LockOnOrder { get; set; }
    public int? StockOnOrder { get; set; }
    [StringLength(MaterialStockLockShipmentConsts.NoteMaxLength)]
    public string? Note { get; set; }

}
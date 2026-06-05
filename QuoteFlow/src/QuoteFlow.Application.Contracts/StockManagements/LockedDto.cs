using System;

namespace QuoteFlow.StockManagements;
public class LockedDto
{
    public string? DPONo { get; set; }
    public string? BuyerShortName { get; set; }
    public Guid? StockCategoryId { get; set; }
    public string? StockName { get; set; }
    public decimal? LockedQty { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Created { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? Modified { get; set; }

}

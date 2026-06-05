using QuoteFlow.Shared;
using QuoteFlow.StockCategories;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialStocks;

public class MaterialStockDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid MaterialId { get; set; }
    public Guid StockCategoryId { get; set; }
    public string GolfaCode { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int? Qty { get; set; }
    public int? Locked { get; set; }
    public int? LockStockKeeping { get; set; }
    public int? LockStockSO { get; set; }
    public int? Available_Qty { get; set; }
    public string? Note { get; set; }

    public StockCategoryDto? StockCategory { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}
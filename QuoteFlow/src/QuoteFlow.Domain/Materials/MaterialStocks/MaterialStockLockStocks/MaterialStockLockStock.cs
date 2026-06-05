using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.StockCategories;
using System;
using Volo.Abp;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;

public class MaterialStockLockStock : ExtendedFullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string GolfaCode { get; set; }

    public virtual Guid? DPODetailId { get; set; }

    public virtual Guid? StockCategoryId { get; set; }

    public virtual int Qty { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public virtual int ReleasedLock { get; set; }

    public virtual StockCategory? StockCategory { get; set; }

    protected MaterialStockLockStock()
    {

    }

    public MaterialStockLockStock(Guid id, string golfaCode, int qty, int releasedLock, Guid? dPODetailId = null, Guid? stockCategoryId = null, string? note = null)
    {

        Id = id;
        Check.NotNull(golfaCode, nameof(golfaCode));
        Check.Length(golfaCode, nameof(golfaCode), MaterialStockLockStockConsts.GolfaCodeMaxLength, 0);
        Check.Length(note, nameof(note), MaterialStockLockStockConsts.NoteMaxLength, 0);
        GolfaCode = golfaCode;
        Qty = qty;
        ReleasedLock = releasedLock;
        DPODetailId = dPODetailId;
        StockCategoryId = stockCategoryId;
        Note = note;
    }

}
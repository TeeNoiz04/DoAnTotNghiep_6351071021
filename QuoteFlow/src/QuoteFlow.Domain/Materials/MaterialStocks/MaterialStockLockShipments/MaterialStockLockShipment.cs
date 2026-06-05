using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;

public class MaterialStockLockShipment : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string GolfaCode { get; set; }

    public virtual int? LockOnOrder { get; set; }

    public virtual int? StockOnOrder { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    protected MaterialStockLockShipment()
    {

    }

    public MaterialStockLockShipment(Guid id, string golfaCode, int? lockOnOrder = null, int? stockOnOrder = null, string? note = null)
    {

        Id = id;
        Check.NotNull(golfaCode, nameof(golfaCode));
        Check.Length(golfaCode, nameof(golfaCode), MaterialStockLockShipmentConsts.GolfaCodeMaxLength, 0);
        Check.Length(note, nameof(note), MaterialStockLockShipmentConsts.NoteMaxLength, 0);
        GolfaCode = golfaCode;
        LockOnOrder = lockOnOrder;
        StockOnOrder = stockOnOrder;
        Note = note;
    }
    public MaterialStockLockShipment(Guid id, MaterialStockLockShipment createParams)
    {

        Id = id;

        GolfaCode = createParams.GolfaCode;
        LockOnOrder = createParams.LockOnOrder;
        StockOnOrder = createParams.StockOnOrder;
        Note = createParams.Note;
    }
}
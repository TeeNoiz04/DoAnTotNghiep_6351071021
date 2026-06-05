using JetBrains.Annotations;
using QuoteFlow.Materials.MaterialStocks.ParameterObjects;
using QuoteFlow.Shared.Models;
using QuoteFlow.StockCategories;
using System;
using Volo.Abp;

namespace QuoteFlow.Materials.MaterialStocks;

public class MaterialStock : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid MaterialId { get; set; }

    public virtual Guid StockCategoryId { get; set; }

    [NotNull]
    public virtual string GolfaCode { get; set; }
    [NotNull]
    public virtual string Model { get; set; }

    public virtual int? Qty { get; set; }

    public virtual int? Locked { get; set; }

    public virtual int? LockStockKeeping { get; set; }

    public virtual int? LockStockSO { get; set; }

    public virtual int? Available_Qty { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public StockCategory? StockCategory { get; set; }
    public Material Material { get; set; }

    protected MaterialStock()
    {

    }

    public MaterialStock(Guid id, Guid materialId, Guid stockCategoryId, string golfaCode, string model, int? qty = null, int? locked = null, int? lockStockKeeping = null, int? lockStockSO = null, int? available_Qty = null, string? note = null)
    {

        Id = id;
        Check.NotNull(golfaCode, nameof(golfaCode));
        Check.Length(golfaCode, nameof(golfaCode), MaterialStockConsts.GolfaCodeMaxLength, 0);
        Check.Length(note, nameof(note), MaterialStockConsts.NoteMaxLength, 0);
        MaterialId = materialId;
        StockCategoryId = stockCategoryId;
        GolfaCode = golfaCode;
        Model = model;
        Qty = qty;
        Locked = locked;
        LockStockKeeping = lockStockKeeping;
        LockStockSO = lockStockSO;
        Available_Qty = available_Qty;
        Note = note;
    }

    public MaterialStock(Guid id, MaterialStockCreateParams createParams)
    {
        Id = id;
        MaterialId = createParams.MaterialId;
        StockCategoryId = createParams.StockCategoryId;
        GolfaCode = createParams.GolfaCode;
        Model = createParams.Model;
        Qty = createParams.Qty;
        Locked = createParams.Locked;
        LockStockKeeping = createParams.LockStockKeeping;
        LockStockSO = createParams.LockStockSO;
        Available_Qty = createParams.Available_Qty;
        Note = createParams.Note;
    }


}
using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.StockCategories.ParameterObjects;
using System;
using Volo.Abp;

namespace QuoteFlow.StockCategories;

public class StockCategory : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string StockCode { get; set; }

    [NotNull]
    public virtual string StockName { get; set; }
    [CanBeNull]
    public virtual string? SAPCode { get; set; }

    public virtual bool? MainStock { get; set; }

    public virtual bool? DamagedStock { get; set; }
    public virtual bool? FOC { get; set; }

    public virtual int? SortOrder { get; set; }

    public virtual bool? IsDeactive { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    protected StockCategory()
    {

    }

    public StockCategory(Guid id, string stockCode, string stockName, string? sapCode = null, bool? mainStock = null, bool? damagedStock = null, int? sortOrder = null, bool? isDeactive = null, string? note = null, bool? fOC = null)
    {

        Id = id;
        Check.NotNull(stockCode, nameof(stockCode));
        Check.Length(stockCode, nameof(stockCode), StockCategoryConsts.StockCodeMaxLength, 0);
        Check.Length(sapCode, nameof(sapCode), StockCategoryConsts.SAPCodeMaxLength, 0);
        Check.NotNull(stockName, nameof(stockName));
        Check.Length(stockName, nameof(stockName), StockCategoryConsts.StockNameMaxLength, 0);
        Check.Length(note, nameof(note), StockCategoryConsts.NoteMaxLength, 0);
        StockCode = stockCode;
        StockName = stockName;
        SAPCode = sapCode;
        MainStock = mainStock;
        DamagedStock = damagedStock;
        SortOrder = sortOrder;
        IsDeactive = isDeactive;
        Note = note;
        FOC = fOC;
    }

    public StockCategory(Guid id, StockCategoryCreateParams createParams)
    {
        Id = id;
        StockCode = createParams.StockCode;
        StockName = createParams.StockName;
        SAPCode = createParams.SAPCode;
        MainStock = createParams.MainStock;
        FOC = createParams.FOC;
        DamagedStock = createParams.DamagedStock;
        SortOrder = createParams.SortOrder;
        IsDeactive = createParams.IsDeactive;
        Note = createParams.Note;
    }
}
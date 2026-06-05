using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.StockCategories;

public class StockCategoryDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string StockCode { get; set; } = null!;
    public string StockName { get; set; } = null!;
    public string? SAPCode { get; set; }
    public bool? MainStock { get; set; }
    public bool? FOC { get; set; }
    public bool? DamagedStock { get; set; }
    public int? SortOrder { get; set; }
    public bool? IsDeactive { get; set; }
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}
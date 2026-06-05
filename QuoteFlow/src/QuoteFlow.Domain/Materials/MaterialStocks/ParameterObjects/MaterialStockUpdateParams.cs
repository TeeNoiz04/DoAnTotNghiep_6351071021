using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialStocks.ParameterObjects;
public class MaterialStockUpdateParams : IHasConcurrencyStamp
{
    [Required]
    public Guid MaterialId { get; set; }
    [Required]
    public Guid StockCategoryId { get; set; }
    [Required]
    [StringLength(MaterialStockConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int? Qty { get; set; }
    public int? Locked { get; set; }
    public int? LockStockKeeping { get; set; }
    public int? LockStockSO { get; set; }
    public int? Available_Qty { get; set; }
    [StringLength(MaterialStockConsts.NoteMaxLength)]
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.MaterialStockUploadDetails;

public class MaterialStockUploadDetailUpdateDto : IHasConcurrencyStamp
{
    [Required]
    public Guid RequestId { get; set; }
    [Required]
    [StringLength(MaterialStockUploadDetailConsts.MaterialCodeMaxLength)]
    public string MaterialCode { get; set; } = null!;
    [StringLength(MaterialStockUploadDetailConsts.ModelMaxLength)]
    public string? Model { get; set; }
    [StringLength(MaterialStockUploadDetailConsts.StorageMaxLength)]
    public string? Storage { get; set; }
    [StringLength(MaterialStockUploadDetailConsts.StorageDestinationMaxLength)]
    public string? StorageDestination { get; set; }
    public decimal? Qty { get; set; }
    [StringLength(MaterialStockUploadDetailConsts.RefDocMaxLength)]
    public string? RefDoc { get; set; }
    public string? Remark { get; set; }
    public virtual Guid? StorageDesc_Id { get; set; } = null;
    public virtual Guid? StorageSrc_Id { get; set; } = null;
    public string ConcurrencyStamp { get; set; } = null!;
}
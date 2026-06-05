using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.MaterialStockUploads;

public class MaterialStockUploadUpdateDto : IHasConcurrencyStamp
{
    [StringLength(MaterialStockUploadConsts.RequestNoMaxLength)]
    public string? RequestNo { get; set; }
    [StringLength(MaterialStockUploadConsts.ImportTypeMaxLength)]
    public string? ImportType { get; set; }
    [StringLength(MaterialStockUploadConsts.FilNameMaxLength)]
    public string? FileName { get; set; }
    [StringLength(MaterialStockUploadConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [StringLength(MaterialStockUploadConsts.StatusMaxLength)]
    public string? Status { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
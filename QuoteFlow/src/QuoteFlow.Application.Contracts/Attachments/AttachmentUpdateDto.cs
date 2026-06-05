using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Attachments;

public class AttachmentUpdateDto : IHasConcurrencyStamp
{
    [StringLength(AttachmentConsts.RequestPartMaxLength)]
    public string? RequestPart { get; set; }
    [StringLength(AttachmentConsts.AttachCodeMaxLength)]
    public string? AttachCode { get; set; }
    [StringLength(AttachmentConsts.AttachNameMaxLength)]
    public string? AttachName { get; set; }
    [Required]
    [StringLength(AttachmentConsts.FileNameMaxLength)]
    public string FileName { get; set; } = null!;
    [Required]
    [StringLength(AttachmentConsts.FileNameDBMaxLength)]
    public string FileNameDB { get; set; } = null!;
    [StringLength(AttachmentConsts.FilePathMaxLength)]
    public string? FilePath { get; set; }
    [Required]
    public bool OfflineAttachment { get; set; }
    [StringLength(AttachmentConsts.DescriptionMaxLength)]
    public string? Description { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
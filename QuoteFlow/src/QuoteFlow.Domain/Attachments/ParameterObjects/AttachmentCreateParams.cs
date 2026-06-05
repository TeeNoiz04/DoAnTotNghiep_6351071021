using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Attachments.ParameterObjects;
public class AttachmentCreateParams
{
    [StringLength(AttachmentConsts.RequestPartMaxLength)]
    public string? RequestPart { get; set; }

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
}

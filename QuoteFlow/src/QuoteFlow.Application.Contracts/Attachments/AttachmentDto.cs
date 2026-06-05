using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Attachments;

public class AttachmentDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string? RequestPart { get; set; }
    public string? AttachCode { get; set; }
    public string? AttachName { get; set; }
    public string FileName { get; set; } = null!;
    public string FileNameDB { get; set; } = null!;
    public string? FilePath { get; set; }
    public bool OfflineAttachment { get; set; }
    public string? Description { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}
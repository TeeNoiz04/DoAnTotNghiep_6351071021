using JetBrains.Annotations;
using QuoteFlow.Attachments.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.Attachments;

public class Attachment : ExtendedAuditedAggregateRoot<Guid>
{
    [CanBeNull]
    public virtual string? RequestPart { get; set; }

    [CanBeNull]
    public virtual string? AttachCode { get; set; }

    [CanBeNull]
    public virtual string? AttachName { get; set; }

    [NotNull]
    public virtual string FileName { get; set; }

    [NotNull]
    public virtual string FileNameDB { get; set; }

    [CanBeNull]
    public virtual string? FilePath { get; set; }

    public virtual bool OfflineAttachment { get; set; }

    [CanBeNull]
    public virtual string? Description { get; set; }

    protected Attachment()
    {

    }

    public Attachment(Guid id, string fileName, string fileNameDB, bool offlineAttachment, string? requestPart = null, string? attachCode = null, string? attachName = null, string? filePath = null, string? description = null)
    {

        Id = id;
        Check.NotNull(fileName, nameof(fileName));
        Check.Length(fileName, nameof(fileName), AttachmentConsts.FileNameMaxLength, 0);
        Check.NotNull(fileNameDB, nameof(fileNameDB));
        Check.Length(fileNameDB, nameof(fileNameDB), AttachmentConsts.FileNameDBMaxLength, 0);
        Check.Length(requestPart, nameof(requestPart), AttachmentConsts.RequestPartMaxLength, 0);
        Check.Length(attachCode, nameof(attachCode), AttachmentConsts.AttachCodeMaxLength, 0);
        Check.Length(attachName, nameof(attachName), AttachmentConsts.AttachNameMaxLength, 0);
        Check.Length(filePath, nameof(filePath), AttachmentConsts.FilePathMaxLength, 0);
        Check.Length(description, nameof(description), AttachmentConsts.DescriptionMaxLength, 0);
        FileName = fileName;
        FileNameDB = fileNameDB;
        OfflineAttachment = offlineAttachment;
        RequestPart = requestPart;
        AttachCode = attachCode;
        AttachName = attachName;
        FilePath = filePath;
        Description = description;
    }

    public Attachment(Guid id, AttachmentCreateParams createParams)
    {
        Id = id;
        FileName = createParams.FileName;
        FileNameDB = createParams.FileNameDB;
        OfflineAttachment = createParams.OfflineAttachment;
        RequestPart = createParams.RequestPart;
        FilePath = createParams.FilePath;
        Description = createParams.Description;
    }
}
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Attachments;

public class AttachmentManager : DomainService
{
    protected IAttachmentRepository _attachmentRepository;

    public AttachmentManager(IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }

    public virtual async Task<Attachment> CreateAsync(
    string fileName, string fileNameDB, bool offlineAttachment, string? requestPart = null, string? attachCode = null, string? attachName = null, string? filePath = null, string? description = null)
    {
        Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
        Check.Length(fileName, nameof(fileName), AttachmentConsts.FileNameMaxLength);
        Check.NotNullOrWhiteSpace(fileNameDB, nameof(fileNameDB));
        Check.Length(fileNameDB, nameof(fileNameDB), AttachmentConsts.FileNameDBMaxLength);
        Check.Length(requestPart, nameof(requestPart), AttachmentConsts.RequestPartMaxLength);
        Check.Length(attachCode, nameof(attachCode), AttachmentConsts.AttachCodeMaxLength);
        Check.Length(attachName, nameof(attachName), AttachmentConsts.AttachNameMaxLength);
        Check.Length(filePath, nameof(filePath), AttachmentConsts.FilePathMaxLength);
        Check.Length(description, nameof(description), AttachmentConsts.DescriptionMaxLength);

        var attachment = new Attachment(
         GuidGenerator.Create(),
         fileName, fileNameDB, offlineAttachment, requestPart, attachCode, attachName, filePath, description
         );

        return await _attachmentRepository.InsertAsync(attachment);
    }

    public virtual async Task<Attachment> UpdateAsync(
        Guid id,
        string fileName, string fileNameDB, bool offlineAttachment, string? requestPart = null, string? attachCode = null, string? attachName = null, string? filePath = null, string? description = null, [CanBeNull] string? concurrencyStamp = null
    )
    {
        Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
        Check.Length(fileName, nameof(fileName), AttachmentConsts.FileNameMaxLength);
        Check.NotNullOrWhiteSpace(fileNameDB, nameof(fileNameDB));
        Check.Length(fileNameDB, nameof(fileNameDB), AttachmentConsts.FileNameDBMaxLength);
        Check.Length(requestPart, nameof(requestPart), AttachmentConsts.RequestPartMaxLength);
        Check.Length(attachCode, nameof(attachCode), AttachmentConsts.AttachCodeMaxLength);
        Check.Length(attachName, nameof(attachName), AttachmentConsts.AttachNameMaxLength);
        Check.Length(filePath, nameof(filePath), AttachmentConsts.FilePathMaxLength);
        Check.Length(description, nameof(description), AttachmentConsts.DescriptionMaxLength);

        var attachment = await _attachmentRepository.GetAsync(id);

        attachment.FileName = fileName;
        attachment.FileNameDB = fileNameDB;
        attachment.OfflineAttachment = offlineAttachment;
        attachment.RequestPart = requestPart;
        attachment.AttachCode = attachCode;
        attachment.AttachName = attachName;
        attachment.FilePath = filePath;
        attachment.Description = description;

        attachment.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _attachmentRepository.UpdateAsync(attachment);
    }

}
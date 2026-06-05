using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.Attachments;

public interface IAttachmentsAppService : IApplicationService
{

    Task UploadFileAsync(List<IRemoteStreamContent> files, Guid requestId, string attachmentCode);

    Task<FileDto> GetFileByIdAsync(Guid id);  // Download file
    Task DeleteFileAsync(Guid id); // Delete file
}
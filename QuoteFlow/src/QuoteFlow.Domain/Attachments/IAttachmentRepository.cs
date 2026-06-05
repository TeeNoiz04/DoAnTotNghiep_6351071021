using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Attachments;

public interface IAttachmentRepository : IRepository<Attachment, Guid>
{
    Task<List<Attachment>> GetListAsync(
        string? filterText = null,
        string? requestPart = null,
        string? attachCode = null,
        string? attachName = null,
        string? fileName = null,
        string? fileNameDB = null,
        string? filePath = null,
        bool? offlineAttachment = null,
        string? description = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        string? requestPart = null,
        string? attachCode = null,
        string? attachName = null,
        string? fileName = null,
        string? fileNameDB = null,
        string? filePath = null,
        bool? offlineAttachment = null,
        string? description = null,
        CancellationToken cancellationToken = default);
}
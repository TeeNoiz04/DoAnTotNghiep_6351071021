using QuoteFlow.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Attachments;

public class EfCoreAttachmentRepository : EfCoreRepository<QuoteFlowDbContext, Attachment, Guid>, IAttachmentRepository
{
    public EfCoreAttachmentRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<Attachment>> GetListAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, requestPart, attachCode, attachName, fileName, fileNameDB, filePath, offlineAttachment, description);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? AttachmentConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        string? requestPart = null,
        string? attachCode = null,
        string? attachName = null,
        string? fileName = null,
        string? fileNameDB = null,
        string? filePath = null,
        bool? offlineAttachment = null,
        string? description = null,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, requestPart, attachCode, attachName, fileName, fileNameDB, filePath, offlineAttachment, description);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Attachment> ApplyFilter(
        IQueryable<Attachment> query,
        string? filterText = null,
        string? requestPart = null,
        string? attachCode = null,
        string? attachName = null,
        string? fileName = null,
        string? fileNameDB = null,
        string? filePath = null,
        bool? offlineAttachment = null,
        string? description = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.RequestPart!.Contains(filterText!) || e.AttachCode!.Contains(filterText!) || e.AttachName!.Contains(filterText!) || e.FileName!.Contains(filterText!) || e.FileNameDB!.Contains(filterText!) || e.FilePath!.Contains(filterText!) || e.Description!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(requestPart), e => e.RequestPart.Contains(requestPart))
                .WhereIf(!string.IsNullOrWhiteSpace(attachCode), e => e.AttachCode.Contains(attachCode))
                .WhereIf(!string.IsNullOrWhiteSpace(attachName), e => e.AttachName.Contains(attachName))
                .WhereIf(!string.IsNullOrWhiteSpace(fileName), e => e.FileName.Contains(fileName))
                .WhereIf(!string.IsNullOrWhiteSpace(fileNameDB), e => e.FileNameDB.Contains(fileNameDB))
                .WhereIf(!string.IsNullOrWhiteSpace(filePath), e => e.FilePath.Contains(filePath))
                .WhereIf(offlineAttachment.HasValue, e => e.OfflineAttachment == offlineAttachment)
                .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description));
    }
}
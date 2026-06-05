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

namespace QuoteFlow.Messages;

public class EfCoreMessageRepository : EfCoreRepository<QuoteFlowDbContext, Message, Guid>, IMessageRepository
{
    public EfCoreMessageRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<Message>> GetListAsync(
        string? filterText = null,
        string? userName = null,
        string? fullName = null,
        string? sendTo = null,
        string? comment = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, userName, fullName, sendTo, comment);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? MessageConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        string? userName = null,
        string? fullName = null,
        string? sendTo = null,
        string? comment = null,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, userName, fullName, sendTo, comment);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Message> ApplyFilter(
        IQueryable<Message> query,
        string? filterText = null,
        string? userName = null,
        string? fullName = null,
        string? sendTo = null,
        string? comment = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.UserName!.Contains(filterText!) || e.FullName!.Contains(filterText!) || e.SendTo!.Contains(filterText!) || e.Comment!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(userName), e => e.UserName.Contains(userName))
                .WhereIf(!string.IsNullOrWhiteSpace(fullName), e => e.FullName.Contains(fullName))
                .WhereIf(!string.IsNullOrWhiteSpace(sendTo), e => e.SendTo.Contains(sendTo))
                .WhereIf(!string.IsNullOrWhiteSpace(comment), e => e.Comment.Contains(comment));
    }
}
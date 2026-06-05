using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Messages;

public interface IMessageRepository : IRepository<Message, Guid>
{
    Task<List<Message>> GetListAsync(
        string? filterText = null,
        string? userName = null,
        string? fullName = null,
        string? sendTo = null,
        string? comment = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        string? userName = null,
        string? fullName = null,
        string? sendTo = null,
        string? comment = null,
        CancellationToken cancellationToken = default);
}
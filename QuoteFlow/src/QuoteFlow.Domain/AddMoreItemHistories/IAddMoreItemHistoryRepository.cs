using System;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.AddMoreItemHistories;
public interface IAddMoreItemHistoryRepository : IRepository<AddMoreItemHistory, Guid>
{
}

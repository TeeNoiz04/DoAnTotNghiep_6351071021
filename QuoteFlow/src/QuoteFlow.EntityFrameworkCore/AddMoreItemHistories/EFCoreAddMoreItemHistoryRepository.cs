using QuoteFlow.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.AddMoreItemHistories;
public class EFCoreAddMoreItemHistoryRepository : EfCoreRepository<QuoteFlowDbContext, AddMoreItemHistory, Guid>, IAddMoreItemHistoryRepository
{
    public EFCoreAddMoreItemHistoryRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}

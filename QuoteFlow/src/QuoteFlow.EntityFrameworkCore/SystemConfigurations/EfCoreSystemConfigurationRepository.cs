using QuoteFlow.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.SystemConfigurations;

public class EfCoreSystemConfigurationRepository : EfCoreRepository<QuoteFlowDbContext, SystemConfiguration, Guid>, ISystemConfigurationRepository
{
    public EfCoreSystemConfigurationRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<SystemConfiguration>> GetListAsync(
        string? filterText = null,
        string? cfgKey = null,
        string? cfgValue = null,
        string? description = null,
        bool? isSystemCfg = null,
        string? cfgType = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterText, cfgKey, cfgValue, description, cfgType, isSystemCfg);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? SystemConfigurationConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        string? cfgKey = null,
        string? cfgValue = null,
        string? description = null,
        bool? isSystemCfg = null,
         string? cfgType = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterText, cfgKey, cfgValue, description, cfgType, isSystemCfg);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<SystemConfiguration> ApplyFilter(
        IQueryable<SystemConfiguration> query,
        string? filterText = null,
        string? cfgKey = null,
        string? cfgValue = null,
        string? description = null,
        string? cfgType = null,
        bool? isSystemCfg = null)
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.CfgKey!.Contains(filterText!) || e.CfgValue!.Contains(filterText!) || e.Description!.Contains(filterText!))
            .WhereIf(!string.IsNullOrWhiteSpace(cfgKey), e => e.CfgKey == cfgKey)
            .WhereIf(!string.IsNullOrWhiteSpace(cfgValue), e => e.CfgValue == cfgValue)
            .WhereIf(!string.IsNullOrWhiteSpace(cfgType), e => e.CfgType == cfgType)
            .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description == description)
            .WhereIf(isSystemCfg.HasValue, e => e.IsSystemCfg == isSystemCfg);
    }
}
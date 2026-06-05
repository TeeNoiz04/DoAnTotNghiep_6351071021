using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SystemConfigurations;

public interface ISystemConfigurationRepository : IRepository<SystemConfiguration, Guid>
{
    Task<List<SystemConfiguration>> GetListAsync(
        string? filterText = null,
        string? cfgKey = null,
        string? cfgValue = null,
        string? description = null,
        bool? isSystemCfg = null,
        string? cfgType = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        string? cfgKey = null,
        string? cfgValue = null,
        string? description = null,
        bool? isSystemCfg = null,
        string? cfgType = null,
        CancellationToken cancellationToken = default);
}
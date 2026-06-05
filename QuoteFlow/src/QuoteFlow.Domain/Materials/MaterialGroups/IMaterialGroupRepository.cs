using QuoteFlow.Materials.MaterialGroups.ParameterObject;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Materials.MaterialGroups;

public interface IMaterialGroupRepository : IRepository<MaterialGroup, Guid>
{
    Task<List<T>> GetListAsync<T>(
        MaterialGroupFilterParams filterParams,
        Expression<Func<MaterialGroup, T>> selector,
        CancellationToken cancellationToken = default);

    Task<List<MaterialGroup>> GetListAsync(
        MaterialGroupFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );
    Task<long> GetCountAsync(
       MaterialGroupFilterParams filterParams,
       string? sorting = null,
       int maxResultCount = int.MaxValue,
       int skipCount = 0,
       CancellationToken cancellationToken = default
   );
}

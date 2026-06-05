using QuoteFlow.SupplierBUs.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SupplierBUs;

public interface ISupplierBURepository : IRepository<SupplierBU, Guid>
{
    Task<List<SupplierBU>> GetListAsync(
        SupplierBUFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        SupplierBUFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<List<T>> GetListAsync<T>(
    SupplierBUFilterParams filterParams,
    Expression<Func<SupplierBU, T>> selector,
    CancellationToken cancellationToken = default);
}
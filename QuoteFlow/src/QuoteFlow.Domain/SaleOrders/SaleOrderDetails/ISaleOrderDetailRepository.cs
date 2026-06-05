using QuoteFlow.SaleOrders.SaleOrderDetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SaleOrders.SaleOrderDetails;

public interface ISaleOrderDetailRepository : IRepository<SaleOrderDetail, Guid>
{
    Task<List<SaleOrderDetail>> GetListAsync(
       SaleOrderDetailFilterParams filterParams,
        CancellationToken cancellationToken = default
    );
    Task<List<T>> GetListAsync<T>(
     SaleOrderDetailFilterParams filterParams,
     Expression<Func<SaleOrderDetail, T>> selector,
     CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
       SaleOrderDetailFilterParams filterParams,
        CancellationToken cancellationToken = default);
}
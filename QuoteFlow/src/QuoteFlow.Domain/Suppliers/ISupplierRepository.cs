using QuoteFlow.Suppliers.ParameterObject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Suppliers;

public interface ISupplierRepository : IRepository<Supplier, Guid>
{
    Task<List<Supplier>> GetListAsync(
    SupplierFilterParams filterParams,
    CancellationToken cancellationToken = default
);

    Task<long> GetCountAsync(
        SupplierFilterParams filterParams,
        CancellationToken cancellationToken = default);
}

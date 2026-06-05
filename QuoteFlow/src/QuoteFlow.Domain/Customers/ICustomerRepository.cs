using QuoteFlow.Customers.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Customers;

public interface ICustomerRepository : IRepository<Customer, Guid>
{
    Task<List<Customer>> GetListAsync(
        CustomerFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        CustomerFilterParams filterParams,
        CancellationToken cancellationToken = default);
}
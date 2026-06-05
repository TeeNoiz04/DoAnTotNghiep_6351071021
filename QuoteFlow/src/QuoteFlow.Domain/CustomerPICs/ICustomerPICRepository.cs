using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.CustomerPICs;

public interface ICustomerPICRepository : IRepository<CustomerPIC, Guid>
{
    Task<List<CustomerPIC>> GetListAsync(
        string? filterText = null,
        Guid? keyAccountId = null,
        string? pICName = null,
        string? pIC_Phone = null,
        string? pIC_Email = null,
        string? pIC_JobTitle = null,
        string? remark = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        Guid? keyAccountId = null,
        string? pICName = null,
        string? pIC_Phone = null,
        string? pIC_Email = null,
        string? pIC_JobTitle = null,
        string? remark = null,
        CancellationToken cancellationToken = default);
}
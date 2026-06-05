using QuoteFlow.SalesAssignments.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SalesAssignments;

public interface ISalesAssignmentRepository : IRepository<SalesAssignment, Guid>
{
    Task<List<SalesAssignment>> GetListAsync(
        SalesAssignmentFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        SalesAssignmentFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<List<SaleReportByCustomer>> ExportSaleReportAsync(SaleReportFillterParams fillterParams);
    Task<List<SaleReportByCustomerR05>> ExportSaleReportR05Async(SaleReportFillterParams fillterParams);
}

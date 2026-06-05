using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SaleOrdersSapImports;

public interface ISaleOrdersSapImportRepository : IRepository<SaleOrdersSapImport, Guid>
{
    Task<List<SaleOrdersSapImport>> GetListAsync(
        SaleOrderSapImportFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        SaleOrderSapImportFilterParams filterParams,
        CancellationToken cancellationToken = default);
}
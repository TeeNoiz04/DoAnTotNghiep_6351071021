using QuoteFlow.CfgDiscountRatios.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.CfgDiscountRatios
{
    public interface ICfgDiscountRatioRepository : IRepository<CfgDiscountRatio, Guid>
    {
        Task<List<CfgDiscountRatio>> GetListAsync(
            CfgDiscountRatioFilterParams filterParams,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            CfgDiscountRatioFilterParams filterParams,
            CancellationToken cancellationToken = default);
    }
}
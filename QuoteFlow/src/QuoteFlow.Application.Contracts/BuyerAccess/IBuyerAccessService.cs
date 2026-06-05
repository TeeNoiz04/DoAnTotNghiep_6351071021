using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace QuoteFlow.BuyerAccess;

public interface IBuyerAccessService : ITransientDependency
{
    Task<BuyerAccessResult> GetBuyerAccessAsync(string? currentUserName = null);
    Task<bool> HasFullBuyerAccessAsync(string? currentUserName = null);
    Task<List<Guid>> GetRestrictedBuyerIdsAsync(string? currentUserName = null);
    Task<bool> CanAccessBuyerAsync(Guid buyerId, string? currentUserName = null);
    Task<bool> IsSalePICForBuyerAsync(Guid buyerId, string? currentUserName = null);
}
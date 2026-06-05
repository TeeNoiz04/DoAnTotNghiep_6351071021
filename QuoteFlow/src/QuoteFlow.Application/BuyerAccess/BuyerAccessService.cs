using QuoteFlow.Permissions;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SalesAssignments;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization;

namespace QuoteFlow.BuyerAccess;

public class BuyerAccessService : IBuyerAccessService
{
    protected readonly ISalesAssignmentRepository _salesAssignmentRepository;
    protected readonly IEffectiveUserContext _currentUser;
    protected readonly IAbpAuthorizationService _authorizationService;
    protected readonly IReadOnlyCollection<string> _fullAccessRoles =
    [
        QuoteFlowPermissions.General.FullAccessToSalesDimensions,
    ];

    public BuyerAccessService(
        ISalesAssignmentRepository salesAssignmentRepository,
        IEffectiveUserContext currentUser,
        IAbpAuthorizationService authorizationService)
    {
        _salesAssignmentRepository = salesAssignmentRepository;
        _currentUser = currentUser;
        _authorizationService = authorizationService;
    }

    public virtual async Task<BuyerAccessResult> GetBuyerAccessAsync(string? currentUserName = null)
    {
        var userName = currentUserName ?? _currentUser.Username ?? string.Empty;

        var saleAssignments = await _salesAssignmentRepository.GetListAsync(x => x.SaleUserName == userName);
        var hasFullBuyerAccess = await _authorizationService.IsGrantedAnyAsync([.. _fullAccessRoles]);

        var isSalePIC = saleAssignments.Count > 0;

        var restrictedBuyerIds = new List<Guid>();
        var restrictedMaterialTypes = new List<string>();
        var restrictedLocationIds = new List<Guid>();
        if (isSalePIC && !hasFullBuyerAccess)
        {
            restrictedBuyerIds = saleAssignments.Select(x => x.BuyerId).Distinct().ToList();
            restrictedMaterialTypes = saleAssignments.Select(x => x.MaterialType).Distinct().ToList();
            restrictedLocationIds = saleAssignments.Select(x => x.LocationId).Distinct().ToList();
        }

        return new BuyerAccessResult
        {
            HasFullAccess = hasFullBuyerAccess,
            IsSalePIC = isSalePIC,
            RestrictedBuyerIds = restrictedBuyerIds,
            RestrictedMaterialTypes = restrictedMaterialTypes,
            RestrictedLocationIds = restrictedLocationIds
        };
    }

    public virtual async Task<bool> HasFullBuyerAccessAsync(string? currentUserName = null)
    {
        var accessResult = await GetBuyerAccessAsync(currentUserName);
        return accessResult.HasFullAccess;
    }

    public virtual async Task<List<Guid>> GetRestrictedBuyerIdsAsync(string? currentUserName = null)
    {
        var accessResult = await GetBuyerAccessAsync(currentUserName);
        return accessResult.RestrictedBuyerIds;
    }

    public virtual async Task<bool> CanAccessBuyerAsync(Guid buyerId, string? currentUserName = null)
    {
        var accessResult = await GetBuyerAccessAsync(currentUserName);
        return accessResult.CanAccess(buyerId);
    }

    public virtual async Task<bool> IsSalePICForBuyerAsync(Guid buyerId, string? currentUserName = null)
    {
        var accessResult = await GetBuyerAccessAsync(currentUserName);
        return accessResult.IsSalePIC && accessResult.RestrictedBuyerIds.Contains(buyerId);
    }
}
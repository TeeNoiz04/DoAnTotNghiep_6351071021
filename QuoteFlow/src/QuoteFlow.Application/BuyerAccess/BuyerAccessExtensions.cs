using System;

namespace QuoteFlow.BuyerAccess;

public static class BuyerAccessExtensions
{
    public static void ApplyBuyerRestrictions<T>(
        this T filterParams,
        BuyerAccessResult accessResult,
        string? currentUserName = null)
        where T : IBuyerRestrictable
    {
        if (!accessResult.HasFullAccess && !accessResult.IsSalePIC)
        {
            filterParams.RestrictedBuyerIds = [Guid.Empty];
            filterParams.BuyerId = null;
            return;
        }

        if (filterParams.BuyerId.HasValue && !accessResult.CanAccess(filterParams.BuyerId.Value))
        {
            filterParams.RestrictedBuyerIds = accessResult.RestrictedBuyerIds;
            return;
        }

        filterParams.RestrictedBuyerIds = filterParams.BuyerId.HasValue
            ? []
            : accessResult.RestrictedBuyerIds;
    }

    public static void ApplyLocationRestrictions<T>(
        this T filterParams,
        BuyerAccessResult accessResult,
        string? currentUserName = null)
        where T : ILocationRestrictable
    {
        if (!accessResult.HasFullAccess && !accessResult.IsSalePIC)
        {
            filterParams.RestrictedLocationIds = [Guid.Empty];
            filterParams.LocationId = null;
            return;
        }

        if (filterParams.LocationId.HasValue &&
            !accessResult.CanAccess(null, null, filterParams.LocationId.Value))
        {
            filterParams.RestrictedLocationIds = accessResult.RestrictedLocationIds;
            return;
        }

        filterParams.RestrictedLocationIds = filterParams.LocationId.HasValue
            ? []
            : accessResult.RestrictedLocationIds;
    }

    public static void ApplyMaterialTypeRestrictions<T>(
        this T filterParams,
        BuyerAccessResult accessResult,
        string? currentUserName = null)
        where T : IMaterialTypeRestrictable
    {
        if (!accessResult.HasFullAccess && !accessResult.IsSalePIC)
        {
            filterParams.RestrictedMaterialTypes = [string.Empty];
            filterParams.MaterialType = null;
            return;
        }

        if (!string.IsNullOrEmpty(filterParams.MaterialType) &&
            !accessResult.CanAccess(null, filterParams.MaterialType, null))
        {
            filterParams.RestrictedMaterialTypes = accessResult.RestrictedMaterialTypes;
            return;
        }

        filterParams.RestrictedMaterialTypes = !string.IsNullOrEmpty(filterParams.MaterialType)
            ? []
            : accessResult.RestrictedMaterialTypes;
    }
}
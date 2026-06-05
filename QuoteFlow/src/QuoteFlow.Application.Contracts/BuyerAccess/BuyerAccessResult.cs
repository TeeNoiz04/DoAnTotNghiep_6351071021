using System;
using System.Collections.Generic;
using Volo.Abp;

namespace QuoteFlow.BuyerAccess;

public class BuyerAccessResult
{
    public bool HasFullAccess { get; set; }
    public bool IsSalePIC { get; set; }
    public List<Guid> RestrictedBuyerIds { get; set; } = new();
    public List<string> RestrictedMaterialTypes { get; set; } = new();
    public List<Guid> RestrictedLocationIds { get; set; } = new();

    public void ValidateBuyerAccess(
        Guid? buyerId,
        string? materialType = null,
        Guid? locationId = null
    )
    {
        var buyerNotValid = buyerId.HasValue && !RestrictedBuyerIds.Contains(buyerId.Value);
        var materialTypeNotValid = !string.IsNullOrEmpty(materialType) && !RestrictedMaterialTypes.Contains(materialType);
        var locationIdNotValid = locationId.HasValue && !RestrictedLocationIds.Contains(locationId.Value);

        if (IsSalePIC
            && !HasFullAccess
            && (buyerNotValid || materialTypeNotValid || locationIdNotValid)
        )
        {
            throw new UserFriendlyException("You do not have access to the specified buyer's DPOs.");
        }
        else if (!HasFullAccess && !IsSalePIC)
        {
            throw new UserFriendlyException("No access to buyer data - you must be a sale PIC or have full buyer access permission.");
        }
    }

    /// <summary>
    /// Silently checks if user has access without throwing exceptions
    /// </summary>
    public bool HasGeneralAccess()
    {
        return HasFullAccess || IsSalePIC;
    }

    public bool CanAccess(Guid? buyerId, string? materialType = null, Guid? locationId = null)
    {
        if (HasFullAccess) return true;
        if (!IsSalePIC) return false;
        if (!buyerId.HasValue && string.IsNullOrEmpty(materialType) && !locationId.HasValue) return true;

        bool buyerOk = !buyerId.HasValue || RestrictedBuyerIds.Contains(buyerId.Value);
        bool materialOk = string.IsNullOrEmpty(materialType) || RestrictedMaterialTypes.Contains(materialType);
        bool locationOk = !locationId.HasValue || RestrictedLocationIds.Contains(locationId.Value);

        return buyerOk && materialOk && locationOk;
    }

    public List<Guid> GetAllowedBuyerIds()
    {
        return HasFullAccess ? [] : RestrictedBuyerIds;
    }

    public List<string> GetAllowedMaterialTypes()
    {
        return HasFullAccess ? [] : RestrictedMaterialTypes;
    }

    public List<Guid> GetAllowedLocationIds()
    {
        return HasFullAccess ? [] : RestrictedLocationIds;
    }
}
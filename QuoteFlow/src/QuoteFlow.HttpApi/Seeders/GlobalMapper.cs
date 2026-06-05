namespace QuoteFlow.Seeders;

public static class GlobalMapper
{
    //public static KeyAccountWithNavigationListDto MapToNavigationListDto(KeyAccountDto x)
    //{
    //    return new KeyAccountWithNavigationListDto()
    //    {
    //        BuyerId = x.BuyerId,
    //        TaxCode = x.TaxCode,
    //        KeyAccountCode = x.KeyAccountCode,
    //        KeyAccountShortName = x.KeyAccountShortName,
    //        KeyAccountName = x.KeyAccountName,
    //        KeyAccountTypeId = x.KeyAccountTypeId,
    //        KeyAccountClassId = x.KeyAccountClassId,
    //        CustomerLocationId = x.CustomerLocationId,
    //        Status = x.Status,
    //        CurrentApprovalRouteInstanceId = x.CurrentApprovalRouteInstanceId,
    //        CurrentApproverRoleName = x.CurrentApproverRoleName,
    //        CurrentApprovalStepSequence = x.CurrentApprovalStepSequence,

    //        KeyAccountClass = MapToListDto(x.KeyAccountClass),
    //        KeyAccountType = MapToListDto(x.KeyAccountType)!,
    //        Buyer = MapToListDto(x.Buyer),

    //        Id = x.Id,
    //        CreatorId = x.CreatorId,
    //        CreatorUsername = x.CreatorUsername,
    //        CreatorName = x.CreatorName,
    //        CreationTime = x.CreationTime,
    //        LastModifierId = x.LastModifierId,
    //        LastModifierUsername = x.LastModifierUsername,
    //        LastModifierName = x.LastModifierName,
    //        LastModificationTime = x.LastModificationTime,
    //    };
    //}

    //public static KeyAccountListDto MapToListDto(KeyAccountWithNavigationListDto x)
    //{
    //    return new KeyAccountListDto()
    //    {
    //        BuyerId = x.BuyerId,
    //        TaxCode = x.TaxCode,
    //        KeyAccountCode = x.KeyAccountCode,
    //        KeyAccountShortName = x.KeyAccountShortName,
    //        KeyAccountName = x.KeyAccountName,
    //        KeyAccountTypeId = x.KeyAccountTypeId,
    //        KeyAccountClassId = x.KeyAccountClassId,
    //        CustomerLocationId = x.CustomerLocationId,
    //        Status = x.Status,
    //        CurrentApprovalRouteInstanceId = x.CurrentApprovalRouteInstanceId,
    //        CurrentApproverRoleName = x.CurrentApproverRoleName,
    //        CurrentApprovalStepSequence = x.CurrentApprovalStepSequence,

    //        Id = x.Id,
    //        CreatorId = x.CreatorId,
    //        CreatorUsername = x.CreatorUsername,
    //        CreatorName = x.CreatorName,
    //        CreationTime = x.CreationTime,
    //        LastModifierId = x.LastModifierId,
    //        LastModifierUsername = x.LastModifierUsername,
    //        LastModifierName = x.LastModifierName,
    //        LastModificationTime = x.LastModificationTime,
    //    };
    //}

    //public static SystemCategoryListDto? MapToListDto(SystemCategoryDto? dto)
    //{
    //    if (dto is null) return null;

    //    return new SystemCategoryListDto()
    //    {
    //        Code = dto.Code,
    //        Description = dto.Description,
    //        CategoryType = dto.CategoryType,
    //        Value = dto.Value,
    //        IsDeactive = dto.IsDeactive,
    //        Note = dto.Note,

    //        Id = dto.Id,
    //        CreatorId = dto.CreatorId,
    //        CreatorUsername = dto.CreatorUsername,
    //        CreatorName = dto.CreatorName,
    //        CreationTime = dto.CreationTime,
    //        LastModifierId = dto.LastModifierId,
    //        LastModifierUsername = dto.LastModifierUsername,
    //        LastModifierName = dto.LastModifierName,
    //        LastModificationTime = dto.LastModificationTime,
    //    };
    //}

    //public static BuyerListDto MapToListDto(BuyerDto dto)
    //{
    //    return new BuyerListDto()
    //    {
    //        Id = dto.Id,
    //        Code = dto.Code,
    //        Name = dto.Name,
    //        Address = dto.Address,
    //        ContactInfo = dto.ContactInfo,
    //        PriceColumn = dto.PriceColumn,
    //        Note = dto.Note,
    //        IsDeactive = dto.IsDeactive,
    //    };
    //}

    //public static KeyAccountUpgradeDto MapToListDto(KeyAccountUpgradeDto dto)
    //{
    //    return new KeyAccountUpgradeDto()
    //    {
    //        Id = dto.Id,
    //        KeyAccountClassCode = dto.KeyAccountClassCode,
    //        KeyAccountName = dto.KeyAccountName,
    //        KeyAccountClassName = dto.KeyAccountClassName,
    //        KeyAccountCode = dto.KeyAccountCode,
    //        ClassSuggestion = dto.ClassSuggestion,
    //        Buyer = dto.Buyer,
    //        KeyAccountTypeCode = dto.KeyAccountTypeCode,
    //        KeyAccountTypeId = dto.KeyAccountTypeId,
    //        KeyAccountTypeName = dto.KeyAccountTypeName,
    //        MaterialType = dto.MaterialType,
    //        Revenue = dto.Revenue,
    //        SalePIC = dto.SalePIC,
    //        ClassificationValue = dto.ClassificationValue
    //    };
    //}

    //public static PriceOfferReportDetailDto MapToListDto(PriceOfferReportDetailDto item)
    //{
    //    return new PriceOfferReportDetailDto()
    //    {
    //        RowNo = item.RowNo,
    //        PriceOffer_Code = item.PriceOffer_Code,
    //        PriceOffer_Name = item.PriceOffer_Name,
    //        Status = item.Status,
    //        GolfaCode = item.GolfaCode,
    //        Model = item.Model,
    //        PriceOffer_Type = item.PriceOffer_Type,
    //        EUTypeBusiness = item.EUTypeBusiness,
    //        Created = item.Created,
    //        Competitor = item.Competitor,
    //        CloseDate = item.CloseDate,
    //        PanelSI = item.PanelSI,
    //        GP = item.GP,
    //        EULocation = item.EULocation,
    //        Qty = item.Qty,
    //        StandardPrice = item.StandardPrice,
    //        StandardPriceAmount = item.StandardPriceAmount,
    //        SaleOfferPrice = item.SaleOfferPrice,
    //        SaleOfferPriceAmount = item.SaleOfferPriceAmount,
    //        DiscountRatio = item.DiscountRatio,
    //        OfferMargin = item.OfferMargin,
    //        PriceOffer_Location = item.PriceOffer_Location,
    //        DPO_Qty = item.DPO_Qty,
    //        DPO_Amount = item.DPO_Amount,
    //        DeliveredQty = item.DeliveredQty,
    //        DeliveredAmount = item.DeliveredAmount,
    //        Buyer = item.Buyer,
    //        Material_Group = item.Material_Group
    //    };
    //}

    //public static PriceOfferReportGeneralDto MapToListDto(PriceOfferReportGeneralDto item)
    //{
    //    return new PriceOfferReportGeneralDto()
    //    {
    //        RowNo = item.RowNo,
    //        PriceOffer_Code = item.PriceOffer_Code,
    //        PriceOffer_Name = item.PriceOffer_Name,
    //        PriceOffer_Type = item.PriceOffer_Type,
    //        EUTypeBusiness = item.EUTypeBusiness,
    //        Competitor = item.Competitor,
    //        Created = item.Created,
    //        CloseDate = item.CloseDate,
    //        TaxCode = item.TaxCode,
    //        MaterialType = item.MaterialType,
    //        CustomerName = item.CustomerName,
    //        TotalStandardPrice = item.TotalStandardPrice,
    //        TotalOfferPrice = item.TotalOfferPrice,
    //        TotalOrdered = item.TotalOrdered,
    //        DeliveredAmount = item.DeliveredAmount,
    //        DiscountRatio = item.DiscountRatio,
    //        OrderRatio = item.OrderRatio,
    //        Buyer = item.Buyer,
    //        WarningDate = item.WarningDate,
    //        Status = item.Status
    //    };
    //}


}

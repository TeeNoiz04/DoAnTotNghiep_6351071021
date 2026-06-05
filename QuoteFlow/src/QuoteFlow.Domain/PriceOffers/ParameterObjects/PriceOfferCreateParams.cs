using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.PriceOffers.ParameterObjects;

public class PriceOfferCreateParams : IApprovalRouteAuditedObject
{
    // optionally set the Id
    public Guid? Id { get; set; }

    [Required]
    [StringLength(PriceOfferConsts.PriceOfferCodeMaxLength)]
    public string PriceOfferCode { get; set; }

    [Required]
    public Guid? BuyerId { get; set; }

    [Required]
    public Guid? BuyerTypeId { get; set; }

    [Required]
    [StringLength(PriceOfferConsts.MaterialTypeMaxLength)]
    public string MaterialType { get; set; }

    public Guid? LocationId { get; set; }

    [StringLength(PriceOfferConsts.LocationOldMaxLength)]
    public string? LocationOld { get; set; }

    [StringLength(PriceOfferConsts.ProjectNameMaxLength)]
    public string? ProjectName { get; set; }

    public Guid? ProjectTypeId { get; set; }

    public Guid? EUIndustryId { get; set; }

    [StringLength(PriceOfferConsts.ApplicationMaxLength)]
    public string? Application { get; set; }

    [StringLength(PriceOfferConsts.CountryMaxLength)]
    public string? Country { get; set; }

    [StringLength(PriceOfferConsts.ProvinceMaxLength)]
    public string? Province { get; set; }

    [StringLength(PriceOfferConsts.DetailedAddressMaxLength)]
    public string? DetailedAddress { get; set; }

    [StringLength(PriceOfferConsts.CompetitorBrandMaxLength)]
    public string? CompetitorBrand { get; set; }

    [StringLength(PriceOfferConsts.PriceGapWithCompetitorMaxLength)]
    public string? PriceGapWithCompetitor { get; set; }

    [StringLength(PriceOfferConsts.DecisionRightMaxLength)]
    public string? DecisionRight { get; set; }

    public DateTime? POPlannedDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    [StringLength(PriceOfferConsts.UpcomingPotentialProjectsMaxLength)]
    public string? UpcomingPotentialProjects { get; set; }

    [StringLength(PriceOfferConsts.OtherPJInformationMaxLength)]
    public string? OtherPJInformation { get; set; }

    [StringLength(PriceOfferConsts.FileNameMaxLength)]
    public string? FileName { get; set; }

    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }

    public DateTime? CloseDate { get; set; }

    public decimal? TotalMEVNOfferAmount { get; set; }

    [StringLength(PriceOfferConsts.AccountNoMaxLength)]
    public string? AccountNo { get; set; }

    public Guid? KeyAccountId { get; set; }

    public Guid? KeyAccountTypeId { get; set; }

    public Guid? KeyAccountClassId { get; set; }

    [StringLength(PriceOfferConsts.BuyerTypeDescriptionMaxLength)]
    public string? BuyerTypeDescription { get; set; }

    [StringLength(PriceOfferConsts.ProjectTypeDescriptionMaxLength)]
    public string? ProjectTypeDescription { get; set; }

    [StringLength(PriceOfferConsts.EUIndustryDescriptionMaxLength)]
    public string? EUIndustryDescription { get; set; }

    [StringLength(PriceOfferConsts.KeyAccountClassDescriptionMaxLength)]
    public string? KeyAccountClassDescription { get; set; }

    [StringLength(PriceOfferConsts.KeyAccountTypeDescriptionMaxLength)]
    public string? KeyAccountTypeDescription { get; set; }

    [StringLength(PriceOfferConsts.LocationDescriptionMaxLength)]
    public string? LocationDescription { get; set; }

    [StringLength(PriceOfferConsts.BuyerCodeMaxLength)]
    public string? BuyerCode { get; set; }

    public DateTime? LastApprovalRouteCreationTime { get; set; }
    public string? LastApprovalRouteCreatorName { get; set; }
    public string? LastApprovalRouteCreatorUsername { get; set; }
    public Guid? LastApprovalRouteCreatorId { get; set; }

    public ICollection<PriceOfferCustomerCreateParams>? Customers { get; set; }

    public ICollection<PriceOfferDetailCreateParams>? Details { get; set; }

    protected PriceOfferCreateParams()
    {
        // For deserialization purposes only
        PriceOfferCode = string.Empty;
        MaterialType = string.Empty;
    }

    public PriceOfferCreateParams(
        string priceOfferCode,
        Guid buyerId,
        Guid buyerTypeId,
        string materialType,
        Guid? locationId = null,
        string? projectName = null,
        Guid? projectTypeId = null,
        Guid? euIndustryId = null,
        string? application = null,
        string? country = null,
        string? province = null,
        string? detailedAddress = null,
        string? competitorBrand = null,
        string? priceGapWithCompetitor = null,
        string? decisionRight = null,
        DateTime? poPlannedDate = null,
        DateTime? deliveryDate = null,
        string? upcomingPotentialProjects = null,
        string? otherPJInformation = null,
        string? fileName = null,
        decimal? totalAmount = null,
        string? accountNo = null,
        ICollection<PriceOfferCustomerCreateParams>? customers = null,
        ICollection<PriceOfferDetailCreateParams>? details = null,
        string? note = null,
        DateTime? closeDate = null,
        Guid? keyAccountId = null,
        Guid? keyAccountTypeId = null,
        Guid? keyAccountClassId = null,
        string? projectTypeDescription = null,
        string? euIndustryDescription = null,
        string? keyAccountClassDescription = null,
        string? keyAccountTypeDescription = null,
        string? locationDescription = null)
    {
        PriceOfferCode = priceOfferCode;
        BuyerId = buyerId;
        BuyerTypeId = buyerTypeId;
        MaterialType = materialType;
        LocationId = locationId;
        ProjectName = projectName;
        ProjectTypeId = projectTypeId;
        EUIndustryId = euIndustryId;
        Application = application;
        Country = country;
        Province = province;
        DetailedAddress = detailedAddress;
        CompetitorBrand = competitorBrand;
        PriceGapWithCompetitor = priceGapWithCompetitor;
        DecisionRight = decisionRight;
        POPlannedDate = poPlannedDate;
        DeliveryDate = deliveryDate;
        UpcomingPotentialProjects = upcomingPotentialProjects;
        OtherPJInformation = otherPJInformation;
        FileName = fileName;
        Note = note;
        CloseDate = closeDate;
        TotalMEVNOfferAmount = totalAmount;
        AccountNo = accountNo;
        KeyAccountId = keyAccountId;
        KeyAccountTypeId = keyAccountTypeId;
        KeyAccountClassId = keyAccountClassId;
        ProjectTypeDescription = projectTypeDescription;
        EUIndustryDescription = euIndustryDescription;
        KeyAccountClassDescription = keyAccountClassDescription;
        KeyAccountTypeDescription = keyAccountTypeDescription;
        LocationDescription = locationDescription;

        Customers = customers;
        Details = details;
    }

    public PriceOfferCreateParams(
        string priceOfferCode,
        Guid? buyerId,
        Guid? buyerTypeId,
        string materialType,
        Guid? locationId = null,
        string? projectName = null,
        Guid? projectTypeId = null,
        Guid? euIndustryId = null,
        string? application = null,
        string? country = null,
        string? province = null,
        string? detailedAddress = null,
        string? competitorBrand = null,
        string? priceGapWithCompetitor = null,
        string? decisionRight = null,
        DateTime? poPlannedDate = null,
        DateTime? deliveryDate = null,
        string? upcomingPotentialProjects = null,
        string? otherPJInformation = null,
        string? fileName = null,
        decimal? totalAmount = null,
        string? accountNo = null,
        ICollection<PriceOfferCustomerCreateParams>? customers = null,
        ICollection<PriceOfferDetailCreateParams>? details = null,
        string? note = null,
        DateTime? closeDate = null,
        Guid? keyAccountId = null,
        Guid? keyAccountTypeId = null,
        Guid? keyAccountClassId = null,
        string? projectTypeDescription = null,
        string? euIndustryDescription = null,
        string? keyAccountClassDescription = null,
        string? keyAccountTypeDescription = null,
        string? locationDescription = null)
    {
        PriceOfferCode = priceOfferCode;
        BuyerId = buyerId;
        BuyerTypeId = buyerTypeId;
        MaterialType = materialType;
        LocationId = locationId;
        ProjectName = projectName;
        ProjectTypeId = projectTypeId;
        EUIndustryId = euIndustryId;
        Application = application;
        Country = country;
        Province = province;
        DetailedAddress = detailedAddress;
        CompetitorBrand = competitorBrand;
        PriceGapWithCompetitor = priceGapWithCompetitor;
        DecisionRight = decisionRight;
        POPlannedDate = poPlannedDate;
        DeliveryDate = deliveryDate;
        UpcomingPotentialProjects = upcomingPotentialProjects;
        OtherPJInformation = otherPJInformation;
        FileName = fileName;
        Note = note;
        CloseDate = closeDate;
        TotalMEVNOfferAmount = totalAmount;
        AccountNo = accountNo;
        KeyAccountId = keyAccountId;
        KeyAccountTypeId = keyAccountTypeId;
        KeyAccountClassId = keyAccountClassId;
        ProjectTypeDescription = projectTypeDescription;
        EUIndustryDescription = euIndustryDescription;
        KeyAccountClassDescription = keyAccountClassDescription;
        KeyAccountTypeDescription = keyAccountTypeDescription;
        LocationDescription = locationDescription;

        Customers = customers;
        Details = details;
    }
}

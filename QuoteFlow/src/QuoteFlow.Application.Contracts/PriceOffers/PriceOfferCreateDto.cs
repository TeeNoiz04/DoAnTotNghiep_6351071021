using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.PriceOffers;

public class PriceOfferCreateDto
{
    [Required]
    [StringLength(PriceOfferConsts.PriceOfferCodeMaxLength)]
    public string PriceOfferCode { get; set; } = null!;
    [Required]
    public Guid BuyerId { get; set; }
    [Required]
    public Guid BuyerTypeId { get; set; }
    [Required]
    [StringLength(PriceOfferConsts.MaterialTypeMaxLength)]
    public string MaterialType { get; set; } = null!;
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
    public string? UpcomingPotentialProjects { get; set; }
    public string? OtherPJInformation { get; set; }
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public DateTime? CloseDate { get; set; }
    public decimal? TotalMEVNOfferAmount { get; set; }
    [StringLength(PriceOfferConsts.AccountNoMaxLength)]
    public string? AccountNo { get; set; }
    public Guid? KeyAccountId { get; set; }
    public Guid? KeyAccountTypeId { get; set; }
    public Guid? KeyAccountClassId { get; set; }

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
}
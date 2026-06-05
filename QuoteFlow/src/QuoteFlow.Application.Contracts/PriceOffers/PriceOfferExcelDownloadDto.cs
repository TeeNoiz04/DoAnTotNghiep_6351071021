using System;

namespace QuoteFlow.PriceOffers;

public class PriceOfferExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;

    public string? FilterText { get; set; }

    public string? PriceOfferCode { get; set; }
    public Guid? BuyerId { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public string? MaterialType { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationOld { get; set; }
    public string? ProjectName { get; set; }
    public Guid? ProjectTypeId { get; set; }
    public Guid? EUIndustryId { get; set; }
    public string? Application { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? DetailedAddress { get; set; }
    public string? CompetitorBrand { get; set; }
    public string? PriceGapWithCompetitor { get; set; }
    public string? DecisionRight { get; set; }
    public DateTime? POPlannedDateMin { get; set; }
    public DateTime? POPlannedDateMax { get; set; }
    public DateTime? DeliveryDateMin { get; set; }
    public DateTime? DeliveryDateMax { get; set; }
    public string? UpcomingPotentialProjects { get; set; }
    public string? OtherPJInformation { get; set; }
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public DateTime? CloseDateMin { get; set; }
    public DateTime? CloseDateMax { get; set; }
    public decimal? TotalAmountMin { get; set; }
    public decimal? TotalAmountMax { get; set; }
    public string? ApprovalStatus { get; set; }
    public string? ProjectResultStatus { get; set; }
    public string? AccountNo { get; set; }
    public Guid? KeyAccountId { get; set; }
    public Guid? KeyAccountTypeId { get; set; }
    public Guid? KeyAccountClassId { get; set; }

    public PriceOfferExcelDownloadDto()
    {

    }
}
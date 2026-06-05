using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.PriceOffers;

public class GetPriceOffersInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public string? PriceOfferType { get; set; }
    public string? MaterialType { get; set; }
    public Guid? BuyerId { get; set; }
    public string? PriceOfferCode { get; set; }
    public string? CustomerTaxCode { get; set; }
    public string? CustomerName { get; set; }
    public string? ApprovalStatus { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public bool RelatedToMe { get; set; } = false;
    public string? ProjectName { get; set; }
    public string? ProjectResultStatus { get; set; }

    //public Guid? ProjectTypeId { get; set; }

    //public Guid? LocationId { get; set; }
    //public string? LocationOld { get; set; }
    //public Guid? EUIndustryId { get; set; }
    //public string? Application { get; set; }
    //public string? Country { get; set; }
    //public string? Province { get; set; }
    //public string? DetailedAddress { get; set; }
    //public string? CompetitorBrand { get; set; }
    //public string? PriceGapWithCompetitor { get; set; }
    //public string? DecisionRight { get; set; }
    //public DateTime? POPlannedDateMin { get; set; }
    //public DateTime? POPlannedDateMax { get; set; }
    //public DateTime? DeliveryDateMin { get; set; }
    //public DateTime? DeliveryDateMax { get; set; }
    //public string? UpcomingPotentialProjects { get; set; }
    //public string? OtherPJInformation { get; set; }
    //public string? FileName { get; set; }
    //public string? Note { get; set; }
    //public DateTime? CloseDateMin { get; set; }
    //public DateTime? CloseDateMax { get; set; }
    //public decimal? TotalAmountMin { get; set; }
    //public decimal? TotalAmountMax { get; set; }

    //public string? AccountNo { get; set; }
    //public Guid? KeyAccountId { get; set; }
    //public Guid? KeyAccountTypeId { get; set; }
    //public Guid? KeyAccountClassId { get; set; }
    public GetPriceOffersInput()
    {

    }
}
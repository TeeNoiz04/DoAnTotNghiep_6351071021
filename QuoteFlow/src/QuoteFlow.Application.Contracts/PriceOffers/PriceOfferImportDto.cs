using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Excels;
using System;

namespace QuoteFlow.PriceOffers;

public class PriceOfferImportDto
{
    // Manually calculated
    public string? PriceOfferCode { get; set; } = null!;
    public string? FileName { get; set; }
    public decimal? TotalMEVNOfferAmount { get; set; }
    public decimal? TotalPriceToCustomer { get; set; }
    public decimal? TotalRequestedAmount { get; set; }
    public decimal? TotalStandardAmount { get; set; }
    public virtual decimal? DiscountRatio { get; set; }
    public virtual decimal? DiscountRatioConfigured { get; set; }

    // Select from dropdown before import
    public Guid? BuyerId { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public Guid LocationId { get; set; }
    public string? AccountNo { get; set; }
    public string? Note { get; set; }
    public DateTime CloseDate { get; set; }
    public Guid? KeyAccountId { get; set; }
    public Guid? KeyAccountTypeId { get; set; }
    public Guid? KeyAccountClassId { get; set; }

    // In Excel file
    public string? MaterialType { get; set; } = null!;
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
    public DateTime? POPlannedDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? UpcomingPotentialProjects { get; set; }
    public string? OtherPJInformation { get; set; }

    // Deserialized from Excel file
    public string? ProjectTypeDescription { get; set; } = null!;
    public string? EUIndustryDescription { get; set; } = null!;

    // Sub-entities
    public ExcelValidationResult<PriceOfferDetailImportDto>? Details { get; set; }
    public ExcelValidationResult<PriceOfferCustomerImportDto>? Customers { get; set; }
}

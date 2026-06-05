using QuoteFlow.Shared;
using System;

namespace QuoteFlow.PriceOffers;

public class PriceOfferListDto : ExtendedAuditedEntityDto<Guid>
{
    public string? ApprovalStatus { get; set; }
    public string? ProjectResultStatus { get; set; }
    public string PriceOfferCode { get; set; } = null!;
    public Guid BuyerId { get; set; }
    public Guid BuyerTypeId { get; set; }
    public string? ProjectName { get; set; }
    public decimal? TotalMEVNOfferAmount { get; set; }
    public decimal? TotalDpoUsedAmount { get; set; }
    public decimal? TotalStandardAmount { get; set; }
    public Guid? CurrentApprovalRouteInstanceId { get; set; }
    public string? CurrentApprovalStepSequence { get; set; }
    public string? CurrentApproverRoleName { get; set; }

    public PriceOfferFlagsDto Flags { get; set; } = null!;
}
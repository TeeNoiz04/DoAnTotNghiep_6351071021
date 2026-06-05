using QuoteFlow.ApprovalHistories;
using QuoteFlow.Attachments;
using QuoteFlow.Buyers;
using QuoteFlow.KeyAccounts;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.PriceOffers;

public class PriceOfferDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string PriceOfferCode { get; set; } = null!;
    public Guid BuyerId { get; set; }
    public virtual string? BuyerCode { get; set; }
    public Guid BuyerTypeId { get; set; }
    public virtual string? BuyerTypeDescription { get; set; }
    public string MaterialType { get; set; } = null!;
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
    public DateTime? POPlannedDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? UpcomingPotentialProjects { get; set; }
    public string? OtherPJInformation { get; set; }
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public DateTime? CloseDate { get; set; }
    public decimal? TotalMEVNOfferAmount { get; set; }
    public string? ApprovalStatus { get; set; }
    public Guid? KeyAccountId { get; set; }
    public Guid? KeyAccountTypeId { get; set; }
    public Guid? KeyAccountClassId { get; set; }
    public Guid? CurrentApprovalRouteInstanceId { get; set; }
    public string? CurrentApprovalStepSequence { get; set; }
    public string? CurrentApproverRoleName { get; set; }
    public string? ProjectResultStatus { get; set; }
    public string? ProjectResultNote { get; set; }
    public DateTime? ProjectResultSubmittedAt { get; set; }
    public Guid? ProjectResultSubmitterId { get; set; }
    public string? ProjectResultSubmitterUsername { get; set; }
    public string? ProjectResultSubmitterFullName { get; set; }

    public string? ProjectTypeDescription { get; set; }
    public string? EUIndustryDescription { get; set; }
    public string? KeyAccountClassDescription { get; set; }
    public string? KeyAccountTypeDescription { get; set; }
    public string? LocationDescription { get; set; }

    public decimal? TotalUsableOfferAmount { get; set; }
    public decimal? TotalDpoUsedPercentage { get; set; }
    public decimal? TotalDpoUsedAmount { get; set; }
    public virtual decimal TotalStandardAmount { get; set; }
    public virtual decimal TotalRequestedAmount { get; set; }
    public virtual decimal TotalPriceToCustomer { get; set; }
    public virtual decimal TotalRequestedDiscountAmount { get; set; }
    public virtual decimal TotalLandedCost { get; set; }
    public virtual decimal TotalGP { get; set; }

    public virtual decimal? DiscountRatio { get; set; }
    public virtual decimal? DiscountRatioConfigured { get; set; }
    public virtual int? TotalMarginIssues { get; set; }

    public virtual string? AccountNo { get; set; }
    public virtual Guid? SpecialInputPriceId { get; set; }
    public virtual string? SpecialInputPriceAssignmentNote { get; set; }
    public virtual string? SpecialInputPriceAccountName { get; set; }
    public virtual DateTime? SpecialInputPriceAssignedTime { get; set; }
    public virtual Guid? SpecialInputPriceAssignerId { get; set; }
    public virtual string? SpecialInputPriceAssignerUsername { get; set; }
    public virtual string? SpecialInputPriceAssignerFullName { get; set; }
    public virtual decimal InitialTotalMEVNOfferAmount { get; set; }
    public virtual bool HasDPOUsed { get; set; } = false;

    public string ConcurrencyStamp { get; set; } = null!;

    public BuyerDto? Buyer { get; set; } = null!;
    public KeyAccountListDto? KeyAccount { get; set; } = null!;
    public ICollection<PriceOfferCustomerDto>? Customers { get; set; }
    public ICollection<PriceOfferDetailDto>? Details { get; set; }
    public ICollection<ApprovalHistoryDto> ApprovalHistories { get; set; } = [];
    public ICollection<AttachmentDto> Attachments { get; set; } = [];
    public PriceOfferFlagsDto Flags { get; set; } = null!;
}
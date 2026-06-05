using JetBrains.Annotations;
using QuoteFlow.Buyers;
using QuoteFlow.KeyAccounts;
using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.Interfaces;
using QuoteFlow.Shared.Models;
using QuoteFlow.SpecialInputPrices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Volo.Abp;

namespace QuoteFlow.PriceOffers;

public class PriceOffer : ExtendedAuditedAggregateRoot<Guid>, IHasApprovalRoute, IApprovalRouteAuditedObject
{
    [NotNull]
    public virtual string PriceOfferCode { get; set; }

    public virtual Guid? BuyerId { get; set; }

    public virtual string? BuyerCode { get; set; }

    public virtual Guid? BuyerTypeId { get; set; }

    [NotNull]
    public virtual string MaterialType { get; set; }

    public virtual Guid? LocationId { get; set; }

    [CanBeNull]
    public virtual string? LocationOld { get; set; }

    [CanBeNull]
    public virtual string? ProjectName { get; set; }

    public virtual Guid? ProjectTypeId { get; set; }

    public virtual Guid? EUIndustryId { get; set; }

    [CanBeNull]
    public virtual string? Application { get; set; }

    [CanBeNull]
    public virtual string? Country { get; set; }

    [CanBeNull]
    public virtual string? Province { get; set; }

    [CanBeNull]
    public virtual string? DetailedAddress { get; set; }

    [CanBeNull]
    public virtual string? CompetitorBrand { get; set; }

    [CanBeNull]
    public virtual string? PriceGapWithCompetitor { get; set; }

    [CanBeNull]
    public virtual string? DecisionRight { get; set; }

    public virtual DateTime? POPlannedDate { get; set; }

    public virtual DateTime? DeliveryDate { get; set; }

    [CanBeNull]
    public virtual string? UpcomingPotentialProjects { get; set; }

    [CanBeNull]
    public virtual string? OtherPJInformation { get; set; }

    [CanBeNull]
    public virtual string? FileName { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public virtual DateTime? CloseDate { get; set; }

    public virtual decimal? TotalMEVNOfferAmount { get; set; }

    [CanBeNull]
    public virtual string? ApprovalStatus { get; set; }

    public virtual Guid? KeyAccountId { get; set; }

    public virtual Guid? KeyAccountTypeId { get; set; }

    public virtual Guid? KeyAccountClassId { get; set; }

    [CanBeNull]
    public Guid? CurrentApprovalRouteInstanceId { get; set; }

    [CanBeNull]
    public int? CurrentApprovalStepSequence { get; set; }

    [CanBeNull]
    public string? CurrentApproverRoleName { get; set; }

    [CanBeNull]
    public string? CurrentApproverRoleCode { get; set; }

    // Sales Outcome Properties
    [CanBeNull]
    public virtual string? ProjectResultStatus { get; set; } // "Won" or "Lost"

    [CanBeNull]
    public virtual string? ProjectResultNote { get; set; }
    public virtual DateTime? ProjectResultSubmittedAt { get; set; }

    public virtual Guid? ProjectResultSubmitterId { get; set; }

    [CanBeNull]
    public virtual string? ProjectResultSubmitterUsername { get; set; }

    [CanBeNull]
    public virtual string? ProjectResultSubmitterFullName { get; set; }


    // Denormalization fields for SystemCategory descriptions
    [CanBeNull]
    public virtual string? BuyerTypeDescription { get; set; }

    [CanBeNull]
    public virtual string? ProjectTypeDescription { get; set; }

    [CanBeNull]
    public virtual string? EUIndustryDescription { get; set; }

    [CanBeNull]
    public virtual string? KeyAccountClassDescription { get; set; }

    [CanBeNull]
    public virtual string? KeyAccountTypeDescription { get; set; }

    [CanBeNull]
    public virtual string? LocationDescription { get; set; }

    public virtual decimal TotalStandardAmount { get; set; }

    public virtual decimal TotalRequestedAmount { get; set; }

    public virtual decimal TotalPriceToCustomer { get; set; }

    [CanBeNull]
    public virtual decimal? DiscountRatio { get; set; }

    [CanBeNull]
    public virtual decimal? DiscountRatioConfigured { get; set; }

    [CanBeNull]
    public virtual int? TotalMarginIssues { get; set; }

    [CanBeNull]
    public virtual string? AccountNo { get; set; }

    [CanBeNull]
    public virtual Guid? SpecialInputPriceId { get; set; }

    [CanBeNull]
    public virtual string? SpecialInputPriceAssignmentNote { get; set; }

    [CanBeNull]
    public virtual DateTime? SpecialInputPriceAssignedTime { get; set; }

    [CanBeNull]
    public virtual string? SpecialInputPriceAccountName { get; set; }

    [CanBeNull]
    public virtual Guid? SpecialInputPriceAssignerId { get; set; }

    [CanBeNull]
    public virtual string? SpecialInputPriceAssignerUsername { get; set; }

    [CanBeNull]
    public virtual string? SpecialInputPriceAssignerFullName { get; set; }

    public virtual decimal InitialTotalMEVNOfferAmount { get; set; }

    // DPO Usage Tracking
    public virtual bool HasDPOUsed { get; set; } = false;

    // Track approval route audit info
    public DateTime? LastApprovalRouteCreationTime { get; set; }
    public string? LastApprovalRouteCreatorName { get; set; }
    public string? LastApprovalRouteCreatorUsername { get; set; }
    public Guid? LastApprovalRouteCreatorId { get; set; }

    #region Calculated Properties
    public DateTime? SubmittedDate
    {
        get
        {
            return ApprovalHistories
                .Where(x => x.Action == HistoryActions.Submitted)
                .OrderByDescending(x => x.ActionDate)
                .Select(x => x.ActionDate)
                .FirstOrDefault();
        }
    }


    [NotMapped]
    public decimal TotalLandedCost => Details?.Where(x => x.Status != "MERGED" && x.Status != "REJECTED" && x.Status != "CANCELLED").Sum(d => (d.LandingCost ?? 0) * d.Qty) ?? 0;

    //[NotMapped]
    //public decimal TotalGP => TotalPriceToCustomer == 0 ? 0 :
    //        (1 - (TotalLandedCost / TotalPriceToCustomer));
    #endregion

    #region Navigation Properties
    public virtual Buyer? Buyer { get; set; }
    public virtual KeyAccount? KeyAccount { get; set; }
    public virtual ICollection<PriceOfferCustomer> Customers { get; set; } = [];
    public virtual ICollection<PriceOfferDetail> Details { get; set; } = [];
    public virtual ICollection<PriceOfferApprovalHistory> ApprovalHistories { get; set; } = [];
    public virtual ICollection<PriceOfferApprovalRoute> ApprovalRoutes { get; set; } = [];
    public virtual ICollection<PriceOfferAttachment> Attachments { get; set; } = [];
    public virtual ICollection<PriceOfferMessage> Messages { get; set; } = [];
    #endregion

    protected PriceOffer()
    {
        PriceOfferCode = string.Empty;
        MaterialType = string.Empty;
    }

    public PriceOffer(Guid id, PriceOfferCreateParams createParams)
    {
        Id = id;
        PriceOfferCode = createParams.PriceOfferCode;
        BuyerId = createParams.BuyerId;
        BuyerCode = createParams.BuyerCode;
        BuyerTypeId = createParams.BuyerTypeId;
        BuyerTypeDescription = createParams.BuyerTypeDescription;
        MaterialType = createParams.MaterialType;
        LocationId = createParams.LocationId;
        LocationOld = createParams.LocationOld;
        ProjectName = createParams.ProjectName;
        ProjectTypeId = createParams.ProjectTypeId;
        EUIndustryId = createParams.EUIndustryId;
        Application = createParams.Application;
        Country = createParams.Country;
        Province = createParams.Province;
        DetailedAddress = createParams.DetailedAddress;
        CompetitorBrand = createParams.CompetitorBrand;
        PriceGapWithCompetitor = createParams.PriceGapWithCompetitor;
        DecisionRight = createParams.DecisionRight;
        POPlannedDate = createParams.POPlannedDate;
        DeliveryDate = createParams.DeliveryDate;
        UpcomingPotentialProjects = createParams.UpcomingPotentialProjects;
        OtherPJInformation = createParams.OtherPJInformation;
        FileName = createParams.FileName;
        Note = createParams.Note;
        CloseDate = createParams.CloseDate;
        TotalMEVNOfferAmount = createParams.TotalMEVNOfferAmount;
        AccountNo = createParams.AccountNo;
        KeyAccountId = createParams.KeyAccountId;
        KeyAccountTypeId = createParams.KeyAccountTypeId;
        KeyAccountClassId = createParams.KeyAccountClassId != Guid.Empty ? createParams.KeyAccountClassId : null;
        ProjectTypeDescription = createParams.ProjectTypeDescription;
        EUIndustryDescription = createParams.EUIndustryDescription;
        KeyAccountClassDescription = createParams.KeyAccountClassId != Guid.Empty ? createParams.KeyAccountClassDescription : "N/A";
        KeyAccountTypeDescription = createParams.KeyAccountTypeDescription;
        LocationDescription = createParams.LocationDescription;

        ApprovalStatus = QuoteFlowStatuses.Verifying;
        ProjectResultStatus = null;
        HasDPOUsed = false;
    }

    public void Submit()
    {
        if (!IsDraft() && !IsCancelled() && !IsVerifying() && !IsRejected())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.InvalidStatusForSubmission);
        }

        if (CloseDate > DateTime.Now.Date.AddYears(1))
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.PriceOfferCloseDateMustBeWithinOneYear);
        }

        ApprovalStatus = QuoteFlowStatuses.InProgress;

        foreach (var detail in Details)
        {
            if (detail.IsDraft() || detail.IsCancelled() || detail.IsVerifying() || detail.IsRejected())
            {
                detail.Submit();
            }
        }
    }

    public void SubmitMoreItems(Guid importGuid)
    {
        if (!IsApproved())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.PriceOfferNotApproved)
                .WithData("priceOfferId", Id);
        }

        //if (!IsApproved() && !IsWon() && !IsPreOrder() && IsProjectPriceOffer())
        //{
        //    throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ProjectPriceOfferNotWon)
        //        .WithData("priceOfferId", Id);
        //}

        ApprovalStatus = QuoteFlowStatuses.InProgress;

        foreach (var detail in Details.Where(d => d.ImportGuid == importGuid))
        {
            detail.Status = QuoteFlowStatuses.InProgress;
        }
    }

    public void Approve(DateTime actionDate, string? note = null, bool isLastStep = false)
    {
        if (!IsInProgress())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeApproved);
        }

        if (isLastStep)
        {
            if (GetPriceOfferType() == PriceOfferTypes.PriceOfferPP && IsAwaitingInitialApproval())
            {
                ProjectResultStatus = QuoteFlowStatuses.PriceOffer.Pending;
            }

            ApprovalStatus = QuoteFlowStatuses.Approved;
        }
        else
        {
            ApprovalStatus = QuoteFlowStatuses.InProgress;
        }


        foreach (var detail in Details)
        {
            if (detail.IsInProgress())
            {
                detail.Approve(isLastStep);
            }
        }

        var latestUnapprovedStep = GetLatestUnapprovedStep();
        foreach (var route in ApprovalRoutes.Where(x => x.StepSequence <= latestUnapprovedStep.StepSequence && !x.IsApproved))
        {
            route.Approve(actionDate, note);
        }
    }

    public void Reject()
    {
        if (!IsInProgress())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeRejected);
        }

        if (IsAwaitingInitialApproval()) // if is first approval
        {
            ApprovalStatus = QuoteFlowStatuses.Rejected;
        }
        else
        {
            ApprovalStatus = QuoteFlowStatuses.Approved;
        }

        foreach (var detail in Details)
        {
            if (detail.IsInProgress())
            {
                detail.Reject();
            }
        }
    }

    public void Cancel()
    {
        if (!IsInProgress())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeCancelled);
        }

        if (IsAwaitingInitialApproval()) // if is first approval
        {
            ApprovalStatus = QuoteFlowStatuses.Cancelled;
        }
        else
        {
            ApprovalStatus = QuoteFlowStatuses.Approved;
        }

        foreach (var detail in Details)
        {
            if (detail.IsInProgress())
            {
                detail.Cancel();
            }
        }
    }

    public void Close()
    {
        if (!IsApproved())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.OnlyApprovedCanBeClosed);
        }

        this.SetCurrentRoute(null);

        ApprovalStatus = QuoteFlowStatuses.Closed;

        foreach (var detail in Details)
        {
            if (detail.IsApproved())
            {
                detail.Close();
            }
        }
    }

    public void RecordAction(PriceOfferApprovalHistory history)
    {
        ApprovalHistories ??= [];
        ApprovalHistories.Add(history);
    }

    public void RecordMessage(PriceOfferMessage message)
    {
        Messages ??= [];
        Messages.Add(message);
    }

    public PriceOfferApprovalRoute GetLatestUnapprovedStep()
    {
        if (ApprovalRoutes == null || ApprovalRoutes.Count == 0)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.NoApprovalRouteFound)
                .WithData("entityId", Id);
        }

        var latestUnapproved = ApprovalRoutes
            .Where(x => !x.IsApproved)
            .MinBy(x => x.StepSequence)
            ?? throw new BusinessException(QuoteFlowDomainErrorCodes.NoUnapprovedStepFound)
                .WithData("entityId", Id);

        return latestUnapproved;
    }

    public string GetPriceOfferType()
    {
        var result = PriceOfferCode[..2].ToUpperInvariant();

        var validTypes = PriceOfferTypes.AllTypes;
        if (!validTypes.Contains(result))
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.InvalidPriceOfferType)
                .WithData("expectedTypes", string.Join(", ", validTypes))
                .WithData("actualType", result);
        }

        return result;
    }

    public bool IsAwaitingInitialApproval() => Details.All(x => x.IsInProgress());

    public bool IsDraft() => ApprovalStatus == QuoteFlowStatuses.Draft;
    public bool IsCancelled() => ApprovalStatus == QuoteFlowStatuses.Cancelled;
    public bool IsVerifying() => ApprovalStatus == QuoteFlowStatuses.Verifying;
    public bool IsInProgress() => ApprovalStatus == QuoteFlowStatuses.InProgress;
    public bool IsApproved() => ApprovalStatus == QuoteFlowStatuses.Approved;
    public bool IsRejected() => ApprovalStatus == QuoteFlowStatuses.Rejected;

    public bool IsPendingForSales() => ProjectResultStatus == QuoteFlowStatuses.PriceOffer.Pending && IsProjectPriceOffer();
    public bool IsWon() => !IsProjectPriceOffer() || ProjectResultStatus == QuoteFlowStatuses.PriceOffer.Won;
    public bool IsLost() => ProjectResultStatus == QuoteFlowStatuses.PriceOffer.Lost;
    public bool IsPreOrder() => ProjectResultStatus == QuoteFlowStatuses.PriceOffer.PreOrder;
    public bool IsProjectPriceOffer() => GetPriceOfferType() == PriceOfferTypes.PriceOfferPP;
    public bool IsBuyerStockPriceOffer() => GetPriceOfferType() == PriceOfferTypes.PriceOfferDS;
    public bool IsKeyAccountPriceOffer() => GetPriceOfferType() == PriceOfferTypes.PriceOfferAP;
    public bool IsNoBuyerPriceOffer() => GetPriceOfferType() == PriceOfferTypes.PriceOfferNB;

    public void SubmitProjectResult(string resultStatus, string note, Guid submitterId, string submitterUsername, string submitterFullName, Dictionary<string, Guid>? winningCustomersByChannel = null)
    {
        // Validation: Only allow if already approved
        if (!IsApproved())
        {
            throw new UserFriendlyException("Price Offer must be Approved to use this feature");
        }

        // Check if sales outcome already submitted (except PreOrder can be re-confirmed)
        if (!string.IsNullOrEmpty(ProjectResultStatus) && !IsPendingForSales() && !IsPreOrder())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ProjectResultAlreadySubmitted);
        }

        // Validate result status
        var validStatuses = new[] { QuoteFlowStatuses.PriceOffer.Won, QuoteFlowStatuses.PriceOffer.PreOrder, QuoteFlowStatuses.PriceOffer.Lost };
        if (!validStatuses.Contains(resultStatus))
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.InvalidProjectResultStatus)
                .WithData("providedStatus", resultStatus)
                .WithData("validStatuses", string.Join(", ", validStatuses));
        }

        // Validate note is required
        if (string.IsNullOrWhiteSpace(note))
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ProjectResultNoteRequired);
        }

        if (resultStatus == QuoteFlowStatuses.PriceOffer.Won || resultStatus == QuoteFlowStatuses.PriceOffer.PreOrder)
        {
            // Validation: WinningCustomers is required for wins and pre-orders
            if (winningCustomersByChannel == null || !winningCustomersByChannel.Any())
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ProjectResultWinningCustomersRequired);
            }

            // Validate that each channel has only one customer per channel
            var customerChannels = Customers.GroupBy(c => c.SaleChannel).ToList();
            foreach (var channelGroup in customerChannels)
            {
                if (!winningCustomersByChannel.ContainsKey(channelGroup.Key))
                {
                    throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ProjectResultMissingWinningCustomerForChannel)
                        .WithData("channelName", channelGroup.Key);
                }
            }

            ProjectResultStatus = resultStatus;
        }
        else // Lost
        {
            // Validation: WinningCustomers must be null for losses
            if (winningCustomersByChannel != null && winningCustomersByChannel.Count != 0)
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ProjectResultNoWinningCustomersForLoss);
            }

            ProjectResultStatus = QuoteFlowStatuses.PriceOffer.Lost;
        }
        ProjectResultNote = note;
        ProjectResultSubmittedAt = DateTime.Now;
        ProjectResultSubmitterId = submitterId;
        ProjectResultSubmitterUsername = submitterUsername;
        ProjectResultSubmitterFullName = submitterFullName;
    }

    public void ConfirmPreOrderStatus(string resultStatus, string note, Guid submitterId, string submitterUsername, string submitterFullName)
    {
        // Validation: Only allow if current status is PRE_ORDER
        if (!IsPreOrder())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ProjectResultNotPreOrder)
                .WithData("currentStatus", ProjectResultStatus ?? "NULL");
        }

        // Validate result status - only WON or LOST allowed
        var validStatuses = new[] { QuoteFlowStatuses.PriceOffer.Won, QuoteFlowStatuses.PriceOffer.Lost };
        if (!validStatuses.Contains(resultStatus))
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.InvalidProjectResultStatus)
                .WithData("providedStatus", resultStatus)
                .WithData("validStatuses", string.Join(", ", validStatuses));
        }

        // Validate note is required
        if (string.IsNullOrWhiteSpace(note))
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ProjectResultNoteRequired);
        }

        // Update project result fields
        ProjectResultStatus = resultStatus;
        ProjectResultNote = note;
        ProjectResultSubmittedAt = DateTime.Now;
        ProjectResultSubmitterId = submitterId;
        ProjectResultSubmitterUsername = submitterUsername;
        ProjectResultSubmitterFullName = submitterFullName;
    }

    public void SetWinningCustomerForChannel(string channel, Guid customerId)
    {
        var channelCustomers = Customers.Where(c => c.SaleChannel == channel).ToList();

        foreach (var customer in channelCustomers)
        {
            if (customer.CustomerId == customerId)
            {
                // This is the winning customer - keep it active
                continue;
            }
            else
            {
                // This customer didn't win - soft delete it
                customer.IsDeleted = true;
                customer.DeletionTime = DateTime.UtcNow;
                customer.DeleterId = ProjectResultSubmitterId;
                customer.DeleterUsername = ProjectResultSubmitterUsername;
                customer.DeleterName = ProjectResultSubmitterFullName;
            }
        }
    }

    public void AssignSpecialInputPrice(SpecialInputPrice inputPrice, Guid assignerId, string assignerUsername, string assignerFullName, string? assignmentNote = null)
    {
        if (inputPrice == null)
        {
            throw new ArgumentNullException(nameof(inputPrice), "Special input price cannot be null.");
        }

        if (inputPrice.IsValidFor(DateTime.Now.Date))
        {
            // Assign the special input price
            SpecialInputPriceId = inputPrice.Id;
            AccountNo = inputPrice.AccountNo;
            SpecialInputPriceAccountName = inputPrice.AccountName;
            SpecialInputPriceAssignedTime = DateTime.Now;
            SpecialInputPriceAssignerId = assignerId;
            SpecialInputPriceAssignerUsername = assignerUsername;
            SpecialInputPriceAssignerFullName = assignerFullName;
            SpecialInputPriceAssignmentNote = assignmentNote;
            //// Update the total price to customer based on the special input price
            //TotalPriceToCustomer = inputPrice.CalculateTotalPrice(Details);
        }
        else
        {
            var validFrom = inputPrice.ValidFrom;
            var validTo = inputPrice.ValidTo;
            if (validFrom is null && validTo is null)
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.SpecialInputPriceNotValid);
            }
            else if (validFrom is null && validTo is not null)
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.SpecialInputPriceNotYetValid)
                    .WithData("specialInputPriceId", inputPrice.Id)
                    .WithData("validTo", validTo.Value.ToStandardString());
            }
            else if (validTo is null && validFrom is not null)
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.SpecialInputPriceExpired)
                    .WithData("specialInputPriceId", inputPrice.Id)
                    .WithData("validFrom", validFrom.Value.ToStandardString());
            }
        }

    }

    public List<Guid> MergeDetails(Guid newestImportGuid)
    {
        var approvedLookup = Details
        .Where(d => d.Status == QuoteFlowStatuses.Approved && d.ImportGuid != newestImportGuid)
        .ToDictionary(
            d => (d.GolfaCode, d.MEVNOfferPrice),
            d => d);

        var detailsToMerge = Details
            .Where(d => d.ImportGuid == newestImportGuid)
            .ToList();

        List<Guid> mergedIds = new List<Guid>();
        foreach (var detail in detailsToMerge)
        {
            if (approvedLookup.TryGetValue(
                (detail.GolfaCode, detail.MEVNOfferPrice),
                out var receiver))
            {
                receiver.Merge(detail);
                detail.MarkAsMerged();

                mergedIds.Add(receiver.Id);
            }
        }

        return mergedIds;
    }
    public void AddedAttachmentAction(PriceOfferAttachment attachment)
    {
        Attachments ??= [];

        Attachments.Add(attachment);
    }

    public void MarkDPOUsed()
    {
        if (!HasDPOUsed)
        {
            HasDPOUsed = true;
            InitialTotalMEVNOfferAmount = TotalMEVNOfferAmount ?? 0;
        }
    }
}
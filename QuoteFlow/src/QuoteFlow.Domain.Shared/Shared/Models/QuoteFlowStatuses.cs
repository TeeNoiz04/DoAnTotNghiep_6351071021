using System.Collections.Generic;

namespace QuoteFlow.Shared.Models;
public static class QuoteFlowStatuses
{
    public const string Verifying = "VERIFYING";
    public const string Draft = "DRAFT";
    public const string InProgress = "IN_PROGRESS";
    public const string Approved = "APPROVED";
    public const string Rejected = "REJECTED";
    public const string Cancelled = "CANCELLED";
    public const string Closed = "CLOSED";
    public const string SelfApproved = "SELF_APPROVED";

    public static class PriceOfferDetail
    {
        public const string Merged = "MERGED";
    }

    public static class PriceOffer
    {
        public const string Won = "WON";
        public const string Lost = "LOST";
        public const string PreOrder = "PRE_ORDER";
        public const string Pending = "PENDING";
    }

    public static class DPO
    {
        public const string Submitted = "SUBMITTED";
        public const string Confirmed = "CONFIRMED";
        public const string LockedStock = "LOCKED_STOCK";
    }

    public static class GIC
    {
        public const string Submitted = "SUBMITTED";
        public const string Confirmed = "CONFIRMED";
        public const string LockedStock = "LOCKED_STOCK";
    }

    public static class GKR
    {
        public const string Submitted = "SUBMITTED";
        public const string Confirmed = "CONFIRMED";
        public const string LockedStock = "LOCKED_STOCK";
    }


    public static class PSI
    {
        public const string Outdated = "OUTDATED";
    }

    public static class SpecialInputPrice
    {
        public const string Closed = "CLOSED";
        public const string Valid = "Valid";
        public const string Expired = "Expired";
        public const string Deactivated = "Deactive";
    }

    public static class AssetRequest
    {
        public const string WaitForReturn = "WAIT_FOR_RETURN";
        public const string Returning = "RETURNING";
        public const string ConfirmClose = "CONFIRM_CLOSE";
        public const string WaitForClose = "WAIT_FOR_CLOSE";
    }

    public static readonly HashSet<string> AllStatuses =
    [
        Draft,
        InProgress,
        Approved,
        Rejected,
        Cancelled,
        Closed,
        SelfApproved,
    ];
}

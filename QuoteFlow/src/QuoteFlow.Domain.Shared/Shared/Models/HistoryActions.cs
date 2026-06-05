namespace QuoteFlow.Shared.Models;

public static class HistoryActions
{
    public const string None = "None";
    public const string Submitted = "Submitted";
    public const string SubmittedLending = "SubmittedLending";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Cancelled = "Cancelled";
    public const string Closed = "Closed";
    public const string Deleted = "Deleted";
    //public const string 

    public static class PriceOffer
    {
        public const string Confirm = "Confirm";
        public const string SubmittedAsWin = "SubmittedAsWin";
        public const string SubmittedAsLoss = "SubmittedAsLoss";
        public const string SubmittedAsPreOrder = "SubmittedAsPreOrder";
        public const string ApprovedNotConfirm = "ApprovedNotConfirm";
        public const string LandingCostUpdated = "LandingCostUpdated";
        public const string SubmittedMoreItems = "SubmittedMoreItems";
        public const string SpecialInputPriceAssigned = "SpecialInputPriceAssigned";
    }

    public static class Material
    {
        public const string Active = "Active";
        public const string Deactive = "Deactive";
        public const string Discontinue = "Discontinue";
    }

    public static class PriceOfferDetail
    {
        public const string QuantityMerged = "QuantityMerged";
        public const string SpecialInputPriceAssigned = "SpecialInputPriceAssigned";
    }

    public static class Discussion
    {
        public const string MessageSent = "MessageSent";
    }

    public static class DPO
    {
        public const string Confirmed = "Confirmed";
        public const string ConfirmedLockStock = "ConfirmedLockStock";
        public const string ConfirmedLockOnOrder = "ConfirmedLockOnOrder";

        public const string AutoLockStock = "AutoLockStock";
        public const string AutoLockOnOrder = "AutoLockOnOrder";
        public const string ReleaseLockStock = "ReleaseLockStock";
        public const string ReleaseLockOnOrderStock = "ReleaseLockOnOrderStock";

        public const string Deleted = "Deleted";
        public const string Cancelled = "Cancelled";
        public const string Closed = "Closed";
    }

    public static class GIC
    {
        public const string ConfirmedLockStock = "ConfirmedLockStock";
        public const string ConfirmedLockOnOrder = "ConfirmedLockOnOrder";
        public const string ConfirmedNote = "ConfirmedNote";
        public const string AddedExtraFee = "AddedExtraFee";
        public const string CancelItems = "CancelItems";
        public const string LockedStock = "LockedStock";
        public const string LockedOnOrder = "LockedOnOrder";
        public const string Viewed = "Viewed";
    }
    public static class SaleOrder
    {
        public const string Repoen = "Reopen";
        public const string ImportSAPData = "ImportSAPData";
        public const string ImportIUChange = "ImportIUChange";
        public const string ConfirmedDelivery = "ConfirmedDelivery";
        public const string Update = "Update";
    }

    public static class AssetRequest
    {
        public const string Return = "Return";
        public const string Extend = "Extend";
        public const string ConfirmClose = "ConfirmClose";
    }
}

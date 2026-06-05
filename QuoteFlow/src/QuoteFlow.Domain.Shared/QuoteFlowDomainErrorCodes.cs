namespace QuoteFlow;

public static class QuoteFlowDomainErrorCodes
{
    /* You can add your business exception error codes here, as constants */
    // 101xxxxx
    // General error codes
    // Approval Workflow
    public const string InvalidApprovalAction = "QuoteFlow:10101001"; //  "You don't have approval permission for this {entityName} (ID: {entityId})."
    public const string NoApprovalRouteFound = "QuoteFlow:10104001"; //  "No approval route found for the entity. Entity id: {entityId}"
    public const string InvalidStatusForSubmission = "QuoteFlow:10103001"; //  "Only draft, cancelled, and rejected entity can be submitted."
    public const string OnlyInProgressCanBeRejected = "QuoteFlow:10103002"; //  "Only in-progress entity can be rejected."
    public const string OnlyInProgressCanBeApproved = "QuoteFlow:10103003"; //  "Only in-progress entity can be approved."
    public const string OnlyInProgressCanBeCancelled = "QuoteFlow:10103004"; //  "Only in-progress entity can be cancelled."
    public const string NoUnapprovedStepFound = "QuoteFlow:10104002"; //  "No unapproved approval step found for the current entity. All steps may have already been approved. Entity id: {entityId}"

    // System errors
    public const string InvalidEmailAddress = "QuoteFlow:11201001"; //  "Invalid e‑mail address: {email}"

    // 102xxxxx
    public static class SystemCategory
    {
        public const string SystemCategoryCodeExists = "QuoteFlow:10203001"; //  "System category code already exists."
    }

    // 103xxxxx
    public static class PSI
    {

    }

    // 104xxxxx
    public static class Cargo
    {

    }

    // 105xxxxx
    public static class StockTracing
    {
        public const string FileAlreadyExists = "QuoteFlow:10501001"; //  "A file named {fileName} already exists; please choose a different name."
    }


    // 106xxxxx
    public static class DPO
    {
        public const string DPONoAlreadyExists = "QuoteFlow:10603001"; //  "The DPO No '{dpoNo}' already exists."
        public const string OnlyConfirmedDPOCanBeLockedStock = "QuoteFlow:10603002"; //  "Only confirmed DPO can be locked stock."
        public const string OnlyLockedStockDPOCanBeInProgress = "QuoteFlow:10603003"; //  "Only locked stock DPO can be set to in progress."
        public const string InsufficientStockForUpdate = "QuoteFlow:10603004"; //  "Insufficient stock available. Available: {availableQty}, Required: {requiredQty}."
        public const string OnlySubmittedDPOCanBeApproved = "QuoteFlow:10603005"; //  "Only submitted DPO can be approved."
        public const string OnlySubmittedDPOCanBeRejected = "QuoteFlow:10603006"; //  "Only submitted DPO can be rejected."
        public const string ImportPriceMismatchWarning = "QuoteFlow:10603007"; //  "Row {rowNumbers} have price mismatch with the price in the system, are you sure you want to proceed?"
        public const string ImportDeactiveMaterialError = "QuoteFlow:10603008"; //  "Row {rowNumbers} Material been Discontinue, are you sure you want to proceed?"
    }


    // 107xxxxx
    public static class PriceOffer
    {
        public const string PriceOfferCloseDateMustBeWithinOneYear = "QuoteFlow:10701001"; //  "The close date of the price offer must be within one year from today."
        public const string ApprovalLevelExceededMessage = "QuoteFlow:10703001"; //  "This Price Offer will require Level 5 approval. Please confirm before proceeding."
        public const string ProjectResultRequiresApprovedStatus = "QuoteFlow:10703002"; //  "Project result can only be submitted for approved price offers."
        public const string ProjectResultAlreadySubmitted = "QuoteFlow:10703003"; //  "Project result has already been submitted for this price offer."
        public const string ProjectResultNoteRequired = "QuoteFlow:10703004"; //  "Note is required when submitting a project result."
        public const string ProjectResultWinningCustomersRequired = "QuoteFlow:10703005"; //  "Winning customers must be specified when submitting a winning project result."
        public const string ProjectResultMissingWinningCustomerForChannel = "QuoteFlow:10703006"; //  "Must select a winning customer for channel: {channelName}."
        public const string ProjectResultNoWinningCustomersForLoss = "QuoteFlow:10703007"; //  "Winning customers must not be specified when submitting a lost project result."
        public const string InvalidPriceOfferStatusForLandingCostUpdate = "QuoteFlow:10703008"; //  "Can only update these fields if the Price Offer is in progress and its approval process has not reached Level 3."
        public const string SpecialInputPriceNotValid = "QuoteFlow:10703009"; //  "The selected special input price is not valid."
        public const string SpecialInputPriceNotYetValid = "QuoteFlow:10703010"; //  "The special input price with ID {specialInputPriceId} is not yet valid. It will be valid starting from {validFrom}."
        public const string SpecialInputPriceExpired = "QuoteFlow:10703011"; //  "The special input price with ID {specialInputPriceId} has expired. It was valid until {validTo}."
        public const string ProjectPriceOfferNotWon = "QuoteFlow:10703012"; //  "The project price offer with ID {priceOfferId} has not been won or pre-order yet; adding more items is not allowed."
        public const string PriceOfferNotApproved = "QuoteFlow:10703013"; //  "The price offer with ID {priceOfferId} is not approved; adding more items is not allowed."
        public const string OnlyInProgressItemCanBeMerged = "QuoteFlow:10703014"; //  "Only in-progress items can be merged."
        public const string InvalidQtyToMerge = "QuoteFlow:10703015"; //  "Invalid quantity to merge. Existing quantity: {qty}, quantity being merged: {additionalQty}. The merge would drive the total below zero."
        public const string InvalidProjectResultStatus = "QuoteFlow:10703016"; //  "Invalid project result status."
        public const string CannotAddMoreItemsWhenDPOUsed = "QuoteFlow:10703017"; //  "Cannot add more items beyond the configured limit when DPO has been used. Reason: {reason}"
        public const string ProjectResultNotPreOrder = "QuoteFlow:10703018"; //  "Can only confirm pre-order status when the current project result status is Pre-order."
        public const string PriceOfferDetailCancelValidationError = "QuoteFlow:10703019"; //  "Validation error occurred while cancelling price offer details: {message}."
        public const string PriceOfferDetailsNotFound = "QuoteFlow:10703020"; //  "One or more price offer details not found or do not belong to the specified price offer."
        public const string InvalidPriceOfferType = "QuoteFlow:10704001"; //  "Invalid Price Offer type, expected {expectedTypes}, but got {actualType}."
        public const string InvalidDpoUsedQuantity = "QuoteFlow:10701002"; //  "Invalid DPO used quantity: {qty}. Quantity must be greater than zero."
        public const string DpoUsedExceedsAvailableQuantity = "QuoteFlow:10703021"; //  "DPO used quantity exceeds available quantity. Available: {availableQty}, Attempted to use: {qtyToAdd}.",
        public const string OnlyApprovedCanBeClosed = "QuoteFlow:10703022"; //  "Only approved Price Offer can be closed."
    }

    // 108xxxxx
    public static class Material
    {

    }

    // 109xxxxx
    public static class KeyAccount
    {

    }

    // 110xxxxx
    public static class Customer
    {

    }

    // 111xxxxx
    public static class Buyer
    {
        public const string BuyerCodeAlreadyExists = "QuoteFlow:11104001"; //  "The buyer code already exists."
    }

    // 112xxxxx
    public static class SalesTeam
    {
        public const string UserAlreadyInSaleTeam = "QuoteFlow:11201002"; //  "The user already exists in the Sales Team"
    }

    // 113xxxxx
    public static class Discussion
    {
        public const string RecipientRequired = "QuoteFlow:11303001"; //  "A message must have at least one recipient."
        public const string RecipientRequiredForSalePIC = "QuoteFlow:11303002"; //  "You must select at least one recipient since you are the Sale PIC of this Price Offer."
    }

    // 114xxxxx
    public static class GIC
    {
        public const string GICNoAlreadyExists = "QuoteFlow:11403001"; //  "The GIC No '{gicNo}' already exists."
    }

    // 115xxxxx
    public static class SpecialInputPrice
    {
        public const string SpecialInputPriceAccountNoAlreadyExists = "QuoteFlow:11503001"; //  "The Special Input Price Account No '{accountNo}' already exists.";
    }

    // 116xxxxx
    public static class SalesOrder
    {
        public const string SalesOrderNoAlreadyExists = "QuoteFlow:11603001"; //  "The Sales Order No '{salesOrderNo}' already exists.";
        public const string OnlyInProgressCanConfirmDelivery = "QuoteFlow:11603002"; //  "Only in-progress Sales Order can be confirmed for delivery.";
    }

    public static class Category
    {
        public const string RecordInUseCannotDelete = "QuoteFlow:10203002"; //  "Unable to delete this record because it is currently in use."
    }

    public static class SystemSQL
    {
        public const string MaxLengthExceeded = "QuoteFlow:11204001"; //  "The maximum length for field '{fieldName}' is {maxLength}."
    }

    // 117xxxxx
    public static class GKR
    {
        public const string OnlySubmittedCanBeApproved = "QuoteFlow:11703001"; //  "Only submitted GKR can be approved."
    }

    public static class Asset
    {
        public const string AssetIsPendingApproval = "QuoteFlow:11803001"; //  "Asset {assetName} is in Progress"
    }

}

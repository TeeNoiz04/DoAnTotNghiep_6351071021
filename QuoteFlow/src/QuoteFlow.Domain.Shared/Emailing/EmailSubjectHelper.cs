using QuoteFlow.Shared.Models;

namespace QuoteFlow.Emailing;

public static class EmailSubjectHelper
{
    public static string Generate(string sender, string action, string entityName, EmailRecipientRole recipientRole, string? entityCode = null)
    {
        string baseSubject = EmailConsts.SubjectPrefix;
        string fullEntityName = string.IsNullOrWhiteSpace(entityCode) ? entityName : $"{entityCode}";

        string actionSubject = action switch
        {

            HistoryActions.Submitted => GenerateSubmittedSubject(fullEntityName, recipientRole),
            HistoryActions.SubmittedLending => GenerateSubmittedLendingSubject(fullEntityName, recipientRole),
            HistoryActions.Approved => GenerateApprovedSubject(fullEntityName, recipientRole),
            HistoryActions.Rejected => GenerateRejectedSubject(fullEntityName, recipientRole),
            HistoryActions.Cancelled => GenerateCancelledSubject(fullEntityName, recipientRole),
            HistoryActions.PriceOfferDetail.QuantityMerged => GenerateQuantityMergedSubject(fullEntityName, recipientRole),
            HistoryActions.PriceOffer.SpecialInputPriceAssigned => GenerateApplySpecialInputPriceSubject(fullEntityName, recipientRole),
            HistoryActions.PriceOffer.SubmittedAsWin => GenerateSubmittedAsWinSubject(fullEntityName, recipientRole),
            HistoryActions.PriceOffer.SubmittedAsLoss => GenerateSubmittedAsLossSubject(fullEntityName, recipientRole),
            HistoryActions.PriceOffer.SubmittedAsPreOrder => GenerateSubmittedAsPreOrderSubject(fullEntityName, recipientRole),
            HistoryActions.PriceOffer.LandingCostUpdated => GenerateLandingCostUpdatedSubject(fullEntityName, recipientRole),
            HistoryActions.PriceOffer.SubmittedMoreItems => GenerateSubmittedMoreItemsSubject(fullEntityName, recipientRole),
            HistoryActions.PriceOffer.Confirm => GenerateConfirmedSubject(fullEntityName, recipientRole),
            HistoryActions.DPO.Confirmed => GenerateConfirmedSubject(fullEntityName, recipientRole),
            HistoryActions.Discussion.MessageSent => GenerateDiscussionMessageSentSubject(fullEntityName, recipientRole),
            HistoryActions.Closed => GenerateClosedSubject(fullEntityName, recipientRole),
            _ => throw new System.NotImplementedException()
        };
        return $"{baseSubject} {actionSubject}";
    }

    private static string GenerateConfirmedSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"The {fullEntityName} is waiting for your confirm",
            EmailRecipientRole.Sender => $"The {fullEntityName} has been confirmed",
            _ => $"{fullEntityName} has been confirmed"
        };
    }
    private static string GenerateSubmittedSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"The {fullEntityName} has been submitted for your approval",
            EmailRecipientRole.Sender => $"You have submitted a new {fullEntityName}",
            _ => $"{fullEntityName} has been submitted"
        };
    }
    private static string GenerateSubmittedLendingSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"The {fullEntityName} has been submitted for your approval",
            EmailRecipientRole.Sender => $"New Request (For Your Information) - {fullEntityName}",
            _ => $"{fullEntityName} has been submitted"
        };
    }
    private static string GenerateApprovedSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"The {fullEntityName} is waiting for your approval.",
            EmailRecipientRole.Sender => $"The {fullEntityName} has been approved",
            _ => $"The {fullEntityName} has been approved"
        };
    }

    private static string GenerateRejectedSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"You rejected the {fullEntityName}",
            EmailRecipientRole.Sender => $"The {fullEntityName} has been rejected",
            _ => $"The {fullEntityName} has been rejected"
        };
    }

    private static string GenerateCancelledSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"The {fullEntityName} has been cancelled",
            EmailRecipientRole.Sender => $"The {fullEntityName} has been cancelled",
            _ => $"The {fullEntityName} has been cancelled"
        };
    }

    private static string GenerateQuantityMergedSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"The quantity for {fullEntityName} has been merged",
            EmailRecipientRole.Sender => $"You have merged the quantity for {fullEntityName}",
            _ => $"{fullEntityName} quantity has been merged"
        };
    }

    private static string GenerateSubmittedAsWinSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"{fullEntityName} has been submitted as a win",
            EmailRecipientRole.Sender => $"You have submitted {fullEntityName} as a win",
            _ => $"{fullEntityName} has been submitted as a win"
        };
    }

    private static string GenerateSubmittedAsLossSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"{fullEntityName} has been submitted as a loss",
            EmailRecipientRole.Sender => $"You have submitted {fullEntityName} as a loss",
            _ => $"{fullEntityName} has been submitted as a loss"
        };
    }

    private static string GenerateSubmittedAsPreOrderSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"{fullEntityName} has been submitted as a pre-order",
            EmailRecipientRole.Sender => $"You have submitted {fullEntityName} as a pre-order",
            _ => $"{fullEntityName} has been submitted as a pre-order"
        };
    }

    private static string GenerateLandingCostUpdatedSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"{fullEntityName} landing cost has been updated",
            EmailRecipientRole.Sender => $"You have updated the landing cost for {fullEntityName}",
            _ => $"{fullEntityName} landing cost has been updated"
        };
    }

    private static string GenerateApplySpecialInputPriceSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"The {fullEntityName} special input price has been applied",
            EmailRecipientRole.Sender => $"The {fullEntityName} special input price has been applied",
            _ => $"{fullEntityName} special input price has been applied"
        };
    }

    private static string GenerateSubmittedMoreItemsSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"The {fullEntityName} has more items submitted for your review",
            EmailRecipientRole.Sender => $"You have submitted more items for {fullEntityName}",
            _ => $"The {fullEntityName} has more items submitted"
        };
    }

    public static string GenerateDiscussionMessageSentSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"New message in discussion in {fullEntityName}",
            EmailRecipientRole.Sender => $"You sent a message in discussion in {fullEntityName}",
            EmailRecipientRole.NormalRecipient => $"New message in discussion in {fullEntityName}",
            _ => $"New message in discussion in {fullEntityName}"
        };
    }

    public static string GenerateClosedSubject(string fullEntityName, EmailRecipientRole recipientRole)
    {
        return recipientRole switch
        {
            EmailRecipientRole.Approver => $"The {fullEntityName} has been closed",
            EmailRecipientRole.Sender => $"The {fullEntityName} has been closed",
            _ => $"{fullEntityName} has been closed"
        };
    }
}

namespace QuoteFlow.Emailing;

public enum EmailRecipientRole
{
    Default = 0,
    Sender = 1,
    Approver = 2,

    Buyer = 3,
    NormalRecipient = 4,
    Reject = 5,
    Cancel = 6,
    Confirm = 7,
}

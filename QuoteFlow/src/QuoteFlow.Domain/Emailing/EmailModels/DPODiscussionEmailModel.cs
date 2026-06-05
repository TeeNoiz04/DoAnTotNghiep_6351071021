using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Shared.Extensions;
using System;
using System.Text.Json;

namespace QuoteFlow.Emailing.EmailModels;

public class DPODiscussionEmailModel : IEmailModel
{
    public string TemplateName => EmailConsts.DPODiscussionEmailTemplateName;

    public DPOEmailInfo DistributorPurchaseOrder { get; set; }
    public string RecipientFullName { get; set; }

    public string SenderFullName { get; set; }

    public string Comment { get; set; }

    public string FormattedSentDate { get; set; }
    public object TemplateDataModel => new
    {
        DistributorPurchaseOrder,
        RecipientFullName,
        SenderFullName,
        Comment,
        FormattedSentDate
    };

    public DPODiscussionEmailModel(
        DPOEmailInfo distributorPurchaseOrder,
        string recipientFullName,
        string senderFullName,
        string comment,
        DateTime formattedSentDate)
    {
        DistributorPurchaseOrder = distributorPurchaseOrder;
        RecipientFullName = recipientFullName;
        SenderFullName = senderFullName;
        Comment = comment;
        FormattedSentDate = formattedSentDate.ToStandardString();
    }


    public DPODiscussionEmailModel() { }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}

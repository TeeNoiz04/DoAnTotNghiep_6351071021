using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Shared.Extensions;
using System;
using System.Text.Json;

namespace QuoteFlow.Emailing.EmailModels;

public class PriceOfferDiscussionEmailModel : IEmailModel
{
    public string TemplateName => EmailConsts.PriceOfferDiscussionEmailTemplateName;

    public PriceOfferEmailInfo PriceOffer { get; set; }

    public string RecipientFullName { get; set; }

    public string SenderFullName { get; set; }

    public string Comment { get; set; }

    public string FormattedSentDate { get; set; }

    public object TemplateDataModel => new
    {
        PriceOffer,
        RecipientFullName,
        SenderFullName,
        Comment,
        FormattedSentDate
    };

    public PriceOfferDiscussionEmailModel(
        PriceOfferEmailInfo priceOffer,
        string recipientFullName,
        string senderFullName,
        string comment,
        DateTime formattedSentDate)
    {
        PriceOffer = priceOffer;
        RecipientFullName = recipientFullName;
        SenderFullName = senderFullName;
        Comment = comment;
        FormattedSentDate = formattedSentDate.ToStandardString();
    }

    // Parameterless constructor for serialization purposes
    public PriceOfferDiscussionEmailModel()
    {
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}

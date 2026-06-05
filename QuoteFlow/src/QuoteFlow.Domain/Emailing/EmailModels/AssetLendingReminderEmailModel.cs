
using QuoteFlow.Shared.Extensions;
using System;


namespace QuoteFlow.Emailing.Models;
public class AssetLendingReminderEmailModel : IEmailModel
{
    public string TemplateName => EmailConsts.AssetLendingReminderEmailTemplateName;

    public string? ReceiverFullName { get; set; }
    public string RequestNo { get; set; }
    public string RequesterFullName { get; set; }
    public string FormattedSubmittedDate { get; set; }
    public string RequestTitle { get; set; }
    public string DueDate { get; set; }
    public string ApprovalUrl { get; set; }

    public object TemplateDataModel => new
    {
        RequestNo,
        RequesterFullName,
        FormattedSubmittedDate,
        RequestTitle,
        DueDate,
        ApprovalUrl,
        ReceiverFullName
    };

    public AssetLendingReminderEmailModel(
        string requestNo,
        string requesterFullName,
        DateTime submittedDate,
        string requestTitle,
        DateTime dueDate,
        string approvalUrl, string? receiverFullNamerec)
    {
        RequestNo = requestNo;
        RequesterFullName = requesterFullName;
        FormattedSubmittedDate = submittedDate.ToStandardString();
        RequestTitle = requestTitle;
        DueDate = dueDate.ToStandardString();
        ApprovalUrl = approvalUrl;
        ReceiverFullName = receiverFullNamerec;
    }

    public AssetLendingReminderEmailModel()
    {

    }
}

using QuoteFlow.Shared.Extensions;
using System;

namespace QuoteFlow.Emailing.EmailModels;
public class AssetLendingNotificationEmailModel : IEmailModel
{
    public string TemplateName => EmailConsts.AssetLendingNotifyEmailTemplatePath;
    public string RequestNo { get; set; }
    public string RequesterFullName { get; set; }
    public string FormattedSubmittedDate { get; set; }
    public string RequestTitle { get; set; }
    public string Note { get; set; }
    public string ApprovalUrl { get; set; }
    
    public object TemplateDataModel => new
    {
        RequestNo,
        RequesterFullName,
        FormattedSubmittedDate,
        RequestTitle,
        ApprovalUrl
    };

    public AssetLendingNotificationEmailModel(
        string requestNo,
        string requesterFullName,
        DateTime submittedDate,
        string requestTitle,
        string approvalUrl,
        string note)
    {
        RequestNo = requestNo;
        RequesterFullName = requesterFullName;
        FormattedSubmittedDate = submittedDate.ToStandardString();
        RequestTitle = requestTitle;
        ApprovalUrl = approvalUrl;
        Note = note;
    }

    public AssetLendingNotificationEmailModel()
    {

    }
}

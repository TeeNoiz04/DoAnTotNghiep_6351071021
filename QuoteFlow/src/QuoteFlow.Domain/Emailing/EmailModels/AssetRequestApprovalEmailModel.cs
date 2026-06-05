using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace QuoteFlow.Emailing.EmailModels;
public class AssetRequestApprovalEmailModel : IHasApprovalHistoryEmailModel
{
    public List<ApprovalHistoryEmailInfo>? ApprovalHistories { get; set; }
    public string TemplateName => EmailConsts.AssetRequestApprovalEmailTemplateName;
    public AssetRequestEmailInfor AssetRequestInfor { get; set; }
    public string Action { get; set; }
    public string RequestType { get; set; }
    public string CreatorName { get; set; }
    public string RequestNo { get; set; }
    public string Status { get; set; }
    public string FormattedSubmittedDate { get; set; }
    public string ApprovalRoute { get; set; }
    public string ApprovedNotConfirm { get; set; }
    public object TemplateDataModel => new
    {
        Action,
        AssetRequestInfor,
        FormattedSubmittedDate,
        ApprovalHistories,
        ApprovalRoute,
        ApprovedNotConfirm,
        RequestNo,
        Status,
        RequestType,
        CreatorName
    };

    public AssetRequestApprovalEmailModel(
        string action,
        AssetRequestEmailInfor assetRequestInfor,
        DateTime formattedSubmittedDate,
        List<ApprovalHistoryEmailInfo> approvalHistories,
        string requestNo,
        string requestType,
        string creatorName,
        string status,
        string approvalRoute,
        string approvedNotConfirm)
    {
        Action = action;
        AssetRequestInfor = assetRequestInfor;
        FormattedSubmittedDate = formattedSubmittedDate.ToStandardString();
        ApprovalHistories = approvalHistories.OrderBy(x => x.ActionDate).ToList();
        RequestNo = requestNo;
        RequestType = requestType;
        CreatorName = creatorName;
        Status = status;
        ApprovalRoute = approvalRoute;
        ApprovedNotConfirm = approvedNotConfirm;
    }

    // Parameterless constructor for serialization purposes
    public AssetRequestApprovalEmailModel()
    {
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}

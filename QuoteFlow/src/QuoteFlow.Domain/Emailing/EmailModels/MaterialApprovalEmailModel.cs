using QuoteFlow.Emailing.EmailInfoModel;
using System.Collections.Generic;
using System.Linq;

namespace QuoteFlow.Emailing.EmailModels;
public class MaterialApprovalEmailModel : IHasApprovalHistoryEmailModel
{
    public List<ApprovalHistoryEmailInfo>? ApprovalHistories { get; set; }

    public string TemplateName => EmailConsts.MaterialApprovalEmailTemplateName;

    public string ApproverRoleName { get; set; }

    public string MaterialCode { get; set; }

    public string ImportType { get; set; }

    public string FormattedSubmittedDate { get; set; }
    public string Status { get; set; }
    public string CreatorName { get; set; }
    public string FileName { get; set; }
    public string Note { get; set; }

    public string ApprovalRoute { get; set; }

    public object TemplateDataModel => new
    {
        ApproverRoleName,
        MaterialCode,
        ImportType,
        FormattedSubmittedDate,
        Status,
        CreatorName,
        FileName,
        Note,
        ApprovalHistories,
        ApprovalRoute
    };

    public MaterialApprovalEmailModel(
        string fileName,
        string note,
        string status,
        string creatorName,
        string materialCode,
        string importType,
        string approverRoleName,
        string formattedSubmittedDate,
        string approvalRoute,
        List<ApprovalHistoryEmailInfo> approvalHistories)
    {
        FileName = fileName;
        Note = note;
        Status = status;
        CreatorName = creatorName;
        MaterialCode = materialCode;
        ImportType = importType;
        ApproverRoleName = approverRoleName;
        FormattedSubmittedDate = formattedSubmittedDate;
        ApprovalRoute = approvalRoute;
        ApprovalHistories = approvalHistories.OrderBy(x => x.ActionDate).ToList();
    }

    protected MaterialApprovalEmailModel()
    {

    }
}

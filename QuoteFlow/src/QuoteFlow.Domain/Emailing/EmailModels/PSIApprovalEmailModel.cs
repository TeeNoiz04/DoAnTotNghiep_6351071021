using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace QuoteFlow.Emailing.EmailModels;

public class PSIApprovalEmailModel : IHasApprovalHistoryEmailModel
{
    public List<ApprovalHistoryEmailInfo>? ApprovalHistories { get; set; }

    public string TemplateName => EmailConsts.PSIApprovalEmailTemplateName;
    public string FileName { get; set; }
    public int? FinancialYear { get; set; }
    public string MaterialType { get; set; }
    public string ApproverRoleName { get; set; }

    public string PsiCode { get; set; }

    public string ImportType { get; set; }

    public string FormattedSubmittedDate { get; set; }

    public string Status { get; set; }
    public string CreatorName { get; set; }

    public string Note { get; set; }
    public string ApprovalRoute { get; set; }
    public object TemplateDataModel => new
    {
        FileName,
        FinancialYear,
        MaterialType,
        Status,
        CreatorName,
        PsiCode,
        ImportType,
        FormattedSubmittedDate,
        ApprovalHistories,
        Note,
        ApprovalRoute
    };

    public PSIApprovalEmailModel(
        int? financialYear,
        string fileName,
        string materialType,
        string status,
        string creatorName,
        string psiCode,
        string importType,
        string approverRoleName,
        string note,
        DateTime formattedSubmittedDate,
        string approvalRoute,
        List<ApprovalHistoryEmailInfo> approvalHistories)
    {
        FinancialYear = financialYear;
        FileName = fileName;
        MaterialType = materialType;
        Status = status;
        CreatorName = creatorName;
        PsiCode = psiCode;
        ImportType = importType;
        ApproverRoleName = approverRoleName;
        Note = note;
        ApprovalRoute = approvalRoute;
        FormattedSubmittedDate = formattedSubmittedDate.ToStandardString(); ;
        ApprovalHistories = approvalHistories;
    }

    public PSIApprovalEmailModel()
    {

    }


    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}

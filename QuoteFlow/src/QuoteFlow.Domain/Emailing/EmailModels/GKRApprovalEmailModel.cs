using QuoteFlow.Emailing.EmailInfoModel;
using System.Text.Json;

namespace QuoteFlow.Emailing.EmailModels;

public class GKRApprovalEmailModel : IEmailModel
{
    public string TemplateName => EmailConsts.GKRApprovalEmailTemplateName;
    public GKREmailInfo Gkr { get; set; }
    public string Action { get; set; }
    public bool IsLastRoute { get; set; }
    public string ApprovalRoute { get; set; }

    public object TemplateDataModel => new
    {
        Gkr,
        Action,
        IsLastRoute,
        ApprovalRoute
    };

    public GKRApprovalEmailModel(
        GKREmailInfo gkr,
        string action,
        bool isLastRoute = false,
        string approvalRoute = ""
    )
    {
        Gkr = gkr;
        Action = action;
        IsLastRoute = isLastRoute;
        ApprovalRoute = approvalRoute;
    }

    // Parameterless constructor for serialization purposes
    public GKRApprovalEmailModel()
    {
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}

using QuoteFlow.Emailing.EmailInfoModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace QuoteFlow.Emailing.EmailModels;
public class KeyAccountApprovalEmailModel : IHasApprovalHistoryEmailModel
{
    public List<ApprovalHistoryEmailInfo>? ApprovalHistories { get; set; }

    public string TemplateName => EmailConsts.KeyAccountApprovalEmailTemplateName;

    public KeyAccountEmailInfo KeyAccount { get; set; }
    public string ApprovalRoute { get; set; }


    public object TemplateDataModel => new
    {
        KeyAccount,
        ApprovalHistories,
        ApprovalRoute
    };

    public KeyAccountApprovalEmailModel(
        KeyAccountEmailInfo keyAccount,

        List<ApprovalHistoryEmailInfo> approvalHistories,
        string approvalRoute)
    {
        KeyAccount = keyAccount;

        ApprovalHistories = approvalHistories.OrderBy(x => x.ActionDate).ToList();
        ApprovalRoute = approvalRoute;
    }

    // Parameterless constructor for serialization purposes
    public KeyAccountApprovalEmailModel(
    )
    {
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}

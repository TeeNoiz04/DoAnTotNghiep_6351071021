using QuoteFlow.Emailing.EmailInfoModel;
using System.Text.Json;

namespace QuoteFlow.Emailing.EmailModels;

public class DPOApprovalEmailModel : IEmailModel
{
    public string TemplateName => EmailConsts.DPOApprovalEmailTemplateName;
    public DPOEmailInfo Dpo { get; set; }
    public string Action { get; set; }

    public object TemplateDataModel => new
    {
        Dpo,
        Action
    };

    public DPOApprovalEmailModel(
        DPOEmailInfo dpo,
        string action
    )
    {
        Dpo = dpo;
        Action = action;
    }

    // Parameterless constructor for serialization purposes
    public DPOApprovalEmailModel(
    )
    {
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}

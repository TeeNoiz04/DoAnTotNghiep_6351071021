using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace QuoteFlow.Emailing.EmailModels;

public class PriceOfferApprovalEmailModel : IHasApprovalHistoryEmailModel
{
    public List<ApprovalHistoryEmailInfo>? ApprovalHistories { get; set; }

    public string TemplateName => EmailConsts.PriceOfferApprovalEmailTemplateName;

    public PriceOfferEmailInfo PriceOffer { get; set; }

    public string Action { get; set; }
    public string FormattedSubmittedDate { get; set; }

    public string SpecialInputPriceId { get; set; }
    public string AccountNo { get; set; }
    public string CustomerName { get; set; }
    public string ProjectName { get; set; }
    public string ValidFrom { get; set; }
    public string ValidTo { get; set; }
    public string ApprovedNotConfirm { get; set; }
    public string ApprovalRoute { get; set; }

    public object TemplateDataModel => new
    {
        Action,
        PriceOffer,
        FormattedSubmittedDate,
        ApprovalHistories,
        AccountNo,
        CustomerName,
        ProjectName,
        ValidFrom,
        ValidTo,
        SpecialInputPriceId,
        ApprovalRoute,
        ApprovedNotConfirm
    };

    public PriceOfferApprovalEmailModel(
        string action,
        PriceOfferEmailInfo priceOffer,
        DateTime formattedSubmittedDate,
        List<ApprovalHistoryEmailInfo> approvalHistories,
        string accountNo,
        string customerName,
        string projectName,
        DateTime validFrom,
        DateTime validTo,
        string specialInputPriceId,
        string approvalRoute,
        string approvedNotConfirm)
    {
        Action = action;
        PriceOffer = priceOffer;
        FormattedSubmittedDate = formattedSubmittedDate.ToStandardString();
        ApprovalHistories = approvalHistories.OrderBy(x => x.ActionDate).ToList();
        AccountNo = accountNo;
        CustomerName = customerName;
        ProjectName = projectName;
        ValidFrom = validFrom.ToStandardString();
        ValidTo = validTo.ToStandardString();
        SpecialInputPriceId = specialInputPriceId;
        ApprovalRoute = approvalRoute;
        ApprovedNotConfirm = approvedNotConfirm;
    }

    // Parameterless constructor for serialization purposes
    public PriceOfferApprovalEmailModel(
    )
    {
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}

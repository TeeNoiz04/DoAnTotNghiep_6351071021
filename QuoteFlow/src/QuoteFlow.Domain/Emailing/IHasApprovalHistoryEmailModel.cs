using QuoteFlow.Emailing.EmailInfoModel;
using System.Collections.Generic;

namespace QuoteFlow.Emailing;

public interface IHasApprovalHistoryEmailModel : IEmailModel
{
    List<ApprovalHistoryEmailInfo>? ApprovalHistories { get; set; }
}


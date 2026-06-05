using QuoteFlow.ApprovalHistories;
using System;

namespace QuoteFlow.Materials.MaterialApprovalRequests;
public class MaterialApprovalRequestHistoryDto : ApprovalHistoryDto
{
    public Guid MaterialApprovalRequestId { get; set; }

}

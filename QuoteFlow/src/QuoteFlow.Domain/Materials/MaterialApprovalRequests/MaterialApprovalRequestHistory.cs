using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using System;

namespace QuoteFlow.Materials.MaterialApprovalRequests;
public class MaterialApprovalRequestHistory : ApprovalHistory
{
    public Guid MaterialApprovalRequestId { get; set; }

    protected MaterialApprovalRequestHistory() : base()
    {

    }

    public MaterialApprovalRequestHistory(Guid id, Guid materialApprovalRequestId, ApprovalHistoryCreateParams createParams)
        : base(id, createParams)
    {
        MaterialApprovalRequestId = materialApprovalRequestId;
        EntityType = EntityTypes.MaterialApprovalRequest;
    }
}

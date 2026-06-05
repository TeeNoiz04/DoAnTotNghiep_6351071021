using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using System;

namespace QuoteFlow.DPOs.DPODetails;

public class DPODetailApprovalHistory : ApprovalHistory
{
    public virtual Guid DPODetailId { get; set; }

    public virtual DPODetail DPODetail { get; set; } = null!;

    protected DPODetailApprovalHistory()
    {
    }

    public DPODetailApprovalHistory(Guid id, Guid dpoDetailId, ApprovalHistoryCreateParams createParams)
        : base(id, createParams)
    {
        DPODetailId = dpoDetailId;
        EntityType = createParams.EntityType == EntityTypes.GICDetail ? EntityTypes.GICDetail : createParams.EntityType == EntityTypes.GKRDetail ? EntityTypes.GKRDetail : EntityTypes.DPODetail;
    }
}
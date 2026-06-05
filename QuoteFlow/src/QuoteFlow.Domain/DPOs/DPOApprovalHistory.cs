using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using System;

namespace QuoteFlow.DPOs;

public class DPOApprovalHistory : ApprovalHistory
{
    public virtual Guid DPOId { get; set; }

    public virtual DPO DPO { get; set; } = null!;

    protected DPOApprovalHistory()
    {
    }

    public DPOApprovalHistory(Guid id, Guid dpoId, ApprovalHistoryCreateParams createParams)
        : base(id, createParams)
    {
        DPOId = dpoId;
        EntityType = createParams.EntityType == EntityTypes.GIC ? EntityTypes.GIC : createParams.EntityType == EntityTypes.GKR ? EntityTypes.GKR : EntityTypes.DPO; // prevent other entity types from being used
    }
}
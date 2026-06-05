using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using System;

namespace QuoteFlow.SaleOrders;
public class SOHistory : ApprovalHistory
{
    public virtual Guid SOId { get; set; }

    public SOHistory(Guid id, Guid soId, ApprovalHistoryCreateParams createParams)
        : base(id, createParams)
    {
        SOId = soId;
        EntityType = EntityTypes.SO;
    }

    public SOHistory()
    {

    }
}

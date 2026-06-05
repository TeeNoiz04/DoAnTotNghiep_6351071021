using QuoteFlow.Shared.Flagging;
using System.Collections.Generic;

namespace QuoteFlow.Materials.Flagging.MaterialApprovalRequests;

public class MaterialApprovalRequestFlaggingContext : BaseFlaggingContext
{

    public MaterialApprovalRequestFlaggingContext()
    {
    }

    public MaterialApprovalRequestFlaggingContext(string currentUsername, IEnumerable<string> roles, Dictionary<string, object>? additionalData = null)
        : base(currentUsername, roles, additionalData)
    {
    }
}


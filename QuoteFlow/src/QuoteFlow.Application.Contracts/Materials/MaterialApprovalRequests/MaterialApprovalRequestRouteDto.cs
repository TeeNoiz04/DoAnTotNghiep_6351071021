using QuoteFlow.ApprovalRoutes;
using System;

namespace QuoteFlow.Materials.MaterialApprovalRequests;
public class MaterialApprovalRequestRouteDto : ApprovalRouteDto
{
    public Guid MaterialApprovalRequestId { get; set; }
}

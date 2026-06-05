using System;

namespace QuoteFlow.Shared.Interfaces;

public interface IApprovalRouteAuditedObject
{
    DateTime? LastApprovalRouteCreationTime { get; set; }

    string? LastApprovalRouteCreatorName { get; set; }

    string? LastApprovalRouteCreatorUsername { get; set; }

    Guid? LastApprovalRouteCreatorId { get; set; }
}
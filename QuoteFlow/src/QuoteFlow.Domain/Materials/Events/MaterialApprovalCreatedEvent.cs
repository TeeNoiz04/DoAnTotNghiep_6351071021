using System;

namespace QuoteFlow.Materials.Events;
public record MaterialApprovalCreatedEvent(
    Guid MaterialApprovalRequestId,
    string? Note = null
);


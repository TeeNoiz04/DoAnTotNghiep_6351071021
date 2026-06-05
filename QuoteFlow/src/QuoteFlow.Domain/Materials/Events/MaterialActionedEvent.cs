using System;

namespace QuoteFlow.Materials.Events;
public record MaterialActionedEvent(
    Guid MaterialApprovalRequestId,
    string Action,
    string ActionerUsername,
    DateTime SubmittedDate,
    string? Comment = null
    );

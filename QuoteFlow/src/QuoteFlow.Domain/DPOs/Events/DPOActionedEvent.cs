using System;

namespace QuoteFlow.DPOs.Events;

public record DPOActionedEvent(
    Guid DPOId,
    string Action,
    string ActionerUsername,
    DateTime ActionedAt,
    string? Comment = null
);

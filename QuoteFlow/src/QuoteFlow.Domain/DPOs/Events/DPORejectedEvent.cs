using System;
using System.Collections.Generic;

namespace QuoteFlow.DPOs.Events;

public record DPORejectedEvent(
    Guid DPOId,
    List<DPODeletedEventDetail> RejectedDetails
);

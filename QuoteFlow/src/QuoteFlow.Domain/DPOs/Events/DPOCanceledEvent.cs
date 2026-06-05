using System;
using System.Collections.Generic;

namespace QuoteFlow.DPOs.Events;

public record DPOCanceledEvent(
    Guid DPOId,
    List<DPODeletedEventDetail> CanceledDetails
);
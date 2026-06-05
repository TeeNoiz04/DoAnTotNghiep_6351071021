using System;
using System.Collections.Generic;

namespace QuoteFlow.DPOs.Events;

public record DPODeletedEvent(
    Guid DPOId,
    List<DPODeletedEventDetail> Details
);

public record DPODeletedEventDetail(
    Guid DPODetailId,
    string SPOCode,
    string GolfaCode,
    decimal UnitPrice,
    int Qty
);
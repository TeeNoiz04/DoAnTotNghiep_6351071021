using System;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;

public class SpoBatchRequestDetailImportDto
{
    public Guid RequestId { get; set; }
    public string? SPOCode { get; set; }
    public string? GolfaCode { get; set; }
    public string? Action { get; set; }
    public DateTime? ActionDate { get; set; }
    public string? Note { get; set; }

}

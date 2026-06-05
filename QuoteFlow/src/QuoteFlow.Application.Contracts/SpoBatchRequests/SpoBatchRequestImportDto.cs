using QuoteFlow.Shared.Excels;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;

namespace QuoteFlow.SpoBatchRequests;

public class SpoBatchRequestImportDto
{
    public string RequestNo { get; set; } = null!;
    public string ImportType { get; set; } = null!;
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }

    public ExcelValidationResult<SpoBatchRequestDetailImportDto>? Details { get; set; }
}

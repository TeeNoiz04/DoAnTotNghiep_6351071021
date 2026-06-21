using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SpoBatchRequests;

public class GetSpoBatchRequestsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? RequestNo { get; set; }
    public string? ImportType { get; set; }
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public string? SPOCode { get; set; }
    public string? GolfaCode { get; set; }

    public GetSpoBatchRequestsInput()
    {

    }
}
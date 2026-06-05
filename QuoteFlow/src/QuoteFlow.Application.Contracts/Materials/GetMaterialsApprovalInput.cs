using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Materials;
public class GetMaterialsApprovalInput : PagedAndSortedResultRequestDto
{
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? ImportType { get; set; }
    public string? ApprovalStatus { get; set; }

}

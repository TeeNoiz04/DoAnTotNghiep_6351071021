using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
public class MaterialApprovalRequestSubmitParams : IHasConcurrencyStamp
{
    public string ImportType { get; set; } = null!;
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public string RequestNo { get; set; } = null!;
    public string ConcurrencyStamp { get; set; } = null!;
}

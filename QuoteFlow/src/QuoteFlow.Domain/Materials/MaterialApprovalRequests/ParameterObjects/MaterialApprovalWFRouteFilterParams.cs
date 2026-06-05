using System;

namespace QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
public class MaterialApprovalWFRouteFilterParams
{
    public Guid MaterialId { get; set; }
    public string ImportType { get; set; } = null!;


    public string? Note { get; set; }
}

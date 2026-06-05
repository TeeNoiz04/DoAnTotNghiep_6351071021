using System;

namespace QuoteFlow.SalesAssignments.ParameterObjects;

public class SalesAssignmentFilterParams
{
    public string? FilterText { get; set; }
    public string? SaleUserName { get; set; }
    public string? MaterialType { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? BuyerId { get; set; }
    public Guid? BuyerTypeId { get; set; }
}
using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SalesAssignments;

public class GetSalesAssignmentInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public string? SaleUserName { get; set; }
    public string? MaterialType { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? BuyerId { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public GetSalesAssignmentInput()
    {
    }
}
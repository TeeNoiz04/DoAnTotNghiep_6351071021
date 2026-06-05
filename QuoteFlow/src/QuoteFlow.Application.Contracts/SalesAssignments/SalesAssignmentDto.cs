using QuoteFlow.Buyers;
using QuoteFlow.Shared;
using QuoteFlow.SystemCategories;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SalesAssignments;

public class SalesAssignmentDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string SaleUserName { get; set; } = null!;

    public string? SaleFullName { get; set; }

    public string MaterialType { get; set; } = null!;

    public Guid LocationId { get; set; }

    public Guid BuyerId { get; set; }

    public string? BuyerShortName { get; set; }

    public Guid BuyerTypeId { get; set; }

    public string? Note { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;

    public SystemCategoryListDto? BuyerType { get; set; }
    public SystemCategoryListDto? Location { get; set; }
    public BuyerListDto? Buyer { get; set; }
}

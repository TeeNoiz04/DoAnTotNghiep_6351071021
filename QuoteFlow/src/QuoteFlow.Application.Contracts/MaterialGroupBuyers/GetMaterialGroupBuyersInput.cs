using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.MaterialGroupBuyers;

public class GetMaterialGroupBuyersInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? MaterialGroupId { get; set; }
    public string? MaterialGroupCode { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerShortName { get; set; }
    public string? Note { get; set; }

    public GetMaterialGroupBuyersInput()
    {

    }
}
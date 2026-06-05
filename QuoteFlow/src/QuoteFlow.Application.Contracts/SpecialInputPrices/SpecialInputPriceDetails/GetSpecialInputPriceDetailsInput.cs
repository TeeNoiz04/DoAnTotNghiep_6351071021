using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;

public class GetSpecialInputPriceDetailsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? SpecialInputPriceId { get; set; }
    public string? MaterialCode { get; set; }
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public int? LimitQtyMin { get; set; }
    public int? LimitQtyMax { get; set; }
    public decimal? InputPriceMin { get; set; }
    public decimal? InputPriceMax { get; set; }
    public decimal? LandedCostMin { get; set; }
    public decimal? LandedCostMax { get; set; }
    public int? UsedMin { get; set; }
    public int? UsedMax { get; set; }
    public string? Note { get; set; }

    public GetSpecialInputPriceDetailsInput()
    {

    }
}
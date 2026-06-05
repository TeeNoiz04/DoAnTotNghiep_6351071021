using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SpecialInputPrices;

public class GetSpecialInputPricesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? AccountNo { get; set; }
    public string? AccountName { get; set; }
    public List<string>? Materials { get; set; }
    public List<string>? Models { get; set; }
    //public List<string>? AccountNos { get; set; }
    //public List<string>? AccountNames { get; set; }
    public string? ProjectName { get; set; }
    public DateTime? ValidFromMin { get; set; }
    public DateTime? ValidFromMax { get; set; }
    public DateTime? ValidToMin { get; set; }
    public DateTime? ValidToMax { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }

    public GetSpecialInputPricesInput()
    {

    }
}
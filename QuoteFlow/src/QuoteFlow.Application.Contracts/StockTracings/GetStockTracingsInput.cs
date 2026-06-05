using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.StockTracings;

public class GetStockTracingsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? FileName { get; set; }
    public ReportType? ReportType { get; set; }
    public DateTime? FromDateMin { get; set; }
    public DateTime? FromDateMax { get; set; }
    public DateTime? ToDateMin { get; set; }
    public DateTime? ToDateMax { get; set; }
    public string? Note { get; set; }

    public GetStockTracingsInput()
    {

    }
}
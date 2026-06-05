using QuoteFlow.StockTracings;
using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.StockTracingDetails;

public class GetStockTracingDetailsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? StockTracingId { get; set; }
    public ReportType? ReportType { get; set; }
    public int? RowNoMin { get; set; }
    public int? RowNoMax { get; set; }
    public string? PackingListCode { get; set; }
    public string? CheckListCode { get; set; }
    public DateTime? DateEnteredMin { get; set; }
    public DateTime? DateEnteredMax { get; set; }
    public string? Stock { get; set; }
    public string? BU { get; set; }
    public string? Customer { get; set; }
    public string? Category { get; set; }
    public string? GIV { get; set; }
    public string? Invoice { get; set; }
    public string? SKUCode { get; set; }
    public string? SKUName { get; set; }
    public string? Quality { get; set; }
    public string? Warranty { get; set; }
    public string? Unit { get; set; }
    public string? Series { get; set; }
    public string? OriginCode { get; set; }
    public DateTime? ProductionDateMin { get; set; }
    public DateTime? ProductionDateMax { get; set; }
    public string? Location { get; set; }
    public string? GolfaCode { get; set; }
    public string? Note { get; set; }
    public string? MaterialType { get; set; }
    public string? Model { get; set; }

    public GetStockTracingDetailsInput()
    {

    }
}
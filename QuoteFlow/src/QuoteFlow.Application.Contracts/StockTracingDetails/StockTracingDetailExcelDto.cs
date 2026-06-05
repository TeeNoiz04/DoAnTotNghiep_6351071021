using QuoteFlow.StockTracings;
using MiniExcelLibs.Attributes;
using System;

namespace QuoteFlow.StockTracingDetails;

public class StockTracingDetailExcelDto
{
    //public Guid StockTracingId { get; set; }
    [ExcelColumnWidth(20)]
    public ReportType ReportType { get; set; }
    //public int? RowNo { get; set; }
    [ExcelColumnWidth(30)]
    public string? PackingListCode { get; set; }
    [ExcelColumnWidth(30)]
    public string? CheckListCode { get; set; }
    [ExcelColumnWidth(20)]
    public DateTime? DateEntered { get; set; }
    [ExcelColumnWidth(20)]
    public string? Stock { get; set; }
    [ExcelColumnWidth(20)]
    public string? BU { get; set; }
    [ExcelColumnWidth(40)]
    public string? Customer { get; set; }
    [ExcelColumnWidth(40)]
    public string? Category { get; set; }
    [ExcelColumnWidth(20)]
    public string? GIV { get; set; }
    [ExcelColumnWidth(20)]
    public string? Invoice { get; set; }
    [ExcelColumnWidth(20)]
    public string? SKUCode { get; set; }
    [ExcelColumnWidth(20)]
    public string? SKUName { get; set; }
    [ExcelColumnWidth(20)]
    public string? Quality { get; set; }
    [ExcelColumnWidth(20)]
    public string? Warranty { get; set; }
    [ExcelColumnWidth(20)]
    public string? Unit { get; set; }
    [ExcelColumnWidth(20)]
    public string? Series { get; set; }
    [ExcelColumnWidth(20)]
    public string? OriginCode { get; set; }
    [ExcelColumnWidth(20)]
    public DateTime? ProductionDate { get; set; }
    [ExcelColumnWidth(30)]
    public string? Location { get; set; }
    [ExcelColumnWidth(20)]
    public string? GolfaCode { get; set; }
    [ExcelColumnWidth(40)]
    public string? Note { get; set; }
}
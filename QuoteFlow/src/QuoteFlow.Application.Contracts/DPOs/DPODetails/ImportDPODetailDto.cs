using System;

namespace QuoteFlow.DPOs.DPODetails;

public class ImportDPODetailDto
{
    public int? RowNo { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public Guid? SPOId { get; set; }
    public string? SPOCode { get; set; }
    public Guid? CustomerId { get; set; }
    public int? Qty { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? RequestedETA { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerTaxCode { get; set; }
    public string? CustomerType { get; set; }
    public string? CustomerIndustry { get; set; }
    public string? Note { get; set; }
}
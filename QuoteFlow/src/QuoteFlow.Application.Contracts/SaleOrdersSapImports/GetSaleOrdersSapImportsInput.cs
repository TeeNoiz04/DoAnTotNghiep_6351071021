using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SaleOrdersSapImports;

public class GetSaleOrdersSapImportsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? SONo { get; set; }
    public string? DONo { get; set; }
    public DateTime? DODateMin { get; set; }
    public DateTime? DODateMax { get; set; }
    public string? DONote { get; set; }
    public string? SOSAPNo { get; set; }
    public string? DOSAPNo { get; set; }
    public string? BillingNo { get; set; }
    public string? InvoiceNo { get; set; }
    public DateTime? InvoiceDateMin { get; set; }
    public DateTime? InvoiceDateMax { get; set; }
    public bool? Deleted { get; set; }

    public GetSaleOrdersSapImportsInput()
    {

    }
}
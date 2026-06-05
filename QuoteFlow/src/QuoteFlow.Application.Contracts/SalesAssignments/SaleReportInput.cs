using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SalesAssignments;
public class SaleReportInput : PagedAndSortedResultRequestDto
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? InvoiceFromDate { get; set; }
    public DateTime? InvoiceToDate { get; set; }

}

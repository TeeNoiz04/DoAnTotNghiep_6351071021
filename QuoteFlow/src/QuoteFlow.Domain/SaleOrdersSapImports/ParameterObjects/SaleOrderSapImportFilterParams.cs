using System;

namespace QuoteFlow.SaleOrdersSapImports.ParameterObjects;
public class SaleOrderSapImportFilterParams
{
    public string? FilterText { get; set; }

    public string? SONo { get; set; }
    public string? SOSAPNo { get; set; }
    public string? DOSAPNo { get; set; }
    //public string? DONote { get; set; }
    public string? BillingNo { get; set; }
    public string? InvoiceNo { get; set; }
    public DateTime? InvoiceDateMin { get; set; }
    public DateTime? InvoiceDateMax { get; set; }

    public string? Note { get; set; }
    public string? FileName { get; set; }
    public Guid? ImportKey { get; set; }
    public bool? IsDeleted { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}

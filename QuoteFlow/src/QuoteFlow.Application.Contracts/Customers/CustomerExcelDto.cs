using MiniExcelLibs.Attributes;

namespace QuoteFlow.Customers;

public class CustomerExcelDto
{
    [ExcelColumnWidth(40)]
    public virtual string? TaxCode { get; set; }

    [ExcelColumnWidth(40)]
    public virtual string? CustomerName { get; set; }

    [ExcelColumnWidth(40)]
    public virtual string? CustomerShortName { get; set; }

    [ExcelColumnWidth(40)]
    public virtual string? Address { get; set; }

    [ExcelColumnWidth(40)]
    public virtual string? Phone { get; set; }

    [ExcelColumnWidth(40)]
    public virtual string? Country { get; set; }

    [ExcelColumnWidth(40)]
    public virtual string? Province { get; set; }

    [ExcelColumnWidth(40)]
    public virtual string? Website { get; set; }

    [ExcelColumnWidth(40)]
    public virtual string? CustomerType { get; set; }
    [ExcelColumnWidth(40)]
    public virtual string? CustomerIndustry { get; set; }

    [ExcelColumnWidth(40)]
    public virtual string? Note { get; set; }


}
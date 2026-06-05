using System;

namespace QuoteFlow.Customers;

public class CustomerExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;
    public string? TaxCode { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerShortName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Country { get; set; }

    public string? Province { get; set; }

    public string? Website { get; set; }
    public string? CustomerType { get; set; }
    public string? CustomerIndustry { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public string? Note { get; set; }

    public bool IsDeactive { get; set; }

    public CustomerExcelDownloadDto()
    {

    }
}
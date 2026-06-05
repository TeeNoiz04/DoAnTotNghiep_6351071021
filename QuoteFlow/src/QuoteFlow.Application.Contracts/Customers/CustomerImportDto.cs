using System;

namespace QuoteFlow.Customers;
public class CustomerImportDto
{
    public string? TaxCode { get; set; } // A.
    public string? CustomerName { get; set; } // B
    public string? ShortName { get; set; } // C
    public string? Address { get; set; } // D
    public string? Phone { get; set; } // E
    public string? Country { get; set; } // F
    public string? Province { get; set; } // G
    public string? Website { get; set; } // H
    public string? CustomerType { get; set; } // I
    public string? CustomerIndustry { get; set; } // J
    public string? Note { get; set; } // K
    public bool? IsUpdate { get; set; } = false;
    public Guid? IdUpdate { get; set; } = null;
    public string? ConcurrencyStamp { get; set; } = null;
}

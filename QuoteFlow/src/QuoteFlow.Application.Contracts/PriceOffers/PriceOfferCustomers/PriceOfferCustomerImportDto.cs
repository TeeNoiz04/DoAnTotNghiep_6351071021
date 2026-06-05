using System;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers;

public class PriceOfferCustomerImportDto
{
    public string? SaleChannel { get; set; }
    public string? CustomerTaxCode { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public string? CustomerNationality { get; set; }
    public Guid? CustomerNationalityId { get; set; }
    public string? CustomerType { get; set; }
    public string? CustomerIndustry { get; set; }
    public bool HasKeyAccount { get; set; } = false; // This will be set by the service after import
}

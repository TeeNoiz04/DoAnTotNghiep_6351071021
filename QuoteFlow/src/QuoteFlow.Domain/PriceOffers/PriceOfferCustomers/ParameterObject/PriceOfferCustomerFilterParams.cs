using System;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;

public class PriceOfferCustomerFilterParams
{
    public string? FilterText { get; set; }

    public Guid? PriceOfferId { get; set; }

    public string? SaleChannel { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CustomerTaxCode { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerAddress { get; set; }

    public string? CustomerNationality { get; set; }

    public string? CustomerType { get; set; }

    public string? Note { get; set; }

}

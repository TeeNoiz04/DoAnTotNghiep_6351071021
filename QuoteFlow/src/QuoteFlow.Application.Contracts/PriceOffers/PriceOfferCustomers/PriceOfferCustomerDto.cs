using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers;

public class PriceOfferCustomerDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid PriceOfferId { get; set; }
    public string SaleChannel { get; set; } = null!;
    public int SaleChannelNumber { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerTaxCode { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public string? CustomerNationality { get; set; }
    public string? CustomerType { get; set; }
    public string? CustomerIndustry { get; set; }
    public string? Note { get; set; }
    public bool HasKeyAccount { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}
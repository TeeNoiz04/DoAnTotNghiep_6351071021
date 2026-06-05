using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers;

public class PriceOfferCustomerUpdateDto : IHasConcurrencyStamp
{
    [Required]
    public Guid PriceOfferId { get; set; }
    [Required]
    [StringLength(PriceOfferCustomerConsts.SaleChannelMaxLength)]
    public string SaleChannel { get; set; } = null!;
    [Required]
    public Guid CustomerId { get; set; }
    [StringLength(PriceOfferCustomerConsts.CustomerTaxCodeMaxLength)]
    public string? CustomerTaxCode { get; set; }
    [StringLength(PriceOfferCustomerConsts.CustomerNameMaxLength)]
    public string? CustomerName { get; set; }
    [StringLength(PriceOfferCustomerConsts.CustomerAddressMaxLength)]
    public string? CustomerAddress { get; set; }
    [StringLength(PriceOfferCustomerConsts.CustomerNationalityMaxLength)]
    public string? CustomerNationality { get; set; }
    [StringLength(PriceOfferCustomerConsts.CustomerTypeMaxLength)]
    public string? CustomerType { get; set; }
    [StringLength(PriceOfferCustomerConsts.NoteMaxLength)]
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
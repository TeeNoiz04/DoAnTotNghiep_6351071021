using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;

public class PriceOfferCustomerCreateParams
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

    [StringLength(PriceOfferCustomerConsts.CustomerIndustryMaxLength)]
    public virtual string? CustomerIndustry { get; set; }

    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }

    // For deserialization purposes only
    protected PriceOfferCustomerCreateParams()
    {

    }

    public PriceOfferCustomerCreateParams(Guid priceOfferId, string saleChannel, Guid customerId, string customerTaxCode, string? customerName = null, string? customerAddress = null, string? customerNationality = null, string? customerType = null, string? customerIndustry = null, string? note = null)
    {
        PriceOfferId = priceOfferId;
        SaleChannel = saleChannel;
        CustomerId = customerId;
        CustomerTaxCode = customerTaxCode;
        CustomerName = customerName;
        CustomerAddress = customerAddress;
        CustomerNationality = customerNationality;
        CustomerType = customerType;
        CustomerIndustry = customerIndustry;
        Note = note;
    }
}

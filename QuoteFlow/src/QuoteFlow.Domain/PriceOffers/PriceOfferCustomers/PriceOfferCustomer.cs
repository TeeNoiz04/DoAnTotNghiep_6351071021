using JetBrains.Annotations;
using QuoteFlow.Customers;
using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using QuoteFlow.Shared.Models;
using System;
using System.Text.RegularExpressions;
using Volo.Abp;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers;

public class PriceOfferCustomer : ExtendedFullAuditedAggregateRoot<Guid>
{
    public virtual Guid PriceOfferId { get; set; }

    [NotNull]
    public virtual string SaleChannel
    {
        get => _saleChannel;
        set
        {
            _saleChannel = value;
            SaleChannelNumber = ExtractNumberFromSaleChannel(value);
        }
    }

    private string _saleChannel = string.Empty;

    public virtual int SaleChannelNumber { get; private set; }

    public virtual Guid CustomerId { get; set; }

    [CanBeNull]
    public virtual string? CustomerTaxCode { get; set; }

    [CanBeNull]
    public virtual string? CustomerName { get; set; }

    [CanBeNull]
    public virtual string? CustomerAddress { get; set; }

    [CanBeNull]
    public virtual string? CustomerNationality { get; set; }

    [CanBeNull]
    public virtual string? CustomerType { get; set; }

    [CanBeNull]
    public virtual string? CustomerIndustry { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    #region Navigation Properties
    public virtual PriceOffer? PriceOffer { get; set; }
    public virtual Customer? Customer { get; set; }
    #endregion

    protected PriceOfferCustomer()
    {
        SaleChannel = string.Empty;
    }

    private static int ExtractNumberFromSaleChannel(string saleChannel)
    {
        if (string.IsNullOrEmpty(saleChannel))
            return 0;

        var match = Regex.Match(saleChannel, @"\d+");
        return match.Success ? int.Parse(match.Value) : 0;
    }

    public PriceOfferCustomer(Guid id, Guid priceOfferId, string saleChannel, Guid customerId, string? customerTaxCode = null, string? customerName = null, string? customerAddress = null, string? customerNationality = null, string? customerType = null, string? note = null)
    {

        Id = id;
        Check.NotNull(saleChannel, nameof(saleChannel));
        Check.Length(saleChannel, nameof(saleChannel), PriceOfferCustomerConsts.SaleChannelMaxLength, 0);
        Check.Length(customerTaxCode, nameof(customerTaxCode), PriceOfferCustomerConsts.CustomerTaxCodeMaxLength, 0);
        Check.Length(customerName, nameof(customerName), PriceOfferCustomerConsts.CustomerNameMaxLength, 0);
        Check.Length(customerAddress, nameof(customerAddress), PriceOfferCustomerConsts.CustomerAddressMaxLength, 0);
        Check.Length(customerNationality, nameof(customerNationality), PriceOfferCustomerConsts.CustomerNationalityMaxLength, 0);
        Check.Length(customerType, nameof(customerType), PriceOfferCustomerConsts.CustomerTypeMaxLength, 0);
        Check.Length(note, nameof(note), PriceOfferCustomerConsts.NoteMaxLength, 0);
        PriceOfferId = priceOfferId;
        SaleChannel = saleChannel; // This will automatically set SaleChannelNumber through the setter
        CustomerId = customerId;
        CustomerTaxCode = customerTaxCode;
        CustomerName = customerName;
        CustomerAddress = customerAddress;
        CustomerNationality = customerNationality;
        CustomerType = customerType;
        Note = note;
    }
    public PriceOfferCustomer(Guid id, PriceOfferCustomerCreateParams createParams)
    {
        Id = id;
        PriceOfferId = createParams.PriceOfferId;
        SaleChannel = createParams.SaleChannel; // This will automatically set SaleChannelNumber through the setter
        CustomerId = createParams.CustomerId;
        CustomerTaxCode = createParams.CustomerTaxCode;
        CustomerName = createParams.CustomerName;
        CustomerAddress = createParams.CustomerAddress;
        CustomerNationality = createParams.CustomerNationality;
        CustomerType = createParams.CustomerType;
        CustomerIndustry = createParams.CustomerIndustry;
        Note = createParams.Note;
    }
}
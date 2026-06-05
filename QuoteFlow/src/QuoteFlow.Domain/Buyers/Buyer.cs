using JetBrains.Annotations;
using QuoteFlow.Buyers.ParameterObjects;
using QuoteFlow.Shared.Models;
using QuoteFlow.SystemCategories;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Buyers;

public class Buyer : ExtendedAuditedAggregateRoot<Guid>
{

    [Required]
    public Guid BuyerTypeId { get; set; }

    [Required]
    public string BuyerTypeCode { get; set; }

    [Required]
    public string BuyerCode { get; set; } = null!;

    [CanBeNull]
    public string? ShortName { get; set; }

    [CanBeNull]
    public string? FullName { get; set; }

    [CanBeNull]
    public string? TaxCode { get; set; }

    [CanBeNull]
    public string? Address { get; set; }

    [CanBeNull]
    public string? ContactPerson { get; set; }

    [CanBeNull]
    public string? ContactEmail { get; set; }

    [CanBeNull]
    public string? ContactPhoneNumber { get; set; }

    [CanBeNull]
    public string? PaymentTermCode { get; set; }

    [CanBeNull]
    public string? PaymentTermDescription { get; set; }

    [CanBeNull]
    public decimal? CreditLimit { get; set; }

    [CanBeNull]
    public decimal? CreditExposure { get; set; }

    [CanBeNull]
    public decimal? AvailableCredit { get; set; }

    [CanBeNull]
    public int? AppliedPrice { get; set; }

    [CanBeNull]
    public bool? Deactive { get; set; }

    [CanBeNull]
    public string? Note { get; set; }

    public SystemCategory BuyerType { get; set; } = null!;
    public Buyer()
    {

    }
    public Buyer(Guid id, BuyerCreateParams createParams)
    {
        Id = id;
        BuyerTypeCode = createParams.BuyerTypeCode;
        BuyerTypeId = createParams.BuyerTypeId;
        BuyerCode = createParams.BuyerCode;
        ShortName = createParams.ShortName;
        FullName = createParams.FullName;
        TaxCode = createParams.TaxCode;
        Address = createParams.Address;
        ContactPerson = createParams.ContactPerson;
        ContactEmail = createParams.ContactEmail;
        ContactPhoneNumber = createParams.ContactPhoneNumber;
        PaymentTermCode = createParams.PaymentTermCode;
        PaymentTermDescription = createParams.PaymentTermDescription;
        CreditLimit = createParams.CreditLimit;
        CreditExposure = createParams.CreditExposure;
        AppliedPrice = createParams.AppliedPrice;
        Deactive = createParams.Deactive;
        Note = createParams.Note;
    }
}

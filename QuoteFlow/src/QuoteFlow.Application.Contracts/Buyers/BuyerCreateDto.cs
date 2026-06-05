using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Buyers;

public class BuyerCreateDto
{
    [Required]
    public Guid BuyerTypeId { get; set; }
    [Required]
    [StringLength(BuyerConsts.BuyerCodeMaxLength)]
    public string BuyerTypeCode { get; set; } = null!;

    [Required]
    [StringLength(BuyerConsts.BuyerCodeMaxLength)]
    public string BuyerCode { get; set; } = null!;
    [CanBeNull]
    [StringLength(BuyerConsts.ShortNameMaxLength)]
    public string? ShortName { get; set; }
    [CanBeNull]
    [StringLength(BuyerConsts.FullNameMaxLength)]
    public string? FullName { get; set; }
    [CanBeNull]
    [StringLength(BuyerConsts.TaxCodeMaxLength)]
    public string? TaxCode { get; set; }
    [CanBeNull]
    [StringLength(BuyerConsts.AddressMaxLength)]
    public string? Address { get; set; }
    [CanBeNull]
    [StringLength(BuyerConsts.ContactPersonMaxLength)]
    public string? ContactPerson { get; set; }
    [CanBeNull]
    [StringLength(BuyerConsts.ContactEmailMaxLength)]
    public string? ContactEmail { get; set; }
    [CanBeNull]
    [StringLength(BuyerConsts.ContactPhoneNumberMaxLength)]
    public string? ContactPhoneNumber { get; set; }
    [CanBeNull]
    [StringLength(BuyerConsts.PaymentTermCodeMaxLength)]
    public string? PaymentTermCode { get; set; }
    [CanBeNull]
    [StringLength(BuyerConsts.PaymentTermDescriptionMaxLength)]
    public string? PaymentTermDescription { get; set; }
    [CanBeNull]
    public decimal? CreditLimit { get; set; }
    [CanBeNull]
    public decimal? CreditExposure { get; set; }
    [CanBeNull]
    public int? AppliedPrice { get; set; }
    [CanBeNull]
    public bool? Deactive { get; set; }
    [CanBeNull]
    [StringLength(BuyerConsts.NoteMaxLength)]
    public string? Note { get; set; }
}

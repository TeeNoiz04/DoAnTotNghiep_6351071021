using QuoteFlow.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Buyers.ParameterObjects;

public class BuyerUpdateParams : BaseUpdateParams
{
    [Required]
    public Guid BuyerTypeId { get; set; } = Guid.Empty!;

    [Required]
    [StringLength(BuyerConsts.BuyerCodeMaxLength)]
    public string BuyerTypeCode { get; set; } = null!;
    [Required]
    public string BuyerCode { get; set; } = null!;
    [StringLength(BuyerConsts.ShortNameMaxLength)]
    public string? ShortName { get; set; }
    [StringLength(BuyerConsts.FullNameMaxLength)]
    public string? FullName { get; set; }
    [StringLength(BuyerConsts.TaxCodeMaxLength)]
    public string? TaxCode { get; set; }
    [StringLength(BuyerConsts.AddressMaxLength)]
    public string? Address { get; set; }
    [StringLength(BuyerConsts.ContactPersonMaxLength)]
    public string? ContactPerson { get; set; }
    [StringLength(BuyerConsts.ContactEmailMaxLength)]
    public string? ContactEmail { get; set; }
    [StringLength(BuyerConsts.ContactPhoneNumberMaxLength)]
    public string? ContactPhoneNumber { get; set; }
    [StringLength(BuyerConsts.PaymentTermCodeMaxLength)]
    public string? PaymentTermCode { get; set; }
    [StringLength(BuyerConsts.PaymentTermDescriptionMaxLength)]
    public string? PaymentTermDescription { get; set; }
    public decimal? CreditLimit { get; set; }
    public decimal? CreditExposure { get; set; }
    public int? AppliedPrice { get; set; }
    public bool? Deactive { get; set; }
    [StringLength(BuyerConsts.NoteMaxLength)]
    public string? Note { get; set; }
}

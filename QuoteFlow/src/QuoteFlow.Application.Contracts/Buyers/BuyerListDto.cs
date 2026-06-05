using QuoteFlow.Shared;
using System;

namespace QuoteFlow.Buyers;
public class BuyerListDto : ExtendedAuditedEntityDto<Guid>
{
    public Guid BuyerTypeId { get; set; } = Guid.Empty!;
    public string BuyerTypeCode { get; set; } = null!;

    public string DiscriptionCategory { get; set; } = null!;
    public string BuyerCode { get; set; } = null!;
    public string? ShortName { get; set; }
    public string? FullName { get; set; }
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? PaymentTermCode { get; set; }
    public string? PaymentTermDescription { get; set; }
    public decimal? CreditLimit { get; set; }
    public decimal? CreditExposure { get; set; }
    public decimal? AvailableCredit { get; set; }
    public int? AppliedPrice { get; set; }
    public bool? Deactive { get; set; }
    public string? Note { get; set; }
}

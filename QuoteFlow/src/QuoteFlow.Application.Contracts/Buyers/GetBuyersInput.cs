using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Buyers;

public class GetBuyersInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public string? BuyerCode { get; set; }
    public string? ShortName { get; set; }
    public string? FullName { get; set; }
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? PaymentTermCode { get; set; }
    public string? PaymentTermDescription { get; set; }
    public decimal? CreditLimitMax { get; set; }
    public decimal? CreditLimitMin { get; set; }
    public decimal? CreditExposureMax { get; set; }
    public decimal? CreditExposureMin { get; set; }
    public int? AppliedPrice { get; set; }
    public bool? Deactive { get; set; }
    public string? Note { get; set; }
    public GetBuyersInput()
    {

    }
}

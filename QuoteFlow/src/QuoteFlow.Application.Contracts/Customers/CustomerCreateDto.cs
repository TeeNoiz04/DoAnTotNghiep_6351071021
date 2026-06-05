using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Customers;

public class CustomerCreateDto
{
    [Required]
    [StringLength(CustomerConsts.TaxCodeMaxLength)]
    public string TaxCode { get; set; } = null!;

    [Required]
    [StringLength(CustomerConsts.CustomerNameMaxLength)]
    public string CustomerName { get; set; } = null!;

    [StringLength(CustomerConsts.CustomerShortNameMaxLength)]
    public string? CustomerShortName { get; set; }

    [StringLength(CustomerConsts.AddressMaxLength)]
    public string? Address { get; set; }

    [StringLength(CustomerConsts.WebsiteMaxLength)]
    public string? Website { get; set; }

    [StringLength(CustomerConsts.PhoneMaxLength)]
    public string? Phone { get; set; }

    public string? Country { get; set; }

    [StringLength(CustomerConsts.ProvinceMaxLength)]
    public string? Province { get; set; }

    [StringLength(CustomerConsts.CustomerIndustryMaxLength)]
    public string? CustomerIndustry { get; set; }

    [StringLength(CustomerConsts.CustomerTypeMaxLength)]
    public string? CustomerType { get; set; }


    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }
}
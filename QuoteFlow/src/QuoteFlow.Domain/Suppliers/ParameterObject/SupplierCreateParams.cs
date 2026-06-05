using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Suppliers.ParameterObject;

public class SupplierCreateParams
{
    [Required]
    [StringLength(SupplierConsts.SupplierCodeMaxLength)]
    public string SupplierCode { get; set; } = null!;
    [StringLength(SupplierConsts.SAPCodeMaxLength)]
    public string? SAPCode { get; set; }
    [Required]
    [StringLength(SupplierConsts.ShortNameMaxLength)]
    public string ShortName { get; set; } = null!;
    [Required]
    [StringLength(SupplierConsts.FullNameMaxLength)]
    public string FullName { get; set; } = null!;
    [StringLength(SupplierConsts.TaxCodeMaxLength)]
    public string? TaxCode { get; set; }
    [StringLength(SupplierConsts.AddressMaxLength)]
    public string? Address { get; set; }
    public bool? IsDeactive { get; set; } = false;
}

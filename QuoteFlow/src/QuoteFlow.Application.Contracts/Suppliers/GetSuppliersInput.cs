using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Suppliers;

public class GetSuppliersInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }
    public string? SupplierCode { get; set; }

    public string? ShortName { get; set; }
    public string? FullName { get; set; }
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    public bool? IsDeactive { get; set; } = null;
}

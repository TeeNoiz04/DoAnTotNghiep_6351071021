using Volo.Abp.Application.Dtos;

namespace QuoteFlow.StockManagements;
public class GetStockManagementsInput : PagedAndSortedResultRequestDto
{

    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? SAPCode { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialGroup { get; set; }
    //public Guid? Factory { get; set; }
    //public Guid? Vendor { get; set; }

    public string? MaterialStatus { get; set; }

    public string? SupplierBU { get; set; }
    public string? Supplier { get; set; }
    public int? StockQty { get; set; }
    public int? OnOderStock { get; set; }
    public bool? Deactive { get; set; }


}
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.StockCategories;

public class GetStockCategoriesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public bool? FOC { get; set; }
    public bool? MainStock { get; set; }
    public bool? DamagedStock { get; set; }
    public int? SortOrderMin { get; set; }
    public int? SortOrderMax { get; set; }
    public bool? IsDeactive { get; set; }
    public string? Note { get; set; }

    public GetStockCategoriesInput()
    {

    }
}
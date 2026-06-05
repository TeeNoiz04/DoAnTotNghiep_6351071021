namespace QuoteFlow.Shared;

public class StockCategoryLookupDto<TKey> : LookupDto<TKey>
{
    public int AvailableQuantity { get; set; }
}

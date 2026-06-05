namespace QuoteFlow.Shared;
public class SupplierBULookupDto<TKey> : LookupDto<TKey>
{
    public string? MaterialType { get; set; }
    public string? Currency { get; set; }
}

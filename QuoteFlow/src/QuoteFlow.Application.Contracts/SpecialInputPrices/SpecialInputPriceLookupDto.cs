namespace QuoteFlow.SpecialInputPrices;

public class SpecialInputPriceLookupDto<TKey>
{
    public TKey Id { get; set; }
    public string AccountName { get; set; }
    public string AccountNo { get; set; }
    public string? Status { get; set; }
    public string? MaterialType { get; set; }
}

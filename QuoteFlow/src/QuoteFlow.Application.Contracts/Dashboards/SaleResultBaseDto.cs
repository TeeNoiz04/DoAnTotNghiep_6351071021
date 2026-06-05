namespace QuoteFlow.Dashboards;

public class SaleResultBaseDto
{
    public string? BuyerCode { get; set; }
    public string? MaterialType { get; set; }


    public decimal? SaleResult { get; set; }
    public decimal? SaleTarget { get; set; }

}

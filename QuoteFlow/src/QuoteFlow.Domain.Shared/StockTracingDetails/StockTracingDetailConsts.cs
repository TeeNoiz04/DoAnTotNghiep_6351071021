namespace QuoteFlow.StockTracingDetails;

public static class StockTracingDetailConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "StockTracingDetail." : string.Empty);
    }

    public const int PackingListCodeMaxLength = 100;
    public const int CheckListCodeMaxLength = 100;
    public const int StockMaxLength = 200;
    public const int BUMaxLength = 50;
    public const int CustomerMaxLength = 1000;
    public const int CategoryMaxLength = 200;
    public const int GIVMaxLength = 100;
    public const int InvoiceMaxLength = 100;
    public const int SKUCodeMaxLength = 500;
    public const int SKUNameMaxLength = 1000;
    public const int QualityMaxLength = 1000;
    public const int WarrantyMaxLength = 100;
    public const int UnitMaxLength = 100;
    public const int SeriesMaxLength = 200;
    public const int OriginCodeMaxLength = 100;
    public const int LocationMaxLength = 1000;
    public const int GolfaCodeMaxLength = 100;
}
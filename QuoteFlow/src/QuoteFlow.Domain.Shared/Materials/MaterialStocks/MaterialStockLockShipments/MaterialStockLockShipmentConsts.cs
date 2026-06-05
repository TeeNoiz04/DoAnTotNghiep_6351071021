namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;

public static class MaterialStockLockShipmentConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialStockLockShipment." : string.Empty);
    }

    public const int GolfaCodeMaxLength = 50;
    public const int NoteMaxLength = 4000;
}
namespace QuoteFlow.StockTracings;

public static class StockTracingConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "StockTracing." : string.Empty);
    }

    public const int FileNameMaxLength = 255;

    //Sheet nameAdd commentMore actions
    public const string DeliveryImport = "Import_Delivery";
    public const string InventoryImport = "Import_Inventory";
    public const string ReceiptImport = "Import_Receipt";

    //Cells
    public const string DeliveryImportStartCell = "A6";
    public const string DeliveryImportEndCell = "T200000";

    public const string InventoryImportStartCell = "A6";
    public const string InventoryImportEndCell = "O200000";

    public const string ReceiptImportStartCell = "A6";
    public const string ReceiptImportEndCell = "R200000";

    //Columns
    public const string DeliveryImportStartColumn = "A";
    public const string DeliveryImportEndColumn = "T";

    public const string InventoryImportStartColumn = "A";
    public const string InventoryImportEndColumn = "O";

    public const string ReceiptImportStartColumn = "A";
    public const string ReceiptImportEndColumn = "R";
}
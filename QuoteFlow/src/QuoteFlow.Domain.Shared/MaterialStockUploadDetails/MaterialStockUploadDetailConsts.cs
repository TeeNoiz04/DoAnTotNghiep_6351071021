namespace QuoteFlow.MaterialStockUploadDetails;

public static class MaterialStockUploadDetailConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialStockUploadDetail." : string.Empty);
    }

    public const int MaterialCodeMaxLength = 50;
    public const int ModelMaxLength = 4000;
    public const int StorageMaxLength = 50;
    public const int StorageDestinationMaxLength = 50;
    public const int RefDocMaxLength = 4000;

    // SheetName
    public const string ExcelImportSheet = "ImportData";


    // Columns

    public const string ExcelMaterialCodeColumn = "A";
    public const string ExcelRemarkColumn = "F";

    // Cells
    public const string ExcelMaterialCodeStartCell = "A2";

    public const string ExcelRemarkEndCell = "F1000";
}
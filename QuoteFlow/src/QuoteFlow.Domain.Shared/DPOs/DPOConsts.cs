namespace QuoteFlow.DPOs;

public static class DPOConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "DPO." : string.Empty);
    }

    public const int DPONoMaxLength = 50;
    public const int DPOTypeMaxLength = 50;
    public const int DPOSubTypeMaxLength = 50;
    public const int MaterialTypeMaxLength = 50;
    public const int CostCenterMaxLength = 50;
    public const int StatusMaxLength = 50;
    public const int BuyerShortNameMaxLength = 50;
    public const int RemarkMaxLength = 4000;
    public const int FileNameMaxLength = 255;
    public const int GICProcessMaxLength = 1000;
    public const int ReferenceDocMaxLength = 4000;


    // Excel Import Constants
    public const string ExcelImportSheetName = "DPO Template";
    public const string ExcelDetailsStartCell = "A8";
    public const string ExcelDetailsEndCell = "N2000";
    public const string ExcelDetailsStopColumn = "B";
}
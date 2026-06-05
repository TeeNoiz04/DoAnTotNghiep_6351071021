namespace QuoteFlow.SaleOrders;

public static class SaleOrderConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "SaleOrder." : string.Empty);
    }

    public const string SaleOrderDPOPrefix = "SO";
    public const string SaleOrderGICPrefix = "SO.GIC";
    public const string SaleOrderGICIUPrefix = "SO-GIC-IU-";
    public const string SaleOrderGICWOPrefix = "SO-GIC-WO-";
    public const string SaleOrderGICWRPrefix = "SO-GIC-WR-";
    public const string SaleOrderGICFOCPrefix = "SO-GIC-FOC-";

    public const int SONoMaxLength = 50;
    public const int SOSAPNoMaxLength = 50;
    public const int MaterialTypeMaxLength = 50;
    public const int BuyerCodeMaxLength = 50;
    public const int BuyerNameMaxLength = 255;
    public const int StatusCodeMaxLength = 50;
    public const int NoteMaxLength = 4000;
    public const int SOTypeMaxLength = 50;
    public const int GICTypeMaxLength = 50;
    public const int GICProcessMaxLength = 1000;

    public const string ImportSheetName = "Import_Data";
    public const string ImportStartCell = "A2";
    public const string ImportEndCell = "F1000";
    public const string ImportStartColumn = "A";
    public const string ImportEndColumn = "F";

}
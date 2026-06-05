namespace QuoteFlow.SaleOrdersSapImports;

public static class SaleOrdersSapImportConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "SaleOrdersSapImport." : string.Empty);
    }

    public const int SONoMaxLength = 50;
    public const int DONoMaxLength = 50;
    public const int DONoteMaxLength = 4000;
    public const int SOSAPNoMaxLength = 50;
    public const int DOSAPNoMaxLength = 50;
    public const int BillingNoMaxLength = 50;
    public const int InvoiceNoMaxLength = 50;
    public const int NoteMaxLength = 4000;
    public const int FileNameMaxLength = 400;

    // GIC
    public const int GICPORNoMaxLength = 50;
    public const int GICPRNoMaxLength = 50;
    public const int GICGivNoMaxLength = 50;
    public const int GICSalesPICMaxLength = 50;
    public const int GICLocationMaxLength = 50;
    public const int GICReservationNoMaxLength = 50;
    public const int GICAssetClassMaxLength = 50;
    public const int GICMainAssetCodeMaxLength = 50;
    public const int GICSubAssetCodeMaxLength = 50;
    public const int GICAssetNameMaxLength = 50;
}
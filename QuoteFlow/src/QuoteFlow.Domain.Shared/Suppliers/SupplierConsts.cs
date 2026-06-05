namespace QuoteFlow.Suppliers;

public static class SupplierConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Supplier." : string.Empty);
    }
    public const int SupplierCodeMaxLength = 50;
    public const int SAPCodeMaxLength = 50;
    public const int ShortNameMaxLength = 500;
    public const int FullNameMaxLength = 1000;
    public const int TaxCodeMaxLength = 50;
    public const int AddressMaxLength = 4000;
}

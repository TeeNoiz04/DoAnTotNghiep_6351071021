namespace QuoteFlow.Customers;

public static class CustomerConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Customer." : string.Empty);
    }

    public const int TaxCodeMaxLength = 50;
    public const int CustomerNameMaxLength = 400;
    public const int CustomerTypeMaxLength = 500;
    public const int CustomerShortNameMaxLength = 400;
    public const int WebsiteMaxLength = 4000;
    public const int AddressMaxLength = 4000;
    public const int PhoneMaxLength = 400;
    public const int ProvinceMaxLength = 50;
    public const int CustomerIndustryMaxLength = 50;
    public const int CountryMaxLength = 50;

    public const string CustomerImport = "Customers";

    //Cells
    public const string CustomerImportStartCell = "A2";
    public const string CustomerImportEndCell = "K30000";

    //Columns
    public const string CustomerImportStartColumn = "A";
    public const string CustomerImportEndColumn = "K";
}
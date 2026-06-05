namespace QuoteFlow.PriceOffers.PriceOfferCustomers;

public static class PriceOfferCustomerConsts
{
    private const string DefaultSorting = "{0}SaleChannelNumber asc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "PriceOfferCustomer." : string.Empty);
    }

    public const int SaleChannelMaxLength = 100;
    public const int CustomerTaxCodeMaxLength = 50;
    public const int CustomerNameMaxLength = 500;
    public const int CustomerAddressMaxLength = 500;
    public const int CustomerNationalityMaxLength = 50;
    public const int CustomerTypeMaxLength = 50;
    public const int CustomerIndustryMaxLength = 500;
    public const int NoteMaxLength = 4000;

    public const string DefaultTaxCode = "N/A";
}
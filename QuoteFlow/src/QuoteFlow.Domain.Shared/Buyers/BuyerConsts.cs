namespace QuoteFlow.Buyers;

public static class BuyerConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Buyer." : string.Empty);
    }

    public const int BuyerCodeMaxLength = 50;
    public const int ShortNameMaxLength = 200;
    public const int FullNameMaxLength = 500;
    public const int TaxCodeMaxLength = 50;
    public const int AddressMaxLength = 500;
    public const int ContactPersonMaxLength = 200;
    public const int ContactEmailMaxLength = 256;
    public const int ContactPhoneNumberMaxLength = 50;
    public const int PaymentTermCodeMaxLength = 50;
    public const int PaymentTermDescriptionMaxLength = 500;
    public const int NoteMaxLength = 1000;
}


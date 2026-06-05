namespace QuoteFlow.CustomerPICs;

public static class CustomerPICConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "CustomerPIC." : string.Empty);
    }

    public const int PICNameMaxLength = 255;
    public const int PIC_PhoneMaxLength = 255;
    public const int PIC_EmailMaxLength = 255;
    public const int PIC_JobTitleMaxLength = 255;
    public const int RemarkMaxLength = 4000;
}
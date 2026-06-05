namespace QuoteFlow.SystemConfigurations;

public static class SystemConfigurationConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "SystemConfiguration." : string.Empty);
    }

    public const int CfgKeyMaxLength = 50;
    public const int CfgValueMaxLength = 4000;
    public const int DescriptionMaxLength = 4000;
}
namespace QuoteFlow.SystemCategories;

public static class SystemCategoryConsts
{
    private const string DefaultSorting = "{0}SortOrder asc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "SystemCategory." : string.Empty);
    }

    public const int CodeMaxLength = 50;
    public const int DescriptionMaxLength = 500;
    public const int CategoryTypeMaxLength = 50;
}
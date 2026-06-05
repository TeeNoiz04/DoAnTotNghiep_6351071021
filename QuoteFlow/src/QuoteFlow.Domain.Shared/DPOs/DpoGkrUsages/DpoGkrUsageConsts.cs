namespace QuoteFlow.DPOs.DpoGkrUsages;

public static class DpoGkrUsageConsts
{
    private const string DefaultSorting = "{0}GkrId asc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "DpoGkrUsage." : string.Empty);
    }
}

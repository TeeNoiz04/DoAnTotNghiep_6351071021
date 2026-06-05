namespace QuoteFlow.ApprovalRoutes;

public static class ApprovalRouteConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "ApprovalRoute." : string.Empty);
    }

    public const int EntityTypeMaxLength = 50;
    public const int ApproverMaxLength = 100;
    public const int ApproverRoleCodeMaxLength = 50;
    public const int ApproverRoleNameMaxLength = 255;
    public const int NotesMaxLength = 4000;
}
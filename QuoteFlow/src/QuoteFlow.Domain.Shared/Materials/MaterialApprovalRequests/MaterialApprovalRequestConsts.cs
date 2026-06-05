namespace QuoteFlow.Materials.MaterialApprovalRequests;

public static class MaterialApprovalRequestConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialApprovalRequest." : string.Empty);
    }

    public const int ImportTypeMaxLength = 50;
    public const int FileNameMaxLength = 400;
    public const int NoteMaxLength = 4000;
    public const int StatusMaxLength = 50;
    public const int RequestNoMaxLength = 50;
    public const int CurrentApprovalMaxLength = 100;
    public const int CurrentApproverRoleCodeMaxLength = 50;
    public const int CurrentApproverRoleNameMaxLength = 255;
}
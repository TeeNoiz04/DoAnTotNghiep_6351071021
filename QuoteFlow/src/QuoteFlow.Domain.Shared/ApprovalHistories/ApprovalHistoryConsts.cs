namespace QuoteFlow.ApprovalHistories;

public static class ApprovalHistoryConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "ApprovalHistory." : string.Empty);
    }

    public const int EntityTypeMaxLength = 50;
    public const int ApproverRoleCodeMaxLength = 50;
    public const int ApproverRoleNameMaxLength = 1000;
    public const int ApproverUsernameMaxLength = 320;
    public const int ApproverFullNameMaxLength = 1000;
    public const int NoteMaxLength = 4000;
}
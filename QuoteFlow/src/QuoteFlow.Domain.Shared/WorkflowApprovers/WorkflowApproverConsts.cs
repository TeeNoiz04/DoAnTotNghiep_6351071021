namespace QuoteFlow.WorkflowApprovers;

public static class WorkflowApproverConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "WorkflowApprover." : string.Empty);
    }

    public const int ApproverMaxLength = 100;
    public const int NoteMaxLength = 4000;
}
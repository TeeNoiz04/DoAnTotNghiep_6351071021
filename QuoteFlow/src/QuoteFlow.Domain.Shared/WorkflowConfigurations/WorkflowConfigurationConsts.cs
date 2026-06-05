namespace QuoteFlow.WorkflowConfigurations;

public static class WorkflowConfigurationConsts
{
    private const string DefaultSorting = "{0}WorkflowLevel asc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "WorkflowConfiguration." : string.Empty);
    }

    public const int WorkflowTypeMaxLength = 50;
    public const int WorkflowRoleMaxLength = 4000;
    public const int ConditionMaxLength = 4000;
    public const int NoteMaxLength = 4000;
}
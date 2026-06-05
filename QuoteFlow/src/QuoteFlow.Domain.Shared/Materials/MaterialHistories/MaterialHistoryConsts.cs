namespace QuoteFlow.Materials.MaterialHistories;

public static class MaterialHistoryConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialHistory." : string.Empty);
    }

    public const int ActionMaxLength = 50;
    public const int NoteMaxLength = 10;
}
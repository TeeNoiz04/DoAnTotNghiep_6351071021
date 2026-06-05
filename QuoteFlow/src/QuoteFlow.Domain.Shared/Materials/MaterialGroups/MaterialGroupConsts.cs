namespace QuoteFlow.Materials.MaterialGroups;

public class MaterialGroupConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialGroups." : string.Empty);
    }

    public const int CodeMaxLength = 50;
    public const int NameMaxLength = 200;
    public const int NoteMaxLength = 4000;
    public const int MaterialTypeMaxLength = 200;
    public const int MaterialGroupPSIMaxLength = 1000;
}

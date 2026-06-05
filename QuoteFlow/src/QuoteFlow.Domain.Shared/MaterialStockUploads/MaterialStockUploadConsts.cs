namespace QuoteFlow.MaterialStockUploads;

public static class MaterialStockUploadConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialStockUpload." : string.Empty);
    }

    public const int RequestNoMaxLength = 50;
    public const int ImportTypeMaxLength = 50;
    public const int FilNameMaxLength = 400;
    public const int NoteMaxLength = 4000;
    public const int StatusMaxLength = 50;
}
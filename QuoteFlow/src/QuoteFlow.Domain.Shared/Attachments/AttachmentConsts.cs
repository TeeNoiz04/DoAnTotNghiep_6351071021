namespace QuoteFlow.Attachments;

public static class AttachmentConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Attachment." : string.Empty);
    }

    public const int RequestPartMaxLength = 50;
    public const int AttachCodeMaxLength = 50;
    public const int AttachNameMaxLength = 500;
    public const int FileNameMaxLength = 500;
    public const int FileNameDBMaxLength = 500;
    public const int FilePathMaxLength = 4000;
    public const int DescriptionMaxLength = 4000;
    public const int AttachmentFileMaxSizeInMB = 50;
}
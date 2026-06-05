namespace QuoteFlow.Messages;

public static class MessageConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Message." : string.Empty);
    }

    public const int UserNameMaxLength = 100;
    public const int FullNameMaxLength = 255;
    public const int SendToMaxLength = 4000;
}
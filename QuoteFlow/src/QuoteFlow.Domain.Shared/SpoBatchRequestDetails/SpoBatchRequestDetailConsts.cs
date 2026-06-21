namespace QuoteFlow.SpoBatchRequestDetails
{
    public static class SpoBatchRequestDetailConsts
    {
        private const string DefaultSorting = "{0}CreationTime desc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "SpoBatchRequestDetail." : string.Empty);
        }

        public const int SPOCodeMaxLength = 50;
        public const int GolfaCodeMaxLength = 50;
        public const int ActionMaxLength = 50;
        public const int NoteMaxLength = 4000;
    }
}
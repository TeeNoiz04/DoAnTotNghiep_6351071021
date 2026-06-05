namespace QuoteFlow.HistoryTrackings
{
    public static class HistoryTrackingConsts
    {
        private const string DefaultSorting = "{0}CreationTime desc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "HistoryTracking." : string.Empty);
        }

        public const int TrackingTypeMaxLength = 50;
        public const int ActionMaxLength = 50;
        public const int ObjectIdMaxLength = 50;
        public const int GolfaCodeMaxLength = 50;
        public const int ModelMaxLength = 255;
        public const int StockNameMaxLength = 1000;
        public const int NoteMaxLength = 4000;
    }
}
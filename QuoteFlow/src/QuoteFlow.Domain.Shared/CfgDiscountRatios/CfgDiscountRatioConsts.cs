namespace QuoteFlow.CfgDiscountRatios
{
    public static class CfgDiscountRatioConsts
    {
        private const string DefaultSorting = "{0}CreationTime desc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "CfgDiscountRatio." : string.Empty);
        }

        public const int Approval_TypeMaxLength = 50;
        public const int Product_TypeMaxLength = 50;
        public const int AccountClassifyMaxLength = 50;
    }
}
namespace QuoteFlow.DPOs.DPODetails;

public static class DPODetailConsts
{
    private const string DefaultSorting = "{0}RowNo asc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "DPODetail." : string.Empty);
    }

    public const int StatusMaxLength = 50;
    public const int GolfaCodeMaxLength = 40;
    public const int ModelMaxLength = 100;
    public const int Spec1MaxLength = 400;
    public const int Spec2MaxLength = 400;
    public const int SPOCodeMaxLength = 50;
    public const int CustomerTaxCodeMaxLength = 50;
    public const int CustomerNameMaxLength = 400;
    public const int CustomerTypeMaxLength = 500;
    public const int CustomerIndustryMaxLength = 500;
    public const int NoteMaxLength = 4000;
    public const int AccountNoMaxLength = 50;
    public const int DamagedProductMaxLength = 500;
    public const int ProductSerialNoMaxLength = 500;
    public const int MEVNSellingInvoiceNoMaxLength = 500;
}
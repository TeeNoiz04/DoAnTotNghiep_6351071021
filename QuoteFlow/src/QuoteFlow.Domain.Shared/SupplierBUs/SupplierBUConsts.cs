namespace QuoteFlow.SupplierBUs;

public static class SupplierBUConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "SupplierBU." : string.Empty);
    }

    public const int SupplierBUCodeMaxLength = 200;
    public const int SupplierBURemarksMaxLength = 200;
    public const int OrderMethodMaxLength = 200;
    public const int POTemplateMaxLength = 200;
    public const int ContactMaxLength = 200;
    public const int EmailMaxLength = 200;
    public const int INCOTermMaxLength = 200;
    public const int PaymentTermCodeMaxLength = 200;
    public const int PaymentDescriptionMaxLength = 200;
    public const int CurrencyMaxLength = 50;
    public const int MaterialTypeMaxLength = 200;
    public const int SupplierCodeMaxLength = 200;
    public const int SupplierShortNameMaxLength = 200;
    public const int SupplierAddressMaxLength = 200;
    public const int FASCMVendorCodeMaxLength = 200;
    public const int FASCMBuyerCodeMaxLength = 200;
    public const int FASCMConsigneeCodeMaxLength = 200;
    public const int FASCMSectionCodeMaxLength = 200;
    public const int FASCMPaymentTermMaxLength = 200;
    public const int FASCMFreightMethodMaxLength = 200;
    public const int FASCMDeliveryTermsMaxLength = 200;
    public const int FASCMPlaceOfDeliveryTermsMaxLength = 200;
    public const int FASCMShippingMarkCodeMaxLength = 200;

    public const string SupplierBUImport = "SupplierBU_Data";

    //Cells
    public const string SupplierBUImportStartCell = "B3";
    public const string SupplierBUImportEndCell = "AF30000";

    //Columns
    public const string SupplierBUImportStartColumn = "B";
    public const string SupplierBUImportEndColumn = "AF";
}
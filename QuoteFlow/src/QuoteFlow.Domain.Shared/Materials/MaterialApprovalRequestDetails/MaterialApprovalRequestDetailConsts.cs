namespace QuoteFlow.Materials.MaterialApprovalRequestDetails;

public static class MaterialApprovalRequestDetailConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialApprovalRequestDetail." : string.Empty);
    }

    public const int GolfaCodeMaxLength = 50;
    public const int ModelMaxLength = 100;
    public const int SAP_CodeMaxLength = 50;
    public const int Spec1MaxLength = 4000;
    public const int Spec2MaxLength = 4000;
    public const int Spec3MaxLength = 4000;
    public const int Spec4MaxLength = 4000;
    public const int Description_ENMaxLength = 4000;
    public const int Description_VNMaxLength = 4000;
    public const int MaterialTypeMaxLength = 50;
    public const int UnitMaxLength = 50;
    public const int Material_SEC_ClassificationMaxLength = 4000;
    public const int MaterialGroupMaxLength = 50;
    public const int SAPMatGroupMaxLength = 4000;
    public const int Product_HierarchyMaxLength = 50;
    public const int ProductHierarchyDescriptionMaxLength = 4000;
    public const int CountryOfOriginMaxLength = 200;
    public const int ReferenceLeadTimeMaxLength = 400;
    public const int InventoryCategoryMaxLength = 4000;
    public const int HS_CodeMaxLength = 400;
    public const int SupplierBUCodeMaxLength = 50;
    public const int Factory_TextMaxLength = 400;
    public const int InputCurrencyMaxLength = 50;
    public const int INCOTERMSMaxLength = 200;
    public const int MaterialStatusMaxLength = 50;
    public const int Description_GroupMaxLength = 4000;
    public const int OriginMaxLength = 50;
    public const int KindMaxLength = 20;
    public const int NoteMaxLength = 4000;
    public const int SourceMaxLength = 4000;
    public const int ReasonMaxLength = 4000;
    public const int SupplierCodeMaxLength = 400;
    public const int MaterialClassMaxLength = 400;
    public const int FactoryRefDocMaxLength = 4000;
    public const int CargoNoteMaxLength = 500;
    public const int WeightMaxLength = 500;
    public const int SizeMaxLength = 500;
    public const int QRCodeMaxLength = 500;
    public const int ActionMaxLength = 50;

}
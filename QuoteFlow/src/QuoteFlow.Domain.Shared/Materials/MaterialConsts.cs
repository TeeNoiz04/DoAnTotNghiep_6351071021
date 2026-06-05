namespace QuoteFlow.Materials;

public static class MaterialConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Material." : string.Empty);
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
    public const int MaterialGroupMaxLength = 50;
    public const int UnitMaxLength = 50;
    public const int Material_SEC_ClassificationMaxLength = 4000;
    public const int SAPMatGroupMaxLength = 4000;
    public const int Product_HierarchyMaxLength = 50;
    public const int ProductHierarchyDescriptionMaxLength = 4000;
    public const int CountryOfOriginMaxLength = 200;
    public const int ReferenceLeadTimeMaxLength = 400;
    public const int InventoryCategoryMaxLength = 4000;
    public const int HS_CodeMaxLength = 400;
    public const int InputCurrencyMaxLength = 50;
    public const int SupplierBUMaxLength = 50;
    public const int Factory_nvarcharMaxLength = 400;
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
    public const int CargoNoteMaxLength = 500;
    public const int WeightMaxLength = 500;
    public const int SizeMaxLength = 500;
    public const int QRCodeMaxLength = 500;
    // SheetName
    public const string ExcelImportSheetUpdatePrice = "ImportData"; //M2U
    public const string ExcelImportSheetNewRegistration = "ImportData"; //M1U
    public const string ExcelImportSheetWithoutUpdatePrice = "ImportData"; //M3U
    public const string ExcelImportSheetUpdateInventoryPlan = "ImportData"; //M5U
    public const string ExcelImportSheetMaterialSAP = "ImportData"; //M7U
    public const string ExcelImportSheetMaterialFactory = "ImportData"; //M6U
    public const string ExcelImportSheetMaterialStatus = "ImportData"; //M4U Material Status Update Active

    //Columns
    public const string ExcelMaterialCodeColumn = "A";
    public const string ExcelRegistrationDateColumn = "A";
    public const string ExcelUpdateWithoutStartColumn = "A"; // M3U
    public const string ExcelGolfaCodeColumn = "A";
    public const string ExcelMaterialSAPStartColumn = "A";
    public const string ExcelMaterialFactoryStartColumn = "A";
    public const string ExcelMaterialStatusStartColumn = "A";

    public const string ExcelUpdateWithoutEndColumn = "AE"; // M3U
    public const string ExcelSellingPrice5_NewRegistrationColumn = "AX";

    public const string ExcelSellingPrice5Column = "V";
    public const string ExcelStockWarningColumn = "D";
    public const string ExcelMaterialSAPEndColumn = "F";
    public const string ExcelMaterialFactoryEndColumn = "E"; //M6U
    public const string ExcelMaterialStatusEndColumn = "H";

    // Cells
    public const string ExcelUpdatePriceStartCell = "A2";
    public const string ExcelUpdatePriceEndCell = "V100000";

    public const string ExcelNewRegistrationStartCell = "A4";
    public const string ExcelNewRegistrationEndCell = "AX100000";

    public const string ExcelUpdateWithoutPriceStatCell = "A4"; // M3U 
    public const string ExcelUpdateWithoutPriceEndCell = "AE100000";// M3U

    public const string ExcelUpdateInventoryPlanStatCell = "A2";
    public const string ExcelUpdateInventoryPlanEndCell = "D100000";

    public const string ExcelMaterialSAPStartCell = "A2";
    public const string ExcelMaterialSAPEndCell = "F100000";

    public const string ExcelMaterialFactoryStartCell = "A2";
    public const string ExcelMaterialFactoryEndCell = "E100000";

    public const string ExcelMaterialStatusStartCell = "A2";
    public const string ExcelMaterialStatusEndCell = "H100000";
}
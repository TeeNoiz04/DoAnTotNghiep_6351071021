namespace QuoteFlow.PriceOffers;

public static class PriceOfferConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "PriceOffer." : string.Empty);
    }

    public const int SalePicStepSequence = 10;

    public const int PriceOfferCodeMaxLength = 50;
    public const int MaterialTypeMaxLength = 50;
    public const int LocationOldMaxLength = 4000;
    public const int ProjectNameMaxLength = 4000;
    public const int ApplicationMaxLength = 4000;
    public const int CountryMaxLength = 255;
    public const int ProvinceMaxLength = 4000;
    public const int DetailedAddressMaxLength = 4000;
    public const int CompetitorBrandMaxLength = 4000;
    public const int PriceGapWithCompetitorMaxLength = 4000;
    public const int DecisionRightMaxLength = 4000;
    public const int AccountNoMaxLength = 50;
    public const int FileNameMaxLength = 255;
    public const int UpcomingPotentialProjectsMaxLength = 4000;
    public const int OtherPJInformationMaxLength = 4000;
    public const int ProjectResultStatusMaxLength = 50;
    public const int ProjectResultNoteMaxLength = 4000;
    public const int ProjectResultSubmitterUsernameMaxLength = 100;
    public const int ProjectResultSubmitterFullNameMaxLength = 255;
    public const int BuyerCodeMaxLength = 50;

    // Denormalization fields
    public const int BuyerTypeDescriptionMaxLength = 500;
    public const int ProjectTypeDescriptionMaxLength = 500;
    public const int EUIndustryDescriptionMaxLength = 500;
    public const int KeyAccountClassDescriptionMaxLength = 500;
    public const int KeyAccountTypeDescriptionMaxLength = 500;
    public const int LocationDescriptionMaxLength = 500;

    public const string ProjectPrefix = "PP";
    public const string BuyerPrefix = "DS";
    public const string KeyAccountPrefix = "AP";
    public const string NoBuyerPrefix = "NB";

    public const string SalePICApproverRoleCode = "SalePIC";
    public const string SalePICApproverRoleName = "Sale PIC";

    // SheetName
    public const string ExcelImportSheetAddMoreItem = "ImportData";
    public const string ExcelImportSheetPP = "Special price_PP";
    public const string ExcelImportSheetAP = "Special price_AP";
    public const string ExcelImportSheetDS = "Special price_DS";
    public const string ExcelImportSheetNB = "Special price_NB";
    public const string ExcelImportSheetUpdateLandingCost = "Update Info";

    // Headers
    public const string ExcelModelNameHeader = "Model Name (*)";

    // Columns
    public const string ExcelModelNameColumn = "B";
    public const string ExcelCompetitorPriceColumn = "P";
    public const string ExcelMEVNOfferPrice = "M";
    public const string ExcelSalesChannelColumn = "B";
    public const string ExcelSecCheckColumn = "J";
    public const string ExcelGolfaCodeColumn = "A";
    public const string ExcelNewSaleOfferPriceColumn = "F";

    // Cells
    public const string ExcelCustomerStartCell = "B11";
    public const string ExcelMaterialCodeStartCell = "B5";
    public const string ExcelCompetitorPriceEndCell = "P1000";
    public const string ExcelMEVNOfferPriceEndCell = "M2000";
    public const string ExcelCustomerEndCell = "J100";
    public const string ExcelOfferStartCell = "L11";
    public const string ExcelOfferEndCell = "O22";
    public const string ExcelUpdateLandingCostStartCell = "A3";
    public const string ExcelUpdateLandingCostEndCell = "F1000";
}
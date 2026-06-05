namespace QuoteFlow.PriceOffers;

public static class PriceOfferTypes
{
    public const string PriceOfferPP = "PP"; // Special Price for Project
    public const string PriceOfferDS = "DS"; // Special Price for Buyer
    public const string PriceOfferAP = "AP"; // Special Price for Key Account
    public const string PriceOfferNB = "NB"; // Special Price with No Buyer

    public static readonly string[] AllTypes = { PriceOfferPP, PriceOfferDS, PriceOfferAP, PriceOfferNB };
}

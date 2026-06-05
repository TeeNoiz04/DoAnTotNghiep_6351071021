namespace QuoteFlow.Emailing.EmailInfoModel;

public class PriceOfferEmailInfo
{
    public string ApproverRoleCode { get; set; }
    public string ResultStatus { get; set; }
    public string Status { get; set; }
    public string CreatorName { get; set; }
    public string PriceOfferCode { get; set; }

    public string ProjectName { get; set; }

    public string BuyerName { get; set; }

    public string MaterialType { get; set; }

    public decimal TotalStandard { get; set; }
    public decimal TotalOffer { get; set; }



    public PriceOfferEmailInfo(
        string approverRoleCode,
        string resultStatus,
        string status,
        string creatorName,
        string priceOfferCode,
        string projectName,
        string buyerName,
        string materialType,
        decimal totalStandard,
        decimal totalOffer
    )
    {
        ApproverRoleCode = approverRoleCode;
        ResultStatus = resultStatus;
        Status = status;
        CreatorName = creatorName;
        PriceOfferCode = priceOfferCode;
        ProjectName = projectName;
        BuyerName = buyerName;
        MaterialType = materialType;
        TotalStandard = totalStandard;
        TotalOffer = totalOffer;
    }

    public PriceOfferEmailInfo()
    {

    }
}

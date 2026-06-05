namespace QuoteFlow.Emailing.EmailInfoModel;
public class AssetRequestEmailInfor
{
    public string ApproverRoleCode { get; set; }
    public string ResultStatus { get; set; }
    public string CreatorName { get; set; }
    public string RequestCode { get; set; }

    public string RequestType { get; set; }






    public AssetRequestEmailInfor()
    {

    }

    public AssetRequestEmailInfor(string approverRoleCode, string resultStatus, string creatorName, string requestCode, string requestType)
    {
        ApproverRoleCode = approverRoleCode;
        ResultStatus = resultStatus;
        CreatorName = creatorName;
        RequestCode = requestCode;
        RequestType = requestType;
    }
}

namespace QuoteFlow.Emailing.EmailInfoModel;
public class KeyAccountEmailInfo
{
    public string Status { get; set; }
    public string CreatorName { get; set; }
    public string KeyAccountCode { get; set; }
    public string KeyAccountName { get; set; }
    public string BuyerName { get; set; }
    public string TaxCode { get; set; }
    public string KeyAccountBuyerTypeName { get; set; }
    public string KeyAccountBuyerClassName { get; set; }
    public string KeyAccountTypeName { get; set; }
    public string KeyAccountClassName { get; set; }
    public string MaterialType { get; set; }


    public KeyAccountEmailInfo(
        string status,
        string creatorName,
        string keyAccountCode,
        string keyAccountName,
        string taxCode,
        string buyerName,
        string buyerTypeName,
        string buyerClassName,
        string keyAccountTypeName,
        string keyAccountClassName,
        string materialType
    )
    {
        Status = status;
        CreatorName = creatorName;
        KeyAccountCode = keyAccountCode;
        KeyAccountName = keyAccountName;
        TaxCode = taxCode;
        BuyerName = buyerName;
        KeyAccountBuyerTypeName = buyerTypeName;
        KeyAccountBuyerClassName = buyerClassName;
        KeyAccountTypeName = keyAccountTypeName;
        KeyAccountClassName = keyAccountClassName;
        MaterialType = materialType;
    }

    public KeyAccountEmailInfo()
    {

    }
}

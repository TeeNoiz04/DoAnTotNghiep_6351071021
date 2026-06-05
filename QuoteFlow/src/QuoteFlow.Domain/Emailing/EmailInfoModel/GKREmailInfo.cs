using QuoteFlow.Shared.Extensions;
using System;

namespace QuoteFlow.Emailing.EmailInfoModel;

public class GKREmailInfo
{
    public string Status { get; set; }
    public string CreatorName { get; set; }
    public string No { get; set; }
    public string MaterialType { get; set; }
    public string BuyerName { get; set; }
    public string ExpirationDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Note { get; set; }

    public GKREmailInfo(
        string status,
        string creatorName,
        string no,
        string materialType,
        string buyerName,
        string note,
        DateTime expirationDate,
        decimal totalAmount
    )
    {
        Status = status;
        CreatorName = creatorName;
        No = no;
        MaterialType = materialType;
        BuyerName = buyerName;
        Note = note;
        ExpirationDate = expirationDate.ToDateOrDateTimeString();
        TotalAmount = totalAmount;
    }

    public GKREmailInfo() { }
}

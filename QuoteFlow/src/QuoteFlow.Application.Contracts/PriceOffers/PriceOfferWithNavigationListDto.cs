using QuoteFlow.ApprovalHistories;
using QuoteFlow.Buyers;
using QuoteFlow.SystemCategories;
using System.Collections.Generic;

namespace QuoteFlow.PriceOffers;

public class PriceOfferWithNavigationListDto : PriceOfferListDto
{
    public ICollection<ApprovalHistoryDto> ApprovalHistories { get; set; } = [];
    public BuyerListDto Buyer { get; set; } = null!;
    public SystemCategoryListDto BuyerType { get; set; } = null!;
}

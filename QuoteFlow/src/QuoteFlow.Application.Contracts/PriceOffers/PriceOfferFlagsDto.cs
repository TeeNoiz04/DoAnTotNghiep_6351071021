using QuoteFlow.Shared.Flagging;

namespace QuoteFlow.PriceOffers;

public class PriceOfferFlagsDto : IBaseFlags, IHasApprovalFlags
{
    public bool IsEditable { get; set; }
    public bool IsRemovable { get; set; }
    public bool IsViewable { get; set; }

    public bool IsApprovable { get; set; }
    public bool IsRejectable { get; set; }
    public bool IsCancellable { get; set; }
    public bool IsClosable { get; set; }
    public bool IsProjectResultSubmittable { get; set; }
    public bool IsPreOrderResultConfirmable { get; set; }
    public bool IsGPViewable { get; set; }
    public bool IsLandedCostViewable { get; set; }
    public bool IsDetailPropertiesChangeable { get; set; }
    public bool CanAddMoreItems { get; set; }
    public bool IsSpecialInputPriceApplicable { get; set; }
    public bool IsSpecialInputPriceViewable { get; set; }

    public bool IsDetailsPropertiesTemplateDownloadable { get; set; } = false;
    public bool CanCancelItem { get; set; } = false;
}

using QuoteFlow.Shared.Flagging;

namespace QuoteFlow.SaleOrders;

public class SaleOrderFlagsDto : IBaseFlags
{
    public bool IsEditable { get; set; }
    public bool IsRemovable { get; set; }
    public bool IsViewable { get; set; }
    public bool CanConfirmDelivery { get; set; }
    public bool CanReOpenSO { get; set; }
    public bool CanEditSAPInfo { get; set; }
}
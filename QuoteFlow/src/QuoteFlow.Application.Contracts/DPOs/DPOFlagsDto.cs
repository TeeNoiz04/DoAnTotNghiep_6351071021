using QuoteFlow.Shared.Flagging;

namespace QuoteFlow.DPOs;

public class DPOFlagsDto : IBaseFlags
{
    public bool IsEditable { get; set; }
    public bool IsRemovable { get; set; }
    public bool IsViewable { get; set; }
    public bool CanLockStock { get; set; }
    public bool CanLockOnOrder { get; set; }
    public bool CanConfirmLockStock { get; set; }
    public bool CanConfirmLockOnOrder { get; set; }
    public bool CanApprove { get; set; }
    public bool CanReject { get; set; }
    public bool CanEditItem { get; set; }
    public bool CanDelete { get; set; }
    public bool CanAllocateGKR { get; set; }
}
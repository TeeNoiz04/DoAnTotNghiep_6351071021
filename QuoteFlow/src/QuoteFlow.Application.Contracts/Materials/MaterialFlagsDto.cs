using QuoteFlow.Shared.Flagging;

namespace QuoteFlow.Materials;

public class MaterialFlagsDto : IBaseFlags, IHasApprovalFlags
{
    public bool IsEditable { get; set; }
    public bool IsRemovable { get; set; }
    public bool IsViewable { get; set; }

    public bool IsApprovable { get; set; }
    public bool IsRejectable { get; set; }
    public bool IsCancellable { get; set; }
}

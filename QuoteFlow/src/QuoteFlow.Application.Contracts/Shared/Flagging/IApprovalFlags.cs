namespace QuoteFlow.Shared.Flagging;

public interface IHasApprovalFlags
{
    bool IsApprovable { get; set; }
    bool IsRejectable { get; set; }
    bool IsCancellable { get; set; }
}

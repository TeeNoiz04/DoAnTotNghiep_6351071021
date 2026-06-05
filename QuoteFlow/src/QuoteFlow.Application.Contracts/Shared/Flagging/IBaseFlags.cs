namespace QuoteFlow.Shared.Flagging;

public interface IBaseFlags
{
    public bool IsEditable { get; set; }
    public bool IsRemovable { get; set; }
    public bool IsViewable { get; set; }
}

namespace QuoteFlow.Shared;

/// <summary>
/// Represents a material combination for price offer validation with row tracking
/// </summary>
public class MaterialFullCombinationWithRowIndexDto : MaterialFullCombinationDto
{
    /// <summary>
    /// Excel row index for error reporting
    /// </summary>
    public int RowIndex { get; set; } = 0;

    public int? AppliedPrice { get; set; }
    public string? Spec1 { get; set; }
}

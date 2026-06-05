namespace QuoteFlow.Shared;

/// <summary>
/// Represents a material combination for price offer validation with row tracking
/// </summary>
public class MaterialBasicCombinationWithRowIndexDto : MaterialBasicCombinationDto
{
    /// <summary>
    /// Excel row index for error reporting
    /// </summary>
    public int RowIndex { get; set; } = 0;
}

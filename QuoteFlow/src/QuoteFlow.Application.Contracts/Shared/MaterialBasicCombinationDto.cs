using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Shared;

/// <summary>
/// Represents a material combination for price offer validation with row tracking
/// </summary>
public class MaterialBasicCombinationDto
{
    /// <summary>
    /// Golfa code identifier
    /// </summary>
    [Required]
    public string GolfaCode { get; set; } = string.Empty;

    /// <summary>
    /// Model name
    /// </summary>
    [Required]
    public string ModelName { get; set; } = string.Empty;
}

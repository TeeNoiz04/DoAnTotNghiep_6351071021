using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Shared;

/// <summary>
/// Represents a material combination for price offer validation with row tracking
/// </summary>
public class MaterialFullCombinationDto : MaterialBasicCombinationDto
{
    /// <summary>
    /// Type of material
    /// </summary>
    [Required]
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// Standard price of the material
    /// </summary>
    [Required]
    public decimal StandardPrice { get; set; }
}
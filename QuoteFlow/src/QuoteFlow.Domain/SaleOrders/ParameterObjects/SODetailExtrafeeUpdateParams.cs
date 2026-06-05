using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SaleOrders.ParameterObjects;
public class SODetailExtrafeeUpdateParams
{
    [Required]
    public Guid SODetailId { get; set; }
    [Required]
    public decimal Extrafee { get; set; }
    [Required]
    public string ExtrafeeNode { get; set; }
    public string? UserName { get; set; }
    public string? UserFullName { get; set; }
}

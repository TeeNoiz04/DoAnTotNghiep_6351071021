using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SaleOrders;
public class SODetailExtrafeeUpdateInput
{
    [Required]
    public Guid SODetailId { get; set; }
    [Required]
    public decimal Extrafee { get; set; }
    [Required]
    public string ExtrafeeNode { get; set; }
}

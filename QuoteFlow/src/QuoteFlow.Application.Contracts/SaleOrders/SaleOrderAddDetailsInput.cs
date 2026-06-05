using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SaleOrders;
public class SaleOrderAddDetailsInput : PagedAndSortedResultRequestDto
{
    [Required]
    public Guid BuyerId { get; set; }

    [Required]
    public Guid StockCategoryId { get; set; }
    [Required]
    public decimal VAT { get; set; }
}

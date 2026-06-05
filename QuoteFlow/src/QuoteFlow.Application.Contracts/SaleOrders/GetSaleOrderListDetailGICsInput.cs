using QuoteFlow.GICs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SaleOrders;

public class GetSaleOrderListDetailGICsInput : PagedAndSortedResultRequestDto
{
    [Required]
    public string GICType { get; set; } = null!;

    public string? GICProcess { get; set; }

    [Required]
    public Guid BuyerId { get; set; }

    [Required]
    public string MaterialType { get; set; } = null!;
    public decimal? VAT { get; set; } = null;

    [Required]
    public Guid StockCategoryId { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
        {
            yield return result;
        }

        if ((GICType == GICTypeCodes.Internal || GICType == GICTypeCodes.Warranty)
            && string.IsNullOrEmpty(GICProcess))
        {
            yield return new ValidationResult($"GICProcess is required when GICType is '{GICTypeCodes.Internal}' or '{GICTypeCodes.Warranty}'", new[] { nameof(GICProcess) });
        }
    }
}


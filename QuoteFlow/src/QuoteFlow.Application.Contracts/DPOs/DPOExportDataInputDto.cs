using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;

public class DPOExportDataInputDto : IValidatableObject
{
    public string? DPONo { get; set; }
    public string? Status { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? PONo { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? BuyerTypeId { get; set; }
    public string? BuyerId { get; set; }
    public string? MaterialType { get; set; }
    public string? SupplierCode { get; set; }
    public string? SPOCode { get; set; }
    public string? TaxCode { get; set; }
    public string? MaterialGroup { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FromDate.HasValue && ToDate.HasValue)
        {
            if (ToDate < FromDate)
            {
                yield return new ValidationResult("To Date must be greater than or equal to From Date", new[] { nameof(ToDate), nameof(FromDate) });
            }
            //else if ((ToDate.Value - FromDate.Value).TotalDays > 366)
            //{
            //    yield return new ValidationResult("The date range between From Date and To Date must not exceed 1 year", new[] { nameof(ToDate), nameof(FromDate) });
            //}
        }
    }
}
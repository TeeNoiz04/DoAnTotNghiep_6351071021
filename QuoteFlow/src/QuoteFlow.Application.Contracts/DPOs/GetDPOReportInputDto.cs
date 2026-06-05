using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;
public class GetDPOReportInputDto : IValidatableObject
{
    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }

    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // to date - from date must be in a year
        if (ToDate < FromDate)
        {
            yield return new ValidationResult("ToDate must be greater than or equal to FromDate.", new[] { nameof(ToDate) });
        }
        if ((ToDate - FromDate).TotalDays > 365)
        {
            yield return new ValidationResult("The date range must not exceed one year.", new[] { nameof(FromDate), nameof(ToDate) });
        }
    }
}

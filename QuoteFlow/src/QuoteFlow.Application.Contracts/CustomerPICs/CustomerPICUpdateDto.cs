using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.CustomerPICs;

public class CustomerPICUpdateDto : IHasConcurrencyStamp
{
    [Required]
    public Guid KeyAccountId { get; set; }
    [StringLength(CustomerPICConsts.PICNameMaxLength)]
    public string? PICName { get; set; }
    [StringLength(CustomerPICConsts.PIC_PhoneMaxLength)]
    public string? PICPhone { get; set; }
    [StringLength(CustomerPICConsts.PIC_EmailMaxLength)]
    public string? PICEmail { get; set; }
    [StringLength(CustomerPICConsts.PIC_JobTitleMaxLength)]
    public string? PICJobTitle { get; set; }
    [StringLength(CustomerPICConsts.RemarkMaxLength)]
    public string? Remark { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
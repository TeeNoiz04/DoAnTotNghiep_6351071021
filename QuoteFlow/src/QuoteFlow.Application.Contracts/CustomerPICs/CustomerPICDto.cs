using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.CustomerPICs;

public class CustomerPICDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid KeyAccountId { get; set; }
    public string? PICName { get; set; }
    public string? PICPhone { get; set; }
    public string? PICEmail { get; set; }
    public string? PICJobTitle { get; set; }
    public string? Remark { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}
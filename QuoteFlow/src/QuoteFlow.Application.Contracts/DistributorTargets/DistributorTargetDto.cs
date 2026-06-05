using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.DistributorTargets;

public class DistributorTargetDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid? BuyerTypeId { get; set; }
    public Guid BuyerId { get; set; }
    public string? BuyerCode { get; set; }
    public string? BuyerName { get; set; }
    public string MaterialType { get; set; } = null!;
    public int? FinanceYear { get; set; }
    public decimal? FirstFYTarget { get; set; }
    public decimal? SecondFYTarget { get; set; }
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}
using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.CfgDiscountRatios
{
    public class CfgDiscountRatioDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? Approval_Type { get; set; }
        public string? Product_Type { get; set; }
        public string? AccountClassify { get; set; }
        public string? KAType { get; set; }
        public decimal? Value_Min { get; set; }
        public decimal? Value_Max { get; set; }
        public decimal? DiscountRatio { get; set; }
        public string? Note { get; set; }
        public string ConcurrencyStamp { get; set; } = null!;

    }
}
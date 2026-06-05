using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;

namespace QuoteFlow.CfgDiscountRatios
{
    public class CfgDiscountRatio : ExtendedAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string? Approval_Type { get; set; }

        [CanBeNull]
        public virtual string? Product_Type { get; set; }

        [CanBeNull]
        public virtual string? AccountClassify { get; set; }

        [NotMapped]
        public virtual string? KAType { get; set; }

        public virtual decimal? Value_Min { get; set; }

        public virtual decimal? Value_Max { get; set; }

        public virtual decimal? DiscountRatio { get; set; }

        [CanBeNull]
        public virtual string? Note { get; set; }

        public CfgDiscountRatio()
        {

        }

        public CfgDiscountRatio(Guid id, string? approval_Type = null, string? product_Type = null, string? accountClassify = null, decimal? value_Min = null, decimal? value_Max = null, decimal? discountRatio = null, string? note = null)
        {

            Id = id;
            Check.Length(approval_Type, nameof(approval_Type), CfgDiscountRatioConsts.Approval_TypeMaxLength, 0);
            Check.Length(product_Type, nameof(product_Type), CfgDiscountRatioConsts.Product_TypeMaxLength, 0);
            Check.Length(accountClassify, nameof(accountClassify), CfgDiscountRatioConsts.AccountClassifyMaxLength, 0);
            Approval_Type = approval_Type;
            Product_Type = product_Type;
            AccountClassify = accountClassify;
            Value_Min = value_Min;
            Value_Max = value_Max;
            DiscountRatio = discountRatio;
            Note = note;
        }

    }
}
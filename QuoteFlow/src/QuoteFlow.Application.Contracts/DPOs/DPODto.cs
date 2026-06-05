using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.Shared;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.DPOs;

public class DPODto : ExtendedFullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string? DPONo { get; set; }
    public string? GICNo { get; set; }
    public string? DPOType { get; set; }
    public string? GICType { get; set; }
    public string? MaterialType { get; set; }
    public string? CostCenter { get; set; }
    public string? Status { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerShortName { get; set; }
    public string? BuyerDescription { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalAmountIncludeExtraFee { get; set; }
    public string? Remark { get; set; }
    public string? FileName { get; set; }
    public virtual string? ReferenceDoc { get; set; }

    public virtual string? GICProcess { get; set; }
    public virtual string? ConfirmNoted { get; set; }
    public List<DPODetailDto>? Details { get; set; }
    public DPOFlagsDto Flags { get; set; } = null!;
    public string ConcurrencyStamp { get; set; } = null!;
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }

}
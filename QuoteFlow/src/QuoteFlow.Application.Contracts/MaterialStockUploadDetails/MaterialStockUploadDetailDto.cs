using System;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.MaterialStockUploadDetails;

public class MaterialStockUploadDetailDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid RequestId { get; set; }
    public string MaterialCode { get; set; } = null!;
    public string? Model { get; set; }
    public string? Storage { get; set; }
    public string? StorageDestination { get; set; }
    public decimal? Qty { get; set; }
    public string? RefDoc { get; set; }
    public string? Remark { get; set; }
    public Guid? StorageDesc_Id { get; set; } = null;
    public Guid? StorageSrc_Id { get; set; } = null;
    public string ConcurrencyStamp { get; set; } = null!;

}
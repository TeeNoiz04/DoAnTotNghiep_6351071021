using System;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.MaterialStockUploads;

public class MaterialStockUploadDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string? RequestNo { get; set; }
    public string? ImportType { get; set; }
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}
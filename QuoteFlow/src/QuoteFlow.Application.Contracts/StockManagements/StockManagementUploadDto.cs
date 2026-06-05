using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.Shared;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.StockManagements;
public class StockManagementUploadDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string? RequestNo { get; set; }
    public string? ImportType { get; set; }
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public List<MaterialStockUploadDetailDto>? MaterialStockUploadDetails { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;

}
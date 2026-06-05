using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SpoBatchRequests;

public class SpoBatchRequestDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string RequestNo { get; set; } = null!;
    public string ImportType { get; set; } = null!;
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public List<SpoBatchRequestDetailDto>? SpoBatchRequestDetails { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;

}
using System;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;

public class SpoBatchRequestDetailDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid RequestId { get; set; }
    public string? SPOCode { get; set; }
    public string? GolfaCode { get; set; }
    public string? Action { get; set; }
    public DateTime? ActionDate { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;

}
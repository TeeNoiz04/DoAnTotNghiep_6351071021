using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.ApprovalRoutes;

public class ApproverDto : EntityDto<Guid>
{
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? RoleCode { get; set; }
    public string? RoleName { get; set; }
}

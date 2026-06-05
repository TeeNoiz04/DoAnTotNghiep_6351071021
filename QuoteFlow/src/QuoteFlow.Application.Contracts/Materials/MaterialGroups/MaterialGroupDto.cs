using System;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialGroups;

public class MaterialGroupDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid? Parent { get; set; }
    public int SortOrder { get; set; }
    public string? Note { get; set; }
    public bool IsDeActive { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialGroupPSI { get; set; }
    public bool AllowKeyAccount { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}
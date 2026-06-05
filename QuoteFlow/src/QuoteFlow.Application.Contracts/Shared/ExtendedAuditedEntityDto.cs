using QuoteFlow.Shared.Interfaces;
using MiniExcelLibs.Attributes;
using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Shared;

public class ExtendedAuditedEntityDto<TKey> : EntityDto<TKey>, IExtendedAuditedObject
{
    [ExcelIgnore]
    public Guid? CreatorId { get; set; }

    [ExcelIgnore]
    public string? CreatorUsername { get; set; }

    [ExcelIgnore]
    public string? CreatorName { get; set; }

    [ExcelIgnore]
    public DateTime? CreationTime { get; set; }

    [ExcelIgnore]
    public Guid? LastModifierId { get; set; }

    [ExcelIgnore]
    public string? LastModifierUsername { get; set; }

    [ExcelIgnore]
    public string? LastModifierName { get; set; }

    [ExcelIgnore]
    public DateTime? LastModificationTime { get; set; }
}
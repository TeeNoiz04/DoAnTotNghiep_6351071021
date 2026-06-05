using QuoteFlow.Shared;
using System;
using System.Text.Json.Serialization;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Suppliers;

public class SupplierDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string SupplierCode { get; set; } = null!;
    [JsonPropertyName("sapCode")]
    public string? SAPCode { get; set; }
    public string ShortName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    public bool? IsDeactive { get; set; } = false;
    public string ConcurrencyStamp { get; set; } = null!;
}

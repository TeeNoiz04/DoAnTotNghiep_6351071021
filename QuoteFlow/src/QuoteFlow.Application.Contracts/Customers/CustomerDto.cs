using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Customers;

public class CustomerDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public virtual string TaxCode { get; set; }

    public virtual string CustomerName { get; set; }

    public virtual string? CustomerShortName { get; set; }

    public virtual string? Address { get; set; }

    public virtual string? Phone { get; set; }

    public virtual string? Country { get; set; }

    public virtual string? Province { get; set; }

    public virtual string? Website { get; set; }

    public virtual string? CustomerType { get; set; }

    public virtual string? CustomerIndustry { get; set; }

    public virtual string? Note { get; set; }

    public virtual bool IsDeactive { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
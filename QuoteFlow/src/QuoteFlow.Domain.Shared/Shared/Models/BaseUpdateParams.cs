using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Shared.Models;

public class BaseUpdateParams : IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = null!;
}
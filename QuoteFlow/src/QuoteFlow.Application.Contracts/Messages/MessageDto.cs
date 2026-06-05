using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Messages;

public class MessageDto : ExtendedFullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string UserName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string SendTo { get; set; } = null!;
    public string Comment { get; set; } = null!;

    public string ConcurrencyStamp { get; set; } = null!;

}
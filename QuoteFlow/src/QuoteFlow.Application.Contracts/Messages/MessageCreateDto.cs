using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Messages;

public class MessageCreateDto
{
    [Required]
    [StringLength(MessageConsts.UserNameMaxLength)]
    public string UserName { get; set; } = null!;
    [Required]
    [StringLength(MessageConsts.FullNameMaxLength)]
    public string FullName { get; set; } = null!;
    [Required]
    public IEnumerable<string> SendToEmails { get; set; } = null!;
    [Required]
    public string Comment { get; set; } = null!;
}
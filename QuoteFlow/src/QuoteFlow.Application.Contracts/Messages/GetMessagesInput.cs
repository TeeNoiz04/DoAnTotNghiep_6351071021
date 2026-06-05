using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Messages;

public class GetMessagesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? SendTo { get; set; }
    public string? Comment { get; set; }

    public GetMessagesInput()
    {

    }
}
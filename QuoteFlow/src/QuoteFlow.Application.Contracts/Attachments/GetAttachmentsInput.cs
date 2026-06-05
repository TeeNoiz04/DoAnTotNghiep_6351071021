using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Attachments;

public class GetAttachmentsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? RequestPart { get; set; }
    public string? AttachCode { get; set; }
    public string? AttachName { get; set; }
    public string? FileName { get; set; }
    public string? FileNameDB { get; set; }
    public string? FilePath { get; set; }
    public bool? OfflineAttachment { get; set; }
    public string? Description { get; set; }

    public GetAttachmentsInput()
    {

    }
}
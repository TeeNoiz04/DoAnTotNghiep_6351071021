using System.Collections.Generic;
using Volo.Abp.Content;

namespace QuoteFlow.Attachments;

public class FilesInput
{
    public List<IRemoteStreamContent> Files { get; set; }
}

using Asp.Versioning;
using QuoteFlow.Attachments;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.Attachments;
[RemoteService]
[Area("app")]
[ControllerName("Attachment")]
[Route("api/app/attachments")]
[RequestSizeLimit(2L * 1024 * 1024 * 1024)]
public class AttachmentController : AbpController
{
    protected IAttachmentsAppService _attachmentsAppService;

    public AttachmentController(IAttachmentsAppService attachmentsAppService)
    {
        _attachmentsAppService = attachmentsAppService;
    }

    // Upload file 
    [HttpPost]
    [Route("upload")]
    public Task UploadFileAsync([FromForm] FilesInput input, Guid requestId, string attachmentCode)
    {
        return _attachmentsAppService.UploadFileAsync(input.Files, requestId, attachmentCode);
    }

    // Download file 
    [HttpGet]
    [Route("download")]
    public async Task<IActionResult> DownloadFileAsync(Guid id)
    {
        // Call the AppService to get the file details
        var fileData = await _attachmentsAppService.GetFileByIdAsync(id);

        if (fileData == null || fileData.Content == null)
        {
            return NotFound("File not found");
        }

        // Return the file as a response
        return File(fileData.Content, fileData.ContentType, fileData.FileName);
    }

    [HttpDelete]
    [Route("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _attachmentsAppService.DeleteFileAsync(id);
    }
}
using Asp.Versioning;
using QuoteFlow.Messages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.Messages;

[RemoteService]
[Area("app")]
[ControllerName("Message")]
[Route("api/app/discussions")]

public class MessageController : AbpController, IMessagesAppService
{
    protected IMessagesAppService _discussionsAppService;

    public MessageController(IMessagesAppService discussionsAppService)
    {
        _discussionsAppService = discussionsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<MessageDto>> GetListAsync(GetMessagesInput input)
    {
        return _discussionsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<MessageDto> GetAsync(Guid id)
    {
        return _discussionsAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<MessageDto> CreateAsync(MessageCreateDto input)
    {
        return _discussionsAppService.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _discussionsAppService.DeleteAsync(id);
    }
}
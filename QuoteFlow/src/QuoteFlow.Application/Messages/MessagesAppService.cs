using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Messages;

[RemoteService(IsEnabled = false)]

public class MessagesAppService : QuoteFlowAppService, IMessagesAppService
{

    protected IMessageRepository _discussionRepository;
    protected MessageManager _discussionManager;

    public MessagesAppService(IMessageRepository discussionRepository, MessageManager discussionManager)
    {

        _discussionRepository = discussionRepository;
        _discussionManager = discussionManager;

    }

    public virtual async Task<PagedResultDto<MessageDto>> GetListAsync(GetMessagesInput input)
    {
        var totalCount = await _discussionRepository.GetCountAsync(input.FilterText, input.UserName, input.FullName, input.SendTo, input.Comment);
        var items = await _discussionRepository.GetListAsync(input.FilterText, input.UserName, input.FullName, input.SendTo, input.Comment, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<MessageDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Message>, List<MessageDto>>(items)
        };
    }

    public virtual async Task<MessageDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Message, MessageDto>(await _discussionRepository.GetAsync(id));
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        await _discussionRepository.DeleteAsync(id);
    }

    public virtual async Task<MessageDto> CreateAsync(MessageCreateDto input)
    {

        var discussion = await _discussionManager.CreateAsync(
        input.UserName, input.FullName, input.SendToEmails, input.Comment
        );

        return ObjectMapper.Map<Message, MessageDto>(discussion);
    }
}
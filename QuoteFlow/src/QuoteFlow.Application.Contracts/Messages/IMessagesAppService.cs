using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.Messages;

public interface IMessagesAppService : IApplicationService
{

    Task<PagedResultDto<MessageDto>> GetListAsync(GetMessagesInput input);

    Task<MessageDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<MessageDto> CreateAsync(MessageCreateDto input);
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Messages;

public class MessageManager : DomainService
{
    protected IMessageRepository _discussionRepository;

    public MessageManager(IMessageRepository discussionRepository)
    {
        _discussionRepository = discussionRepository;
    }

    public virtual async Task<Message> CreateAsync(
    string userName, string fullName, IEnumerable<string> sendToEmails, string comment)
    {
        Check.NotNullOrWhiteSpace(userName, nameof(userName));
        Check.Length(userName, nameof(userName), MessageConsts.UserNameMaxLength);
        Check.NotNullOrWhiteSpace(fullName, nameof(fullName));
        Check.Length(fullName, nameof(fullName), MessageConsts.FullNameMaxLength);
        Check.NotNullOrWhiteSpace(comment, nameof(comment));

        var discussion = new Message(
            GuidGenerator.Create(),
            userName, fullName, sendToEmails, comment
        );

        return await _discussionRepository.InsertAsync(discussion);
    }
}
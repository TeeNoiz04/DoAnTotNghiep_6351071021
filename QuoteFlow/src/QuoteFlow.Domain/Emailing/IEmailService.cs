using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace QuoteFlow.Emailing;

public interface IEmailService : ISingletonDependency
{
    Task SendAsync<T>(string to, string subject, T emailModel, List<string>? cc = null, List<EmailAttachment>? attachments = null)
        where T : IEmailModel;

    Task SendAsync<T>(List<string> to, string subject, T emailModel, List<string>? cc = null, List<EmailAttachment>? attachments = null)
        where T : IEmailModel;
}
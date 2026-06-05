using QuoteFlow.Shared.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;

namespace QuoteFlow.Emailing.Services;

[DependsOn(
    typeof(IEmailSender),
    typeof(ITemplateRenderer)
)]
public class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IEmailSender emailSender, ITemplateRenderer templateRenderer, ILogger<EmailService> logger)
    {
        _emailSender = emailSender;
        _templateRenderer = templateRenderer;
        //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        _logger = logger;
    }

    public async Task SendAsync<T>(string to, string subject, T emailModel, List<string>? cc = null, List<EmailAttachment>? attachments = null)
    where T : IEmailModel
    {
        await SendEmailAsync(
            [to],
            subject,
            emailModel,
            cc,
            attachments
        );
    }

    public async Task SendAsync<T>(List<string> to, string subject, T emailModel, List<string>? cc = null, List<EmailAttachment>? attachments = null)
        where T : IEmailModel
    {
        await SendEmailAsync(to, subject, emailModel, cc, attachments);
    }

    private async Task SendEmailAsync<T>(List<string> to, string subject, T emailModel, List<string>? cc = null, List<EmailAttachment>? attachments = null)
        where T : IEmailModel
    {
        var body = await _templateRenderer.RenderAsync(
            emailModel.TemplateName,
            emailModel.TemplateDataModel
        );

        var recipients = string.Join(", ", to);
        _logger.LogInformation("[EmailSender] Sending email to {Recipients}, subject: {Subject} at {Time}",
            recipients, subject, DateTime.Now.ToStandardString());

        // Create mail message
        var mailMessage = new MailMessage()
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        // Add recipients
        foreach (var recipient in to)
        {
            mailMessage.To.Add(recipient);
        }

        // Add CC if provided
        if (cc != null)
        {
            foreach (var recipient in cc)
            {
                mailMessage.CC.Add(recipient);
            }
        }

        // Add attachments if provided
        if (attachments != null && attachments.Count > 0)
        {
            foreach (var attachment in attachments)
            {
                if (attachment.File == null || attachment.Name == null)
                {
                    _logger.LogWarning("[EmailSender] Skipping invalid attachment with null file or name");
                    continue;
                }

                var attachmentStream = new MemoryStream(attachment.File);
                mailMessage.Attachments.Add(new Attachment(attachmentStream, attachment.Name));
            }
        }

        await SendWithRetryAsync(mailMessage, recipients, subject);
    }

    private async Task SendWithRetryAsync(MailMessage mailMessage, string recipients, string subject)
    {
        int retryCount = 0;
        const int maxRetries = 8;

        while (retryCount < maxRetries)
        {
            try
            {
                if (retryCount > 0)
                {
                    _logger.LogWarning("[Retry {RetryCount}] Retrying to send email to {Recipients}, subject {Subject} at {Time}",
                        retryCount, recipients, subject, DateTime.Now.ToStandardString());
                }

                await _emailSender.SendAsync(mailMessage);

                _logger.LogInformation("[Retry Count: {RetryCount}] Successfully sent email to {Recipients}, subject {Subject} at {Time}",
                    retryCount, recipients, subject, DateTime.Now.ToStandardString());
                return; // Success
            }
            catch (Exception ex)
            {
                if (ShouldRetry(ex))
                {
                    retryCount++;
                    var delay = TimeSpan.FromSeconds(5 * retryCount + Random.Shared.NextDouble());
                    await Task.Delay(delay);
                    continue;
                }

                _logger.LogError(ex, "Unexpected error while sending email");
                return;
            }
        }

        // Max retries reached
        _logger.LogError("Failed to send email to {Recipients}, subject {Subject} after {Retries} retries",
            recipients, subject, maxRetries);
    }

    private bool ShouldRetry(Exception ex)
    {
        if (ex is SmtpException smtp && IsTransient(smtp)) return true;
        if (ex.Message?.Contains("Concurrent connections limit exceeded") == true) return true;
        return false;
    }

    private bool IsTransient(SmtpException ex)
    {
        // Microsoft usually responds with 421, 451 or 4xx series for temp issues
        return ex.StatusCode == SmtpStatusCode.ServiceNotAvailable ||
               ex.StatusCode == SmtpStatusCode.MailboxBusy ||
               (int)ex.StatusCode >= 400 && (int)ex.StatusCode < 500;
    }
}
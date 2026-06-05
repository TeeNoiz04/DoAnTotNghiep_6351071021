using QuoteFlow.Emailing;
using QuoteFlow.Emailing.EmailModels;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.NamingPolicies;
using QuoteFlow.Shared.Utils;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace QuoteFlow.BackgroundJobs.Emailing;

public class SendEmailQuartzJob : IJob, ITransientDependency
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailQuartzJob> _logger;

    public SendEmailQuartzJob(IEmailService emailService, ILogger<SendEmailQuartzJob> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [UnitOfWork]
    public virtual async Task Execute(IJobExecutionContext context)
    {
        var jobData = context.JobDetail.JobDataMap;

        // Extract job arguments from JobDataMap
        var to = jobData.GetString("To")?.Split(',').ToList() ?? [];
        var subject = jobData.GetString("Subject") ?? string.Empty;
        var emailModelJson = jobData.GetString("EmailModelJson") ?? string.Empty;
        var emailModelType = jobData.GetString("EmailModelType") ?? string.Empty;
        var ccRecipients = jobData.GetString("CcRecipients")?.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList() ?? [];

        _logger.LogInformation("[SendEmailQuartzJob] Sending email to {To} with subject: {Subject}",
            string.Join(", ", to), subject);

        try
        {
            var modelType = ModelTypeResolver.ResolveModelType(emailModelType);
            var emailModel = DeserializeEmailModel(emailModelJson, modelType);

            if (emailModel is IHasApprovalHistoryEmailModel historyEmailModel)
            {
                historyEmailModel.ApprovalHistories =
                    [.. (historyEmailModel.ApprovalHistories ?? [])
                        .OrderByDescending(x =>
                            DateTime.ParseExact(x.ActionDate, DateTimeExtensions.StandardFormat, CultureInfo.InvariantCulture)
                        )];
            }

            await _emailService.SendAsync(to, subject, emailModel, cc: ccRecipients);

            _logger.LogInformation("[SendEmailQuartzJob] Email sent successfully to {To}", string.Join(", ", to));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SendEmailQuartzJob] Failed to send email to {To}", string.Join(", ", to));
            throw; // Re-throw to let Quartz handle retry logic
        }
    }

    private IEmailModel DeserializeEmailModel(string emailModelJson, Type modelType)
    {
        var result = JsonSerializer.Deserialize(
            emailModelJson,
            modelType,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = new PascalCaseJsonNamingPolicy(),
                PropertyNameCaseInsensitive = true
            }) as IEmailModel;

        if (result == null)
        {
            _logger.LogError("Failed to deserialize email model of type {ModelType}, emailModelJson: {EmailModelJson}",
                modelType.FullName, emailModelJson);
        }

        return result ?? new NullEmailModel();
    }
}
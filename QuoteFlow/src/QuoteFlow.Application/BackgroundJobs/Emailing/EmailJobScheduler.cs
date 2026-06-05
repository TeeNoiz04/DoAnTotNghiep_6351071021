using QuoteFlow.Emailing;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace QuoteFlow.BackgroundJobs.Emailing;

public class EmailJobScheduler : IEmailJobScheduler, ITransientDependency
{
    private readonly IScheduler _scheduler;
    private readonly ILogger<EmailJobScheduler> _logger;

    public EmailJobScheduler(IScheduler scheduler, ILogger<EmailJobScheduler> logger)
    {
        _scheduler = scheduler;
        _logger = logger;
    }

    public async Task<string> ScheduleEmailAsync(List<string> to, string subject, IEmailModel emailModel,
        List<string>? ccRecipients = null, DateTimeOffset? scheduleAt = null)
    {
        var args = new SendEmailJobArgs(to, subject, emailModel, ccRecipients);
        return await ScheduleEmailAsync(args, scheduleAt);
    }

    public async Task<string> ScheduleEmailAsync(SendEmailJobArgs args, DateTimeOffset? scheduleAt = null)
    {
        try
        {
            // Create unique job identity
            var jobKey = new JobKey($"SendEmail_{Guid.NewGuid()}", "EmailJobs");

            // Build job with data
            var job = JobBuilder.Create<SendEmailQuartzJob>()
                .WithIdentity(jobKey)
                .UsingJobData("To", string.Join(",", args.To))
                .UsingJobData("Subject", args.Subject)
                .UsingJobData("EmailModelJson", args.EmailModelJson)
                .UsingJobData("EmailModelType", args.EmailModelType)
                .UsingJobData("CcRecipients", string.Join(",", args.CcRecipients))
                .Build();

            // Create trigger
            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity($"SendEmailTrigger_{Guid.NewGuid()}", "EmailTriggers");

            if (scheduleAt.HasValue)
            {
                triggerBuilder.StartAt(scheduleAt.Value);
            }
            else
            {
                triggerBuilder.StartNow();
            }

            var trigger = triggerBuilder.Build();

            // Schedule the job
            await _scheduler.ScheduleJob(job, trigger);

            _logger.LogInformation("Email job scheduled with key: {JobKey}", jobKey);

            return jobKey.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to schedule email job");
            throw;
        }
    }
}

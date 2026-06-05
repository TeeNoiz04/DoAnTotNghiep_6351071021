using QuoteFlow.Emailing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuoteFlow.BackgroundJobs.Emailing;

public interface IEmailJobScheduler
{
    Task<string> ScheduleEmailAsync(List<string> to, string subject, IEmailModel emailModel,
        List<string>? ccRecipients = null, DateTimeOffset? scheduleAt = null);
    Task<string> ScheduleEmailAsync(SendEmailJobArgs args, DateTimeOffset? scheduleAt = null);
}

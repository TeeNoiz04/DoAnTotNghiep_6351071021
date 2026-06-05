using QuoteFlow.Emailing;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.NamingPolicies;
using QuoteFlow.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace QuoteFlow.BackgroundJobs.Emailing;

public class SendEmailJobArgs
{
    public List<string> To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string EmailModelJson { get; set; } = null!;
    public string EmailModelType { get; set; } = null!;
    public List<string> CcRecipients { get; set; } = [];

    // Parameterless constructor required for deserialization
    public SendEmailJobArgs() { }

    public SendEmailJobArgs(List<string> to, string subject, IEmailModel emailModel, List<string>? ccRecipients = null)
    {
        To = to
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(x => x.Trim().ToLower())
        .Where(x => x.IsValidEmail())
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();

        Subject = subject;
        CcRecipients = ccRecipients ?? [];
        try
        {
            EmailModelType = emailModel.GetType().FullName!;
            EmailModelJson = JsonSerializer.Serialize(
                emailModel,
                ModelTypeResolver.ResolveModelType(EmailModelType),
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new PascalCaseJsonNamingPolicy(),
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (Exception ex)
        {
            EmailModelType = string.Empty;
            EmailModelJson = string.Empty;

            Console.WriteLine("Failed to serialize email model of type {0}, error: {1}", EmailModelType, ex.Message);
        }
    }
}
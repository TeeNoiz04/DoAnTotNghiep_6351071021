using QuoteFlow.Emailing;
using QuoteFlow.Emailing.EmailModels;
using QuoteFlow.Shared.NamingPolicies;
using QuoteFlow.Shared.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace MEVN.eRequest.BackgroundJobs.Emailing;

public class SendRequestApprovalEmailJob :
    AsyncBackgroundJob<SendRequestApprovalEmailJobArgs>,
    ITransientDependency
{
    private readonly IEmailService _emailService;

    public SendRequestApprovalEmailJob(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public override async Task ExecuteAsync(SendRequestApprovalEmailJobArgs args)
    {
        Logger.LogInformation($"[SendJobArgs] Sending email to {args.To} with subject: {args.Subject}", args.To, args.Subject);

        var modelType = ModelTypeResolver.ResolveModelType(args.EmailModelType);
        var emailModel = DeserializeEmailModel(args.EmailModelJson, modelType);

        if (emailModel is IHasApprovalHistoryEmailModel historyEmailModel)
        {
            historyEmailModel.ApprovalHistories = historyEmailModel.ApprovalHistories
                .OrderByDescending(x => DateTime.ParseExact(x.ActionDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture))
                .ToList();
        }

        await _emailService.SendAsync(args.To, args.Subject, emailModel, cc: args.CcRecipients);
    }

    #region Private Helpers
    private IEmailModel DeserializeEmailModel(string emailModelJson, Type modelType)
    {
        var result = JsonSerializer.Deserialize(
            emailModelJson,
            modelType,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = new PascalCaseJsonNamingPolicy(),
                PropertyNameCaseInsensitive = true
            }) as IEmailModel;

        if (result == null)
        {
            Logger.LogError("Failed to deserialize email model of type {modelType}, emailModelJson: {emailModelJson}", modelType.FullName, emailModelJson);
        }

        return result ?? new NullEmailModel();
    }

    #endregion
}

public class SendRequestApprovalEmailJobArgs
{
    public List<string> To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string EmailModelJson { get; set; } = null!;
    public string EmailModelType { get; set; } = null!;
    public List<string> CcRecipients { get; set; } = [];

    // Parameterless constructor required for deserialization
    public SendRequestApprovalEmailJobArgs() { }

    public SendRequestApprovalEmailJobArgs(List<string> to, string subject, IEmailModel emailModel,List<string>? ccRecipients = null)
    {
        To = to;
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
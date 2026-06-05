using QuoteFlow.BackgroundJobs.Emailing;
using QuoteFlow.Emailing;
using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Emailing.EmailModels;
using QuoteFlow.Materials.Events;
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.RequesterContexts;
using QuoteFlow.Shared.Models;
using QuoteFlow.Shared.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace QuoteFlow.Materials.EventHandlers;
public class SendEmailOnMaterialActionedEventHandler : ILocalEventHandler<MaterialActionedEvent>, IScopedDependency
{
    private readonly IEmailJobScheduler _emailJobScheduler;
    private readonly IMaterialApprovalRequestRepository _materialApprovalRequestRepository;
    private readonly IEffectiveUserContext _currentUser;
    private readonly IConfiguration _configuration;
    public SendEmailOnMaterialActionedEventHandler(IEmailJobScheduler emailJobScheduler, IEffectiveUserContext effectiveUserContext, IMaterialApprovalRequestRepository materialApprovalRequestRepository, IConfiguration configuration)
    {
        _emailJobScheduler = emailJobScheduler;
        _currentUser = effectiveUserContext;
        _materialApprovalRequestRepository = materialApprovalRequestRepository;
        _configuration = configuration;
    }

    public async Task HandleEventAsync(MaterialActionedEvent eventData)
    {
        var materialId = eventData.MaterialApprovalRequestId;
        var material = await _materialApprovalRequestRepository.GetWithDetailNoTrackingAsync(materialId);
        var action = eventData.Action;
        var actionerUsername = eventData.ActionerUsername;
        var actionAt = eventData.SubmittedDate;
        var approvalRoute = GenerateApprovalUrl(material, material.Status!);
        var comment = eventData.Comment;
        var cc = new List<string> { material.CreatorUsername!.ToLower() }
        .Distinct()
        .ToList();

        var emailData = new MaterialApprovalEmailModel(
            material.FileName ?? "",
            material.Note ?? "",
            material.Status!,
            material.CreatorName!,
            material.RequestNo,
            material.ImportType,
            material.CurrentApproverRoleName,
            actionAt.ToString(),
            approvalRoute,
            [.. material.MaterialHistories.Select(x => new ApprovalHistoryEmailInfo(
                x.Action,
                x.ActionDate!,
                x.ApproverRoleName!,

                x.ApproverFullName!,
                x.Note
            )).OrderBy(x => x.ActionDate)]
        );

        if (action == HistoryActions.Submitted || action == HistoryActions.Approved)
        {
            var hasNextStep = material.CurrentApprovalRouteInstanceId != null;
            if (!hasNextStep)
            {
                var to = material.CreatorUsername.ToLower();
                cc.RemoveAll(x => to.Contains(x));
                var subject = EmailSubjectHelper.Generate(
                    _currentUser.FullName ?? "N/A",
                    action,
                    NameHelper.ConvertClassNameToEntityName(nameof(Material)),
                    EmailRecipientRole.Sender,
                    material.RequestNo
                );

                await _emailJobScheduler.ScheduleEmailAsync(new(
                    [to],
                    subject,
                    emailData
                ));
            }
            else
            {


                var to = material.MaterialRoutes
                .Where(x =>
                    x.StepSequence == material.CurrentApprovalStepSequence
                    && !x.IsApproved
                    && x.InstanceId == material.CurrentApprovalRouteInstanceId
                )
                .Select(x => x.Approver.ToLower() ?? "")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

                cc.RemoveAll(x => to.Contains(x));

                var subject = EmailSubjectHelper.Generate(
                    _currentUser.FullName ?? "N/A",
                    action,
                    NameHelper.ConvertClassNameToEntityName(nameof(Material)),
                    EmailRecipientRole.Approver,
                    material.RequestNo
                );

                await _emailJobScheduler.ScheduleEmailAsync(new(
                    to,
                    subject,
                    emailData,
                    cc
                ));
            }

        }

        else if (action == HistoryActions.Rejected)
        {
            var to = material.CreatorUsername.ToLower();

            cc.RemoveAll(x => to.Contains(x));

            var subject = EmailSubjectHelper.Generate(
                actionerUsername,
                action,
                NameHelper.ConvertClassNameToEntityName(nameof(Material)),
                EmailRecipientRole.Sender,
                material.RequestNo
            );
            var emailArgs = new SendEmailJobArgs(
               [to],
               subject,
               emailData
           );
            await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
        }
        else
        {

        }



    }

    private string GenerateApprovalUrl(MaterialApprovalRequest request, string status)
    {
        var angularUrl = _configuration["App:AngularUrl"];
        string view = "";

        if (status == QuoteFlowStatuses.Rejected || request.Status == QuoteFlowStatuses.Approved)
        {
            view = "import-details";
        }
        else
        {
            view = "import-details/my-approvals";
        }

        return $"{angularUrl}/materials/{request.Id}/{view}";
    }
}

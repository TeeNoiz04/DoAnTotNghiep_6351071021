using QuoteFlow.ApprovalRoutes;
using QuoteFlow.BackgroundJobs.Emailing;
using QuoteFlow.DPOs.Events;
using QuoteFlow.Emailing;
using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Emailing.EmailModels;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared.Models;
using QuoteFlow.Shared.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace QuoteFlow.DPOs.EventHandlers;

public class SendEmailOnDPOActionedEventHandler : ILocalEventHandler<DPOActionedEvent>, IScopedDependency
{
    protected readonly IEmailJobScheduler _emailJobScheduler;
    protected readonly ISalesAssignmentRepository _salesAssignmentRepository;
    protected readonly IEffectiveUserContext _currentUser;
    protected readonly IDPORepository _dPORepository;
    protected readonly IApprovalRouteRepository _approvalRouteRepository;
    protected readonly ApprovalRouteManager _approvalRouteManager;
    private readonly IConfiguration _configuration;
    public SendEmailOnDPOActionedEventHandler(IEmailJobScheduler emailJobScheduler, ISalesAssignmentRepository salesAssignmentRepository, IEffectiveUserContext currentUser, IDPORepository dPORepository, IApprovalRouteRepository approvalRouteRepository, ApprovalRouteManager approvalRouteManager, IConfiguration configuration)
    {
        _emailJobScheduler = emailJobScheduler;

        _salesAssignmentRepository = salesAssignmentRepository;
        _currentUser = currentUser;
        _dPORepository = dPORepository;
        _approvalRouteRepository = approvalRouteRepository;
        _approvalRouteManager = approvalRouteManager;
        _configuration = configuration;
    }

    public async Task HandleEventAsync(DPOActionedEvent eventData)
    {

        var dpo = await _dPORepository.GetWithDetailsNoTrackingAsync(eventData.DPOId);
        var saleAssignments = await _salesAssignmentRepository.GetListAsync(
           new() { BuyerId = dpo.BuyerId }
       );
        var cc = new List<string>();

        cc.Add(dpo.CreatorUsername!.ToLower());

        var actionerUsername = eventData.ActionerUsername;
        var actionedAt = eventData.ActionedAt;
        var comment = eventData.Comment;
        var action = eventData.Action;

        var emailData = new DPOApprovalEmailModel(

           new DPOEmailInfo(
                dpo.Status ?? "",
                dpo.CreatorName!,
                dpo.DPONo!,
                dpo.MaterialType!,
                dpo.BuyerShortName!,
                dpo.Remark ?? "",
                dpo.OrderDate ?? DateTime.Now,
                dpo.TotalAmount
           ),
           action

        );

        if (eventData.Action == HistoryActions.Rejected)
        {
            var to = dpo.CreatorUsername.ToLower();
            cc.RemoveAll(x => to.Contains(x));
            var subject = EmailSubjectHelper.Generate(
                actionerUsername,
                action,
                NameHelper.ConvertClassNameToEntityName(nameof(DPO)),
                EmailRecipientRole.Sender,
                dpo.DPONo
            );
            var emailArgs = new SendEmailJobArgs(
                [to],
                subject,
                emailData,
                cc
            );
            await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
        }
        else if (eventData.Action == HistoryActions.DPO.Confirmed)
        {
            var to = dpo.CreatorUsername.ToLower();
            cc.RemoveAll(x => to.Contains(x));
            var subject = EmailSubjectHelper.Generate(
                actionerUsername,
                action,
                NameHelper.ConvertClassNameToEntityName(nameof(DPO)),
                EmailRecipientRole.Sender,
                dpo.DPONo
            );
            var emailArgs = new SendEmailJobArgs(
                [to],
                subject,
                emailData,
                cc
            );
            await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
        }
        else if (action == HistoryActions.Approved)
        {
            // this is GKR Approval, just lazy to split event
            var approvalRoutes = dpo.ApprovalRoutes;
            bool isLastRoute = approvalRoutes is null || approvalRoutes.Count == 0;
            var approvalUrl = GenerateApprovalUrl(dpo);
            var gkrEmailData = new GKRApprovalEmailModel(
                new GKREmailInfo(
                    dpo.Status ?? "",
                    dpo.CreatorName!,
                    dpo.DPONo!,
                    dpo.MaterialType!,
                    dpo.BuyerShortName!,
                    dpo.Remark ?? "",
                    dpo.ExpirationDate ?? DateTime.Now,
                    dpo.TotalAmount
                ),
                action,
                isLastRoute: isLastRoute,
                approvalRoute: approvalUrl
            );
            string subject = "";
            SendEmailJobArgs emailArgs;
            HashSet<string> toList = [];

            if (isLastRoute)
            {
                // all approved, send to creator
                toList.Add(dpo.CreatorUsername.ToLower());
                cc.RemoveAll(toList.Contains);
                subject = EmailSubjectHelper.Generate(
                    actionerUsername,
                    action,
                    "GKR",
                    EmailRecipientRole.Sender,
                    dpo.DPONo
                );
            }
            else
            {
                var currentRoutes = _approvalRouteManager.GetLatestUnapprovedSteps(approvalRoutes!, dpo.Id);

                // send to next approver(s)
                foreach (var route in approvalRoutes!)
                {
                    if (currentRoutes.Exists(x => x.StepSequence == route.StepSequence))
                    {
                        if (!string.IsNullOrWhiteSpace(route.Approver))
                        {
                            toList.Add(route.Approver.ToLower());
                        }
                    }
                }
                cc.RemoveAll(x => toList.Contains(x));
                subject = EmailSubjectHelper.Generate(
                    actionerUsername,
                    action,
                    "GKR",
                    EmailRecipientRole.Approver,
                    dpo.DPONo
                );
            }


            emailArgs = new SendEmailJobArgs(
                [.. toList],
                subject,
                gkrEmailData,
                cc
            );
            await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
        }
    }

    private string GenerateApprovalUrl(DPO gkr)
    {
        var angularUrl = _configuration["App:AngularUrl"];
        return $"{angularUrl}/gkr/{gkr.Id}";
    }
}

using QuoteFlow.BackgroundJobs.Emailing;
using QuoteFlow.Emailing;
using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Emailing.EmailModels;
using QuoteFlow.PriceOffers.Events;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared.Models;
using QuoteFlow.Shared.Utils;
using QuoteFlow.SpecialInputPrices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;

namespace QuoteFlow.PriceOffers.EventHandlers;

public class SendEmailOnPriceOfferActionedEventHandler : ILocalEventHandler<PriceOfferActionedEvent>, IScopedDependency
{
    protected readonly IEmailJobScheduler _emailJobScheduler;
    protected readonly IPriceOfferRepository _priceOfferRepository;
    protected readonly ISalesAssignmentRepository _salesAssignmentRepository;
    protected readonly IEffectiveUserContext _currentUser;
    protected readonly ISpecialInputPriceRepository _specialInputPriceRepository;
    private readonly IConfiguration _configuration;
    public SendEmailOnPriceOfferActionedEventHandler(IEmailJobScheduler emailJobScheduler, IPriceOfferRepository priceOfferRepository, ISalesAssignmentRepository salesAssignmentRepository, IEffectiveUserContext currentUser, ISpecialInputPriceRepository specialInputPriceRepository, IConfiguration configuration)
    {
        _emailJobScheduler = emailJobScheduler;
        _priceOfferRepository = priceOfferRepository;
        _salesAssignmentRepository = salesAssignmentRepository;
        _currentUser = currentUser;
        _specialInputPriceRepository = specialInputPriceRepository;
        _configuration = configuration;
    }

    public async Task HandleEventAsync(PriceOfferActionedEvent eventData)
    {
        // Email send rule:
        // If the action is "Approved":
        // 1. This is the final action in the approval route => Send email to the creator of the price offer
        // 2. There's at least one more step in the approval route => Send email to next approver

        // If the action is "Rejected" => Send email to the creator of the price offer


        var priceOffer = await _priceOfferRepository.GetWithDetailsNoTrackingAsync(eventData.PriceOfferId);

        // Note: For NB (No Buyer) price offers, BuyerId is null, so we skip fetching sales assignments
        var saleAssignments = priceOffer.BuyerId.HasValue
            ? await _salesAssignmentRepository.GetListAsync(new() { BuyerId = priceOffer.BuyerId })
            : new System.Collections.Generic.List<SalesAssignments.SalesAssignment>();
        var cc = new List<string>()
        {
            priceOffer.LastApprovalRouteCreatorUsername?.Trim().ToLower() ?? "",
        };

        var action = eventData.Action;
        var actionerUsername = eventData.ActionerUsername;
        var actionedAt = eventData.ActionedAt;
        var comment = eventData.Comment;
        var approvalUrl = GenerateApprovalUrl(priceOffer);
        var submittedDate = priceOffer.SubmittedDate;

        SpecialInputPrice? specialInputPrice = null;
        if (priceOffer.SpecialInputPriceId != null)
        {
            specialInputPrice = await _specialInputPriceRepository.FirstOrDefaultAsync(x => x.Id == priceOffer.SpecialInputPriceId);
        }


        var emailData = new PriceOfferApprovalEmailModel(
            action,
            new(priceOffer.CurrentApproverRoleCode ?? "", priceOffer.ProjectResultStatus ?? "", priceOffer.ApprovalStatus!, priceOffer.CreatorName!, priceOffer.PriceOfferCode, priceOffer.ProjectName!, priceOffer.BuyerCode!, priceOffer.MaterialType, priceOffer.TotalStandardAmount, priceOffer.TotalMEVNOfferAmount ?? 0),
            submittedDate!.Value,
            [.. priceOffer.ApprovalHistories.Select(x => new ApprovalHistoryEmailInfo(
                x.Action,
                x.ActionDate,
                x.ApproverRoleName!,

                x.ApproverFullName!,
                x.Note
            )).OrderBy(x => x.ActionDate)],
            specialInputPrice != null ? specialInputPrice.AccountNo : "",
            specialInputPrice != null ? specialInputPrice.AccountName : "",
            specialInputPrice != null ? specialInputPrice.ProjectName ?? "" : "",
            specialInputPrice != null ? specialInputPrice.ValidFrom ?? DateTime.Now : DateTime.Now,
            specialInputPrice != null ? specialInputPrice.ValidTo ?? DateTime.Now : DateTime.Now,
            specialInputPrice != null ? specialInputPrice.Id.ToString() : "",
            approvalUrl,
            priceOffer.ApprovalHistories.Any(x => x.Action == HistoryActions.PriceOffer.SubmittedMoreItems) ? "SubmittedMoreItems" : ""
        );

        if (action == HistoryActions.Approved
            || (action == HistoryActions.Cancelled && priceOffer.IsPendingForSales()) // cancelled but still pending for sales approval
        )
        {
            var hasNextStep = priceOffer.CurrentApprovalRouteInstanceId != null;
            if (hasNextStep)
            {
                var nextApprovers = priceOffer.ApprovalRoutes
                    .Where(x =>
                        x.InstanceId == priceOffer.CurrentApprovalRouteInstanceId
                        && x.StepSequence == priceOffer.CurrentApprovalStepSequence
                    )
                    .Select(x => x.Approver?.Trim().ToLower());
                if (nextApprovers != null && nextApprovers.Any())
                {
                    var to = nextApprovers;
                    cc = ListHelper.RemoveDuplicates(cc, [.. to]);

                    var subject = EmailSubjectHelper.Generate(
                        actionerUsername,
                        priceOffer.ProjectResultStatus == QuoteFlowStatuses.PriceOffer.Pending ? HistoryActions.PriceOffer.Confirm : action,
                        NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)),
                        EmailRecipientRole.Approver,
                        priceOffer.PriceOfferCode
                    );
                    var emailArgs = new SendEmailJobArgs(
                        [.. to],
                        subject,
                        emailData,
                        cc
                    );
                    await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
                }
            }
            else
            {
                var to = priceOffer.LastApprovalRouteCreatorUsername?.Trim().ToLower();

                var subject = EmailSubjectHelper.Generate(
                    actionerUsername,
                    action,
                    NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)),
                    EmailRecipientRole.Sender,
                    priceOffer.PriceOfferCode
                );
                var emailArgs = new SendEmailJobArgs(
                    [to],
                    subject,
                    emailData
                );
                await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
            }
        }
        else if (action == HistoryActions.Rejected)
        {
            // Send email to the creator of the price offer
            var to = priceOffer.LastApprovalRouteCreatorUsername?.Trim().ToLower();

            var subject = EmailSubjectHelper.Generate(
                actionerUsername,
                action,
                NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)),
                EmailRecipientRole.Sender,
                priceOffer.PriceOfferCode
            );
            var emailArgs = new SendEmailJobArgs(
                [to],
                subject,
                emailData
            );
            await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
        }
        else if (
            action == HistoryActions.PriceOffer.SubmittedAsWin ||
            action == HistoryActions.PriceOffer.SubmittedAsLoss ||
            action == HistoryActions.PriceOffer.SubmittedAsPreOrder
        )
        {
            var to = new List<string>() {
                priceOffer.CreatorUsername?.Trim().ToLower()  ?? "",
                priceOffer.LastApprovalRouteCreatorUsername?.Trim().ToLower() ?? ""
            };
            to = [.. to.Distinct()];

            var subject = EmailSubjectHelper.Generate(
                actionerUsername,
                action,
                NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)),
                EmailRecipientRole.Sender,
                priceOffer.PriceOfferCode
            );
            var emailArgs = new SendEmailJobArgs(
                to,
                subject,
                emailData
            );
            await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
        }
        else if (action == HistoryActions.PriceOffer.SpecialInputPriceAssigned)
        {
            var to = new List<string>() {
                priceOffer.CreatorUsername?.Trim().ToLower()  ?? "",
                priceOffer.LastApprovalRouteCreatorUsername?.Trim().ToLower() ?? ""
            };
            to = [.. to.Distinct()];

            var subject = EmailSubjectHelper.Generate(
                actionerUsername,
                action,
                NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)),
                EmailRecipientRole.Sender,
                priceOffer.PriceOfferCode
            );
            var emailArgs = new SendEmailJobArgs(
                to,
                subject,
                emailData
            );
            await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
        }
        else if (action == HistoryActions.Closed)
        {
            var to = new List<string>() {
                priceOffer.CreatorUsername?.Trim().ToLower()  ?? "",
                priceOffer.LastApprovalRouteCreatorUsername?.Trim().ToLower() ?? ""
            };
            to = [.. to.Distinct()];
            var subject = EmailSubjectHelper.Generate(
                actionerUsername,
                action,
                NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)),
                EmailRecipientRole.Sender,
                priceOffer.PriceOfferCode
            );
            var emailArgs = new SendEmailJobArgs(
                to,
                subject,
                emailData
            );
            await _emailJobScheduler.ScheduleEmailAsync(emailArgs);
        }
    }

    private string GenerateApprovalUrl(PriceOffer request)
    {
        var angularUrl = _configuration["App:AngularUrl"];
        string view = "details";

        return $"{angularUrl}/price-offer/{request.Id}/{view}";
    }
}

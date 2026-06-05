using QuoteFlow.BackgroundJobs.Emailing;
using QuoteFlow.Emailing;
using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Emailing.EmailModels;
using QuoteFlow.PriceOffers.Events;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared.Models;
using QuoteFlow.Shared.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace QuoteFlow.PriceOffers.EventHandlers;

public class SendEmailOnPriceOfferSubmittedEventHandler
    : ILocalEventHandler<PriceOfferSubmittedEvent>, IScopedDependency
{
    private readonly IEmailJobScheduler _emailJobScheduler;
    private readonly IPriceOfferRepository _priceOfferRepository;
    private readonly IEffectiveUserContext _currentUser;
    private readonly IConfiguration _configuration;
    protected readonly ISalesAssignmentRepository _salesAssignmentRepository;

    public SendEmailOnPriceOfferSubmittedEventHandler(IEmailJobScheduler emailJobScheduler, IPriceOfferRepository priceOfferRepository, IEffectiveUserContext effectiveUserContext, IConfiguration configuration, ISalesAssignmentRepository salesAssignmentRepository)
    {
        _emailJobScheduler = emailJobScheduler;
        _priceOfferRepository = priceOfferRepository;
        _currentUser = effectiveUserContext;
        _configuration = configuration;
        _salesAssignmentRepository = salesAssignmentRepository;
    }

    public async Task HandleEventAsync(PriceOfferSubmittedEvent eventData)
    {
        var priceOfferId = eventData.PriceOfferId;
        var priceOffer = await _priceOfferRepository.GetWithDetailsNoTrackingAsync(priceOfferId);
        var action = eventData.Action;

        var to = priceOffer.ApprovalRoutes
            .Where(x =>
                x.ApproverRoleName == priceOffer.CurrentApproverRoleName
                && !x.IsApproved
                && x.InstanceId == priceOffer.CurrentApprovalRouteInstanceId
            ).Select(x => x.Approver.Trim().ToLower() ?? "")
            .ToList();


        // Note: For NB (No Buyer) price offers, BuyerId is null, so we skip fetching sales assignments
        var saleAssignments = priceOffer.BuyerId.HasValue
            ? await _salesAssignmentRepository.GetListAsync(new() { BuyerId = priceOffer.BuyerId })
            : new System.Collections.Generic.List<SalesAssignments.SalesAssignment>();
        var cc = new List<string>
        {
            priceOffer.LastApprovalRouteCreatorUsername?.Trim().ToLower() ?? "",
        };
        cc.RemoveAll(cc => string.IsNullOrWhiteSpace(cc) || to.Contains(cc));

        var subject = EmailSubjectHelper.Generate(
            _currentUser.FullName ?? "N/A",
            HistoryActions.Submitted,
            NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)),
            EmailRecipientRole.Approver,
            priceOffer.PriceOfferCode
        );
        // Schedule the email job
        var emailArgs = new PriceOfferApprovalEmailModel(
            action,
            new(priceOffer.CurrentApproverRoleCode ?? "", priceOffer.ProjectResultStatus ?? "", priceOffer.ApprovalStatus, priceOffer.CreatorName, priceOffer.PriceOfferCode, priceOffer.ProjectName, priceOffer.BuyerCode, priceOffer.MaterialType, priceOffer.TotalStandardAmount, priceOffer.TotalMEVNOfferAmount ?? 0),
            eventData.SubmittedDate,
            priceOffer.ApprovalHistories.Select(x => new ApprovalHistoryEmailInfo(
                x.Action,
                x.ActionDate,
                x.ApproverRoleName!,

                x.ApproverFullName,
                x.Note
            )).ToList(),
            "", "", "", DateTime.Now, DateTime.Now, "", GenerateApprovalUrl(priceOffer), ""
        );

        await _emailJobScheduler.ScheduleEmailAsync(new(
            to,
            subject,
            emailArgs,
            cc
        ));
    }
    private string GenerateApprovalUrl(PriceOffer request)
    {
        var angularUrl = _configuration["App:AngularUrl"];
        string view = "details";

        return $"{angularUrl}/price-offer/{request.Id}/{view}";
    }
}

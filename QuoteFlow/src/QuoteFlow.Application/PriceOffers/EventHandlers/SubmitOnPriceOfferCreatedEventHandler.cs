using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalRoutes;
using QuoteFlow.PriceOffers.Events;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace QuoteFlow.PriceOffers.EventHandlers;

[LocalEventHandlerOrder(1)]
public class SubmitOnPriceOfferCreatedEventHandler
    : ILocalEventHandler<PriceOfferCreatedEvent>,
    ILocalEventHandler<PriceOfferItemsImportedEvent>,
    IScopedDependency
{
    protected IPriceOfferRepository _priceOfferRepository;
    protected ISalesAssignmentRepository _salesAssignmentRepository;
    protected ILocalEventBus _localEventBus;
    protected IGuidGenerator _guidGenerator;
    protected IEffectiveUserContext _currentUser;
    protected ApprovalRouteManager _approvalRouteManager;

    public SubmitOnPriceOfferCreatedEventHandler(IPriceOfferRepository priceOfferRepository, ILocalEventBus localEventBus, IGuidGenerator guidGenerator, IEffectiveUserContext currentUser, ApprovalRouteManager approvalRouteManager, ISalesAssignmentRepository salesAssignmentRepository)
    {
        _priceOfferRepository = priceOfferRepository;
        _localEventBus = localEventBus;
        _guidGenerator = guidGenerator;
        _currentUser = currentUser;
        _approvalRouteManager = approvalRouteManager;
        _salesAssignmentRepository = salesAssignmentRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(PriceOfferCreatedEvent eventData)
    {
        var priceOfferId = eventData.PriceOfferId;
        var forceSubmit = eventData.ForceSubmit;

        PriceOffer priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);
        priceOffer.Submit();
        await HandlePriceOfferSubmissionAsync(priceOfferId, priceOffer, HistoryActions.Submitted, forceSubmit);
    }


    [UnitOfWork]
    public virtual async Task HandleEventAsync(PriceOfferItemsImportedEvent eventData)
    {
        var priceOfferId = eventData.PriceOfferId;
        var forceSubmit = true;
        var comment = eventData.Comment;

        PriceOffer priceOffer = await _priceOfferRepository.GetAsync(priceOfferId);

        // handle the case where more items are imported while price offer waiting for sale pic confirm win/loss
        // 1. Clear the current approval route (Sale PIC)
        var importGuid = eventData.ImportGuid;
        priceOffer.SubmitMoreItems(importGuid);

        if (priceOffer.CurrentApprovalRouteInstanceId.HasValue)
        {
            await _approvalRouteManager.RemoveRoutes(priceOffer.CurrentApprovalRouteInstanceId.Value, EntityTypes.PriceOffer);
            priceOffer.SetCurrentRoute(null);
        }

        await HandlePriceOfferSubmissionAsync(priceOfferId, priceOffer, HistoryActions.PriceOffer.SubmittedMoreItems, forceSubmit, importGuid, comment);
    }

    private async Task HandlePriceOfferSubmissionAsync(
        Guid priceOfferId,
        PriceOffer priceOffer,
        string action,
        bool forceSubmit = true,
        Guid? importGuid = null,
        string? comment = null
    )
    {
        var saleTeam = await _salesAssignmentRepository.GetListAsync(new SalesAssignments.ParameterObjects.SalesAssignmentFilterParams());
        var isRequester = priceOffer.CreatorUsername?.ToLowerInvariant() == _currentUser.Username?.ToLowerInvariant();
        var isSaleTeam = saleTeam.FirstOrDefault(x => x.SaleUserName.ToLowerInvariant() == _currentUser.Username?.ToLowerInvariant()) != null;
        priceOffer = await _priceOfferRepository.UpdateCalculatedFieldsAsync(priceOfferId);
        var hasLevel5Approval = await _priceOfferRepository.HasLevel5ApprovalAsync(priceOfferId, priceOffer.PriceOfferCode);
        if (hasLevel5Approval && !forceSubmit)
        {
            // return an exception to notify the user that this will go thru level 5 approval,
            // and they need to force the submission if they want to proceed.
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ApprovalLevelExceededMessage);
        }

        await _priceOfferRepository.GenerateApprovalRouteAsync(priceOfferId, priceOffer.PriceOfferCode);

        var approvalRoutes = await _priceOfferRepository.GetListApprovalRoutesAsync(priceOfferId);
        if (approvalRoutes.Count == 0)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.NoApprovalRouteFound)
               .WithData("entityId", priceOfferId);
        }
        RecordLatestPersonCreateRoute(priceOffer);

        var latestStep = _approvalRouteManager.GetLatestUnapprovedSteps(approvalRoutes, priceOfferId).First();

        var approverRoleCode = latestStep.ApproverRoleCode;
        var approverRoleName = latestStep.ApproverRoleName;
        var submittedDate = DateTime.Now;

        priceOffer.SetCurrentRoute(new(
            latestStep.InstanceId,
            latestStep.StepSequence,
            approverRoleCode,
            approverRoleName
        ));
        // record history
        if (importGuid is not null)
        {
            priceOffer.RecordAction(new PriceOfferApprovalHistory(
            _guidGenerator.Create(),
            importGuid: importGuid,
            priceOfferId,
            new(
                isRequester ? "Requester" : isSaleTeam ? "SaleTeam" : "FA Team",
                isRequester ? "Requester" : isSaleTeam ? "SaleTeam" : "FA Team",
                _currentUser.Username,
                _currentUser.FullName,
                action,
                submittedDate,
                note: comment
            )
            ));
        }
        else
        {
            priceOffer.RecordAction(new PriceOfferApprovalHistory(
            _guidGenerator.Create(),
            priceOfferId,
            new(
                "Requester",
                "Requester",
                _currentUser.Username,
                _currentUser.FullName,
                action,
                submittedDate,
                note: comment
            )
            ));
        }
        // record child histories
        foreach (var detail in priceOffer.Details)
        {
            if (detail.IsInProgress()) // Only record history for details that are just submitted
            {
                if (importGuid is not null)
                {
                    detail.RecordAction(new PriceOfferDetailApprovalHistory(
                        _guidGenerator.Create(),
                        importGuid: importGuid,
                        detail.Id,
                        new(
                            isRequester ? "Requester" : isSaleTeam ? "SaleTeam" : "FA Team",
                            isRequester ? "Requester" : isSaleTeam ? "SaleTeam" : "FA Team",
                            _currentUser.Username,
                            _currentUser.FullName,
                            action,
                            submittedDate,
                            note: comment
                        )
                    ));
                }
                else
                {
                    detail.RecordAction(new PriceOfferDetailApprovalHistory(
                        _guidGenerator.Create(),
                        detail.Id,
                        new(
                            "Requester",
                            "Requester",
                            _currentUser.Username,
                            _currentUser.FullName,
                            action,
                            submittedDate,
                            note: comment
                        )
                    ));
                }
            }
            //else if (importGuid is not null)
            //{
            //    detail.RecordAction(new PriceOfferDetailApprovalHistory(
            //            _guidGenerator.Create(),
            //            importGuid: importGuid,
            //            detail.Id,
            //            new(
            //                "User",
            //                "User",
            //                _currentUser.Username,
            //                _currentUser.FullName,
            //                action,
            //                submittedDate,
            //                null
            //            )
            //        ));
            //}
        }

        await _priceOfferRepository.UpdateAsync(priceOffer, autoSave: true);
        await _localEventBus.PublishAsync(new PriceOfferSubmittedEvent(priceOfferId, submittedDate, action), onUnitOfWorkComplete: false);
    }

    protected virtual void RecordLatestPersonCreateRoute(PriceOffer priceOffer)
    {
        priceOffer.LastApprovalRouteCreatorId = _currentUser.Id;
        priceOffer.LastApprovalRouteCreatorUsername = _currentUser.Username;
        priceOffer.LastApprovalRouteCreatorName = _currentUser.FullName;
        priceOffer.LastApprovalRouteCreationTime = DateTime.Now;
    }
}

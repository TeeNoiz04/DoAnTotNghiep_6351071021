using QuoteFlow.Permissions;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared.Flagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace QuoteFlow.PriceOffers;

public class PriceOfferFlaggingService : BaseFlaggingService<PriceOffer, PriceOfferFlagsDto, PriceOfferFlaggingContext>
{
    protected readonly ISalesAssignmentRepository _salesAssignmentRepository;

    public PriceOfferFlaggingService(ISalesAssignmentRepository salesAssignmentRepository)
    {
        _salesAssignmentRepository = salesAssignmentRepository;
    }

    protected override async Task<Dictionary<Guid, PriceOfferFlagsDto>> CreateBulkFlagsAsync(IEnumerable<PriceOffer> entities, PriceOfferFlaggingContext context)
    {
        var flags = new Dictionary<Guid, PriceOfferFlagsDto>();
        var currentUserName = context.CurrentUsername;
        var saleAssignments = await _salesAssignmentRepository.GetListAsync(
            new() { SaleUserName = currentUserName }
        );

        // Check permissions once
        var hasViewStrategicPricePermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.Materials.ViewStrategicPrice);
        var hasApplySpecialInputPricePermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.PriceOffers.ApplySpecialInputPrice);
        var hasChangeItemPropertiesPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.PriceOffers.Uploads.ChangeItemProperties);
        var hasAddMoreItemsPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.PriceOffers.Uploads.AddMoreItems);
        var hasCancelPriceOfferPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.PriceOffers.Cancel);
        var hasClosePriceOfferPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.PriceOffers.Close);
        var hasConfirmProjectResultPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.PriceOffers.ConfirmProjectResult);
        var hasImportNoBuyerPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.PriceOffers.Uploads.PriceOfferNB);
        foreach (var offer in entities)
        {
            var offerType = offer.GetPriceOfferType();
            bool isCurrentApprover = offer.ApprovalRoutes.Any(x => x.Approver != null && x.Approver.Equals(currentUserName, StringComparison.OrdinalIgnoreCase) && x.StepSequence == offer.CurrentApprovalStepSequence);
            bool isCreator = currentUserName.Equals(offer.CreatorUsername, StringComparison.OrdinalIgnoreCase);
            bool isInProgress = offer.IsInProgress();
            bool isApproved = offer.IsApproved();
            bool isInSameSaleTeam = saleAssignments.Any(x =>
                x.BuyerId == offer.BuyerId &&
                x.BuyerTypeId == offer.BuyerTypeId &&
                x.LocationId == offer.LocationId &&
                x.MaterialType == offer.MaterialType
            ) || isCreator;
            bool isNewestAddMorePerson = offer.LastApprovalRouteCreatorUsername != null && currentUserName.Equals(offer.LastApprovalRouteCreatorUsername, StringComparison.OrdinalIgnoreCase);
            bool isPendingProjectResult = offer.IsPendingForSales();
            bool isPreOrderProjectResult = offer.IsPreOrder();
            bool notReachedLevel3 = offer.CurrentApprovalStepSequence is not null && offer.CurrentApprovalStepSequence < 3;
            bool isWon = offerType != PriceOfferTypes.PriceOfferPP || offer.IsWon();
            bool isLost = offerType == PriceOfferTypes.PriceOfferPP && offer.IsLost();
            bool isAtLevel1 = offer.CurrentApprovalStepSequence is not null && offer.CurrentApprovalStepSequence == 1;

            // Special input price and change item properties: Check permission and status
            bool canApplySpecialInputPrice = hasApplySpecialInputPricePermission && notReachedLevel3;
            bool canChangeItemProperties = hasChangeItemPropertiesPermission && notReachedLevel3;
            bool baseCanAddMoreItem = isInSameSaleTeam && isApproved && !isLost && hasAddMoreItemsPermission;
            bool canAddMoreItem = baseCanAddMoreItem && (!offer.IsBuyerStockPriceOffer() || !offer.HasDPOUsed);
            bool canAddMoreItemNB = hasAddMoreItemsPermission && isApproved && hasImportNoBuyerPermission && offer.IsNoBuyerPriceOffer();

            flags[offer.Id] = new PriceOfferFlagsDto
            {
                IsEditable = false,
                IsViewable = true,
                IsRemovable = false,
                IsGPViewable = hasViewStrategicPricePermission,
                IsLandedCostViewable = hasViewStrategicPricePermission,
                IsSpecialInputPriceViewable = hasApplySpecialInputPricePermission,
                IsApprovable = isCurrentApprover && isInProgress,
                IsRejectable = isCurrentApprover && isInProgress,
                IsCancellable = (isCreator || hasCancelPriceOfferPermission) && isInProgress,
                IsClosable = (isCreator || isNewestAddMorePerson || hasClosePriceOfferPermission) && isApproved,
                CanCancelItem = offerType == PriceOfferTypes.PriceOfferAP && isApproved,
                IsProjectResultSubmittable = (isInSameSaleTeam || hasConfirmProjectResultPermission) && isPendingProjectResult && isApproved,
                IsPreOrderResultConfirmable = (isInSameSaleTeam || hasConfirmProjectResultPermission) && isPreOrderProjectResult,
                IsDetailPropertiesChangeable = canChangeItemProperties,
                CanAddMoreItems = canAddMoreItem || canAddMoreItemNB,
                IsSpecialInputPriceApplicable = canApplySpecialInputPrice,
                IsDetailsPropertiesTemplateDownloadable = hasChangeItemPropertiesPermission
            };
        }

        return flags;
    }

    protected override PriceOfferFlaggingContext CreateFlaggingContext()
    {
        var currentUsername = CurrentUser.Username
            ?? throw new UserFriendlyException("Current user is not authenticated.");

        return new PriceOfferFlaggingContext(currentUsername);
    }
}

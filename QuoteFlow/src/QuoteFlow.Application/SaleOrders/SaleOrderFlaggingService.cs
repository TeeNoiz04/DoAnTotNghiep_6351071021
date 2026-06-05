using QuoteFlow.GICs;
using QuoteFlow.Permissions;
using QuoteFlow.Shared.Flagging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace QuoteFlow.SaleOrders;

public class SaleOrderFlaggingService : BaseFlaggingService<SaleOrder, SaleOrderFlagsDto, SaleOrderFlaggingContext>
{
    protected override async Task<Dictionary<Guid, SaleOrderFlagsDto>> CreateBulkFlagsAsync(IEnumerable<SaleOrder> entities, SaleOrderFlaggingContext context)
    {
        var flags = new Dictionary<Guid, SaleOrderFlagsDto>();

        // Check permissions once
        var hasConfirmDeliveryPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.SaleOrders.ConfirmDelivery);
        var hasEditPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.SaleOrders.Edit);
        var hasEditSAPInfoPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.SaleOrders.EditSAPInfo);
        var hasReopenPermission = await IsCurrentUserAuthorizedAsync(QuoteFlowPermissions.SaleOrders.Reopen);
        foreach (var saleOrder in entities)
        {
            bool isInProgress = saleOrder.IsInProgress;
            bool canConfirmDelivery = hasConfirmDeliveryPermission && isInProgress;
            bool canReopenSO = hasReopenPermission && saleOrder.IsClosed && saleOrder.GICType != GICTypeCodes.WriteOff;
            bool canEditSAPInfo = (hasEditSAPInfoPermission && saleOrder.IsClosed) || (saleOrder.IsDraft || saleOrder.IsInProgress);


            flags[saleOrder.Id] = new SaleOrderFlagsDto
            {
                IsEditable = hasEditPermission && saleOrder.IsDraft,
                IsViewable = true,
                IsRemovable = false,
                CanConfirmDelivery = canConfirmDelivery,
                CanReOpenSO = canReopenSO,
                CanEditSAPInfo = canEditSAPInfo
            };
        }

        return flags;
    }

    protected override SaleOrderFlaggingContext CreateFlaggingContext()
    {
        var currentUsername = CurrentUser.Username
            ?? throw new UserFriendlyException("Current user is not authenticated.");

        return new SaleOrderFlaggingContext(currentUsername);
    }
}
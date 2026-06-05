using QuoteFlow.Permissions;
using QuoteFlow.Shared.Flagging;
using QuoteFlow.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteFlow.DPOs;

public class DPOFlaggingService : BaseFlaggingService<DPO, DPOFlagsDto, DPOFlaggingContext>
{
    protected override async Task<Dictionary<Guid, DPOFlagsDto>> CreateBulkFlagsAsync(
        IEnumerable<DPO> entities,
        DPOFlaggingContext context)
    {
        var flags = new Dictionary<Guid, DPOFlagsDto>();
        var hasLockStockPermission = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.MovingOrders.DPOs.LockStock);
        var hasLockOnOrderPermission = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.MovingOrders.DPOs.LockOnOrderStock);
        var hasApprovePermission = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.MovingOrders.DPOs.ConfirmReject);

        var currentUserId = CurrentUser.Id!.Value;
        var currentRoles = await IdentityUserRepository.GetRolesAsync(currentUserId);

        var isAdmin = currentRoles.Select(x => x.Name).Any(n => n.Contains("SalesAdmin", StringComparison.OrdinalIgnoreCase) || n.Contains("admin", StringComparison.OrdinalIgnoreCase) || n.Contains("Admin_FA", StringComparison.OrdinalIgnoreCase));

        foreach (var dpo in entities)
        {
            var isSubmitted = dpo.Status == QuoteFlowStatuses.DPO.Submitted;

            var flag = new DPOFlagsDto
            {
                IsViewable = true, // All users can view DPO
                IsEditable = false,
                IsRemovable = false,
                CanLockStock = hasLockStockPermission && (dpo.IsConfirmed || dpo.IsInProgress),
                CanLockOnOrder = hasLockOnOrderPermission && (dpo.IsLockedStock || dpo.IsInProgress),
                CanConfirmLockStock = hasLockStockPermission && dpo.IsConfirmed,
                CanConfirmLockOnOrder = hasLockOnOrderPermission && dpo.IsLockedStock,
                CanApprove = hasApprovePermission && isSubmitted,
                CanReject = hasApprovePermission && isSubmitted,
                CanEditItem = isAdmin,
                CanDelete = !(dpo.Status == QuoteFlowStatuses.Closed),
                CanAllocateGKR = (hasLockStockPermission || hasLockOnOrderPermission) && (dpo.IsConfirmed),
            };

            flags[dpo.Id] = flag;
        }

        return flags;
    }
    protected override DPOFlaggingContext CreateFlaggingContext()
    {
        return new DPOFlaggingContext();
    }
}
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.Shared.Flagging;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace QuoteFlow.Materials.Flagging.MaterialApprovalRequests;

public class MaterialApprovalRequestFlaggingService : BaseFlaggingService<MaterialApprovalRequest, MaterialFlagsDto, MaterialApprovalRequestFlaggingContext>
{
    public MaterialApprovalRequestFlaggingService()
    { }

    protected override async Task<Dictionary<Guid, MaterialFlagsDto>> CreateBulkFlagsAsync(IEnumerable<MaterialApprovalRequest> entities, MaterialApprovalRequestFlaggingContext context)
    {
        var flags = new Dictionary<Guid, MaterialFlagsDto>();
        var currentUserName = context.CurrentUsername;
        var currentUserRoles = context.CurrentUserRoles;


        foreach (var material in entities)
        {
            //var importType = material.ImportType;
            //bool isNewMaterial = importType.Equals(MaterialImportType.M2U);
            bool isCurrentApprover = material.MaterialRoutes is not null ? material.MaterialRoutes.Any(x => x.Approver != null && x.Approver.Equals(currentUserName, StringComparison.OrdinalIgnoreCase) && x.StepSequence == material.CurrentApprovalStepSequence) : false;
            bool isCreator = currentUserName.Equals(material.CreatorUsername, StringComparison.OrdinalIgnoreCase);
            bool isInProgress = material.Status == QuoteFlowStatuses.InProgress;


            flags[material.Id] = new MaterialFlagsDto
            {
                IsEditable = false,
                IsViewable = true,
                IsRemovable = false,
                IsApprovable = isCurrentApprover && isInProgress,
                IsRejectable = isCurrentApprover && isInProgress,
                IsCancellable = isCreator && isInProgress,

            };
        }

        return flags;
    }

    protected override MaterialApprovalRequestFlaggingContext CreateFlaggingContext()
    {
        var currentUsername = CurrentUser.Username
            ?? throw new UserFriendlyException("Current user is not authenticated.");

        return new MaterialApprovalRequestFlaggingContext(
            currentUsername,
            CurrentUser.Roles
        );
    }
}
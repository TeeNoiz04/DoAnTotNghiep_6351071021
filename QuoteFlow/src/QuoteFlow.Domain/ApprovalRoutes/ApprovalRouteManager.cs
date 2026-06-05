using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.ApprovalRoutes;

public class ApprovalRouteManager : DomainService
{
    protected IApprovalRouteRepository _approvalRouteRepository;

    public ApprovalRouteManager(IApprovalRouteRepository approvalRouteRepository)
    {
        _approvalRouteRepository = approvalRouteRepository;
    }

    public virtual async Task<ApprovalRoute> CreateAsync(
    int stepSequence, string approverRoleCode, string approverRoleName, bool isApproved, string? entityType = null, Guid? instanceId = null, string? approver = null, DateTime? approvalDate = null, string? notes = null)
    {
        Check.NotNullOrWhiteSpace(approverRoleCode, nameof(approverRoleCode));
        Check.Length(approverRoleCode, nameof(approverRoleCode), ApprovalRouteConsts.ApproverRoleCodeMaxLength);
        Check.NotNullOrWhiteSpace(approverRoleName, nameof(approverRoleName));
        Check.Length(approverRoleName, nameof(approverRoleName), ApprovalRouteConsts.ApproverRoleNameMaxLength);
        Check.Length(entityType, nameof(entityType), ApprovalRouteConsts.EntityTypeMaxLength);
        Check.Length(approver, nameof(approver), ApprovalRouteConsts.ApproverMaxLength);
        Check.Length(notes, nameof(notes), ApprovalRouteConsts.NotesMaxLength);

        var approvalRoute = new ApprovalRoute(
         GuidGenerator.Create(),
         stepSequence, approverRoleCode, approverRoleName, isApproved, entityType, instanceId, approver, approvalDate, notes
         );

        return await _approvalRouteRepository.InsertAsync(approvalRoute);
    }

    public virtual async Task<ApprovalRoute> UpdateAsync(
        Guid id,
        int stepSequence, string approverRoleCode, string approverRoleName, bool isApproved, Guid? entityId = null, string? entityType = null, Guid? instanceId = null, string? approver = null, DateTime? approvalDate = null, string? notes = null, [CanBeNull] string? concurrencyStamp = null
    )
    {
        Check.NotNullOrWhiteSpace(approverRoleCode, nameof(approverRoleCode));
        Check.Length(approverRoleCode, nameof(approverRoleCode), ApprovalRouteConsts.ApproverRoleCodeMaxLength);
        Check.NotNullOrWhiteSpace(approverRoleName, nameof(approverRoleName));
        Check.Length(approverRoleName, nameof(approverRoleName), ApprovalRouteConsts.ApproverRoleNameMaxLength);
        Check.Length(entityType, nameof(entityType), ApprovalRouteConsts.EntityTypeMaxLength);
        Check.Length(approver, nameof(approver), ApprovalRouteConsts.ApproverMaxLength);
        Check.Length(notes, nameof(notes), ApprovalRouteConsts.NotesMaxLength);

        var approvalRoute = await _approvalRouteRepository.GetAsync(id);

        approvalRoute.StepSequence = stepSequence;
        approvalRoute.ApproverRoleCode = approverRoleCode;
        approvalRoute.ApproverRoleName = approverRoleName;
        approvalRoute.IsApproved = isApproved;
        approvalRoute.EntityType = entityType;
        approvalRoute.InstanceId = instanceId;
        approvalRoute.Approver = approver;
        approvalRoute.ApprovalDate = approvalDate;
        approvalRoute.Notes = notes;

        approvalRoute.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _approvalRouteRepository.UpdateAsync(approvalRoute);
    }

    public bool IsLastApprovalStep<TRoute>(TRoute routeToCheck, IEnumerable<TRoute> routeList)
        where TRoute : ApprovalRoute
    {
        if (!routeList.Any())
        {
            return false;
        }

        var maxStep = routeList.Max(r => r.StepSequence);
        return routeToCheck.StepSequence == maxStep;
    }

    public List<TRoute> GetLatestUnapprovedSteps<TRoute>(IEnumerable<TRoute> routeList, Guid entityId)
    where TRoute : ApprovalRoute
    {
        if (routeList is null || !routeList.Any())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.NoApprovalRouteFound)
                .WithData("entityId", entityId);
        }

        var unapprovedSteps = routeList.Where(x => !x.IsApproved).ToList();

        if (unapprovedSteps.Count == 0)
        {
            return [];
        }

        var maxStepSequence = unapprovedSteps.Min(x => x.StepSequence);

        return [.. unapprovedSteps.Where(x => x.StepSequence == maxStepSequence)];
    }

    // GetNextApprovalStep
    public TRoute? GetNextApprovalStep<TRoute>(IEnumerable<TRoute> routeList, TRoute currentStep)
        where TRoute : ApprovalRoute
    {
        if (routeList is null)
        {
            return null;
        }

        // Avoid multiple enumeration if routeList is not a List/Array
        var filtered = routeList
            .Where(x => x.StepSequence > currentStep.StepSequence && !x.IsApproved);

        if (!filtered.Any())
        {
            return null;
        }


        return filtered.MinBy(x => x.StepSequence);
    }

    public async Task RemoveRoutes(Guid instanceId, string entityType)
    {
        await _approvalRouteRepository.DeleteAsync(x => x.InstanceId == instanceId && x.EntityType == entityType);
    }
}
using QuoteFlow.Materials.Events;
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
using QuoteFlow.RequesterContexts;
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

namespace QuoteFlow.Materials.EventHandlers;
public class SubmitOnMaterialApprovalCreatedEventHandler : ILocalEventHandler<MaterialApprovalCreatedEvent>, IScopedDependency
{
    protected IMaterialApprovalRequestRepository _materialApprovalRequestRepository;
    protected ILocalEventBus _localEventBus;
    protected IGuidGenerator _guidGenerator;
    protected IEffectiveUserContext _currentUser;

    public SubmitOnMaterialApprovalCreatedEventHandler(ILocalEventBus localEventBus, IGuidGenerator guidGenerator, IEffectiveUserContext currentUser, IMaterialApprovalRequestRepository materialApprovalRequestRepository)
    {
        _localEventBus = localEventBus;
        _guidGenerator = guidGenerator;
        _currentUser = currentUser;
        _materialApprovalRequestRepository = materialApprovalRequestRepository;
    }

    public virtual async Task HandleEventAsync(MaterialApprovalCreatedEvent eventData)
    {
        var materialId = eventData.MaterialApprovalRequestId;

        MaterialApprovalRequest material = await _materialApprovalRequestRepository.GetWithDetailAsync(materialId);

        material.Submit();

        var filterRouteParams = new MaterialApprovalWFRouteFilterParams()
        {
            MaterialId = materialId,
            ImportType = material.ImportType

        };

        await _materialApprovalRequestRepository.CreateApprovalRoute(filterRouteParams);

        var approvalRoutes = await _materialApprovalRequestRepository.GetListApprovalRoutesAsync(materialId);
        if (material.ImportType == MaterialImportType.M1U || material.ImportType == MaterialImportType.M2U || material.ImportType == MaterialImportType.M5U)
        {
            if (approvalRoutes.Count <= 1)
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.NoApprovalRouteFound)
                    .WithData("entityId", material.Id);
            }
        }

        var latestStep = approvalRoutes
               .OrderBy(x => x.StepSequence)
               .FirstOrDefault(x => !x.IsApproved &&
                            approvalRoutes
                                .Where(y => y.StepSequence < x.StepSequence)
                                .All(y => y.IsApproved))
               ?? throw new BusinessException(QuoteFlowDomainErrorCodes.NoApprovalRouteFound)
                   .WithData("entityId", material.Id);

        var approverRoleCode = latestStep.ApproverRoleCode;
        var approverRoleName = latestStep.ApproverRoleName;
        var approverUsername = _currentUser.Username;
        var approverFullName = _currentUser.FullName;
        var submittedDate = DateTime.Now;
        var note = eventData.Note;

        material.SetCurrentRoute(new(
            latestStep.InstanceId,
            latestStep.StepSequence,
            approverRoleCode,
            approverRoleName
        ));


        // record history
        material.RecordAction(new MaterialApprovalRequestHistory(
            _guidGenerator.Create(),
            materialId,
            new(
                null,
                null,
                approverUsername,
                approverFullName,
                HistoryActions.Submitted,
                submittedDate,
                note
            )
        ));

        await _materialApprovalRequestRepository.UpdateAsync(material, autoSave: true);

        await _localEventBus.PublishAsync(new MaterialActionedEvent(materialId, HistoryActions.Submitted, _currentUser.Username, DateTime.Now), onUnitOfWorkComplete: false);
    }
}

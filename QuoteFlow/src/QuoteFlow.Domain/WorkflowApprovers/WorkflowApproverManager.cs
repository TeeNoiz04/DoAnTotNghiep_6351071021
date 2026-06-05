using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.WorkflowApprovers;

public class WorkflowApproverManager : DomainService
{
    protected IWorkflowApproverRepository _workflowApproverRepository;

    public WorkflowApproverManager(IWorkflowApproverRepository workflowApproverRepository)
    {
        _workflowApproverRepository = workflowApproverRepository;
    }

    public virtual async Task<WorkflowApprover> CreateAsync(
    Guid wFId, string approver, string? note = null)
    {
        Check.NotNullOrWhiteSpace(approver, nameof(approver));
        Check.Length(approver, nameof(approver), WorkflowApproverConsts.ApproverMaxLength);
        Check.Length(note, nameof(note), WorkflowApproverConsts.NoteMaxLength);

        var workflowApprover = new WorkflowApprover(
         GuidGenerator.Create(),
         wFId, approver, note
         );

        return await _workflowApproverRepository.InsertAsync(workflowApprover);
    }

    public virtual async Task<WorkflowApprover> UpdateAsync(
        Guid id,
        Guid wFId, string approver
    )
    {

        var workflowApprover = await _workflowApproverRepository.GetAsync(id);

        workflowApprover.WFId = wFId;
        workflowApprover.Approver = approver;

        return await _workflowApproverRepository.UpdateAsync(workflowApprover);
    }

}
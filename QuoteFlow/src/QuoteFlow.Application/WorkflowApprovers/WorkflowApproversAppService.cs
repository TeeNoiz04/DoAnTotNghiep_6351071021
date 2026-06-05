using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.WorkflowApprovers;

[RemoteService(IsEnabled = false)]

public class WorkflowApproversAppService : QuoteFlowAppService, IWorkflowApproversAppService
{

    protected IWorkflowApproverRepository _workflowApproverRepository;
    protected WorkflowApproverManager _workflowApproverManager;

    public WorkflowApproversAppService(IWorkflowApproverRepository workflowApproverRepository, WorkflowApproverManager workflowApproverManager)
    {

        _workflowApproverRepository = workflowApproverRepository;
        _workflowApproverManager = workflowApproverManager;

    }

    public virtual async Task<PagedResultDto<WorkflowApproverDto>> GetListAsync(GetWorkflowApproversInput input)
    {
        var totalCount = await _workflowApproverRepository.GetCountAsync(input.FilterText, input.WFId, input.Approver, input.Note);
        var items = await _workflowApproverRepository.GetListAsync(input.FilterText, input.WFId, input.Approver, input.Note, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<WorkflowApproverDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<WorkflowApprover>, List<WorkflowApproverDto>>(items)
        };
    }

    public virtual async Task<WorkflowApproverDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<WorkflowApprover, WorkflowApproverDto>(await _workflowApproverRepository.GetAsync(id));
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        await _workflowApproverRepository.DeleteAsync(id);
    }

    public virtual async Task<WorkflowApproverDto> CreateAsync(WorkflowApproverCreateDto input)
    {

        var workflowApprover = await _workflowApproverManager.CreateAsync(
        input.WFId, input.Approver, input.Note
        );

        return ObjectMapper.Map<WorkflowApprover, WorkflowApproverDto>(workflowApprover);
    }


    public virtual async Task<WorkflowApproverDto> UpdateAsync(Guid id, WorkflowApproverUpdateDto input)
    {

        var workflowApprover = await _workflowApproverManager.UpdateAsync(
        id,
        input.WFId, input.Approver
        );

        return ObjectMapper.Map<WorkflowApprover, WorkflowApproverDto>(workflowApprover);
    }
}
using Asp.Versioning;
using QuoteFlow.WorkflowApprovers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.WorkflowApprovers;

[RemoteService]
[Area("app")]
[ControllerName("WorkflowApprover")]
[Route("api/app/workflow-approvers")]

public class WorkflowApproverController : AbpController, IWorkflowApproversAppService
{
    protected IWorkflowApproversAppService _workflowApproversAppService;

    public WorkflowApproverController(IWorkflowApproversAppService workflowApproversAppService)
    {
        _workflowApproversAppService = workflowApproversAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<WorkflowApproverDto>> GetListAsync(GetWorkflowApproversInput input)
    {
        return _workflowApproversAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<WorkflowApproverDto> GetAsync(Guid id)
    {
        return _workflowApproversAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<WorkflowApproverDto> CreateAsync(WorkflowApproverCreateDto input)
    {
        return _workflowApproversAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<WorkflowApproverDto> UpdateAsync(Guid id, WorkflowApproverUpdateDto input)
    {
        return _workflowApproversAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _workflowApproversAppService.DeleteAsync(id);
    }
}
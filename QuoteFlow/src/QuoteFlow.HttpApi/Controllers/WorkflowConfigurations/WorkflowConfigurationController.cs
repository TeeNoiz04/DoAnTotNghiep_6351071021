using Asp.Versioning;
using QuoteFlow.WorkflowConfigurations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.WorkflowConfigurations;

[RemoteService]
[Area("app")]
[ControllerName("WorkflowConfiguration")]
[Route("api/app/workflow-configurations")]

public class WorkflowConfigurationController : AbpController, IWorkflowConfigurationsAppService
{
    protected IWorkflowConfigurationsAppService _workflowConfigurationsAppService;

    public WorkflowConfigurationController(IWorkflowConfigurationsAppService workflowConfigurationsAppService)
    {
        _workflowConfigurationsAppService = workflowConfigurationsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<WorkflowConfigurationDto>> GetListAsync(GetWorkflowConfigurationsInput input)
    {
        return _workflowConfigurationsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<WorkflowConfigurationDto> GetAsync(Guid id)
    {
        return _workflowConfigurationsAppService.GetAsync(id);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<WorkflowConfigurationDto> UpdateAsync(Guid id, WorkflowConfigurationUpdateDto input)
    {
        return _workflowConfigurationsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _workflowConfigurationsAppService.DeleteAsync(id);
    }
}
using QuoteFlow.Permissions;
using QuoteFlow.WorkflowApprovers;
using QuoteFlow.WorkflowConfigurations.ParameterObject;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.WorkflowConfigurations;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.WorkflowConfigurations.Default)]
public class WorkflowConfigurationsAppService : QuoteFlowAppService, IWorkflowConfigurationsAppService
{

    protected IWorkflowConfigurationRepository _workflowConfigurationRepository;
    protected WorkflowConfigurationManager _workflowConfigurationManager;
    protected IWorkflowApproverRepository _workflowApproverRepository;
    protected WorkflowApproverManager _workflowApproverManager;

    public WorkflowConfigurationsAppService(IWorkflowConfigurationRepository workflowConfigurationRepository, WorkflowConfigurationManager workflowConfigurationManager, IWorkflowApproverRepository workflowApproverRepository, WorkflowApproverManager workflowApproverManager)
    {

        _workflowConfigurationRepository = workflowConfigurationRepository;
        _workflowConfigurationManager = workflowConfigurationManager;
        _workflowApproverRepository = workflowApproverRepository;
        _workflowApproverManager = workflowApproverManager;
    }

    public virtual async Task<PagedResultDto<WorkflowConfigurationDto>> GetListAsync(GetWorkflowConfigurationsInput input)
    {
        var filter = ObjectMapper.Map<GetWorkflowConfigurationsInput, WorkflowFilterParams>(input);
        var totalCount = await _workflowConfigurationRepository.GetCountAsync(filter);
        var items = await _workflowConfigurationRepository.GetListAsync(filter);

        return new PagedResultDto<WorkflowConfigurationDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<WorkflowConfiguration>, List<WorkflowConfigurationDto>>(items)
        };
    }

    public virtual async Task<WorkflowConfigurationDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<WorkflowConfiguration, WorkflowConfigurationDto>(await _workflowConfigurationRepository.GetAsync(id));
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        await _workflowConfigurationRepository.DeleteAsync(id);
    }

    [Authorize(QuoteFlowPermissions.WorkflowConfigurations.Edit)]
    public virtual async Task<WorkflowConfigurationDto> UpdateAsync(Guid id, WorkflowConfigurationUpdateDto input)
    {

        var workflowConfiguration = await _workflowConfigurationManager.UpdateAsync(
        id,
         input.WorkflowRole, input.Note, input.ConcurrencyStamp
        );

        var listApproversCurrent = await _workflowApproverRepository.GetListAsync(x => x.WFId == id);

        if (input.Approvers != null)
        {

            var currentSet = listApproversCurrent
                .Select(x => x.Approver?.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var inputSet = input.Approvers
                .Select(x => x.Approver?.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);


            var toAdd = inputSet.Except(currentSet);
            var toRemove = currentSet.Except(inputSet);

            if (toRemove.Any())
            {
                var removeEntities = listApproversCurrent
                    .Where(x => toRemove.Contains(x.Approver?.Trim() ?? string.Empty, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                foreach (var wa in removeEntities)
                {
                    await _workflowApproverRepository.DeleteAsync(wa.Id);
                }
            }

            foreach (var approver in toAdd)
            {
                await _workflowApproverManager.CreateAsync(
                    workflowConfiguration.Id,
                    approver,
                    input.Note
                );
            }
        }

        return ObjectMapper.Map<WorkflowConfiguration, WorkflowConfigurationDto>(workflowConfiguration);
    }
}
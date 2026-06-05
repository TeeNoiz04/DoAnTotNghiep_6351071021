using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.WorkflowConfigurations;

public class WorkflowConfigurationManager : DomainService
{
    protected IWorkflowConfigurationRepository _workflowConfigurationRepository;

    public WorkflowConfigurationManager(IWorkflowConfigurationRepository workflowConfigurationRepository)
    {
        _workflowConfigurationRepository = workflowConfigurationRepository;
    }

    public virtual async Task<WorkflowConfiguration> CreateAsync(
    string workflowType, short workflowLevel, string workflowRole, string? condition = null, string? note = null)
    {
        Check.NotNullOrWhiteSpace(workflowType, nameof(workflowType));
        Check.Length(workflowType, nameof(workflowType), WorkflowConfigurationConsts.WorkflowTypeMaxLength);
        Check.NotNullOrWhiteSpace(workflowRole, nameof(workflowRole));
        Check.Length(workflowRole, nameof(workflowRole), WorkflowConfigurationConsts.WorkflowRoleMaxLength);
        Check.Length(condition, nameof(condition), WorkflowConfigurationConsts.ConditionMaxLength);
        Check.Length(note, nameof(note), WorkflowConfigurationConsts.NoteMaxLength);

        var workflowConfiguration = new WorkflowConfiguration(
         GuidGenerator.Create(),
         workflowType, workflowLevel, workflowRole, condition, note
         );

        return await _workflowConfigurationRepository.InsertAsync(workflowConfiguration);
    }

    public virtual async Task<WorkflowConfiguration> UpdateAsync(
        Guid id,
        string workflowRole, string? note = null, [CanBeNull] string? concurrencyStamp = null
    )
    {


        var workflowConfiguration = await _workflowConfigurationRepository.GetAsync(id);

        workflowConfiguration.WorkflowRole = workflowRole;
        workflowConfiguration.Note = note;

        workflowConfiguration.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _workflowConfigurationRepository.UpdateAsync(workflowConfiguration);
    }

}
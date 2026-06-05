using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.WorkflowConfigurations;

public interface IWorkflowConfigurationsAppService : IApplicationService
{

    Task<PagedResultDto<WorkflowConfigurationDto>> GetListAsync(GetWorkflowConfigurationsInput input);

    Task<WorkflowConfigurationDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<WorkflowConfigurationDto> UpdateAsync(Guid id, WorkflowConfigurationUpdateDto input);
}
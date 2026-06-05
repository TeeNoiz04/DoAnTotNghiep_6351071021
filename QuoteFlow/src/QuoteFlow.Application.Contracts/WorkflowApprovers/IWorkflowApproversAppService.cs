using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.WorkflowApprovers;

public interface IWorkflowApproversAppService : IApplicationService
{

    Task<PagedResultDto<WorkflowApproverDto>> GetListAsync(GetWorkflowApproversInput input);

    Task<WorkflowApproverDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<WorkflowApproverDto> CreateAsync(WorkflowApproverCreateDto input);

    Task<WorkflowApproverDto> UpdateAsync(Guid id, WorkflowApproverUpdateDto input);
}
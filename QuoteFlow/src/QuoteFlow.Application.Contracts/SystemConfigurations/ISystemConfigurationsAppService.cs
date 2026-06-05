using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.SystemConfigurations;

public interface ISystemConfigurationsAppService : IApplicationService
{

    Task<PagedResultDto<SystemConfigurationDto>> GetListAsync(GetSystemConfigurationsInput input);

    Task<SystemConfigurationDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<SystemConfigurationDto> CreateAsync(SystemConfigurationCreateDto input);

    Task<SystemConfigurationDto> UpdateAsync(Guid id, SystemConfigurationUpdateDto input);
}
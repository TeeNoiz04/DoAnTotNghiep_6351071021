using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.Materials.MaterialGroups;

public interface IMaterialGroupsAppService : IApplicationService
{

    Task<PagedResultDto<MaterialGroupDto>> GetListAsync(GetMaterialGroupsInput input);

    Task<MaterialGroupDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<MaterialGroupDto> CreateAsync(MaterialGroupCreateDto input);

    Task<MaterialGroupDto> UpdateAsync(Guid id, MaterialGroupUpdateDto input);
}
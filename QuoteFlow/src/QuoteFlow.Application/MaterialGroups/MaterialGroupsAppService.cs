using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.Materials.MaterialGroups.ParameterObject;
using QuoteFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.MaterialGroups;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.MasterDatas.MaterialGroup)]
public class MaterialGroupsAppService : QuoteFlowAppService, IMaterialGroupsAppService
{

    protected IMaterialGroupRepository _materialGroupRepository;
    protected MaterialGroupManager _materialGroupManager;

    public MaterialGroupsAppService(IMaterialGroupRepository materialGroupRepository, MaterialGroupManager materialGroupManager)
    {

        _materialGroupRepository = materialGroupRepository;
        _materialGroupManager = materialGroupManager;

    }

    public virtual async Task<PagedResultDto<MaterialGroupDto>> GetListAsync(GetMaterialGroupsInput input)
    {
        var filterParams = ObjectMapper.Map<GetMaterialGroupsInput, MaterialGroupFilterParams>(input);
        var totalCount = await _materialGroupRepository.GetCountAsync(filterParams, input.Sorting, input.MaxResultCount, input.SkipCount);
        var items = await _materialGroupRepository.GetListAsync(filterParams, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<MaterialGroupDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<MaterialGroup>, List<MaterialGroupDto>>(items)
        };
    }

    public virtual async Task<MaterialGroupDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<MaterialGroup, MaterialGroupDto>(await _materialGroupRepository.GetAsync(id));
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        try
        {
            await _materialGroupRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("This material group is in use and cannot be deleted.");
        }

    }

    public virtual async Task<MaterialGroupDto> CreateAsync(MaterialGroupCreateDto input)
    {
        var createParams = ObjectMapper.Map<MaterialGroupCreateDto, MaterialGroupCreateParams>(input);
        var materialGroup = await _materialGroupManager.CreateAsync(
        createParams
        );

        return ObjectMapper.Map<MaterialGroup, MaterialGroupDto>(materialGroup);
    }


    public virtual async Task<MaterialGroupDto> UpdateAsync(Guid id, MaterialGroupUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<MaterialGroupUpdateDto, MaterialGroupUpdateParams>(input);
        var materialGroup = await _materialGroupManager.UpdateAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<MaterialGroup, MaterialGroupDto>(materialGroup);
    }
}
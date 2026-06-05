using QuoteFlow.MaterialGroupBuyers.ParameterObjects;
using QuoteFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;

namespace QuoteFlow.MaterialGroupBuyers;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.MasterDatas.AddMaterialGroup)]
public class MaterialGroupBuyersAppService : QuoteFlowAppService, IMaterialGroupBuyersAppService
{

    protected IMaterialGroupBuyerRepository _materialGroupBuyerRepository;
    protected MaterialGroupBuyerManager _materialGroupBuyerManager;

    public MaterialGroupBuyersAppService(IMaterialGroupBuyerRepository materialGroupBuyerRepository, MaterialGroupBuyerManager materialGroupBuyerManager)
    {

        _materialGroupBuyerRepository = materialGroupBuyerRepository;
        _materialGroupBuyerManager = materialGroupBuyerManager;

    }

    public virtual async Task<PagedResultDto<MaterialGroupBuyerDto>> GetListAsync(GetMaterialGroupBuyersInput input)
    {
        var filterParams = ObjectMapper.Map<GetMaterialGroupBuyersInput, MaterialGroupBuyerFilterParams>(input);
        var totalCount = await _materialGroupBuyerRepository.GetCountAsync(filterParams);
        var items = await _materialGroupBuyerRepository.GetListAsync(filterParams);

        return new PagedResultDto<MaterialGroupBuyerDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<MaterialGroupBuyer>, List<MaterialGroupBuyerDto>>(items)
        };
    }

    public virtual async Task<MaterialGroupBuyerDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<MaterialGroupBuyer, MaterialGroupBuyerDto>(await _materialGroupBuyerRepository.GetAsync(id));
    }
    public virtual async Task<List<MaterialGroupBuyerDto>> GetListByBuyerAsync(Guid BuyerId)
    {
        var item = await _materialGroupBuyerRepository.GetListAsync(x => x.BuyerId == BuyerId);
        return ObjectMapper.Map<List<MaterialGroupBuyer>, List<MaterialGroupBuyerDto>>(item);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await _materialGroupBuyerRepository.DeleteAsync(x => x.BuyerId == id);
    }


    public virtual async Task<List<MaterialGroupBuyerDto>> CreateAsync(MaterialGroupBuyerCreatesDto input)
    {
        var createListParams = new List<MaterialGroupBuyerCreateParams>();

        if (input.MaterialGroups != null && input.MaterialGroups.Any())
        {
            foreach (var materialGroup in input.MaterialGroups)
            {
                var entity = new MaterialGroupBuyerCreateParams
                {
                    MaterialGroupId = materialGroup.MaterialGroupId,
                    MaterialGroupCode = materialGroup.MaterialGroupCode,
                    BuyerId = input.BuyerId,
                    BuyerShortName = input.BuyerShortName,
                    Note = input.Note
                };

                createListParams.Add(entity);
            }
        }
        else
        {
            await _materialGroupBuyerRepository.DeleteAsync(x => x.BuyerId == input.BuyerId);
            return [];
        }

        return ObjectMapper.Map<List<MaterialGroupBuyer>, List<MaterialGroupBuyerDto>>(await _materialGroupBuyerManager.CreateAsync(createListParams));


        //return ObjectMapper.Map<MaterialGroupBuyer, MaterialGroupBuyerDto>(materialGroupBuyer);
    }

    [UnitOfWork]
    public virtual async Task<List<MaterialGroupBuyerDto>> UpdateAsync(MaterialGroupBuyerCreatesDto input)
    {
        // Get existing buyers for this BuyerId
        var existingBuyers = await _materialGroupBuyerRepository
            .GetListAsync(x => x.BuyerId == input.BuyerId);

        var existingGroupIds = existingBuyers.Select(x => x.MaterialGroupId).ToList();
        var inputGroupIds = input.MaterialGroups.Select(x => x.MaterialGroupId).ToList();

        // Determine changes
        var groupsToDelete = existingGroupIds.Except(inputGroupIds).ToList();
        var groupsToKeep = existingGroupIds.Intersect(inputGroupIds).ToList();
        var groupsToCreate = inputGroupIds.Except(existingGroupIds).ToList();

        // Prepare CREATE objects
        var createParams = input.MaterialGroups
            .Where(x => groupsToCreate.Contains(x.MaterialGroupId))
            .Select(x => new MaterialGroupBuyerCreateParams
            {
                MaterialGroupId = x.MaterialGroupId,
                MaterialGroupCode = x.MaterialGroupCode,
                BuyerId = input.BuyerId,
                BuyerShortName = input.BuyerShortName,
                Note = input.Note
            })
            .ToList();

        // Collect DELETE ids
        var deleteIds = existingBuyers
            .Where(x => groupsToDelete.Contains(x.MaterialGroupId))
            .Select(x => x.Id)
            .ToList();

        // Keep objects that still exist
        var keepObjects = existingBuyers
            .Where(x => groupsToKeep.Contains(x.MaterialGroupId))
            .ToList();

        // Apply changes
        var createdObjects = await _materialGroupBuyerManager.CreateAsync(createParams);

        if (deleteIds.Any())
        {
            await _materialGroupBuyerRepository.DeleteAsync(x => deleteIds.Contains(x.Id));
        }

        // Combine kept + newly created
        var finalResult = keepObjects.Concat(createdObjects).ToList();

        return ObjectMapper.Map<List<MaterialGroupBuyer>, List<MaterialGroupBuyerDto>>(finalResult);
    }

}
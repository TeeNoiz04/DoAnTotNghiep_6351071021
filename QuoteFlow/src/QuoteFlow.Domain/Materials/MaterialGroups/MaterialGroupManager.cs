using QuoteFlow.Materials.MaterialGroups.ParameterObject;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Materials.MaterialGroups;

public class MaterialGroupManager : DomainService
{
    protected IMaterialGroupRepository _materialGroupRepository;
    public MaterialGroupManager(IMaterialGroupRepository materialGroupRepository)
    {
        _materialGroupRepository = materialGroupRepository;
    }
    public virtual async Task<MaterialGroup> CreateAsync(MaterialGroupCreateParams createParams)
    {
        var materialGroup = new MaterialGroup(
            GuidGenerator.Create(),
            createParams
        );


        return await _materialGroupRepository.InsertAsync(materialGroup);
    }

    public virtual async Task<MaterialGroup> UpdateAsync(
        Guid id,
        MaterialGroupUpdateParams updateParams
    )
    {
        var materialGroup = await _materialGroupRepository.GetAsync(id);

        materialGroup.Code = updateParams.Code;
        materialGroup.Name = updateParams.Name;
        materialGroup.Parent = updateParams.Parent;
        materialGroup.SortOrder = updateParams.SortOrder;
        materialGroup.Note = updateParams.Note;
        materialGroup.IsDeActive = updateParams.IsDeActive;
        materialGroup.MaterialType = updateParams.MaterialType;
        materialGroup.MaterialGroupPSI = updateParams.MaterialGroupPSI;
        materialGroup.AllowKeyAccount = updateParams.AllowKeyAccount;

        materialGroup.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _materialGroupRepository.UpdateAsync(materialGroup);
    }

}

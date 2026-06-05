using QuoteFlow.Materials.MaterialHistories.ParameterObjects;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Materials.MaterialHistories;

public class MaterialHistoryManager : DomainService
{
    protected IMaterialHistoryRepository _materialHistoryRepository;

    public MaterialHistoryManager(IMaterialHistoryRepository materialHistoryRepository)
    {
        _materialHistoryRepository = materialHistoryRepository;
    }

    public virtual async Task<MaterialHistory> CreateAsync(
        MaterialHistoryCreateParams createParams)
    {
        var materialHistory = new MaterialHistory(
         GuidGenerator.Create(),
            createParams
         );

        return await _materialHistoryRepository.InsertAsync(materialHistory);
    }

    public virtual async Task<MaterialHistory> UpdateAsync(
        Guid id,
        MaterialHistoryUpdateParams updateParams
    )
    {
        var materialHistory = await _materialHistoryRepository.GetAsync(id);

        materialHistory.MaterialId = updateParams.MaterialId;
        materialHistory.Action = updateParams.Action;
        materialHistory.Note = updateParams.Note;

        materialHistory.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _materialHistoryRepository.UpdateAsync(materialHistory);
    }

}
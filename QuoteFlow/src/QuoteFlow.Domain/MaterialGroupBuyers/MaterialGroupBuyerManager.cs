using QuoteFlow.MaterialGroupBuyers.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.MaterialGroupBuyers;

public class MaterialGroupBuyerManager : DomainService
{
    protected IMaterialGroupBuyerRepository _materialGroupBuyerRepository;

    public MaterialGroupBuyerManager(IMaterialGroupBuyerRepository materialGroupBuyerRepository)
    {
        _materialGroupBuyerRepository = materialGroupBuyerRepository;
    }

    public virtual async Task<MaterialGroupBuyer> CreateAsync(
    MaterialGroupBuyerCreateParams createParams)
    {

        var materialGroupBuyer = new MaterialGroupBuyer(
         GuidGenerator.Create(),
         createParams
         );

        return await _materialGroupBuyerRepository.InsertAsync(materialGroupBuyer);
    }
    public virtual async Task<List<MaterialGroupBuyer>> CreateAsync(List<MaterialGroupBuyerCreateParams> createParams)
    {
        var data = new List<MaterialGroupBuyer>();

        foreach (var param in createParams)
        {
            var materialGroupBuyer = new MaterialGroupBuyer(
                GuidGenerator.Create(),
                param
            );

            data.Add(materialGroupBuyer);
        }

        await _materialGroupBuyerRepository.InsertManyAsync(data, autoSave: true);

        return data;
    }

    public virtual async Task<MaterialGroupBuyer> UpdateAsync(
        Guid id,
        MaterialGroupBuyerUpdateParams updateParams
    )
    {

        var materialGroupBuyer = await _materialGroupBuyerRepository.GetAsync(id);

        materialGroupBuyer.BuyerId = updateParams.BuyerId;
        materialGroupBuyer.MaterialGroupId = updateParams.MaterialGroupId;
        materialGroupBuyer.MaterialGroupCode = updateParams.MaterialGroupCode;
        materialGroupBuyer.BuyerShortName = updateParams.BuyerShortName;
        materialGroupBuyer.Note = updateParams.Note;

        materialGroupBuyer.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _materialGroupBuyerRepository.UpdateAsync(materialGroupBuyer);
    }

}
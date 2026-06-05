using QuoteFlow.Materials.MaterialStocks.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Materials.MaterialStocks;

public class MaterialStockManager : DomainService
{
    protected IMaterialStockRepository _materialStockRepository;

    public MaterialStockManager(IMaterialStockRepository materialStockRepository)
    {
        _materialStockRepository = materialStockRepository;
    }

    public virtual async Task<MaterialStock> CreateAsync(
    MaterialStockCreateParams createParams)
    {

        var materialStock = new MaterialStock(
         GuidGenerator.Create(),
         createParams
         );

        return await _materialStockRepository.InsertAsync(materialStock);
    }
    public virtual async Task<List<MaterialStock>> CreateListAsync(List<MaterialStockCreateParams> createParamsList)
    {
        var result = new List<MaterialStock>();
        foreach (var createParams in createParamsList)
        {
            var checkExist = await _materialStockRepository.GetListAsync(x => x.GolfaCode == createParams.GolfaCode);
            if (checkExist.Count == 0)
            {
                var material = new MaterialStock(
                    GuidGenerator.Create(),
                    createParams
                );
                result.Add(await _materialStockRepository.InsertAsync(material));
            }

        }

        return result;
    }
    public virtual async Task<MaterialStock> UpdateAsync(
        Guid id,
        MaterialStockUpdateParams updateParams
    )
    {
        var materialStock = await _materialStockRepository.GetAsync(id);

        materialStock.MaterialId = updateParams.MaterialId;
        materialStock.StockCategoryId = updateParams.StockCategoryId;
        materialStock.GolfaCode = updateParams.GolfaCode;
        materialStock.Model = updateParams.Model;
        materialStock.Qty = updateParams.Qty;
        materialStock.Locked = updateParams.Locked;
        materialStock.LockStockKeeping = updateParams.LockStockKeeping;
        materialStock.LockStockSO = updateParams.LockStockSO;
        materialStock.Available_Qty = updateParams.Available_Qty;
        materialStock.Note = updateParams.Note;

        materialStock.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _materialStockRepository.UpdateAsync(materialStock);
    }


}
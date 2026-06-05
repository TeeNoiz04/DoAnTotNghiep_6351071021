using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;

public class MaterialStockLockShipmentManager : DomainService
{
    protected IMaterialStockLockShipmentRepository _materialStockLockShipmentRepository;

    public MaterialStockLockShipmentManager(IMaterialStockLockShipmentRepository materialStockLockShipmentRepository)
    {
        _materialStockLockShipmentRepository = materialStockLockShipmentRepository;
    }

    public virtual async Task<MaterialStockLockShipment> CreateAsync(
    string golfaCode, int? lockOnOrder = null, int? stockOnOrder = null, string? note = null)
    {
        Check.NotNullOrWhiteSpace(golfaCode, nameof(golfaCode));
        Check.Length(golfaCode, nameof(golfaCode), MaterialStockLockShipmentConsts.GolfaCodeMaxLength);
        Check.Length(note, nameof(note), MaterialStockLockShipmentConsts.NoteMaxLength);

        var materialStockLockShipment = new MaterialStockLockShipment(
         GuidGenerator.Create(),
         golfaCode, lockOnOrder, stockOnOrder, note
         );

        return await _materialStockLockShipmentRepository.InsertAsync(materialStockLockShipment);
    }

    public virtual async Task<List<MaterialStockLockShipment>> CreateListAsync(List<MaterialStockLockShipment> createParamsList)
    {
        var result = new List<MaterialStockLockShipment>();
        foreach (var createParams in createParamsList)
        {
            var checkExites = await _materialStockLockShipmentRepository.GetListAsync(x => x.GolfaCode == createParams.GolfaCode);
            if (checkExites.Count == 0)
            {
                var material = new MaterialStockLockShipment(
                   GuidGenerator.Create(),
                   createParams
                );
                result.Add(await _materialStockLockShipmentRepository.InsertAsync(material));
            }

        }

        return result;
    }

    public virtual async Task<MaterialStockLockShipment> UpdateAsync(
        Guid id,
        string golfaCode, int? lockOnOrder = null, int? stockOnOrder = null, string? note = null
    )
    {
        Check.NotNullOrWhiteSpace(golfaCode, nameof(golfaCode));
        Check.Length(golfaCode, nameof(golfaCode), MaterialStockLockShipmentConsts.GolfaCodeMaxLength);
        Check.Length(note, nameof(note), MaterialStockLockShipmentConsts.NoteMaxLength);

        var materialStockLockShipment = await _materialStockLockShipmentRepository.GetAsync(id);

        materialStockLockShipment.GolfaCode = golfaCode;
        materialStockLockShipment.LockOnOrder = lockOnOrder;
        materialStockLockShipment.StockOnOrder = stockOnOrder;
        materialStockLockShipment.Note = note;

        return await _materialStockLockShipmentRepository.UpdateAsync(materialStockLockShipment);
    }

}
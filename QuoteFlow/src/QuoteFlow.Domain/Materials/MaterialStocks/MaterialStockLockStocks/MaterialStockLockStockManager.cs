using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;

public class MaterialStockLockStockManager : DomainService
{
    protected IMaterialStockLockStockRepository _materialStockLockStockRepository;

    public MaterialStockLockStockManager(IMaterialStockLockStockRepository materialStockLockStockRepository)
    {
        _materialStockLockStockRepository = materialStockLockStockRepository;
    }

    public virtual async Task<MaterialStockLockStock> CreateAsync(
    string golfaCode, int qty, int releasedLock, Guid? dPODetailId = null, Guid? stockCategoryId = null, string? note = null)
    {
        Check.NotNullOrWhiteSpace(golfaCode, nameof(golfaCode));
        Check.Length(golfaCode, nameof(golfaCode), MaterialStockLockStockConsts.GolfaCodeMaxLength);
        Check.Length(note, nameof(note), MaterialStockLockStockConsts.NoteMaxLength);

        var materialStockLockStock = new MaterialStockLockStock(
         GuidGenerator.Create(),
         golfaCode, qty, releasedLock, dPODetailId, stockCategoryId, note
         );

        return await _materialStockLockStockRepository.InsertAsync(materialStockLockStock);
    }

    public virtual async Task<MaterialStockLockStock> UpdateAsync(
        Guid id,
        string golfaCode, int qty, int releasedLock, Guid? dPODetailId = null, Guid? stockCategoryId = null, string? note = null
    )
    {
        Check.NotNullOrWhiteSpace(golfaCode, nameof(golfaCode));
        Check.Length(golfaCode, nameof(golfaCode), MaterialStockLockStockConsts.GolfaCodeMaxLength);
        Check.Length(note, nameof(note), MaterialStockLockStockConsts.NoteMaxLength);

        var materialStockLockStock = await _materialStockLockStockRepository.GetAsync(id);

        materialStockLockStock.GolfaCode = golfaCode;
        materialStockLockStock.Qty = qty;
        materialStockLockStock.ReleasedLock = releasedLock;
        materialStockLockStock.DPODetailId = dPODetailId;
        materialStockLockStock.StockCategoryId = stockCategoryId;
        materialStockLockStock.Note = note;

        return await _materialStockLockStockRepository.UpdateAsync(materialStockLockStock);
    }

}
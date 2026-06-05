using QuoteFlow.SpecialInputPrices.ParameterObject;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SpecialInputPrices;

public class SpecialInputPriceManager : DomainService
{
    protected ISpecialInputPriceRepository _specialInputPriceRepository;
    protected ISpecialInputPriceDetailRepository _specialInputPriceDetailRepository;

    public SpecialInputPriceManager(
        ISpecialInputPriceRepository specialInputPriceRepository, ISpecialInputPriceDetailRepository specialInputPriceDetailRepository)
    {
        _specialInputPriceRepository = specialInputPriceRepository;
        _specialInputPriceDetailRepository = specialInputPriceDetailRepository;
    }

    public virtual async Task<SpecialInputPrice> CreateAsync(SpecialInputPriceCreateParams createParams)
    {
        var specialInputPrice = new SpecialInputPrice(GuidGenerator.Create(), createParams);

        var result = await _specialInputPriceRepository.InsertAsync(specialInputPrice);

        return result;
    }

    public virtual async Task<SpecialInputPrice> UpdateAsync(Guid id, SpecialInputPriceUpdateParams updateParams)
    {
        var specialInputPrice = await _specialInputPriceRepository.GetAsync(id);

        specialInputPrice.AccountNo = updateParams.AccountNo;
        specialInputPrice.AccountName = updateParams.AccountName;
        specialInputPrice.ProjectName = updateParams.ProjectName;
        specialInputPrice.MaterialType = updateParams.MaterialType;
        specialInputPrice.SupplierId = updateParams.SupplierId;
        specialInputPrice.SupplierBUId = updateParams.SupplierBUId;
        specialInputPrice.Currency = updateParams.Currency;
        specialInputPrice.ValidFrom = updateParams.ValidFrom;
        specialInputPrice.ValidTo = updateParams.ValidTo;
        specialInputPrice.Note = updateParams.Note;

        // if update status immediately, update below
        //specialInputPrice.Status = updateParams.Status;

        specialInputPrice.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        var result = await _specialInputPriceRepository.UpdateAsync(specialInputPrice);

        return result;
    }

    public virtual async Task DeleteBySpecialInputPriceIdAysnc(Guid specialInputPriceId)
    {
        var spo = await _specialInputPriceRepository.GetAsync(specialInputPriceId);
        var details = await _specialInputPriceDetailRepository.GetListAsync(x => x.SpecialInputPriceId == specialInputPriceId);
        await _specialInputPriceDetailRepository.DeleteManyAsync(details);
        await _specialInputPriceRepository.DeleteAsync(spo);
    }

}
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.SaleOrders.SaleOrderDetails.ParameterObjects;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SaleOrders.SaleOrderDetails;

public class SaleOrderDetailManager : DomainService
{
    protected ISaleOrderDetailRepository _saleOrderDetailRepository;
    protected IDPODetailRepository _dPODetailRepository;

    public SaleOrderDetailManager(ISaleOrderDetailRepository saleOrderDetailRepository, IDPODetailRepository dPODetailRepository)
    {
        _saleOrderDetailRepository = saleOrderDetailRepository;
        _dPODetailRepository = dPODetailRepository;
    }

    public virtual async Task<SaleOrderDetail> CreateAsync(SaleOrderDetailCreateParams input)
    {

        var entity = new SaleOrderDetail(GuidGenerator.Create(), input);

        return await _saleOrderDetailRepository.InsertAsync(entity);
    }


    public virtual async Task<SaleOrderDetail> UpdateAsync(Guid id, SaleOrderDetailUpdateParams input)
    {

        var entity = await _saleOrderDetailRepository.GetAsync(id);

        entity.SaleOrderId = input.SaleOrderId;
        entity.DPODetailId = input.DPODetailId;
        entity.StatusCode = input.StatusCode;
        entity.GolfaCode = input.GolfaCode;
        entity.Qty = input.Qty;
        entity.Price = input.Price;

        entity.Amount = input.Amount;

        entity.VAT = input.VAT;
        entity.StockCategoryId = input.StockCategoryId;
        entity.Note = input.Note;
        entity.Extrafee_Note = input.Extrafee_Note;
        entity.LockStockId = input.LockStockId;

        entity.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        return await _saleOrderDetailRepository.UpdateAsync(entity);
    }
    public virtual async Task<SaleOrderDetail> UpdatePriceAsync(Guid id, SaleOrderDetailUpdateParams input)
    {
        var entity = await _saleOrderDetailRepository.GetAsync(id);


        var oldExtrafee = entity.Extrafee;


        var deltaExtrafee = input.Extrafee - oldExtrafee;


        if (deltaExtrafee > entity.DPODetail.ExtrafeeAvailable)
        {
            throw new UserFriendlyException("The extra fee amount cannot exceed the available DPO extra fee.");
        }

        entity.Price = input.Price;
        entity.Amount = input.Price * entity.Qty;
        entity.Extrafee = input.Extrafee;
        entity.Note = input.Note;
        entity.Extrafee_Note = input.Extrafee_Note;

        entity.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        entity.DPODetail.ExtrafeeUsedInSO += deltaExtrafee;

        entity.DPODetail.ExtrafeeAvailable -= deltaExtrafee;

        await _dPODetailRepository.UpdateAsync(entity.DPODetail, autoSave: true);

        return await _saleOrderDetailRepository.UpdateAsync(entity);
    }

    public virtual async Task<SaleOrderDetail> UpdateNoteAsync(Guid id, SaleOrderDetailUpdateParams input)
    {
        var entity = await _saleOrderDetailRepository.GetAsync(id);



        entity.Note = input.Note;


        entity.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);


        return await _saleOrderDetailRepository.UpdateAsync(entity);
    }


}
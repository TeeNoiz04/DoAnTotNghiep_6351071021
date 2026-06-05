using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.DPOs.DPODetails;

public class DPODetailManager : DomainService
{
    protected IDPODetailRepository _dPODetailRepository;

    public DPODetailManager(IDPODetailRepository dPODetailRepository)
    {
        _dPODetailRepository = dPODetailRepository;
    }

    public virtual async Task<DPODetail> CreateAsync(DPODetailCreateParams createParams)
    {

        var dpoDetail = new DPODetail(GuidGenerator.Create(), createParams);

        return await _dPODetailRepository.InsertAsync(dpoDetail);
    }

    public virtual async Task<List<DPODetail>> CreateListAsync(List<DPODetailCreateParams> createParamsList)
    {
        var result = new List<DPODetail>();
        foreach (var createParams in createParamsList)
        {
            var dpoDetail = new DPODetail(
                GuidGenerator.Create(),
                createParams
            );

            result.Add(await _dPODetailRepository.InsertAsync(dpoDetail));
        }

        return result;
    }

    public virtual async Task<DPODetail> UpdateAsync(Guid id, DPODetailUpdateParams updateParams)
    {

        var dpoDetail = await _dPODetailRepository.GetAsync(id);

        dpoDetail.DPOId = updateParams.DPOId;
        dpoDetail.Status = updateParams.Status;
        dpoDetail.GolfaCode = updateParams.GolfaCode;
        dpoDetail.Model = updateParams.Model;
        dpoDetail.Spec1 = updateParams.Spec1;
        dpoDetail.Spec2 = updateParams.Spec2;
        dpoDetail.Qty = updateParams.Qty;
        dpoDetail.UnitPrice = updateParams.UnitPrice;
        dpoDetail.Amount = updateParams.Amount;
        dpoDetail.RequestedETA = updateParams.RequestedETA;
        dpoDetail.SPOId = updateParams.SPOId;
        dpoDetail.SPOCode = updateParams.SPOCode;
        dpoDetail.CustomerTaxCode = updateParams.CustomerTaxCode;
        dpoDetail.CustomerName = updateParams.CustomerName;
        dpoDetail.LockStock = updateParams.LockStock;
        dpoDetail.LockStockSO = updateParams.LockStockSO;
        dpoDetail.LockShipment = updateParams.LockShipment;
        dpoDetail.Delivered = updateParams.Delivered;
        dpoDetail.NeedDelivery = updateParams.NeedDelivery;
        dpoDetail.Note = updateParams.Note;
        dpoDetail.OrderReason = updateParams.OrderReason;
        dpoDetail.ConfirmNoted = updateParams.ConfirmNoted;
        dpoDetail.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _dPODetailRepository.UpdateAsync(dpoDetail);
    }

    public virtual async Task<IEnumerable<ValidationResult>> ValidationCancelAsync(List<DPODetail> dpoDetails)
    {
        var results = new List<ValidationResult>();
        foreach (var dpoDetail in dpoDetails)
        {
            if ((dpoDetail.LockStockSO != null && dpoDetail.LockStockSO > 0)
                || (dpoDetail.LockStock != null && dpoDetail.LockStock > 0)
                || (dpoDetail.LockShipment != null && dpoDetail.LockShipment > 0)
                || (dpoDetail.Delivered != null && dpoDetail.Delivered > 0))
            {
                results.Add(new ValidationResult($"{dpoDetail.GolfaCode} cannot be cancelled because it has reserved stock, shipment allocation or delivered."));
            }
        }

        return results;

    }
}
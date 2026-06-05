using JetBrains.Annotations;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails.ParameterObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;

public class SpecialInputPriceDetailManager : DomainService
{
    protected ISpecialInputPriceDetailRepository _specialInputPriceDetailRepository;

    public SpecialInputPriceDetailManager(ISpecialInputPriceDetailRepository specialInputPriceDetailRepository)
    {
        _specialInputPriceDetailRepository = specialInputPriceDetailRepository;
    }

    public virtual async Task<SpecialInputPriceDetail> CreateAsync(
    SpecialInputPriceDetailCreateParams createParams)
    {


        var specialInputPriceDetail = new SpecialInputPriceDetail(
         GuidGenerator.Create(),
         createParams
         );

        return await _specialInputPriceDetailRepository.InsertAsync(specialInputPriceDetail);
    }

    public virtual async Task<SpecialInputPriceDetail> UpdateAsync(
        Guid id,
        decimal inputPrice, decimal landedCost, int used, Guid? specialInputPriceId = null, string? materialCode = null, string? model = null, string? spec1 = null, int? limitQty = null, string? note = null, [CanBeNull] string? concurrencyStamp = null
    )
    {
        Check.Length(materialCode, nameof(materialCode), SpecialInputPriceDetailConsts.MaterialCodeMaxLength);
        Check.Length(model, nameof(model), SpecialInputPriceDetailConsts.ModelMaxLength);
        Check.Length(spec1, nameof(spec1), SpecialInputPriceDetailConsts.Spec1MaxLength);
        Check.Length(note, nameof(note), SpecialInputPriceDetailConsts.NoteMaxLength);

        var specialInputPriceDetail = await _specialInputPriceDetailRepository.GetAsync(id);

        specialInputPriceDetail.InputPrice = inputPrice;
        specialInputPriceDetail.LandedCost = landedCost;
        specialInputPriceDetail.Used = used;
        specialInputPriceDetail.SpecialInputPriceId = specialInputPriceId;
        specialInputPriceDetail.MaterialCode = materialCode;
        specialInputPriceDetail.Model = model;
        specialInputPriceDetail.Spec1 = spec1;
        specialInputPriceDetail.LimitQty = limitQty;
        specialInputPriceDetail.Note = note;

        specialInputPriceDetail.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _specialInputPriceDetailRepository.UpdateAsync(specialInputPriceDetail);
    }

    public virtual async Task CreateOrUpdateBatchAsync(
        List<SpecialInputPriceDetailCreateParams> specialDetailCreateParams)
    {
        if (specialDetailCreateParams == null || specialDetailCreateParams.Count == 0)
        {
            return;
        }
        Check.NotNull(specialDetailCreateParams, nameof(specialDetailCreateParams));

        var specialInputPriceIds = specialDetailCreateParams
            .Select(x => x.SpecialInputPriceId)
            .Where(x => x.HasValue)
            .Distinct()
            .ToList();
        var specialInputPriceDetails = await _specialInputPriceDetailRepository.GetListAsync(x => specialInputPriceIds.Contains(x.SpecialInputPriceId), includeDetails: true);

        var detailsToUpdate = new List<SpecialInputPriceDetail>();
        var detailsToInsert = new List<SpecialInputPriceDetail>();

        foreach (var createParams in specialDetailCreateParams)
        {
            var specialInputPriceDetail = specialInputPriceDetails
                .FirstOrDefault(x =>
                    x.SpecialInputPrice!.AccountNo == createParams.AccountNo
                    && string.Equals(x.MaterialCode, createParams.MaterialCode, StringComparison.OrdinalIgnoreCase)
                );

            if (specialInputPriceDetail == null)
            {
                specialInputPriceDetail = new SpecialInputPriceDetail(GuidGenerator.Create(), createParams);
                detailsToInsert.Add(specialInputPriceDetail);
            }
            else
            {
                if (specialInputPriceDetail.Used <= createParams.LimitQty)
                {
                    specialInputPriceDetail.LimitQty = createParams.LimitQty;
                }

                specialInputPriceDetail.InputPrice = createParams.InputPrice;
                specialInputPriceDetail.LandedCost = createParams.LandedCost;
                specialInputPriceDetail.SpecialInputPriceId = createParams.SpecialInputPriceId;
                specialInputPriceDetail.Spec1 = createParams.Spec1;

                //specialInputPriceDetail.Used = createParams.Used;
                //specialInputPriceDetail.MaterialCode = createParams.MaterialCode;
                //specialInputPriceDetail.Model = createParams.Model;
                //specialInputPriceDetail.Note = createParams.Note;

                detailsToUpdate.Add(specialInputPriceDetail);
            }
        }

        if (detailsToInsert.Count > 0)
        {
            await _specialInputPriceDetailRepository.InsertManyAsync(detailsToInsert);
        }
        if (detailsToUpdate.Count > 0)
        {
            await _specialInputPriceDetailRepository.UpdateManyAsync(detailsToUpdate);
        }
    }

}
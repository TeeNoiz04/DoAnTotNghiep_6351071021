using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class PriceOfferDetailManager : DomainService
{
    protected IPriceOfferDetailRepository _priceOfferDetailRepository;

    public PriceOfferDetailManager(IPriceOfferDetailRepository priceOfferDetailRepository)
    {
        _priceOfferDetailRepository = priceOfferDetailRepository;
    }

    public virtual async Task<PriceOfferDetail> CreateAsync(
    PriceOfferDetailCreateParams createParams)
    {
        var priceOfferDetail = new PriceOfferDetail(GuidGenerator.Create(), createParams);

        return await _priceOfferDetailRepository.InsertAsync(priceOfferDetail);
    }

    public virtual async Task CreateBatchAsync(
        List<PriceOfferDetailCreateParams> createParamsList
    )
    {
        var priceOfferDetails = new List<PriceOfferDetail>();
        foreach (var createParams in createParamsList)
        {
            var priceOfferDetail = new PriceOfferDetail(GuidGenerator.Create(), createParams);
            priceOfferDetails.Add(priceOfferDetail);
        }

        await _priceOfferDetailRepository.InsertManyAsync(priceOfferDetails, autoSave: true);
    }

    public virtual async Task<PriceOfferDetail> UpdateAsync(
        Guid id,
        Guid priceOfferId, string golfaCode, string modelName, decimal qty, decimal standardPrice, decimal standardAmount, decimal mEVNOfferPrice, Guid importGuid, string? specialSpec1 = null, string? specialSpec2 = null, decimal? dpoUsed = null, decimal? buyerPrice = null, decimal? requestedAmount = null, decimal? requestedDiscountRatio = null, decimal? priceToCustomer = null, string? competitorBrand = null, string? competitorModel = null, decimal? competitorPrice = null, decimal? landingCost = null, decimal? inputPrice = null, string? inputCurrency = null, decimal? managerMargin = null, decimal? priceOfferDetailMargin = null, string? accountCode = null, string? note = null
    )
    {
        Check.NotNullOrWhiteSpace(golfaCode, nameof(golfaCode));
        Check.Length(golfaCode, nameof(golfaCode), PriceOfferDetailConsts.GolfaCodeMaxLength);
        Check.NotNullOrWhiteSpace(modelName, nameof(modelName));
        Check.Length(modelName, nameof(modelName), PriceOfferDetailConsts.ModelNameMaxLength);
        Check.Length(specialSpec1, nameof(specialSpec1), PriceOfferDetailConsts.SpecialSpec1MaxLength);
        Check.Length(specialSpec2, nameof(specialSpec2), PriceOfferDetailConsts.SpecialSpec2MaxLength);
        Check.Length(competitorBrand, nameof(competitorBrand), PriceOfferDetailConsts.CompetitorBrandMaxLength);
        Check.Length(competitorModel, nameof(competitorModel), PriceOfferDetailConsts.CompetitorModelMaxLength);
        Check.Length(inputCurrency, nameof(inputCurrency), PriceOfferDetailConsts.InputCurrencyMaxLength);
        Check.Length(accountCode, nameof(accountCode), PriceOfferDetailConsts.AccountCodeMaxLength);
        Check.Length(note, nameof(note), PriceOfferDetailConsts.NoteMaxLength);

        var priceOfferDetail = await _priceOfferDetailRepository.GetAsync(id);

        priceOfferDetail.PriceOfferId = priceOfferId;
        priceOfferDetail.GolfaCode = golfaCode;
        priceOfferDetail.ModelName = modelName;
        priceOfferDetail.Qty = qty;
        priceOfferDetail.StandardPrice = standardPrice;
        priceOfferDetail.StandardAmount = standardAmount;
        priceOfferDetail.MEVNOfferPrice = mEVNOfferPrice;
        priceOfferDetail.ImportGuid = importGuid;
        priceOfferDetail.SpecialSpec1 = specialSpec1;
        priceOfferDetail.SpecialSpec2 = specialSpec2;
        priceOfferDetail.DpoUsed = dpoUsed;
        priceOfferDetail.BuyerPrice = buyerPrice;
        priceOfferDetail.RequestedAmount = requestedAmount;
        priceOfferDetail.RequestedDiscountRatio = requestedDiscountRatio;
        priceOfferDetail.PriceToCustomer = priceToCustomer;
        priceOfferDetail.CompetitorBrand = competitorBrand;
        priceOfferDetail.CompetitorModel = competitorModel;
        priceOfferDetail.CompetitorPrice = competitorPrice;
        priceOfferDetail.LandingCost = landingCost;
        priceOfferDetail.InputPrice = inputPrice;
        priceOfferDetail.InputCurrency = inputCurrency;
        priceOfferDetail.ManagerMargin = managerMargin;
        priceOfferDetail.PriceOfferDetailMargin = priceOfferDetailMargin;
        priceOfferDetail.AccountCode = accountCode;
        priceOfferDetail.Note = note;

        return await _priceOfferDetailRepository.UpdateAsync(priceOfferDetail);
    }

    public virtual async Task<PriceOfferDetail> UpdateLandingCostAsync(
        PriceOfferDetailUpdateLandingCostParams updateParams)
    {
        var priceOfferDetail = await _priceOfferDetailRepository.GetAsync(updateParams.Id);
        if (!priceOfferDetail.IsInProgress())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.InvalidPriceOfferStatusForLandingCostUpdate);
        }

        if (updateParams.Qty.HasValue)
            priceOfferDetail.Qty = updateParams.Qty.Value;

        if (updateParams.LandingCost.HasValue)
            priceOfferDetail.LandingCost = updateParams.LandingCost.Value;

        if (updateParams.MEVNOfferPrice.HasValue)
            priceOfferDetail.MEVNOfferPrice = updateParams.MEVNOfferPrice.Value;

        priceOfferDetail.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _priceOfferDetailRepository.UpdateAsync(priceOfferDetail);
    }

    public virtual async Task<IEnumerable<ValidationResult>> ValidationCancelAsync(List<PriceOfferDetail> priceOfferDetails)
    {
        var results = new List<ValidationResult>();
        foreach (var priceOfferDetail in priceOfferDetails)
        {
            if (priceOfferDetail.Status == QuoteFlowStatuses.Cancelled)
            {
                results.Add(new ValidationResult($"{priceOfferDetail.GolfaCode} is already cancelled."));
                continue;
            }

            if (priceOfferDetail.Status == QuoteFlowStatuses.Approved)
            {
                // For SPO.AP type, allow cancelling approved items
                if (priceOfferDetail.PriceOffer?.GetPriceOfferType() == PriceOfferTypes.PriceOfferAP)
                {
                    // Check if it has been used in DPO (DpoUsed > 0)
                    if (priceOfferDetail.DpoUsed > 0)
                    {
                        results.Add(new ValidationResult($"{priceOfferDetail.GolfaCode} cannot be cancelled because it has been used in DPO."));
                    }
                }
                else
                {
                    results.Add(new ValidationResult($"{priceOfferDetail.GolfaCode} cannot be cancelled because it is already approved."));
                }
            }
            else if (priceOfferDetail.Status == QuoteFlowStatuses.Closed)
            {
                results.Add(new ValidationResult($"{priceOfferDetail.GolfaCode} cannot be cancelled because it is closed."));
            }
        }

        return results;
    }

    public virtual async Task<List<PriceOfferDetail>> CancelDetailsAsync(List<PriceOfferDetail> priceOfferDetails, string note)
    {
        var validationResults = await ValidationCancelAsync(priceOfferDetails);
        if (validationResults.Any())
        {
            var errorMessage = string.Join("; ", validationResults.Select(v => v.ErrorMessage));
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.PriceOfferDetailCancelValidationError).WithData("message", errorMessage);
        }

        var updatedDetails = new List<PriceOfferDetail>();

        foreach (var detail in priceOfferDetails)
        {
            detail.Status = QuoteFlowStatuses.Cancelled;
            detail.Note = string.IsNullOrEmpty(detail.Note) ? note : $"{detail.Note}\n[CANCELLED]: {note}";

            var updatedDetail = await _priceOfferDetailRepository.UpdateAsync(detail);
            updatedDetails.Add(updatedDetail);
        }

        return updatedDetails;
    }

}
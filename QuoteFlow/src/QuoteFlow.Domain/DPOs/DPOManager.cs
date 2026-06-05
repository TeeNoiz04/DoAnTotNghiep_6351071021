using QuoteFlow.Buyers;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.SystemCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.DPOs;

public class DPOManager : DomainService
{
    protected IDPORepository _dPORepository;
    protected IDPODetailRepository _dPODetailRepository;
    protected IPriceOfferRepository _priceOfferRepository;
    protected IPriceOfferDetailRepository _priceOfferDetailRepository;

    public DPOManager(IDPORepository dPORepository, IDPODetailRepository dPODetailRepository, IPriceOfferRepository priceOfferRepository, IPriceOfferDetailRepository priceOfferDetailRepository)
    {
        _dPORepository = dPORepository;
        _dPODetailRepository = dPODetailRepository;
        _priceOfferRepository = priceOfferRepository;
        _priceOfferDetailRepository = priceOfferDetailRepository;
    }

    public virtual async Task<DPO> CreateAsync(DPOCreateParams createParams)
    {
        await PopulateDenormalizationFieldsAsync(createParams);

        var isDPONoExists = await _dPORepository.AnyAsync(d => d.DPONo == createParams.DPONo);
        if (isDPONoExists)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.DPO.DPONoAlreadyExists)
                .WithData("dpoNo", createParams.DPONo);
        }

        var dPO = new DPO(createParams.Id ?? GuidGenerator.Create(), createParams);

        var result = await _dPORepository.InsertAsync(dPO, autoSave: true);

        var dpoDetails = (createParams.Details ?? [])
            .Select(detail => new DPODetail(GuidGenerator.Create(), detail));

        await _dPODetailRepository.InsertManyAsync(dpoDetails, autoSave: true);

        // Mark PriceOffers as having DPO used
        await MarkPriceOffersAsDPOUsedAsync(dpoDetails);

        return result;
    }

    public virtual async Task<DPO> CreateAsync(GICCreateParams createParams)
    {
        await PopulateDenormalizationFieldsAsync(createParams);

        createParams.DPONo = await _dPORepository.GenerateGICNoAsync(
            createParams.MaterialType!,
            createParams.GICType,
            createParams.GICProcess,
            createParams.BuyerShortName
        );

        var gic = new DPO(createParams.Id ?? GuidGenerator.Create(), createParams);

        var result = await _dPORepository.InsertAsync(gic, autoSave: true);

        var dpoDetails = (createParams.Details ?? [])
            .Select(detail => new DPODetail(GuidGenerator.Create(), detail));

        await _dPODetailRepository.InsertManyAsync(dpoDetails, autoSave: true);

        // Mark PriceOffers as having DPO used (same logic as DPO)
        await MarkPriceOffersAsDPOUsedAsync(dpoDetails);

        return result;
    }

    public virtual async Task<DPO> CreateIUAsync(GICCreateParams createParams)
    {
        await PopulateDenormalizationFieldsAsync(createParams);

        createParams.DPONo = await _dPORepository.GenerateGICNoAsync(
            createParams.MaterialType!,
            createParams.GICType,
            createParams.GICProcess,
            createParams.BuyerShortName
        );

        var gic = new DPO(createParams.Id ?? GuidGenerator.Create(), createParams);

        var result = await _dPORepository.InsertAsync(gic, autoSave: true);

        var dpoDetails = (createParams.Details ?? [])
         .Select(detail =>
         {
             detail.CustomerId = createParams.BuyerId;
             detail.CustomerName = createParams.BuyerShortName;

             return new DPODetail(GuidGenerator.Create(), detail);
         });

        await _dPODetailRepository.InsertManyAsync(dpoDetails, autoSave: true);

        // Mark PriceOffers as having DPO used (same logic as DPO)
        await MarkPriceOffersAsDPOUsedAsync(dpoDetails);

        return result;
    }

    public virtual async Task<DPO> CreateAsync(GKRCreateParams createParams)
    {
        await PopulateDenormalizationFieldsAsync(createParams);

        var isDPONoExists = await _dPORepository.AnyAsync(d => d.DPONo == createParams.DPONo);
        if (isDPONoExists)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.DPO.DPONoAlreadyExists)
                .WithData("dpoNo", createParams.DPONo);
        }

        var dPO = new DPO(createParams.Id ?? GuidGenerator.Create(), createParams);

        var result = await _dPORepository.InsertAsync(dPO, autoSave: true);

        var dpoDetails = (createParams.Details ?? [])
            .Select(detail => new DPODetail(GuidGenerator.Create(), detail));

        await _dPODetailRepository.InsertManyAsync(dpoDetails, autoSave: true);

        // Mark PriceOffers as having DPO used
        await MarkPriceOffersAsDPOUsedAsync(dpoDetails);

        return result;
    }

    public virtual async Task<DPO> UpdateAsync(Guid id, DPOUpdateParams updateParams)
    {
        await PopulateDenormalizationFieldsAsync(updateParams);

        var dPO = await _dPORepository.GetAsync(id);

        dPO.DPONo = updateParams.DPONo;
        dPO.TotalAmount = updateParams.TotalAmount;
        dPO.DPOType = updateParams.DPOType;
        dPO.GICType = updateParams.GICType;
        dPO.MaterialType = updateParams.MaterialType;
        dPO.CostCenter = updateParams.CostCenter;
        dPO.Status = updateParams.Status;
        dPO.BuyerTypeId = updateParams.BuyerTypeId;
        dPO.BuyerId = updateParams.BuyerId;
        dPO.BuyerShortName = updateParams.BuyerShortName;
        dPO.BuyerTypeDescription = updateParams.BuyerTypeDescription;
        dPO.OrderDate = updateParams.OrderDate;
        dPO.Remark = updateParams.Remark;
        dPO.FileName = updateParams.FileName;

        dPO.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _dPORepository.UpdateAsync(dPO);
    }

    private async Task PopulateDenormalizationFieldsAsync(DPOCreateParams createParams)
    {
        await PopulateDenormalizationFieldsHelperAsync(
            createParams.BuyerTypeId, buyerTypeDescription => createParams.BuyerTypeDescription = buyerTypeDescription, createParams.BuyerTypeDescription,
            createParams.BuyerId, buyerShortName => createParams.BuyerShortName = buyerShortName, createParams.BuyerShortName
        );
    }

    private async Task PopulateDenormalizationFieldsAsync(GICCreateParams createParams)
    {
        await PopulateDenormalizationFieldsHelperAsync(
            createParams.BuyerTypeId, buyerTypeDescription => createParams.BuyerTypeDescription = buyerTypeDescription, createParams.BuyerTypeDescription,
            createParams.BuyerId, buyerShortName => createParams.BuyerShortName = buyerShortName, createParams.BuyerShortName
        );
    }

    private async Task PopulateDenormalizationFieldsAsync(GKRCreateParams createParams)
    {
        await PopulateDenormalizationFieldsHelperAsync(
            createParams.BuyerTypeId, buyerTypeDescription => createParams.BuyerTypeDescription = buyerTypeDescription, createParams.BuyerTypeDescription,
            createParams.BuyerId, buyerShortName => createParams.BuyerShortName = buyerShortName, createParams.BuyerShortName
        );
    }

    private async Task PopulateDenormalizationFieldsAsync(DPOUpdateParams updateParams)
    {
        await PopulateDenormalizationFieldsHelperAsync(
            updateParams.BuyerTypeId, buyerTypeDescription => updateParams.BuyerTypeDescription = buyerTypeDescription, updateParams.BuyerTypeDescription,
            updateParams.BuyerId, buyerShortName => updateParams.BuyerShortName = buyerShortName, updateParams.BuyerShortName
        );
    }

    private async Task PopulateDenormalizationFieldsHelperAsync(
        Guid? buyerTypeId, Action<string?> setBuyerTypeDescription, string? currentBuyerTypeDescription,
        Guid? buyerId, Action<string?> setBuyerShortName, string? currentBuyerShortName)
    {
        var buyerRepository = LazyServiceProvider.LazyGetRequiredService<IBuyerRepository>();
        var systemCategoryRepository = LazyServiceProvider.LazyGetRequiredService<ISystemCategoryRepository>();

        if (buyerTypeId.HasValue)
        {
            var buyerType = await systemCategoryRepository.GetAsync(buyerTypeId.Value);
            if (string.IsNullOrEmpty(currentBuyerTypeDescription))
                setBuyerTypeDescription(buyerType.Description);
        }

        if (buyerId.HasValue)
        {
            var buyer = await buyerRepository.GetAsync(buyerId.Value);

            if (string.IsNullOrEmpty(currentBuyerShortName))
                setBuyerShortName(buyer.ShortName);
        }
    }

    private async Task MarkPriceOffersAsDPOUsedAsync(IEnumerable<DPODetail> dpoDetails)
    {
        // Get DPO details with SPO codes
        var dpoDetailsWithSPO = dpoDetails
            .Where(detail => !string.IsNullOrWhiteSpace(detail.SPOCode))
            .ToList();

        if (!dpoDetailsWithSPO.Any())
            return;

        var spoCodes = dpoDetailsWithSPO
            .Select(detail => detail.SPOCode!)
            .Distinct()
            .ToList();

        // Get the corresponding PriceOffers
        var priceOffers = await _priceOfferRepository.GetListAsync(
            x => spoCodes.Contains(x.PriceOfferCode));

        // Create lookup for SPO codes to PriceOffer IDs
        var spoToPriceOfferId = priceOffers.ToDictionary(po => po.PriceOfferCode, po => po.Id);

        // Get all price offer details for the affected price offers in a single query
        var priceOfferIds = priceOffers.Select(po => po.Id).ToList();
        var allPriceOfferDetails = await _priceOfferDetailRepository.GetListAsync(
            pod => priceOfferIds.Contains(pod.PriceOfferId));

        // Update DpoUsed quantities in PriceOfferDetails
        var updatedPriceOfferDetails = new List<PriceOfferDetail>();

        foreach (var dpoDetail in dpoDetailsWithSPO)
        {
            if (!spoToPriceOfferId.TryGetValue(dpoDetail.SPOCode!, out var priceOfferId))
                continue;

            // Find matching price offer detail based on SPOCode, GolfaCode, Model, and UnitPrice
            var matchingPriceOfferDetail = allPriceOfferDetails
                .FirstOrDefault(pod =>
                    pod.PriceOfferId == priceOfferId &&
                    pod.GolfaCode == dpoDetail.GolfaCode &&
                    pod.ModelName == dpoDetail.Model &&
                    pod.MEVNOfferPrice == dpoDetail.UnitPrice &&
                    pod.IsApproved());

            if (matchingPriceOfferDetail != null)
            {
                // Update DpoUsed with the imported quantity
                var currentDpoUsed = matchingPriceOfferDetail.DpoUsed ?? 0;
                matchingPriceOfferDetail.UsedByDPO(dpoDetail.Qty!.Value);
                updatedPriceOfferDetails.Add(matchingPriceOfferDetail);
            }
        }

        // Update the modified PriceOfferDetails
        if (updatedPriceOfferDetails.Any())
        {
            await _priceOfferDetailRepository.UpdateManyAsync(updatedPriceOfferDetails);
        }

        // Mark each PriceOffer as having DPO used
        foreach (var priceOffer in priceOffers)
        {
            priceOffer.MarkDPOUsed();
        }

        // Update the PriceOffers
        await _priceOfferRepository.UpdateManyAsync(priceOffers);
    }

}
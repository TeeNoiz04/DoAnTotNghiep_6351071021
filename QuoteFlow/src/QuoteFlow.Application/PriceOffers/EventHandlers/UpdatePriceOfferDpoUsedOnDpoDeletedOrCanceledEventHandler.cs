using QuoteFlow.DPOs.Events;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace QuoteFlow.PriceOffers.EventHandlers;

public class UpdatePriceOfferDpoUsedOnDpoDeletedOrCanceledEventHandler :
    ILocalEventHandler<DPODeletedEvent>,
    ILocalEventHandler<DPOCanceledEvent>,
    ILocalEventHandler<DPORejectedEvent>,
    IScopedDependency
{
    protected readonly IPriceOfferRepository _priceOfferRepository;
    protected readonly IPriceOfferDetailRepository _priceOfferDetailRepository;
    protected readonly ILogger<UpdatePriceOfferDpoUsedOnDpoDeletedOrCanceledEventHandler> _logger;

    public UpdatePriceOfferDpoUsedOnDpoDeletedOrCanceledEventHandler(
        IPriceOfferRepository priceOfferRepository,
        IPriceOfferDetailRepository priceOfferDetailRepository,
        ILogger<UpdatePriceOfferDpoUsedOnDpoDeletedOrCanceledEventHandler> logger)
    {
        _priceOfferRepository = priceOfferRepository;
        _priceOfferDetailRepository = priceOfferDetailRepository;
        _logger = logger;
    }

    public async Task HandleEventAsync(DPODeletedEvent eventData)
    {
        await ProcessDPOUsedReductionAsync(eventData.DPOId, eventData.Details, "deletion");
    }

    public async Task HandleEventAsync(DPOCanceledEvent eventData)
    {
        await ProcessDPOUsedReductionAsync(eventData.DPOId, eventData.CanceledDetails, "cancellation");
    }

    public async Task HandleEventAsync(DPORejectedEvent eventData)
    {
        await ProcessDPOUsedReductionAsync(eventData.DPOId, eventData.RejectedDetails, "rejection");
    }

    private async Task ProcessDPOUsedReductionAsync(Guid dpoId, List<DPODeletedEventDetail> details, string operation)
    {
        _logger.LogInformation("Processing DPO {Operation} event for DPO ID: {DPOId} with {DetailCount} details",
            operation, dpoId, details.Count);

        if (!details.Any())
        {
            _logger.LogInformation("No details to process for DPO ID: {DPOId}", dpoId);
            return;
        }

        try
        {
            // Get unique SPOCodes from details
            var spoCodesList = details
                .Where(d => !string.IsNullOrWhiteSpace(d.SPOCode))
                .Select(d => d.SPOCode)
                .Distinct()
                .ToList();

            if (!spoCodesList.Any())
            {
                _logger.LogWarning("No valid SPOCodes found in {Operation} details for DPO ID: {DPOId}", operation, dpoId);
                return;
            }

            // Batch load all relevant PriceOffers with their Details in a single query
            // Since SPOCode is unique, we can use Include to load details efficiently
            var relevantPriceOffers = await _priceOfferRepository.GetListAsync(
                po => spoCodesList.Contains(po.PriceOfferCode));

            // Load details for these price offers
            var priceOfferIds = relevantPriceOffers.Select(po => po.Id).ToList();
            var allPriceOfferDetails = await _priceOfferDetailRepository.GetListAsync(
                pod => priceOfferIds.Contains(pod.PriceOfferId));

            // Create lookup for faster matching
            var priceOfferDetailLookup = allPriceOfferDetails
                .ToLookup(pod => new { pod.PriceOfferId, pod.GolfaCode, pod.MEVNOfferPrice });

            var priceOfferLookup = relevantPriceOffers
                .ToDictionary(po => po.PriceOfferCode, po => po.Id);

            var updatedDetails = new List<PriceOfferDetail>();
            var updateLog = new List<string>();

            // Process each detail
            foreach (var detail in details)
            {
                if (string.IsNullOrWhiteSpace(detail.SPOCode) ||
                    !priceOfferLookup.TryGetValue(detail.SPOCode, out var priceOfferId))
                {
                    _logger.LogWarning("SPOCode {SPOCode} not found in price offers", detail.SPOCode);
                    continue;
                }

                // Find matching PriceOfferDetails using the lookup
                var matchingDetails = priceOfferDetailLookup[new
                {
                    PriceOfferId = priceOfferId,
                    GolfaCode = detail.GolfaCode,
                    MEVNOfferPrice = detail.UnitPrice
                }].ToList();

                foreach (var priceOfferDetail in matchingDetails)
                {
                    // Calculate new DpoUsed value
                    var currentDpoUsed = priceOfferDetail.DpoUsed ?? 0;
                    var newDpoUsed = Math.Max(0, currentDpoUsed - detail.Qty);

                    if (currentDpoUsed != newDpoUsed)
                    {
                        priceOfferDetail.DpoUsed = newDpoUsed;
                        updatedDetails.Add(priceOfferDetail);

                        updateLog.Add($"PriceOfferDetail {priceOfferDetail.Id}: DpoUsed {currentDpoUsed} -> {newDpoUsed} " +
                                    $"(SPOCode: {detail.SPOCode}, GolfaCode: {detail.GolfaCode}, UnitPrice: {detail.UnitPrice})");
                    }
                }
            }

            // Batch update all modified details
            if (updatedDetails.Any())
            {
                await _priceOfferDetailRepository.UpdateManyAsync(updatedDetails);

                _logger.LogInformation(
                    "Batch updated {UpdateCount} PriceOfferDetails for DPO {Operation}. Updates: {Updates}",
                    updatedDetails.Count, operation, string.Join("; ", updateLog));
            }
            else
            {
                _logger.LogInformation("No PriceOfferDetails required updates for DPO ID: {DPOId}", dpoId);
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex,
                "Error batch updating DpoUsed for DPO {Operation} ID: {DPOId}",
                operation, dpoId);

            throw; // Re-throw to ensure the event handler failure is properly handled
        }

        _logger.LogInformation("Completed processing DPO {Operation} event for DPO ID: {DPOId}", operation, dpoId);
    }
}
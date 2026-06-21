using QuoteFlow.Materials;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SpoBatchRequests.Excel;

public class SpoBatchRequestDetailValidator : BaseExcelValidator<SpoBatchRequestDetailImportDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly IMaterialRepository _materialRepository;
    protected readonly IPriceOfferRepository _priceOfferRepository;
    protected readonly IPriceOfferDetailRepository _priceOfferDetailRepository;

    private Dictionary<string, PriceOffer> _spoByCodeLookup = null!;
    // Changed lookup value to store just the Status string, as we don't need the full object
    private Dictionary<Guid, Dictionary<string, string>> _spoMaterialStatusLookup = null!;
    private Dictionary<string, List<string>> _approvedSpoByMaterialLookup = null!;
    private Dictionary<string, List<string>> _closedSpoByMaterialLookup = null!;

    public SpoBatchRequestDetailValidator(
        ExcelValidationConfig config,
        IExcelRowValidator<SpoBatchRequestDetailImportDto> rowValidator,
        ILogger<BaseExcelValidator<SpoBatchRequestDetailImportDto>> logger,
        IServiceProvider provider)
        : base(config, rowValidator, logger)
    {
        _provider = provider;
        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
        _priceOfferDetailRepository = _provider.GetRequiredService<IPriceOfferDetailRepository>();
        _priceOfferRepository = _provider.GetRequiredService<IPriceOfferRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<SpoBatchRequestDetailImportDto> result)
    {

        // ================= 1. PRELOAD MATERIALS =================
        var materials = await _materialRepository.GetListWithDeactiveAsync(
            new(),
            x => new MaterialSupportInfo(x.Id) { GolfaCode = x.GolfaCode });

        var materialLookup = materials
            .Where(x => !string.IsNullOrWhiteSpace(x.GolfaCode))
            .GroupBy(x => x.GolfaCode!.ToUpperInvariant())
            .ToDictionary(g => g.Key, g => g.First());


        // ================= 2. EXTRACT INPUT DATA =================
        var spoCodes = result.ListData
            .Select(x => x.RowData.SPOCode)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var materialCodesFromInput = result.ListData
            .Where(x => string.IsNullOrWhiteSpace(x.RowData.SPOCode))
            .Select(x => x.RowData.GolfaCode)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();


        // ================= 3. LOAD SPO HEADERS =================
        // Loads relevant Price Offers based on Code OR Status (if material lookup is needed)
        var allRelevantSpos = await _priceOfferRepository.GetListAsync(x =>
            spoCodes.Contains(x.PriceOfferCode) ||
            (materialCodesFromInput.Any() &&
             (x.ApprovalStatus == QuoteFlowStatuses.InProgress ||
              x.ApprovalStatus == QuoteFlowStatuses.Approved ||
              x.ApprovalStatus == QuoteFlowStatuses.Closed)));

        _spoByCodeLookup = allRelevantSpos
            .Where(x => spoCodes.Contains(x.PriceOfferCode))
            .ToDictionary(x => x.PriceOfferCode, x => x, StringComparer.OrdinalIgnoreCase);


        // ================= 4. LOAD SPO DETAILS (OPTIMIZED) =================
        var allSpoIds = allRelevantSpos.Select(x => x.Id).ToList();

        // PERFORMANCE FIX: Use GetQueryableAsync + Select
        // This avoids fetching unused columns (CreationTime, CreatorId, etc.) for thousands of rows.
        var detailsQueryable = await _priceOfferDetailRepository.GetQueryableAsync();

        var allSpoDetails = detailsQueryable
            .Where(x => allSpoIds.Contains(x.PriceOfferId))
            .Select(x => new
            {
                x.PriceOfferId,
                x.GolfaCode,
                x.Status
            })
            .ToList();


        // ================= 5. BUILD LOOKUPS =================

        // 5.1: SPO ID -> [Material Code -> Status]
        // This allows O(1) checking if a material exists in an SPO and what its status is.
        _spoMaterialStatusLookup = allSpoDetails
            .GroupBy(x => x.PriceOfferId)
            .ToDictionary(
                g => g.Key,
                g => g.GroupBy(d => d.GolfaCode, StringComparer.OrdinalIgnoreCase)
                      .ToDictionary(
                          dg => dg.Key,
                          // If duplicates exist, pick the one that is 'Approved' if possible
                          dg => dg.OrderByDescending(d => d.Status == QuoteFlowStatuses.Approved)
                                  .Select(d => d.Status)
                                  .First(),
                          StringComparer.OrdinalIgnoreCase
                      )
            );

        // 5.2: Material Code -> List of Approved SPO Codes
        // Only needed when user provides Material Code but NO SPO Code
        _approvedSpoByMaterialLookup = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        _closedSpoByMaterialLookup = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        if (materialCodesFromInput.Any())
        {
            var spoById = allRelevantSpos.ToDictionary(x => x.Id);

            foreach (var detail in allSpoDetails)
            {
                if (!spoById.TryGetValue(detail.PriceOfferId, out var spo) ||
                    string.IsNullOrWhiteSpace(detail.GolfaCode))
                    continue;

                // Build Approved lookup
                if (detail.Status == QuoteFlowStatuses.Approved &&
                    (spo.ApprovalStatus == QuoteFlowStatuses.Approved ||
                     spo.ApprovalStatus == QuoteFlowStatuses.InProgress))
                {
                    if (!_approvedSpoByMaterialLookup.TryGetValue(detail.GolfaCode, out var list))
                    {
                        list = new List<string>();
                        _approvedSpoByMaterialLookup[detail.GolfaCode] = list;
                    }

                    if (!list.Contains(spo.PriceOfferCode))
                    {
                        list.Add(spo.PriceOfferCode);
                    }
                }

                // Build Closed lookup
                if (detail.Status == QuoteFlowStatuses.Closed)
                {
                    if (!_closedSpoByMaterialLookup.TryGetValue(detail.GolfaCode, out var list))
                    {
                        list = new List<string>();
                        _closedSpoByMaterialLookup[detail.GolfaCode] = list;
                    }

                    if (!list.Contains(spo.PriceOfferCode))
                    {
                        list.Add(spo.PriceOfferCode);
                    }
                }
            }
        }

        // ================= 6. VALIDATE ROWS =================
        foreach (var row in result.ListData)
        {
            var spoCode = row.RowData.SPOCode?.Trim();
            var materialCode = row.RowData.GolfaCode?.Trim();
            var materialKey = materialCode?.ToUpperInvariant() ?? "";
            var action = row.RowData.Action?.Trim();

            bool hasSpo = !string.IsNullOrWhiteSpace(spoCode);
            bool hasMaterial = !string.IsNullOrWhiteSpace(materialCode);
            //bool isClosedAction = string.Equals(action, "Close", StringComparison.OrdinalIgnoreCase);

            // 6.1 Check Material Existence in Master Data
            if (hasMaterial && !materialLookup.ContainsKey(materialKey))
            {
                row.Errors.Add($"Material Code '{materialCode}' was not found in the system.");
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
                continue;
            }

            // ========== VALIDATION FOR "CLOSED" ACTION ==========
            if (string.Equals(action, "Open", StringComparison.OrdinalIgnoreCase))
            {
                if (hasSpo && hasMaterial)
                {
                    // Case 1: Both SPO and Material provided
                    // => Material must have Closed status and exist in the SPO (SPO status doesn't matter)
                    if (!_spoByCodeLookup.TryGetValue(spoCode!, out var spo))
                    {
                        row.Errors.Add($"SPO Code = '{spoCode}' is not existed");
                    }
                    else
                    {
                        if (!_spoMaterialStatusLookup.TryGetValue(spo.Id, out var materialStatusMap) ||
                            !materialStatusMap.TryGetValue(materialCode!, out var detailStatus))
                        {
                            row.Errors.Add($"Material Code '{materialCode}' does not exist in SPO '{spoCode}'.");
                        }
                        else if (detailStatus != QuoteFlowStatuses.Closed)
                        {
                            row.Errors.Add($"Material Code '{materialCode}' in SPO '{spoCode}' must have status CLOSED.");
                        }
                    }
                }
                else if (hasSpo && !hasMaterial)
                {
                    // Case 2: Only SPO provided
                    // => SPO must have Closed status
                    if (!_spoByCodeLookup.TryGetValue(spoCode!, out var spo))
                    {
                        row.Errors.Add($"SPO Code = '{spoCode}' is not existed");
                    }
                    else if (spo.ApprovalStatus != QuoteFlowStatuses.Closed)
                    {
                        row.Errors.Add($"SPO '{spoCode}' must have status CLOSED.");
                    }
                }
                else if (!hasSpo && hasMaterial)
                {
                    // Case 3: Only Material provided
                    // => At least one MaterialCode in the list must have Closed status
                    if (!_closedSpoByMaterialLookup.TryGetValue(materialCode!, out var closedSpos) ||
                        !closedSpos.Any())
                    {
                        row.Errors.Add($"Material Code '{materialCode}' must exist in at least one SPO Detail with status CLOSED.");
                    }
                }
                else
                {
                    row.Errors.Add($"Either SPO Code or Material Code must be provided.");
                }
            }
            // ========== VALIDATION FOR NON-CLOSED ACTIONS ==========
            else if (string.Equals(action, "Close", StringComparison.OrdinalIgnoreCase))
            {
                // 6.2 Validation Scenario: SPO Code is provided
                if (hasSpo)
                {
                    if (!_spoByCodeLookup.TryGetValue(spoCode!, out var spo))
                    {
                        row.Errors.Add($"SPO Code = '{spoCode}' is not existed");
                    }
                    else
                    {
                        if (hasMaterial)
                        {
                            // Check if Material exists in this SPO
                            if (!_spoMaterialStatusLookup.TryGetValue(spo.Id, out var materialStatusMap) ||
                                !materialStatusMap.TryGetValue(materialCode!, out var detailStatus))
                            {
                                row.Errors.Add($"Material Code '{materialCode}' does not exist in SPO '{spoCode}'.");
                            }
                            else
                            {
                                bool isSpoApproved = spo.ApprovalStatus == QuoteFlowStatuses.Approved;
                                bool isSpoInProgress = spo.ApprovalStatus == QuoteFlowStatuses.InProgress;
                                bool isSpoDetailApproved = detailStatus == QuoteFlowStatuses.Approved;

                                // Complex Logic: 
                                // 1. If SPO is Approved, Detail MUST be Approved.
                                // 2. If SPO is InProgress, Detail MUST be Approved.
                                if (!(isSpoApproved && isSpoDetailApproved) &&
                                    !(isSpoInProgress && isSpoDetailApproved))
                                {
                                    if (isSpoInProgress && !isSpoDetailApproved)
                                        row.Errors.Add($"The Item that '{materialCode}' belongs to must be APPROVED when SPO '{spoCode}' is In Progress.");
                                    else if (isSpoApproved && !isSpoDetailApproved)
                                        row.Errors.Add($"The Item that '{materialCode}' belongs to must be APPROVED.");
                                    else
                                        row.Errors.Add($"SPO '{spoCode}' must be APPROVED or IN_PROGRESS.");
                                }
                            }
                        }
                        else
                        {
                            // Scenario: Only SPO Code provided (No Material)
                            if (spo.ApprovalStatus != QuoteFlowStatuses.Approved)
                            {
                                row.Errors.Add($"SPO '{spoCode}' must be APPROVED.");
                            }
                        }
                    }
                }
                // 6.3 Validation Scenario: Only Material Code is provided
                else if (hasMaterial)
                {
                    //if (!_approvedSpoByMaterialLookup.TryGetValue(materialCode!, out var approvedSpos) ||
                    //    !approvedSpos.Any())
                    //{
                    //    row.Errors.Add($"Material Code '{materialCode}' must exist in at least one APPROVED SPO Detail.");
                    //}
                }
                else
                {
                    row.Errors.Add($"Either SPO Code or Material Code must be provided.");
                }
            }

            if (row.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
        }
    }

    private class MaterialSupportInfo : Entity<Guid>
    {
        public string GolfaCode { get; set; } = null!;

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}
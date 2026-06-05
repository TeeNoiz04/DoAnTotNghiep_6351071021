using QuoteFlow.GICs;
using QuoteFlow.SaleOrders.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SaleOrders;

public class SaleOrderManager : DomainService
{
    protected ISaleOrderRepository _saleOrderRepository;

    public SaleOrderManager(ISaleOrderRepository saleOrderRepository)
    {
        _saleOrderRepository = saleOrderRepository;
    }


    public virtual async Task<SaleOrder> CreateAsync(SaleOrderCreateParams createParams)
    {
        var newCode = GenerateDraftCode(createParams.SOType ?? SaleOrderTypes.DPO, createParams.GICType);
        var saleOrder = new SaleOrder(GuidGenerator.Create(), newCode, createParams);

        return await _saleOrderRepository.InsertAsync(saleOrder);
    }


    public virtual async Task<SaleOrder> UpdateAsync(Guid id, SaleOrderUpdateParams updateParams)
    {
        var saleOrder = await _saleOrderRepository.GetAsync(id);

        saleOrder.SOSAPNo = updateParams.SOSAPNo;
        saleOrder.MaterialType = updateParams.MaterialType;
        saleOrder.BuyerId = updateParams.BuyerId;
        saleOrder.BuyerType = updateParams.BuyerType;
        saleOrder.BuyerCode = updateParams.BuyerCode;
        saleOrder.BuyerName = updateParams.BuyerName;
        saleOrder.OrderDate = updateParams.OrderDate;
        saleOrder.StockCategoryId = updateParams.StockCategoryId;
        saleOrder.SO_VAT = updateParams.SO_VAT;
        saleOrder.Note = updateParams.Note;
        saleOrder.SAPDONo = updateParams.SAPDONo;
        saleOrder.SAPBillingNo = updateParams.SAPBillingNo;
        saleOrder.SAPDeliveryDate = updateParams.SAPDeliveryDate;
        saleOrder.SAPInvoice = updateParams.SAPInvoice;
        saleOrder.SAPInvoiceDate = updateParams.SAPInvoiceDate;
        saleOrder.DeliveryConfirmed = updateParams.DeliveryConfirmed;
        saleOrder.CompletelyClosed = updateParams.CompletelyClosed;
        if (!string.IsNullOrEmpty(updateParams.GICType))
        {
            saleOrder.GICProcess = updateParams.GICProcess;
            saleOrder.GICType = updateParams.GICType;

            saleOrder.GICGivNo = updateParams.GICGivNo;
            saleOrder.GICGivDate = updateParams.GICGivDate;



        }

        saleOrder.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _saleOrderRepository.UpdateAsync(saleOrder);
    }
    public virtual async Task UpdateStatusInProgressAsync(Guid id)
    {
        var saleOrder = await _saleOrderRepository.GetAsync(id);
        if (saleOrder.StatusCode == QuoteFlowStatuses.Draft && saleOrder.SaleOrderDetails.Any())
        {
            saleOrder.SONo = await GenerateNewCodeAsync(saleOrder.SOType ?? SaleOrderTypes.DPO, saleOrder.GICType);
            saleOrder.StatusCode = QuoteFlowStatuses.InProgress;

            await _saleOrderRepository.UpdateAsync(saleOrder, autoSave: true);
        }
        else if (saleOrder.StatusCode != QuoteFlowStatuses.InProgress)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.InvalidStatusForSubmission);
        }
    }

    public async Task<string> GenerateNewCodeAsync(string soType, string? gicType = null)
    {
        Check.NotNullOrWhiteSpace(soType, nameof(soType));
        var prefix = GetSaleOrderPrefix(soType, gicType);

        var now = DateTime.Now;
        var yearPart = now.Year % 100;
        var yearPrefix = $"{prefix}{yearPart:D2}";

        var latestCode = await _saleOrderRepository.GetLatestCodeAsync(yearPrefix);

        int nextSequence = 1;

        if (!string.IsNullOrEmpty(latestCode))
        {
            var match = Regex.Match(latestCode, @"^" + Regex.Escape(yearPrefix) + @"(\d{4})$");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int lastNumber))
            {
                nextSequence = lastNumber + 1;
            }
        }

        return $"{yearPrefix}{nextSequence:D4}";
    }

    public string GenerateDraftCode(string soType, string? gicType = null)
    {
        Check.NotNullOrWhiteSpace(soType, nameof(soType));
        string prefix = GetSaleOrderPrefix(soType, gicType);

        var draftCode = $"{prefix}{DateTime.Now:yyMM}XX";
        return draftCode;
    }


    public async Task<string> CheckSameGICTypeAsync(string ids, bool? exportSAP = false)
    {
        if (string.IsNullOrWhiteSpace(ids))
            throw new UserFriendlyException("Please select at least one Sales Order before exporting.");

        var idList = ids
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(id => Guid.Parse(id.Trim()))
            .ToList();

        var gicTypeList = new List<string>();
        var gicProcessList = new List<string>();
        foreach (var id in idList)
        {
            var saleOrder = await _saleOrderRepository.GetAsync(id);
            if (!string.IsNullOrWhiteSpace(saleOrder.GICType) &&
                !gicTypeList.Contains(saleOrder.GICType, StringComparer.OrdinalIgnoreCase))
            {
                gicTypeList.Add(saleOrder.GICType);
            }
            gicProcessList.Add(saleOrder.GICProcess ?? string.Empty);
        }
        if (gicTypeList.Count != 1)
        {
            throw new UserFriendlyException("All selected items must share the same GIC Type.");
        }

        var inputGicType = gicTypeList[0];

        if (inputGicType.Equals(GICTypeCodes.Internal, StringComparison.OrdinalIgnoreCase))
        {
            var distinctGICProcess = gicProcessList.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (distinctGICProcess.Count > 1)
            {
                throw new UserFriendlyException("All selected SO-GIC-IU items must share the same GIC Process.");
            }
        }
        else
        {
            if (exportSAP != true)
            {
                throw new UserFriendlyException("All selected SO-GIC must be **Internal Use**.");
            }
        }

        return inputGicType;

    }

    private static string GetSaleOrderPrefix(string soType, string? gicType = null)
    {
        if (soType.Equals(SaleOrderTypes.DPO, StringComparison.OrdinalIgnoreCase))
        {
            return SaleOrderConsts.SaleOrderDPOPrefix;
        }

        if (soType.Equals(SaleOrderTypes.GIC, StringComparison.OrdinalIgnoreCase))
        {
            return gicType switch
            {
                GICTypeCodes.Warranty => SaleOrderConsts.SaleOrderGICWRPrefix,
                GICTypeCodes.Internal => SaleOrderConsts.SaleOrderGICIUPrefix,
                GICTypeCodes.GivingSponsor => SaleOrderConsts.SaleOrderGICFOCPrefix,
                GICTypeCodes.WriteOff => SaleOrderConsts.SaleOrderGICWOPrefix,
                _ => SaleOrderConsts.SaleOrderGICPrefix,
            };
        }
        throw new UserFriendlyException("Invalid Sale Order type.");
    }
}
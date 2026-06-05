using JetBrains.Annotations;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SaleOrdersSapImports;

public class SaleOrdersSapImportManager : DomainService
{
    protected ISaleOrdersSapImportRepository _saleOrdersSapImportRepository;

    public SaleOrdersSapImportManager(ISaleOrdersSapImportRepository saleOrdersSapImportRepository)
    {
        _saleOrdersSapImportRepository = saleOrdersSapImportRepository;
    }

    public virtual async Task<SaleOrdersSapImport> CreateAsync(
    SaleOrderSapImportCreateParams createParams)
    {

        var saleOrdersSapImport = new SaleOrdersSapImport(
         GuidGenerator.Create(),
         createParams
         );

        return await _saleOrdersSapImportRepository.InsertAsync(saleOrdersSapImport);
    }

    public virtual async Task<SaleOrdersSapImport> UpdateAsync(
        Guid id,
        SaleOrderSapImportUpdateParams updateParams, [CanBeNull] string? concurrencyStamp = null
    )
    {
        var saleOrdersSapImport = await _saleOrdersSapImportRepository.GetAsync(id);

        saleOrdersSapImport.MaterialCode = updateParams.MaterialCode;
        saleOrdersSapImport.ModelName = updateParams.ModelName;
        saleOrdersSapImport.SOType = updateParams.SOType;

        saleOrdersSapImport.SONo = updateParams.SONo;
        saleOrdersSapImport.SOSAPNo = updateParams.SOSAPNo;
        saleOrdersSapImport.DOSAPNo = updateParams.DOSAPNo;
        saleOrdersSapImport.BillingNo = updateParams.BillingNo;
        saleOrdersSapImport.DOSAPNo = updateParams.DOSAPNo;
        saleOrdersSapImport.InvoiceNo = updateParams.InvoiceNo;
        saleOrdersSapImport.InvoiceDate = updateParams.InvoiceDate;
        saleOrdersSapImport.Note = updateParams.Note;
        saleOrdersSapImport.FileName = updateParams.FileName;
        saleOrdersSapImport.ImportKey = updateParams.ImportKey;
        saleOrdersSapImport.IsDeleted = updateParams.IsDeleted;

        saleOrdersSapImport.GICLandingCost = updateParams.GICLandingCost;
        saleOrdersSapImport.GICAmountLandingCost = updateParams.GICAmountLandingCost;
        saleOrdersSapImport.GICPORNo = updateParams.GICPORNo;
        saleOrdersSapImport.GICPRNo = updateParams.GICPRNo;
        saleOrdersSapImport.GICGivNo = updateParams.GICGivNo;
        saleOrdersSapImport.GICGivDate = updateParams.GICGivDate;
        saleOrdersSapImport.GICSalesPIC = updateParams.GICSalesPIC;
        saleOrdersSapImport.GICLocation = updateParams.GICLocation;
        saleOrdersSapImport.GICReservationNo = updateParams.GICReservationNo;
        saleOrdersSapImport.GICAssetClass = updateParams.GICAssetClass;
        saleOrdersSapImport.GICMainAssetCode = updateParams.GICMainAssetCode;
        saleOrdersSapImport.GICSubAssetCode = updateParams.GICSubAssetCode;
        saleOrdersSapImport.GICAssetName = updateParams.GICAssetName;
        saleOrdersSapImport.GICNo = updateParams.GICNo;
        saleOrdersSapImport.Disposed = updateParams.Disposed;

        saleOrdersSapImport.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _saleOrdersSapImportRepository.UpdateAsync(saleOrdersSapImport);
    }

    public virtual async Task CreateBatchAsync(
    List<SaleOrderSapImportCreateParams> createParamsList
)
    {
        var details = new List<SaleOrdersSapImport>();
        foreach (var createParams in createParamsList)
        {
            var soDetail = new SaleOrdersSapImport(GuidGenerator.Create(), createParams);
            details.Add(soDetail);
        }

        await _saleOrdersSapImportRepository.InsertManyAsync(details, autoSave: true);
    }

}
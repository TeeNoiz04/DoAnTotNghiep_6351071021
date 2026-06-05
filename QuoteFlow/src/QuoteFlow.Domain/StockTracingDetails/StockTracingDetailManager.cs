using JetBrains.Annotations;
using QuoteFlow.StockTracingDetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.StockTracingDetails;

public class StockTracingDetailManager : DomainService
{
    protected IStockTracingDetailRepository _stockTracingDetailRepository;

    public StockTracingDetailManager(IStockTracingDetailRepository stockTracingDetailRepository)
    {
        _stockTracingDetailRepository = stockTracingDetailRepository;
    }

    public virtual async Task<StockTracingDetail> CreateAsync(StockTracingDetailCreateParams createParams)
    {
        var stockTracingDetail = new StockTracingDetail(
         GuidGenerator.Create(),
         createParams
         );

        return await _stockTracingDetailRepository.InsertAsync(stockTracingDetail);
    }

    public virtual async Task<StockTracingDetail> UpdateAsync(
        Guid id,
        StockTracingDetailUpdateParams updateParams,
        [CanBeNull] string? concurrencyStamp = null
    )
    {


        var stockTracingDetail = await _stockTracingDetailRepository.GetAsync(id);

        stockTracingDetail.StockTracingId = updateParams.StockTracingId;
        stockTracingDetail.ReportType = updateParams.ReportType;
        stockTracingDetail.RowNo = updateParams.RowNo;
        stockTracingDetail.PackingListCode = updateParams.PackingListCode;
        stockTracingDetail.CheckListCode = updateParams.CheckListCode;
        stockTracingDetail.DateEntered = updateParams.DateEntered;
        stockTracingDetail.Stock = updateParams.Stock;
        stockTracingDetail.BU = updateParams.BU;
        stockTracingDetail.Customer = updateParams.Customer;
        stockTracingDetail.Category = updateParams.Category;
        stockTracingDetail.GIV = updateParams.GIV;
        stockTracingDetail.Invoice = updateParams.Invoice;
        stockTracingDetail.SKUCode = updateParams.SKUCode;
        stockTracingDetail.SKUName = updateParams.SKUName;
        stockTracingDetail.Quality = updateParams.Quality;
        stockTracingDetail.Warranty = updateParams.Warranty;
        stockTracingDetail.Unit = updateParams.Unit;
        stockTracingDetail.Series = updateParams.Series;
        stockTracingDetail.OriginCode = updateParams.OriginCode;
        stockTracingDetail.ProductionDate = updateParams.ProductionDate;
        stockTracingDetail.Location = updateParams.Location;
        stockTracingDetail.GolfaCode = updateParams.GolfaCode;

        stockTracingDetail.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _stockTracingDetailRepository.UpdateAsync(stockTracingDetail);
    }

    public virtual async Task CreateBatchAsync(
        List<StockTracingDetailCreateParams> createParamsList
    )
    {
        var details = new List<StockTracingDetail>();
        foreach (var createParams in createParamsList)
        {
            var stockTracingDetail = new StockTracingDetail(GuidGenerator.Create(), createParams);
            details.Add(stockTracingDetail);
        }

        await _stockTracingDetailRepository.InsertManyAsync(details, autoSave: true);
    }

}
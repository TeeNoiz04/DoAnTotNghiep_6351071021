using QuoteFlow.StockTracings.ParameterObjects;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.StockTracings;

public class StockTracingManager : DomainService
{
    protected IStockTracingRepository _stockTracingRepository;

    public StockTracingManager(IStockTracingRepository stockTracingRepository)
    {
        _stockTracingRepository = stockTracingRepository;
    }

    public virtual async Task<StockTracing> CreateAsync(StockTracingCreateParams createParams)
    {

        var stockTracing = new StockTracing(
         GuidGenerator.Create(),
         createParams
         );

        return await _stockTracingRepository.InsertAsync(stockTracing);
    }

    public virtual async Task<StockTracing> UpdateAsync(
        Guid id,
        StockTracingUpdateParams updateParams
    )
    {

        var stockTracing = await _stockTracingRepository.GetAsync(id);

        stockTracing.ReportType = updateParams.ReportType;
        stockTracing.FileName = updateParams.FileName;
        stockTracing.FromDate = updateParams.FromDate;
        stockTracing.ToDate = updateParams.ToDate;
        stockTracing.Note = updateParams.Note;

        stockTracing.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _stockTracingRepository.UpdateAsync(stockTracing);
    }

}
using Dapper;
using QuoteFlow.Buyers;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Helper;
using QuoteFlow.SalesAssignments.ParameterObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.SalesAssignments;

public class EFCoreSalesAssignmentRepository : EfCoreRepository<QuoteFlowDbContext, SalesAssignment, Guid>, ISalesAssignmentRepository
{
    public EFCoreSalesAssignmentRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<SalesAssignment>> GetListAsync(
        SalesAssignmentFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    )
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query
            .Include(x => x.Buyer)
            .Include(x => x.BuyerType)
            .Include(x => x.Location);

        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? BuyerConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        SalesAssignmentFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);

        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<SalesAssignment> ApplyFilter(
        IQueryable<SalesAssignment> query,
        SalesAssignmentFilterParams filterParams)
    {
        var filterText = filterParams.FilterText;
        var buyerId = filterParams.BuyerId;
        var buyerTypeId = filterParams.BuyerTypeId;
        var locationId = filterParams.LocationId;
        var saleUserName = filterParams.SaleUserName;
        var materialType = filterParams.MaterialType;

        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText), e =>
                (e.SaleUserName != null && e.SaleUserName.Contains(filterText!)) ||
                (e.MaterialType != null && e.MaterialType.Contains(filterText!)))
            .WhereIf(buyerId.HasValue, e => e.BuyerId == buyerId!.Value)
            .WhereIf(buyerTypeId.HasValue, e => e.BuyerTypeId == buyerTypeId!.Value)
            .WhereIf(locationId.HasValue, e => e.LocationId == locationId!.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(saleUserName), QueryFilterHelper.BuildMultiFieldSearch<SalesAssignment>(saleUserName, e => e.SaleUserName))
            .WhereIf(!string.IsNullOrWhiteSpace(materialType), e => e.MaterialType.Contains(materialType!));
    }

    public async Task<List<SaleReportByCustomer>> ExportSaleReportAsync(SaleReportFillterParams filterParams)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@pr_fromdate", filterParams.FromDate);
        parameters.Add("@pr_todate", filterParams.ToDate);

        // Added missing Invoice Date parameters
        parameters.Add("@pr_invoice_fromdate", filterParams.InvoiceFromDate);
        parameters.Add("@pr_invoice_todate", filterParams.InvoiceToDate);

        parameters.Add("@pr_userName", filterParams.UserName);
        parameters.Add("@pr_hasFullBuyerAccess", filterParams.HasFullBuyerAccess);
        parameters.Add("@pr_hasStrategicPriceAccess", filterParams.HasStrategicPriceAccess);


        var result = (await connection.QueryAsync<SaleReportByCustomer>(
            "usp_Report_SaleReportByCustomer_R06",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction()
        ))
        .ToList();

        return result;
    }

    public async Task<List<SaleReportByCustomerR05>> ExportSaleReportR05Async(SaleReportFillterParams filterParams)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@fromdate", filterParams.FromDate);
        parameters.Add("@todate", filterParams.ToDate);

        // Added missing Invoice Date parameters
        parameters.Add("@invoice_fromdate", filterParams.InvoiceFromDate);
        parameters.Add("@invoice_todate", filterParams.InvoiceToDate);

        parameters.Add("@userName", filterParams.UserName);
        parameters.Add("@hasFullBuyerAccess", filterParams.HasFullBuyerAccess);
        parameters.Add("@hasStrategicPriceAccess", filterParams.HasStrategicPriceAccess);


        var result = (await connection.QueryAsync<SaleReportByCustomerR05>(
            "usp_Report_SaleReportByCustomer_R05",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction()
        )).ToList();

        return result;
    }
}
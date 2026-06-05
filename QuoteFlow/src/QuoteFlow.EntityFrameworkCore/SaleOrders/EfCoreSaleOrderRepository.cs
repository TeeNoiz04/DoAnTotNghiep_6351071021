using Dapper;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Extensions;
using QuoteFlow.Helper;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.SaleOrders.ParameterObjects;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.SaleOrders;

public class EfCoreSaleOrderRepository : EfCoreRepository<QuoteFlowDbContext, SaleOrder, Guid>, ISaleOrderRepository
{
    public EfCoreSaleOrderRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<SaleOrder>> GetListAsync(
    SaleOrderFilterParams filterParams,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        //var saleOrderDetails = dbContext.Set<SaleOrderDetail>();
        var material = dbContext.Set<Material>();
        var materialGroups = dbContext.Set<MaterialGroup>();
        var saleOrderDetails = dbContext.Set<SaleOrderDetail>();
        var dpoDetails = dbContext.Set<DPODetail>();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams, material, materialGroups, saleOrderDetails, dpoDetails);

        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? SaleOrderConsts.GetDefaultSorting(false)
            : filterParams.Sorting);

        var result = await query
            .Select(x => new SaleOrder(x.Id)
            {
                SONo = x.SONo,
                SOSAPNo = x.SOSAPNo,
                MaterialType = x.MaterialType,
                BuyerId = x.BuyerId,
                BuyerType = x.BuyerType,
                BuyerCode = x.BuyerCode,
                BuyerName = x.BuyerName,
                OrderDate = x.OrderDate,
                StatusCode = x.StatusCode,
                StockCategoryId = x.StockCategoryId,
                SO_VAT = x.SO_VAT,
                SOType = x.SOType,
                GICType = x.GICType,
                GICProcess = x.GICProcess,
                DeliveryConfirmed = x.DeliveryConfirmed,
                LastModifierId = x.LastModifierId,
                CreatorId = x.CreatorId,
                SAPBillingNo = x.SAPBillingNo,
                SAPDeliveryDate = x.SAPDeliveryDate,
                SAPInvoiceDate = x.SAPInvoiceDate,
                Note = x.Note,
                IsDeleted = x.IsDeleted,
                TotalAmount = (decimal)x.SaleOrderDetails
                    .Where(d => !d.IsDeleted)
                    .Sum(d => d.AmountIncludeExtrafee),
                CompletelyClosed = x.CompletelyClosed == null ? null : x.CompletelyClosed == true ? true : false,
                SAPInvoice = x.SAPInvoice,
                CreationTime = x.CreationTime,
                LastModificationTime = x.LastModificationTime,
                LastModifierName = x.LastModifierName,
                LastModifierUsername = x.LastModifierUsername,
                CreatorName = x.CreatorName,
                CreatorUsername = x.CreatorUsername,
                ConcurrencyStamp = x.ConcurrencyStamp
            })
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }
    public virtual async Task<List<T>> GetListAsync<T>(
        SaleOrderFilterParams filterParams,
        Expression<Func<SaleOrder, T>> selector,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var material = dbContext.Set<Material>();
        var materialGroups = dbContext.Set<MaterialGroup>();
        var saleOrderDetails = dbContext.Set<SaleOrderDetail>();
        var dpoDetails = dbContext.Set<DPODetail>();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams, material, materialGroups, saleOrderDetails, dpoDetails);

        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? SaleOrderConsts.GetDefaultSorting(false)
            : filterParams.Sorting);
        var resultQuery = query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .Select(selector).AsQueryable();

        var result = await resultQuery.ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }


    public virtual async Task<long> GetCountAsync(
    SaleOrderFilterParams filterParams,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var material = dbContext.Set<Material>();
        var materialGroups = dbContext.Set<MaterialGroup>();
        var saleOrderDetails = dbContext.Set<SaleOrderDetail>();
        var dpoDetails = dbContext.Set<DPODetail>();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams, material, materialGroups, saleOrderDetails, dpoDetails);
        return await query.CountNoLockAsync(dbContext, cancellationToken);
    }


    protected virtual IQueryable<SaleOrder> ApplyFilter(
    IQueryable<SaleOrder> query,
    SaleOrderFilterParams filter,
    IQueryable<Material> materials = null,
    IQueryable<MaterialGroup> materialGroups = null,
    IQueryable<SaleOrderDetail> saleOrderDetails = null,
    IQueryable<DPODetail> dPODetails = null)
    {
        List<string> statusCodes = [.. filter.StatusCodes.Where(x => !string.IsNullOrEmpty(x))];
        var model = filter.Model;

        query = query
            .WhereIf(!string.IsNullOrWhiteSpace(filter.SONo), e => e.SONo.Contains(filter.SONo!))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.SOSAPNo), e => e.SOSAPNo!.Contains(filter.SOSAPNo!))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.MaterialType), e => e.MaterialType!.Contains(filter.MaterialType!))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.BuyerType), e => e.BuyerType!.Contains(filter.BuyerType!))
            .WhereIf(filter.BuyerId.HasValue, e => e.BuyerId == filter.BuyerId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.BuyerCode), e => e.BuyerCode!.Contains(filter.BuyerCode!))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.BuyerName), e => e.BuyerName!.Contains(filter.BuyerName!))
            .WhereIf(filter.OrderDateMin.HasValue, e => e.OrderDate >= filter.OrderDateMin!.Value)
            .WhereIf(filter.OrderDateMax.HasValue, e => e.OrderDate <= filter.OrderDateMax!.Value)
            .WhereIf(statusCodes.Count > 0, e => statusCodes.Contains(e.StatusCode!))
            .WhereIf(!string.IsNullOrEmpty(filter.InvoiceNo), QueryFilterHelper.BuildMultiFieldSearch<SaleOrder>(filter.InvoiceNo, e => e.SAPInvoice))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.DPONo), e =>
                e.SaleOrderDetails.Any(d => !d.IsDeleted && d.DPODetail.DPO.DPONo != null && d.DPODetail.DPO.DPONo.Contains(filter.DPONo!)))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.MaterialCode), e =>
                e.SaleOrderDetails.Any(d => !d.IsDeleted && d.DPODetail.GolfaCode != null && d.DPODetail.GolfaCode.Contains(filter.MaterialCode!)))
            //.WhereIf(!string.IsNullOrWhiteSpace(filter.Model), e =>
            //    e.SaleOrderDetails.Any(d =>
            //        !d.IsDeleted &&
            //        d.DPODetail != null &&
            //        !string.IsNullOrWhiteSpace(d.DPODetail.Model) &&
            //        d.DPODetail.Model.Contains(filter.Model!)))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Model),
                QueryFilterHelper.BuildDoubleNestedCollectionSearch<SaleOrder, SaleOrderDetail, DPODetail>(
                    filter.Model,
                    e => e.SaleOrderDetails,      // Collection
                    d => d.DPODetail,              // Nested object
                    dpo => dpo.Model,              // Field to search
                    d => !d.IsDeleted))            // Child filter (optional)
            .WhereIf(filter.SODateFrom.HasValue, e => e.OrderDate!.Value.Date >= filter.SODateFrom!.Value.Date)
            .WhereIf(filter.SODateTo.HasValue, e => e.OrderDate!.Value.Date <= filter.SODateTo!.Value.Date)
            .WhereIf(filter.VATDateFrom.HasValue, e => e.SAPInvoiceDate.HasValue && e.SAPInvoiceDate.Value.Date >= filter.VATDateFrom!.Value.Date)
            .WhereIf(filter.VATDateTo.HasValue, e => e.SAPInvoiceDate.HasValue && e.SAPInvoiceDate.Value.Date <= filter.VATDateTo!.Value.Date)
            .ApplyBuyerFilter(filter, x => x.BuyerId)
            .ApplyMaterialTypeFilter(filter, x => x.MaterialType)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.SOType), e => e.SOType != null && e.SOType.Contains(filter.SOType!))
            .WhereIf(!string.IsNullOrEmpty(filter.GicProcess), e => e.GICProcess != null && e.GICProcess == filter.GicProcess)
            .WhereIf(!string.IsNullOrEmpty(filter.GicType), e => e.GICType != null && e.GICType == filter.GicType)

            .WhereIf(!string.IsNullOrEmpty(filter.SAPGICGivNo), e => e.GICGivNo != null && e.GICGivNo.Contains(filter.SAPGICGivNo))
            .WhereIf(filter.SAPGICGivDate.HasValue, e => e.GICGivDate == filter.SAPGICGivDate)
            .WhereIf(filter.CompletelyClosed.HasValue, e => e.CompletelyClosed == filter.CompletelyClosed);


        // Material group filter
        if (!string.IsNullOrWhiteSpace(filter.MaterialGroup) && materials != null && materialGroups != null)
        {
            query = query.Where(e =>
                e.SaleOrderDetails.Any(sod =>
                    !sod.IsDeleted &&
                    materials.Any(m =>
                        m.GolfaCode == sod.GolfaCode &&
                        m.Material_Group == filter.MaterialGroup)));
        }

        // Tax code filter
        if (!string.IsNullOrWhiteSpace(filter.TaxCode) && dPODetails != null && saleOrderDetails != null)
        {
            query = query.Where(e =>
                e.SaleOrderDetails.Any(sod =>
                    !sod.IsDeleted &&
                    dPODetails.Any(dpod =>
                        dpod.Id == sod.DPODetailId &&
                        dpod.CustomerTaxCode == filter.TaxCode)));
        }

        return query.Where(x => !x.IsDeleted);
    }


    public override async Task<SaleOrder> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();

        var result = await query
            .Include(x => x.SaleOrderDetails
                .Where(d => d.IsDeleted == false || d.IsDeleted == null))
                .ThenInclude(d => d.DPODetail)
                    .ThenInclude(dp => dp.DPO)
            .Include(x => x.SaleOrderDetails
                .Where(d => d.IsDeleted == false || d.IsDeleted == null))
                .ThenInclude(d => d.StockCategory)
            .FirstOrDefaultAsync(e => e.Id == id, GetCancellationToken(cancellationToken))
            ?? throw new EntityNotFoundException(typeof(SaleOrder), id);


        if (result != null && result.SaleOrderDetails != null)
        {
            result.SaleOrderDetails = result.SaleOrderDetails
                .OrderByDescending(d => d.DPODetail.DPO.DPONo)
                .ThenByDescending(d => d.CreationTime)
                .ThenBy(d => d.GolfaCode)
                .ToList();

            int index = 1;
            foreach (var detail in result.SaleOrderDetails)
            {
                detail.No = index++;
            }
        }

        return result;
    }

    public async Task<string?> GetLatestCodeAsync(string prefix)
    {
        var query = await GetQueryableAsync();

        var result = await query
            .Where(x => x.SONo != null && x.SONo.StartsWith(prefix) && x.IsDeleted != true && !x.SONo.EndsWith("XX"))
            .OrderByDescending(x => x.SONo)
            .Select(x => x.SONo)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<List<SaleOrderListDetailDPO>> GetListAddDetailDPOAsync(SaleOrderGetListDetailDPOParams filterParams, CancellationToken cancellationToken)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new
        {

            buyerId = filterParams.BuyerId,
            materialType = filterParams.MaterialType,
            vat = filterParams.VAT,
            stockCategoryId = filterParams.StockCategoryId

        };

        var result = (await connection.QueryAsync<SaleOrderListDetailDPO>(
            "usp_SO_GetListDPOsV2",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        )).AsList();
        return result;
    }
    public async Task<long> GetListAddDetailDPOCountAsync(SaleOrderGetListDetailDPOParams filterParams, CancellationToken cancellationToken)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new
        {

            buyerId = filterParams.BuyerId,
            materialType = filterParams.MaterialType,
            vat = filterParams.VAT,
            stockCategoryId = filterParams.StockCategoryId

        };

        var result = (await connection.QueryAsync<SaleOrderListDetailDPO>(
            "usp_SO_GetListDPOsV2",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        )).AsList();
        return result
             .Count();
    }

    public async Task<List<SaleOrderListDetailGIC>> GetListAddDetailGICAsync(SaleOrderGetListDetailGICParams filterParams, CancellationToken cancellationToken)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new
        {
            buyerId = filterParams.BuyerId,
            materialType = filterParams.MaterialType,
            vat = filterParams.VAT,
            stockCategoryId = filterParams.StockCategoryId,
            gicType = filterParams.GICType,
            gicProcess = filterParams.GICProcess
        };

        var result = (await connection.QueryAsync<SaleOrderListDetailGIC>(
            "usp_SO_GetListGICsV2",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        )).AsList();
        return result;
    }

    public async Task<string?> DeleteSODetailAsync(Guid id, string userName, string fullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();


        var parameters = new DynamicParameters();
        parameters.Add("@prSODetailId", id);
        parameters.Add("@userName", userName);
        parameters.Add("@userFullName", fullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_SO_Detail_Delete",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@errMsg");
    }
    public async Task<string?> DeleteSOAsync(Guid saleOrderId, string userName, string fullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@prSOId", saleOrderId);
        parameters.Add("@userName", userName);
        parameters.Add("@userFullName", fullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_SO_Delete",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@errMsg");
    }

    public async Task<string?> SaveSODetailAsync(List<SaleOrderAddedDetailDPOParams> addedList)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var errorList = new List<string>();

        try
        {
            await dbContext.Database.OpenConnectionAsync();

            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var added in addedList)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@prSOId", added.PrSOId);
                        parameters.Add("@dpoDetailId", added.DPODetailId);
                        parameters.Add("@lockstockId", added.LockStockId);
                        parameters.Add("@materialCode", added.MaterialCode);
                        parameters.Add("@qty", added.Qty);
                        parameters.Add("@price", added.Price);
                        parameters.Add("@vat", added.VAT);
                        parameters.Add("@stockCategoryId", added.StockCategoryId);
                        parameters.Add("@extrafee", added.Extrafee);
                        parameters.Add("@note", added.Note);
                        parameters.Add("@materialtype", added.MaterialType);
                        parameters.Add("@userName", added.UserName);
                        parameters.Add("@userFullName", added.UserFullName);
                        parameters.Add("@errMsg", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);

                        await connection.ExecuteAsync(
                            "usp_SO_Detail_AddNew",
                            parameters,
                            transaction: transaction.GetDbTransaction(),
                            commandType: CommandType.StoredProcedure
                        );

                        var errorMsg = parameters.Get<string>("@errMsg");
                        if (!string.IsNullOrWhiteSpace(errorMsg))
                        {
                            errorList.Add($"{Environment.NewLine} {errorMsg};");
                        }
                    }
                    var limitedErrors = errorList.Take(10).ToList();
                    var message = string.Join(Environment.NewLine, limitedErrors);
                    if (errorList.Count > 0)
                    {
                        if (errorList.Count > 10)
                        {
                            message += $"{Environment.NewLine}... {errorList.Count - 10} more errors.";
                        }

                        await transaction.RollbackAsync();
                        return message;
                    }

                    await transaction.CommitAsync();
                    return null;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        finally
        {
            await dbContext.Database.CloseConnectionAsync();
        }

        return null;
    }


    public async Task<string?> ReOpenSO(Guid saleOrderId, string userName, string fullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var parameters = new DynamicParameters();
        parameters.Add("@prSOId", saleOrderId);
        parameters.Add("@userName", userName);
        parameters.Add("@userFullName", fullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_SO_ReOpen_ClosedSO",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@errMsg");
    }
    public async Task<string?> ImportSAPDataAsync(Guid importGuid, string userName, string userFullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@importGuid", importGuid);
        parameters.Add("@userName", userName);
        parameters.Add("@userFullName", userFullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_SO_ImportSAPData",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@errMsg");
    }
    public async Task<string?> ImportSAPDataGICAsync(Guid importGuid, string userName, string userFullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@importGuid", importGuid);
        parameters.Add("@userName", userName);
        parameters.Add("@userFullName", userFullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_SO_ImportSAPData_GIC",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@errMsg");
    }

    public async Task<string?> ImportInternalUseChangeDataGICAsync(Guid importGuid, string userName, string userFullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@importGuid", importGuid);
        parameters.Add("@userName", userName);
        parameters.Add("@userFullName", userFullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_SO_ImportChanges",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@errMsg");
    }

    public async Task<string?> ConfirmDelivery(Guid saleOrderId, string userName, string userFullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@prSOId", saleOrderId);
        parameters.Add("@userName", userName);
        parameters.Add("@userFullName", userFullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_SO_ConfirmDelivery",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@errMsg");
    }

    public async Task<string?> ConfirmDeliveryGIC(Guid saleOrderId, string userName, string userFullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@prSOId", saleOrderId);
        parameters.Add("@userName", userName);
        parameters.Add("@userFullName", userFullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_SO_ConfirmDelivery_GIC",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@errMsg");
    }

    private static object? NullIfEmpty(string? s)
       => string.IsNullOrWhiteSpace(s) ? null : s;
    public async Task<List<SaleOrderExportSAPGICData>> ExportSAPGICDataAsync(SaleOrderListExportSAPDataParams input, bool? isExport = false)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@username", NullIfEmpty(input.Username));
        parameters.Add("@statusCode", NullIfEmpty(input.StatusCode));
        parameters.Add("@sono", NullIfEmpty(input.SONo));
        parameters.Add("@sosapno", NullIfEmpty(input.SOSAPNo));
        parameters.Add("@dpono", NullIfEmpty(input.DPONo));
        parameters.Add("@golfacode", NullIfEmpty(input.MaterialCode));
        parameters.Add("@invoiceNo", NullIfEmpty(input.InvoiceNo));
        parameters.Add("@model", NullIfEmpty(input.Model));
        parameters.Add("@buyerType", NullIfEmpty(input.BuyerType));
        parameters.Add("@buyer", NullIfEmpty(input.BuyerCode));
        parameters.Add("@materialGroup", NullIfEmpty(input.MaterialGroup));
        parameters.Add("@customerTaxCode", NullIfEmpty(input.CustomerTaxCode));
        parameters.Add("@materialType", NullIfEmpty(input.MaterialType));
        parameters.Add("@gicType", NullIfEmpty(input.GicType));
        parameters.Add("@gicProcess", NullIfEmpty(input.GicProcess));
        parameters.Add("@lstSO", NullIfEmpty(input.LstSO));

        parameters.Add("@fromdate", input.SODateFrom);
        parameters.Add("@todate", input.SODateTo);
        parameters.Add("@invoiceFromdate", input.VATDateFrom);
        parameters.Add("@invoiceTodate", input.VATDateTo);

        if (isExport == true)
        {
            var result = (await connection.QueryAsync<SaleOrderExportSAPGICData>(
            "usp_SO_ExportSAPData_GIC_InternalUseChanges",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandTimeout: 180
            )).ToList();

            return result;
        }
        else
        {
            var result = (await connection.QueryAsync<SaleOrderExportSAPGICData>(
            "usp_SO_ExportSAPData_GIC",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandTimeout: 180
            )).ToList();

            return result;
        }
    }


    //private static object? NullIfEmpty(string? s)
    //    => string.IsNullOrWhiteSpace(s) ? null : s;

    public async Task<List<SaleOrderListExportSAPData>> ExportSAPDataAsync(SaleOrderListExportSAPDataParams input)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@username", NullIfEmpty(input.Username));
        parameters.Add("@fromdate", input.SODateFrom);
        parameters.Add("@todate", input.SODateTo);
        parameters.Add("@statusCode", NullIfEmpty(input.StatusCode));
        parameters.Add("@sono", NullIfEmpty(input.SONo));
        parameters.Add("@sosapno", NullIfEmpty(input.SOSAPNo));
        parameters.Add("@dpono", NullIfEmpty(input.DPONo));
        parameters.Add("@dono", NullIfEmpty(input.DONo));
        parameters.Add("@golfacode", NullIfEmpty(input.MaterialCode));
        parameters.Add("@lstSO", NullIfEmpty(input.LstSO));
        parameters.Add("@invoiceNo", NullIfEmpty(input.InvoiceNo));
        parameters.Add("@model", NullIfEmpty(input.Model));
        parameters.Add("@buyerType", NullIfEmpty(input.BuyerType));
        parameters.Add("@buyer", NullIfEmpty(input.BuyerCode));
        parameters.Add("@materialGroup", NullIfEmpty(input.MaterialGroup));
        parameters.Add("@customerTaxCode", NullIfEmpty(input.CustomerTaxCode));
        parameters.Add("@materialType", NullIfEmpty(input.MaterialType));
        parameters.Add("@invoiceFromdate", input.VATDateFrom);
        parameters.Add("@invoiceTodate", input.VATDateTo);

        var result = (await connection.QueryAsync<SaleOrderListExportSAPData>(
            "usp_SO_ExportSAPData",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandTimeout: 180
        )).ToList();

        return result;
    }

    public async Task<List<SaleOrderExportData>> ExportSODataAsync(SaleOrderListExportSAPDataParams input)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@fromdate", input.SODateFrom);
        parameters.Add("@todate", input.SODateTo);
        parameters.Add("@status", NullIfEmpty(input.StatusCode));
        parameters.Add("@sono", NullIfEmpty(input.SONo));
        parameters.Add("@sosapno", NullIfEmpty(input.SOSAPNo));
        parameters.Add("@dpono", NullIfEmpty(input.DPONo));
        //parameters.Add("@donoNullIfEmpty(", input.DONo));
        parameters.Add("@golfacode", NullIfEmpty(input.MaterialCode));
        parameters.Add("@invoiceNo", NullIfEmpty(input.InvoiceNo));
        parameters.Add("@model", NullIfEmpty(input.Model));
        parameters.Add("@buyerType", NullIfEmpty(input.BuyerType));
        parameters.Add("@buyerId", (input.BuyerId));
        parameters.Add("@materialGroup", NullIfEmpty(input.MaterialGroup));
        parameters.Add("@customerTaxCode", NullIfEmpty(input.CustomerTaxCode));
        parameters.Add("@materialType", NullIfEmpty(input.MaterialType));
        parameters.Add("@invoiceFromdate", (input.VATDateFrom));
        parameters.Add("@invoiceTodate", (input.VATDateTo));
        parameters.Add("@lstSO", NullIfEmpty(input.LstSO));
        parameters.Add("@username", NullIfEmpty(input.Username));
        parameters.Add("@hasFullBuyerAccess", input.HasFullBuyerAccess);

        var result = (await connection.QueryAsync<SaleOrderExportData>(
            "usp_SO_ExportData",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandTimeout: 180
        )).ToList();

        return result;
    }

    public async Task<string?> UpdateSODetailExtrafeeAsync(SODetailExtrafeeUpdateParams input)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@prSODetailId", input.SODetailId);
        parameters.Add("@extrafee", input.Extrafee);
        parameters.Add("@extrafeeNote", input.ExtrafeeNode);
        parameters.Add("@userName", input.UserName);
        parameters.Add("@userFullName", input.UserFullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
           "usp_SO_Detail_Extrafee_Update",
           parameters,
           commandType: CommandType.StoredProcedure,
           transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction()
        );

        // L?y gi� tr? t? output parameter
        var errMsg = parameters.Get<string>("@errMsg");

        return errMsg; // n?u null th� coi nh? th�nh c�ng
    }

    public async Task<string?> ImportSAPDataGICInternalUseAsync(Guid importGuid, string userName, string userFullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@importGuid", importGuid);
        parameters.Add("@userName", userName);
        parameters.Add("@userFullName", userFullName);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_SO_ImportSAPData_GIC_InternalUse",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<string>("@errMsg");
    }
}
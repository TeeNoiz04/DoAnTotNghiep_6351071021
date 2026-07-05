using Dapper;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.Models;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Extensions;
using QuoteFlow.Helper;
using QuoteFlow.Materials;
using QuoteFlow.PriceOffers;
using QuoteFlow.Shared.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.DPOs;

public class EfCoreDPORepository : EfCoreRepository<QuoteFlowDbContext, DPO, Guid>, IDPORepository
{
    private IConnectionStringResolver _connectionStringResolver;
    public EfCoreDPORepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider, IConnectionStringResolver connectionStringResolver)
        : base(dbContextProvider)
    {
        _connectionStringResolver = connectionStringResolver;
    }

    public virtual async Task<List<DPO>> GetListAsync(
        DPOFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryableAsync())
            .Where(x => !string.IsNullOrWhiteSpace(x.DPONo));
        var dbContext = await GetDbContextAsync();

        var materials = dbContext.Set<Material>();
    
        var priceOffers = dbContext.Set<PriceOffer>();
  

        query = ApplyFilter(query, filterParams, materials, priceOffers);

        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DPOConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<List<DPO>> GetListGICAsync(
        GICFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilterGIC((await GetQueryableAsync()), filterParams);

        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DPOConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }





    public override async Task<DPO> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        var result = await query
            .Include(x => x.Details.OrderBy(x => x.RowNo))
            .FirstOrDefaultAsync(e => e.Id == id, GetCancellationToken(cancellationToken))
            ?? throw new EntityNotFoundException(typeof(DPO), id);

        return result;
    }

    public virtual async Task<long> GetCountAsync(
        DPOFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var materials = dbContext.Set<Material>();
    
        var priceOffers = dbContext.Set<PriceOffer>();
       

        var query = ApplyFilter((await GetDbSetAsync()), filterParams, materials, priceOffers);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<DPO> ApplyFilter(
    IQueryable<DPO> query,
    DPOFilterParams filterParams,
    IQueryable<Material> materials,
    IQueryable<PriceOffer> priceOffers)
    {
        var filterText = filterParams.FilterText;
        var dPONo = filterParams.DPONo;
        var materialCode = filterParams.MaterialCode;
        var modelName = filterParams.ModelName;
        var poNo = filterParams.PONo;
        var customerName = filterParams.CustomerName;
        var orderDateMin = filterParams.OrderDateMin;
        var orderDateMax = filterParams.OrderDateMax;
        var specialPriceCode = filterParams.SpecialPriceCode;
        var materialGroup = filterParams.MaterialGroup;
        var taxCode = filterParams.TaxCode;
        var salesOrg = filterParams.SalesOrg;
        var materialType = filterParams.MaterialType;
        var supplierId = filterParams.SupplierId;
        var dPOSubType = filterParams.DPOSubType;
        var costCenter = filterParams.CostCenter;
        var status = filterParams.Status;
        var buyerTypeId = filterParams.BuyerTypeId;
        var buyerShortName = filterParams.BuyerShortName;
        var totalAmountMin = filterParams.TotalAmountMin;
        var totalAmountMax = filterParams.TotalAmountMax;
        var remark = filterParams.Remark;
        var fileName = filterParams.FileName;

        query = query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.DPONo!.Contains(filterText!) || e.DPOType!.Contains(filterText!) || e.MaterialType!.Contains(filterText!) || e.CostCenter!.Contains(filterText!) || e.Status!.Contains(filterText!) || e.BuyerShortName!.Contains(filterText!) || e.Remark!.Contains(filterText!) || e.FileName!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(dPONo), QueryFilterHelper.BuildMultiFieldSearch<DPO>(dPONo, e => e.DPONo))
                .WhereIf(!string.IsNullOrWhiteSpace(customerName), QueryFilterHelper.BuildNestedCollectionSearch<DPO, DPODetail>(customerName, e => e.Details, d => d.CustomerName))
                .WhereIf(!string.IsNullOrWhiteSpace(taxCode), e => e.Details.Any(d => d.CustomerTaxCode.Contains(taxCode)))
                .Where(e => e.DPOType == DPOTypes.DPO)
                .WhereIf(!string.IsNullOrWhiteSpace(materialType), e => e.MaterialType.Contains(materialType))
                .WhereIf(!string.IsNullOrWhiteSpace(costCenter), e => e.CostCenter.Contains(costCenter))
                .WhereIf(!string.IsNullOrWhiteSpace(status), e => e.Status.Contains(status))
                .WhereIf(buyerTypeId.HasValue, e => e.BuyerTypeId == buyerTypeId)
                .WhereIf(!string.IsNullOrWhiteSpace(buyerShortName), e => e.BuyerShortName.Contains(buyerShortName))
                .WhereIf(orderDateMin.HasValue, e => e.OrderDate!.Value.Date >= orderDateMin!.Value.Date)
                .WhereIf(orderDateMax.HasValue, e => e.OrderDate!.Value.Date <= orderDateMax!.Value.Date)
                .WhereIf(totalAmountMin.HasValue, e => e.TotalAmount >= totalAmountMin!.Value)
                .WhereIf(totalAmountMax.HasValue, e => e.TotalAmount <= totalAmountMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(remark), e => e.Remark.Contains(remark))
                .WhereIf(!string.IsNullOrWhiteSpace(fileName), e => e.FileName.Contains(fileName))
                .Where(x => !string.IsNullOrWhiteSpace(x.DPONo))
                .ApplyBuyerFilter(filterParams, x => x.BuyerId)
                .ApplyMaterialTypeFilter(filterParams, x => x.MaterialType);

        if (materials is not null)
        {
            var hasMaterialCode = !string.IsNullOrWhiteSpace(materialCode);
            var hasModelName = !string.IsNullOrWhiteSpace(modelName);
            var hasSupplierId = !string.IsNullOrWhiteSpace(supplierId);
            var hasMaterialGroup = !string.IsNullOrWhiteSpace(materialGroup);

            if (hasMaterialCode || hasModelName || hasSupplierId || hasMaterialGroup)
            {
                var filteredMaterials = materials.AsQueryable();

                if (hasMaterialCode)
                {
                    filteredMaterials = filteredMaterials.Where(
                        QueryFilterHelper.BuildMultiFieldSearch<Material>(materialCode, m => m.GolfaCode));
                }

                if (hasModelName)
                {
                    filteredMaterials = filteredMaterials.Where(
                        QueryFilterHelper.BuildMultiFieldSearch<Material>(modelName, m => m.Model));
                }

                if (hasSupplierId)
                {
                    filteredMaterials = filteredMaterials.Where(m => m.SupplierCode == supplierId);
                }

                if (hasMaterialGroup)
                {
                    filteredMaterials = filteredMaterials.Where(m => m.Material_Group == materialGroup);
                }

                // Áp dụng vào DPO query
                query = query.Where(e =>
                    e.Details.Any(dpod =>
                        filteredMaterials.Any(m => m.GolfaCode == dpod.GolfaCode)));
            }
        }

        // Special Price Code filter
        if (!string.IsNullOrWhiteSpace(specialPriceCode) && priceOffers is not null)
        {
            query = query.Where(e =>
                e.Details.Any(dpod =>
                    priceOffers.Any(po =>
                        po.Id == dpod.SPOId &&
                        po.PriceOfferCode == specialPriceCode)));
        }

        // PONO filter
        //if (!string.IsNullOrWhiteSpace(poNo) && purchaseOrderLockShipment is not null)
        //{
        //    query = query.Where(e =>
        //        e.Details.Any(dpod =>
        //            purchaseOrderLockShipment.Any(pol =>
        //                pol.DPODetailId == dpod.Id &&
        //                pol.PONo != null &&
        //                pol.PONo.Contains(poNo))));
        //}

        return query;
    }


    // protected virtual IQueryable<DPO> ApplyFilterGIC(
    //     IQueryable<DPO> query,
    //     string? filterText = null,
    //     string? gicNo = null,
    //     string? gicType = null,
    //     string? gicProcess = null,
    //     string? materialType = null,
    //     string? costCenter = null,
    //     string? status = null,
    //     Guid? buyerTypeId = null,
    //     Guid? buyerId = null,
    //     string? buyerShortName = null,
    //     DateTime? orderDateMin = null,
    //     DateTime? orderDateMax = null,
    //     decimal? totalAmountMin = null,
    //     decimal? totalAmountMax = null,
    //     string? remark = null,
    //     string? fileName = null)
    // {
    //     return query
    //             .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.DPONo!.Contains(filterText!) || e.DPOType!.Contains(filterText!) || e.GICType!.Contains(filterText!) || e.MaterialType!.Contains(filterText!) || e.CostCenter!.Contains(filterText!) || e.Status!.Contains(filterText!) || e.BuyerShortName!.Contains(filterText!) || e.Remark!.Contains(filterText!) || e.FileName!.Contains(filterText!))
    //             .WhereIf(!string.IsNullOrWhiteSpace(gicNo), e => e.DPONo.Contains(gicNo))
    //             .WhereIf(!string.IsNullOrWhiteSpace(gicType), e => e.GICType.Contains(gicType))
    //             .WhereIf(!string.IsNullOrWhiteSpace(gicProcess), e => e.GICProcess.Contains(gicProcess))
    //             .WhereIf(!string.IsNullOrWhiteSpace(materialType), e => e.MaterialType.Contains(materialType))
    //             .WhereIf(!string.IsNullOrWhiteSpace(costCenter), e => e.CostCenter.Contains(costCenter))
    //             .WhereIf(!string.IsNullOrWhiteSpace(status), e => e.Status.Contains(status))
    //             .WhereIf(buyerTypeId.HasValue, e => e.BuyerTypeId == buyerTypeId)
    //             .WhereIf(buyerId.HasValue, e => e.BuyerId == buyerId)
    //             .WhereIf(!string.IsNullOrWhiteSpace(buyerShortName), e => e.BuyerShortName.Contains(buyerShortName))
    //             .WhereIf(orderDateMin.HasValue, e => e.OrderDate >= orderDateMin!.Value)
    //             .WhereIf(orderDateMax.HasValue, e => e.OrderDate <= orderDateMax!.Value)
    //             .WhereIf(totalAmountMin.HasValue, e => e.TotalAmount >= totalAmountMin!.Value)
    //             .WhereIf(totalAmountMax.HasValue, e => e.TotalAmount <= totalAmountMax!.Value)
    //             .WhereIf(!string.IsNullOrWhiteSpace(remark), e => e.Remark.Contains(remark))
    //             .WhereIf(!string.IsNullOrWhiteSpace(fileName), e => e.FileName.Contains(fileName))
    //             .Where(x => !string.IsNullOrWhiteSpace(x.DPONo));
    // }

    public virtual async Task LockStockAutoAsync(
        Guid dpoId,
        Guid stockCategoryId,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@pr_DPOId", dpoId, DbType.Guid);
        parameters.Add("@stockCategoryId", stockCategoryId, DbType.Guid);
        parameters.Add("@userName", userName, DbType.String, size: 50);
        parameters.Add("@userFullName", userFullName, DbType.String, size: 500);
        parameters.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

        await connection.ExecuteAsync(
            "usp_DPO_LockStockAuto",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        var errorMessage = parameters.Get<string>("@errMsg");
        if (!string.IsNullOrEmpty(errorMessage))
        {
            throw new UserFriendlyException(errorMessage);
        }
    }

    public virtual async Task LockStockAutoV2Async(
        List<Guid> dpoDetailIds,
        Guid stockCategoryId,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        foreach (var dpoDetailId in dpoDetailIds)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@stockCategoryId", stockCategoryId, DbType.Guid);
            parameters.Add("@pr_DPODetailId", dpoDetailId, DbType.Guid);
            parameters.Add("@userName", userName, DbType.String, size: 50);
            parameters.Add("@userFullName", userFullName, DbType.String, size: 500);
            parameters.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            await connection.ExecuteAsync(
                "usp_DPO_LockStockAutoV2",
                parameters,
                commandType: CommandType.StoredProcedure,
                transaction: await GetDbTransactionAsync()
            );

            var errorMessage = parameters.Get<string>("@errMsg");
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new UserFriendlyException(errorMessage);
            }
        }
    }


    public virtual async Task<List<DPOListPOsModel>> GetListAvailablePOsAsync(
        Guid dpoDetailId,
        string? materialCode,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@dpoDetailId", dpoDetailId, DbType.Guid);
        parameters.Add("@materialCode", materialCode, DbType.String, size: 50);

        var result = await connection.QueryAsync<DPOListPOsModel>(
            "usp_DPO_GetListPOs",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        return result.ToList();
    }



    public virtual async Task<List<DPOLockStockEtaEtdModel>> GetListLockStockEtaEtdAsync(
        Guid dpoDetailId,
        Guid poDetailId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@dpodeailId", dpoDetailId, DbType.Guid);
        parameters.Add("@poDetailId", poDetailId, DbType.Guid);

        var result = await connection.QueryAsync<DPOLockStockEtaEtdModel>(
            "usp_DPO_Lockshipment_ETAETD",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        return result.ToList();
    }

    public virtual async Task<string?> DeleteDPOLockStockAsync(Guid dpoDetailId, Guid lockStockId, string userName, string userFullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@@materialLockStockId", lockStockId, DbType.Guid);

        parameters.Add("@UserName", userName, DbType.String, size: 50);
        parameters.Add("@userFullName", userFullName, DbType.String, size: 500);
        parameters.Add("@errMsg", dbType: DbType.String, size: -1, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_DPO_LockStock_Delete",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );
        return parameters.Get<string>("@errMsg");
    }

    public virtual async Task<string?> DeleteLockOnOrderStockAsync(Guid poDetailId, Guid dpoDetailId, string userName, string userFullName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@prPODetailId", poDetailId, DbType.Guid);
        parameters.Add("@prDPODetailId", dpoDetailId, DbType.Guid);
        parameters.Add("@userName", userName, DbType.String, size: 50);
        parameters.Add("@userFullName", userFullName, DbType.String, size: 500);
        parameters.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

        await connection.ExecuteAsync(
            "usp_DPO_Detail_DeleteLockOnOrderStock",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        return parameters.Get<string>("@errMsg");
    }

    public virtual async Task<string?> UpdateLockStockAsync(
        Guid dpoDetailId,
        string golfaCode,
        Guid stockCategoryId,
        int lockQty,
        string? note,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@dpoDetailId", dpoDetailId, DbType.Guid);
        parameters.Add("@golfaCode", golfaCode, DbType.String, size: 50);
        parameters.Add("@stockCategoryId", stockCategoryId, DbType.Guid);
        parameters.Add("@lockQty", lockQty, DbType.Int32);
        parameters.Add("@note", note, DbType.String, size: 500);
        parameters.Add("@userName", userName, DbType.String, size: 50);
        parameters.Add("@userFullName", userFullName, DbType.String, size: 500);
        parameters.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

        await connection.ExecuteAsync(
            "usp_DPO_LockStock",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        return parameters.Get<string>("@errMsg");
    }

    // GIC-specific methods
    public virtual async Task<long> GetGICCountAsync(
        GICFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilterGIC((await GetDbSetAsync()), filterParams);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    // GIC-specific methods
    //public virtual async Task<long> GetGKRCountAsync(
    //    GKRFilterParams filterParams,
    //    CancellationToken cancellationToken = default)
    //{
    //    var dbContext = await GetDbContextAsync();
    //    var materials = dbContext.Set<Material>();
    //    //var specials = dbContext.Set<SpecialInputPrice>();
    //    var priceOffers = dbContext.Set<PriceOffer>();
    //    //var purchaseOrderLockShipments = dbContext.Set<PurchaseOrderLockShipment>();

    //    var query = ApplyFilterGKR(
    //        (await GetQueryableAsync()),
    //        filterParams,
    //        materials,
    //        priceOffers
    //    );
    //    return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    //}

    public virtual async Task<List<DPO>> GetListPendingAsync(
        GKRFilterParams filterParams,
        string approverUsername,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var materials = dbContext.Set<Material>();
        var priceOffers = dbContext.Set<PriceOffer>();
        var query = ApplyFilterGKR(
            (await GetQueryableAsync()),
            filterParams,
            materials,
            priceOffers
        );

        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DPOConsts.GetDefaultSorting(false) : sorting);
        query = query.Where(x => x.CurrentApprovalRouteInstanceId != null);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }


    public virtual async Task<List<StatusCount>> GetGroupedCountAsync(
        DPOFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var materials = dbContext.Set<Material>();
       
        var priceOffers = dbContext.Set<PriceOffer>();
       

        var query = ApplyFilter(await GetQueryableAsync(), filterParams, materials, priceOffers);
        var result = await query
            .GroupBy(x => x.Status)
            .Select(g => new StatusCount(g.Key!, g.LongCount()))
            .ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }

    public virtual async Task<List<StatusCount>> GetGICGroupedCountAsync(
        GICFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilterGIC(await GetQueryableAsync(), filterParams);
        var result = await query
            .GroupBy(x => x.Status)
            .Select(g => new StatusCount(g.Key!, g.LongCount()))
            .ToListAsync();

        return result;
    }


    //public virtual async Task<List<StatusCount>> GetGKRGroupedCountAsync(
    //    GKRFilterParams filterParams,
    //    CancellationToken cancellationToken = default)
    //{
    //    var dbContext = await GetDbContextAsync();
    //    var materials = dbContext.Set<Material>();
    //    var specials = dbContext.Set<SpecialInputPrice>();
    //    var priceOffers = dbContext.Set<PriceOffer>();
    //    var purchaseOrderLockShipments = dbContext.Set<PurchaseOrderLockShipment>();

    //    var query = ApplyFilterGKR(
    //        (await GetQueryableAsync()),
    //        filterParams,
    //        materials,
    //        specials,
    //        priceOffers,
    //        purchaseOrderLockShipments
    //    );
    //    var result = await query
    //        .GroupBy(x => x.Status)
    //        .Select(g => new StatusCount(g.Key!, g.LongCount()))
    //        .ToListAsync(cancellationToken: cancellationToken);

    //    return result;
    //}

    public async Task<List<DPOReportDto>> GetListDPOReportAsync(Guid? buyerTypeId, Guid? buyerId, DateTime fromDate, DateTime toDate, bool hasFullBuyerAccess, string userName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var result = await connection.QueryAsync<DPOReportDto>(
            "usp_Report_DPO_R25",
            new
            {
                BuyerTypeId = buyerTypeId,
                BuyerId = buyerId,
                FromDate = fromDate,
                ToDate = toDate,
                HasFullBuyerAccess = hasFullBuyerAccess,
                UserName = userName
            },
            commandType: CommandType.StoredProcedure,
            commandTimeout: 180
        );

        return result.ToList();

    }
    public async Task<List<DPOProcessingReport>> GetListDPOProcessingReportAsync(Guid? buyerTypeId, Guid? buyerId, DateTime fromDate, DateTime toDate, bool hasFullBuyerAccess, string userName)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var result = await connection.QueryAsync<DPOProcessingReport>(
            "usp_Report_DPO_R24",
            new
            {
                buyerTypeId = buyerTypeId,
                buyerId = buyerId,
                fromdate = fromDate,
                todate = toDate,
                hasFullBuyerAccess = hasFullBuyerAccess,
                userName = userName
            },
            commandType: CommandType.StoredProcedure,
            commandTimeout: 180
        );

        return result.ToList();


    }


    public virtual async Task<List<DPOMessage>> GetListMessagesAsync(Guid dpoId, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        var items = await query
            .Where(d => d.Id == dpoId)
            .SelectMany(x => x.Messages)
            .OrderByDescending(x => x.CreationTime)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);

        items.Reverse();

        return items;
    }

    public virtual async Task<long> GetCountMessagesAsync(Guid dpoId, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet
            .Where(d => d.Id == dpoId)
            .SelectMany(d => d.Messages);

        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<DPO> GetWithDetailsAsync(Guid id, System.Linq.Expressions.Expression<System.Func<DPO, object>>? include = null, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        var query = dbSet.Where(x => x.Id == id);

        if (include != null)
        {
            query = query.Include(include);
        }

        var dpo = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        if (dpo == null)
        {
            throw new EntityNotFoundException(typeof(DPO), id);
        }

        return dpo;
    }

    public virtual async Task<DPO> GetWithDetailsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();

        var dpo = await dbSet
            .Include(x => x.Details)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));

        if (dpo == null)
        {
            throw new EntityNotFoundException(typeof(DPO), id);
        }

        return dpo;
    }

    public virtual async Task<List<DPOExportDataModel>> GetExportDataAsync(
        string? dpoNo,
        string? status,
        string? golfaCode,
        string? model,
        string? poNo,
        string? customerName,
        DateTime? fromDate,
        DateTime? toDate,
        string? buyerTypeId,
        string? buyerId,
        string? materialType,
        string? supplierCode,
        string? spoCode,
        string? taxCode,
        string? materialGroup,
        string? userName,
        bool fullBuyerPermission,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@dpono", dpoNo, DbType.String, size: 50);
        parameters.Add("@status", status, DbType.String, size: 50);
        parameters.Add("@golfacode", golfaCode, DbType.String, size: 50);
        parameters.Add("@model", model, DbType.String, size: 255);
        parameters.Add("@poNo", poNo, DbType.String, size: 50);
        parameters.Add("@customerName", customerName, DbType.String, size: 255);
        parameters.Add("@fromdate", fromDate, DbType.Date);
        parameters.Add("@todate", toDate, DbType.Date);
        parameters.Add("@buyerTypeId", buyerTypeId, DbType.String, size: 50);
        parameters.Add("@buyerId", buyerId, DbType.String, size: 50);
        parameters.Add("@materialtype", materialType, DbType.String, size: 50);
        parameters.Add("@supplierCode", supplierCode, DbType.String, size: 50);
        parameters.Add("@spoCode", spoCode, DbType.String, size: 50);
        parameters.Add("@taxCode", taxCode, DbType.String, size: 50);
        parameters.Add("@materialGroup", materialGroup, DbType.String, size: 50);
        parameters.Add("@userName", userName, DbType.String, size: 50);
        parameters.Add("@fullBuyerPermission", fullBuyerPermission, DbType.Boolean);

        var result = await connection.QueryAsync<DPOExportDataModel>(
            "usp_DPO_ExportData",
            parameters,
            commandType: CommandType.StoredProcedure,
            commandTimeout: 180
        );

        return result.ToList();
    }
    public virtual async Task<string?> UpdateDPODetailAsync(
        Guid dpoDetailId,
        int qty,
        string? confirmNote,
        string? note,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@pr_DPODetailId", dpoDetailId, DbType.Guid);
        parameters.Add("@qty", qty, DbType.Int32);
        parameters.Add("@note", note, DbType.String, size: 4000);
        parameters.Add("@confirmNote", confirmNote, DbType.String, size: 4000);
        parameters.Add("@UserName", userName, DbType.String, size: 50);
        parameters.Add("@userFullName", userFullName, DbType.String, size: 500);
        parameters.Add("@errorMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

        await connection.ExecuteAsync(
            "usp_DPO_Detail_Update",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        return parameters.Get<string>("@errorMsg");
    }

    public virtual async Task<string?> UpdateDPODetailGKRAsync(
        Guid dpoDetailId,
        int qty,
        string? confirmNote,
        string? note,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@pr_DPODetailId", dpoDetailId, DbType.Guid);
        parameters.Add("@note", note, DbType.String, size: 4000);
        parameters.Add("@confirmNote", confirmNote, DbType.String, size: 4000);
        parameters.Add("@UserName", userName, DbType.String, size: 50);
        parameters.Add("@userFullName", userFullName, DbType.String, size: 500);
        parameters.Add("@errorMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

        await connection.ExecuteAsync(
            "usp_DPO_Detail_Update_GKR",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        return parameters.Get<string>("@errorMsg");
    }

    public virtual async Task<string?> AllocateGkrToDpoAsync(Guid dpoId, Guid gkrId, string? note, string userName, string userFullName)
    {
        var dbContext = await GetDbContextAsync();
        var connectionString = await _connectionStringResolver.ResolveAsync();

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@pr_dpoId", dpoId, DbType.Guid);
        parameters.Add("@pr_gkrId", gkrId, DbType.Guid);
        parameters.Add("@pr_note", note, DbType.String, size: 4000);
        parameters.Add("@pr_userName", userName, DbType.String, size: 50);
        parameters.Add("@pr_userFullName", userFullName, DbType.String, size: 500);
        parameters.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);
        await connection.ExecuteAsync(
            "usp_DPO_Allocate_GKR",
            parameters,
            commandType: CommandType.StoredProcedure
        //transaction: null //await GetDbTransactionAsync()
        );
        return parameters.Get<string>("@errMsg");
    }

    public virtual Task GenerateApprovalRouteAsync(Guid gkrId) => Task.CompletedTask;

    public Task<List<GKRApprovalRoute>> GetListApprovalRoutesAsync(Guid gkrId)
        => Task.FromResult(new List<GKRApprovalRoute>());

    public virtual async Task<string> GenerateGICNoAsync(
        string materialType,
        string gicTypeCode,
        string? gicProcessCode = null,
        string? buyerShortName = null,
        CancellationToken cancellationToken = default)
    {
        /*
        EXEC GenerateGICNo
            @GICTypeCode = 'GIC-Warranty',
            @MaterialType = 'PHONE',
            @GICProcessCode = 'GIC-Warranty-ProductExchange',
            @BuyerShortName = 'JohnDoe';
        GO
         */

        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("GICTypeCode", gicTypeCode);
        parameters.Add("MaterialType", materialType);
        parameters.Add("GICProcessCode", gicProcessCode ?? string.Empty);
        parameters.Add("BuyerShortName", buyerShortName ?? string.Empty);
        var gicNo = await connection.QueryFirstAsync<string>(
            "usp_GIC_GenerateGICNo",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );
        return gicNo;
    }

    protected virtual IQueryable<DPO> ApplyFilterGIC(
        IQueryable<DPO> query,
        GICFilterParams filterParams)
    {
        var filterText = filterParams.FilterText;
        var gicNo = filterParams.GicNo;
        var materialCode = filterParams.MaterialCode;
        var modelName = filterParams.ModelName;
        var materialType = filterParams.MaterialType;
        var costCenter = filterParams.CostCenter;
        var status = filterParams.Status;
        var buyerTypeId = filterParams.BuyerTypeId;
        var buyerId = filterParams.BuyerId;
        var buyerShortName = filterParams.BuyerShortName;
        var orderDateMin = filterParams.OrderDateMin;
        var orderDateMax = filterParams.OrderDateMax;
        var totalAmountMin = filterParams.TotalAmountMin;
        var totalAmountMax = filterParams.TotalAmountMax;
        var remark = filterParams.Remark;
        var fileName = filterParams.FileName;

        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.DPONo!.Contains(filterText!) || e.DPOType!.Contains(filterText!) || e.MaterialType!.Contains(filterText!) || e.CostCenter!.Contains(filterText!) || e.Status!.Contains(filterText!) || e.BuyerShortName!.Contains(filterText!) || e.Remark!.Contains(filterText!) || e.FileName!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(gicNo), QueryFilterHelper.BuildMultiFieldSearch<DPO>(gicNo, e => e.DPONo))
                .WhereIf(!string.IsNullOrWhiteSpace(materialCode), QueryFilterHelper.BuildNestedCollectionSearch<DPO, DPODetail>(materialCode, e => e.Details, d => d.GolfaCode))
                .WhereIf(!string.IsNullOrWhiteSpace(modelName), QueryFilterHelper.BuildNestedCollectionSearch<DPO, DPODetail>(modelName, e => e.Details, d => d.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(materialType), e => e.MaterialType.Contains(materialType))
                .WhereIf(!string.IsNullOrWhiteSpace(costCenter), QueryFilterHelper.BuildMultiFieldSearch<DPO>(costCenter, e => e.CostCenter))
                .WhereIf(!string.IsNullOrWhiteSpace(status), e => e.Status.Contains(status))
                .WhereIf(buyerTypeId.HasValue, e => e.BuyerTypeId == buyerTypeId)
                .WhereIf(buyerId.HasValue, e => e.BuyerId == buyerId)
                .WhereIf(!string.IsNullOrWhiteSpace(buyerShortName), e => e.BuyerShortName.Contains(buyerShortName))
                .WhereIf(orderDateMin.HasValue, e => e.OrderDate!.Value.Date >= orderDateMin!.Value.Date)
                .WhereIf(orderDateMax.HasValue, e => e.OrderDate!.Value.Date <= orderDateMax!.Value.Date)
                .WhereIf(totalAmountMin.HasValue, e => e.TotalAmount >= totalAmountMin!.Value)
                .WhereIf(totalAmountMax.HasValue, e => e.TotalAmount <= totalAmountMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(remark), e => e.Remark.Contains(remark))
                .WhereIf(!string.IsNullOrWhiteSpace(fileName), e => e.FileName.Contains(fileName))
                .Where(x => !string.IsNullOrWhiteSpace(x.DPONo))
                .Where(x => x.DPOType == DPOTypes.GIC);
    }

    protected virtual IQueryable<DPO> ApplyFilterGKR(
        IQueryable<DPO> query,
        GKRFilterParams filterParams,
        IQueryable<Material> materials,
    IQueryable<PriceOffer> priceOffers)
    {
        var gkrNo = filterParams.GKRNo;
        var materialCode = filterParams.MaterialCode;
        var modelName = filterParams.ModelName;
        var customerName = filterParams.CustomerName;
        var customerTaxCode = filterParams.CustomerTaxCode;
        var poNo = filterParams.PONo;
        var supplierCode = filterParams.SupplierCode;
        var materialGroup = filterParams.MaterialGroup;
        var specialPriceCode = filterParams.SpecialPriceCode;
        var materialType = filterParams.MaterialType;
        var costCenter = filterParams.CostCenter;
        var status = filterParams.Status;
        var buyerTypeId = filterParams.BuyerTypeId;
        var buyerId = filterParams.BuyerId;
        var buyerShortName = filterParams.BuyerShortName;
        var orderDateMin = filterParams.OrderDateMin;
        var orderDateMax = filterParams.OrderDateMax;
        var totalAmountMin = filterParams.TotalAmountMin;
        var totalAmountMax = filterParams.TotalAmountMax;

        query = query
            .WhereIf(!string.IsNullOrWhiteSpace(gkrNo), QueryFilterHelper.BuildMultiFieldSearch<DPO>(gkrNo, e => e.DPONo))
            .WhereIf(!string.IsNullOrWhiteSpace(materialCode), e => e.Details.Any(d => d.GolfaCode.Contains(materialCode)))
            .WhereIf(!string.IsNullOrWhiteSpace(modelName), QueryFilterHelper.BuildNestedCollectionSearch<DPO, DPODetail>(modelName, e => e.Details, d => d.Model))
            .WhereIf(!string.IsNullOrWhiteSpace(customerName), QueryFilterHelper.BuildNestedCollectionSearch<DPO, DPODetail>(customerName, e => e.Details, d => d.CustomerName))
            .WhereIf(!string.IsNullOrWhiteSpace(customerTaxCode), e => e.Details.Any(d => d.CustomerTaxCode.Contains(customerTaxCode)))
            .WhereIf(!string.IsNullOrWhiteSpace(materialType), e => e.MaterialType.Contains(materialType))
            .WhereIf(!string.IsNullOrWhiteSpace(costCenter), QueryFilterHelper.BuildMultiFieldSearch<DPO>(costCenter, e => e.CostCenter))
            .WhereIf(!string.IsNullOrWhiteSpace(status), e => e.Status.Contains(status))
            .WhereIf(buyerTypeId.HasValue, e => e.BuyerTypeId == buyerTypeId)
            .WhereIf(buyerId.HasValue, e => e.BuyerId == buyerId)
            .WhereIf(!string.IsNullOrWhiteSpace(buyerShortName), e => e.BuyerShortName.Contains(buyerShortName))
            .WhereIf(orderDateMin.HasValue, e => e.OrderDate!.Value.Date >= orderDateMin!.Value.Date)
            .WhereIf(orderDateMax.HasValue, e => e.OrderDate!.Value.Date <= orderDateMax!.Value.Date)
            .WhereIf(totalAmountMin.HasValue, e => e.TotalAmount >= totalAmountMin.Value)
            .WhereIf(totalAmountMax.HasValue, e => e.TotalAmount <= totalAmountMax.Value)
            .Where(x => !string.IsNullOrWhiteSpace(x.DPONo))
            .Where(x => x.DPOType == DPOTypes.GKR);

        // Supplier filter via Material table
        if (!string.IsNullOrWhiteSpace(supplierCode) && materials is not null)
        {
            query = query.Where(e =>
                e.Details.Any(d =>
                    materials.Any(m =>
                        m.GolfaCode == d.GolfaCode &&
                        m.SupplierCode == supplierCode)));
        }

        // Material Group filter
        if (!string.IsNullOrWhiteSpace(materialGroup) && materials is not null)
        {
            query = query.Where(e =>
                e.Details.Any(d =>
                    materials.Any(m =>
                        m.GolfaCode == d.GolfaCode &&
                        m.Material_Group == materialGroup)));
        }

        // Special Price filter
        if (!string.IsNullOrWhiteSpace(specialPriceCode) && priceOffers is not null)
        {
            query = query.Where(e =>
                e.Details.Any(d =>
                    priceOffers.Any(po =>
                        po.Id == d.SPOId &&
                        po.PriceOfferCode == specialPriceCode)));
        }

        // PONo filter
        //if (!string.IsNullOrWhiteSpace(poNo) && purchaseOrderLockShipment is not null)
        //{
        //    query = query.Where(e =>
        //        e.Details.Any(d =>
        //            purchaseOrderLockShipment.Any(pol =>
        //                pol.DPODetailId == d.Id &&
        //                pol.PONo != null &&
        //                pol.PONo.Contains(poNo))));
        //}

        return query;
    }

    public async Task<List<GICInternalUseModel>> GetListGICInternalUseAsync(
       GICFilterParams filterParams,
       CancellationToken cancellationToken = default
    )
    {

        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();

        var list = await connection.QueryAsync<GICInternalUseModel>(
            "N/A",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );
        return list.ToList();
    }
    public async Task<List<GICFOCModel>> GetListGICFOCAsync(
        GICFilterParams filterParams,
        CancellationToken cancellationToken = default
    )
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();

        var list = await connection.QueryAsync<GICFOCModel>(
            "N/A",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );
        return list.ToList();
    }

    public async Task<List<GICWarrantyModel>> GetListGICWarrantyAsync(
        GICFilterParams filterParams,
        CancellationToken cancellationToken = default
    )
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();

        var list = await connection.QueryAsync<GICWarrantyModel>(
            "N/A",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );
        return list.ToList();
    }

    public virtual async Task<List<GKRExportModel>> GetExportGKRDataAsync(
       string? dpoNo,
       string? status,
       string? golfaCode,
       string? model,
       string? poNo,
       string? customerName,
       DateTime? fromDate,
       DateTime? toDate,
       string? buyerTypeId,
       string? buyerId,
       string? materialType,
       string? supplierCode,
       string? spoCode,
       string? taxCode,
       string? materialGroup,
       string? userName,
       bool fullBuyerPermission,
       CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@gkrno", dpoNo, DbType.String, size: 50);
        parameters.Add("@status", status, DbType.String, size: 50);
        parameters.Add("@golfacode", golfaCode, DbType.String, size: 50);
        parameters.Add("@model", model, DbType.String, size: 255);
        parameters.Add("@poNo", poNo, DbType.String, size: 50);
        parameters.Add("@customerName", customerName, DbType.String, size: 255);
        parameters.Add("@fromdate", fromDate, DbType.Date);
        parameters.Add("@todate", toDate, DbType.Date);
        parameters.Add("@buyerTypeId", buyerTypeId, DbType.String, size: 50);
        parameters.Add("@buyerId", buyerId, DbType.String, size: 50);
        parameters.Add("@materialtype", materialType, DbType.String, size: 50);
        parameters.Add("@supplierCode", supplierCode, DbType.String, size: 50);

        parameters.Add("@taxCode", taxCode, DbType.String, size: 50);
        parameters.Add("@materialGroup", materialGroup, DbType.String, size: 50);
        parameters.Add("@userName", userName, DbType.String, size: 50);
        parameters.Add("@fullBuyerPermission", fullBuyerPermission, DbType.Boolean);

        var result = await connection.QueryAsync<GKRExportModel>(
            "usp_DPO_ExportData_GKR",
            parameters,
            commandType: CommandType.StoredProcedure,
            commandTimeout: 180
        );

        return result.ToList();
    }

    public virtual async Task<List<GICExportModel>> GetExportGICDataAsync(
    string? gicNo,
    string? status,
    string? golfaCode,
    string? modelName,
    string? costCenter,
    string? gicType,
    string? gicProcess,
    DateTime? fromDate,
    DateTime? toDate,
    string? buyerTypeId,
    string? buyerId,
    string? materialType,
    string? userName,
    bool fullBuyerPermission,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@gicNo", gicNo, DbType.String, size: 50);
        parameters.Add("@status", status, DbType.String, size: 50);
        parameters.Add("@golfaCode", golfaCode, DbType.String, size: 50);
        parameters.Add("@model", modelName, DbType.String, size: 100);
        parameters.Add("@costCenter", costCenter, DbType.String, size: 50);
        parameters.Add("@gicType", gicType, DbType.String, size: 50);
        parameters.Add("@gicProcess", gicProcess, DbType.String, size: 1000);
        parameters.Add("@fromDate", fromDate, DbType.Date);
        parameters.Add("@toDate", toDate, DbType.Date);
        parameters.Add("@buyerType", buyerTypeId, DbType.String, size: 50);
        parameters.Add("@buyerId", buyerId, DbType.String, size: 50);
        parameters.Add("@materialType", materialType, DbType.String, size: 50);
        parameters.Add("@userName", userName, DbType.String, size: 50);
        parameters.Add("@fullBuyerPermission", fullBuyerPermission, DbType.Boolean);

        var result = await connection.QueryAsync<GICExportModel>(
            "usp_DPO_ExportData_GIC",
            parameters,
            commandType: CommandType.StoredProcedure,
            commandTimeout: 180
        );

        return result.ToList();
    }

    public Task<List<DPO>> GetListGKRAsync(GKRFilterParams filterParams, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
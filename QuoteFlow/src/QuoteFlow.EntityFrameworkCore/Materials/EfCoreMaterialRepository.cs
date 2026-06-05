using Dapper;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Extensions;
using QuoteFlow.Helper;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.StockManagements;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Materials;

public class EfCoreMaterialRepository : EfCoreRepository<QuoteFlowDbContext, Material, Guid>, IMaterialRepository
{
    public EfCoreMaterialRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<Material>> GetListAsync(
        MaterialFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query.Include(x => x.MaterialStock).ThenInclude(y => y.StockCategory);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting) ? MaterialConsts.GetDefaultSorting(false) : filterParams.Sorting);
        return await query.PageBy(filterParams.SkipCount, filterParams.MaxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<Material> GetWithDetailAsync(string golfaCode, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        var result = await query
            .Where(x => x.GolfaCode == golfaCode)
            .FirstOrDefaultNoLockAsync(dbContext, GetCancellationToken(cancellationToken))
            ?? throw new EntityNotFoundException(typeof(Material), golfaCode);

        return result;
    }

    public virtual async Task<List<T>> GetListAsync<T>(
        MaterialFilterParams filterParams,
        Expression<Func<Material, T>> selector,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);

        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? MaterialConsts.GetDefaultSorting(false)
            : filterParams.Sorting);
        var resultQuery = query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .Select(selector).AsQueryable();

        var result = await resultQuery.ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }
    public virtual async Task<List<T>> GetListWithDeactiveAsync<T>(
        MaterialFilterParams filterParams,
        Expression<Func<Material, T>> selector,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilterWithDeactive((await GetQueryableAsync()), filterParams);

        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? MaterialConsts.GetDefaultSorting(false)
            : filterParams.Sorting);
        var resultQuery = query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .Select(selector).AsQueryable();

        var result = await resultQuery.ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }

    public virtual async Task<long> GetCountAsync(
        MaterialFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Material> ApplyFilter(
    IQueryable<Material> query,
    MaterialFilterParams filterParams)
    {
        var golfaCodes = filterParams.GolfaCodes;
        var models = filterParams.Models;
        var sapCode = filterParams.SAPCode;
        var materialType = filterParams.MaterialType;
        var status = filterParams.MaterialStatus;

        return query
            .WhereIf(!string.IsNullOrEmpty(golfaCodes), QueryFilterHelper.BuildMultiFieldSearch<Material>(golfaCodes!, e => e.GolfaCode))
            .WhereIf(!string.IsNullOrEmpty(models), QueryFilterHelper.BuildMultiFieldSearch<Material>(models!, e => e.Model))
            .WhereIf(!string.IsNullOrWhiteSpace(sapCode), e => (e.SAP_Code ?? "").Contains(sapCode!))
            .WhereIf(!string.IsNullOrWhiteSpace(status), e => e.MaterialStatus == status)
            .WhereIf(string.IsNullOrWhiteSpace(status), e => e.MaterialStatus != MaterialStatuses.Deactivated)
            .ApplyMaterialTypeFilter(filterParams, e => e.MaterialType)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.MaterialGroup), e => e.Material_Group == filterParams.MaterialGroup)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.SupplierBU), e => e.SupplierBUCode == filterParams.SupplierBU)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Supplier), e => e.SupplierCode == filterParams.Supplier);
    }
    protected virtual IQueryable<Material> ApplyFilterWithDeactive(
    IQueryable<Material> query,
    MaterialFilterParams filterParams)
    {
        var golfaCodes = filterParams.GolfaCodes;
        var models = filterParams.Models;
        var sapCode = filterParams.SAPCode;
        var materialType = filterParams.MaterialType;
        var status = filterParams.MaterialStatus;

        return query
            .WhereIf(!string.IsNullOrEmpty(golfaCodes), QueryFilterHelper.BuildMultiFieldSearch<Material>(golfaCodes!, e => e.GolfaCode))
            .WhereIf(!string.IsNullOrEmpty(models), QueryFilterHelper.BuildMultiFieldSearch<Material>(models!, e => e.Model))
            .WhereIf(!string.IsNullOrWhiteSpace(sapCode), e => (e.SAP_Code ?? "").Contains(sapCode!))
            .WhereIf(!string.IsNullOrWhiteSpace(status), e => e.MaterialStatus == status)
            //.WhereIf(string.IsNullOrWhiteSpace(status), e => e.MaterialStatus != MaterialStatuses.Deactivated)
            //.WhereIf(!string.IsNullOrWhiteSpace(materialType), e => (e.MaterialType ?? "").Contains(materialType!))
            .ApplyMaterialTypeFilter(filterParams, e => e.MaterialType)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.MaterialGroup), e => e.Material_Group == filterParams.MaterialGroup)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.SupplierBU), e => e.SupplierBUCode == filterParams.SupplierBU)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Supplier), e => e.SupplierCode == filterParams.Supplier);
    }

    public async Task<List<StockManagementList>> GetStockManagementListAsync(
    StockManagementFilterParams filterParams,
    bool? exportExcel = null,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var restrictedMaterialTypes = StoredProcedureParameterHelper.ListToStoredProcString(
                filterParams.RestrictedMaterialTypes);

        var parameters = new
        {
            supplierCode = filterParams.SupplierCode,
            supplierBUCode = filterParams.SupplierBUCode,
            materialType = filterParams.MaterialType,
            golfacode = filterParams.GolfaCode,
            model = filterParams.Model,
            materialGroup = filterParams.MaterialGroup,
            stockCategoryId = filterParams.StockCategoryId,
            greaterStockQty = filterParams.GreaterStockQty,
            greaterOnOrderStockQty = filterParams.GreaterOnOrderStockQty,
            exportExcel = exportExcel,
            status = filterParams.Status,
            restrictedMaterialTypes,
        };

        var result = await connection.QueryAsync<StockManagementList>(
            "[dbo].[usp_StockManagement_GetList]",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result.ToList();
    }

    public async Task<List<StockQty>> StockQtyAsync(string? materialCode, Guid? stockId)
    {
        var dbContext = await GetDbContextAsync();

        var query =
            from ms in dbContext.MaterialStocks
            join s in dbContext.StockCategories
                on ms.StockCategoryId equals s.Id
            where ms.GolfaCode == materialCode
                  && (stockId == null || ms.StockCategoryId == stockId)
            //&& (s.FOC == false || s.DamagedStock == null)
            //&& (s.DamagedStock == false || s.DamagedStock == null)
            orderby ms.CreationTime descending
            select new StockQty
            {
                StockCode = s.StockCode,
                StockName = s.StockName,
                GolfaCode = ms.GolfaCode,
                Qty = ms.Qty,
                AvailableStock = (ms.Qty ?? 0) - (ms.Locked ?? 0) - (ms.LockStockKeeping ?? 0),
                Created = ms.CreationTime,
                CreatedBy = ms.CreatorName,
                Modified = ms.LastModificationTime,
                ModifiedBy = ms.LastModifierName
            };

        return await query.ToListAsync();
    }

    public async Task<List<Locked>> LockedAsync(string materialCode)
    {
        var dbContext = await GetDbContextAsync();

        var result = await (
            from mls in dbContext.MaterialStockLockStocks
            join dpod in dbContext.DPODetails
                on mls.DPODetailId equals dpod.Id
            join dpo in dbContext.DPOs
                on dpod.DPOId equals dpo.Id
            join s in dbContext.StockCategories
                on mls.StockCategoryId equals s.Id
            where
                mls.GolfaCode == materialCode &&
                dpod.Status == "IN_PROGRESS" &&
                (dpod.IsDeleted == false || dpod.IsDeleted == null) &&
                dpo.Status != "CLOSED" &&
                (dpo.IsDeleted == false || dpo.IsDeleted == null) &&
                (s.DamagedStock == false || s.DamagedStock == null) &&
                (s.FOC == false || s.FOC == null) &&
                (mls.ReleasedLock == 0 || mls.ReleasedLock == null)
            orderby mls.CreationTime descending

            select new Locked
            {
                DPONo = dpo.DPONo,
                BuyerShortName = dpo.BuyerShortName,
                StockCategoryId = s.Id,
                StockName = s.StockName,
                LockedQty = mls.Qty,
                Created = mls.CreationTime,
                CreatedBy = mls.CreatorName,
                Modified = mls.LastModificationTime,
                ModifiedBy = mls.LastModifierName
            }
        ).ToListNoLockAsync(dbContext);

        return result;
    }




    public async Task<List<StockOfSO>> StockOfSOAsync(string? materialCode, Guid? stockId)
    {
        var dbContext = await GetDbContextAsync();

        var query =
            from sod in dbContext.SaleOrderDetails
            join so in dbContext.SaleOrders on sod.SaleOrderId equals so.Id
            join s in dbContext.StockCategories on sod.StockCategoryId equals s.Id
            where sod.StatusCode == "IN_PROGRESS"
                  && (so.IsDeleted == false)
                  && (sod.IsDeleted == false)
                  && (stockId == null || sod.StockCategoryId == stockId)
                  && sod.GolfaCode == materialCode
                  && (s.FOC == false || s.DamagedStock == null)
                  && (s.DamagedStock == false || s.DamagedStock == null)
            orderby sod.CreationTime descending
            select new StockOfSO
            {
                SONo = so.SONo,
                StockName = s.StockName,
                Qty = sod.Qty,
                DONo = so.SAPDONo,
                Buyer = so.BuyerCode,
                Created = sod.CreationTime,
                CreatedBy = sod.CreatorName,
                Modified = sod.LastModificationTime,
                ModifiedBy = sod.LastModifierName
            };

        return await query.ToListNoLockAsync(dbContext);
    }

    public async Task<List<LockShipment>> GetLockShipmentAsync(string? materialCode)
    {
        var dbContext = await GetDbContextAsync();

        var query =
            from pols in dbContext.PurchaseOrderLockShipments
            join pod in dbContext.PurchaseOrderDetails
                on pols.PODetailId equals pod.Id
            join po in dbContext.PurchaseOrders
                on pod.PurchaseOrderId equals po.Id
            join dpod in dbContext.DPODetails
                on pols.DPODetailId equals dpod.Id
            join dpo in dbContext.DPOs
                on dpod.DPOId equals dpo.Id

            where pols.MaterialCode == materialCode
                && (pols.QtyNeed ?? 0) > 0
                && (pols.IsDeleted == false || pols.IsDeleted == null)
                && pod.StatusCode == "IN_PROGRESS" && (pod.IsDeleted == false || pod.IsDeleted == null)
                && po.StatusCode == "IN_PROGRESS" && (po.IsDeleted == false || po.IsDeleted == null)
                && dpod.Status == "IN_PROGRESS" && (dpod.IsDeleted == false || dpod.IsDeleted == null)
                && dpo.Status == "IN_PROGRESS" && (dpo.IsDeleted == false || dpo.IsDeleted == null)
            orderby pols.CreationTime descending

            select new LockShipment
            {
                GolfaCode = pod.GolfaCode,
                Qty = pols.Qty,
                QtyDisposed = pols.QtyDisposed,
                QtyNeed = pols.QtyNeed,
                PONo = po.PONo,
                DPONo = dpo.DPONo,
                MachineNo = pod.MachineNumber,
                SupplierReply = pod.STCReply,
                MEVNAddRequest = pod.MEVNAddedRequest,
                MEVNRequest = pod.MEVNRequest,
                Modified = pod.LastModificationTime,
                ModifiedBy = pod.LastModifierUsername
            };

        return await query.ToListNoLockAsync(dbContext);
    }


    public async Task<List<OnOrderStock>> OnOrderStockAsync(string? materialCode)
    {
        var dbContext = await GetDbContextAsync();

        var query =
            from pod in dbContext.PurchaseOrderDetails
            join po in dbContext.PurchaseOrders
                on pod.PurchaseOrderId equals po.Id
            where !(pod.IsDeleted ?? false)
                && !(po.IsDeleted ?? false)
                && pod.GolfaCode == materialCode
                && pod.StatusCode == "IN_PROGRESS"
                && (pod.QtyAvailable ?? 0) > 0
            orderby pod.CreationTime descending
            select new OnOrderStock
            {
                PONo = po.PONo,
                GolfaCode = pod.GolfaCode,
                PODate = po.PODate,
                QtyAvailable = pod.QtyAvailable,
                MachineNo = pod.MachineNumber,
                SupplierReply = pod.STCReply,
                MEVNAddRequest = pod.MEVNAddedRequest,
                MEVNRequest = pod.MEVNRequest,
            };

        return await query.ToListNoLockAsync(dbContext);
    }


    public async Task<List<MaterialOverallStockReport>> GetListStockOverallAsync()
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var result = await connection.QueryAsync<MaterialOverallStockReport>(
            "usp_Report_StockOverall_R21",
            commandType: CommandType.StoredProcedure,
            commandTimeout: 0
        );

        return result.ToList();
    }

    public async Task<string?> ValidationTransferAsync(
        Guid requestId,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("requestId", requestId);
        parameters.Add("userName", userName);
        parameters.Add("userFullName", userFullName);
        parameters.Add("@errorMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

        var result = await connection.ExecuteAsync(
            "usp_StockManagement_StockTransfer",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        var errorMsg = parameters.Get<string>("errorMsg");
        if (!string.IsNullOrWhiteSpace(errorMsg))
        {
            return errorMsg;
        }

        return null;
    }

    public async Task<string?> ValidationInventoryAsync(
        Guid requestId,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("requestId", requestId);
        parameters.Add("userName", userName);
        parameters.Add("userFullName", userFullName);
        parameters.Add("@errorMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

        var result = await connection.ExecuteAsync(
            "usp_StockManagement_StockInventory",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        var errorMsg = parameters.Get<string>("errorMsg");
        if (!string.IsNullOrWhiteSpace(errorMsg))
        {
            return errorMsg;
        }

        return null;
    }

    public async Task<List<InventoryReport>> GetInventoryReportAsync(
        ExcelInventoryReportFilterParams filterParams)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("materialCode", filterParams.MaterialCode);
        parameters.Add("inventoryCategory", filterParams.InventoryCategory);
        parameters.Add("materialGroup", filterParams.MaterialGroup);


        var result = await connection.QueryAsync<InventoryReport>(
            "usp_Report_Inventory_R15",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync(),
            commandTimeout: 180
        );

        if (filterParams.Export == true)
        {
            return result.ToList();
        }

        return result.Skip(filterParams.SkipCount).Take(filterParams.MaxResultCount).ToList();

    }

    public async Task<long> GetCountInventoryReportAsync(
        ExcelInventoryReportFilterParams filterParams)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("materialCode", filterParams.MaterialCode);
        parameters.Add("inventoryCategory", filterParams.InventoryCategory);
        parameters.Add("materialGroup", filterParams.MaterialGroup);


        var result = await connection.QueryAsync<InventoryReport>(
            "usp_Report_Inventory_R15",
            parameters,
            commandTimeout: 180
        );



        return result.Count();
    }
    public async Task UpdateStatusAsync(Guid materialApprovalId)
    {
        var dbContext = await GetDbContextAsync();

        var parameter = new SqlParameter("@prRequestId", SqlDbType.UniqueIdentifier)
        {
            Value = materialApprovalId
        };

        await dbContext.Database.ExecuteSqlRawAsync(
            "EXEC [dbo].[usp_TimerJob_Materials_UpdateStatus] @prRequestId",
            parameter
        );
    }

    public async Task BulkUpdateFactoryAsync(
        List<ExcelMaterialFactoryUpdateParams> updates,
        CancellationToken cancellationToken = default)
    {
        if (updates == null || updates.Count == 0) return;

        // Get connection string from DbContext and create a NEW independent connection
        // This avoids transaction conflicts with EF Core's managed connection
        var dbContext = await GetDbContextAsync();
        var connectionString = dbContext.Database.GetConnectionString();

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        // Start transaction for atomicity (temp table + update must succeed together)
        using var transaction = connection.BeginTransaction();

        try
        {
            // Create DataTable for bulk copy
            var idField = nameof(ExcelMaterialFactoryUpdateParams.Id);
            var referenceLeadTimeField = nameof(ExcelMaterialFactoryUpdateParams.ReferenceLeadTime);
            var maxlotField = nameof(ExcelMaterialFactoryUpdateParams.Maxlot);
            var countryOfOriginField = nameof(ExcelMaterialFactoryUpdateParams.CountryOfOrigin);
            var concurrencyStampField = nameof(ExcelMaterialFactoryUpdateParams.ConcurrencyStamp);


            var dataTable = new DataTable();
            dataTable.Columns.Add(idField, typeof(Guid));
            dataTable.Columns.Add(referenceLeadTimeField, typeof(int));
            dataTable.Columns.Add(maxlotField, typeof(int));
            dataTable.Columns.Add(countryOfOriginField, typeof(string));
            dataTable.Columns.Add(concurrencyStampField, typeof(string));

            foreach (var update in updates)
            {
                var row = dataTable.NewRow();
                row[idField] = update.Id;
                row[referenceLeadTimeField] = update.ReferenceLeadTime.HasValue ? update.ReferenceLeadTime.Value : DBNull.Value;
                row[maxlotField] = update.Maxlot.HasValue ? update.Maxlot.Value : DBNull.Value;
                row[countryOfOriginField] = update.CountryOfOrigin ?? (object)DBNull.Value;
                row[concurrencyStampField] = update.ConcurrencyStamp ?? (object)DBNull.Value;
                dataTable.Rows.Add(row);
            }

            // Create temp table
            await connection.ExecuteAsync(@"
                CREATE TABLE #TempMaterialFactoryUpdate (
                    Id UNIQUEIDENTIFIER NOT NULL,
                    ReferenceLeadTime INT NULL,
                    Maxlot INT NULL,
                    CountryOfOrigin NVARCHAR(255) NULL,
                    ConcurrencyStamp NVARCHAR(40) NULL
                )", param: null, transaction: transaction, commandTimeout: 120);

            // Bulk copy to temp table (pass transaction)
            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = "#TempMaterialFactoryUpdate";
                bulkCopy.BatchSize = 5000; // Prevents lock escalation
                bulkCopy.BulkCopyTimeout = 300; // 5 minutes

                bulkCopy.ColumnMappings.Add(idField, idField);
                bulkCopy.ColumnMappings.Add(referenceLeadTimeField, referenceLeadTimeField);
                bulkCopy.ColumnMappings.Add(maxlotField, maxlotField);
                bulkCopy.ColumnMappings.Add(countryOfOriginField, countryOfOriginField);
                bulkCopy.ColumnMappings.Add(concurrencyStampField, concurrencyStampField);

                await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);
            }

            // MERGE with ROWLOCK hint to prevent lock escalation
            // -1 means clear the value (set to NULL), NULL means keep existing value
            await connection.ExecuteAsync(@"
                UPDATE m WITH (ROWLOCK)
                SET
                    m.ReferenceLeadTime = CASE
                        WHEN t.ReferenceLeadTime = -1 THEN NULL
                        WHEN t.ReferenceLeadTime IS NOT NULL THEN t.ReferenceLeadTime
                        ELSE m.ReferenceLeadTime
                    END,
                    m.Maxlot = CASE
                        WHEN t.Maxlot = -1 THEN NULL
                        WHEN t.Maxlot IS NOT NULL THEN t.Maxlot
                        ELSE m.Maxlot
                    END,
                    m.CountryOfOrigin = CASE
                        WHEN t.CountryOfOrigin = '-1' THEN NULL
                        WHEN t.CountryOfOrigin IS NOT NULL THEN t.CountryOfOrigin
                        ELSE m.CountryOfOrigin
                    END,
                    m.ConcurrencyStamp = NEWID(),
                    m.LastModificationTime = GETDATE()
                FROM Materials m
                INNER JOIN #TempMaterialFactoryUpdate t ON m.Id = t.Id;

                DROP TABLE #TempMaterialFactoryUpdate;",
                param: null,
                transaction: transaction,
                commandTimeout: 300
            );

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }


    public async Task BulkUpdateMaterialWithOutPriceAsync(
    List<ExcelMaterialUpdateWithoutPrriceParams> updates,
    CancellationToken cancellationToken = default)
    {
        if (updates == null || updates.Count == 0) return;

        var dbContext = await GetDbContextAsync();
        var connectionString = dbContext.Database.GetConnectionString();

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        using var transaction = connection.BeginTransaction();

        try
        {
            var dataTable = new DataTable();
            // ... (Các cột khác giữ nguyên) ...
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Id), typeof(Guid));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Model), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.RegistrationDate), typeof(DateTime));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ValidFrom), typeof(DateTime));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ValidTo), typeof(DateTime));
            // ... (Các cột khác khai báo như cũ) ...
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec1), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec2), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec3), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec4), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Description_EN), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Description_VN), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierCode), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierBUCode), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierBUId), typeof(Guid));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Factory_Text), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.MaterialType), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Unit), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Material_SEC_Classification), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Material_Group), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.MaterialGroup), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.SAPMatGroup), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ProductHierarchyDescription), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.CountryOfOrigin), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ReferenceLeadTime), typeof(int));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.WarrantyTime), typeof(int));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.InventoryCategory), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Maxlot), typeof(int));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.StockWarning), typeof(int));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.StockQty), typeof(int));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.HS_Code), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.CargoNote), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Weight), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Size), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.QRCode), typeof(string));
            dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ConcurrencyStamp), typeof(string));

            foreach (var update in updates)
            {
                var row = dataTable.NewRow();
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Id)] = update.Id ?? Guid.Empty;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Model)] = update.Model ?? (object)DBNull.Value;

                // --- THAY ĐỔI 1: Logic C# ---
                // DateTime.MinValue trong C# là 0001-01-01, DATETIME2 hỗ trợ tốt giá trị này.
                if (update.RegistrationDate.HasValue && update.RegistrationDate.Value == DateTime.MinValue)
                {
                    // Truyền thẳng MinValue xuống DB để làm cờ hiệu (flag)
                    row[nameof(ExcelMaterialUpdateWithoutPrriceParams.RegistrationDate)] = DateTime.MinValue;
                }
                else
                {
                    row[nameof(ExcelMaterialUpdateWithoutPrriceParams.RegistrationDate)] = update.RegistrationDate.HasValue ? update.RegistrationDate.Value : DBNull.Value;
                }

                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ValidFrom)] = update.ValidFrom.HasValue ? update.ValidFrom.Value : DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ValidTo)] = update.ValidTo.HasValue ? update.ValidTo.Value : DBNull.Value;

                // ... (Các dòng map dữ liệu khác giữ nguyên) ...
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec1)] = update.Spec1 ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec2)] = update.Spec2 ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec3)] = update.Spec3 ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec4)] = update.Spec4 ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Description_EN)] = update.Description_EN ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Description_VN)] = update.Description_VN ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierCode)] = update.SupplierCode ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierBUCode)] = update.SupplierBUCode ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierBUId)] = update.SupplierBUId.HasValue ? update.SupplierBUId.Value : DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Factory_Text)] = update.Factory_Text ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.MaterialType)] = update.MaterialType ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Unit)] = update.Unit ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Material_SEC_Classification)] = update.Material_SEC_Classification ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Material_Group)] = update.Material_Group ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.MaterialGroup)] = update.MaterialGroup ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.SAPMatGroup)] = update.SAPMatGroup ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ProductHierarchyDescription)] = update.ProductHierarchyDescription ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.CountryOfOrigin)] = update.CountryOfOrigin ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ReferenceLeadTime)] = update.ReferenceLeadTime.HasValue ? update.ReferenceLeadTime.Value : DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.WarrantyTime)] = update.WarrantyTime.HasValue ? update.WarrantyTime.Value : DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.InventoryCategory)] = update.InventoryCategory ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Maxlot)] = update.Maxlot.HasValue ? update.Maxlot.Value : DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.StockWarning)] = update.StockWarning.HasValue ? update.StockWarning.Value : DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.StockQty)] = update.StockQty.HasValue ? update.StockQty.Value : DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.HS_Code)] = update.HS_Code ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.CargoNote)] = update.CargoNote ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Weight)] = update.Weight ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Size)] = update.Size ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.QRCode)] = update.QRCode ?? (object)DBNull.Value;
                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ConcurrencyStamp)] = update.ConcurrencyStamp ?? (object)DBNull.Value;
                dataTable.Rows.Add(row);
            }

            // --- THAY ĐỔI 2: CREATE TABLE ---
            // Sử dụng DATETIME2(7) thay cho DATETIME
            await connection.ExecuteAsync(@"
        CREATE TABLE #TempMaterialUpdate (
            Id UNIQUEIDENTIFIER NOT NULL,
            Model NVARCHAR(MAX) NULL,
            RegistrationDate DATETIME2(7) NULL,  -- Đổi thành DATETIME2
            ValidFrom DATETIME2(7) NULL,         -- Đổi thành DATETIME2
            ValidTo DATETIME2(7) NULL,           -- Đổi thành DATETIME2
            Spec1 NVARCHAR(MAX) NULL,
            Spec2 NVARCHAR(MAX) NULL,
            Spec3 NVARCHAR(MAX) NULL,
            Spec4 NVARCHAR(MAX) NULL,
            Description_EN NVARCHAR(MAX) NULL,
            Description_VN NVARCHAR(MAX) NULL,
            SupplierCode NVARCHAR(MAX) NULL,
            SupplierBUCode NVARCHAR(MAX) NULL,
            SupplierBUId UNIQUEIDENTIFIER NULL,
            Factory_Text NVARCHAR(MAX) NULL,
            MaterialType NVARCHAR(MAX) NULL,
            Unit NVARCHAR(MAX) NULL,
            Material_SEC_Classification NVARCHAR(MAX) NULL,
            Material_Group NVARCHAR(MAX) NULL,
            MaterialGroup NVARCHAR(MAX) NULL,
            SAPMatGroup NVARCHAR(MAX) NULL,
            ProductHierarchyDescription NVARCHAR(MAX) NULL,
            CountryOfOrigin NVARCHAR(MAX) NULL,
            ReferenceLeadTime INT NULL,
            WarrantyTime INT NULL,
            InventoryCategory NVARCHAR(MAX) NULL,
            Maxlot INT NULL,
            StockWarning INT NULL,
            StockQty INT NULL,
            HS_Code NVARCHAR(MAX) NULL,
            CargoNote NVARCHAR(MAX) NULL,
            Weight NVARCHAR(MAX) NULL,
            Size NVARCHAR(MAX) NULL,
            QRCode NVARCHAR(MAX) NULL,
            ConcurrencyStamp NVARCHAR(40) NULL
        )", param: null, transaction: transaction, commandTimeout: 120);

            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = "#TempMaterialUpdate";
                bulkCopy.BatchSize = 5000;
                bulkCopy.BulkCopyTimeout = 300;

                foreach (DataColumn column in dataTable.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);
            }

            // --- THAY ĐỔI 3: UPDATE Logic ---
            // Kiểm tra với '0001-01-01' thay vì '1753-01-01'
            await connection.ExecuteAsync(@"
        UPDATE m WITH (ROWLOCK)
        SET
            m.Model = CASE WHEN t.Model IS NOT NULL THEN t.Model ELSE m.Model END,
            
            -- RegistrationDate: DateTime.MinValue (0001-01-01) clears it
            m.RegistrationDate = CASE 
                WHEN t.RegistrationDate = '0001-01-01' THEN NULL  -- Logic mới cho MinValue
                WHEN t.RegistrationDate IS NOT NULL THEN t.RegistrationDate 
                ELSE m.RegistrationDate 
            END,
            
            m.ValidFrom = CASE WHEN t.ValidFrom IS NOT NULL THEN t.ValidFrom ELSE m.ValidFrom END,
            m.ValidTo = CASE WHEN t.ValidTo IS NOT NULL THEN t.ValidTo ELSE m.ValidTo END,
            
            -- (Các phần dưới giữ nguyên) --
            m.Spec1 = CASE WHEN t.Spec1 = '-1' THEN NULL WHEN t.Spec1 IS NOT NULL THEN t.Spec1 ELSE m.Spec1 END,
            m.Spec2 = CASE WHEN t.Spec2 = '-1' THEN NULL WHEN t.Spec2 IS NOT NULL THEN t.Spec2 ELSE m.Spec2 END,
            m.Spec3 = CASE WHEN t.Spec3 = '-1' THEN NULL WHEN t.Spec3 IS NOT NULL THEN t.Spec3 ELSE m.Spec3 END,
            m.Spec4 = CASE WHEN t.Spec4 = '-1' THEN NULL WHEN t.Spec4 IS NOT NULL THEN t.Spec4 ELSE m.Spec4 END,
            
            m.Description_EN = CASE WHEN t.Description_EN IS NOT NULL THEN t.Description_EN ELSE m.Description_EN END,
            m.Description_VN = CASE WHEN t.Description_VN = '-1' THEN NULL WHEN t.Description_VN IS NOT NULL THEN t.Description_VN ELSE m.Description_VN END,
            
            m.SupplierCode = CASE WHEN t.SupplierCode IS NOT NULL THEN t.SupplierCode ELSE m.SupplierCode END,
            m.SupplierBUCode = CASE WHEN t.SupplierBUCode IS NOT NULL THEN t.SupplierBUCode ELSE m.SupplierBUCode END,
            m.SupplierBUId = CASE WHEN t.SupplierBUId IS NOT NULL THEN t.SupplierBUId ELSE m.SupplierBUId END,
            m.Factory_Text = CASE WHEN t.Factory_Text IS NOT NULL THEN t.Factory_Text ELSE m.Factory_Text END,
            m.MaterialType = CASE WHEN t.MaterialType IS NOT NULL THEN t.MaterialType ELSE m.MaterialType END,
            m.Unit = CASE WHEN t.Unit IS NOT NULL THEN t.Unit ELSE m.Unit END,
            m.Material_Group = CASE WHEN t.Material_Group IS NOT NULL THEN t.Material_Group ELSE m.Material_Group END,
            
            m.SAPMatGroup = CASE WHEN t.SAPMatGroup = '-1' THEN NULL WHEN t.SAPMatGroup IS NOT NULL THEN t.SAPMatGroup ELSE m.SAPMatGroup END,
            m.ProductHierarchyDescription = CASE WHEN t.ProductHierarchyDescription = '-1' THEN NULL WHEN t.ProductHierarchyDescription IS NOT NULL THEN t.ProductHierarchyDescription ELSE m.ProductHierarchyDescription END,
            m.CountryOfOrigin = CASE WHEN t.CountryOfOrigin = '-1' THEN NULL WHEN t.CountryOfOrigin IS NOT NULL THEN t.CountryOfOrigin ELSE m.CountryOfOrigin END,
            
            m.ReferenceLeadTime = CASE WHEN t.ReferenceLeadTime = -1 THEN NULL WHEN t.ReferenceLeadTime IS NOT NULL THEN t.ReferenceLeadTime ELSE m.ReferenceLeadTime END,
            
            m.WarrantyTime = CASE WHEN t.WarrantyTime IS NOT NULL THEN t.WarrantyTime ELSE m.WarrantyTime END,
            
            m.InventoryCategory = CASE WHEN t.InventoryCategory = '-1' THEN NULL WHEN t.InventoryCategory IS NOT NULL THEN t.InventoryCategory ELSE m.InventoryCategory END,
            m.CargoNote = CASE WHEN t.CargoNote = '-1' THEN NULL WHEN t.CargoNote IS NOT NULL THEN t.CargoNote ELSE m.CargoNote END,
            m.Weight = CASE WHEN t.Weight = '-1' THEN NULL WHEN t.Weight IS NOT NULL THEN t.Weight ELSE m.Weight END,
            m.Size = CASE WHEN t.Size = '-1' THEN NULL WHEN t.Size IS NOT NULL THEN t.Size ELSE m.Size END,
            m.QRCode = CASE WHEN t.QRCode = '-1' THEN NULL WHEN t.QRCode IS NOT NULL THEN t.QRCode ELSE m.QRCode END,
            m.HS_Code = CASE WHEN t.HS_Code = '-1' THEN NULL WHEN t.HS_Code IS NOT NULL THEN t.HS_Code ELSE m.HS_Code END,
            
            m.Maxlot = CASE WHEN t.Maxlot = -1 THEN NULL WHEN t.Maxlot IS NOT NULL THEN t.Maxlot ELSE m.Maxlot END,
            m.StockWarning = CASE WHEN t.StockWarning = -1 THEN NULL WHEN t.StockWarning IS NOT NULL THEN t.StockWarning ELSE m.StockWarning END,
            
            m.ConcurrencyStamp = NEWID(),
            m.LastModificationTime = GETDATE()
        FROM Materials m
        INNER JOIN #TempMaterialUpdate t ON m.Id = t.Id;

        DROP TABLE #TempMaterialUpdate;",
                param: null,
                transaction: transaction,
                commandTimeout: 300
            );

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    //public async Task BulkUpdateMaterialWithOutPriceAsync(
    //List<ExcelMaterialUpdateWithoutPrriceParams> updates,
    //CancellationToken cancellationToken = default)
    //{
    //    if (updates == null || updates.Count == 0) return;

    //    // Get connection string from DbContext and create a NEW independent connection
    //    // This avoids transaction conflicts with EF Core's managed connection
    //    var dbContext = await GetDbContextAsync();
    //    var connectionString = dbContext.Database.GetConnectionString();

    //    using var connection = new SqlConnection(connectionString);
    //    await connection.OpenAsync(cancellationToken);

    //    // Start transaction for atomicity (temp table + update must succeed together)
    //    using var transaction = connection.BeginTransaction();

    //    try
    //    {
    //        // Create DataTable for bulk copy (excluding GolfaCode since it won't be updated)
    //        var dataTable = new DataTable();
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Id), typeof(Guid));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Model), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.RegistrationDate), typeof(DateTime));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ValidFrom), typeof(DateTime));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ValidTo), typeof(DateTime));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec1), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec2), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec3), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec4), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Description_EN), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Description_VN), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierCode), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierBUCode), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierBUId), typeof(Guid));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Factory_Text), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.MaterialType), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Unit), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Material_SEC_Classification), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Material_Group), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.MaterialGroup), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.SAPMatGroup), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ProductHierarchyDescription), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.CountryOfOrigin), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ReferenceLeadTime), typeof(int));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.WarrantyTime), typeof(int));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.InventoryCategory), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Maxlot), typeof(int));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.StockWarning), typeof(int));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.StockQty), typeof(int));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.HS_Code), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.CargoNote), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Weight), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.Size), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.QRCode), typeof(string));
    //        dataTable.Columns.Add(nameof(ExcelMaterialUpdateWithoutPrriceParams.ConcurrencyStamp), typeof(string));

    //        foreach (var update in updates)
    //        {
    //            var row = dataTable.NewRow();
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Id)] = update.Id ?? Guid.Empty;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Model)] = update.Model ?? (object)DBNull.Value;

    //            // Special handling for RegistrationDate: DateTime.MinValue means clear it
    //            if (update.RegistrationDate.HasValue && update.RegistrationDate.Value == DateTime.MinValue)
    //            {
    //                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.RegistrationDate)] = DateTime.MinValue;
    //            }
    //            else
    //            {
    //                row[nameof(ExcelMaterialUpdateWithoutPrriceParams.RegistrationDate)] = update.RegistrationDate.HasValue ? update.RegistrationDate.Value : DBNull.Value;
    //            }

    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ValidFrom)] = update.ValidFrom.HasValue ? update.ValidFrom.Value : DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ValidTo)] = update.ValidTo.HasValue ? update.ValidTo.Value : DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec1)] = update.Spec1 ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec2)] = update.Spec2 ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec3)] = update.Spec3 ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Spec4)] = update.Spec4 ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Description_EN)] = update.Description_EN ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Description_VN)] = update.Description_VN ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierCode)] = update.SupplierCode ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierBUCode)] = update.SupplierBUCode ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.SupplierBUId)] = update.SupplierBUId.HasValue ? update.SupplierBUId.Value : DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Factory_Text)] = update.Factory_Text ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.MaterialType)] = update.MaterialType ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Unit)] = update.Unit ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Material_SEC_Classification)] = update.Material_SEC_Classification ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Material_Group)] = update.Material_Group ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.MaterialGroup)] = update.MaterialGroup ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.SAPMatGroup)] = update.SAPMatGroup ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ProductHierarchyDescription)] = update.ProductHierarchyDescription ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.CountryOfOrigin)] = update.CountryOfOrigin ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ReferenceLeadTime)] = update.ReferenceLeadTime.HasValue ? update.ReferenceLeadTime.Value : DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.WarrantyTime)] = update.WarrantyTime.HasValue ? update.WarrantyTime.Value : DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.InventoryCategory)] = update.InventoryCategory ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Maxlot)] = update.Maxlot.HasValue ? update.Maxlot.Value : DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.StockWarning)] = update.StockWarning.HasValue ? update.StockWarning.Value : DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.StockQty)] = update.StockQty.HasValue ? update.StockQty.Value : DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.HS_Code)] = update.HS_Code ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.CargoNote)] = update.CargoNote ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Weight)] = update.Weight ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.Size)] = update.Size ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.QRCode)] = update.QRCode ?? (object)DBNull.Value;
    //            row[nameof(ExcelMaterialUpdateWithoutPrriceParams.ConcurrencyStamp)] = update.ConcurrencyStamp ?? (object)DBNull.Value;
    //            dataTable.Rows.Add(row);
    //        }

    //        // Create temp table (excluding GolfaCode)
    //        await connection.ExecuteAsync(@"
    //        CREATE TABLE #TempMaterialUpdate (
    //            Id UNIQUEIDENTIFIER NOT NULL,
    //            Model NVARCHAR(MAX) NULL,
    //            RegistrationDate DATETIME NULL,
    //            ValidFrom DATETIME NULL,
    //            ValidTo DATETIME NULL,
    //            Spec1 NVARCHAR(MAX) NULL,
    //            Spec2 NVARCHAR(MAX) NULL,
    //            Spec3 NVARCHAR(MAX) NULL,
    //            Spec4 NVARCHAR(MAX) NULL,
    //            Description_EN NVARCHAR(MAX) NULL,
    //            Description_VN NVARCHAR(MAX) NULL,
    //            SupplierCode NVARCHAR(MAX) NULL,
    //            SupplierBUCode NVARCHAR(MAX) NULL,
    //            SupplierBUId UNIQUEIDENTIFIER NULL,
    //            Factory_Text NVARCHAR(MAX) NULL,
    //            MaterialType NVARCHAR(MAX) NULL,
    //            Unit NVARCHAR(MAX) NULL,
    //            Material_SEC_Classification NVARCHAR(MAX) NULL,
    //            Material_Group NVARCHAR(MAX) NULL,
    //            MaterialGroup NVARCHAR(MAX) NULL,
    //            SAPMatGroup NVARCHAR(MAX) NULL,
    //            ProductHierarchyDescription NVARCHAR(MAX) NULL,
    //            CountryOfOrigin NVARCHAR(MAX) NULL,
    //            ReferenceLeadTime INT NULL,
    //            WarrantyTime INT NULL,
    //            InventoryCategory NVARCHAR(MAX) NULL,
    //            Maxlot INT NULL,
    //            StockWarning INT NULL,
    //            StockQty INT NULL,
    //            HS_Code NVARCHAR(MAX) NULL,
    //            CargoNote NVARCHAR(MAX) NULL,
    //            Weight NVARCHAR(MAX) NULL,
    //            Size NVARCHAR(MAX) NULL,
    //            QRCode NVARCHAR(MAX) NULL,
    //            ConcurrencyStamp NVARCHAR(40) NULL
    //        )", param: null, transaction: transaction, commandTimeout: 120);

    //        // Bulk copy to temp table
    //        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
    //        {
    //            bulkCopy.DestinationTableName = "#TempMaterialUpdate";
    //            bulkCopy.BatchSize = 5000;
    //            bulkCopy.BulkCopyTimeout = 300;

    //            // Map all columns
    //            foreach (DataColumn column in dataTable.Columns)
    //            {
    //                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
    //            }

    //            await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);
    //        }

    //        // UPDATE with ROWLOCK hint
    //        // Based on UpdateListM3UFromExcelAsync logic:
    //        // - GolfaCode: NOT updated
    //        // - Model: only edits (NULL keeps existing, no clear)
    //        // - RegistrationDate: DateTime.MinValue clears it, NULL keeps existing
    //        // - ValidFrom, ValidTo: NOT allow NULL (NULL keeps existing)
    //        // - Spec1-4: '-1' clears, NULL keeps existing (Allow NULL)
    //        // - Description_EN: NOT allow NULL (NULL keeps existing)
    //        // - Description_VN: '-1' clears, NULL keeps existing (Allow NULL)
    //        // - SupplierCode, SupplierBUCode, Factory_Text, MaterialType, Unit, Material_Group: NOT allow NULL (NULL keeps existing)
    //        // - SAPMatGroup, ProductHierarchyDescription, CountryOfOrigin: '-1' clears, NULL keeps existing (Allow NULL)
    //        // - ReferenceLeadTime: -1 clears, NULL keeps existing (Allow NULL)
    //        // - WarrantyTime: NOT allow NULL (NULL keeps existing)
    //        // - InventoryCategory, CargoNote, Weight, Size, QRCode, HS_Code: '-1' clears, NULL keeps existing (Allow NULL)
    //        // - Maxlot, StockWarning: -1 clears, NULL keeps existing (Allow NULL)
    //        await connection.ExecuteAsync(@"
    //        UPDATE m WITH (ROWLOCK)
    //        SET
    //            -- Model: only edits (no clear)
    //            m.Model = CASE WHEN t.Model IS NOT NULL THEN t.Model ELSE m.Model END,

    //            -- RegistrationDate: DateTime.MinValue (1753-01-01) clears it
    //            m.RegistrationDate = CASE 
    //                WHEN t.RegistrationDate = '1753-01-01' THEN NULL 
    //                WHEN t.RegistrationDate IS NOT NULL THEN t.RegistrationDate 
    //                ELSE m.RegistrationDate 
    //            END,

    //            -- ValidFrom, ValidTo: NOT allow NULL
    //            m.ValidFrom = CASE WHEN t.ValidFrom IS NOT NULL THEN t.ValidFrom ELSE m.ValidFrom END,
    //            m.ValidTo = CASE WHEN t.ValidTo IS NOT NULL THEN t.ValidTo ELSE m.ValidTo END,

    //            -- Spec1-4: Allow NULL (clear with '-1')
    //            m.Spec1 = CASE WHEN t.Spec1 = '-1' THEN NULL WHEN t.Spec1 IS NOT NULL THEN t.Spec1 ELSE m.Spec1 END,
    //            m.Spec2 = CASE WHEN t.Spec2 = '-1' THEN NULL WHEN t.Spec2 IS NOT NULL THEN t.Spec2 ELSE m.Spec2 END,
    //            m.Spec3 = CASE WHEN t.Spec3 = '-1' THEN NULL WHEN t.Spec3 IS NOT NULL THEN t.Spec3 ELSE m.Spec3 END,
    //            m.Spec4 = CASE WHEN t.Spec4 = '-1' THEN NULL WHEN t.Spec4 IS NOT NULL THEN t.Spec4 ELSE m.Spec4 END,

    //            -- Description_EN: NOT allow NULL
    //            m.Description_EN = CASE WHEN t.Description_EN IS NOT NULL THEN t.Description_EN ELSE m.Description_EN END,

    //            -- Description_VN: Allow NULL (clear with '-1')
    //            m.Description_VN = CASE WHEN t.Description_VN = '-1' THEN NULL WHEN t.Description_VN IS NOT NULL THEN t.Description_VN ELSE m.Description_VN END,

    //            -- SupplierCode, SupplierBUCode, Factory_Text, MaterialType, Unit, Material_Group: NOT allow NULL
    //            m.SupplierCode = CASE WHEN t.SupplierCode IS NOT NULL THEN t.SupplierCode ELSE m.SupplierCode END,
    //            m.SupplierBUCode = CASE WHEN t.SupplierBUCode IS NOT NULL THEN t.SupplierBUCode ELSE m.SupplierBUCode END,
    //            m.SupplierBUId = CASE WHEN t.SupplierBUId IS NOT NULL THEN t.SupplierBUId ELSE m.SupplierBUId END,
    //            m.Factory_Text = CASE WHEN t.Factory_Text IS NOT NULL THEN t.Factory_Text ELSE m.Factory_Text END,
    //            m.MaterialType = CASE WHEN t.MaterialType IS NOT NULL THEN t.MaterialType ELSE m.MaterialType END,
    //            m.Unit = CASE WHEN t.Unit IS NOT NULL THEN t.Unit ELSE m.Unit END,
    //            m.Material_Group = CASE WHEN t.Material_Group IS NOT NULL THEN t.Material_Group ELSE m.Material_Group END,

    //            -- SAPMatGroup, ProductHierarchyDescription, CountryOfOrigin: Allow NULL (clear with '-1')
    //            m.SAPMatGroup = CASE WHEN t.SAPMatGroup = '-1' THEN NULL WHEN t.SAPMatGroup IS NOT NULL THEN t.SAPMatGroup ELSE m.SAPMatGroup END,
    //            m.ProductHierarchyDescription = CASE WHEN t.ProductHierarchyDescription = '-1' THEN NULL WHEN t.ProductHierarchyDescription IS NOT NULL THEN t.ProductHierarchyDescription ELSE m.ProductHierarchyDescription END,
    //            m.CountryOfOrigin = CASE WHEN t.CountryOfOrigin = '-1' THEN NULL WHEN t.CountryOfOrigin IS NOT NULL THEN t.CountryOfOrigin ELSE m.CountryOfOrigin END,

    //            -- ReferenceLeadTime: Allow NULL (clear with -1)
    //            m.ReferenceLeadTime = CASE WHEN t.ReferenceLeadTime = -1 THEN NULL WHEN t.ReferenceLeadTime IS NOT NULL THEN t.ReferenceLeadTime ELSE m.ReferenceLeadTime END,

    //            -- WarrantyTime: NOT allow NULL
    //            m.WarrantyTime = CASE WHEN t.WarrantyTime IS NOT NULL THEN t.WarrantyTime ELSE m.WarrantyTime END,

    //            -- InventoryCategory, CargoNote, Weight, Size, QRCode, HS_Code: Allow NULL (clear with '-1')
    //            m.InventoryCategory = CASE WHEN t.InventoryCategory = '-1' THEN NULL WHEN t.InventoryCategory IS NOT NULL THEN t.InventoryCategory ELSE m.InventoryCategory END,
    //            m.CargoNote = CASE WHEN t.CargoNote = '-1' THEN NULL WHEN t.CargoNote IS NOT NULL THEN t.CargoNote ELSE m.CargoNote END,
    //            m.Weight = CASE WHEN t.Weight = '-1' THEN NULL WHEN t.Weight IS NOT NULL THEN t.Weight ELSE m.Weight END,
    //            m.Size = CASE WHEN t.Size = '-1' THEN NULL WHEN t.Size IS NOT NULL THEN t.Size ELSE m.Size END,
    //            m.QRCode = CASE WHEN t.QRCode = '-1' THEN NULL WHEN t.QRCode IS NOT NULL THEN t.QRCode ELSE m.QRCode END,
    //            m.HS_Code = CASE WHEN t.HS_Code = '-1' THEN NULL WHEN t.HS_Code IS NOT NULL THEN t.HS_Code ELSE m.HS_Code END,

    //            -- Maxlot, StockWarning: Allow NULL (clear with -1)
    //            m.Maxlot = CASE WHEN t.Maxlot = -1 THEN NULL WHEN t.Maxlot IS NOT NULL THEN t.Maxlot ELSE m.Maxlot END,
    //            m.StockWarning = CASE WHEN t.StockWarning = -1 THEN NULL WHEN t.StockWarning IS NOT NULL THEN t.StockWarning ELSE m.StockWarning END,

    //            -- Always update these
    //            m.ConcurrencyStamp = NEWID(),
    //            m.LastModificationTime = GETDATE()
    //        FROM Materials m
    //        INNER JOIN #TempMaterialUpdate t ON m.Id = t.Id;

    //        DROP TABLE #TempMaterialUpdate;",
    //            param: null,
    //            transaction: transaction,
    //            commandTimeout: 300
    //        );

    //        transaction.Commit();
    //    }
    //    catch
    //    {
    //        transaction.Rollback();
    //        throw;
    //    }
    //}
}
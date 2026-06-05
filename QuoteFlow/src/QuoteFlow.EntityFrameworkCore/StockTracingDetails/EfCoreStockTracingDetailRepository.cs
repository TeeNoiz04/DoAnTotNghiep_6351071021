using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Helper;
using QuoteFlow.Materials;
using QuoteFlow.Shared.Utils;
using QuoteFlow.StockTracingDetails.ParameterObjects;
using QuoteFlow.StockTracings;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace QuoteFlow.StockTracingDetails;

public class EfCoreStockTracingDetailRepository : EfCoreRepository<QuoteFlowDbContext, StockTracingDetail, Guid>, IStockTracingDetailRepository
{
    private readonly ICurrentUser _currentUser;

    public EfCoreStockTracingDetailRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider, ICurrentUser currentUser)
        : base(dbContextProvider)
    {
        _currentUser = currentUser;
    }

    public virtual async Task<List<StockTracingDetail>> GetListAsync(
        StockTracingDetailFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        var materialType = filterParams.MaterialType?.ToUpper();
        var model = filterParams.Model?.ToUpper();

        if (!string.IsNullOrWhiteSpace(materialType) || !string.IsNullOrWhiteSpace(model))
        {
            var materials = dbContext.Set<Material>().AsQueryable();

            query =
                from d in query
                join m in materials on d.GolfaCode equals m.GolfaCode into matJoin
                from m in matJoin.DefaultIfEmpty()
                where
                    (string.IsNullOrEmpty(materialType) || (m.MaterialType ?? "").ToUpper().Contains(materialType)) &&
                    (string.IsNullOrEmpty(model) || (m.Model ?? "").ToUpper().Contains(model))
                select d;
        }

        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting) ? StockTracingDetailConsts.GetDefaultSorting(false) : filterParams.Sorting);

        return await query.PageBy(filterParams.SkipCount, filterParams.MaxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }


    public virtual async Task<long> GetCountAsync(
        StockTracingDetailFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        var materialType = filterParams.MaterialType?.ToUpper();
        var model = filterParams.Model?.ToUpper();

        if (!string.IsNullOrWhiteSpace(materialType) || !string.IsNullOrWhiteSpace(model))
        {
            var materials = dbContext.Set<Material>().AsQueryable();

            query =
                from d in query
                join m in materials on d.GolfaCode equals m.GolfaCode into matJoin
                from m in matJoin.DefaultIfEmpty()
                where
                    (string.IsNullOrEmpty(materialType) || (m.MaterialType ?? "").ToUpper().Contains(materialType)) &&
                    (string.IsNullOrEmpty(model) || (m.Model ?? "").ToUpper().Contains(model))
                select d;
        }

        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<StockTracingDetail> ApplyFilter(
        IQueryable<StockTracingDetail> query,
        StockTracingDetailFilterParams filterParams)
    {
        var filterText = filterParams.FilterText?.ToUpper();
        var stockTracingId = filterParams.StockTracingId;
        var reportType = filterParams.ReportType;
        var rowNoMin = filterParams.RowNoMin;
        var rowNoMax = filterParams.RowNoMax;
        var packingListCode = filterParams.PackingListCode?.ToUpper();
        var checkListCode = filterParams.CheckListCode;
        var dateEnteredMin = filterParams.DateEnteredMin;
        var dateEnteredMax = filterParams.DateEnteredMax;
        var stock = filterParams.Stock?.ToUpper();
        var bu = filterParams.BU?.ToUpper();
        var customer = filterParams.Customer?.ToUpper();
        var category = filterParams.Category?.ToUpper();
        var giv = filterParams.GIV?.ToUpper();
        var invoice = filterParams.Invoice?.ToUpper();
        var skuCode = filterParams.SKUCode?.ToUpper();
        var skuName = filterParams.SKUName;
        var quality = filterParams.Quality?.ToUpper();
        var warranty = filterParams.Warranty?.ToUpper();
        var unit = filterParams.Unit?.ToUpper();
        var series = filterParams.Series?.ToUpper();
        var originCode = filterParams.OriginCode?.ToUpper();
        var productionDateMin = filterParams.ProductionDateMin;
        var productionDateMax = filterParams.ProductionDateMax;
        var location = filterParams.Location?.ToUpper();
        var golfaCode = filterParams.GolfaCode?.ToUpper();
        var note = filterParams.Note?.ToUpper();
        var materialType = filterParams.MaterialType?.ToUpper();
        var model = filterParams.Model;

        return query
    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e =>
        (e.PackingListCode ?? "").ToUpper().Contains(filterText!) ||
        (e.CheckListCode ?? "").ToUpper().Contains(filterText!) ||
        (e.Stock ?? "").ToUpper().Contains(filterText!) ||
        (e.BU ?? "").ToUpper().Contains(filterText!) ||
        (e.Customer ?? "").ToUpper().Contains(filterText!) ||
        (e.Category ?? "").ToUpper().Contains(filterText!) ||
        (e.GIV ?? "").ToUpper().Contains(filterText!) ||
        (e.Invoice ?? "").ToUpper().Contains(filterText!) ||
        (e.SKUCode ?? "").ToUpper().Contains(filterText!) ||
        (e.SKUName ?? "").ToUpper().Contains(filterText!) ||
        (e.Quality ?? "").ToUpper().Contains(filterText!) ||
        (e.Warranty ?? "").ToUpper().Contains(filterText!) ||
        (e.Unit ?? "").ToUpper().Contains(filterText!) ||
        (e.Series ?? "").ToUpper().Contains(filterText!) ||
        (e.OriginCode ?? "").ToUpper().Contains(filterText!) ||
        (e.Location ?? "").ToUpper().Contains(filterText!) ||
        (e.GolfaCode ?? "").ToUpper().Contains(filterText!)
    )
            .WhereIf(stockTracingId.HasValue, e => e.StockTracingId == stockTracingId)
            .WhereIf(reportType != ReportType.None && reportType.HasValue, e => e.ReportType == reportType)
            .WhereIf(rowNoMin.HasValue, e => e.RowNo >= rowNoMin!.Value)
            .WhereIf(rowNoMax.HasValue, e => e.RowNo <= rowNoMax!.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(packingListCode), e => e.PackingListCode == packingListCode)
            .WhereIf(!string.IsNullOrWhiteSpace(checkListCode), e => e.CheckListCode == checkListCode)
            .WhereIf(dateEnteredMin.HasValue, e => e.StockTracing.FromDate >= dateEnteredMin!.Value)
            .WhereIf(dateEnteredMax.HasValue, e => e.StockTracing.ToDate <= dateEnteredMax!.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(stock), e => e.Stock == stock)
            .WhereIf(!string.IsNullOrWhiteSpace(bu), e => e.BU == bu)
            .WhereIf(!string.IsNullOrWhiteSpace(customer), QueryFilterHelper.BuildMultiFieldSearch<StockTracingDetail>(customer, e => e.Customer))
            .WhereIf(!string.IsNullOrWhiteSpace(category), e => e.Category == category)
            .WhereIf(!string.IsNullOrWhiteSpace(giv), e => e.GIV == giv)
            .WhereIf(!string.IsNullOrWhiteSpace(invoice), e => e.Invoice == invoice)
            .WhereIf(!string.IsNullOrWhiteSpace(skuCode), e => e.SKUCode == skuCode)
            .WhereIf(!string.IsNullOrWhiteSpace(skuName), e => e.SKUName == skuName)
            .WhereIf(!string.IsNullOrWhiteSpace(quality), e => e.Quality == quality)
            .WhereIf(!string.IsNullOrWhiteSpace(warranty), e => e.Warranty == warranty)
            .WhereIf(!string.IsNullOrWhiteSpace(unit), e => e.Unit == unit)
            .WhereIf(!string.IsNullOrWhiteSpace(series), QueryFilterHelper.BuildMultiFieldSearch<StockTracingDetail>(series, e => e.Series))
            .WhereIf(!string.IsNullOrWhiteSpace(originCode), e => e.OriginCode == originCode)
            .WhereIf(productionDateMin.HasValue, e => e.ProductionDate >= productionDateMin!.Value)
            .WhereIf(productionDateMax.HasValue, e => e.ProductionDate <= productionDateMax!.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(location), e => e.Location == location)
            .WhereIf(!string.IsNullOrWhiteSpace(golfaCode), QueryFilterHelper.BuildMultiFieldSearch<StockTracingDetail>(golfaCode, e => e.GolfaCode))
            .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note == note);
    }

    public async Task BulkInsertAsync(
        List<StockTracingDetail> details,
        CancellationToken cancellationToken = default)
    {
        if (details == null || details.Count == 0) return;

        // Get connection string from DbContext and create a NEW independent connection
        var dbContext = await GetDbContextAsync();
        var connectionString = dbContext.Database.GetConnectionString();

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        // Start transaction for atomicity
        using var transaction = connection.BeginTransaction();

        try
        {
            var fId = nameof(StockTracingDetail.Id);
            var fStockTracingId = nameof(StockTracingDetail.StockTracingId);
            var fReportType = nameof(StockTracingDetail.ReportType);
            var fRowNo = nameof(StockTracingDetail.RowNo);
            var fPackingListCode = nameof(StockTracingDetail.PackingListCode);
            var fCheckListCode = nameof(StockTracingDetail.CheckListCode);
            var fDateEntered = nameof(StockTracingDetail.DateEntered);
            var fStock = nameof(StockTracingDetail.Stock);
            var fBU = nameof(StockTracingDetail.BU);
            var fCustomer = nameof(StockTracingDetail.Customer);
            var fCategory = nameof(StockTracingDetail.Category);
            var fGIV = nameof(StockTracingDetail.GIV);
            var fInvoice = nameof(StockTracingDetail.Invoice);
            var fSKUCode = nameof(StockTracingDetail.SKUCode);
            var fSKUName = nameof(StockTracingDetail.SKUName);
            var fQuality = nameof(StockTracingDetail.Quality);
            var fWarranty = nameof(StockTracingDetail.Warranty);
            var fUnit = nameof(StockTracingDetail.Unit);
            var fSeries = nameof(StockTracingDetail.Series);
            var fOriginCode = nameof(StockTracingDetail.OriginCode);
            var fProductionDate = nameof(StockTracingDetail.ProductionDate);
            var fLocation = nameof(StockTracingDetail.Location);
            var fGolfaCode = nameof(StockTracingDetail.GolfaCode);
            var fNote = nameof(StockTracingDetail.Note);
            var fExtraProperties = nameof(StockTracingDetail.ExtraProperties);
            var fConcurrencyStamp = nameof(StockTracingDetail.ConcurrencyStamp);
            var fCreationTime = nameof(StockTracingDetail.CreationTime);
            var fCreatorId = nameof(StockTracingDetail.CreatorId);
            var fCreatorUsername = nameof(StockTracingDetail.CreatorUsername);
            var fCreatorFullName = nameof(StockTracingDetail.CreatorName);
            var fLastModificationTime = nameof(StockTracingDetail.LastModificationTime);
            var fLastModifierId = nameof(StockTracingDetail.LastModifierId);
            var fLastModifierUsername = nameof(StockTracingDetail.LastModifierUsername);
            var fLastModifierFullName = nameof(StockTracingDetail.LastModifierName);

            // Get current user info for audit fields
            var currentUserId = _currentUser.Id;
            var currentUsername = _currentUser.UserName;
            var currentFullName = UserHelper.GetFullName(_currentUser.Name, _currentUser.SurName);
            var currentTime = DateTime.UtcNow;

            // Create DataTable for bulk copy
            var dataTable = new DataTable();
            dataTable.Columns.Add(fId, typeof(Guid));
            dataTable.Columns.Add(fStockTracingId, typeof(Guid));
            dataTable.Columns.Add(fReportType, typeof(int));
            dataTable.Columns.Add(fRowNo, typeof(int));
            dataTable.Columns.Add(fPackingListCode, typeof(string));
            dataTable.Columns.Add(fCheckListCode, typeof(string));
            dataTable.Columns.Add(fDateEntered, typeof(DateTime));
            dataTable.Columns.Add(fStock, typeof(string));
            dataTable.Columns.Add(fBU, typeof(string));
            dataTable.Columns.Add(fCustomer, typeof(string));
            dataTable.Columns.Add(fCategory, typeof(string));
            dataTable.Columns.Add(fGIV, typeof(string));
            dataTable.Columns.Add(fInvoice, typeof(string));
            dataTable.Columns.Add(fSKUCode, typeof(string));
            dataTable.Columns.Add(fSKUName, typeof(string));
            dataTable.Columns.Add(fQuality, typeof(string));
            dataTable.Columns.Add(fWarranty, typeof(string));
            dataTable.Columns.Add(fUnit, typeof(string));
            dataTable.Columns.Add(fSeries, typeof(string));
            dataTable.Columns.Add(fOriginCode, typeof(string));
            dataTable.Columns.Add(fProductionDate, typeof(DateTime));
            dataTable.Columns.Add(fLocation, typeof(string));
            dataTable.Columns.Add(fGolfaCode, typeof(string));
            dataTable.Columns.Add(fNote, typeof(string));
            dataTable.Columns.Add(fExtraProperties, typeof(string));
            dataTable.Columns.Add(fConcurrencyStamp, typeof(string));
            dataTable.Columns.Add(fCreationTime, typeof(DateTime));
            dataTable.Columns.Add(fCreatorId, typeof(Guid));
            dataTable.Columns.Add(fCreatorUsername, typeof(string));
            dataTable.Columns.Add(fCreatorFullName, typeof(string));
            dataTable.Columns.Add(fLastModificationTime, typeof(DateTime));
            dataTable.Columns.Add(fLastModifierId, typeof(Guid));
            dataTable.Columns.Add(fLastModifierUsername, typeof(string));
            dataTable.Columns.Add(fLastModifierFullName, typeof(string));

            foreach (var detail in details)
            {
                var row = dataTable.NewRow();
                row[fId] = detail.Id;
                row[fStockTracingId] = detail.StockTracingId;
                row[fReportType] = (int)detail.ReportType;
                row[fRowNo] = detail.RowNo.HasValue ? detail.RowNo.Value : DBNull.Value;
                row[fPackingListCode] = detail.PackingListCode ?? (object)DBNull.Value;
                row[fCheckListCode] = detail.CheckListCode ?? (object)DBNull.Value;
                row[fDateEntered] = detail.DateEntered.HasValue ? detail.DateEntered.Value : DBNull.Value;
                row[fStock] = detail.Stock ?? (object)DBNull.Value;
                row[fBU] = detail.BU ?? (object)DBNull.Value;
                row[fCustomer] = detail.Customer ?? (object)DBNull.Value;
                row[fCategory] = detail.Category ?? (object)DBNull.Value;
                row[fGIV] = detail.GIV ?? (object)DBNull.Value;
                row[fInvoice] = detail.Invoice ?? (object)DBNull.Value;
                row[fSKUCode] = detail.SKUCode ?? (object)DBNull.Value;
                row[fSKUName] = detail.SKUName ?? (object)DBNull.Value;
                row[fQuality] = detail.Quality ?? (object)DBNull.Value;
                row[fWarranty] = detail.Warranty ?? (object)DBNull.Value;
                row[fUnit] = detail.Unit ?? (object)DBNull.Value;
                row[fSeries] = detail.Series ?? (object)DBNull.Value;
                row[fOriginCode] = detail.OriginCode ?? (object)DBNull.Value;
                row[fProductionDate] = detail.ProductionDate.HasValue ? detail.ProductionDate.Value : DBNull.Value;
                row[fLocation] = detail.Location ?? (object)DBNull.Value;
                row[fGolfaCode] = detail.GolfaCode ?? (object)DBNull.Value;
                row[fNote] = detail.Note ?? (object)DBNull.Value;
                row[fExtraProperties] = "{}";
                row[fConcurrencyStamp] = Guid.NewGuid().ToString();
                row[fCreationTime] = currentTime;
                row[fCreatorId] = currentUserId.HasValue ? currentUserId.Value : DBNull.Value;
                row[fCreatorUsername] = currentUsername ?? (object)DBNull.Value;
                row[fCreatorFullName] = currentFullName ?? (object)DBNull.Value;
                row[fLastModificationTime] = currentTime;
                row[fLastModifierId] = currentUserId.HasValue ? currentUserId.Value : DBNull.Value;
                row[fLastModifierUsername] = currentUsername ?? (object)DBNull.Value;
                row[fLastModifierFullName] = currentFullName ?? (object)DBNull.Value;
                dataTable.Rows.Add(row);
            }

            // Bulk copy directly to main table (no temp table needed for insert-only operation)
            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = "StockTracingDetails";
                bulkCopy.BatchSize = 5000; // Prevents lock escalation
                bulkCopy.BulkCopyTimeout = 300; // 5 minutes

                // Map all columns
                bulkCopy.ColumnMappings.Add(fId, fId);
                bulkCopy.ColumnMappings.Add(fStockTracingId, fStockTracingId);
                bulkCopy.ColumnMappings.Add(fReportType, fReportType);
                bulkCopy.ColumnMappings.Add(fRowNo, fRowNo);
                bulkCopy.ColumnMappings.Add(fPackingListCode, fPackingListCode);
                bulkCopy.ColumnMappings.Add(fCheckListCode, fCheckListCode);
                bulkCopy.ColumnMappings.Add(fDateEntered, fDateEntered);
                bulkCopy.ColumnMappings.Add(fStock, fStock);
                bulkCopy.ColumnMappings.Add(fBU, fBU);
                bulkCopy.ColumnMappings.Add(fCustomer, fCustomer);
                bulkCopy.ColumnMappings.Add(fCategory, fCategory);
                bulkCopy.ColumnMappings.Add(fGIV, fGIV);
                bulkCopy.ColumnMappings.Add(fInvoice, fInvoice);
                bulkCopy.ColumnMappings.Add(fSKUCode, fSKUCode);
                bulkCopy.ColumnMappings.Add(fSKUName, fSKUName);
                bulkCopy.ColumnMappings.Add(fQuality, fQuality);
                bulkCopy.ColumnMappings.Add(fWarranty, fWarranty);
                bulkCopy.ColumnMappings.Add(fUnit, fUnit);
                bulkCopy.ColumnMappings.Add(fSeries, fSeries);
                bulkCopy.ColumnMappings.Add(fOriginCode, fOriginCode);
                bulkCopy.ColumnMappings.Add(fProductionDate, fProductionDate);
                bulkCopy.ColumnMappings.Add(fLocation, fLocation);
                bulkCopy.ColumnMappings.Add(fGolfaCode, fGolfaCode);
                bulkCopy.ColumnMappings.Add(fNote, fNote);
                bulkCopy.ColumnMappings.Add(fExtraProperties, fExtraProperties);
                bulkCopy.ColumnMappings.Add(fConcurrencyStamp, fConcurrencyStamp);
                bulkCopy.ColumnMappings.Add(fCreationTime, fCreationTime);
                bulkCopy.ColumnMappings.Add(fCreatorId, fCreatorId);
                bulkCopy.ColumnMappings.Add(fCreatorUsername, fCreatorUsername);
                bulkCopy.ColumnMappings.Add(fCreatorFullName, fCreatorFullName);
                bulkCopy.ColumnMappings.Add(fLastModificationTime, fLastModificationTime);
                bulkCopy.ColumnMappings.Add(fLastModifierId, fLastModifierId);
                bulkCopy.ColumnMappings.Add(fLastModifierUsername, fLastModifierUsername);
                bulkCopy.ColumnMappings.Add(fLastModifierFullName, fLastModifierFullName);

                await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
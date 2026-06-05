using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
using QuoteFlow.Shared.Models;
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
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Materials.MaterialApprovalRequestDetails;

public class EfCoreMaterialApprovalRequestDetailRepository : EfCoreRepository<QuoteFlowDbContext, MaterialApprovalRequestDetail, Guid>, IMaterialApprovalRequestDetailRepository
{
    public EfCoreMaterialApprovalRequestDetailRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<MaterialApprovalRequestDetail>> GetListAsync(

        Guid approvalId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), approvalId);
        //query = query.Include(x => x.MaterialGroupCategory);
        query = query.Include(x => x.InputCurrency);
        query = query.OrderBy(x => x.GolfaCode);
        return await query.PageBy(0, 1000).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        Guid approvalId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), approvalId);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<MaterialApprovalRequestDetail> ApplyFilter(
        IQueryable<MaterialApprovalRequestDetail> query,
        Guid approvalId)
    {
        return query

                .Where(e => e.MaterialApprovalId == approvalId);
    }

    public virtual async Task<List<MaterialApprovalRequestDetail>> GetListByApprovalIdAsync(
     Guid approvalId,
     CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        return await dbContext.MaterialApprovalRequestDetails
            .Where(x => x.MaterialApprovalId == approvalId)
            .ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<List<T>> GetListAsync<T>(MaterialDetailFilterParams filterParams, Expression<Func<MaterialApprovalRequestDetail, T>> selector, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);

        query = query.OrderBy(MaterialApprovalRequestDetailConsts.GetDefaultSorting(false));
        var resultQuery = query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .Select(selector).AsQueryable();

        var result = await resultQuery.ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }

    public virtual async Task<List<MaterialApprovalRequestDetail>> GetListAsync(
        MaterialDetailFilterParams filterParams,
        CancellationToken cancellationToken = default
    )
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);

        query = query.OrderBy(MaterialApprovalRequestDetailConsts.GetDefaultSorting(false));
        var result = await query.ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }

    public virtual async Task ActionAsync(
        Guid materialApprovalId,
        string action,
        CancellationToken cancellationToken = default
    )
    {
        var dbContext = await GetDbContextAsync();

        await dbContext.MaterialApprovalRequestDetails
            .Where(x => x.MaterialApprovalId == materialApprovalId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.MaterialStatus, action));

    }

    protected virtual IQueryable<MaterialApprovalRequestDetail> ApplyFilter(
        IQueryable<MaterialApprovalRequestDetail> query,
        MaterialDetailFilterParams filterParams)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.GolfaCode), e => e.GolfaCode.Contains(filterParams.GolfaCode!))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Model), e => e.Model.Contains(filterParams.Model!))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.MaterialType), e => e.MaterialType.Contains(filterParams.MaterialType!))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.MaterialGroup), e => e.Material_Group.Contains(filterParams.MaterialGroup!))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.SAPCode), e => e.SAP_Code.Contains(filterParams.SAPCode!))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.SupplierBU), e => e.SupplierBUCode.Contains(filterParams.SupplierBU!))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Supplier), e => e.SupplierCode.Contains(filterParams.Supplier!));
    }

    public async Task BulkInsertAsync(
        List<MaterialApprovalRequestDetail> details,
        CancellationToken cancellationToken = default)
    {
        if (details == null || details.Count == 0) return;

        // Get connection string from DbContext and create a NEW independent connection
        // This avoids transaction conflicts with EF Core's managed connection
        var dbContext = await GetDbContextAsync();
        var connectionString = dbContext.Database.GetConnectionString();

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        // Start transaction for atomicity
        using var transaction = connection.BeginTransaction();

        try
        {
            // Create DataTable with columns matching the table structure
            string idField = nameof(MaterialApprovalRequestDetail.Id);
            string materialApprovalIdField = nameof(MaterialApprovalRequestDetail.MaterialApprovalId);
            string golfaCodeField = nameof(MaterialApprovalRequestDetail.GolfaCode);
            string modelField = nameof(MaterialApprovalRequestDetail.Model);
            string referenceLeadTimeField = nameof(MaterialApprovalRequestDetail.ReferenceLeadTime);
            string countryOfOriginField = nameof(MaterialApprovalRequestDetail.CountryOfOrigin);
            string maxlotField = nameof(MaterialApprovalRequestDetail.Maxlot);
            string materialStatusField = nameof(MaterialApprovalRequestDetail.MaterialStatus);
            string creationTimeField = nameof(MaterialApprovalRequestDetail.CreationTime);
            string concurrencyStampField = nameof(MaterialApprovalRequestDetail.ConcurrencyStamp);
            string extraPropertiesField = nameof(MaterialApprovalRequestDetail.ExtraProperties);

            var dataTable = new DataTable();
            dataTable.Columns.Add(idField, typeof(Guid));
            dataTable.Columns.Add(materialApprovalIdField, typeof(Guid));
            dataTable.Columns.Add(golfaCodeField, typeof(string));
            dataTable.Columns.Add(modelField, typeof(string));
            dataTable.Columns.Add(referenceLeadTimeField, typeof(int));
            dataTable.Columns.Add(countryOfOriginField, typeof(string));
            dataTable.Columns.Add(maxlotField, typeof(int));
            dataTable.Columns.Add(materialStatusField, typeof(string));
            dataTable.Columns.Add(creationTimeField, typeof(DateTime));
            dataTable.Columns.Add(concurrencyStampField, typeof(string));
            dataTable.Columns.Add(extraPropertiesField, typeof(string));

            var now = DateTime.Now;
            foreach (var detail in details)
            {
                var row = dataTable.NewRow();
                row[idField] = detail.Id;
                row[materialApprovalIdField] = detail.MaterialApprovalId;
                row[golfaCodeField] = detail.GolfaCode;
                row[modelField] = detail.Model;
                row[referenceLeadTimeField] = detail.ReferenceLeadTime.HasValue ? detail.ReferenceLeadTime.Value : DBNull.Value;
                row[countryOfOriginField] = detail.CountryOfOrigin ?? (object)DBNull.Value;
                row[maxlotField] = detail.Maxlot.HasValue ? detail.Maxlot.Value : DBNull.Value;
                row[materialStatusField] = detail.MaterialStatus ?? QuoteFlowStatuses.Draft;
                row[creationTimeField] = now;
                row[concurrencyStampField] = Guid.NewGuid().ToString();
                row[extraPropertiesField] = "{}";
                dataTable.Rows.Add(row);
            }

            // Use SqlBulkCopy for fast insert (pass transaction)
            using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction)
            {
                DestinationTableName = nameof(MaterialApprovalRequestDetail),
                BatchSize = 5000, // Prevents lock escalation
                BulkCopyTimeout = 300 // 5 minutes
            };

            bulkCopy.ColumnMappings.Add(idField, idField);
            bulkCopy.ColumnMappings.Add(materialApprovalIdField, materialApprovalIdField);
            bulkCopy.ColumnMappings.Add(golfaCodeField, golfaCodeField);
            bulkCopy.ColumnMappings.Add(modelField, modelField);
            bulkCopy.ColumnMappings.Add(referenceLeadTimeField, referenceLeadTimeField);
            bulkCopy.ColumnMappings.Add(countryOfOriginField, countryOfOriginField);
            bulkCopy.ColumnMappings.Add(maxlotField, maxlotField);
            bulkCopy.ColumnMappings.Add(materialStatusField, materialStatusField);
            bulkCopy.ColumnMappings.Add(creationTimeField, creationTimeField);
            bulkCopy.ColumnMappings.Add(concurrencyStampField, concurrencyStampField);
            bulkCopy.ColumnMappings.Add(extraPropertiesField, extraPropertiesField);

            await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task BulkInsertM3UAsync(
    List<MaterialApprovalRequestDetail> details,
    CancellationToken cancellationToken = default)
    {
        if (details == null || details.Count == 0) return;

        // Get connection string from DbContext and create a NEW independent connection
        // This avoids transaction conflicts with EF Core's managed connection
        var dbContext = await GetDbContextAsync();
        var connectionString = dbContext.Database.GetConnectionString();

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        // Start transaction for atomicity
        using var transaction = connection.BeginTransaction();

        try
        {
            // Create DataTable with columns matching MaterialUpdateWithoutPriceImportDto
            string idField = nameof(MaterialApprovalRequestDetail.Id);
            string materialApprovalIdField = nameof(MaterialApprovalRequestDetail.MaterialApprovalId);
            string golfaCodeField = nameof(MaterialApprovalRequestDetail.GolfaCode);
            string modelField = nameof(MaterialApprovalRequestDetail.Model);
            string registrationDateField = nameof(MaterialApprovalRequestDetail.RegistrationDate);
            string validFromField = nameof(MaterialApprovalRequestDetail.ValidFrom);
            string validToField = nameof(MaterialApprovalRequestDetail.ValidTo);
            string spec1Field = nameof(MaterialApprovalRequestDetail.Spec1);
            string spec2Field = nameof(MaterialApprovalRequestDetail.Spec2);
            string spec3Field = nameof(MaterialApprovalRequestDetail.Spec3);
            string spec4Field = nameof(MaterialApprovalRequestDetail.Spec4);
            string descriptionENField = nameof(MaterialApprovalRequestDetail.Description_EN);
            string descriptionVNField = nameof(MaterialApprovalRequestDetail.Description_VN);
            string supplierCodeField = nameof(MaterialApprovalRequestDetail.SupplierCode);
            string supplierBUCodeField = nameof(MaterialApprovalRequestDetail.SupplierBUCode);
            string supplierBUIdField = nameof(MaterialApprovalRequestDetail.SupplierBUId);
            string factoryTextField = nameof(MaterialApprovalRequestDetail.Factory_Text);
            string materialTypeField = nameof(MaterialApprovalRequestDetail.MaterialType);
            string unitField = nameof(MaterialApprovalRequestDetail.Unit);
            string materialGroupField = nameof(MaterialApprovalRequestDetail.Material_Group);
            string sapMatGroupField = nameof(MaterialApprovalRequestDetail.SAPMatGroup);
            string productHierarchyDescriptionField = nameof(MaterialApprovalRequestDetail.ProductHierarchyDescription);
            string countryOfOriginField = nameof(MaterialApprovalRequestDetail.CountryOfOrigin);
            string referenceLeadTimeField = nameof(MaterialApprovalRequestDetail.ReferenceLeadTime);
            string warrantyTimeField = nameof(MaterialApprovalRequestDetail.WarrantyTime);
            string inventoryCategoryField = nameof(MaterialApprovalRequestDetail.InventoryCategory);
            string cargoNoteField = nameof(MaterialApprovalRequestDetail.CargoNote);
            string weightField = nameof(MaterialApprovalRequestDetail.Weight);
            string sizeField = nameof(MaterialApprovalRequestDetail.Size);
            string qrCodeField = nameof(MaterialApprovalRequestDetail.QRCode);
            string maxlotField = nameof(MaterialApprovalRequestDetail.Maxlot);
            string stockWarningField = nameof(MaterialApprovalRequestDetail.StockWarning);
            //string stockQtyField = nameof(MaterialApprovalRequestDetail.StockQty);
            string hsCodeField = nameof(MaterialApprovalRequestDetail.HS_Code);
            string materialStatusField = nameof(MaterialApprovalRequestDetail.MaterialStatus);
            string creationTimeField = nameof(MaterialApprovalRequestDetail.CreationTime);
            string concurrencyStampField = nameof(MaterialApprovalRequestDetail.ConcurrencyStamp);
            string extraPropertiesField = nameof(MaterialApprovalRequestDetail.ExtraProperties);

            var dataTable = new DataTable();
            dataTable.Columns.Add(idField, typeof(Guid));
            dataTable.Columns.Add(materialApprovalIdField, typeof(Guid));
            dataTable.Columns.Add(golfaCodeField, typeof(string));
            dataTable.Columns.Add(modelField, typeof(string));
            dataTable.Columns.Add(registrationDateField, typeof(DateTime));
            dataTable.Columns.Add(validFromField, typeof(DateTime));
            dataTable.Columns.Add(validToField, typeof(DateTime));
            dataTable.Columns.Add(spec1Field, typeof(string));
            dataTable.Columns.Add(spec2Field, typeof(string));
            dataTable.Columns.Add(spec3Field, typeof(string));
            dataTable.Columns.Add(spec4Field, typeof(string));
            dataTable.Columns.Add(descriptionENField, typeof(string));
            dataTable.Columns.Add(descriptionVNField, typeof(string));
            dataTable.Columns.Add(supplierCodeField, typeof(string));
            dataTable.Columns.Add(supplierBUCodeField, typeof(string));
            dataTable.Columns.Add(supplierBUIdField, typeof(Guid));
            dataTable.Columns.Add(factoryTextField, typeof(string));
            dataTable.Columns.Add(materialTypeField, typeof(string));
            dataTable.Columns.Add(unitField, typeof(string));
            dataTable.Columns.Add(materialGroupField, typeof(string));
            dataTable.Columns.Add(sapMatGroupField, typeof(string));
            dataTable.Columns.Add(productHierarchyDescriptionField, typeof(string));
            dataTable.Columns.Add(countryOfOriginField, typeof(string));
            dataTable.Columns.Add(referenceLeadTimeField, typeof(int));
            dataTable.Columns.Add(warrantyTimeField, typeof(int));
            dataTable.Columns.Add(inventoryCategoryField, typeof(string));
            dataTable.Columns.Add(cargoNoteField, typeof(string));
            dataTable.Columns.Add(weightField, typeof(string));
            dataTable.Columns.Add(sizeField, typeof(string));
            dataTable.Columns.Add(qrCodeField, typeof(string));
            dataTable.Columns.Add(maxlotField, typeof(int));
            dataTable.Columns.Add(stockWarningField, typeof(int));
            //dataTable.Columns.Add(stockQtyField, typeof(int));
            dataTable.Columns.Add(hsCodeField, typeof(string));
            dataTable.Columns.Add(materialStatusField, typeof(string));
            dataTable.Columns.Add(creationTimeField, typeof(DateTime));
            dataTable.Columns.Add(concurrencyStampField, typeof(string));
            dataTable.Columns.Add(extraPropertiesField, typeof(string));

            var now = DateTime.Now;
            foreach (var detail in details)
            {
                var row = dataTable.NewRow();
                row[idField] = detail.Id;
                row[materialApprovalIdField] = detail.MaterialApprovalId;
                row[golfaCodeField] = detail.GolfaCode;
                row[modelField] = detail.Model;
                row[registrationDateField] = detail.RegistrationDate.HasValue ? detail.RegistrationDate.Value : DBNull.Value;
                row[validFromField] = detail.ValidFrom.HasValue ? detail.ValidFrom.Value : DBNull.Value;
                row[validToField] = detail.ValidTo.HasValue ? detail.ValidTo.Value : DBNull.Value;
                row[spec1Field] = detail.Spec1 ?? (object)DBNull.Value;
                row[spec2Field] = detail.Spec2 ?? (object)DBNull.Value;
                row[spec3Field] = detail.Spec3 ?? (object)DBNull.Value;
                row[spec4Field] = detail.Spec4 ?? (object)DBNull.Value;
                row[descriptionENField] = detail.Description_EN ?? (object)DBNull.Value;
                row[descriptionVNField] = detail.Description_VN ?? (object)DBNull.Value;
                row[supplierCodeField] = detail.SupplierCode ?? (object)DBNull.Value;
                row[supplierBUCodeField] = detail.SupplierBUCode ?? (object)DBNull.Value;
                row[supplierBUIdField] = detail.SupplierBUId.HasValue ? detail.SupplierBUId.Value : DBNull.Value;
                row[factoryTextField] = detail.Factory_Text ?? (object)DBNull.Value;
                row[materialTypeField] = detail.MaterialType ?? (object)DBNull.Value;
                row[unitField] = detail.Unit ?? (object)DBNull.Value;
                row[materialGroupField] = detail.Material_Group ?? (object)DBNull.Value;
                row[sapMatGroupField] = detail.SAPMatGroup ?? (object)DBNull.Value;
                row[productHierarchyDescriptionField] = detail.ProductHierarchyDescription ?? (object)DBNull.Value;
                row[countryOfOriginField] = detail.CountryOfOrigin ?? (object)DBNull.Value;
                row[referenceLeadTimeField] = detail.ReferenceLeadTime.HasValue ? detail.ReferenceLeadTime.Value : DBNull.Value;
                row[warrantyTimeField] = detail.WarrantyTime.HasValue ? detail.WarrantyTime.Value : DBNull.Value;
                row[inventoryCategoryField] = detail.InventoryCategory ?? (object)DBNull.Value;
                row[cargoNoteField] = detail.CargoNote ?? (object)DBNull.Value;
                row[weightField] = detail.Weight ?? (object)DBNull.Value;
                row[sizeField] = detail.Size ?? (object)DBNull.Value;
                row[qrCodeField] = detail.QRCode ?? (object)DBNull.Value;
                row[maxlotField] = detail.Maxlot.HasValue ? detail.Maxlot.Value : DBNull.Value;
                row[stockWarningField] = detail.StockWarning.HasValue ? detail.StockWarning.Value : DBNull.Value;
                //row[stockQtyField] = detail.StockQty.HasValue ? detail.StockQty.Value : DBNull.Value;
                row[hsCodeField] = detail.HS_Code ?? (object)DBNull.Value;
                row[materialStatusField] = detail.MaterialStatus ?? QuoteFlowStatuses.Draft;
                row[creationTimeField] = now;
                row[concurrencyStampField] = Guid.NewGuid().ToString();
                row[extraPropertiesField] = "{}";
                dataTable.Rows.Add(row);
            }

            // Use SqlBulkCopy for fast insert (pass transaction)
            using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction)
            {
                DestinationTableName = nameof(MaterialApprovalRequestDetail),
                BatchSize = 5000, // Prevents lock escalation
                BulkCopyTimeout = 300 // 5 minutes
            };

            bulkCopy.ColumnMappings.Add(idField, idField);
            bulkCopy.ColumnMappings.Add(materialApprovalIdField, materialApprovalIdField);
            bulkCopy.ColumnMappings.Add(golfaCodeField, golfaCodeField);
            bulkCopy.ColumnMappings.Add(modelField, modelField);
            bulkCopy.ColumnMappings.Add(registrationDateField, registrationDateField);
            bulkCopy.ColumnMappings.Add(validFromField, validFromField);
            bulkCopy.ColumnMappings.Add(validToField, validToField);
            bulkCopy.ColumnMappings.Add(spec1Field, spec1Field);
            bulkCopy.ColumnMappings.Add(spec2Field, spec2Field);
            bulkCopy.ColumnMappings.Add(spec3Field, spec3Field);
            bulkCopy.ColumnMappings.Add(spec4Field, spec4Field);
            bulkCopy.ColumnMappings.Add(descriptionENField, descriptionENField);
            bulkCopy.ColumnMappings.Add(descriptionVNField, descriptionVNField);
            bulkCopy.ColumnMappings.Add(supplierCodeField, supplierCodeField);
            bulkCopy.ColumnMappings.Add(supplierBUCodeField, supplierBUCodeField);
            bulkCopy.ColumnMappings.Add(supplierBUIdField, supplierBUIdField);
            bulkCopy.ColumnMappings.Add(factoryTextField, factoryTextField);
            bulkCopy.ColumnMappings.Add(materialTypeField, materialTypeField);
            bulkCopy.ColumnMappings.Add(unitField, unitField);
            bulkCopy.ColumnMappings.Add(materialGroupField, materialGroupField);
            bulkCopy.ColumnMappings.Add(sapMatGroupField, sapMatGroupField);
            bulkCopy.ColumnMappings.Add(productHierarchyDescriptionField, productHierarchyDescriptionField);
            bulkCopy.ColumnMappings.Add(countryOfOriginField, countryOfOriginField);
            bulkCopy.ColumnMappings.Add(referenceLeadTimeField, referenceLeadTimeField);
            bulkCopy.ColumnMappings.Add(warrantyTimeField, warrantyTimeField);
            bulkCopy.ColumnMappings.Add(inventoryCategoryField, inventoryCategoryField);
            bulkCopy.ColumnMappings.Add(cargoNoteField, cargoNoteField);
            bulkCopy.ColumnMappings.Add(weightField, weightField);
            bulkCopy.ColumnMappings.Add(sizeField, sizeField);
            bulkCopy.ColumnMappings.Add(qrCodeField, qrCodeField);
            bulkCopy.ColumnMappings.Add(maxlotField, maxlotField);
            bulkCopy.ColumnMappings.Add(stockWarningField, stockWarningField);
            //bulkCopy.ColumnMappings.Add(stockQtyField, stockQtyField);
            bulkCopy.ColumnMappings.Add(hsCodeField, hsCodeField);
            bulkCopy.ColumnMappings.Add(materialStatusField, materialStatusField);
            bulkCopy.ColumnMappings.Add(creationTimeField, creationTimeField);
            bulkCopy.ColumnMappings.Add(concurrencyStampField, concurrencyStampField);
            bulkCopy.ColumnMappings.Add(extraPropertiesField, extraPropertiesField);

            await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
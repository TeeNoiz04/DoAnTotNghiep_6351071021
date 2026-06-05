using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;

namespace QuoteFlow.EntityFrameworkCore.Interceptors;
/// <summary>
/// Interceptor để xử lý lỗi String/Binary data truncation
/// Tự động convert sang BusinessException với thông tin chi tiết
/// </summary>
/// <summary>
/// Interceptor to handle String/Binary data truncation errors
/// Automatically converts to BusinessException with detailed information
/// </summary>
public class DataTruncationInterceptor : SaveChangesInterceptor
{
    private const string SQL_SERVER_TRUNCATE_ERROR = "String or binary data would be truncated";
    private const string POSTGRES_TRUNCATE_ERROR = "value too long for type";

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ValidateBeforeSave(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ValidateBeforeSave(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        HandleTruncationError(eventData.Exception, eventData.Context);
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        HandleTruncationError(eventData.Exception, eventData.Context);
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    /// <summary>
    /// Validate before save to detect truncation errors early
    /// </summary>
    private void ValidateBeforeSave(DbContext context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .ToList();

        foreach (var entry in entries)
        {
            var entityType = entry.Metadata;

            foreach (var property in entry.Properties)
            {
                if (property.CurrentValue == null) continue;

                var propertyMetadata = property.Metadata;

                // Check string properties
                if (property.CurrentValue is string stringValue)
                {
                    var maxLength = propertyMetadata.GetMaxLength();
                    if (maxLength.HasValue && stringValue.Length > maxLength.Value)
                    {
                        throw new BusinessException(
                            SystemSQL.MaxLengthExceeded,
                            $"The maximum length for field '{propertyMetadata.Name}' is {maxLength.Value}."
                        )
                        .WithData("fieldName", propertyMetadata.Name)
                        .WithData("entityName", entityType.DisplayName())
                        .WithData("maxLength", maxLength.Value)
                        .WithData("actualLength", stringValue.Length);
                    }
                }

                // Check binary properties
                if (property.CurrentValue is byte[] byteArray)
                {
                    var maxLength = propertyMetadata.GetMaxLength();
                    if (maxLength.HasValue && byteArray.Length > maxLength.Value)
                    {
                        throw new BusinessException(
                            SystemSQL.MaxLengthExceeded,
                            $"The maximum length for field '{propertyMetadata.Name}' is {maxLength.Value}."
                        )
                        .WithData("fieldName", propertyMetadata.Name)
                        .WithData("entityName", entityType.DisplayName())
                        .WithData("maxLength", maxLength.Value)
                        .WithData("actualLength", byteArray.Length);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handle exception when save failed
    /// </summary>
    private void HandleTruncationError(Exception exception, DbContext context)
    {
        if (exception == null) return;

        var dbException = FindDbException(exception);
        if (dbException == null) return;

        var errorMessage = dbException.Message;

        // Check if it's a truncation error
        if (errorMessage.Contains(SQL_SERVER_TRUNCATE_ERROR) ||
            errorMessage.Contains(POSTGRES_TRUNCATE_ERROR))
        {
            var entityInfo = ExtractTruncationDetails(errorMessage, context);

            if (entityInfo != null)
            {
                throw new BusinessException(
                    SystemSQL.MaxLengthExceeded,
                    $"The maximum length for field '{entityInfo.PropertyName}' is {entityInfo.MaxLength}."
                )
                .WithData("fieldName", entityInfo.PropertyName)
                .WithData("entityName", entityInfo.EntityName)
                .WithData("maxLength", entityInfo.MaxLength);
            }
            else
            {
                // Fallback when cannot determine specific field
                throw new BusinessException(
                    SystemSQL.MaxLengthExceeded,
                    "One or more fields exceed the maximum allowed length."
                );
            }
        }
    }

    /// <summary>
    /// Find DbException in exception chain
    /// </summary>
    private DbException FindDbException(Exception exception)
    {
        var current = exception;
        while (current != null)
        {
            if (current is DbException dbEx)
                return dbEx;

            current = current.InnerException;
        }
        return null;
    }

    /// <summary>
    /// Extract detailed information about truncation error
    /// </summary>
    private EntityPropertyInfo ExtractTruncationDetails(string errorMessage, DbContext context)
    {
        // Pattern to extract table and column name from SQL Server
        var sqlServerPattern = @"table '([^']+)'.*column '([^']+)'";
        var match = Regex.Match(errorMessage, sqlServerPattern, RegexOptions.IgnoreCase);

        if (match.Success)
        {
            var tableName = match.Groups[1].Value;
            var columnName = match.Groups[2].Value;

            // Find corresponding entity and property
            return FindEntityInfo(context, tableName, columnName);
        }

        return null;
    }

    /// <summary>
    /// Find entity and property information from table and column name
    /// </summary>
    private EntityPropertyInfo FindEntityInfo(DbContext context, string tableName, string columnName)
    {
        if (context == null) return null;

        var entityTypes = context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var table = entityType.GetTableName();
            if (table?.Equals(tableName, StringComparison.OrdinalIgnoreCase) != true)
                continue;

            foreach (var property in entityType.GetProperties())
            {
                var column = property.GetColumnName();
                if (column?.Equals(columnName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return new EntityPropertyInfo
                    {
                        EntityName = entityType.DisplayName(),
                        PropertyName = property.Name,
                        MaxLength = property.GetMaxLength() ?? 0
                    };
                }
            }
        }

        return null;
    }

    private class EntityPropertyInfo
    {
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public int MaxLength { get; set; }
    }
}

/// <summary>
/// SQL-related error codes
/// </summary>
public static class SystemSQL
{
    /// <summary>
    /// Error code for maximum length exceeded
    /// Message: "The maximum length for field '{fieldName}' is {maxLength}."
    /// </summary>
    public const string MaxLengthExceeded = "QuoteFlow:11204001";
}


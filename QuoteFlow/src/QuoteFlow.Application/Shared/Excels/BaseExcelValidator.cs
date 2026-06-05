using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;

namespace QuoteFlow.Shared.Excels;

public class BaseExcelValidator<TImportDto> : IExcelValidator<TImportDto>
{
    protected readonly ExcelValidationConfig _config;
    protected readonly IExcelRowValidator<TImportDto> _rowValidator;
    protected readonly ILogger<BaseExcelValidator<TImportDto>> _logger;

    public BaseExcelValidator(ExcelValidationConfig config, IExcelRowValidator<TImportDto> rowValidator, ILogger<BaseExcelValidator<TImportDto>> logger)
    {
        _config = config;
        _rowValidator = rowValidator;
        _logger = logger;
    }

    public virtual async Task<ExcelValidationResult<TImportDto>> ValidateAsync(Stream stream, string fileName, ExcelImportContext? context = null)
    {
        var result = new ExcelValidationResult<TImportDto>(singleRow: false, fileName);
        try
        {
            stream.Seek(0, SeekOrigin.Begin); // Rewind before reading

            var preValidationResult = await PreValidateAsync(stream, context);
            if (!preValidationResult.IsValid)
            {
                result.Errors.AddRange(preValidationResult.Errors);
                return result;
            }

            var rows = ReadRows(stream);
            await ValidateRowsAsync(rows, result);

            if (context is null)
            {
                await PostValidateAsync(result);
            }
            else
            {
                await PostValidateAsync(result, context);
            }
        }
        catch (UserFriendlyException ex)
        {
            _logger.LogError(ex, "Error processing Excel file");
            throw;
        }
        catch (Exception ex)
        {
            result.Errors.Add($"Error processing file: {ex.Message}");
            _logger.LogError(ex, "Error processing Excel file");
        }

        return result;
    }

    protected virtual async Task<ValidationResult> PreValidateAsync(Stream stream, ExcelImportContext? context)
    {
        var result = new ValidationResult();

        // Check if sheet exists
        // Check file format
        // Check basic structure

        return result;
    }

    protected virtual IEnumerable<IDictionary<string, object>> ReadRows(Stream stream)
    {
        IEnumerable<IDictionary<string, object>> rows;
        if (string.IsNullOrEmpty(_config.SheetName) && _config.GetStartCellConfigMethod() == StartCellConfigMethod.FixedStartCell)
        {
            rows = MiniExcelHelper.ReadExcelRows(stream, _config.StartCell!, _config.EndCell!, _config.HasHeader);
        }
        else if (string.IsNullOrEmpty(_config.SheetName) && _config.GetStartCellConfigMethod() == StartCellConfigMethod.DynamicHeader)
        {
            throw new NotSupportedException();
        }
        else if (_config.GetStartCellConfigMethod() == StartCellConfigMethod.FixedStartCell)
        {
            rows = MiniExcelHelper.ReadExcelRows(stream, _config.SheetName, _config.StartCell!, _config.EndCell!, _config.HasHeader);
        }
        else if (_config.GetStartCellConfigMethod() == StartCellConfigMethod.DynamicHeader)
        {
            rows = MiniExcelHelper.ReadExcelRows(stream, _config.SheetName, _config.SpecificHeader!, _config.StartColumn!, _config.EndColumn, _config.HasHeader);
        }
        else rows = [];


        if (!string.IsNullOrWhiteSpace(_config.StartColumn) && !string.IsNullOrWhiteSpace(_config.EndColumn))
        {
            rows = MiniExcelHelper.FilterBlankRows(rows, _config.StartColumn, _config.EndColumn, _config.StopColumn);
        }

        return rows;
    }
    protected virtual async Task ValidateRowsAsync(IEnumerable<IDictionary<string, object>> rows, ExcelValidationResult<TImportDto> result)
    {
        int rowIndex = 1;

        foreach (var row in rows)
        {
            try
            {
                var validation = _rowValidator.ValidateRow(row, rowIndex);

                List<TImportDto> parsedItems;

                if (_rowValidator is IExcelMultiRowValidator<TImportDto> multiRowValidator)
                {
                    parsedItems = multiRowValidator.ParseRows(row, rowIndex);
                }
                else
                {

                    parsedItems = new List<TImportDto> { _rowValidator.ParseRow(row) };
                }

                foreach (var item in parsedItems)
                {
                    var rowResult = new ExcelRowResult<TImportDto>
                    {
                        RowIndex = rowIndex,
                        RowData = item
                    };

                    if (!validation.IsValid)
                    {
                        rowResult.Errors.AddRange(validation.Errors);
                    }

                    result.ListData.Add(rowResult);
                    ExcelUtils.AddRowErrors(result, rowIndex, rowResult.Errors);
                }
            }
            catch (Exception ex)
            {
                var rowResult = new ExcelRowResult<TImportDto>
                {
                    RowIndex = rowIndex,
                    Errors = { $"Error processing row {rowIndex}: {ex.Message}" }
                };
                result.ListData.Add(rowResult);
                ExcelUtils.AddRowErrors(result, rowIndex, rowResult.Errors);
            }

            rowIndex++;
        }
    }



    protected virtual async Task PostValidateAsync(ExcelValidationResult<TImportDto> result)
    {
        // Override in derived classes for cross-row validations
    }

    protected virtual async Task PostValidateAsync(ExcelValidationResult<TImportDto> result, ExcelImportContext context)
    {
        // Override in derived classes for cross-row validations (with context)
    }
}

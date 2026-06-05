using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using static QuoteFlow.Shared.Excels.ExcelImportContextKeys;

namespace QuoteFlow.Shared.Excels;

public abstract class ExcelDtoConverter<TImportDto, TCreateParams> : IExcelDtoConverter<TImportDto, TCreateParams>
{
    protected readonly IObjectMapper _objectMapper;
    protected readonly IGuidGenerator _guidGenerator;

    protected ExcelDtoConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator)
    {
        _objectMapper = objectMapper;
        _guidGenerator = guidGenerator;
    }

    public virtual async Task<TCreateParams?> ConvertToCreateParamsAsync(ExcelRowResult<TImportDto> rowResult, ExcelImportContext context, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateRowAsync(rowResult, context, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new UserFriendlyException($"Row {rowResult.RowIndex} has validation errors: {string.Join(", ", validationResult.Errors)}");
        }

        return await MapToCreateParamsAsync(rowResult.RowData, context, cancellationToken);
    }

    /// <summary>
    /// Validates the row data and returns a <see cref="ValidationResult"/>.
    /// This method contains only business rule validations specific to the import logic.
    /// Basic structural checks (e.g., null row, invalid format) should be handled by implementing the <see cref="IExcelRowValidator{T}"/>.
    /// </summary>
    /// <param name="rowResult">The result object representing a single row of imported Excel data, including the DTO and any preliminary validation errors.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete, allowing the operation to be cancelled.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="ValidationResult"/> with any business logic validation errors.
    /// If no errors are found, the result will be valid.
    /// </returns>
    public virtual async Task<ValidationResult> ValidateRowAsync(ExcelRowResult<TImportDto> rowResult, ExcelImportContext context, CancellationToken cancellationToken = default)
    {
        var validationResult = new ValidationResult();

        if (rowResult.RowData == null)
        {
            validationResult.AddError("Row data is null.");
            return validationResult;
        }

        if (rowResult.HasErrors)
        {
            validationResult.AddErrors(rowResult.Errors);
            return validationResult;
        }

        var contextValidationResult = ValidateMissingContext(context);
        if (!contextValidationResult.IsValid)
        {
            validationResult.AddErrors(contextValidationResult.Errors);
            return validationResult;
        }

        return validationResult;
    }

    private ValidationResult ValidateMissingContext(
        ExcelImportContext context)
    {
        ValidationResult validationResult = new();

        foreach (var key in RequiredValidationContextKeys)
        {
            if (!KeyTypeMap.TryGetValue(key, out var type))
            {
                validationResult.AddError($"Unknown context key: {key}");
                continue;
            }

            var value = context.GetDataDynamic(type, key);

            bool isMissing = value == null ||
                (type.IsValueType && value.Equals(Activator.CreateInstance(type)));

            if (isMissing)
            {
                validationResult.AddError($"{key} is not provided in the context.");
            }
        }

        return validationResult;
    }

    protected abstract IEnumerable<string> RequiredValidationContextKeys { get; }
    protected abstract Task<TCreateParams?> MapToCreateParamsAsync(
        TImportDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken);
}
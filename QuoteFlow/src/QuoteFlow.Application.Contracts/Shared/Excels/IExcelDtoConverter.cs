using System.Threading;
using System.Threading.Tasks;

namespace QuoteFlow.Shared.Excels;

public interface IExcelDtoConverter<TImportDto, TCreateParams>
{
    Task<TCreateParams?> ConvertToCreateParamsAsync(ExcelRowResult<TImportDto> rowResult, ExcelImportContext context, CancellationToken cancellationToken = default);
    Task<ValidationResult> ValidateRowAsync(ExcelRowResult<TImportDto> rowResult, ExcelImportContext context, CancellationToken cancellationToken = default);
}
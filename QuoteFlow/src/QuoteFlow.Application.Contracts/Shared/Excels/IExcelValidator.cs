using System.IO;
using System.Threading.Tasks;

namespace QuoteFlow.Shared.Excels;

public interface IExcelValidator<T>
{
    Task<ExcelValidationResult<T>> ValidateAsync(Stream stream, string fileName, ExcelImportContext? context = null);
}
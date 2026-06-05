using System.Collections.Generic;
using System.Linq;
namespace QuoteFlow.Shared.Excels;
public class ExcelValidationResult<T>
{
    public bool HasErrors => Errors.Count != 0 || ListData.Any(d => d.HasErrors);
    public bool HasWarnings => Warnings?.Count > 0 || ListData?.Any(d => d.HasWarnings) == true;
    public bool IsValid => !HasErrors;
    public bool HasNotFoundWarnings => ListData?.Any(x => x.HasWarnings) == true;
    public bool SingleRow { get; private set; }
    public string FileName { get; set; }
    public List<string> Errors { get; set; }
    public List<string> Warnings { get; set; }
    public List<ExcelRowResult<T>> ListData { get; set; }
    public ExcelValidationResult(bool singleRow, string fileName)
    {
        SingleRow = singleRow;
        FileName = fileName;
        Errors = [];
        Warnings = [];
        ListData = [];
    }
    protected ExcelValidationResult()
    {
        SingleRow = false;
        Errors = [];
        Warnings = [];
        ListData = [];
    }
}
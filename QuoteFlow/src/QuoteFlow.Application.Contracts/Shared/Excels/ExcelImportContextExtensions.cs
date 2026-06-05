using System;

namespace QuoteFlow.Shared.Excels;

public static class ExcelImportContextExtensions
{
    public static object? GetDataDynamic(this ExcelImportContext context, Type type, string key)
    {
        var method = typeof(ExcelImportContext)
            .GetMethod(nameof(ExcelImportContext.GetData))?
            .MakeGenericMethod(type);

        return method?.Invoke(context, new object[] { key });
    }
}

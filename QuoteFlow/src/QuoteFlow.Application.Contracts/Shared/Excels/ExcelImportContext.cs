using System.Collections.Generic;

namespace QuoteFlow.Shared.Excels;

public class ExcelImportContext
{
    private Dictionary<string, object?> AdditionalData { get; set; } = [];

    public T? GetData<T>(string key)
    {
        ExcelImportContextKeys.IsExistingKey(key);
        return AdditionalData.TryGetValue(key, out var value) && value is not null ? (T)value : default;
    }

    public void SetData<T>(string key, T value)
    {
        ExcelImportContextKeys.ValidateKeyAndType(key, value); // Validate key and type
        AdditionalData[key] = value ?? default;
    }
}

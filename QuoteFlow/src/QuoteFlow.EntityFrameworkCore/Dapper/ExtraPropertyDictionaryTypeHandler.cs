using Dapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using Volo.Abp.Data;

namespace QuoteFlow.Dapper;

public class ExtraPropertyDictionaryTypeHandler : SqlMapper.TypeHandler<ExtraPropertyDictionary>
{
    public override ExtraPropertyDictionary? Parse(object value)
    {
        if (value is string stringValue)
        {
            try
            {
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(stringValue);
                return new ExtraPropertyDictionary(dictionary);
            }
            catch (JsonException)
            {
                // Log the error or handle it as appropriate for your application
                return new ExtraPropertyDictionary();
            }
        }
        return new ExtraPropertyDictionary();
    }

    public override void SetValue(IDbDataParameter parameter, ExtraPropertyDictionary? value)
    {
        parameter.Value = JsonConvert.SerializeObject(value);
    }
}
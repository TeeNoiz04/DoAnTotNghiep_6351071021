using System;
using System.Linq;

namespace QuoteFlow.Shared.Utils;

public class ModelTypeResolver
{
    public static Type ResolveModelType(string modelTypeName)
    {
        return Type.GetType(modelTypeName + ", QuoteFlow") ??
               AppDomain.CurrentDomain.GetAssemblies()
                   .SelectMany(a => a.GetTypes())
                   .FirstOrDefault(t => t.FullName == modelTypeName)
                   ?? throw new ArgumentException($"Could not resolve type: {modelTypeName}");
    }
}
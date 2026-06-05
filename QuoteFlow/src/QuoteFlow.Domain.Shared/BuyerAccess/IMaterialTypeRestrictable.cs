using System.Collections.Generic;

namespace QuoteFlow.BuyerAccess;

public interface IMaterialTypeRestrictable
{
    string? MaterialType { get; set; }
    List<string> RestrictedMaterialTypes { get; set; }
}

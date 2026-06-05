using System.Collections.Generic;

namespace QuoteFlow.Shared.Flagging;

public class BaseFlaggingContext
{
    public string CurrentUsername { get; set; } = null!;
    public IEnumerable<string> CurrentUserRoles { get; set; } = [];

    public Dictionary<string, object> AdditionalData { get; set; } = new();

    public BaseFlaggingContext()
    {

    }

    public BaseFlaggingContext(string currentUsername, IEnumerable<string>? roles = null, Dictionary<string, object>? additionalData = null)
    {
        CurrentUsername = currentUsername;

        CurrentUserRoles = roles ?? [];

        if (additionalData != null)
        {
            AdditionalData = additionalData;
        }
    }
}
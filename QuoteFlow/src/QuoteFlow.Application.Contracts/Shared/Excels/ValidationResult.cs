using System.Collections.Generic;
using System.Linq;

namespace QuoteFlow.Shared.Excels;

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<string> Errors { get; set; } = [];

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public void AddErrors(List<string> errors)
    {
        Errors.AddRange(errors);
    }
}

using System;
using System.Collections.Generic;

namespace QuoteFlow.RequesterContexts;

public interface IEffectiveUserContext
{
    Guid? Id { get; }
    string? Username { get; }
    string? FullName { get; }
    IEnumerable<string> Roles { get; }
}

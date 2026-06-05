using System;

namespace QuoteFlow.RequesterContexts;

public interface IRequesterContext
{
    Guid? Id { get; set; }
    string? Username { get; set; }
    string? FullName { get; set; }
}
using System;

namespace QuoteFlow.Shared.Models;

public class BaseCreateParams
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity. <br/>
    /// This is optionally set, allowing the creation of new entities without specifying an ID.
    /// </summary>
    public Guid? Id { get; set; }
}

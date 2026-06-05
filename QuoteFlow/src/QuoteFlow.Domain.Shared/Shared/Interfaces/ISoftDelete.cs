using System;

namespace QuoteFlow.Shared.Interfaces;

public interface ISoftDelete
{
    Guid? DeleterId { get; set; }

    string? DeleterUsername { get; set; }

    string? DeleterName { get; set; }

    DateTime? DeletionTime { get; set; }

    bool IsDeleted { get; set; }

    bool ForceDelete { get; set; }
}

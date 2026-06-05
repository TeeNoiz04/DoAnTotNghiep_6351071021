using JetBrains.Annotations;
using QuoteFlow.Materials.MaterialHistories.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.Materials.MaterialHistories;

public class MaterialHistory : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid MaterialId { get; set; }

    [CanBeNull]
    public virtual string? Action { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    protected MaterialHistory()
    {

    }

    public MaterialHistory(Guid id, Guid materialId, string? action = null, string? note = null)
    {

        Id = id;
        Check.Length(action, nameof(action), MaterialHistoryConsts.ActionMaxLength, 0);
        Check.Length(note, nameof(note), MaterialHistoryConsts.NoteMaxLength, 0);
        MaterialId = materialId;
        Action = action;
        Note = note;
    }

    public MaterialHistory(Guid id, MaterialHistoryCreateParams createParams)
    {
        Id = id;
        MaterialId = createParams.MaterialId;
        Action = createParams.Action;
        Note = createParams.Note;
    }

}
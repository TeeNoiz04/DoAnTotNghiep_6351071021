using JetBrains.Annotations;
using QuoteFlow.Materials.MaterialGroups.ParameterObject;
using QuoteFlow.Shared.Models;
using System;

namespace QuoteFlow.Materials.MaterialGroups;

public class MaterialGroup : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Code { get; set; }
    [NotNull]
    public virtual string Name { get; set; }
    [CanBeNull]
    public virtual Guid? Parent { get; set; }
    [NotNull]
    public virtual int SortOrder { get; set; }
    [CanBeNull]
    public virtual string? Note { get; set; }
    [NotNull]
    public virtual bool IsDeActive { get; set; }
    [CanBeNull]
    public string? MaterialType { get; set; }
    [CanBeNull]
    public virtual string? MaterialGroupPSI { get; set; }
    [NotNull]
    public virtual bool AllowKeyAccount { get; set; }
    public MaterialGroup()
    {

    }
    public MaterialGroup(Guid id, string code, string name, Guid? parent, int sortOrder, string? note = null, bool isDeActive = false, string? materialGroupPSI = null, bool allowKeyAccount = false, string? materialType = null)
    {
        Id = id;
        Code = code;
        Name = name;
        Parent = parent;
        SortOrder = sortOrder;
        Note = note;
        IsDeActive = isDeActive;
        MaterialGroupPSI = materialGroupPSI;
        AllowKeyAccount = allowKeyAccount;
        MaterialType = materialType;
    }
    public MaterialGroup(Guid id, MaterialGroupCreateParams createParams)
    {
        Id = id;
        Code = createParams.Code;
        Name = createParams.Name;
        Parent = createParams.Parent;
        SortOrder = createParams.SortOrder;
        Note = createParams.Note;
        IsDeActive = createParams.IsDeActive;
        MaterialType = createParams.MaterialType;
        MaterialGroupPSI = createParams.MaterialGroupPSI;
        AllowKeyAccount = createParams.AllowKeyAccount;
    }
}

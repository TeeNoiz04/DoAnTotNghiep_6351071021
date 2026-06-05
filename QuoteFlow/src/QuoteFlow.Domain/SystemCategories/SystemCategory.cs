using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.SystemCategories.ParameterObjects;
using System;
using Volo.Abp;

namespace QuoteFlow.SystemCategories;

public class SystemCategory : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid? ParentId { get; set; }

    [NotNull]
    public virtual string Code { get; set; }

    [NotNull]
    public virtual string Description { get; set; }

    public virtual decimal? Value { get; set; }

    [NotNull]
    public virtual string CategoryType { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public virtual bool IsDeactive { get; set; }

    public virtual int SortOrder { get; set; }

    protected SystemCategory()
    {

    }

    public SystemCategory(Guid id, string code, string description, string categoryType, bool isDeactive, int sortOrder, Guid? parentId = null, decimal? value = null, string? note = null)
    {

        Id = id;
        Check.NotNull(code, nameof(code));
        Check.Length(code, nameof(code), SystemCategoryConsts.CodeMaxLength, 0);
        Check.NotNull(description, nameof(description));
        Check.Length(description, nameof(description), SystemCategoryConsts.DescriptionMaxLength, 0);
        Check.NotNull(categoryType, nameof(categoryType));
        Check.Length(categoryType, nameof(categoryType), SystemCategoryConsts.CategoryTypeMaxLength, 0);
        Check.Length(note, nameof(note), QuoteFlowSharedConsts.NoteMaxLength, 0);
        Code = code;
        Description = description;
        CategoryType = categoryType;
        IsDeactive = isDeactive;
        SortOrder = sortOrder;
        ParentId = parentId;
        Value = value;
        Note = note;
    }
    public SystemCategory(Guid id, SystemCategoryCreateParams createParams)
    {
        Id = id;
        Code = createParams.Code;
        Description = createParams.Description;
        CategoryType = createParams.CategoryType;
        IsDeactive = false;
        SortOrder = 1;
        ParentId = createParams.ParentId;
        Value = createParams.Value;
        Note = createParams.Note;
    }

}
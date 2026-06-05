using JetBrains.Annotations;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.MaterialStockUploads.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace QuoteFlow.MaterialStockUploads;

public class MaterialStockUpload : ExtendedAuditedAggregateRoot<Guid>
{
    [CanBeNull]
    public virtual string? RequestNo { get; set; }

    [CanBeNull]
    public virtual string? ImportType { get; set; }

    [CanBeNull]
    public virtual string? FileName { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    [CanBeNull]
    public virtual string? Status { get; set; }

    public ICollection<MaterialStockUploadDetail>? MaterialStockUploadDetails { get; set; }

    protected MaterialStockUpload()
    {

    }

    public MaterialStockUpload(Guid id, string? requestNo = null, string? importType = null, string? filName = null, string? note = null, string? status = null)
    {

        Id = id;
        Check.Length(requestNo, nameof(requestNo), MaterialStockUploadConsts.RequestNoMaxLength, 0);
        Check.Length(importType, nameof(importType), MaterialStockUploadConsts.ImportTypeMaxLength, 0);
        Check.Length(filName, nameof(filName), MaterialStockUploadConsts.FilNameMaxLength, 0);
        Check.Length(note, nameof(note), MaterialStockUploadConsts.NoteMaxLength, 0);
        Check.Length(status, nameof(status), MaterialStockUploadConsts.StatusMaxLength, 0);
        RequestNo = requestNo;
        ImportType = importType;
        FileName = filName;
        Note = note;
        Status = status;
    }

    public MaterialStockUpload(Guid id, MaterialStockUploadCreateParams createParams)
    {
        Id = id;
        RequestNo = createParams.RequestNo;
        ImportType = createParams.ImportType;
        FileName = createParams.FileName;
        Note = createParams.Note;
        Status = createParams.Status;

    }

}
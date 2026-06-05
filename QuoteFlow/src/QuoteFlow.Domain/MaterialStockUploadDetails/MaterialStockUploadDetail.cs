using JetBrains.Annotations;
using QuoteFlow.MaterialStockUploadDetails.ParameterObjects;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace QuoteFlow.MaterialStockUploadDetails;

public class MaterialStockUploadDetail : AuditedAggregateRoot<Guid>
{
    public virtual Guid RequestId { get; set; }

    [NotNull]
    public virtual string MaterialCode { get; set; }

    [CanBeNull]
    public virtual string? Model { get; set; }

    [CanBeNull]
    public virtual string? Storage { get; set; }

    [CanBeNull]
    public virtual string? StorageDestination { get; set; }

    public virtual Guid? StorageDesc_Id { get; set; } = null;
    public virtual Guid? StorageSrc_Id { get; set; } = null;

    public virtual decimal? Qty { get; set; }

    [CanBeNull]
    public virtual string? RefDoc { get; set; }

    [CanBeNull]
    public virtual string? Remark { get; set; }

    protected MaterialStockUploadDetail()
    {

    }

    public MaterialStockUploadDetail(Guid id, Guid requestId, string materialCode, string? model = null, string? storage = null, string? storageDestination = null, decimal? qty = null, string? refDoc = null, string? remark = null)
    {

        Id = id;
        Check.NotNull(materialCode, nameof(materialCode));
        Check.Length(materialCode, nameof(materialCode), MaterialStockUploadDetailConsts.MaterialCodeMaxLength, 0);
        Check.Length(model, nameof(model), MaterialStockUploadDetailConsts.ModelMaxLength, 0);
        Check.Length(storage, nameof(storage), MaterialStockUploadDetailConsts.StorageMaxLength, 0);
        Check.Length(storageDestination, nameof(storageDestination), MaterialStockUploadDetailConsts.StorageDestinationMaxLength, 0);
        Check.Length(refDoc, nameof(refDoc), MaterialStockUploadDetailConsts.RefDocMaxLength, 0);
        RequestId = requestId;
        MaterialCode = materialCode;
        Model = model;
        Storage = storage;
        StorageDestination = storageDestination;
        Qty = qty;
        RefDoc = refDoc;
        Remark = remark;
    }

    public MaterialStockUploadDetail(Guid id, MaterialStockUploadDetailCreateParams createParams)
    {
        Id = id;
        RequestId = createParams.RequestId;
        MaterialCode = createParams.MaterialCode;
        Model = createParams.Model;
        Storage = createParams.Storage;
        StorageDestination = createParams.StorageDestination;
        Qty = createParams.Qty;
        RefDoc = createParams.RefDoc;
        Remark = createParams.Remark;
        StorageDesc_Id = createParams.StorageDesc_Id;
        StorageSrc_Id = createParams.StorageSrc_Id;
    }

}
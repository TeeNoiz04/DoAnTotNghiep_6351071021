using System;

namespace QuoteFlow.MaterialStockUploadDetails;
public class MaterialStockUploadDetailImportTransferDto
{
    public Guid RequestId { get; set; }
    public string MaterialCode { get; set; } = null!;
    public string? Model { get; set; }
    public string? Storage { get; set; }
    public string? StorageDestination { get; set; }
    public decimal? Qty { get; set; }
    public string? Remark { get; set; }
    public virtual Guid? StorageDesc_Id { get; set; } = null;
    public virtual Guid? StorageSrc_Id { get; set; } = null;

}

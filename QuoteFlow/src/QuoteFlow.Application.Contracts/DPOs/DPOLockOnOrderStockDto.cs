using QuoteFlow.Shared;
using System;

namespace QuoteFlow.DPOs;

public class DPOLockOnOrderStockDto : ExtendedAuditedEntityDto<Guid>
{
    public Guid PODetailId { get; set; }
    public string PONo { get; set; } = null!;
    public decimal POQty { get; set; }
    public DateTime? PODate { get; set; }
    public string? MachineNumber { get; set; }
    public string? STCReply { get; set; }
    public string Status { get; set; } = null!;
    public string? Note { get; set; }

    // From DPO Detail
    public virtual int? QtyLocked { get; set; }
    public virtual int? QtyImported { get; set; }
    public virtual int? QtyNeedImport { get; set; }
}

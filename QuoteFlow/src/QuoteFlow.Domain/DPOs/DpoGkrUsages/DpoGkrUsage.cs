using JetBrains.Annotations;
using QuoteFlow.DPOs.DpoGkrUsages.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;

namespace QuoteFlow.DPOs.DpoGkrUsages;

public class DpoGkrUsage : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual Guid GkrId { get; set; }

    [NotNull]
    public virtual Guid DpoId { get; set; }

    [NotNull]
    public virtual string GkrNo { get; set; }

    [NotNull]
    public virtual string DpoNo { get; set; }

    [NotNull]
    public virtual Guid GkrDetailId { get; set; }

    [NotNull]
    public virtual Guid DpoDetailId { get; set; }

    [NotNull]
    public virtual string GolfaCode { get; set; }

    [NotNull]
    public virtual string Model { get; set; }

    [NotNull]
    public virtual decimal GkrQty { get; set; }

    [NotNull]
    public virtual decimal DpoQty { get; set; }

    [NotNull]
    public virtual decimal GkrLockStockQty { get; set; }

    [NotNull]
    public virtual decimal DpoLockStockQty { get; set; }

    [NotNull]
    public virtual decimal GkrLockShipmentQty { get; set; }

    [NotNull]
    public virtual decimal DpoLockShipmentQty { get; set; }

    public DpoGkrUsage()
    {
    }

    public DpoGkrUsage(Guid id, DpoGkrUsageCreateParams createParams)
    {
        Id = id;
        GkrId = createParams.GkrId;
        DpoId = createParams.DpoId;
        GkrNo = createParams.GkrNo;
        DpoNo = createParams.DpoNo;
        GkrDetailId = createParams.GkrDetailId;
        DpoDetailId = createParams.DpoDetailId;
        GolfaCode = createParams.GolfaCode;
        Model = createParams.Model;
        GkrQty = createParams.GkrQty;
        DpoQty = createParams.DpoQty;
        GkrLockStockQty = createParams.GkrLockStockQty;
        DpoLockStockQty = createParams.DpoLockStockQty;
        GkrLockShipmentQty = createParams.GkrLockShipmentQty;
        DpoLockShipmentQty = createParams.DpoLockShipmentQty;
    }
}
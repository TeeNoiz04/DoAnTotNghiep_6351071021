using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs.DpoGkrUsages.ParameterObjects;

public class DpoGkrUsageCreateParams
{
    [Required]
    public Guid GkrId { get; set; }

    [Required]
    public Guid DpoId { get; set; }

    [Required]
    public string GkrNo { get; set; } = null!;

    [Required]
    public string DpoNo { get; set; } = null!;

    [Required]
    public Guid GkrDetailId { get; set; }

    [Required]
    public Guid DpoDetailId { get; set; }

    [Required]
    public string GolfaCode { get; set; } = null!;

    [Required]
    public string Model { get; set; } = null!;

    [Required]
    public decimal GkrQty { get; set; }

    [Required]
    public decimal DpoQty { get; set; }

    [Required]
    public decimal GkrLockStockQty { get; set; }

    [Required]
    public decimal DpoLockStockQty { get; set; }

    [Required]
    public decimal GkrLockShipmentQty { get; set; }

    [Required]
    public decimal DpoLockShipmentQty { get; set; }
}
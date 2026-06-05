using QuoteFlow.StockTracings;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.StockTracingDetails;

public class StockTracingDetailUpdateDto : IHasConcurrencyStamp
{
    [Required]
    public Guid StockTracingId { get; set; }
    public ReportType ReportType { get; set; }
    public int? RowNo { get; set; }
    [StringLength(StockTracingDetailConsts.PackingListCodeMaxLength)]
    public string? PackingListCode { get; set; }
    [StringLength(StockTracingDetailConsts.CheckListCodeMaxLength)]
    public string? CheckListCode { get; set; }
    public DateTime? DateEntered { get; set; }
    [StringLength(StockTracingDetailConsts.StockMaxLength)]
    public string? Stock { get; set; }
    [StringLength(StockTracingDetailConsts.BUMaxLength)]
    public string? BU { get; set; }
    [StringLength(StockTracingDetailConsts.CustomerMaxLength)]
    public string? Customer { get; set; }
    [StringLength(StockTracingDetailConsts.CategoryMaxLength)]
    public string? Category { get; set; }
    [StringLength(StockTracingDetailConsts.GIVMaxLength)]
    public string? GIV { get; set; }
    [StringLength(StockTracingDetailConsts.InvoiceMaxLength)]
    public string? Invoice { get; set; }
    [StringLength(StockTracingDetailConsts.SKUCodeMaxLength)]
    public string? SKUCode { get; set; }
    [StringLength(StockTracingDetailConsts.SKUNameMaxLength)]
    public string? SKUName { get; set; }
    [StringLength(StockTracingDetailConsts.QualityMaxLength)]
    public string? Quality { get; set; }
    [StringLength(StockTracingDetailConsts.WarrantyMaxLength)]
    public string? Warranty { get; set; }
    [StringLength(StockTracingDetailConsts.UnitMaxLength)]
    public string? Unit { get; set; }
    [StringLength(StockTracingDetailConsts.SeriesMaxLength)]
    public string? Series { get; set; }
    [StringLength(StockTracingDetailConsts.OriginCodeMaxLength)]
    public string? OriginCode { get; set; }
    public DateTime? ProductionDate { get; set; }
    [StringLength(StockTracingDetailConsts.LocationMaxLength)]
    public string? Location { get; set; }
    [StringLength(StockTracingDetailConsts.GolfaCodeMaxLength)]
    public string? GolfaCode { get; set; }
    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
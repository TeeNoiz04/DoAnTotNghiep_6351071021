using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.StockTracingDetails.ParameterObjects;
using QuoteFlow.StockTracings;
using System;
using Volo.Abp;

namespace QuoteFlow.StockTracingDetails;

public class StockTracingDetail : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid StockTracingId { get; set; }

    public virtual ReportType ReportType { get; set; }

    public virtual int? RowNo { get; set; }

    [CanBeNull]
    public virtual string? PackingListCode { get; set; }

    [CanBeNull]
    public virtual string? CheckListCode { get; set; }

    public virtual DateTime? DateEntered { get; set; }

    [CanBeNull]
    public virtual string? Stock { get; set; }

    [CanBeNull]
    public virtual string? BU { get; set; }

    [CanBeNull]
    public virtual string? Customer { get; set; }

    [CanBeNull]
    public virtual string? Category { get; set; }

    [CanBeNull]
    public virtual string? GIV { get; set; }

    [CanBeNull]
    public virtual string? Invoice { get; set; }

    [CanBeNull]
    public virtual string? SKUCode { get; set; }

    [CanBeNull]
    public virtual string? SKUName { get; set; }

    [CanBeNull]
    public virtual string? Quality { get; set; }

    [CanBeNull]
    public virtual string? Warranty { get; set; }

    [CanBeNull]
    public virtual string? Unit { get; set; }

    [CanBeNull]
    public virtual string? Series { get; set; }

    [CanBeNull]
    public virtual string? OriginCode { get; set; }

    public virtual DateTime? ProductionDate { get; set; }

    [CanBeNull]
    public virtual string? Location { get; set; }

    [CanBeNull]
    public virtual string? GolfaCode { get; set; }
    [CanBeNull]
    public virtual string? Note { get; set; }

    public StockTracing StockTracing { get; set; }
    protected StockTracingDetail()
    {

    }

    public StockTracingDetail(Guid id, Guid stockTracingId, ReportType reportType, int? rowNo = null, string? packingListCode = null, string? checkListCode = null, DateTime? dateEntered = null, string? stock = null, string? bU = null, string? customer = null, string? category = null, string? gIV = null, string? invoice = null, string? sKUCode = null, string? sKUName = null, string? quality = null, string? warranty = null, string? unit = null, string? series = null, string? originCode = null, DateTime? productionDate = null, string? location = null, string? golfaCode = null, string? note = null)
    {

        Id = id;
        Check.Length(packingListCode, nameof(packingListCode), StockTracingDetailConsts.PackingListCodeMaxLength, 0);
        Check.Length(checkListCode, nameof(checkListCode), StockTracingDetailConsts.CheckListCodeMaxLength, 0);
        Check.Length(stock, nameof(stock), StockTracingDetailConsts.StockMaxLength, 0);
        Check.Length(bU, nameof(bU), StockTracingDetailConsts.BUMaxLength, 0);
        Check.Length(customer, nameof(customer), StockTracingDetailConsts.CustomerMaxLength, 0);
        Check.Length(category, nameof(category), StockTracingDetailConsts.CategoryMaxLength, 0);
        Check.Length(gIV, nameof(gIV), StockTracingDetailConsts.GIVMaxLength, 0);
        Check.Length(invoice, nameof(invoice), StockTracingDetailConsts.InvoiceMaxLength, 0);
        Check.Length(sKUCode, nameof(sKUCode), StockTracingDetailConsts.SKUCodeMaxLength, 0);
        Check.Length(sKUName, nameof(sKUName), StockTracingDetailConsts.SKUNameMaxLength, 0);
        Check.Length(quality, nameof(quality), StockTracingDetailConsts.QualityMaxLength, 0);
        Check.Length(warranty, nameof(warranty), StockTracingDetailConsts.WarrantyMaxLength, 0);
        Check.Length(unit, nameof(unit), StockTracingDetailConsts.UnitMaxLength, 0);
        Check.Length(series, nameof(series), StockTracingDetailConsts.SeriesMaxLength, 0);
        Check.Length(originCode, nameof(originCode), StockTracingDetailConsts.OriginCodeMaxLength, 0);
        Check.Length(location, nameof(location), StockTracingDetailConsts.LocationMaxLength, 0);
        Check.Length(golfaCode, nameof(golfaCode), StockTracingDetailConsts.GolfaCodeMaxLength, 0);
        StockTracingId = stockTracingId;
        ReportType = reportType;
        RowNo = rowNo;
        PackingListCode = packingListCode;
        CheckListCode = checkListCode;
        DateEntered = dateEntered;
        Stock = stock;
        BU = bU;
        Customer = customer;
        Category = category;
        GIV = gIV;
        Invoice = invoice;
        SKUCode = sKUCode;
        SKUName = sKUName;
        Quality = quality;
        Warranty = warranty;
        Unit = unit;
        Series = series;
        OriginCode = originCode;
        ProductionDate = productionDate;
        Location = location;
        GolfaCode = golfaCode;
        Note = note;
    }
    public StockTracingDetail(Guid id, StockTracingDetailCreateParams createParams)
    {

        Id = id;
        StockTracingId = createParams.StockTracingId;
        ReportType = createParams.ReportType;
        RowNo = createParams.RowNo;
        PackingListCode = createParams.PackingListCode;
        CheckListCode = createParams.CheckListCode;
        DateEntered = createParams.DateEntered;
        Stock = createParams.Stock;
        BU = createParams.BU;
        Customer = createParams.Customer;
        Category = createParams.Category;
        GIV = createParams.GIV;
        Invoice = createParams.Invoice;
        SKUCode = createParams.SKUCode;
        SKUName = createParams.SKUName;
        Quality = createParams.Quality;
        Warranty = createParams.Warranty;
        Unit = createParams.Unit;
        Series = createParams.Series;
        OriginCode = createParams.OriginCode;
        ProductionDate = createParams.ProductionDate;
        Location = createParams.Location;
        GolfaCode = createParams.GolfaCode;
        Note = createParams.Note;
    }

}
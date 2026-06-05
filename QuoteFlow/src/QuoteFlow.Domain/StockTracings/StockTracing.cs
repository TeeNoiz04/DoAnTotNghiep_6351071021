using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.StockTracingDetails;
using QuoteFlow.StockTracings.ParameterObjects;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace QuoteFlow.StockTracings;

public class StockTracing : ExtendedAuditedAggregateRoot<Guid>
{
    [CanBeNull]
    public virtual string? FileName { get; set; }

    public virtual ReportType ReportType { get; set; }

    public virtual DateTime? FromDate { get; set; }

    public virtual DateTime? ToDate { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }
    public virtual ICollection<StockTracingDetail> Details { get; set; }

    protected StockTracing()
    {

    }

    public StockTracing(Guid id, ReportType reportType, string? fileName = null, DateTime? fromDate = null, DateTime? toDate = null, string? note = null)
    {

        Id = id;
        Check.Length(fileName, nameof(fileName), StockTracingConsts.FileNameMaxLength, 0);
        Check.Length(note, nameof(note), QuoteFlowSharedConsts.NoteMaxLength, 0);
        ReportType = reportType;
        FileName = fileName;
        FromDate = fromDate;
        ToDate = toDate;
        Note = note;
    }

    public StockTracing(Guid id, StockTracingCreateParams createParams)
    {

        Id = id;
        ReportType = createParams.ReportType;
        FileName = createParams.FileName;
        FromDate = createParams.FromDate;
        ToDate = createParams.ToDate;
        Note = createParams.Note;
    }

}
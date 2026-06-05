using System;

namespace QuoteFlow.Shared.Excels;

public class ExcelValidationConfig
{
    public string? SheetName { get; private set; }
    public string? StartCell { get; private set; }
    public string? EndCell { get; private set; }
    public bool HasHeader { get; private set; }
    public string StartColumn { get; private set; }
    public string EndColumn { get; private set; }
    public string? StopColumn { get; private set; }
    public string? SpecificHeader { get; private set; }

    protected static ExcelValidationConfig FromFixedStartCell(string sheetName, string startCell, string endCell, string startColumn, string endColumn, bool hasHeader = true, string? stopColumn = null)
    {
        if (string.IsNullOrWhiteSpace(startCell)) throw new ArgumentException("Start cell cannot be null or empty.", nameof(startCell));
        if (string.IsNullOrWhiteSpace(endCell)) throw new ArgumentException("End cell cannot be null or empty.", nameof(endCell));
        if (string.IsNullOrWhiteSpace(startColumn)) throw new ArgumentException("Start column cannot be null or empty.", nameof(startColumn));
        if (string.IsNullOrWhiteSpace(endColumn)) throw new ArgumentException("End column cannot be null or empty.", nameof(endColumn));

        return new ExcelValidationConfig
        {
            SheetName = sheetName,
            StartCell = startCell,
            EndCell = endCell,
            StartColumn = startColumn,
            EndColumn = endColumn,
            StopColumn = stopColumn,
            HasHeader = hasHeader
        };
    }

    protected static ExcelValidationConfig FromFixedStartCell(string startCell, string endCell, string startColumn, string endColumn, bool hasHeader = true, string? stopColumn = null)
    {
        if (string.IsNullOrWhiteSpace(startCell)) throw new ArgumentException("Start cell cannot be null or empty.", nameof(startCell));
        if (string.IsNullOrWhiteSpace(endCell)) throw new ArgumentException("End cell cannot be null or empty.", nameof(endCell));
        if (string.IsNullOrWhiteSpace(startColumn)) throw new ArgumentException("Start column cannot be null or empty.", nameof(startColumn));
        if (string.IsNullOrWhiteSpace(endColumn)) throw new ArgumentException("End column cannot be null or empty.", nameof(endColumn));

        return new ExcelValidationConfig
        {
            StartCell = startCell,
            EndCell = endCell,
            StartColumn = startColumn,
            EndColumn = endColumn,
            StopColumn = stopColumn,
            HasHeader = hasHeader
        };
    }

    protected static ExcelValidationConfig FromDynamicHeader(string sheetName, string specificHeader, string startColumn, string endColumn, bool hasHeader = true, string? stopColumn = null)
    {
        if (string.IsNullOrWhiteSpace(specificHeader)) throw new ArgumentException("Specific header cannot be null or empty.", nameof(specificHeader));
        if (string.IsNullOrWhiteSpace(startColumn)) throw new ArgumentException("Start column cannot be null or empty.", nameof(startColumn));
        if (string.IsNullOrWhiteSpace(endColumn)) throw new ArgumentException("End column cannot be null or empty.", nameof(endColumn));

        return new ExcelValidationConfig
        {
            SheetName = sheetName,
            SpecificHeader = specificHeader,
            StartColumn = startColumn,
            EndColumn = endColumn,
            StopColumn = stopColumn,
            HasHeader = hasHeader
        };
    }

    protected ExcelValidationConfig()
    {
        SheetName = string.Empty;
        HasHeader = true;
        StartColumn = "A";
        EndColumn = "Z";
    }

    public StartCellConfigMethod GetStartCellConfigMethod()
    {
        if (!string.IsNullOrWhiteSpace(StartCell))
        {
            return StartCellConfigMethod.FixedStartCell;
        }
        else if (!string.IsNullOrWhiteSpace(SpecificHeader) && !string.IsNullOrWhiteSpace(StartColumn))
        {
            return StartCellConfigMethod.DynamicHeader;
        }
        throw new InvalidOperationException("Invalid configuration: either StartCell or both of (SpecificHeader, StartColumn) must be set.");
    }

    protected void ApplyConfig(ExcelValidationConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        SheetName = config.SheetName;
        StartCell = config.StartCell;
        EndCell = config.EndCell;
        HasHeader = config.HasHeader;
        StartColumn = config.StartColumn;
        EndColumn = config.EndColumn;
        StopColumn = config.StopColumn;
        SpecificHeader = config.SpecificHeader;
    }
}

public enum StartCellConfigMethod
{
    FixedStartCell,
    DynamicHeader
}
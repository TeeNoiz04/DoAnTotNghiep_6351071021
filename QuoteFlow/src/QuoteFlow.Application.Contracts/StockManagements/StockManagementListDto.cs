using MiniExcelLibs.Attributes;
using System;
using System.Text.Json.Serialization;

namespace QuoteFlow.StockManagements;
public class StockManagementListDto
{

    [ExcelColumnWidth(20)]
    public string? GolfaCode { get; set; }
    [ExcelColumnWidth(20)]
    public string? Model { get; set; }
    [ExcelColumnWidth(20)]
    public string? Spec1 { get; set; }
    [ExcelColumnWidth(20)]
    public string? MaterialStatus { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal Standard_Price { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal Stock_Qty { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal Locked_Qty { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal LockStockSO_Qty { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal Available_Qty { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal Lockshipment_Qty { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal OnOderStock { get; set; }
    [ExcelIgnore]
    public Guid? StockCategoryId { get; set; }
    [ExcelIgnore]
    [JsonPropertyName("sap_Code")]
    public string? SAP_Code { get; set; }
    [ExcelIgnore]
    public string? Material_Group { get; set; }
    [ExcelIgnore]
    public string? ReferenceLeadTime { get; set; }

}

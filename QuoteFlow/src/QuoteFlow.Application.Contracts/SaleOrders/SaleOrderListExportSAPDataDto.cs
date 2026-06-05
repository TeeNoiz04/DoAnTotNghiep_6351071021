using MiniExcelLibs.Attributes;
using System;

namespace QuoteFlow.SaleOrders;
public class SaleOrderListExportSAPDataDto
{
    [ExcelColumnWidth(40)]
    public string? SONo { get; set; }
    [ExcelColumnWidth(40)]
    public string? BuyerName { get; set; }
    [ExcelColumnWidth(40)]
    public string? StockName { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal? SO_VAT { get; set; }
    [ExcelColumnWidth(40)]
    public string? MaterialType { get; set; }

    [ExcelColumnWidth(40)]
    public string? GolfaCode { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal? Qty { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal? Price { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0")]
    public decimal? Amount { get; set; }

    [ExcelColumnWidth(40)]
    public string? SAP_Code { get; set; }
    [ExcelColumnWidth(40)]
    public string? Model { get; set; }
    [ExcelColumnWidth(40)]
    public string? Material_Group { get; set; }
    [ExcelColumnWidth(40)]
    public string? Spec1 { get; set; }
    [ExcelColumnWidth(40)]
    public string? Spec2 { get; set; }
    [ExcelColumnWidth(40)]
    public string? Spec3 { get; set; }

    [ExcelColumnWidth(40)]
    public string? DPONo { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("dd/MM/yyyy")]
    public DateTime? OrderDate { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("dd/MM/yyyy")]
    public DateTime? RequestedETA { get; set; }
    [ExcelColumnWidth(40)]
    public string? CustomerName { get; set; }
    [ExcelColumnWidth(40)]
    public string? CustomerTaxCode { get; set; }

    [ExcelColumnWidth(40)]
    public string? SPOCode { get; set; }

    [ExcelColumnWidth(40)]
    public string? DPOCustomerName { get; set; }

    [ExcelColumnWidth(40)]
    public string? DPOCustomerTaxCode { get; set; }

    [ExcelColumnWidth(40)]
    public string? SPOType { get; set; }

    [ExcelColumnWidth(40)]
    public string? EUIndustryDescription { get; set; }

    [ExcelColumnWidth(40)]
    public string? CustomerType { get; set; }

    [ExcelColumnWidth(40)]
    public string? KeyAccountShortName { get; set; }
    [ExcelColumnWidth(40)]
    public string? SAPSONo { get; set; }

    [ExcelColumnWidth(40)]
    public string? SAPDONo { get; set; }

    [ExcelColumnWidth(40)]
    public string? SAPBillingNo { get; set; }

    [ExcelColumnWidth(40)]
    public string? SAPInvoiceNo { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("dd/MM/yyyy")]
    public DateTime? SAPInvoiceDate { get; set; }

}

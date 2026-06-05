using MiniExcelLibs.Attributes;
using System;

namespace QuoteFlow.Materials;

public class MaterialExportExcelDto
{
    [ExcelColumnWidth(20)]
    [ExcelFormat("dd/MM/yyyy")]
    [ExcelColumnName("Registration date")]
    public DateTime? RegistrationDate { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("dd/MM/yyyy")]
    [ExcelColumnName("Valid from")]
    public DateTime? ValidFrom { get; set; }
    [ExcelColumnWidth(20)]
    [ExcelFormat("dd/MM/yyyy")]
    [ExcelColumnName("Valid to")]
    public DateTime? ValidTo { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Material code")]
    public string GolfaCode { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Material code")]
    public string Model { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("SAP Code")]
    public string? SAP_Code { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Spec1")]
    public string? Spec1 { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Spec2")]
    public string? Spec2 { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Spec3")]
    public string? Spec3 { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Spec4")]
    public string? Spec4 { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Description EN")]
    public string? Description_EN { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Description VN")]
    public string? Description_VN { get; set; }


    [ExcelColumnWidth(20)]
    [ExcelColumnName("Material Type")]
    public string? MaterialType { get; set; }


    [ExcelColumnWidth(20)]
    [ExcelColumnName("Unit")]
    public string? Unit { get; set; }


    [ExcelColumnWidth(20)]
    [ExcelColumnName("Material Class")]
    public string? MaterialClass { get; set; }


    [ExcelColumnWidth(25)]
    [ExcelColumnName("Material  SEC Classification")]
    public string? Material_SEC_Classification { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelColumnName("Material Group")]
    public string? Material_Group { get; set; }


    [ExcelColumnWidth(25)]
    [ExcelColumnName("SAP Mat Group")]
    public string? SAPMatGroup { get; set; }


    [ExcelColumnWidth(30)]
    [ExcelColumnName("Product Hierarchy")]
    public string? Product_Hierarchy { get; set; }


    [ExcelColumnWidth(30)]
    [ExcelColumnName("Product Hierachy description")]
    public string? ProductHierarchyDescription { get; set; }


    [ExcelColumnWidth(25)]
    [ExcelColumnName("Country of Origin")]
    public string? CountryOfOrigin { get; set; }


    [ExcelColumnWidth(30)]
    [ExcelColumnName("Reference Lead Time (Working Day)")]
    public int? ReferenceLeadTime { get; set; }

    [ExcelColumnWidth(40)]
    [ExcelColumnName("Warranty time - month")]
    public int? WarrantyTime { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Inventory Category")]
    public string? InventoryCategory { get; set; }

    [ExcelColumnWidth(40)]
    [ExcelColumnName("Max lot")]
    public int? Maxlot { get; set; }

    [ExcelColumnWidth(40)]
    [ExcelColumnName("Stock Warning")]
    public int? StockWarning { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("VAT")]
    public decimal? VAT { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("HS Code")]
    public string? HS_Code { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Supplier")]
    public string? SupplierCode { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Supplier BU")]
    public string? SupplierBUCode { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("Factory")]
    public string? Factory_Text { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Input Price")]
    public decimal? Input_Price { get; set; }

    [ExcelColumnWidth(40)]
    [ExcelColumnName("Input Currency")]
    public string? InputCurrency { get; set; }


    [ExcelColumnWidth(40)]
    [ExcelColumnName("INCOTERMS")]
    public string? INCOTERMS { get; set; }

    [ExcelColumnWidth(40)]
    [ExcelColumnName("EPA")]
    public bool EPA { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Import duty")]
    public decimal? ImportDuty { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Applied exchange rate")]
    public decimal? AppliedExchangeRate { get; set; }

    [ExcelColumnWidth(35)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Landed Cost (VND)")]
    public decimal? LandedCost { get; set; }

    [ExcelColumnWidth(35)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Max Sales offer price (VND)")]
    public decimal? MaxSalesOfferPrice { get; set; }

    [ExcelColumnWidth(35)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Max Manager offer price (VND)")]
    public decimal? MaxMangerOfferPrice { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Standard Price")]
    public decimal Standard_Price { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Selling Price 1")]
    public decimal? SellingPrice1 { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Selling Price 2")]
    public decimal? SellingPrice2 { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Selling Price 3")]
    public decimal? SellingPrice3 { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Selling Price 4")]
    public decimal? SellingPrice4 { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Selling Price 5")]
    public decimal? SellingPrice5 { get; set; }


    [ExcelColumnWidth(20)]
    [ExcelColumnName("Material status")]
    public string MaterialStatus { get; set; }
}



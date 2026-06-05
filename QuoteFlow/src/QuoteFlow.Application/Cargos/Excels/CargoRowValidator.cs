using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Cargos.Excels;

public class CargoRowValidator : IExcelRowValidator<CargoImportDto>
{
    private const string DATE_TIME_FORMAT = "dd/MM/yyyy";


    public CargoRowValidator()
    {

    }
    public CargoImportDto ParseRow(IDictionary<string, object> rowData)
    {
        return new CargoImportDto
        {
            InvoiceNo = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            SRNo = ExcelParser.GetValue<string?>(rowData, "C")?.Trim(),
            PODetailCode = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            GolfaCode = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            MachineNo = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),
            Classification = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),
            Product = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),
            Model = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            Spec1 = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),
            Spec2 = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),
            Spec3 = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),
            OrderQty = ExcelParser.GetValue<string?>(rowData, "M")?.Trim(),
            ExWorkQty = ExcelParser.GetValue<string?>(rowData, "N")?.Trim(),
            NonExWorks = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),
            StockQuantity = ExcelParser.GetValue<string?>(rowData, "P")?.Trim(),
            Shipped = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim(),
            WaitForShip = ExcelParser.GetValue<string?>(rowData, "R")?.Trim(),
            ShipDate = ExcelParser.GetValue<DateTime?>(rowData, "S"),
            Order = ExcelParser.GetValue<DateTime?>(rowData, "T"),
            ShippingDate = ExcelParser.GetValue<DateTime?>(rowData, "U"),
            ShipmentMethod = ExcelParser.GetValue<string?>(rowData, "V")?.Trim(),
            PORef = ExcelParser.GetValue<string?>(rowData, "W")?.Trim(),
            ETA1 = ExcelParser.GetValue<DateTime?>(rowData, "X"),
            ETA2 = ExcelParser.GetValue<DateTime?>(rowData, "Y"),
            MEVNRequest = ExcelParser.GetValue<string?>(rowData, "Z")?.Trim(),
            STCReply = ExcelParser.GetValue<string?>(rowData, "AA")?.Trim(),
            EU = ExcelParser.GetValue<string?>(rowData, "AB")?.Trim(),
            MEVNAddedRequest = ExcelParser.GetValue<string?>(rowData, "AC")?.Trim(),
            SODate = ExcelParser.GetValue<string?>(rowData, "AD")?.Trim(),
            CellMarker = ExcelParser.GetValue<string?>(rowData, "AE")?.Trim(),
            ShippingForm = ExcelParser.GetValue<string?>(rowData, "AF")?.Trim()
        };


    }
    //public Guid GetPODetailId()
    //{
    //    return new Guid();
    //}


    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var invoiceNo = ExcelParser.GetValue<string?>(rowData, "B");
        var srNo = ExcelParser.GetValue<string?>(rowData, "C");
        var pODetailCode = ExcelParser.GetValue<string?>(rowData, "D");
        var golfaCode = ExcelParser.GetValue<string?>(rowData, "E");
        var machineNo = ExcelParser.GetValue<string?>(rowData, "F");
        var classification = ExcelParser.GetValue<string?>(rowData, "G");
        var product = ExcelParser.GetValue<string?>(rowData, "H");
        var model = ExcelParser.GetValue<string?>(rowData, "I");
        var spec1 = ExcelParser.GetValue<string?>(rowData, "J");
        var spec2 = ExcelParser.GetValue<string?>(rowData, "K");
        var spec3 = ExcelParser.GetValue<string?>(rowData, "L");
        var orderQty = ExcelParser.GetValue<string?>(rowData, "M");
        var exWorkQty = ExcelParser.GetValue<string?>(rowData, "N");
        var nonExWork = ExcelParser.GetValue<string?>(rowData, "O");         // NonExWorks
        var inSTCH = ExcelParser.GetValue<string?>(rowData, "P");            // StockQuantity
        var shipped = ExcelParser.GetValue<string?>(rowData, "Q");
        var waitForShip = ExcelParser.GetValue<string?>(rowData, "R");
        var shipDate = ExcelParser.GetValue<DateTime?>(rowData, "S");
        var orderDate = ExcelParser.GetValue<DateTime?>(rowData, "T");
        var inSTCHDate = ExcelParser.GetValue<DateTime?>(rowData, "U");  // ShippingDate
        var shipmentMethod = ExcelParser.GetValue<string?>(rowData, "V");
        var poRef = ExcelParser.GetValue<string?>(rowData, "W");
        var eta1 = ExcelParser.GetValue<DateTime?>(rowData, "X");
        var eta2 = ExcelParser.GetValue<DateTime?>(rowData, "Y");
        var mevnRequest = ExcelParser.GetValue<string?>(rowData, "Z");
        var stcReply = ExcelParser.GetValue<string?>(rowData, "AA");
        var eu = ExcelParser.GetValue<string?>(rowData, "AB");
        var mevnAddedRequest = ExcelParser.GetValue<string?>(rowData, "AC");
        var soDate = ExcelParser.GetValue<string?>(rowData, "AD");
        var cellMarker = ExcelParser.GetValue<string?>(rowData, "AE");
        var shippingForm = ExcelParser.GetValue<string?>(rowData, "AF");



        //if (string.IsNullOrWhiteSpace(invoiceNo))
        //    result.Errors.Add($"InvoiceNo is required.");
        //if (string.IsNullOrWhiteSpace(srNo))
        //    result.Errors.Add($"SRNo is required.");
        if (string.IsNullOrWhiteSpace(pODetailCode))
            result.Errors.Add($"PO SEQ is required.");
        if (string.IsNullOrWhiteSpace(golfaCode))
            result.Errors.Add($"Material Code is required.");
        //if (string.IsNullOrWhiteSpace(classification))
        //    result.Errors.Add($"Classfication Code is required.");
        //if (string.IsNullOrWhiteSpace(machineNo))
        //    result.Errors.Add($"Marchine No is required.");
        if (string.IsNullOrWhiteSpace(model))
            result.Errors.Add($"Model Name is required.");
        //if (string.IsNullOrWhiteSpace(product))
        //    result.Errors.Add($"Product Code is required.");

        if (!int.TryParse(orderQty, out int orderQtyNumber) || orderQtyNumber < 0)
            result.Errors.Add($"Order Qty must be a non-negative whole number.");

        //if (!int.TryParse(shipped, out int shippedNumber) || shippedNumber < 0)
        //    result.Errors.Add($"Shipped must be a non-negative whole number.");

        //if (!int.TryParse(waitForShip, out int waitForShipNumber) || waitForShipNumber < 0)
        //    result.Errors.Add($"Wait for ship must be a non-negative whole number.");

        if (shipDate.HasValue && shipDate.GetValueOrDefault() == DateTime.MinValue) //not required
            result.Errors.Add($"Ship Date is invalid.");

        if (orderDate.GetValueOrDefault() == DateTime.MinValue)
            result.Errors.Add($"Oder Date is required & invalid.");

        if (inSTCHDate.HasValue && inSTCHDate.GetValueOrDefault() == DateTime.MinValue)
            result.Errors.Add($"In STCH Date is invalid.");
        if (eta1.HasValue && eta1.GetValueOrDefault() == DateTime.MinValue)
            result.Errors.Add($"ETA1 is invalid.");
        if (eta2.HasValue && eta2.GetValueOrDefault() == DateTime.MinValue)
            result.Errors.Add($"ETA2 is invalid.");

        if (string.IsNullOrWhiteSpace(poRef))                                                       //required
            result.Errors.Add($"MEVN PO No is required.");
        //if (string.IsNullOrWhiteSpace(mevnRequest))                                               
        //    result.Errors.Add($"MEVN Request is required.");
        //if (string.IsNullOrWhiteSpace(eu))
        //    result.Errors.Add($"E/U is required.");
        //if (string.IsNullOrWhiteSpace(mevnAddedRequest))
        //    result.Errors.Add($"MEVN Added Request is required.");
        //if (soDate.HasValue && soDate.GetValueOrDefault() == DateTime.MinValue)
        //    result.Errors.Add($"SO Date is invalid.");


        return result;
    }
}

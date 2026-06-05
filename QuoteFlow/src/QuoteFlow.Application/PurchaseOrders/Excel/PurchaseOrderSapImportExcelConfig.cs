using QuoteFlow.PurchaseOrdersSapImports;
using QuoteFlow.Shared.Excels;

namespace QuoteFlow.PurchaseOrders.Excel;
internal class PurchaseOrderSapImportExcelConfig : ExcelValidationConfig
{
    public PurchaseOrderSapImportExcelConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: "Import_SAP_PO",
                startCell: PurchaseOrdersSapImportConsts.ImportStartCell,
                endCell: PurchaseOrdersSapImportConsts.ImportEndCell,
                startColumn: PurchaseOrdersSapImportConsts.ImportStartColumn,
                endColumn: PurchaseOrdersSapImportConsts.ImportEndColumn,
                hasHeader: false
            ));
    }
}
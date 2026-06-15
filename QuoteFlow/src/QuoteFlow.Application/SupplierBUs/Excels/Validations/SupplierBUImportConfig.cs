using QuoteFlow.Shared.Excels;

namespace QuoteFlow.SupplierBUs.Excels.Validations;
public class SupplierBUImportConfig : ExcelValidationConfig
{
    public SupplierBUImportConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: SupplierBUConsts.SupplierBUImport,
                startCell: SupplierBUConsts.SupplierBUImportStartCell,
                endCell: SupplierBUConsts.SupplierBUImportEndCell,
                startColumn: SupplierBUConsts.SupplierBUImportStartColumn,
                endColumn: SupplierBUConsts.SupplierBUImportEndColumn,
                hasHeader: false
            ));
    }
}

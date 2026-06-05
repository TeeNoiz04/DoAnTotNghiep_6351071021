using QuoteFlow.Shared.Excels;

namespace QuoteFlow.Customers.Excels.Validators;
public class ImportCustomerValidationConfig : ExcelValidationConfig
{
    public ImportCustomerValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: CustomerConsts.CustomerImport,
                startCell: CustomerConsts.CustomerImportStartCell,
                endCell: CustomerConsts.CustomerImportEndCell,
                startColumn: CustomerConsts.CustomerImportStartColumn,
                endColumn: CustomerConsts.CustomerImportEndColumn,
                hasHeader: false
            ));
    }
}

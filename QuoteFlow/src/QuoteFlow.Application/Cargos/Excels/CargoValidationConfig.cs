using QuoteFlow.Shared.Excels;

namespace QuoteFlow.Cargos.Excels;

public class CargoValidationConfig : ExcelValidationConfig
{
    public CargoValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: CargoConsts.CargoImport,
                startCell: CargoConsts.CargoImportStartCell,
                endCell: CargoConsts.CargoImportEndCell,
                startColumn: CargoConsts.CargoImportStartColumn,
                endColumn: CargoConsts.CargoImportEndColumn,
                hasHeader: false
            ));
    }
}

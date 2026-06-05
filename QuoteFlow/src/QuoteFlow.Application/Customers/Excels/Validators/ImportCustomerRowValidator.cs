using QuoteFlow.Shared.Excels;
using System.Collections.Generic;

namespace QuoteFlow.Customers.Excels.Validators;
public class ImportCustomerRowValidator : IExcelRowValidator<CustomerImportDto>
{

    public CustomerImportDto ParseRow(IDictionary<string, object> rowData)
    {
        return new CustomerImportDto
        {
            TaxCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim(),
            CustomerName = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            ShortName = ExcelParser.GetValue<string?>(rowData, "C")?.Trim(),
            Address = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            Phone = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            Country = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),
            Province = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),
            Website = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),
            CustomerType = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            CustomerIndustry = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),
            Note = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),
        };
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        // Get and trim the values from the specific columns
        var taxCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim();
        var customerName = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        var country = ExcelParser.GetValue<string?>(rowData, "F")?.Trim();

        // Perform required field validation
        if (string.IsNullOrWhiteSpace(taxCode))
            result.Errors.Add($"Tax Code is required.");

        if (string.IsNullOrWhiteSpace(customerName))
            result.Errors.Add($"Customer Name is required.");

        if (string.IsNullOrWhiteSpace(country))
            result.Errors.Add($"Country is required.");

        return result;
    }
}


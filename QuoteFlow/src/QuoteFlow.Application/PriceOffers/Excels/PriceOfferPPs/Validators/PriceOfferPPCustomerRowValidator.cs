using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Utils;
using System.Collections.Generic;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Validators;

public class PriceOfferPPCustomerRowValidator : IExcelRowValidator<PriceOfferCustomerImportDto>
{
    public PriceOfferPPCustomerRowValidator()
    {

    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();
        var salesChannel = ExcelParser.GetValue<string?>(rowData, "B");
        var taxCode = ExcelParser.GetValue<string?>(rowData, "C");
        var companyName = ExcelParser.GetValue<string?>(rowData, "D");
        var companyAddress = ExcelParser.GetValue<string?>(rowData, "E");
        var nationality = ExcelParser.GetValue<string?>(rowData, "F");
        var type = ExcelParser.GetValue<string?>(rowData, "G");
        var industry = ExcelParser.GetValue<string?>(rowData, "H");

        if (string.IsNullOrEmpty(salesChannel)) result.AddError("Sales channel is required.");
        if (string.IsNullOrEmpty(taxCode)) result.AddError("Tax code is required.");
        else
        {
            var taxCodeError = CodeHelper.ValidateTaxCode(taxCode);
            if (taxCodeError != null)
            {
                result.AddError(taxCodeError);
            }
        }
        if (string.IsNullOrEmpty(companyName)) result.AddError("Company official name is required.");
        if (string.IsNullOrEmpty(companyAddress)) result.AddError("Company official address is required.");
        if (string.IsNullOrEmpty(nationality)) result.AddError("Nationality is required.");
        if (string.IsNullOrEmpty(type)) result.AddError("Type is required.");
        if (string.IsNullOrEmpty(industry)) result.AddError("E/U Industry is required.");

        return result;
    }

    public PriceOfferCustomerImportDto ParseRow(IDictionary<string, object> rowData)
    {
        var salesChannel = ExcelParser.GetValue<string?>(rowData, "B");
        var taxCode = ExcelParser.GetValue<string?>(rowData, "C");
        var companyName = ExcelParser.GetValue<string?>(rowData, "D");
        var companyAddress = ExcelParser.GetValue<string?>(rowData, "E");
        var nationality = ExcelParser.GetValue<string?>(rowData, "F");
        var type = ExcelParser.GetValue<string?>(rowData, "G");
        var industry = ExcelParser.GetValue<string?>(rowData, "H");

        if (string.IsNullOrEmpty(taxCode))
        {
            taxCode = PriceOfferCustomerConsts.DefaultTaxCode;
        }

        return new PriceOfferCustomerImportDto()
        {
            SaleChannel = salesChannel,
            CustomerTaxCode = taxCode,
            CustomerName = companyName,
            CustomerAddress = companyAddress,
            CustomerNationality = nationality,
            CustomerType = type,
            CustomerIndustry = industry
        };
    }
}

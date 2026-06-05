using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.Customers.Excels.Converters;
public class CustomerImportUpdateConverter : ExcelDtoConverter<CustomerImportDto, CustomerUpdateParams>
{
    public CustomerImportUpdateConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    protected override async Task<CustomerUpdateParams> MapToCreateParamsAsync(
         CustomerImportDto importDto,
         ExcelImportContext context,
         CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return createParams;
    }

    private CustomerUpdateParams ToCreateParams(CustomerImportDto importDto, ExcelImportContext context)
    {
        var result = new CustomerUpdateParams
        {
            Id = importDto.IdUpdate!.Value,
            //TaxCode = importDto.TaxCode!, // Must be non-null due to [Required] attribute, assuming validation passed
            CustomerName = importDto.CustomerName!, // Must be non-null due to [Required] attribute, assuming validation passed

            CustomerShortName = importDto.ShortName,
            Address = importDto.Address,
            Phone = importDto.Phone,
            Country = importDto.Country,
            Province = importDto.Province,
            Website = importDto.Website,
            CustomerType = importDto.CustomerType,
            CustomerIndustry = importDto.CustomerIndustry,
            Note = importDto.Note,
            ConcurrencyStamp = importDto.ConcurrencyStamp,
        };
        return result;
    }
}

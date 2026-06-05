using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.Customers;

public interface ICustomersAppService : IApplicationService
{

    Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomersInput input);

    Task<CustomerDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<CustomerDto> CreateAsync(CustomerCreateDto input);
    Task<ExcelValidationResult<CustomerImportDto>> ValidateAndParseCustomerAsync(IRemoteStreamContent file);
    Task<List<CustomerDto>> ImportCustomerAsync(ExcelValidationResult<CustomerImportDto> dataImport);

    Task<CustomerDto> UpdateAsync(Guid id, CustomerUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(CustomerExcelDownloadDto input);

    Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

}
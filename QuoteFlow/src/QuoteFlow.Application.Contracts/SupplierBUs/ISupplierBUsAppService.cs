using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.SupplierBUs;

public interface ISupplierBUsAppService : IApplicationService
{

    Task<PagedResultDto<SupplierBUDto>> GetListAsync(GetSupplierBUsInput input);

    Task<SupplierBUDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<SupplierBUDto> CreateAsync(SupplierBUCreateDto input);

    Task<SupplierBUDto> UpdateAsync(Guid id, SupplierBUUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(SupplierBUExcelDownloadDto input);

    Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    Task<ExcelValidationResult<SupplierBUImportDto>> ValidateAndParseSupplierBUAsync(IRemoteStreamContent file);
    Task<List<SupplierBUDto>> ImportSupplierBUAsync(ExcelValidationResult<SupplierBUImportDto> dataImport);
    Task ChangeDeactiveSupplierBUAsync(List<Guid> ids);

}
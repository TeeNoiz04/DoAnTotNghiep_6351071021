using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.SalesAssignments;

public interface ISalesAssignmentsAppService : IApplicationService
{

    Task<PagedResultDto<SalesAssignmentDto>> GetListAsync(GetSalesAssignmentInput input);
    Task<PagedResultDto<SaleReportByCustomerDto>> GetListSaleReportDetailAsync(SaleReportInput input);
    Task<PagedResultDto<SaleReportByCustomerR05Dto>> GetListSaleReportGeneralAsync(SaleReportInput input);

    Task<SalesAssignmentDto> GetAsync(Guid id);
    Task<IRemoteStreamContent> GetListSaleReportDetailAsExcelAsync(SaleReportInput input);
    Task<IRemoteStreamContent> GetListSaleReportGeneralAsExcelAsync(SaleReportInput input);

    Task DeleteAsync(Guid id);

    Task<List<SalesAssignmentDto>> CreateAsync(SalesAssignmentCreateDto input);

    Task<SalesAssignmentDto> UpdateAsync(Guid id, SalesAssignmentUpdateDto input);

    Task<List<Shared.UserLookupDto>> GetListUserLookup(string name);
}

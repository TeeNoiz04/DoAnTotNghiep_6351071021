using Asp.Versioning;
using QuoteFlow.SalesAssignments;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.SalesAssignments;

[RemoteService]
[Area("app")]
[ControllerName("SalesAssignment")]
[Route("api/app/sales-assignments")]
public class SalesAssignmentController : AbpController, ISalesAssignmentsAppService
{
    protected ISalesAssignmentsAppService _salesAssignmentsAppService;

    public SalesAssignmentController(ISalesAssignmentsAppService SalesAssignmentsAppService)
    {
        _salesAssignmentsAppService = SalesAssignmentsAppService;
    }
    [HttpPost]
    public Task<List<SalesAssignmentDto>> CreateAsync(SalesAssignmentCreateDto input)
    {
        return _salesAssignmentsAppService.CreateAsync(input);
    }
    [HttpDelete]
    [Route("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _salesAssignmentsAppService.DeleteAsync(id);
    }
    [HttpGet]
    [Route("{id}")]
    public Task<SalesAssignmentDto> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    [HttpGet]
    public virtual Task<PagedResultDto<SalesAssignmentDto>> GetListAsync(GetSalesAssignmentInput input)
    {
        return _salesAssignmentsAppService.GetListAsync(input);
    }
    [HttpGet]
    [Route("user-lookups")]
    public Task<List<Shared.UserLookupDto>> GetListUserLookup([FromQuery] string name)
    {
        return _salesAssignmentsAppService.GetListUserLookup(name);
    }

    [HttpGet]
    [Route("report-sale")]
    public Task<IRemoteStreamContent> GetListSaleReportDetailAsExcelAsync(SaleReportInput input)
    {
        return _salesAssignmentsAppService.GetListSaleReportDetailAsExcelAsync(input);
    }
    [HttpGet]
    [Route("report-sale-data")]
    public Task<PagedResultDto<SaleReportByCustomerDto>> GetListSaleReportDetailAsync([FromQuery] SaleReportInput input)
    {
        return _salesAssignmentsAppService.GetListSaleReportDetailAsync(input);
    }
    [HttpGet]
    [Route("report-sale-general")]
    public Task<IRemoteStreamContent> GetListSaleReportGeneralAsExcelAsync(SaleReportInput input)
    {
        return _salesAssignmentsAppService.GetListSaleReportGeneralAsExcelAsync(input);
    }
    [HttpGet]
    [Route("report-sale-general-data")]
    public Task<PagedResultDto<SaleReportByCustomerR05Dto>> GetListSaleReportGeneralAsync([FromQuery] SaleReportInput input)
    {
        return _salesAssignmentsAppService.GetListSaleReportGeneralAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public Task<SalesAssignmentDto> UpdateAsync(Guid id, SalesAssignmentUpdateDto input)
    {
        return _salesAssignmentsAppService.UpdateAsync(id, input);
    }
}

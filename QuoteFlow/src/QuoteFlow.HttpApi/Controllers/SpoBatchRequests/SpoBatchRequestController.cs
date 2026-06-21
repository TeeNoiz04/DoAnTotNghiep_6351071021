using Asp.Versioning;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SpoBatchRequests;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.SpoBatchRequests;

[RemoteService]
[Area("app")]
[ControllerName("SpoBatchRequest")]
[Route("api/app/spo-batch-requests")]

public class SpoBatchRequestController : AbpController, ISpoBatchRequestsAppService
{
    protected ISpoBatchRequestsAppService _spoBatchRequestsAppService;

    public SpoBatchRequestController(ISpoBatchRequestsAppService spoBatchRequestsAppService)
    {
        _spoBatchRequestsAppService = spoBatchRequestsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<SpoBatchRequestDto>> GetListAsync(GetSpoBatchRequestsInput input)
    {
        return _spoBatchRequestsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<SpoBatchRequestDto> GetAsync(Guid id)
    {
        return _spoBatchRequestsAppService.GetAsync(id);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _spoBatchRequestsAppService.DeleteAsync(id);
    }

    [HttpPost]
    [Route("validate-and-parse-batch-request")]
    public virtual Task<ExcelValidationResult<SpoBatchRequestDetailImportDto>> ValidateAndParseBatchRequestAsync(IRemoteStreamContent file)
    {
        return _spoBatchRequestsAppService.ValidateAndParseBatchRequestAsync(file);
    }
    [HttpPost]
    [Route("import-batch-request")]
    public virtual Task ImportSPOBatchRequestAsync(ExcelValidationResult<SpoBatchRequestDetailImportDto> data, string? note)
    {
        return _spoBatchRequestsAppService.ImportSPOBatchRequestAsync(data, note);
    }
    [HttpDelete]
    [Route("{id}/delete-batch-request")]
    public virtual Task DeleteBatchRequestAsync(Guid id)
        => _spoBatchRequestsAppService.DeleteBatchRequestAsync(id);

    [HttpDelete]
    [Route("{id}/delete-items")]
    public virtual Task DeleteBatchRequestItemsAsync(Guid id, [FromBody] List<Guid> itemIds)
        => _spoBatchRequestsAppService.DeleteBatchRequestItemsAsync(id, itemIds);
}

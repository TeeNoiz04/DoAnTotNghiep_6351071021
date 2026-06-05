using QuoteFlow.Shared.Excels;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.SpoBatchRequests;

public interface ISpoBatchRequestsAppService : IApplicationService
{

    Task<PagedResultDto<SpoBatchRequestDto>> GetListAsync(GetSpoBatchRequestsInput input);

    Task<SpoBatchRequestDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);
    Task DeleteBatchRequestAsync(Guid id);
    Task DeleteBatchRequestItemsAsync(Guid batchRequestId, List<Guid> itemIds);
    Task<ExcelValidationResult<SpoBatchRequestDetailImportDto>> ValidateAndParseBatchRequestAsync(IRemoteStreamContent file);
    Task ImportSPOBatchRequestAsync(ExcelValidationResult<SpoBatchRequestDetailImportDto> data, string? note);
}
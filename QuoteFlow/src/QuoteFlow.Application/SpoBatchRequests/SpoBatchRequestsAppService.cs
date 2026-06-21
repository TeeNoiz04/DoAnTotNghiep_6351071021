using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using QuoteFlow.SpoBatchRequests.ParameterObject;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.ParameterObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Uow;

namespace QuoteFlow.SpoBatchRequests;

[RemoteService(IsEnabled = false)]

public class SpoBatchRequestsAppService : QuoteFlowAppService, ISpoBatchRequestsAppService
{

    protected ISpoBatchRequestRepository _spoBatchRequestRepository;
    protected SpoBatchRequestManager _spoBatchRequestManager;
    protected SpoBatchRequestDetailManager _spoBatchRequestDetailManager;
    protected IExcelImportFactory _excelImportFactory;
    protected ISpoBatchRequestDetailRepository _spoBatchRequestDetailRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    public SpoBatchRequestsAppService(ISpoBatchRequestRepository spoBatchRequestRepository, SpoBatchRequestManager spoBatchRequestManager, SpoBatchRequestDetailManager spoBatchRequestDetailManager, ISpoBatchRequestDetailRepository spoBatchRequestDetailRepository, IExcelImportFactory excelImportFactory, IUnitOfWorkManager unitOfWorkManager)
    {

        _spoBatchRequestRepository = spoBatchRequestRepository;
        _spoBatchRequestManager = spoBatchRequestManager;
        _spoBatchRequestDetailManager = spoBatchRequestDetailManager;
        _spoBatchRequestDetailRepository = spoBatchRequestDetailRepository;
        _excelImportFactory = excelImportFactory;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public virtual async Task<PagedResultDto<SpoBatchRequestDto>> GetListAsync(GetSpoBatchRequestsInput input)
    {
        var filterParams = ObjectMapper.Map<GetSpoBatchRequestsInput, SpoBatchRequestFilterParams>(input);
        var totalCount = await _spoBatchRequestRepository.GetCountAsync(filterParams);
        var items = await _spoBatchRequestRepository.GetListAsync(filterParams);

        return new PagedResultDto<SpoBatchRequestDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<SpoBatchRequest>, List<SpoBatchRequestDto>>(items)
        };
    }

    public virtual async Task<SpoBatchRequestDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<SpoBatchRequest, SpoBatchRequestDto>(await _spoBatchRequestRepository.GetAsync(id));
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        await _spoBatchRequestRepository.DeleteAsync(id);
    }



    public virtual async Task<ExcelValidationResult<SpoBatchRequestDetailImportDto>> ValidateAndParseBatchRequestAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<SpoBatchRequestDetailImportDto>(ExcelImporters.BatchRequest);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");
        return result;
    }


    [UnitOfWork(IsDisabled = true)]
    public async Task ImportSPOBatchRequestAsync(ExcelValidationResult<SpoBatchRequestDetailImportDto> data, string? note)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
        {
            var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<SpoBatchRequestDetailImportDto, SpoBatchRequestDetailCreateParams>(ExcelImporters.BatchRequest);
            var createParamBatchRequestUploads = new SpoBatchRequestCreateParams()
            {
                Note = note,
                FileName = data.FileName,
                ImportType = "BATCH_REQUEST",
                Status = QuoteFlowStatuses.InProgress,
            };

            var batchRequest = await _spoBatchRequestManager.CreateAsync(createParamBatchRequestUploads);
            var context = new ExcelImportContext();
            context.SetData(ExcelImportContextKeys.ParentEntityId, batchRequest.Id);

            List<SpoBatchRequestDetailCreateParams> createParams = (await Task.WhenAll(
                data.ListData
                    .Select(x => createParamsConverters.ConvertToCreateParamsAsync(x, context, default))
            )).Where(x => x != null)
            .ToList()!;


            await _spoBatchRequestDetailManager.CreateBatchAsync(createParams);


            await _spoBatchRequestRepository.GetBatchUpdateAsync(batchRequest.Id);

        }
    }
    public virtual async Task DeleteBatchRequestAsync(Guid id)
    {
        var spoBatchRequest = await _spoBatchRequestRepository.GetAsync(id, includeDetails: true);
        var details = spoBatchRequest.SpoBatchRequestDetails.ToList();
        var today = Clock.Now.Date;

        var executedItems = details
            .Where(d => d.ActionDate.HasValue && d.ActionDate.Value.Date <= today)
            .ToList();

        var notExecutedItems = details
            .Where(d => !d.ActionDate.HasValue || d.ActionDate.Value.Date > today)
            .ToList();

        if (!executedItems.Any())
        {
            foreach (var detail in details)
                detail.IsDeleted = true;

            spoBatchRequest.IsDeleted = true;
        }
        else if (executedItems.Any() && notExecutedItems.Any())
        {
            foreach (var item in notExecutedItems)
                item.IsDeleted = true;

            spoBatchRequest.Status = QuoteFlowStatuses.Closed;
        }
        else
        {
            throw new UserFriendlyException("This SPO Batch Request has been fully executed and cannot be deleted.");
        }

        await _spoBatchRequestRepository.UpdateAsync(spoBatchRequest);
    }

    public virtual async Task DeleteBatchRequestItemsAsync(Guid batchRequestId, List<Guid> itemIds)
    {
        var spoBatchRequest = await _spoBatchRequestRepository.GetAsync(batchRequestId, includeDetails: true);
        var details = spoBatchRequest.SpoBatchRequestDetails.ToList();
        var today = Clock.Now.Date;

        var selectedItems = details.Where(d => itemIds.Contains(d.Id)).ToList();

        var invalidItems = selectedItems
            .Where(d => d.ActionDate.HasValue && d.ActionDate.Value.Date <= today)
            .ToList();

        if (invalidItems.Any())
            throw new UserFriendlyException("Only items that have not been executed can be deleted.");

        foreach (var item in selectedItems)
            item.IsDeleted = true;

        var remainingItems = details
            .Where(d => !itemIds.Contains(d.Id) && d.IsDeleted != true)
            .ToList();

        var remainingNotExecuted = remainingItems
            .Where(d => !d.ActionDate.HasValue || d.ActionDate.Value.Date > today)
            .ToList();

        var hasRemainingExecuted = remainingItems
            .Any(d => d.ActionDate.HasValue && d.ActionDate.Value.Date <= today);

        if (!remainingItems.Any())
        {
            spoBatchRequest.IsDeleted = true;
        }
        else if (!remainingNotExecuted.Any() && hasRemainingExecuted)
        {
            spoBatchRequest.Status = QuoteFlowStatuses.Closed;
        }

        await _spoBatchRequestRepository.UpdateAsync(spoBatchRequest);
    }
}

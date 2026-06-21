using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.ParameterObject;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.Params;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
public class SpoBatchRequestDetailManager : DomainService
{
    protected ISpoBatchRequestDetailRepository _spoBatchRequestDetailRepository;

    public SpoBatchRequestDetailManager(ISpoBatchRequestDetailRepository spoBatchRequestDetailRepository)
    {
        _spoBatchRequestDetailRepository = spoBatchRequestDetailRepository;
    }

    public virtual async Task<SpoBatchRequestDetail> CreateAsync(SpoBatchRequestDetailCreateParams input)
    {


        var entity = new SpoBatchRequestDetail(
            GuidGenerator.Create(),
            input
        );

        return await _spoBatchRequestDetailRepository.InsertAsync(entity);
    }

    public virtual async Task<SpoBatchRequestDetail> UpdateAsync(Guid id, SpoBatchRequestDetailUpdateParams input)
    {

        var entity = await _spoBatchRequestDetailRepository.GetAsync(id);

        entity.RequestId = input.RequestId;
        entity.SPOCode = input.SPOCode;
        entity.GolfaCode = input.GolfaCode;
        entity.Action = input.Action;
        entity.ActionDate = input.ActionDate;
        entity.Note = input.Note;

        entity.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        return await _spoBatchRequestDetailRepository.UpdateAsync(entity);
    }
    public virtual async Task CreateBatchAsync(IEnumerable<SpoBatchRequestDetailCreateParams> createParamsList)
    {
        var spoBatchRequestDetailList = new List<SpoBatchRequestDetail>();
        foreach (var createParams in createParamsList)
        {
            var spoBatchRequestDetail = new SpoBatchRequestDetail(GuidGenerator.Create(), createParams);
            spoBatchRequestDetailList.Add(spoBatchRequestDetail);
        }

        await _spoBatchRequestDetailRepository.InsertManyAsync(spoBatchRequestDetailList, autoSave: true);
    }
}

using JetBrains.Annotations;
using QuoteFlow.DistributorTargets.ParameterObjects;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.DistributorTargets;

public class DistributorTargetManager : DomainService
{
    protected IDistributorTargetRepository _distributorTargetRepository;

    public DistributorTargetManager(IDistributorTargetRepository distributorTargetRepository)
    {
        _distributorTargetRepository = distributorTargetRepository;
    }

    public virtual async Task<DistributorTarget> CreateAsync(
    DistributorTargetCreateParams createParams)
    {

        var existingTarget = await _distributorTargetRepository.FirstOrDefaultAsync(x => x.BuyerTypeId == createParams.BuyerTypeId && x.BuyerId == createParams.BuyerId && x.FinanceYear == createParams.FinanceYear && x.MaterialType == createParams.MaterialType);
        if (existingTarget != null)
        {
            throw new UserFriendlyException("A target for this buyer and finance year already exists.");
        }
        var distributorTarget = new DistributorTarget(
         GuidGenerator.Create(),
         createParams
         );

        return await _distributorTargetRepository.InsertAsync(distributorTarget);
    }

    public virtual async Task<DistributorTarget> UpdateAsync(
        Guid id, DistributorTargetUpdateParams updateParams, [CanBeNull] string? concurrencyStamp = null
    )
    {

        var distributorTarget = await _distributorTargetRepository.GetAsync(id);

        distributorTarget.BuyerId = updateParams.BuyerId;
        distributorTarget.MaterialType = updateParams.MaterialType;
        //distributorTarget.BuyerTypeId = updateParams.BuyerTypeId;
        distributorTarget.BuyerCode = updateParams.BuyerCode;
        //distributorTarget.BuyerName = updateParams.BuyerName;
        distributorTarget.FinanceYear = updateParams.FinanceYear;
        distributorTarget.FirstFYTarget = updateParams.FirstFYTarget;
        distributorTarget.SecondFYTarget = updateParams.SecondFYTarget;
        distributorTarget.Note = updateParams.Note;

        distributorTarget.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _distributorTargetRepository.UpdateAsync(distributorTarget);
    }

}
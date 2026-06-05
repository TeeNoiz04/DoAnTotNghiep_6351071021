using QuoteFlow.ApprovalHistories.ParameterObjects;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.ApprovalHistories;

public class ApprovalHistoryManager : DomainService
{
    protected IApprovalHistoryRepository _approvalHistoryRepository;

    public ApprovalHistoryManager(IApprovalHistoryRepository approvalHistoryRepository)
    {
        _approvalHistoryRepository = approvalHistoryRepository;
    }

    public virtual async Task<ApprovalHistory> CreateAsync(
     ApprovalHistoryCreateParams createParams)
    {

        var approvalHistory = new ApprovalHistory(
         GuidGenerator.Create(),
         createParams);

        return await _approvalHistoryRepository.InsertAsync(approvalHistory);
    }


    public virtual async Task<ApprovalHistory> UpdateAsync(
    Guid id,
    ApprovalHistoryUpdateParams updateParams)
    {
        var approvalHistory = await _approvalHistoryRepository.GetAsync(id);

        approvalHistory.Action = updateParams.Action;
        approvalHistory.ActionDate = updateParams.ActionDate;
        approvalHistory.IsLastApprovalInCurrentWorkflow = updateParams.IsLastApprovalInCurrentWorkflow;
        approvalHistory.EntityType = updateParams.EntityType;
        approvalHistory.ApproverRoleCode = updateParams.ApproverRoleCode;
        approvalHistory.ApproverRoleName = updateParams.ApproverRoleName;
        approvalHistory.ApproverUsername = updateParams.ApproverUsername;
        approvalHistory.ApproverFullName = updateParams.ApproverFullName;
        approvalHistory.Note = updateParams.Note;

        approvalHistory.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _approvalHistoryRepository.UpdateAsync(approvalHistory, autoSave: true);
    }


}
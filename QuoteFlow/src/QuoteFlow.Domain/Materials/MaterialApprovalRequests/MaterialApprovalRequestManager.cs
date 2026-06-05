using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
using QuoteFlow.RequesterContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace QuoteFlow.Materials.MaterialApprovalRequests;

public class MaterialApprovalRequestManager : DomainService
{
    protected IMaterialApprovalRequestRepository _materialApprovalRequestRepository;
    protected readonly ICurrentUser _currentUserFromToken;
    protected readonly IRequesterContext _currentUserFromHeader;
    protected readonly IIdentityUserRepository _identityUserRepository;
    protected readonly IApprovalHistoryRepository _approvalHistoryRepository;
    protected IdentityUser? _currentUser;
    public MaterialApprovalRequestManager(IMaterialApprovalRequestRepository materialApprovalRequestRepository, IRequesterContext currentUserFromHeader, ICurrentUser currentUserFromToken, IIdentityUserRepository identityUserRepository, IApprovalHistoryRepository approvalHistoryRepository)
    {
        _materialApprovalRequestRepository = materialApprovalRequestRepository;
        _currentUserFromHeader = currentUserFromHeader;
        _currentUserFromToken = currentUserFromToken;

        _identityUserRepository = identityUserRepository;
        _approvalHistoryRepository = approvalHistoryRepository;
    }

    public async Task<string> GetNewRequestNoAsync(string prefix)
    {
        var lastestRequestNo = await _materialApprovalRequestRepository.GetLastestRequestNoAsync(prefix);
        var newRequestNo = MaterialHelper.IncreaseRequestNo(lastestRequestNo, prefix);
        return newRequestNo;
    }
    public virtual async Task<MaterialApprovalRequest> CreateAsync(
    MaterialApprovalRequestCreateParams createParams)
    {
        string newRequestNo = await GetNewRequestNoAsync(createParams.ImportType);
        var materialApprovalRequest = new MaterialApprovalRequest(
         GuidGenerator.Create(),
         newRequestNo,
         createParams
         );

        return await _materialApprovalRequestRepository.InsertAsync(materialApprovalRequest, autoSave: true);
    }

    public virtual async Task RecordActionAsync(MaterialApprovalRequest request, ApprovalHistoryCreateParams historyCreateParams)
    {
        var history = new MaterialApprovalRequestHistory(GuidGenerator.Create(), request.Id, historyCreateParams);

        await _approvalHistoryRepository.InsertAsync(history, autoSave: true);


    }

    public virtual async Task<MaterialApprovalRequest> UpdateAsync(
        Guid id,
        MaterialApprovalRequestUpdateParams updateParams
    )
    {

        var materialApprovalRequest = await _materialApprovalRequestRepository.GetAsync(id);

        materialApprovalRequest.ImportType = updateParams.ImportType;
        materialApprovalRequest.RequestNo = updateParams.RequestNo;
        materialApprovalRequest.FileName = updateParams.FileName;
        materialApprovalRequest.Note = updateParams.Note;
        materialApprovalRequest.Status = updateParams.Status;


        materialApprovalRequest.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _materialApprovalRequestRepository.UpdateAsync(materialApprovalRequest);
    }
    public async Task<IEnumerable<ValidationResult>> ValidateMaterialApprovalRequestSubmissionAsync(MaterialApprovalRequestSubmitParams submitParams, MaterialApprovalRequest material)
    {
        var results = new List<ValidationResult>();


        return results;
    }

    public async Task<IEnumerable<ValidationResult>> ValidateRequestActionAsync(MaterialApprovalRequest request)
    {
        var results = new List<ValidationResult>();



        return results;
    }
    protected async Task<IdentityUser> GetCurrentUserAsync()
    {
        var currentUserName = _currentUserFromHeader.Username ?? _currentUserFromToken.UserName;

        var result = (await _identityUserRepository.GetListAsync(userName: currentUserName)).FirstOrDefault()
            ?? throw new EntityNotFoundException($"Cannot find user with username {currentUserName}");

        return result;
    }


}
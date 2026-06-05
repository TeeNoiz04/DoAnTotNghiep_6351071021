using JetBrains.Annotations;
using QuoteFlow.SystemCategories.ParameterObjects;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SystemCategories;

public class SystemCategoryManager : DomainService
{
    protected ISystemCategoryRepository _systemCategoryRepository;

    public SystemCategoryManager(ISystemCategoryRepository systemCategoryRepository)
    {
        _systemCategoryRepository = systemCategoryRepository;
    }


    public virtual async Task<SystemCategory> CreateAsync(SystemCategoryCreateParams createParams)
    {
        await CheckCodeAsync(createParams.Code, createParams.CategoryType);
        var systemCategory = new SystemCategory(
         GuidGenerator.Create(),
         createParams
         );

        return await _systemCategoryRepository.InsertAsync(systemCategory);
    }


    public virtual async Task<SystemCategory> UpdateAsync(
        Guid id,
        SystemCategoryUpdateParams updateParams, [CanBeNull] string? concurrencyStamp = null
    )
    {
        var systemCategory = await _systemCategoryRepository.GetAsync(id);

        systemCategory.Description = updateParams.Description;
        systemCategory.CategoryType = updateParams.CategoryType;
        systemCategory.IsDeactive = updateParams.IsDeactive;
        systemCategory.SortOrder = 1;
        systemCategory.ParentId = updateParams.ParentId;
        systemCategory.Value = updateParams.Value;
        systemCategory.Note = updateParams.Note;

        systemCategory.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _systemCategoryRepository.UpdateAsync(systemCategory);
    }

    private async Task CheckCodeAsync(string code, string type)
    {
        var categoryExists = await _systemCategoryRepository.AnyAsync(x => x.Code == code && x.CategoryType == type);
        if (categoryExists)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.SystemCategory.SystemCategoryCodeExists);
        }
    }

}
using QuoteFlow.Buyers;
using QuoteFlow.DistributorTargets.ParameterObjects;
using QuoteFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;

namespace QuoteFlow.DistributorTargets;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.FAAdmins.BuyerTarget)]
public class DistributorTargetsAppService : QuoteFlowAppService, IDistributorTargetsAppService
{
    protected IDistributedCache<DistributorTargetDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IDistributorTargetRepository _distributorTargetRepository;
    protected IBuyerRepository _buyerRepository;
    protected DistributorTargetManager _distributorTargetManager;

    public DistributorTargetsAppService(IDistributorTargetRepository distributorTargetRepository, DistributorTargetManager distributorTargetManager, IDistributedCache<DistributorTargetDownloadTokenCacheItem, string> downloadTokenCache, IBuyerRepository buyerRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _distributorTargetRepository = distributorTargetRepository;
        _distributorTargetManager = distributorTargetManager;
        _buyerRepository = buyerRepository;
    }

    public virtual async Task<PagedResultDto<DistributorTargetDto>> GetListAsync(GetDistributorTargetsInput input)
    {
        var filterParams = ObjectMapper.Map<GetDistributorTargetsInput, DistributorTargetFilterParams>(input);
        var totalCount = await _distributorTargetRepository.GetCountAsync(filterParams);
        var items = await _distributorTargetRepository.GetListAsync(filterParams);
        items = items
         .OrderByDescending(o => o.BuyerTypeId)
         .ThenBy(o => o.BuyerCode).ToList();



        return new PagedResultDto<DistributorTargetDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<DistributorTarget>, List<DistributorTargetDto>>(items)
        };
    }

    public virtual async Task<DistributorTargetDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<DistributorTarget, DistributorTargetDto>(await _distributorTargetRepository.GetAsync(id));
    }

    //[Authorize(QuoteFlowPermissions.DistributorTargets.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        //await _distributorTargetRepository.DeleteAsync(id);
        try
        {
            await _distributorTargetRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.Category.RecordInUseCannotDelete);
        }
    }

    //[Authorize(QuoteFlowPermissions.DistributorTargets.Create)]
    public virtual async Task<DistributorTargetDto> CreateAsync(DistributorTargetCreateDto input)
    {

        var createParams = ObjectMapper.Map<DistributorTargetCreateDto, DistributorTargetCreateParams>(input);
        var distributorTarget = await _distributorTargetManager.CreateAsync(createParams);

        return ObjectMapper.Map<DistributorTarget, DistributorTargetDto>(distributorTarget);
    }

    //[Authorize(QuoteFlowPermissions.DistributorTargets.Edit)]
    public virtual async Task<DistributorTargetDto> UpdateAsync(Guid id, DistributorTargetUpdateDto input)
    {

        var updateParams = ObjectMapper.Map<DistributorTargetUpdateDto, DistributorTargetUpdateParams>(input);
        var distributorTarget = await _distributorTargetManager.UpdateAsync(id, updateParams);

        return ObjectMapper.Map<DistributorTarget, DistributorTargetDto>(distributorTarget);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(DistributorTargetExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }
        var filterParams = ObjectMapper.Map<DistributorTargetExcelDownloadDto, DistributorTargetFilterParams>(input);
        var items = await _distributorTargetRepository.GetListAsync(filterParams);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<DistributorTarget>, List<DistributorTargetExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "DistributorTargets.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new DistributorTargetDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new QuoteFlow.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}
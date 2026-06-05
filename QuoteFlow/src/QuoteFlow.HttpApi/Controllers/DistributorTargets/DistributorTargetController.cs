using Asp.Versioning;
using QuoteFlow.DistributorTargets;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.DistributorTargets;

[RemoteService]
[Area("app")]
[ControllerName("DistributorTarget")]
[Route("api/app/distributor-targets")]

public class DistributorTargetController : AbpController, IDistributorTargetsAppService
{
    protected IDistributorTargetsAppService _distributorTargetsAppService;

    public DistributorTargetController(IDistributorTargetsAppService distributorTargetsAppService)
    {
        _distributorTargetsAppService = distributorTargetsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<DistributorTargetDto>> GetListAsync(GetDistributorTargetsInput input)
    {
        return _distributorTargetsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<DistributorTargetDto> GetAsync(Guid id)
    {
        return _distributorTargetsAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<DistributorTargetDto> CreateAsync(DistributorTargetCreateDto input)
    {
        return _distributorTargetsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<DistributorTargetDto> UpdateAsync(Guid id, DistributorTargetUpdateDto input)
    {
        return _distributorTargetsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _distributorTargetsAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(DistributorTargetExcelDownloadDto input)
    {
        return _distributorTargetsAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _distributorTargetsAppService.GetDownloadTokenAsync();
    }

}
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.DistributorTargets;

public interface IDistributorTargetsAppService : IApplicationService
{

    Task<PagedResultDto<DistributorTargetDto>> GetListAsync(GetDistributorTargetsInput input);

    Task<DistributorTargetDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<DistributorTargetDto> CreateAsync(DistributorTargetCreateDto input);

    Task<DistributorTargetDto> UpdateAsync(Guid id, DistributorTargetUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(DistributorTargetExcelDownloadDto input);

    Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

}
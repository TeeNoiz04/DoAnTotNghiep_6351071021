using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails
{
    public interface ISpoBatchRequestDetailsAppService : IApplicationService
    {

        Task<PagedResultDto<SpoBatchRequestDetailDto>> GetListAsync(GetSpoBatchRequestDetailsInput input);

        Task<SpoBatchRequestDetailDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<SpoBatchRequestDetailDto> CreateAsync(SpoBatchRequestDetailCreateDto input);

        Task<SpoBatchRequestDetailDto> UpdateAsync(Guid id, SpoBatchRequestDetailUpdateDto input);
    }
}
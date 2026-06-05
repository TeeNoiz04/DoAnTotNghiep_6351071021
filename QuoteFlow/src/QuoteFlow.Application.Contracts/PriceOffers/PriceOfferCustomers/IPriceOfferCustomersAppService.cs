using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers;

public interface IPriceOfferCustomersAppService : IApplicationService
{

    Task<PagedResultDto<PriceOfferCustomerDto>> GetListAsync(GetPriceOfferCustomersInput input);

    Task<PriceOfferCustomerDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<PriceOfferCustomerDto> CreateAsync(PriceOfferCustomerCreateDto input);

    Task<PriceOfferCustomerDto> UpdateAsync(Guid id, PriceOfferCustomerUpdateDto input);
}
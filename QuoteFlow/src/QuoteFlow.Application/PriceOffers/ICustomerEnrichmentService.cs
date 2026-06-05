using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuoteFlow.PriceOffers;

public interface ICustomerEnrichmentService
{
    Task SetHasKeyAccountAsync(IEnumerable<PriceOfferCustomerDto> customers);
    Task SetHasKeyAccountAsync(List<ExcelRowResult<PriceOfferCustomerImportDto>> listData);
}
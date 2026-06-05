using QuoteFlow.KeyAccounts;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace QuoteFlow.PriceOffers;

public class CustomerEnrichmentService : ICustomerEnrichmentService, ITransientDependency
{
    private readonly IKeyAccountRepository _keyAccountRepository;

    public CustomerEnrichmentService(IKeyAccountRepository keyAccountRepository)
    {
        _keyAccountRepository = keyAccountRepository;
    }

    public async Task SetHasKeyAccountAsync(IEnumerable<PriceOfferCustomerDto> customers)
    {
        if (customers == null || !customers.Any())
            return;

        var customerTaxCodes = customers.Select(c => c.CustomerTaxCode).ToHashSet();
        var keyAccounts = await _keyAccountRepository.GetListAsync(x => customerTaxCodes.Contains(x.CustomerTaxCode) && x.Status == QuoteFlowStatuses.Approved);
        var keyAccountSet = keyAccounts.Select(ka => ka.CustomerTaxCode).ToHashSet();

        foreach (var customer in customers)
        {
            customer.HasKeyAccount = keyAccountSet.Contains(customer.CustomerTaxCode ?? "");
        }
    }

    public async Task SetHasKeyAccountAsync(List<ExcelRowResult<PriceOfferCustomerImportDto>> listData)
    {
        if (listData == null || !listData.Any())
            return;
        var customerTaxCodes = listData.Select(c => c.RowData.CustomerTaxCode).ToHashSet();
        var keyAccounts = await _keyAccountRepository.GetListAsync(x => customerTaxCodes.Contains(x.CustomerTaxCode));
        var keyAccountSet = keyAccounts.Select(ka => ka.CustomerTaxCode.Trim()).ToHashSet();

        foreach (var row in listData)
        {
            row.RowData.HasKeyAccount = keyAccountSet.Contains(row.RowData.CustomerTaxCode?.Trim() ?? "");
        }
    }
}
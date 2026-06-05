using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers;

public class PriceOfferCustomerManager : DomainService
{
    protected IPriceOfferCustomerRepository _priceOfferCustomerRepository;

    public PriceOfferCustomerManager(IPriceOfferCustomerRepository priceOfferCustomerRepository)
    {
        _priceOfferCustomerRepository = priceOfferCustomerRepository;
    }

    public virtual async Task<PriceOfferCustomer> CreateAsync(PriceOfferCustomerCreateParams createParams)
    {
        var priceOfferCustomer = new PriceOfferCustomer(
            GuidGenerator.Create(),
            createParams
        );

        return await _priceOfferCustomerRepository.InsertAsync(priceOfferCustomer);
    }

    public virtual async Task<PriceOfferCustomer> UpdateAsync(
        Guid id,
        PriceOfferCustomerUpdateParams updateParams
    )
    {
        var priceOfferCustomer = await _priceOfferCustomerRepository.GetAsync(id);

        priceOfferCustomer.PriceOfferId = updateParams.PriceOfferId;
        priceOfferCustomer.SaleChannel = updateParams.SaleChannel;
        priceOfferCustomer.CustomerId = updateParams.CustomerId;
        priceOfferCustomer.CustomerTaxCode = updateParams.CustomerTaxCode;
        priceOfferCustomer.CustomerName = updateParams.CustomerName;
        priceOfferCustomer.CustomerAddress = updateParams.CustomerAddress;
        priceOfferCustomer.CustomerNationality = updateParams.CustomerNationality;
        priceOfferCustomer.CustomerType = updateParams.CustomerType;
        priceOfferCustomer.Note = updateParams.Note;

        priceOfferCustomer.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _priceOfferCustomerRepository.UpdateAsync(priceOfferCustomer);
    }
}
using QuoteFlow.Buyers.ParameterObjects;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Buyers;

public class BuyerManager : DomainService
{
    protected IBuyerRepository _buyerRepository;
    public BuyerManager(IBuyerRepository buyerRepository)
    {
        _buyerRepository = buyerRepository;
    }

    public virtual async Task<Buyer> CreateAsync(BuyerCreateParams createParams)
    {
        var buyer = new Buyer(GuidGenerator.Create(), createParams);
        return await _buyerRepository.InsertAsync(buyer);
    }
    public virtual async Task<Buyer> UpdateAsync(Guid id, BuyerUpdateParams updateParams)
    {
        var buyer = await _buyerRepository.GetAsync(id);
        buyer.BuyerTypeId = updateParams.BuyerTypeId;
        buyer.BuyerTypeCode = updateParams.BuyerTypeCode;
        buyer.BuyerCode = updateParams.BuyerCode;
        buyer.ShortName = updateParams.ShortName;
        buyer.FullName = updateParams.FullName;
        buyer.TaxCode = updateParams.TaxCode;
        buyer.Address = updateParams.Address;
        buyer.ContactPerson = updateParams.ContactPerson;
        buyer.ContactEmail = updateParams.ContactEmail;
        buyer.ContactPhoneNumber = updateParams.ContactPhoneNumber;
        buyer.PaymentTermCode = updateParams.PaymentTermCode;
        buyer.PaymentTermDescription = updateParams.PaymentTermDescription;
        buyer.CreditLimit = updateParams.CreditLimit;
        buyer.CreditExposure = updateParams.CreditExposure;
        buyer.AppliedPrice = updateParams.AppliedPrice;
        buyer.Deactive = updateParams.Deactive;
        buyer.Note = updateParams.Note;
        buyer.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _buyerRepository.UpdateAsync(buyer);
    }
}

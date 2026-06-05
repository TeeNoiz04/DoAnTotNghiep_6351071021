using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Customers;

public class CustomerManager : DomainService
{
    protected ICustomerRepository _customerRepository;

    public CustomerManager(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    

    public virtual async Task<Customer> CreateAsync(CustomerCreateParams createParams)
    {
        var customer = new Customer(GuidGenerator.Create(), createParams);
        var taxCode = customer.TaxCode?.Trim();

        if (!string.IsNullOrWhiteSpace(taxCode))
        {
            var validationErr = CodeHelper.ValidateTaxCode(taxCode);
            if (validationErr != null)
            {
                throw new UserFriendlyException(validationErr);
            }
        }

        return await _customerRepository.InsertAsync(customer);
    }

    public virtual async Task<Customer> UpdateAsync(Guid id, CustomerUpdateParams updateParams)
    {
        // 1. Fetch the existing customer object
        var customer = await _customerRepository.GetAsync(id);
        // customer.TaxCode is NOT updated from params, it retains its existing value.

        // 2. Update properties from params (Country is the key update here)
        customer.IsDeactive = updateParams.IsDeactive;
        customer.CustomerName = updateParams.CustomerName;
        customer.Address = updateParams.Address;
        customer.Phone = updateParams.Phone;
        customer.Country = updateParams.Country; // Update Country
        customer.Note = updateParams.Note;
        customer.Province = updateParams.Province;
        customer.Website = updateParams.Website;
        customer.CustomerIndustry = updateParams.CustomerIndustry;
        customer.CustomerType = updateParams.CustomerType;

        customer.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);
        return await _customerRepository.UpdateAsync(customer);
    }

    public async Task<List<Customer>> CreateManyAsync(List<CustomerCreateParams> createParams)
    {
        var result = new List<Customer>();
        foreach (var create in createParams)
        {
            var item = new Customer(new Guid(), create);
            result.Add(item);
        }
        await _customerRepository.InsertManyAsync(result);
        return result;
    }

    public async Task<List<Customer>> UpdateManyAsync(List<CustomerUpdateParams> updateParams)
    {
        var result = new List<Customer>();

        var ids = updateParams.Select(x => x.Id).ToList();

        var supplierBUs = await _customerRepository.GetListAsync(x => ids.Contains(x.Id));

        var supplierBUMap = supplierBUs.ToDictionary(x => x.Id, x => x);

        foreach (var updateParam in updateParams)
        {
            if (supplierBUMap.TryGetValue(updateParam.Id!.Value, out var cus))
            {
                //cus.IsDeactive = updateParam.IsDeactive;
                cus.CustomerName = updateParam.CustomerName;
                cus.Address = updateParam.Address;
                cus.Phone = updateParam.Phone;
                cus.Country = updateParam.Country; // Update Country
                cus.Note = updateParam.Note;
                cus.Province = updateParam.Province;
                cus.Website = updateParam.Website;
                cus.CustomerIndustry = updateParam.CustomerIndustry;
                cus.CustomerType = updateParam.CustomerType;
                cus.SetConcurrencyStampIfNotNull(updateParam.ConcurrencyStamp);

                result.Add(cus);
            }
        }

        return result;
    }
}
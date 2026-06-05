using JetBrains.Annotations;
using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;

namespace QuoteFlow.Customers;

public class Customer : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string TaxCode { get; set; }

    [CanBeNull]
    public virtual string CustomerName { get; set; }

    [CanBeNull]
    public virtual string? CustomerShortName { get; set; }

    [CanBeNull]
    public virtual string? CustomerType { get; set; }

    [CanBeNull]
    public virtual string? Address { get; set; }

    [CanBeNull]
    public virtual string? Phone { get; set; }

    [CanBeNull]
    public virtual string? Country { get; set; }

    [CanBeNull]
    public virtual string? Province { get; set; }

    [CanBeNull]
    public virtual string? Website { get; set; }

    [CanBeNull]
    public virtual string? CustomerIndustry { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public virtual bool IsDeactive { get; set; }

    protected Customer()
    {

    }

    public Customer(Guid id, CustomerCreateParams createParams)
    {
        Id = id;

        TaxCode = createParams.TaxCode;
        CustomerName = createParams.CustomerName;
        CustomerShortName = createParams.CustomerShortName;
        CustomerType = createParams.CustomerType;
        Address = createParams.Address;
        Phone = createParams.Phone;
        Country = createParams.Country;
        Note = createParams.Note;
        Province = createParams.Province;
        Website = createParams.Website;
        CustomerIndustry = createParams.CustomerIndustry;

        IsDeactive = false;
    }

}
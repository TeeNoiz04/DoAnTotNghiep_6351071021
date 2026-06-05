using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.Suppliers.ParameterObject;
using System;

namespace QuoteFlow.Suppliers;

public class Supplier : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string SupplierCode { get; set; }
    [CanBeNull]
    public virtual string? SAPCode { get; set; }
    [NotNull]
    public virtual string ShortName { get; set; }
    [NotNull]
    public virtual string FullName { get; set; }
    public virtual string? TaxCode { get; set; }
    public virtual string? Address { get; set; }
    public virtual bool? IsDeactive { get; set; }

    public Supplier()
    {

    }

    public Supplier(Guid id, string supplierCode, string sapCode, string shortName, string fullName, string? taxCode = null, string? address = null, bool? isDeactive = false)
    {
        Id = id;
        SupplierCode = supplierCode;
        SAPCode = sapCode;
        ShortName = shortName;
        FullName = fullName;
        TaxCode = taxCode;
        Address = address;
        IsDeactive = isDeactive ?? false;
    }
    public Supplier(Guid id, SupplierCreateParams createParams)
    {
        Id = id;
        SupplierCode = createParams.SupplierCode;
        SAPCode = createParams.SAPCode;
        ShortName = createParams.ShortName;
        FullName = createParams.FullName;
        TaxCode = createParams.TaxCode;
        Address = createParams.Address;
        IsDeactive = createParams.IsDeactive;
    }

}

using QuoteFlow.Suppliers.ParameterObject;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.Suppliers;

public class SupplierManager : DomainService
{
    protected ISupplierRepository _supplierRepository;
    public SupplierManager(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<Supplier> CreateAsync(SupplierCreateParams createParams)
    {
        var supplier = new Supplier(
            GuidGenerator.Create(),
         createParams
            );
        return await _supplierRepository.InsertAsync(supplier);
    }

    public async Task<Supplier> UpdateAsync(Guid id, SupplierUpdateParams updateParams)
    {
        var supplier = await _supplierRepository.GetAsync(id);
        supplier.SupplierCode = updateParams.SupplierCode;
        supplier.SAPCode = updateParams.SAPCode;
        supplier.ShortName = updateParams.ShortName;
        supplier.FullName = updateParams.FullName;
        supplier.TaxCode = updateParams.TaxCode;
        supplier.Address = updateParams.Address;
        supplier.IsDeactive = updateParams.IsDeactive;
        supplier.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _supplierRepository.UpdateAsync(supplier);
    }
}

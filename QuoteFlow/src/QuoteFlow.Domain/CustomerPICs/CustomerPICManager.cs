using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.CustomerPICs;

public class CustomerPICManager : DomainService
{
    protected ICustomerPICRepository _customerPICRepository;

    public CustomerPICManager(ICustomerPICRepository customerPICRepository)
    {
        _customerPICRepository = customerPICRepository;
    }

    public virtual async Task<CustomerPIC> CreateAsync(
    Guid keyAccountId, string? pICName = null, string? pIC_Phone = null, string? pIC_Email = null, string? pIC_JobTitle = null, string? remark = null)
    {
        Check.Length(pICName, nameof(pICName), CustomerPICConsts.PICNameMaxLength);
        Check.Length(pIC_Phone, nameof(pIC_Phone), CustomerPICConsts.PIC_PhoneMaxLength);
        Check.Length(pIC_Email, nameof(pIC_Email), CustomerPICConsts.PIC_EmailMaxLength);
        Check.Length(pIC_JobTitle, nameof(pIC_JobTitle), CustomerPICConsts.PIC_JobTitleMaxLength);
        Check.Length(remark, nameof(remark), CustomerPICConsts.RemarkMaxLength);

        var customerPIC = new CustomerPIC(
         GuidGenerator.Create(),
         keyAccountId, pICName, pIC_Phone, pIC_Email, pIC_JobTitle, remark
         );

        return await _customerPICRepository.InsertAsync(customerPIC);
    }

    public virtual async Task<CustomerPIC> UpdateAsync(
        Guid id,
        Guid keyAccountId, string? pICName = null, string? pIC_Phone = null, string? pIC_Email = null, string? pIC_JobTitle = null, string? remark = null, [CanBeNull] string? concurrencyStamp = null
    )
    {
        Check.Length(pICName, nameof(pICName), CustomerPICConsts.PICNameMaxLength);
        Check.Length(pIC_Phone, nameof(pIC_Phone), CustomerPICConsts.PIC_PhoneMaxLength);
        Check.Length(pIC_Email, nameof(pIC_Email), CustomerPICConsts.PIC_EmailMaxLength);
        Check.Length(pIC_JobTitle, nameof(pIC_JobTitle), CustomerPICConsts.PIC_JobTitleMaxLength);
        Check.Length(remark, nameof(remark), CustomerPICConsts.RemarkMaxLength);

        var customerPIC = await _customerPICRepository.GetAsync(id);

        customerPIC.KeyAccountId = keyAccountId;
        customerPIC.PICName = pICName;
        customerPIC.PIC_Phone = pIC_Phone;
        customerPIC.PIC_Email = pIC_Email;
        customerPIC.PIC_JobTitle = pIC_JobTitle;
        customerPIC.Remark = remark;

        customerPIC.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _customerPICRepository.UpdateAsync(customerPIC);
    }

}
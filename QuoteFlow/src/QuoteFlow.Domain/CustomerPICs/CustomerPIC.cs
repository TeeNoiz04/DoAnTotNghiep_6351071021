using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.CustomerPICs;

public class CustomerPIC : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid KeyAccountId { get; set; }

    [CanBeNull]
    public virtual string? PICName { get; set; }

    [CanBeNull]
    public virtual string? PIC_Phone { get; set; }

    [CanBeNull]
    public virtual string? PIC_Email { get; set; }

    [CanBeNull]
    public virtual string? PIC_JobTitle { get; set; }

    [CanBeNull]
    public virtual string? Remark { get; set; }

    protected CustomerPIC()
    {

    }

    public CustomerPIC(Guid id, Guid keyAccountId, string? pICName = null, string? pIC_Phone = null, string? pIC_Email = null, string? pIC_JobTitle = null, string? remark = null)
    {

        Id = id;
        Check.Length(pICName, nameof(pICName), CustomerPICConsts.PICNameMaxLength, 0);
        Check.Length(pIC_Phone, nameof(pIC_Phone), CustomerPICConsts.PIC_PhoneMaxLength, 0);
        Check.Length(pIC_Email, nameof(pIC_Email), CustomerPICConsts.PIC_EmailMaxLength, 0);
        Check.Length(pIC_JobTitle, nameof(pIC_JobTitle), CustomerPICConsts.PIC_JobTitleMaxLength, 0);
        Check.Length(remark, nameof(remark), CustomerPICConsts.RemarkMaxLength, 0);
        KeyAccountId = keyAccountId;
        PICName = pICName;
        PIC_Phone = pIC_Phone;
        PIC_Email = pIC_Email;
        PIC_JobTitle = pIC_JobTitle;
        Remark = remark;
    }

}
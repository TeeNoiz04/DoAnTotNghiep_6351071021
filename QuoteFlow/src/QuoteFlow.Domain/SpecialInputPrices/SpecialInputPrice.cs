using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.SpecialInputPrices.ParameterObject;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;
using QuoteFlow.SupplierBUs;
using QuoteFlow.Suppliers;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace QuoteFlow.SpecialInputPrices;

public class SpecialInputPrice : ExtendedFullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string AccountNo { get; set; }

    [NotNull]
    public virtual string AccountName { get; set; }

    [CanBeNull]
    public virtual string? ProjectName { get; set; }

    [CanBeNull]
    public virtual string? MaterialType { get; set; }

    public virtual Guid? SupplierId { get; set; }

    public virtual Guid? SupplierBUId { get; set; }

    [CanBeNull]
    public virtual string? Currency { get; set; }

    public virtual DateTime? ValidFrom { get; set; }

    public virtual DateTime? ValidTo { get; set; }

    [NotNull]
    public virtual string Status { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public virtual ICollection<SpecialInputPriceDetail>? Details { get; set; }


    public virtual SupplierBU? SupplierBU { get; set; }

    public virtual Supplier? Supplier { get; set; }
    protected SpecialInputPrice()
    {

    }

    public SpecialInputPrice(Guid id, string accountNo, string accountName, string status, string? projectName = null, DateTime? validFrom = null, DateTime? validTo = null, string? note = null)
    {

        Id = id;
        Check.NotNull(accountNo, nameof(accountNo));
        Check.Length(accountNo, nameof(accountNo), SpecialInputPriceConsts.AccountNoMaxLength, 0);
        Check.NotNull(accountName, nameof(accountName));
        Check.Length(accountName, nameof(accountName), SpecialInputPriceConsts.AccountNameMaxLength, 0);
        Check.Length(projectName, nameof(projectName), SpecialInputPriceConsts.ProjectNameMaxLength, 0);
        Check.Length(note, nameof(note), SpecialInputPriceConsts.NoteMaxLength, 0);
        AccountNo = accountNo;
        AccountName = accountName;
        Status = status;
        ProjectName = projectName;
        ValidFrom = validFrom;
        ValidTo = validTo;
        Note = note;
    }

    public bool IsValidFor(DateTime date)
    {
        if (ValidFrom.HasValue && date.Date < ValidFrom.Value.Date)
        {
            return false;
        }

        if (ValidTo.HasValue && date.Date > ValidTo.Value.Date)
        {
            return false;
        }

        if (Status != QuoteFlowStatuses.SpecialInputPrice.Valid)
        {
            return false;
        }

        return true;
    }

    public SpecialInputPrice(Guid id, SpecialInputPriceCreateParams createParams)
    {
        Id = id;
        AccountNo = createParams.AccountNo;
        AccountName = createParams.AccountName;
        Status = createParams.Status;
        ProjectName = createParams.ProjectName;
        MaterialType = createParams.MaterialType;
        SupplierId = createParams.SupplierId;
        SupplierBUId = createParams.SupplierBUId;
        Currency = createParams.Currency;
        ValidFrom = createParams.ValidFrom;
        ValidTo = createParams.ValidTo;
        Note = createParams.Note;





    }
}
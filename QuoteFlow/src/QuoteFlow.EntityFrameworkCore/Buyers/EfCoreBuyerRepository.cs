using QuoteFlow.Buyers.ParameterObjects;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Helper;
using QuoteFlow.MaterialGroupBuyers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Buyers;

public class EfCoreBuyerRepository : EfCoreRepository<QuoteFlowDbContext, Buyer, Guid>, IBuyerRepository
{
    public EfCoreBuyerRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async virtual Task<List<Buyer>> GetListAsync(BuyerFilterParams filterParams, string? sorting, int maxResultCount, int skipCount, CancellationToken cancellationToken)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);
        query = query.Include(e => e.BuyerType);

        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? BuyerConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }



    public async virtual Task<long> GetCountAsync(BuyerFilterParams filterParams, CancellationToken cancellationToken)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Buyer> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.Set<Buyer>()
            .Include(e => e.BuyerType)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException(typeof(Buyer), id);
    }

    protected virtual IQueryable<Buyer> ApplyFilter(
    IQueryable<Buyer> query,
    BuyerFilterParams filterParams)
    {
        var filterText = filterParams.FilterText;
        var buyerTypeId = filterParams.BuyerTypeId;
        var buyerCode = filterParams.BuyerCode;
        var shortName = filterParams.ShortName;
        var fullName = filterParams.FullName;
        var taxCode = filterParams.TaxCode;
        var address = filterParams.Address;
        var contactPerson = filterParams.ContactPerson;
        var contactEmail = filterParams.ContactEmail;
        var contactPhoneNumber = filterParams.ContactPhoneNumber;
        var paymentTermCode = filterParams.PaymentTermCode;
        var paymentTermDescription = filterParams.PaymentTermDescription;
        var creditLimitMax = filterParams.CreditLimitMax;
        var creditLimitMin = filterParams.CreditLimitMin;
        var creditExposureMax = filterParams.CreditExposureMax;
        var creditExposureMin = filterParams.CreditExposureMin;
        var appliedPrice = filterParams.AppliedPrice;
        var deactive = filterParams.Deactive;
        var note = filterParams.Note;

        return query
    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e =>
        e.BuyerCode.Contains(filterText) ||
        (e.ShortName != null && e.ShortName.Contains(filterText)) ||
        (e.FullName != null && e.FullName.Contains(filterText)) ||
        (e.TaxCode != null && e.TaxCode.Contains(filterText)) ||
        (e.Address != null && e.Address.Contains(filterText)) ||
        (e.ContactPerson != null && e.ContactPerson.Contains(filterText)) ||
        (e.ContactEmail != null && e.ContactEmail.Contains(filterText)) ||
        (e.ContactPhoneNumber != null && e.ContactPhoneNumber.Contains(filterText)) ||
        (e.PaymentTermCode != null && e.PaymentTermCode.Contains(filterText)) ||
        (e.PaymentTermDescription != null && e.PaymentTermDescription.Contains(filterText)) ||
        (e.Note != null && e.Note.Contains(filterText))
    )
    .WhereIf(buyerTypeId.HasValue && buyerTypeId.Value != Guid.Empty, e => e.BuyerTypeId == buyerTypeId!.Value)
    .WhereIf(!string.IsNullOrWhiteSpace(buyerCode), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(buyerCode, e => e.BuyerCode))
    .WhereIf(!string.IsNullOrWhiteSpace(shortName), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(shortName, e => e.ShortName))
    .WhereIf(!string.IsNullOrWhiteSpace(fullName), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(fullName, e => e.FullName))
    .WhereIf(!string.IsNullOrWhiteSpace(taxCode), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(taxCode, e => e.TaxCode))
    .WhereIf(!string.IsNullOrWhiteSpace(address), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(address, e => e.Address))
    .WhereIf(!string.IsNullOrWhiteSpace(contactPerson), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(contactPerson, e => e.ContactPerson))
    .WhereIf(!string.IsNullOrWhiteSpace(contactEmail), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(contactEmail, e => e.ContactEmail))
    .WhereIf(!string.IsNullOrWhiteSpace(contactPhoneNumber), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(contactPhoneNumber, e => e.ContactPhoneNumber))
    .WhereIf(!string.IsNullOrWhiteSpace(paymentTermCode), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(paymentTermCode, e => e.PaymentTermCode))
    .WhereIf(!string.IsNullOrWhiteSpace(paymentTermDescription), QueryFilterHelper.BuildMultiFieldSearch<Buyer>(paymentTermDescription, e => e.PaymentTermDescription))
    .WhereIf(creditLimitMin.HasValue, e => e.CreditLimit >= creditLimitMin!.Value)
    .WhereIf(creditLimitMax.HasValue, e => e.CreditLimit <= creditLimitMax!.Value)
    .WhereIf(creditExposureMin.HasValue, e => e.CreditExposure >= creditExposureMin!.Value)
    .WhereIf(creditExposureMax.HasValue, e => e.CreditExposure <= creditExposureMax!.Value)
    .WhereIf(appliedPrice.HasValue, e => e.AppliedPrice == appliedPrice.Value)
    .WhereIf(deactive.HasValue, e => e.Deactive == deactive.Value)
    .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note != null && e.Note.Contains(note!));

    }

    public async Task<List<Buyer>> GetBuyersNotAssignedToMaterialGroupAsync()
    {
        var dbContext = await GetDbContextAsync();
        var buyerIdsInMaterialGroup = await dbContext.Set<MaterialGroupBuyer>()
            .Select(mg => mg.BuyerId)
            .Distinct()
            .ToListNoLockAsync(dbContext);

        var query = dbContext.Set<Buyer>()
            .Where(buyer => !buyerIdsInMaterialGroup.Contains(buyer.Id));

        return await query.ToListNoLockAsync(dbContext);
    }
}

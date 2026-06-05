using Dapper;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Extensions;
using QuoteFlow.Helper;
using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferReportDetails;
using QuoteFlow.PriceOffers.PriceOfferReportDetails.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals.ParameterObjects;
using QuoteFlow.RequesterContexts;
using QuoteFlow.Shared.Models;
using QuoteFlow.SystemCategories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace QuoteFlow.PriceOffers;

public class EfCorePriceOfferRepository : EfCoreRepository<QuoteFlowDbContext, PriceOffer, Guid>, IPriceOfferRepository
{
    private readonly ICurrentUser _userFromToken;
    private readonly IRequesterContext _userFromHeader;

    public EfCorePriceOfferRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider, ICurrentUser userFromToken, IRequesterContext userFromHeader)
        : base(dbContextProvider)
    {
        _userFromToken = userFromToken;
        _userFromHeader = userFromHeader;
    }

    public virtual async Task<List<PriceOffer>> GetListAsync(
        PriceOfferFilterParams filterParams,
        string? currentUsername = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        query = (await ApplyFilter(query, filterParams, currentUsername))
            .Include(x => x.Details)
            .Include(x => x.Buyer)
                .ThenInclude(x => x.BuyerType)
            .Include(x => x.ApprovalHistories.OrderByDescending(x => x.CreationTime))
            .Include(x => x.ApprovalRoutes);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PriceOfferConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        PriceOfferFilterParams filterParams,
        string? currentUsername = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await ApplyFilter((await GetDbSetAsync()), filterParams, currentUsername);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    public async Task<PriceOffer> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        var queryData = query
            .Include(x => x.Buyer)
                .ThenInclude(x => x.BuyerType)
            .Include(x => x.KeyAccount)
            .Include(x => x.Customers.OrderBy(c => c.SaleChannelNumber))
            .Include(x => x.Details)
                .ThenInclude(x => x.ApprovalHistories.OrderByDescending(x => x.CreationTime))
            .Include(x => x.Attachments)
            .Include(x => x.ApprovalRoutes)
            .Include(x => x.ApprovalHistories.OrderByDescending(x => x.CreationTime))
            .Where(x => x.Id == id);

        var result = await queryData.FirstOrDefaultNoLockAsync(dbContext, cancellationToken)
            ?? throw new EntityNotFoundException(typeof(PriceOffer), id);

        return result;
    }
    public async Task<PriceOffer> GetWithDetailsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        var resultData = query
            .Include(x => x.Buyer)
                .ThenInclude(x => x.BuyerType)
            .Include(x => x.KeyAccount)
            .Include(x => x.Customers.OrderBy(c => c.SaleChannelNumber))
            .Include(x => x.Details)
                .ThenInclude(x => x.ApprovalHistories.OrderByDescending(x => x.CreationTime))
            .Include(x => x.Attachments)
            .Include(x => x.ApprovalRoutes)
            .Include(x => x.ApprovalHistories.OrderByDescending(x => x.CreationTime))
            .AsNoTracking()
            .Where(x => x.Id == id);

        var result = await resultData.FirstOrDefaultNoLockAsync(dbContext, cancellationToken)
            ?? throw new EntityNotFoundException(typeof(PriceOffer), id);
        return result;
    }

    public async Task<PriceOffer> GetWithDetailsAsync(
        Guid id,
        params Expression<Func<PriceOffer, object?>>[] includes)
    {
        var query = (await GetQueryableAsync()).Where(p => p.Id == id);

        foreach (var include in includes)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync()
            ?? throw new EntityNotFoundException(typeof(PriceOffer), id);
    }

    public override async Task<PriceOffer> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        var result = await query
            .Include(x => x.ApprovalRoutes)
            .Include(x => x.ApprovalHistories)
            .Include(x => x.Attachments)
            .Where(x => x.Id == id)
            .FirstOrDefaultNoLockAsync(dbContext, GetCancellationToken(cancellationToken))
            ?? throw new EntityNotFoundException(typeof(PriceOffer), id);

        return result;
    }

    public async Task<string?> GetLatestCodeAsync(
        string prefix,
        string productType,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        var latestCode = await query
            .Where(p => p.PriceOfferCode.StartsWith(prefix) && p.MaterialType == productType)
            .OrderByDescending(p => p.CreationTime)
            .Select(p => p.PriceOfferCode)
            .FirstOrDefaultAsync(cancellationToken);

        return latestCode;
    }

    public async Task<List<PriceOfferReportDetail>> GetListReportDetailAsync(PriceOfferReportDetailFilterParams filterParams)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@fromDate", filterParams.From);
        parameters.Add("@toDate", filterParams.To);
        parameters.Add("@hasFullBuyerAccess", filterParams.HasFullBuyerAccess);
        parameters.Add("@userName", filterParams.UserName);
        parameters.Add("@golfaCode", filterParams.GolfaCode);
        parameters.Add("@modelName", filterParams.ModelName);
        parameters.Add("@buyerCode", filterParams.Buyer);
        parameters.Add("@SPOCode", filterParams.PriceOfferCode);
        parameters.Add("@SPOName", filterParams.PriceOfferName);
        parameters.Add("@materialGroup", filterParams.MaterialGroup);
        parameters.Add("@hasStrategicPriceAccess", filterParams.HasStrategicPriceAccess);


        var result = (await connection.QueryAsync<PriceOfferReportDetail>(
            "usp_Report_SPODetail_R04",
            parameters,
            commandType: CommandType.StoredProcedure,
            commandTimeout: 180
        )).AsList();

        return result

            .ToList();
    }

    public async Task<List<PriceOfferReportGeneral>> GetListReportGeneralAsync(PriceOfferReportGeneralFilterParams filterParams)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new
        {
            fromdate = filterParams.From,
            todate = filterParams.To,
            SPOName = filterParams.PriceOfferName,
            location = filterParams.Location,
            buyerCode = filterParams.Buyer,
            customerName = filterParams.CustomerName,
            SPOCode = filterParams.PriceOfferCode,
            status = filterParams.Status,
            materialType = filterParams.MaterialType,
            orderRatio_Min = filterParams.OrderMin,
            orderRatio_Max = filterParams.OrderMax,
            hasFullBuyerAccess = filterParams.HasFullBuyerAccess,
            hasStrategicPriceAccess = filterParams.HasStrategicPriceAccess,
            userName = filterParams.UserName
        };

        var result = (await connection.QueryAsync<PriceOfferReportGeneral>(
            "usp_Report_SPOGeneral_R03",
            parameters,
            commandType: CommandType.StoredProcedure,
            commandTimeout: 180
        )).AsList();

        return result;
    }

    public virtual async Task GenerateApprovalRouteAsync(Guid priceOfferId, string priceOfferCode, string? note = null, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new
        {
            priceOfferId,
            note
        };

        var priceOfferType = priceOfferCode.Substring(0, 2);
        if (priceOfferType == PriceOfferTypes.PriceOfferPP)
        {
            await connection.ExecuteAsync(
                "usp_PriceOffer_WF_CreateApprovalRoute_PP",
                parameters,
                commandType: CommandType.StoredProcedure,
                transaction: await GetDbTransactionAsync()
            );
        }
        else if (priceOfferType == PriceOfferTypes.PriceOfferAP)
        {
            await connection.ExecuteAsync(
                "usp_PriceOffer_WF_CreateApprovalRoute_AP",
                parameters,
                commandType: CommandType.StoredProcedure,
                transaction: await GetDbTransactionAsync()
            );
        }
        else if (priceOfferType == PriceOfferTypes.PriceOfferDS)
        {
            await connection.ExecuteAsync(
                "usp_PriceOffer_WF_CreateApprovalRoute_DS",
                parameters,
                commandType: CommandType.StoredProcedure,
                transaction: await GetDbTransactionAsync()
            );
        }
        else if (priceOfferType == PriceOfferTypes.PriceOfferNB)
        {
            await connection.ExecuteAsync(
                "usp_PriceOffer_WF_CreateApprovalRoute_NB",
                parameters,
                commandType: CommandType.StoredProcedure,
                transaction: await GetDbTransactionAsync()
            );
        }
        else
        {
            throw new ArgumentException($"Unknown Price Offer Type: {priceOfferType}");
        }
    }

    public virtual async Task<bool> HasLevel5ApprovalAsync(Guid priceOfferId, string priceOfferCode, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(); // Use await instead of .Result
        var connection = dbContext.Database.GetDbConnection();
        var transaction = await GetDbTransactionAsync();

        var priceOfferType = priceOfferCode.Substring(0, 2);
        string functionName = priceOfferType switch
        {
            PriceOfferTypes.PriceOfferPP => "dbo.ufn_ShouldCreateLevel5Route_PP",
            PriceOfferTypes.PriceOfferAP => "dbo.ufn_ShouldCreateLevel5Route_AP",
            PriceOfferTypes.PriceOfferDS => "dbo.ufn_ShouldCreateLevel5Route_DS",
            PriceOfferTypes.PriceOfferNB => "dbo.ufn_ShouldCreateLevel5Route_NB",
            _ => throw new ArgumentException($"Unknown Price Offer Type: {priceOfferType}")
        };

        var sql = $"SELECT {functionName}(@priceOfferId)";
        var parameters = new { priceOfferId };

        var result = await connection.ExecuteScalarAsync<bool>(
            sql,
            parameters,
            transaction
        );

        return result;
    }

    public virtual async Task<PriceOffer> UpdateCalculatedFieldsAsync(Guid priceOfferId, bool applySpecialInputPrice = false, CancellationToken cancellationToken = default)
    {
        var priceOffer = await GetAsync(priceOfferId, cancellationToken: cancellationToken);
        priceOffer = await UpdateAsync(priceOffer, true, cancellationToken);

        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new
        {
            priceOfferId,
            applySpecialAccount = applySpecialInputPrice ? 1 : 0
        };
        await connection.ExecuteAsync(
            "usp_PriceOffer_UpdateDerivedValues",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        dbContext.Entry(priceOffer).State = EntityState.Detached;

        return await GetWithDetailsAsync(priceOfferId);
    }

    public virtual async Task GenerateApprovalRouteAP(Guid priceOfferId, string? note = null, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new
        {
            priceOfferId,
            note
        };

        await connection.ExecuteAsync(
            "usp_PriceOffer_WF_CreateApprovalRoute_AP",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );
    }

    public virtual async Task GenerateApprovalRouteDS(Guid priceOfferId, string? note = null, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new
        {
            priceOfferId,
            note
        };

        await connection.ExecuteAsync(
            "usp_PriceOffer_WF_CreateApprovalRoute_DS",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );
    }

    public virtual async Task<List<PriceOfferApprovalRoute>> GetListApprovalRoutesAsync(
        Guid? priceOfferId
    )
    {
        var query = await GetQueryableAsync();

        var approvalRoutes = query
            .Include(po => po.ApprovalRoutes)
            .WhereIf(priceOfferId.HasValue, po => po.Id == priceOfferId!.Value)
            .SelectMany(po => po.ApprovalRoutes)
            .ToList();

        return approvalRoutes;
    }

    public async Task<long> GetCountMyApprovalsAsync(PriceOfferFilterParams filterParams, string currentUsername)
    {
        var dbContext = await GetDbContextAsync();
        var query = (await ApplyFilter((await GetDbSetAsync()), filterParams))
            .Where(x => x.ApprovalRoutes.Any(ar => ar.Approver == currentUsername && !ar.IsApproved && ar.StepSequence == x.CurrentApprovalStepSequence));

        return await query.CountNoLockAsync(dbContext, GetCancellationToken(CancellationToken.None));
    }
    public async Task<List<PriceOffer>> GetListMyApprovalsAsync(PriceOfferFilterParams filterParams, string currentUsername, string? sorting, int maxResultCount, int skipCount)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        query = (await ApplyFilter(query, filterParams))
            .Where(x => x.ApprovalRoutes.Any(ar => ar.Approver == currentUsername && !ar.IsApproved && ar.StepSequence == x.CurrentApprovalStepSequence))
            .Include(x => x.Buyer)
                .ThenInclude(x => x.BuyerType)
            .Include(x => x.ApprovalHistories.OrderByDescending(x => x.CreationTime))
            .Include(x => x.ApprovalRoutes);

        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PriceOfferConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, CancellationToken.None);
    }

    public async Task<List<PriceOfferMessage>> GetListMessagesAsync(Guid priceOfferId, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        var items = await query
            .Where(x => x.Id == priceOfferId)
            .SelectMany(x => x.Messages)
            .OrderByDescending(x => x.CreationTime)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);

        items.Reverse();

        return items;
    }

    public async Task<long> GetCountMessagesAsync(Guid priceOfferId, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        return await query
            .Where(x => x.Id == priceOfferId)
            .SelectMany(x => x.Messages)
            .LongCountAsync(cancellationToken);
    }

    public async Task<decimal?> GetDiscountRatioConfigured(
        string priceOfferType,
        string materialType,
        decimal totalMEVNOfferAmount,
        string? keyAccountClassDescription = null)
    {
        var dbContext = await GetDbContextAsync(); // Use await instead of .Result
        var connection = dbContext.Database.GetDbConnection();
        var transaction = await GetDbTransactionAsync();

        string functionName = "dbo.ufn_PriceOffer_GetDiscountRatio_CFG_ManualInput";

        var sql = $"SELECT {functionName}(@priceOffer_Type, @productType, @totalAmount, @keyAccountClassify)";
        var parameters = new { priceOffer_Type = priceOfferType, productType = materialType, totalAmount = totalMEVNOfferAmount, keyAccountClassify = keyAccountClassDescription };

        var result = await connection.ExecuteScalarAsync<decimal?>(
            sql,
            parameters,
            transaction
        );

        return result;
    }

    protected virtual async Task<IQueryable<PriceOffer>> ApplyFilter(
        IQueryable<PriceOffer> query,
        PriceOfferFilterParams filterParams,
        string? currentUsername = null)
    {
        var filterText = filterParams.FilterText;
        var priceOfferCode = filterParams.PriceOfferCode;
        var priceOfferType = filterParams.PriceOfferType;
        var customerName = filterParams.CustomerName;
        var createdFrom = filterParams.CreatedFrom;
        var createdTo = filterParams.CreatedTo;
        var relatedToMe = filterParams.RelatedToMe;
        var approvalStatus = filterParams.ApprovalStatus;
        var projectResultStatus = filterParams.ProjectResultStatus;
        var materialType = filterParams.MaterialType;
        var customerTaxCode = filterParams.CustomerTaxCode;
        var projectName = filterParams.ProjectName;
        var currentUserUserName = currentUsername;

        if (!string.IsNullOrWhiteSpace(customerName))
        {
            query = query
                .Include(x => x.KeyAccount)
                .Include(x => x.Customers)
                .AsQueryable();
        }

        var preFiltered = query
                .Where(e => e.ApprovalStatus != QuoteFlowStatuses.Verifying)
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.PriceOfferCode!.Contains(filterText!) || e.MaterialType!.Contains(filterText!) || e.LocationOld!.Contains(filterText!) || e.ProjectName!.Contains(filterText!) || e.Application!.Contains(filterText!) || e.Country!.Contains(filterText!) || e.Province!.Contains(filterText!) || e.DetailedAddress!.Contains(filterText!) || e.CompetitorBrand!.Contains(filterText!) || e.PriceGapWithCompetitor!.Contains(filterText!) || e.DecisionRight!.Contains(filterText!) || e.UpcomingPotentialProjects!.Contains(filterText!) || e.OtherPJInformation!.Contains(filterText!) || e.FileName!.Contains(filterText!) || e.Note!.Contains(filterText!) || e.ApprovalStatus!.Contains(filterText!) || e.AccountNo!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(priceOfferCode), QueryFilterHelper.BuildMultiFieldSearch<PriceOffer>(priceOfferCode, e => e.PriceOfferCode))
                .WhereIf(!string.IsNullOrWhiteSpace(projectName), QueryFilterHelper.BuildMultiFieldSearch<PriceOffer>(projectName, e => e.ProjectName))
                .WhereIf(!string.IsNullOrWhiteSpace(priceOfferType), e => e.PriceOfferCode.Substring(0, 2) == priceOfferType)
                .WhereIf(createdFrom.HasValue, e => e.CreationTime!.Value.Date >= createdFrom!.Value.Date)
                .WhereIf(createdTo.HasValue, e => e.CreationTime!.Value.Date <= createdTo!.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(approvalStatus), e => e.ApprovalStatus == approvalStatus)
                .WhereIf(!string.IsNullOrWhiteSpace(materialType), e => e.MaterialType == materialType)
                .WhereIf(!string.IsNullOrWhiteSpace(customerName),
                    QueryFilterHelper.BuildNestedCollectionSearch<PriceOffer, PriceOfferCustomer>(
                        customerName,
                        e => e.Customers,
                        d => d.CustomerName
                        ))
                .WhereIf(!string.IsNullOrWhiteSpace(customerTaxCode), e =>
                    (e.KeyAccount != null &&
                     e.KeyAccount.CustomerTaxCode != null &&
                     e.KeyAccount.CustomerTaxCode.Contains(customerTaxCode!)) ||
                    e.Customers.Any(c =>
                        c.CustomerTaxCode != null &&
                        c.CustomerTaxCode.Contains(customerTaxCode!)))
                //.WhereIf(buyerId.HasValue, e => e.BuyerId == buyerId)
                .WhereIf(relatedToMe, x => x.CreatorUsername == currentUsername)
                .WhereIf(!string.IsNullOrWhiteSpace(projectResultStatus), e => e.ProjectResultStatus == projectResultStatus);

        var db = await GetDbContextAsync();
        var locationQuery = db.Set<SystemCategory>()
            .Where(x => x.CategoryType == "Location");

        var queryWithJoin =
            from po in preFiltered
            join cat in locationQuery.Where(c => c.CategoryType == CategoryTypes.Location)
                on po.LocationDescription equals cat.Description into catJoin
            from cat in catJoin.DefaultIfEmpty()
            select new
            {
                PriceOffer = po,
                LocationId = (Guid?)cat.Id
            };

        var filtered =
            queryWithJoin
                .ApplyBuyerFilter(filterParams, x => x.PriceOffer.BuyerId)
                .ApplyLocationFilter(filterParams, x => x.LocationId)
                .ApplyMaterialTypeFilter(filterParams, x => x.PriceOffer.MaterialType);

        return filtered.Select(x => x.PriceOffer);
    }

    public async Task<decimal> GetDiscountRatioAsync(Guid? keyAccountId)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var transaction = await GetDbTransactionAsync();

        const string sql = "SELECT dbo.ufn_PriceOffer_GetAutoDiscountRatio_AP(@keyAccountId)";
        var parameters = new { keyAccountId = keyAccountId };

        var discountRatio = await connection.ExecuteScalarAsync<decimal>(
            sql,
            parameters,
            transaction
        );

        return discountRatio;
    }
}
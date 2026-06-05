using QuoteFlow.AddMoreItemHistories;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.BuyerAccess;
using QuoteFlow.Buyers;
using QuoteFlow.CfgDiscountRatios;
using QuoteFlow.Customers;
using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.DistributorTargets;
using QuoteFlow.DPOs;
using QuoteFlow.KeyAccounts;
using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.Materials.MaterialGroups.ParameterObject;
using QuoteFlow.Materials.MaterialStocks;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PSIs;
using QuoteFlow.PurchaseOrders;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SaleOrders;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Models;
using QuoteFlow.Shared.Utils;
using QuoteFlow.SpecialInputPrices;
using QuoteFlow.StockCategories;
using QuoteFlow.SupplierBUs;
using QuoteFlow.Suppliers;
using QuoteFlow.SystemCategories;
using QuoteFlow.WorkflowApprovers;
using QuoteFlow.WorkflowConfigurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace QuoteFlow.GeneralLookups;

[RemoteService(IsEnabled = false)]
public class LookupsAppService : ApplicationService, ILookupsAppService
{
    protected IMemoryCache _lookupsCache;
    protected ISystemCategoryRepository _systemCategoryRepository;
    protected IBuyerRepository _buyerRepository;
    protected IStockCategoryRepository _stockCategoryRepository;
    protected IKeyAccountRepository _keyAccountRepository;
    protected IIdentityUserRepository _identityUserRepository;
    protected ISpecialInputPriceRepository _specialInputPriceRepository;
    protected IMaterialGroupRepository _materialGroupRepository;
    protected ISupplierRepository _supplierRepository;
    protected ISupplierBURepository _supplierBURepository;
    protected IMaterialStockRepository _materialStockRepository;
    protected IDistributorTargetRepository _distributorTargetRepository;
    protected IPurchaseOrderRepository _purchaseOrderRepository;
    protected IWorkflowConfigurationRepository _workflowConfigurationRepository;
    protected ISalesAssignmentRepository _salesAssignmentRepository;
    protected IEffectiveUserContext _effectiveUserContext;
    protected IBuyerAccessService _buyerAccessService;
    protected IWorkflowApproverRepository _workflowApproverRepository;
    protected IPSIRepository _psiRepository;
    protected IApprovalHistoryRepository _approvalHistoryRepository;
    protected IAddMoreItemHistoryRepository _addMoreItemHistoryRepository;
    protected ICfgDiscountRatioRepository _cfgDiscountRatioRepository;
    protected ILogger<LookupsAppService> _logger;
    protected ICustomerRepository _customerRepository;

    public LookupsAppService(IMemoryCache lookupsCache, ISystemCategoryRepository systemCategoryRepository, IIdentityUserRepository identityUserRepository, IBuyerRepository buyerRepository, IStockCategoryRepository stockCategoryRepository, IKeyAccountRepository keyAccountRepository, ISpecialInputPriceRepository specialInputPriceRepository, IMaterialGroupRepository materialGroupRepository, ISupplierRepository supplierRepository, ISupplierBURepository supplierBURepository, IMaterialStockRepository materialStockRepository, IDistributorTargetRepository distributorTargetRepository, IPurchaseOrderRepository purchaseOrderRepository, IWorkflowConfigurationRepository workflowConfigurationRepository, ISalesAssignmentRepository salesAssignmentRepository, IEffectiveUserContext effectiveUserContext, IBuyerAccessService buyerAccessService, IWorkflowApproverRepository workflowApproverRepository, IPSIRepository psiRepository, ILogger<LookupsAppService> logger, IApprovalHistoryRepository approvalHistoryRepository, ICfgDiscountRatioRepository cfgDiscountRatioRepository, IAddMoreItemHistoryRepository addMoreItemHistoryRepository, ICustomerRepository customerRepository)
    {
        _lookupsCache = lookupsCache;
        _systemCategoryRepository = systemCategoryRepository;
        _identityUserRepository = identityUserRepository;
        _buyerRepository = buyerRepository;
        _stockCategoryRepository = stockCategoryRepository;
        _keyAccountRepository = keyAccountRepository;
        _specialInputPriceRepository = specialInputPriceRepository;
        _materialGroupRepository = materialGroupRepository;
        _supplierRepository = supplierRepository;
        _supplierBURepository = supplierBURepository;
        _materialStockRepository = materialStockRepository;
        _distributorTargetRepository = distributorTargetRepository;
        _salesAssignmentRepository = salesAssignmentRepository;
        _effectiveUserContext = effectiveUserContext;
        _buyerAccessService = buyerAccessService;
        _workflowApproverRepository = workflowApproverRepository;
        _purchaseOrderRepository = purchaseOrderRepository;
        _workflowConfigurationRepository = workflowConfigurationRepository;
        _psiRepository = psiRepository;
        _logger = logger;
        _approvalHistoryRepository = approvalHistoryRepository;
        _cfgDiscountRatioRepository = cfgDiscountRatioRepository;
        _addMoreItemHistoryRepository = addMoreItemHistoryRepository;
        _customerRepository = customerRepository;
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetEUIndustryLookupAsync()
    {
        //var cacheKey = "EUIndustryLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.EUIndustry && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
                {
                    Id = items.Id,
                    DisplayCode = items.Code,
                    DisplayName = items.Description
                })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetCustomerTypeLookupAsync()
    {
        //var cacheKey = "CustomerTypeLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.CustomerType && x.IsDeactive == false);
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}
        //return lookup ?? new();
    }


    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountClassLookupAsync()
    {
        //var cacheKey = "KeyAccountClassLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.KeyAccountClassify && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountEvaluationFinancialLookupAsync()
    {
        //var cacheKey = "KeyAccountEvaluationFinancialLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.KeyAccountEvaluationFinancial && x.IsDeactive == false);
        items = items.OrderBy(x => x.SortOrder).ToList();
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountEvaluationProductLookupAsync()
    {
        //var cacheKey = "KeyAccountEvaluationProductLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.KeyAccountEvaluationProduct && x.IsDeactive == false);
        items = items.OrderBy(x => x.SortOrder).ToList();
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountTypeLookupAsync()
    {
        //var cacheKey = "KeyAccountTypeLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = (await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.KeyAccountType && x.IsDeactive == false)).OrderBy(x => x.SortOrder);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }



    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLocationLookupAsync()
    {
        //var cacheKey = "LocationLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.Location && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetMaterialGroupLookupAsync()
    {
        //var cacheKey = "MaterialGroupLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _materialGroupRepository.GetListAsync(x => x.IsDeActive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Name
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetMaterialTypeLookupAsync()
    {
        //var cacheKey = "MaterialTypeLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.MaterialType && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetWareHouseLookupAsync()
    {
        //var cacheKey = "MaterialTypeLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.AssetLocation && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProjectTypeLookupAsync()
    {
        //var cacheKey = "ProjectTypeLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.ProjectType && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetVendorLookupAsync()
    {
        //var cacheKey = "VendorLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.Vendor && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<List<Shared.UserLookupDto>> GetUserLookupAsync()
    {
        var user = await _identityUserRepository.GetListAsync(notActive: false);
        var result = user
            .Select(user => new Shared.UserLookupDto
            {
                Id = user.Id,
                FullName = UserHelper.GetFullName(user.Name, user.Surname),
                UserName = user.UserName,
                Email = user.Email,

                PhoneNumber = user.PhoneNumber
            })
            .ToList();
        return result ?? [];
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetBuyerLookupAsync(bool byPassBuyerCheck)
    {
        //var cacheKey = "BuyerLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        //var hasApprovalPermission = await _workflowApproverRepository.AnyAsync(x => x.Approver == _effectiveUserContext.Username);
        if (byPassBuyerCheck)
        {
            buyerAccess.HasFullAccess = true;
        }

        // Return empty list silently if user has no access
        if (!buyerAccess.HasGeneralAccess())
        {
            return new ListResultDto<LookupDto<Guid>>(new List<LookupDto<Guid>>());
        }

        var items = await _buyerRepository.GetListAsync(x =>
            x.Deactive == false &&
            x.BuyerTypeCode != "GIC" && // Exclude GIC buyers since it's only contains default buyer
            (buyerAccess.HasFullAccess || buyerAccess.RestrictedBuyerIds.Contains(x.Id)));

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.ShortName,
                DisplayName = items.FullName
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetBuyerLookupByBuyerTypeAsync(Guid buyerTypeId)
    {
        //var cacheKey = $"Buyer:{buyerTypeId}"; ;
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();

        // Return empty list silently if user has no access
        if (!buyerAccess.HasGeneralAccess())
        {
            return new ListResultDto<LookupDto<Guid>>(new List<LookupDto<Guid>>());
        }

        //var items = await _buyerRepository.GetListAsync(new() { BuyerTypeId = buyerTypeId, Deactive = false });
        var items = await _buyerRepository.GetListAsync(x =>
            x.Deactive == false
            && x.BuyerTypeId == buyerTypeId
            && (buyerAccess.HasFullAccess || buyerAccess.RestrictedBuyerIds.Contains(x.Id)));

        items = items.OrderBy(x => x.ShortName).ToList();
        if (items is null)
        {
            return new();
        }
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.ShortName,
                DisplayName = items.FullName
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetBuyerTypeLookupAsync(GetBuyerTypeLookupInput input)
    {
        //var cacheKey = "BuyerType";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        List<string> notLoadAsDefaultBuyerTypes = [
            "GIC"
        ];
        Expression<Func<SystemCategory, bool>> predicate = x =>
            x.CategoryType == CategoryTypes.BuyerType
            && x.IsDeactive == false;

        // Apply additional filters based on input
        if (!string.IsNullOrEmpty(input.BuyerTypeCode))
        {
            // If specific BuyerTypeCode is provided, filter by that code
            predicate = predicate.And(x => x.Code == input.BuyerTypeCode);
        }
        else
        {
            // If no specific code, exclude GIC types unless they're explicitly requested
            predicate = predicate.And(x => !notLoadAsDefaultBuyerTypes.Contains(x.Code));
        }

        var items = await _systemCategoryRepository.GetListAsync(predicate);

        items = [.. items.OrderBy(x => x.SortOrder)];
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetNationalityLookupAsync()
    {
        //var cacheKey = "Nationality";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.Nationality && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }
    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetDamagedStockCategoryLookupAsync()
    {
        //var cacheKey = "StockCategory";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _stockCategoryRepository.GetListAsync(x => x.DamagedStock == true && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.StockCode,
                DisplayName = items.StockName
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }


    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetStockCategoryLookupAsync()
    {
        //var cacheKey = "StockCategory";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _stockCategoryRepository.GetListAsync(x => x.DamagedStock != true && x.IsDeactive == false);
        items = items.OrderBy(x => x.SortOrder).ToList();

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.StockCode,
                DisplayName = items.StockName
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }
    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetFOCStockCategoryLookupAsync()
    {
        //var cacheKey = "StockCategory";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _stockCategoryRepository.GetListAsync(x => x.FOC == true && x.IsDeactive == false);
        items = items.OrderBy(x => x.SortOrder).ToList();

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.StockCode,
                DisplayName = items.StockName
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }
    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetMainStockCategoryLookupAsync()
    {
        //var cacheKey = "StockCategory";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _stockCategoryRepository.GetListAsync(x => x.FOC != true && x.DamagedStock != true && x.IsDeactive == false);
        items = items.OrderBy(x => x.SortOrder).ToList();

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.StockCode,
                DisplayName = items.StockName
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<StockCategoryLookupDto<Guid>>> GetStockCategoryLookupWithAvailableAmountAsync(string materialCode, bool? damagedStock)
    {
        var items = await _stockCategoryRepository.GetListAsync(x => x.IsDeactive == false && x.DamagedStock == damagedStock && x.FOC != true);

        var materialStocks = await _materialStockRepository.GetListAsync(x => x.GolfaCode == materialCode);

        var lookup = new ListResultDto<StockCategoryLookupDto<Guid>>()
        {
            Items = [..items.Select(items => new StockCategoryLookupDto<Guid>()
                {
                    Id = items.Id,
                    DisplayCode = items.StockCode,
                    DisplayName = items.StockCode + " - " + items.StockName,
                    AvailableQuantity  = materialStocks
                        .Where(x => x.StockCategoryId == items.Id)
                        .Sum(x => x.Available_Qty) ?? 0
                })]
        };
        return lookup;
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetCurrencyCategoryLookupAsync()
    {
        //var cacheKey = "Currency";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.Currency && x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<KeyAccountLookupDto<Guid>>> GetKeyAccountLookupAsync(Guid? buyerId, string? materialType)
    {
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        if (!buyerAccess.HasGeneralAccess()
            || (buyerId == null || buyerId == Guid.Empty)
            || (!buyerAccess.HasFullAccess && !buyerAccess.RestrictedBuyerIds.Contains(buyerId.Value))
        )
        {
            return new ListResultDto<KeyAccountLookupDto<Guid>>(new List<KeyAccountLookupDto<Guid>>());
        }

        var keyAccounts = await _keyAccountRepository.GetListAsync(new()
        {
            BuyerId = buyerId,
            Status = QuoteFlowStatuses.Approved
        });

        // Filter by materialType if provided
        if (!string.IsNullOrWhiteSpace(materialType))
        {
            keyAccounts = keyAccounts
                .Where(ka => string.Equals(ka.MaterialType, materialType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (keyAccounts == null || keyAccounts.Count == 0)
        {
            return new ListResultDto<KeyAccountLookupDto<Guid>>(new List<KeyAccountLookupDto<Guid>>());
        }

        IEnumerable<string> classOrTypeCategoryTypes = [
            CategoryTypes.KeyAccountClassify,
            CategoryTypes.KeyAccountType
        ];

        var systemCategories = await _systemCategoryRepository.GetListAsync(x =>
            classOrTypeCategoryTypes.Contains(x.CategoryType) && x.IsDeactive == false
        );

        var lookup = new ListResultDto<KeyAccountLookupDto<Guid>>(
            [.. keyAccounts.Select(item =>
                {
                    var keyAccountClass = systemCategories.FirstOrDefault(sc =>
                        sc.CategoryType == CategoryTypes.KeyAccountClassify &&
                        sc.Code == item.KeyAccountClass);

                    var keyAccountType = systemCategories.FirstOrDefault(sc =>
                        sc.CategoryType == CategoryTypes.KeyAccountType &&
                        sc.Code == item.KeyAccountType);

                    return new KeyAccountLookupDto<Guid>
                    {
                        Id = item.Id,
                        DisplayName = item.KeyAccountName ?? "",

                        KeyAccountClassName = item.KeyAccountClass ?? "",
                        KeyAccountTypeName = item.KeyAccountType ?? "",

                        KeyAccountClassId = keyAccountClass?.Id ?? Guid.Empty,
                        KeyAccountTypeId = keyAccountType?.Id ?? Guid.Empty
                    };
                })
            ]);
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime(1));
        return lookup;
        //}
        //return lookup ?? new();
    }
    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetFinancialCategoryLookupAsync()
    {
        //var cacheKey = "Financial";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.KeyAccountEvaluationFinancial && x.IsDeactive == false);
        items = items.OrderBy(x => x.SortOrder).ToList();
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProductCategoryLookupAsync()
    {
        //var cacheKey = "Product";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.KeyAccountEvaluationProduct && x.IsDeactive == false);
        items = items.OrderBy(x => x.SortOrder).ToList();
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }
    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetKeyAccountClassChildLookupAsync(string keyAccountTypeCode, bool hiddenNA)
    {
        //var cacheKey = $"KeyAccountType:{keyAccountTypeCode}";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var item = await _systemCategoryRepository.FirstOrDefaultAsync(x =>
            x.Code == keyAccountTypeCode &&
            x.CategoryType == CategoryTypes.KeyAccountType &&
            x.IsDeactive == false);

        if (item is null)
        {
            return new();
        }

        var items = await _systemCategoryRepository.GetListAsync(x =>
            x.CategoryType == CategoryTypes.KeyAccountClassify &&
            x.ParentId == item.Id &&
            x.IsDeactive == false
            );
        items = items.OrderBy(x => x.SortOrder).ToList();
        var resultItems = items.Select(i => new LookupDto<Guid>
        {
            Id = i.Id,
            DisplayCode = i.Code,
            DisplayName = i.Description
        }).ToList();
        if (!hiddenNA)
        {
            if (!string.Equals(item.Code, "Global_Account", StringComparison.OrdinalIgnoreCase))
            {
                resultItems.Add(new LookupDto<Guid>
                {
                    Id = Guid.Empty,
                    DisplayCode = "N/A",
                    DisplayName = "N/A"
                });
            }
        }

        var lookup = new ListResultDto<LookupDto<Guid>>
        {
            Items = resultItems
        };

        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }



    public virtual async Task<ListResultDto<SpecialInputPriceLookupDto<Guid>>> GetSpecialInputPriceLookupAsync(string? materialType)
    {
        //var cacheKey = "SpecialInputPriceLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<SpecialInputPriceLookupDto<Guid>>? lookup))
        //{
        var items = await _specialInputPriceRepository.GetListAsync(x =>
        (x.Status == QuoteFlowStatuses.SpecialInputPrice.Valid || x.Status == QuoteFlowStatuses.SpecialInputPrice.Expired)
        && (string.IsNullOrWhiteSpace(materialType) || x.MaterialType == materialType)
    );

        var lookup = new ListResultDto<SpecialInputPriceLookupDto<Guid>>()
        {
            Items = [..items.Select(item => new SpecialInputPriceLookupDto<Guid>()
            {
                Id = item.Id,
                AccountNo = item.AccountNo,
                AccountName = item.AccountName,
                Status = item.Status,
                MaterialType = item.MaterialType
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}
        //return lookup ?? new();
    }
    public async Task<List<SupplierPOLookupDto>> GetSupplierPOLookupAsync(string? materialType, string? currency, string? createSource, bool? epa)
    {
        _logger.LogInformation("GetSupplierPOLookupAsync called with materialType: {MaterialType}, currency: {Currency}, createSource: {CreateSource}, epa: {Epa}", materialType, currency, createSource, epa);
        var items = await _purchaseOrderRepository.PurchaseOrderSupplierBUAsync(materialType, currency, createSource, epa);

        _logger.LogInformation("Retrieved {Count} items from PurchaseOrderSupplierBUAsync", items.Count);
        return [.. items.Select(item => new SupplierPOLookupDto
        {
            SupplierId = item.SupplierId,
            SupplierBUId = item.SupplierBUId,
            SupplierCode = item.SupplierCode,
            SupplierBUCode = item.SupplierBUCode,

        })];
    }
    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSupplierLookupAsync()
    {
        //var cacheKey = "SupplierLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _supplierRepository.GetListAsync(x => x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(item => new LookupDto<Guid>()
            {
                Id = item.Id,
                DisplayName = item.ShortName,
                DisplayCode = item.SupplierCode
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}
        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSupplierBULookupAsync()
    {
        //var cacheKey = "SupplierBULookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var items = await _supplierBURepository.GetListAsync(x => x.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(item => new LookupDto<Guid>()
            {
                Id = item.Id,
                DisplayName = item.SupplierBURemarks,
                DisplayCode = item.SupplierBUCode
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}
        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<SupplierBULookupDto<Guid>>> GetSupplierBUBySupplierLookupAsync(Guid supplierId)
    {
        //var cacheKey = $"SupplierBULookup{supplierId}";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<SupplierBULookupDto<Guid>>? lookup))
        //{
        var items = await _supplierBURepository.GetListAsync(new() { SupplierId = supplierId });

        var lookup = new ListResultDto<SupplierBULookupDto<Guid>>()
        {
            Items = [..items.Where(item => item.IsDeactive == false).Select(item => new SupplierBULookupDto<Guid>()
            {
                Id = item.Id,
                DisplayName = item.SupplierBURemarks,
                DisplayCode = item.SupplierBUCode,
                Currency = item.Currency,
                MaterialType = item.MaterialType
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}
        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<AccountCodeLookupDto>> GetAccountNoAsync(string materialCode)
    {
        var items = await _specialInputPriceRepository.GetDetailsByMaterialCodeAsync(materialCode);
        var lookup = new ListResultDto<AccountCodeLookupDto>()
        {
            Items = [..items.Select(item => new AccountCodeLookupDto()
            {
                AccountNo = item.AccountNo,
                AccountName = item.AccountName,
                InputPrice = item.InputPrice,
                LandedCost = item.LandedCost,
                MaterialCode = item.MaterialCode,
                Status = item.Status
            })]
        };
        return lookup ?? new();
    }

    public async Task<List<Shared.UserLookupDto>> GetListUserLookup(string name)
    {
        var users = await _identityUserRepository.GetListAsync(notActive: false);

        var result = users
             .WhereIf(!string.IsNullOrWhiteSpace(name),
                e => e.UserName != null && e.UserName.ToLower().Contains(name.ToLower()))
            .Select(user => new Shared.UserLookupDto
            {
                Id = user.Id,
                FullName = UserHelper.GetFullName(user.Name, user.Surname),
                UserName = user.UserName,
                Email = user.Email,

                PhoneNumber = user.PhoneNumber
            })
            .ToList();
        return result ?? [];
    }

    public async Task<List<Shared.UserLookupDto>> GetListAllUserLookup()
    {
        var users = await _identityUserRepository.GetListAsync(notActive: false);

        var result = users
            .Select(user => new Shared.UserLookupDto
            {
                Id = user.Id,
                FullName = UserHelper.GetFullName(user.Name, user.Surname),
                UserName = user.UserName,
                Email = user.Email,

                PhoneNumber = user.PhoneNumber
            })
            .ToList();
        return result ?? [];
    }
    //get user PIC
    public async Task<List<Shared.UserLookupDto>> GetListUserPICLookup(GetSalePICLookupInput input)
    {
        var name = input.UserName;
        var materialType = input.MaterialType;
        var users = await _salesAssignmentRepository.GetListAsync(x => x.MaterialType == materialType);

        var result = users
            .WhereIf(!string.IsNullOrWhiteSpace(name),
                e => e.SaleUserName != null && e.SaleUserName.Contains(name, StringComparison.OrdinalIgnoreCase))
            .DistinctBy(e => e.SaleUserName?.ToLower())
            .Select(user => new Shared.UserLookupDto
            {
                Id = user.Id,
                FullName = user.SaleFullName,
                UserName = user.SaleUserName,
                Email = null,
                PhoneNumber = null
            })
            .ToList();

        return result ?? new List<Shared.UserLookupDto>();
    }
    public async Task<List<Shared.UserLookupDto>> GetAllSalePICLookupAsync()
    {
        var users = await _salesAssignmentRepository.GetListAsync();

        var result = users
            .DistinctBy(e => e.SaleUserName?.ToLower())
            .Select(user => new Shared.UserLookupDto
            {
                Id = user.Id,
                FullName = user.SaleFullName,
                UserName = user.SaleUserName,
                Email = null,
                PhoneNumber = null
            })
            .ToList();

        return result ?? new List<Shared.UserLookupDto>();
    }

    public virtual Task<ListResultDto<int>> GetYearLookupAsync()
    {
        var currentYear = DateTime.UtcNow.Year;
        var startYear = currentYear - 3;
        var endYear = currentYear + 3;

        var years = Enumerable.Range(startYear, endYear - startYear + 1).ToList();

        return Task.FromResult(new ListResultDto<int>(years));
    }

    public virtual async Task<ListResultDto<int?>> GetFiscalYearOfDistributorTargetLookupAsync()
    {
        var distributorTarget = await _distributorTargetRepository.GetListAsync(new());

        var fiscalYears = distributorTarget
            .Where(x => x.FinanceYear.HasValue)
            .Select(x => x.FinanceYear)
            .Distinct()
            .OrderByDescending(x => x)
            .ToList();

        return new ListResultDto<int?>(fiscalYears);
    }




    public virtual async Task<ListResultDto<GicTypeLookupDto<Guid>>> GetGicTypesLookupAsync()
    {
        //var cacheKey = "GicTypesLookup";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<GicTypeLookupDto<Guid>>? lookup))
        //{
        var gicTypes = await _systemCategoryRepository.GetListAsync(x =>
            x.CategoryType == CategoryTypes.GICType && x.IsDeactive == false);

        var allGicCategories = await _systemCategoryRepository.GetListAsync(x =>
            x.CategoryType == CategoryTypes.GICProcess && x.IsDeactive == false);

        var lookup = new ListResultDto<GicTypeLookupDto<Guid>>()
        {
            Items = gicTypes
                .OrderBy(gt => gt.SortOrder)
                .Select(gicType =>
                {
                    var processes = allGicCategories
                        .Where(p => p.ParentId == gicType.Id)
                        .Select(p => new LookupDto<Guid>
                        {
                            Id = p.Id,
                            DisplayCode = p.Code,
                            DisplayName = p.Description
                        })
                        .ToList();

                    return new GicTypeLookupDto<Guid>
                    {
                        Id = gicType.Id,
                        DisplayCode = gicType.Code,
                        DisplayName = gicType.Description,
                        HasProcess = processes.Count != 0,
                        Processes = processes
                    };
                }).ToList()
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSupplierByMaterialTypeLookupAsync(string materialType)
    {
        //var cacheKey = $"SupplierByMaterialTypeLookup_{materialType}";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        // Get distinct suppliers that have supplier BUs with the specified material type
        var supplierIds = await _supplierBURepository.GetQueryableAsync();
        var distinctSupplierIds = supplierIds
            .Where(sb => sb.MaterialType == materialType && sb.SupplierId.HasValue && sb.IsDeactive == false)
            .Select(sb => sb.SupplierId.Value)
            .Distinct()
            .ToList();

        var suppliers = await _supplierRepository.GetListAsync(s => distinctSupplierIds.Contains(s.Id) && s.IsDeactive == false);

        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..suppliers.Select(item => new LookupDto<Guid>()
            {
                Id = item.Id,
                DisplayCode = item.SupplierCode,
                DisplayName = item.ShortName
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}
        //return lookup ?? new();
    }

    public virtual async Task<ListResultDto<SupplierBULookupDto<Guid>>> GetSupplierBUBySupplierAndMaterialTypeLookupAsync(Guid supplierId, string materialType)
    {
        //var cacheKey = $"SupplierBUBySupplierAndMaterialTypeLookup_{supplierId}_{materialType}";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<SupplierBULookupDto<Guid>>? lookup))
        //{
        var items = await _supplierBURepository.GetListAsync(new() { SupplierId = supplierId });
        var filteredItems = items.Where(sb => sb.MaterialType == materialType && sb.IsDeactive == false).ToList();

        var lookup = new ListResultDto<SupplierBULookupDto<Guid>>()
        {
            Items = [..filteredItems.Select(item => new SupplierBULookupDto<Guid>()
            {
                Id = item.Id,
                DisplayCode = item.SupplierBUCode,
                DisplayName = item.SupplierBURemarks ?? item.SupplierBUCode,
                MaterialType = item.MaterialType,
                Currency = item.Currency
            })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}
        //return lookup ?? new();
    }

    private DateTimeOffset GetCacheExpirationTime(int hours = 3)
    {
        return DateTimeOffset.UtcNow.AddHours(hours);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetMaterialGroupPSILookupAsync(string materialType)
    {
        if (materialType.ToUpper().Equals("FA"))
        {
            materialType = "PSI-FA";
        }
        else if (materialType.ToUpper().Equals("LVS"))
        {
            materialType = "PSI-LVS";
        }
        else
        {
            return new ListResultDto<LookupDto<Guid>>();
        }
        var items = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == materialType);
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.Code,
                DisplayName = items.Description
            })]
        };

        return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetBuyersNotAssignedToMaterialGroupAsync()
    {
        var items = await _buyerRepository.GetBuyersNotAssignedToMaterialGroupAsync();
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.BuyerCode,
                DisplayName = items.ShortName
            })]
        };

        return lookup ?? new();
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetMaterialGroupByTypeAsync(string type)
    {
        type = type.ToUpper();
        //var cacheKey = $"MaterialGroupType_{type}";
        //if (!_lookupsCache.TryGetValue(cacheKey, out ListResultDto<LookupDto<Guid>>? lookup))
        //{
        var filterParams = new MaterialGroupFilterParams();
        filterParams.MaterialType = type;
        var items = await _materialGroupRepository.GetListAsync(filterParams);
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
        {
            Id = items.Id,
            DisplayCode = items.Code,
            DisplayName = items.Name
        })]
        };
        //_lookupsCache.Set(cacheKey, lookup, GetCacheExpirationTime());
        return lookup;
        //}

        //return lookup ?? new();
    }

    public virtual async Task<List<ApprovalHistoryDto>> GetDPOApprovalHistoriesAsync(Guid id)
    {
        var queryable = await _approvalHistoryRepository.GetQueryableAsync();
        var histories = await AsyncExecuter.ToListAsync(
            queryable
                .OfType<DPOApprovalHistory>()
                .Where(x => x.DPOId == id)
        );


        return histories.Select(history => new ApprovalHistoryDto
        {
            Id = history.Id,
            EntityType = history.EntityType,
            ApproverRoleCode = history.ApproverRoleCode,
            ApproverRoleName = history.ApproverRoleName,
            ApproverUsername = history.ApproverUsername,
            ApproverFullName = history.ApproverFullName,
            Action = history.Action,
            ActionDate = history.ActionDate,
            Note = history.Note,
            IsLastApprovalInCurrentWorkflow = history.IsLastApprovalInCurrentWorkflow,
            ConcurrencyStamp = history.ConcurrencyStamp,
        }).ToList();
    }

    public virtual async Task<List<ApprovalHistoryDto>> GetSOHistoriesAsync(Guid id)
    {
        var queryable = await _approvalHistoryRepository.GetQueryableAsync();
        var histories = await AsyncExecuter.ToListAsync(
            queryable
                .OfType<SOHistory>()
                .Where(x => x.SOId == id)
        );


        return histories.Select(history => new ApprovalHistoryDto
        {
            Id = history.Id,
            EntityType = history.EntityType,
            ApproverRoleCode = history.ApproverRoleCode,
            ApproverRoleName = history.ApproverRoleName,
            ApproverUsername = history.ApproverUsername,
            ApproverFullName = history.ApproverFullName,
            Action = history.Action,
            ActionDate = history.ActionDate,
            Note = history.Note,
            IsLastApprovalInCurrentWorkflow = history.IsLastApprovalInCurrentWorkflow,
            ConcurrencyStamp = history.ConcurrencyStamp,
        }).ToList();
    }

    public virtual async Task<List<AddMoreItemHistoryDto>> AddMoreItemHistoryAsync(Guid id)
    {
        var history = await _approvalHistoryRepository.GetAsync(id);

        Guid? importGuid = null;

        if (history is PriceOfferApprovalHistory priceOfferHistory)
        {
            importGuid = priceOfferHistory.ImportGuid;
        }
        else if (history is PriceOfferDetailApprovalHistory detailHistory)
        {
            importGuid = detailHistory.ImportGuid;
        }

        if (importGuid.HasValue)
        {
            var historyAddMore = await _addMoreItemHistoryRepository.GetListAsync(x => x.ImportGuid == importGuid);
            return ObjectMapper.Map<List<AddMoreItemHistory>, List<AddMoreItemHistoryDto>>(historyAddMore);
        }

        return new List<AddMoreItemHistoryDto>();
    }


    public virtual async Task<ListResultDto<int?>> GetYearLookupKeyAccountAsync()
    {
        var keyAccounts = await _keyAccountRepository.GetListAsync();

        var years = keyAccounts
            .Where(x => x.RegistrationYear.HasValue)
            .Select(x => (int?)x.RegistrationYear)
            .Distinct()
            .OrderByDescending(y => y)             // order
            .ToList();

        return new ListResultDto<int?>(years);
    }

    public virtual async Task<ListResultDto<short?>> GetLevelLookupWorkflowAsync(string type)
    {
        var workflows = await _workflowConfigurationRepository.GetListAsync();

        var levels = workflows.Where(w => w.WorkflowLevel > 0 && w.WorkflowType == type)
            .Select(w => (short?)w.WorkflowLevel)
            .Distinct()
            .OrderBy(y => y)             // order
            .ToList();
        return new ListResultDto<short?>(levels);
    }

    public virtual async Task<ListResultDto<string?>> GetConditionLookupWorkflowAsync(string type)
    {
        var workflows = await _workflowConfigurationRepository.GetListAsync();

        var conditions = workflows.Where(w => w.Condition is not null && w.WorkflowType == type)
            .Select(w => (string?)w.Condition)
            .Distinct()
            .OrderBy(y => y)             // order
            .ToList();
        return new ListResultDto<string?>(conditions);
    }

    public virtual async Task<ListResultDto<int?>> GetYearDistinctPSIAsync()
    {
        var psis = await _psiRepository.GetListAsync();

        var years = psis
            .Select(w => (int?)w.FY)
            .Distinct()
            .OrderByDescending(y => y)             // order
            .ToList();
        return new ListResultDto<int?>(years);
    }

    // LookupsAppService.cs


    public virtual async Task<List<string>> GetSpoTypeLookupAsync()
    {
        var items = await _cfgDiscountRatioRepository.GetListAsync();

        var result = items
            .Select(x => x.Approval_Type)
            .Where(type => type != "NB") // Remove items with value "NB"
            .Distinct()
            .ToList();

        return result;
    }


    public virtual async Task<List<string?>> GetKAbySPOAsync(string spoType)
    {
        // Get list AccountClassify from CFG_DiscountRatio
        var itemCfgs = await _cfgDiscountRatioRepository.GetListAsync(x => x.Approval_Type == spoType);
        var accountClassifyList = itemCfgs
            .Select(x => x.AccountClassify)
            .Distinct()
            .ToList();

        // Get ParentId from SystemCategories with CategoryType = Key_Account_Classify
        // and Code in accountClassifyList
        var kaClassifyList = await _systemCategoryRepository.GetListAsync(x =>
            x.CategoryType == CategoryTypes.KeyAccountClassify
            && accountClassifyList.Contains(x.Code));

        var parentIds = kaClassifyList
            .Select(x => x.ParentId)
            .Distinct()
            .ToList();

        // Get list SystemCategories with CategoryType = Key_Account_Type
        //and Id in parentIds
        var result = await _systemCategoryRepository.GetListAsync(x =>
            x.CategoryType == CategoryTypes.KeyAccountType
            && parentIds.Contains(x.Id));

        return result.Select(x => x.Code).ToList();


    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetCustomerTaxCodeLookupAsync(GetCustomersInput input)
    {
        var filterParams = ObjectMapper.Map<GetCustomersInput, CustomerFilterParams>(input);
        var items = await _customerRepository.GetListAsync(filterParams);
        var lookup = new ListResultDto<LookupDto<Guid>>()
        {
            Items = [..items.Select(items => new LookupDto<Guid>()
            {
                Id = items.Id,
                DisplayCode = items.TaxCode,
                DisplayName = items.CustomerName,
            })]
        };

        return lookup ?? new();
    }

}

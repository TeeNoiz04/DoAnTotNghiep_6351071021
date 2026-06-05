using ClosedXML.Excel;
using QuoteFlow.AddMoreItemHistories;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using QuoteFlow.ApprovalRoutes;
using QuoteFlow.BackgroundJobs.Emailing;
using QuoteFlow.BuyerAccess;
using QuoteFlow.Emailing;
using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Emailing.EmailModels;
using QuoteFlow.Materials;
using QuoteFlow.Messages;
using QuoteFlow.Permissions;
using QuoteFlow.PriceOffers.Events;
using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferMessages;
using QuoteFlow.PriceOffers.PriceOfferReportDetails;
using QuoteFlow.PriceOffers.PriceOfferReportDetails.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals.ParameterObjects;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.Flagging;
using QuoteFlow.Shared.Models;
using QuoteFlow.Shared.Utils;
using QuoteFlow.SpecialInputPrices;
using QuoteFlow.SystemConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.FileManagement.Files;

namespace QuoteFlow.PriceOffers;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.PriceOffers.Default)]
public class PriceOffersAppService : QuoteFlowAppService, IPriceOffersAppService
{
    protected readonly IDistributedCache<PriceOfferDownloadTokenCacheItem, string> _downloadTokenCache;
    protected readonly ILogger<PriceOffersAppService> _logger;
    protected readonly IPriceOfferRepository _priceOfferRepository;
    protected readonly PriceOfferManager _priceOfferManager;
    protected readonly IPriceOfferDetailRepository _priceOfferDetailRepository;
    protected readonly PriceOfferDetailManager _priceOfferDetailManager;
    protected readonly IPriceOfferCustomerRepository _priceOfferCustomerRepository;
    protected readonly PriceOfferCustomerManager _priceOfferCustomerManager;
    protected readonly IExcelImportFactory _excelImportFactory;
    protected readonly ILocalEventBus _localEventBus;
    protected readonly IEffectiveUserContext _currentUser;
    protected readonly ApprovalRouteManager _approvalRouteManager;
    protected readonly IApprovalRouteRepository _approvalRouteRepository;
    protected readonly IIdentityUserRepository _identityUserRepository;
    protected readonly ISalesAssignmentRepository _salesAssignmentRepository;
    protected readonly ISpecialInputPriceRepository _specialInputPriceRepository;
    protected readonly IFlaggingService<PriceOffer, PriceOfferFlagsDto> _flaggingService;
    protected readonly ICustomerEnrichmentService _customerEnrichmentService;
    protected readonly IMaterialRepository _materialRepository;
    protected readonly IEmailJobScheduler _emailJobScheduler;
    protected readonly ISystemConfigurationRepository _systemConfigurationRepository;
    protected readonly IAddMoreItemHistoryRepository _addMoreItemHistoryRepository;
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    private readonly FileDescriptorAppService _fileDescriptorAppService;
    private readonly IBuyerAccessService _buyerAccessService;
    public PriceOffersAppService(
        IDistributedCache<PriceOfferDownloadTokenCacheItem, string> downloadTokenCache,
        ILogger<PriceOffersAppService> logger,
        IPriceOfferRepository priceOfferRepository,
        PriceOfferManager priceOfferManager,
        IExcelImportFactory excelValidatorFactory,
        IPriceOfferDetailRepository priceOfferDetailRepository,
        ILocalEventBus localEventBus,
        PriceOfferDetailManager priceOfferDetailManager,
        IEffectiveUserContext effectiveUserContext,
        ApprovalRouteManager approvalRouteManager,
        IApprovalRouteRepository approvalRouteRepository,
        IIdentityUserRepository identityUserRepository,
        ISalesAssignmentRepository salesAssignmentRepository,
        IFlaggingService<PriceOffer, PriceOfferFlagsDto> flaggingService,
        IPriceOfferCustomerRepository priceOfferCustomerRepository,
        PriceOfferCustomerManager priceOfferCustomerManager,
        ISpecialInputPriceRepository specialInputPriceRepository,
        ICustomerEnrichmentService customerEnrichmentService,
        IEmailJobScheduler emailJobScheduler,
        ISystemConfigurationRepository systemConfigurationRepository,
        FileDescriptorAppService fileDescriptorAppService,
        IRepository<FileDescriptor, Guid> fileDescriptorRepository,
        IBuyerAccessService buyerAccessService,
        IMaterialRepository materialRepository,
        IAddMoreItemHistoryRepository addMoreItemHistoryRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _logger = logger;
        _priceOfferRepository = priceOfferRepository;
        _priceOfferManager = priceOfferManager;
        _excelImportFactory = excelValidatorFactory;
        _priceOfferDetailRepository = priceOfferDetailRepository;
        _localEventBus = localEventBus;
        _priceOfferDetailManager = priceOfferDetailManager;
        _currentUser = effectiveUserContext;
        _approvalRouteManager = approvalRouteManager;
        _approvalRouteRepository = approvalRouteRepository;
        _identityUserRepository = identityUserRepository;
        _salesAssignmentRepository = salesAssignmentRepository;
        _flaggingService = flaggingService;
        _priceOfferCustomerRepository = priceOfferCustomerRepository;
        _priceOfferCustomerManager = priceOfferCustomerManager;
        _specialInputPriceRepository = specialInputPriceRepository;
        _customerEnrichmentService = customerEnrichmentService;
        _emailJobScheduler = emailJobScheduler;
        _systemConfigurationRepository = systemConfigurationRepository;
        _fileDescriptorAppService = fileDescriptorAppService;
        _fileDescriptorRepository = fileDescriptorRepository;
        _buyerAccessService = buyerAccessService;
        _materialRepository = materialRepository;
        _addMoreItemHistoryRepository = addMoreItemHistoryRepository;
    }

    public virtual async Task<PagedResultDto<PriceOfferWithNavigationListDto>> GetListAsync(GetPriceOffersInput input)
    {
        var filterParams = ObjectMapper.Map<GetPriceOffersInput, PriceOfferFilterParams>(input);

        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        filterParams.ApplyBuyerRestrictions(buyerAccess);
        filterParams.ApplyLocationRestrictions(buyerAccess);
        filterParams.ApplyMaterialTypeRestrictions(buyerAccess);

        var totalCount = await _priceOfferRepository.GetCountAsync(filterParams, _currentUser.Username);
        var items = await _priceOfferRepository.GetListAsync(filterParams, _currentUser.Username, input.Sorting, input.MaxResultCount, input.SkipCount);

        var flags = await _flaggingService.CreateBulkFlagsAsync(items);
        var itemDtos = ObjectMapper.Map<List<PriceOffer>, List<PriceOfferWithNavigationListDto>>(items);

        itemDtos.ForEach(i =>
        {
            i.Flags = flags[i.Id];
            i.TotalDpoUsedAmount = items.FirstOrDefault(x => x.Id == i.Id)?.Details.Sum(d => d.DpoUsedAmount) ?? 0;
        });

        return new PagedResultDto<PriceOfferWithNavigationListDto>
        {
            TotalCount = totalCount,
            Items = itemDtos
        };
    }

    public virtual async Task<PagedResultDto<PriceOfferDetailDto>> GetListDetailsAsync(Guid priceOfferId, GetPriceOfferDetailsInput input)
    {
        var filterParams = ObjectMapper.Map<GetPriceOfferDetailsInput, PriceOfferDetailFilterParams>(input);
        filterParams.PriceOfferId = priceOfferId;
        filterParams.ExcludedStatuses.AddRange([QuoteFlowStatuses.PriceOfferDetail.Merged]);

        var totalCount = await _priceOfferDetailRepository.GetCountAsync(filterParams);
        var items = await _priceOfferDetailRepository.GetListAsync(filterParams, input.Sorting, input.MaxResultCount, input.SkipCount);

        items = [.. items.GroupBy(i => i.ImportGuid)
            .OrderByDescending(g => g.Min(i => i.CreationTime))   // batches ordered by earliest CreationTime DESC
            .SelectMany(g => g.OrderBy(i => i.RowNo))];

        var itemDtos = ObjectMapper.Map<List<PriceOfferDetail>, List<PriceOfferDetailDto>>(items);

        // Apply permission-based filtering
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);
        var flags = await _flaggingService.CreateFlagsAsync(priceOffer);

        // Apply filtering based on flags and permissions
        List<string> golfas = priceOffer.Details
           .Select(x => x.GolfaCode)
           .Distinct()
           .ToList();

        // Get all materials whose GolfaCode is in the list of golfas
        var materials = await _materialRepository.GetListAsync(
            x => golfas.Contains(x.GolfaCode)
        );



        //var dto = ObjectMapper.Map<PriceOffer, PriceOfferDto>(priceOffer);

        //foreach (var detail in dto.Details)
        //{
        //    var material = materials.FirstOrDefault(m => m.GolfaCode == detail.GolfaCode);
        //    if (material != null)
        //    {
        //        detail.SpecialSpec1 = material.Spec1;
        //        detail.SpecialSpec2 = material.Spec2;
        //    }
        //}
        foreach (var item in itemDtos)
        {
            //get Spec1, Spec2 into Materils
            var material = materials.FirstOrDefault(m => m.GolfaCode == item.GolfaCode);
            if (material != null)
            {
                item.SpecialSpec1 = material.Spec1;
                item.SpecialSpec2 = material.Spec2;
            }
            //end

            item.PriceToCustomerAmount = (item.PriceToCustomer ?? 0) * item.Qty;

            if (!flags.IsLandedCostViewable)
            {
                item.LandingCost = 0;
                item.LandingCostAmount = 0;
            }
            else
            {
                item.LandingCostAmount = (item.LandingCost ?? 0) * item.Qty;
            }

            if (!flags.IsGPViewable)
            {
                item.PriceOfferDetailMargin = 0;
            }
        }

        return new PagedResultDto<PriceOfferDetailDto>
        {
            TotalCount = totalCount,
            Items = itemDtos
        };
    }

    public virtual async Task<PagedResultDto<PriceOfferCustomerDto>> GetListCustomersAsync(Guid priceOfferId, GetPriceOfferCustomersInput input)
    {
        var filterParams = ObjectMapper.Map<GetPriceOfferCustomersInput, PriceOfferCustomerFilterParams>(input);
        filterParams.PriceOfferId = priceOfferId;
        var totalCount = await _priceOfferCustomerRepository.GetCountAsync(filterParams);
        var items = await _priceOfferCustomerRepository.GetListAsync(filterParams, input.Sorting, input.MaxResultCount, input.SkipCount);
        var itemDtos = ObjectMapper.Map<List<PriceOfferCustomer>, List<PriceOfferCustomerDto>>(items);
        await _customerEnrichmentService.SetHasKeyAccountAsync(itemDtos);
        return new PagedResultDto<PriceOfferCustomerDto>
        {
            TotalCount = totalCount,
            Items = itemDtos
        };
    }

    public virtual async Task<PagedResultDto<PriceOfferWithNavigationListDto>> GetMyApprovalsListAsync(GetPriceOffersInput input)
    {
        var filterParams = ObjectMapper.Map<GetPriceOffersInput, PriceOfferFilterParams>(input);
        var currentUsername = _currentUser.Username
            ?? throw new UserFriendlyException("Current user is not authenticated.");

        filterParams.RelatedToMe = false;

        var totalCount = await _priceOfferRepository.GetCountMyApprovalsAsync(filterParams, currentUsername);
        var items = await _priceOfferRepository.GetListMyApprovalsAsync(filterParams, currentUsername, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<PriceOfferWithNavigationListDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<PriceOffer>, List<PriceOfferWithNavigationListDto>>(items)
        };
    }

    public virtual async Task<PagedResultDto<MessageDto>> GetListMessagesAsync(Guid priceOfferId, GetPriceOfferMessagesInput input)
    {
        var totalCount = await _priceOfferRepository.GetCountMessagesAsync(priceOfferId);
        var items = await _priceOfferRepository.GetListMessagesAsync(priceOfferId, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<MessageDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<PriceOfferMessage>, List<MessageDto>>(items)
        };
    }

    public virtual async Task<PriceOfferDto> GetAsync(Guid id)
    {
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(id);

        List<string> golfas = priceOffer.Details
            .Select(x => x.GolfaCode)
            .Distinct()
            .ToList();

        // Get all materials whose GolfaCode is in the list of golfas
        var materials = await _materialRepository.GetListAsync(
            x => golfas.Contains(x.GolfaCode)
        );



        var dto = ObjectMapper.Map<PriceOffer, PriceOfferDto>(priceOffer);

        foreach (var detail in dto.Details)
        {
            var material = materials.FirstOrDefault(m => m.GolfaCode == detail.GolfaCode);
            if (material != null)
            {
                detail.SpecialSpec1 = material.Spec1;
                detail.SpecialSpec2 = material.Spec2;
            }
        }

        // Calculate totals
        dto.TotalPriceToCustomer = priceOffer.Details?.Sum(d => (d.PriceToCustomer ?? 0) * d.Qty) ?? 0;

        // Total requested discount: (1 – Total Amount (with requested price)/ Total Amount (with standard price)) *100%
        dto.TotalRequestedDiscountAmount = priceOffer.TotalStandardAmount == 0 ? 0 :
            (1 - (priceOffer.TotalRequestedAmount / priceOffer.TotalStandardAmount));

        dto.Flags = await _flaggingService.CreateFlagsAsync(priceOffer);

        if (dto.Flags.IsLandedCostViewable)
        {
            dto.TotalLandedCost = priceOffer.TotalLandedCost;
        }
        else
        {
            dto.TotalLandedCost = 0;
        }

        if (dto.Flags.IsGPViewable)
        {
            dto.TotalGP = priceOffer.TotalMEVNOfferAmount == 0 || !priceOffer.TotalMEVNOfferAmount.HasValue ? 0 :
                (1 - (dto.TotalLandedCost / priceOffer.TotalMEVNOfferAmount.Value));
        }
        else
        {
            dto.TotalGP = 0;
        }

        dto.TotalDpoUsedAmount = priceOffer.Details?.Sum(d => d.DpoUsedAmount) ?? 0;
        dto.TotalUsableOfferAmount = priceOffer.Details?
            .Where(d => d.IsApproved() || d.IsClosed())
            .Sum(d => d.MEVNOfferAmount) ?? 0;
        dto.TotalDpoUsedPercentage = dto.TotalUsableOfferAmount == 0 ? 0 :
            (dto.TotalDpoUsedAmount / dto.TotalUsableOfferAmount);
        await _customerEnrichmentService.SetHasKeyAccountAsync(dto.Customers ?? []);

        return dto;
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        await _priceOfferRepository.DeleteAsync(id);
    }

    public virtual async Task<PriceOfferDto> CreateAsync(PriceOfferCreateDto input)
    {
        var createParams = ObjectMapper.Map<PriceOfferCreateDto, PriceOfferCreateParams>(input);
        var priceOffer = await _priceOfferManager.CreateAsync(createParams);

        var dto = ObjectMapper.Map<PriceOffer, PriceOfferDto>(priceOffer);

        // Calculate totals
        dto.TotalLandedCost = priceOffer.TotalLandedCost;

        dto.TotalRequestedDiscountAmount = priceOffer.TotalStandardAmount == 0 ? 0 :
            (1 - (priceOffer.TotalRequestedAmount / priceOffer.TotalStandardAmount));

        dto.TotalGP = priceOffer.TotalMEVNOfferAmount == 0 || !priceOffer.TotalMEVNOfferAmount.HasValue ? 0 :
            (1 - (dto.TotalLandedCost / priceOffer.TotalMEVNOfferAmount.Value));

        return dto;
    }


    public virtual async Task<PriceOfferDto> UpdateAsync(Guid id, PriceOfferUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<PriceOfferUpdateDto, PriceOfferUpdateParams>(input);
        var priceOffer = await _priceOfferManager.UpdateAsync(id, updateParams);

        var dto = ObjectMapper.Map<PriceOffer, PriceOfferDto>(priceOffer);

        // Calculate totals
        dto.TotalLandedCost = priceOffer.TotalLandedCost;

        dto.TotalRequestedDiscountAmount = priceOffer.TotalStandardAmount == 0 ? 0 :
            (1 - (priceOffer.TotalRequestedAmount / priceOffer.TotalStandardAmount));

        dto.TotalGP = priceOffer.TotalMEVNOfferAmount == 0 || !priceOffer.TotalMEVNOfferAmount.HasValue ? 0 :
            (1 - (dto.TotalLandedCost / priceOffer.TotalMEVNOfferAmount.Value));

        return dto;
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(PriceOfferExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var filterParams = ObjectMapper.Map<PriceOfferExcelDownloadDto, PriceOfferFilterParams>(input);
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        filterParams.ApplyBuyerRestrictions(buyerAccess);
        filterParams.ApplyLocationRestrictions(buyerAccess);
        filterParams.ApplyMaterialTypeRestrictions(buyerAccess);

        var items = await _priceOfferRepository.GetListAsync(filterParams);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<PriceOffer>, List<PriceOfferExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "PriceOffers.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListDetailsAsExcelFileAsync(Guid priceOfferId, string downloadToken)
    {
        var tokenItem = await _downloadTokenCache.GetAsync(downloadToken);
        if (tokenItem == null || downloadToken != tokenItem.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + downloadToken);
        }

        // Get price offer details (without pagination to get all details)
        var filterParams = new PriceOfferDetailFilterParams
        {
            PriceOfferId = priceOfferId
        };
        filterParams.ExcludedStatuses.AddRange([QuoteFlowStatuses.PriceOfferDetail.Merged]);

        var items = await _priceOfferDetailRepository.GetListAsync(filterParams);
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);
        var flags = await _flaggingService.CreateFlagsAsync(priceOffer);

        // Sort by import batch and row number (same logic as GetListDetailsAsync)
        items = [.. items.GroupBy(i => i.ImportGuid)
            .OrderByDescending(g => g.Min(i => i.CreationTime))
            .SelectMany(g => g.OrderBy(i => i.RowNo))];
        var itemDtos = ObjectMapper.Map<List<PriceOfferDetail>, List<PriceOfferDetailExcelDto>>(items);

        // Apply filtering based on flags and permissions
        foreach (var item in itemDtos)
        {
            item.PriceToCustomerAmount = (item.PriceToCustomer ?? 0) * item.Qty;

            if (!flags.IsLandedCostViewable)
            {
                item.LandingCost = 0;
            }

            if (!flags.IsGPViewable)
            {
                item.PriceOfferDetailMargin = 0;
            }
        }

        var memoryStream = new MemoryStream();
        await CreatePriceOfferDetailsExcelWithSummaryAsync(itemDtos, memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, $"PriceOffer_{priceOfferId}_Details.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new PriceOfferDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new QuoteFlow.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }

    public async Task<PagedResultDto<PriceOfferReportDetailDto>> GetListReportDetailAsync(GetPriceOfferReportDetailsInput input)
    {
        var filterParams = ObjectMapper.Map<GetPriceOfferReportDetailsInput, PriceOfferReportDetailFilterParams>(input);

        filterParams.HasFullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        filterParams.HasStrategicPriceAccess = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.Materials.ViewStrategicPrice);
        filterParams.UserName = _currentUser.Username ?? string.Empty;
        var items = await _priceOfferRepository.GetListReportDetailAsync(filterParams);

        return new PagedResultDto<PriceOfferReportDetailDto>
        {
            TotalCount = items.Count,
            Items = ObjectMapper.Map<List<PriceOfferReportDetail>, List<PriceOfferReportDetailDto>>(items)
        };
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListDetailAsExcelFileAsync(GetPriceOfferReportDetailsInput input)
    {
        var filterParams = ObjectMapper.Map<GetPriceOfferReportDetailsInput, PriceOfferReportDetailFilterParams>(input);

        filterParams.HasFullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        filterParams.HasStrategicPriceAccess = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.Materials.ViewStrategicPrice);
        filterParams.UserName = _currentUser.Username ?? string.Empty;
        var items = await _priceOfferRepository.GetListReportDetailAsync(filterParams);

        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == "Template_SPO_Detail_Report.xlsx");
        if (fileDescriptor == null)
        {
            throw new Exception("Template Excel not found.");
        }

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();
        int startRow = 3;
        if (items.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(items.Count - 1);
        }
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var row = ws.Row(startRow + i);

            row.Cell(1).Value = i + 1;
            row.Cell(2).Value = item.LocationDescription;
            row.Cell(3).Value = item.BuyerTypeDescription;
            row.Cell(4).Value = item.BuyerCode;
            row.Cell(5).Value = item.MaterialType;
            row.Cell(6).Value = item.SPOType;
            row.Cell(7).Value = item.PriceOffer_Code;
            row.Cell(8).Value = item.ProjectName;
            row.Cell(9).Value = item.ApprovalStatus;
            row.Cell(10).Value = item.SPOValidity_From;
            row.Cell(11).Value = item.SPOValidity_To;
            row.Cell(12).Value = item.Country;
            row.Cell(13).Value = item.Province;
            row.Cell(14).Value = item.ProjectResultStatus;
            row.Cell(15).Value = item.Material_Group;
            row.Cell(16).Value = item.GolfaCode;
            row.Cell(17).Value = item.ModelName;
            row.Cell(18).Value = item.Qty;
            row.Cell(19).Value = item.MEVNOfferPrice;
            row.Cell(20).Value = item.SaleOfferAmount;
            row.Cell(21).Value = item.StandardPrice;
            row.Cell(22).Value = item.StandardAmount;
            row.Cell(23).Value = item.ActualDiscountRatio;
            row.Cell(24).Value = item.PriceOfferDetailMargin;
            row.Cell(25).Value = item.DPO_UsedQty;
            row.Cell(26).Value = item.DPO_UsedAmount;
            row.Cell(27).Value = item.DPO_DeliveredQty;
            row.Cell(28).Value = item.DPO_DeliveredAmount;
            row.Cell(29).Value = item.DOP_BO_Qty;
            row.Cell(30).Value = item.DOP_BO_Amount;
            row.Cell(31).Value = item.SPO_Open_Qty;
            row.Cell(32).Value = item.SPO_Open_Amount;


        }

        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;

        return new RemoteStreamContent(
            output,
            "SPO_Detail_Export.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }


    public async Task<PagedResultDto<PriceOfferReportGeneralDto>> GetListReportGeneralAsync(GetPriceOfferReportGeneralsInput input)
    {
        var filterParams = ObjectMapper.Map<GetPriceOfferReportGeneralsInput, PriceOfferReportGeneralFilterParams>(input);

        filterParams.HasFullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        filterParams.HasStrategicPriceAccess = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.Materials.ViewStrategicPrice);
        filterParams.UserName = _currentUser.Username ?? string.Empty;

        var items = await _priceOfferRepository.GetListReportGeneralAsync(filterParams);

        return new PagedResultDto<PriceOfferReportGeneralDto>
        {
            TotalCount = items.Count,
            Items = ObjectMapper.Map<List<PriceOfferReportGeneral>, List<PriceOfferReportGeneralDto>>(items)
        };
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetListGeneralAsExcelFileAsync(GetPriceOfferReportGeneralsInput input)
    {
        var filterParams = ObjectMapper.Map<GetPriceOfferReportGeneralsInput, PriceOfferReportGeneralFilterParams>(input);

        filterParams.HasFullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        filterParams.HasStrategicPriceAccess = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.Materials.ViewStrategicPrice);
        filterParams.UserName = _currentUser.Username ?? string.Empty;
        var items = await _priceOfferRepository.GetListReportGeneralAsync(filterParams);

        var fileDescriptor = await _fileDescriptorRepository.FirstOrDefaultAsync(fd => fd.Name == "Template_SPO_General_Report.xlsx");
        if (fileDescriptor == null)
        {
            throw new Exception("Template Excel not found.");
        }

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();
        int startRow = 3;
        if (items.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(items.Count - 1);
        }
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var row = ws.Row(startRow + i);

            row.Cell(1).Value = i + 1;
            row.Cell(2).Value = item.BuyerTypeDescription;
            row.Cell(3).Value = item.BuyerCode;
            row.Cell(4).Value = item.MaterialType;
            row.Cell(5).Value = item.SPOType;
            row.Cell(6).Value = item.PriceOffer_Code;
            row.Cell(7).Value = item.ProjectName;
            row.Cell(8).Value = item.CreationTime;
            row.Cell(9).Value = item.ApprovalStatus;
            row.Cell(10).Value = item.ProjectResultStatus;
            row.Cell(11).Value = item.CompetitorBrand;
            row.Cell(12).Value = item.TotalStandardAmount;
            row.Cell(13).Value = item.TotalMEVNOfferAmount;
            row.Cell(14).Value = item.SPO_DiscountRatio;
            row.Cell(15).Value = item.TotalLandedCost;
            row.Cell(16).Value = item.GP == 0m ? null : item.GP;
            row.Cell(17).Value = item.TotalOrderd;
            row.Cell(18).Value = item.TotalDeliveredValue;
            row.Cell(19).Value = item.OrderRatio == 0m ? null : item.OrderRatio;
            row.Cell(20).Value = item.WarningDate == DateTime.MinValue || item.WarningDate == null ? "" : item.WarningDate;
            row.Cell(21).Value = item.CloseDate;

            row.Cell(22).Value = item.Trading_Customer;
            row.Cell(23).Value = item.Trading_TaxCode;
            row.Cell(24).Value = item.PanelBuilder_Customer;
            row.Cell(25).Value = item.PanelBuilder_TaxCode;
            row.Cell(26).Value = item.MEContractor_Customer;
            row.Cell(27).Value = item.MEContractor_TaxCode;
            row.Cell(28).Value = item.MainContractor_Customer;
            row.Cell(29).Value = item.MainContractor_TaxCode;

            row.Cell(30).Value = item.SIOEM_Customer;
            row.Cell(31).Value = item.SIOEM_TaxCode;

            row.Cell(33).Value = item.InvestorEU_Customer;

            row.Cell(34).Value = item.InvestorEU_TaxCode;
            row.Cell(38).Value = item.Country;
            row.Cell(39).Value = item.Province;


        }



        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;

        return new RemoteStreamContent(
            output,
            "SPO_General_Export.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    public virtual async Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParseDSAsync(IRemoteStreamContent file, PriceOfferDSImportInput input)
    {
        var username = _currentUser.Username ?? string.Empty;
        var validator = _excelImportFactory.CreateValidator<PriceOfferImportDto>(ExcelImporters.PriceOfferDS);

        await using var stream = file.GetStream();
        var context = new ExcelImportContext();
        //context.SetData(ExcelImportContextKeys.PriceOffer.MaterialType, input.MaterialType ?? string.Empty);
        //context.SetData(ExcelImportContextKeys.PriceOffer.BuyerId, input.BuyerId);
        context.SetData(ExcelImportContextKeys.PriceOffer.MaterialType, input.MaterialType ?? string.Empty);
        context.SetData(ExcelImportContextKeys.PriceOffer.BuyerTypeId, input.BuyerTypeId);
        context.SetData(ExcelImportContextKeys.PriceOffer.LocationId, input.LocationId);
        context.SetData(ExcelImportContextKeys.PriceOffer.BuyerId, input.BuyerId);
        context.SetData(ExcelImportContextKeys.PriceOffer.CurrentUserName, username);

        // this just validates the file, we also need to validate other fields in input
        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);

        var priceOffer = result.ListData.FirstOrDefault()?.RowData;

        if (priceOffer is null)
        {
            return result;
        }
        var priceOfferCode = _priceOfferManager.GetDraftCode(PriceOfferConsts.BuyerPrefix, input.MaterialType ?? string.Empty);

        var today = DateTime.Now;
        var closingYear = today.Month < 4 ? today.Year : today.Year + 1;
        priceOffer.CloseDate = new DateTime(closingYear, 3, 31);

        priceOffer.PriceOfferCode = priceOfferCode;
        priceOffer.BuyerId = input.BuyerId;
        priceOffer.BuyerTypeId = input.BuyerTypeId;
        priceOffer.LocationId = input.LocationId;
        priceOffer.ProjectName = input.ProjectName;
        priceOffer.MaterialType = input.MaterialType;
        priceOffer.Note = input.Note;

        result.ListData[0].RowData = priceOffer;

        return result;
    }

    public virtual async Task<PriceOfferDto> ImportDSAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false)
    {
        var createParamsConverter = _excelImportFactory.CreateCreateParamsConverter<PriceOfferImportDto, PriceOfferCreateParams>(ExcelImporters.PriceOfferDS);

        var rowResult = validationResult.ListData.FirstOrDefault()
            ?? throw new UserFriendlyException("No valid data found in the import file.");

        var createParams = await createParamsConverter.ConvertToCreateParamsAsync(rowResult, new(), default)
            ?? throw new UserFriendlyException("Failed to convert import data to create parameters.");

        var createResult = await _priceOfferManager.CreateAsync(createParams);
        await UnitOfWorkManager.Current!.SaveChangesAsync();

        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(createResult.Id);

        await _localEventBus.PublishAsync(new PriceOfferCreatedEvent(priceOffer.Id, forceSubmit), onUnitOfWorkComplete: false);

        return ObjectMapper.Map<PriceOffer, PriceOfferDto>(priceOffer);
    }

    public virtual async Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParseNBAsync(IRemoteStreamContent file, PriceOfferNBImportInput input)
    {
        var validator = _excelImportFactory.CreateValidator<PriceOfferImportDto>(ExcelImporters.PriceOfferNB);

        await using var stream = file.GetStream();
        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.PriceOffer.MaterialType, input.MaterialType ?? string.Empty);

        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);

        var priceOffer = result.ListData.FirstOrDefault()?.RowData;

        if (priceOffer is null)
        {
            return result;
        }

        var priceOfferCode = _priceOfferManager.GetDraftCode(PriceOfferConsts.ProjectPrefix, input.MaterialType ?? string.Empty);
        priceOffer.PriceOfferCode = priceOfferCode;
        priceOffer.BuyerId = null;
        priceOffer.BuyerTypeId = null;
        priceOffer.LocationId = input.LocationId;
        priceOffer.ProjectName = input.ProjectName;
        priceOffer.MaterialType = input.MaterialType;
        priceOffer.Note = input.Note;
        priceOffer.CloseDate = input.CloseDate;

        if (priceOffer.Customers is not null)
            await _customerEnrichmentService.SetHasKeyAccountAsync(priceOffer.Customers.ListData);

        result.ListData[0].RowData = priceOffer;

        return result;
    }

    public virtual async Task<PriceOfferDto> ImportNBAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false)
    {
        var createParamsConverter = _excelImportFactory.CreateCreateParamsConverter<PriceOfferImportDto, PriceOfferCreateParams>(ExcelImporters.PriceOfferNB);

        var rowResult = validationResult.ListData.FirstOrDefault()
            ?? throw new UserFriendlyException("No valid data found in the import file.");

        var createParams = await createParamsConverter.ConvertToCreateParamsAsync(rowResult, new(), default)
            ?? throw new UserFriendlyException("Failed to convert import data to create parameters.");

        var createResult = await _priceOfferManager.CreateAsync(createParams);

        await _localEventBus.PublishAsync(new PriceOfferCreatedEvent(createResult.Id, forceSubmit), onUnitOfWorkComplete: false);

        return ObjectMapper.Map<PriceOffer, PriceOfferDto>(createResult);
    }

    public virtual async Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParseAPAsync(IRemoteStreamContent file, PriceOfferAPImportInput input)
    {
        var username = _currentUser.Username ?? string.Empty;
        var validator = _excelImportFactory.CreateValidator<PriceOfferImportDto>(ExcelImporters.PriceOfferAP);

        await using var stream = file.GetStream();
        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.PriceOffer.MaterialType, input.MaterialType ?? string.Empty);
        context.SetData(ExcelImportContextKeys.PriceOffer.KeyAccountClassId, input.KeyAccountClassId);
        context.SetData(ExcelImportContextKeys.PriceOffer.KeyAccountId, input.KeyAccountId);
        context.SetData(ExcelImportContextKeys.PriceOffer.GetPriceAuto, input.GetPriceAutomatically);
        context.SetData(ExcelImportContextKeys.PriceOffer.BuyerTypeId, input.BuyerTypeId);
        context.SetData(ExcelImportContextKeys.PriceOffer.LocationId, input.LocationId);
        context.SetData(ExcelImportContextKeys.PriceOffer.BuyerId, input.BuyerId);
        context.SetData(ExcelImportContextKeys.PriceOffer.CurrentUserName, username);

        // this just validates the file, we also need to validate other fields in input
        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);

        var priceOffer = result.ListData.FirstOrDefault()?.RowData;

        if (priceOffer is null)
        {
            return result;
        }
        var priceOfferCode = _priceOfferManager.GetDraftCode(PriceOfferConsts.KeyAccountPrefix, input.MaterialType ?? string.Empty);

        var today = DateTime.Now;
        var closingYear = today.Month < 4 ? today.Year : today.Year + 1;
        priceOffer.CloseDate = new DateTime(closingYear, 3, 31);
        priceOffer.PriceOfferCode = priceOfferCode;
        priceOffer.BuyerId = input.BuyerId;
        priceOffer.BuyerTypeId = input.BuyerTypeId;
        priceOffer.LocationId = input.LocationId;
        priceOffer.ProjectName = input.ProjectName;
        priceOffer.KeyAccountId = input.KeyAccountId;
        priceOffer.KeyAccountTypeId = input.KeyAccountTypeId;
        priceOffer.KeyAccountClassId = input.KeyAccountClassId;
        priceOffer.MaterialType = input.MaterialType;
        priceOffer.Note = input.Note;

        result.ListData[0].RowData = priceOffer;

        return result;
    }

    public virtual async Task<PriceOfferDto> ImportAPAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false)
    {
        var createParamsConverter = _excelImportFactory.CreateCreateParamsConverter<PriceOfferImportDto, PriceOfferCreateParams>(ExcelImporters.PriceOfferAP);

        var rowResult = validationResult.ListData.FirstOrDefault()
            ?? throw new UserFriendlyException("No valid data found in the import file.");

        var createParams = await createParamsConverter.ConvertToCreateParamsAsync(rowResult, new(), default)
            ?? throw new UserFriendlyException("Failed to convert import data to create parameters.");

        var createResult = await _priceOfferManager.CreateAsync(createParams);
        await UnitOfWorkManager.Current!.SaveChangesAsync();

        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(createResult.Id);

        await _localEventBus.PublishAsync(new PriceOfferCreatedEvent(priceOffer.Id, forceSubmit), onUnitOfWorkComplete: false);

        return ObjectMapper.Map<PriceOffer, PriceOfferDto>(priceOffer);
    }

    public virtual async Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParsePPAsync(IRemoteStreamContent file, PriceOfferImportInput input)
    {
        var username = _currentUser.Username ?? string.Empty;
        var validator = _excelImportFactory.CreateValidator<PriceOfferImportDto>(ExcelImporters.PriceOfferPP);

        await using var stream = file.GetStream();
        var context = new ExcelImportContext();
        //context.SetData(ExcelImportContextKeys.PriceOffer.MaterialType, input.MaterialType ?? string.Empty);
        //context.SetData(ExcelImportContextKeys.PriceOffer.BuyerId, input.BuyerId);
        context.SetData(ExcelImportContextKeys.PriceOffer.MaterialType, input.MaterialType ?? string.Empty);
        context.SetData(ExcelImportContextKeys.PriceOffer.BuyerTypeId, input.BuyerTypeId);
        context.SetData(ExcelImportContextKeys.PriceOffer.LocationId, input.LocationId);
        context.SetData(ExcelImportContextKeys.PriceOffer.BuyerId, input.BuyerId);
        context.SetData(ExcelImportContextKeys.PriceOffer.CurrentUserName, username);

        // this just validates the file, we also need to validate other fields in input
        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);

        var priceOffer = result.ListData.FirstOrDefault()?.RowData;

        if (priceOffer is null)
        {
            return result;
        }

        var nextYear = DateTime.Now.Year;
        priceOffer.CloseDate = new DateTime(nextYear, 3, 31);
        priceOffer.BuyerId = input.BuyerId;
        priceOffer.BuyerTypeId = input.BuyerTypeId;
        priceOffer.LocationId = input.LocationId;
        priceOffer.ProjectName = input.ProjectName;
        priceOffer.CloseDate = input.CloseDate;
        priceOffer.Note = input.Note;

        if (priceOffer.Customers is not null)
            await _customerEnrichmentService.SetHasKeyAccountAsync(priceOffer.Customers.ListData);

        result.ListData[0].RowData = priceOffer;

        return result;
    }

    public virtual async Task<PriceOfferDto> ImportPPAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false)
    {
        var createParamsConverter = _excelImportFactory.CreateCreateParamsConverter<PriceOfferImportDto, PriceOfferCreateParams>(ExcelImporters.PriceOfferPP);

        var rowResult = validationResult.ListData.FirstOrDefault()
            ?? throw new UserFriendlyException("No valid data found in the import file.");

        var createParams = await createParamsConverter.ConvertToCreateParamsAsync(rowResult, new(), default)
            ?? throw new UserFriendlyException("Failed to convert import data to create parameters.");

        var createResult = await _priceOfferManager.CreateAsync(createParams);

        await _localEventBus.PublishAsync(new PriceOfferCreatedEvent(createResult.Id, forceSubmit), onUnitOfWorkComplete: false);

        return ObjectMapper.Map<PriceOffer, PriceOfferDto>(createResult);
    }

    public virtual async Task<ExcelValidationResult<PriceOfferDetailImportDto>> ValidateAndParseAddMoreItemDetailAsync(Guid priceOfferId, IRemoteStreamContent file, bool? getPriceAutomatically)
    {
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);

        //if (priceOffer.IsProjectPriceOffer())
        //{
        //    if (!priceOffer.IsWon())
        //    {
        //        throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.ProjectPriceOfferNotWon)
        //            .WithData("priceOfferId", priceOfferId);
        //    }
        //}
        //else
        //{
        if (!priceOffer.IsApproved())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.PriceOfferNotApproved)
                .WithData("priceOfferId", priceOfferId);
        }
        //}

        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.PriceOffer.MaterialType, priceOffer.MaterialType ?? string.Empty);
        context.SetData(ExcelImportContextKeys.ParentEntityId, priceOfferId);
        context.SetData(ExcelImportContextKeys.PriceOffer.BuyerId, priceOffer.BuyerId);
        context.SetData(ExcelImportContextKeys.PriceOffer.GetPriceAuto, getPriceAutomatically ?? false);

        // Pre-validation for DPO usage constraint
        await using var originalStream = file.GetStream();
        using var memory = new MemoryStream();
        await originalStream.CopyToAsync(memory);

        // Reset for first use
        memory.Position = 0;

        var tempValidator = _excelImportFactory.CreateValidator<PriceOfferDetailImportDto>(
            ExcelImporters.PriceOfferAddMoreItemDetail
        );
        var tempResult = await tempValidator.ValidateAsync(memory, file.FileName ?? "", context);

        if (priceOffer.HasDPOUsed && !tempResult.HasErrors && tempResult.ListData.Any())
        {
            var totalImpact = tempResult.ListData.Sum(x => x.RowData.MEVNOfferAmount ?? 0);
            await ValidateAddMoreItemsLimitAsync(priceOffer, totalImpact);
        }

        // Reset again for the final validation
        memory.Position = 0;

        var validator = _excelImportFactory.CreateValidator<PriceOfferDetailImportDto>(
            ExcelImporters.PriceOfferAddMoreItemDetail
        );
        return await validator.ValidateAsync(memory, file.FileName ?? "", context);
    }

    public virtual async Task<List<PriceOfferDetailDto>> ImportAddMoreItemDetailAsync(
        Guid priceOfferId,
        ImportAddMoreItemsInput input)
    {
        var comment = input.Comment;
        var validationResult = input.ValidationResult;
        // Double-check validation for DPO usage constraint
        var priceOfferForValidation = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);
        if (priceOfferForValidation.HasDPOUsed && validationResult.ListData.Any())
        {
            var totalImpact = validationResult.ListData.Sum(x => x.RowData.MEVNOfferAmount ?? 0);
            await ValidateAddMoreItemsLimitAsync(priceOfferForValidation, totalImpact);
        }

        var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>(ExcelImporters.PriceOfferAddMoreItemDetail);

        var context = new ExcelImportContext();
        var importGuid = GuidGenerator.Create();
        context.SetData(ExcelImportContextKeys.ParentEntityId, priceOfferId);
        context.SetData(ExcelImportContextKeys.PriceOfferDetail.ImportGuid, importGuid);

        List<PriceOfferDetailCreateParams> createParams = (await Task.WhenAll(
            validationResult.ListData
                .Select(x => createParamsConverters.ConvertToCreateParamsAsync(x, context, default))
            )).Where(x => x != null)
            .ToList()!;

        await _priceOfferDetailManager.CreateBatchAsync(createParams);
        await UnitOfWorkManager.Current!.SaveChangesAsync();

        await _localEventBus.PublishAsync(new PriceOfferItemsImportedEvent(priceOfferId, importGuid, comment), onUnitOfWorkComplete: false);

        var addMoreItemPriceOffer = createParams
            .Select(param => new AddMoreItemHistory(
                GuidGenerator.Create(),
                importGuid,
                param
            ))
            .ToList();

        await _addMoreItemHistoryRepository.InsertManyAsync(addMoreItemPriceOffer);


        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);
        var createdPriceOfferDetails = priceOffer.Details
            .Where(x => x.ImportGuid == importGuid)
            .ToList();
        return ObjectMapper.Map<List<PriceOfferDetail>, List<PriceOfferDetailDto>>(createdPriceOfferDetails);
    }

    public virtual async Task<PriceOfferDto> PerformActionAsync(Guid id, ActionDto input)
    {
        using (var uow = UnitOfWorkManager.Begin(new(true), requiresNew: true))
        {
            var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(id);
            await ValidateAction(priceOffer, input);

            var action = input.Action;
            var actionDate = DateTime.Now;

            var actionsWithApprovalRoute = new List<string>
            {
                HistoryActions.Approved,
                HistoryActions.Rejected,
                HistoryActions.Cancelled,
            };
            if (actionsWithApprovalRoute.Contains(action))
            {
                priceOffer = await HandleSpoApprovalActionsAsync(input, priceOffer, actionDate);
            }
            else if (action == HistoryActions.Closed)
            {
                priceOffer = await HandleClosureAsync(priceOffer, input, actionDate);
            }

            var priceOfferNew = await _priceOfferRepository.UpdateAsync(priceOffer, autoSave: true);
            await _localEventBus.PublishAsync(
                new PriceOfferActionedEvent(
                    priceOffer.Id,
                    input.Action,
                    _currentUser.Username!,
                    actionDate,
                    input.Comment),
                onUnitOfWorkComplete: false
            );

            await uow.CompleteAsync();

            return ObjectMapper.Map<PriceOffer, PriceOfferDto>(priceOfferNew);
        }
    }

    private async Task<PriceOffer> HandleSpoApprovalActionsAsync(ActionDto input, PriceOffer priceOffer, DateTime actionDate)
    {
        var note = input.Comment?.Trim() ?? string.Empty;
        var action = input.Action;
        var spoId = priceOffer.Id;
        var saleTeam = await _salesAssignmentRepository.GetListAsync();
        var latestSteps = _approvalRouteManager.GetLatestUnapprovedSteps(priceOffer.ApprovalRoutes, spoId);

        var isRequester = priceOffer.CreatorUsername?.ToLowerInvariant() == _currentUser.Username?.ToLowerInvariant();
        var isSaleTeam = saleTeam.FirstOrDefault(x => x.SaleUserName.ToLowerInvariant() == _currentUser.Username?.ToLowerInvariant()) != null;
        var latestPersonUploadSPO = priceOffer.Details
            .OrderByDescending(d => d.CreationTime)
            .First()
            .CreatorUsername;

        var approvalActions = new List<string> { HistoryActions.Approved, HistoryActions.Rejected };
        if (approvalActions.Contains(action) && !latestSteps.Any(x => x.Approver!.Equals(_currentUser.Username, StringComparison.OrdinalIgnoreCase)))
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.InvalidApprovalAction)
                .WithData("entityName", NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)))
                .WithData("entityId", spoId);
        }

        _logger.LogInformation("Action {action} taken on PriceOffer ID: {id} by User: {currentUsername}", action, spoId, _currentUser.Username);

        var latestStep = latestSteps.First(); // use first since the list is ensured to have at least one step
        string approverRoleCode;
        string approverRoleName;

        if (action == HistoryActions.Cancelled)
        {
            approverRoleCode = "User";
            approverRoleName = "User";
        }
        else
        {
            approverRoleCode = latestStep.ApproverRoleCode;
            approverRoleName = latestStep.ApproverRoleName;
        }
        var isLastRoute = _approvalRouteManager.IsLastApprovalStep(latestStep, priceOffer.ApprovalRoutes);

        if (action == HistoryActions.Approved || action == HistoryActions.Rejected)
        {
            var baseHistoryCreateParams = new ApprovalHistoryCreateParams(
                latestStep.ApproverRoleCode,
                latestStep.ApproverRoleName,
                _currentUser.Username,
                _currentUser.FullName,
                action,
                actionDate,
                note,
                isLastRoute && priceOffer.GetPriceOfferType() != PriceOfferTypes.PriceOfferPP
            );
            var history = new PriceOfferApprovalHistory(
               GuidGenerator.Create(),
               spoId,
               baseHistoryCreateParams
           );
            priceOffer.RecordAction(history);
            foreach (var detail in priceOffer.Details)
            {
                if (!detail.IsInProgress())
                {
                    continue;
                }

                var detailHistory = new PriceOfferDetailApprovalHistory(
                    GuidGenerator.Create(),
                    detail.Id,
                    baseHistoryCreateParams
                );

                detail.RecordAction(detailHistory);
            }
        }

        else if (action == HistoryActions.Cancelled)
        {
            var baseHistoryCreateParams = new ApprovalHistoryCreateParams(
               isRequester ? "Requester" : isSaleTeam ? "Sale Team" : "FA Team",
               isRequester ? "Requester" : isSaleTeam ? "Sale Team" : "FA Team",
               _currentUser.Username,
               _currentUser.FullName,
               action,
               actionDate,
               note,
               isLastRoute && priceOffer.GetPriceOfferType() != PriceOfferTypes.PriceOfferPP
            );
            var history = new PriceOfferApprovalHistory(
               GuidGenerator.Create(),
               spoId,
               baseHistoryCreateParams
            );
            priceOffer.RecordAction(history);
            foreach (var detail in priceOffer.Details)
            {
                if (!detail.IsInProgress())
                {
                    continue;
                }

                var detailHistory = new PriceOfferDetailApprovalHistory(
                    GuidGenerator.Create(),
                    detail.Id,
                    baseHistoryCreateParams
                );

                detail.RecordAction(detailHistory);
            }
        }



        priceOffer = action switch
        {
            HistoryActions.Approved => await HandleApprovalAsync(priceOffer, latestStep, isLastRoute, actionDate, note),
            HistoryActions.Rejected => await HandleRejectionAsync(priceOffer, latestStep),
            HistoryActions.Cancelled => await HandleCancellationAsync(priceOffer, latestStep),
            _ => throw new UserFriendlyException($"Unknown action: {action}"),
        };
        return priceOffer;
    }

    public virtual async Task<List<ApproverDto>> GetListApproversAsync(Guid priceOfferId)
    {
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);

        if (priceOffer.CurrentApprovalRouteInstanceId is null)
        {
            return [];
        }

        var pendingApprovalRoutes = _approvalRouteManager.GetLatestUnapprovedSteps(priceOffer.ApprovalRoutes, priceOffer.Id);
        var allUsers = await _identityUserRepository.GetListAsync();

        return [.. pendingApprovalRoutes.Select(x =>
        {
            var user = allUsers.FirstOrDefault(u => UserHelper.CompareUsername(u.UserName, x.Approver!));

            return new ApproverDto
            {
                RoleCode = x.ApproverRoleCode,
                RoleName = x.ApproverRoleName,
                Username = x.Approver ?? "N/A",
                FullName = UserHelper.GetFullName(user?.Name, user?.Surname) ?? x.Approver ?? "N/A",
            };
        })];
    }
    string GetDisplayNameFromEmail(string email)
    {
        var username = email.Split('@')[0];
        var parts = username.Split('.', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 2)
        {
            return $"{parts[1]}, {InsertSpaces(parts[0])}";
        }

        return InsertSpaces(username);
    }

    string InsertSpaces(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
    }

    [UnitOfWork(IsDisabled = true)]
    public virtual async Task<MessageDto> SendMessageAsync(Guid priceOfferId, MessageCreateDto input)
    {
        using var uow = UnitOfWorkManager.Begin(new(true), requiresNew: true);
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId, x => x.Messages);
        var currentUserFullName = _currentUser.FullName ?? "N/A";
        var currentUserUsername = _currentUser.Username ?? "N/A";

        var comment = input.Comment?.Trim() ?? string.Empty;
        var inputEmails = input.SendToEmails?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? [];

        // Get sales pic emails for the buyer of this PriceOffer
        var filterParams = new SalesAssignments.ParameterObjects.SalesAssignmentFilterParams { BuyerId = priceOffer.BuyerId };
        var salesAssignments = await _salesAssignmentRepository.GetListAsync(filterParams);
        var salePicEmails = new List<string> { priceOffer.CreatorUsername!, priceOffer.LastApprovalRouteCreatorUsername ?? "" };
        salePicEmails = [.. salePicEmails
            .Where(email => !string.IsNullOrWhiteSpace(email))
            .Distinct(StringComparer.OrdinalIgnoreCase)];

        var isCurrentUserSalePic = salePicEmails.Contains(currentUserUsername, StringComparer.OrdinalIgnoreCase);

        List<string> emailsToSend;

        // Apply the cleaned up email determination logic
        if (inputEmails.Count > 0)
        {
            // Check if input emails are valid
            var (validEmails, invalidEmails) = await NormalizeEmailListAsync(inputEmails);

            if (invalidEmails.Count > 0)
            {
                // Invalid emails found, throw error
                throw new UserFriendlyException($"Some emails do not match any user in the system. Please check and try again. Invalid emails: {string.Join(", ", invalidEmails)}");
            }

            // All emails are valid, send to input list
            emailsToSend = validEmails;
        }
        else
        {
            // No input emails provided
            if (isCurrentUserSalePic)
            {
                // Current user is sale pic but no recipients specified
                throw new BusinessException(QuoteFlowDomainErrorCodes.Discussion.RecipientRequiredForSalePIC);
            }
            else
            {
                // Current user is not sale pic, send to sale pic list
                emailsToSend = salePicEmails;
            }
        }

        // Special case: if input has only current user's email and user is sale pic
        if (inputEmails.Count == 1 &&
            inputEmails[0].Equals(currentUserUsername, StringComparison.OrdinalIgnoreCase) &&
            isCurrentUserSalePic)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.Discussion.RecipientRequiredForSalePIC);
        }

        // Normalize final email list
        var (finalValidEmails, finalInvalidEmails) = await NormalizeEmailListAsync(emailsToSend);
        if (finalInvalidEmails.Count > 0)
        {
            _logger.LogWarning("The following emails are in an invalid format: {emails}", string.Join(", ", finalInvalidEmails));
        }

        if (finalValidEmails.Count == 0)
        {
            _logger.LogWarning("No valid emails provided for PriceOffer ID: {id}", priceOfferId);
            throw new UserFriendlyException("No valid emails provided. Please check and try again.");
        }

        emailsToSend = [.. finalValidEmails.Distinct()];

        var message = new PriceOfferMessage(
            GuidGenerator.Create(),
            priceOffer.Id,
            _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
            _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
            emailsToSend,
            comment
        );

        priceOffer.RecordMessage(message);

        try
        {
            await _priceOfferRepository.UpdateAsync(priceOffer);
            await uow.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send message for PriceOffer ID: {id}", priceOfferId);
            throw new UserFriendlyException("Failed to send message. Please try again later.");
        }

        // Schedule email to send
        var emailModel = new PriceOfferDiscussionEmailModel(
            new PriceOfferEmailInfo(priceOffer.CurrentApproverRoleCode ?? "", priceOffer.ProjectResultStatus ?? "", priceOffer.ApprovalStatus, priceOffer.CreatorName, priceOffer.PriceOfferCode, priceOffer.ProjectName, priceOffer.BuyerCode, priceOffer.MaterialType, priceOffer.TotalStandardAmount, priceOffer.TotalMEVNOfferAmount ?? 0),
            emailsToSend.Count > 1 ? $"Dear All" : $"Dear Mr/Ms {GetDisplayNameFromEmail(emailsToSend.First())}",
            currentUserFullName,
            comment,
            DateTime.Now
        );
        var subject = EmailSubjectHelper.Generate(
            currentUserUsername,
            HistoryActions.Discussion.MessageSent,
            NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)),
            EmailRecipientRole.NormalRecipient,
            priceOffer.PriceOfferCode
        );

        var emailArgs = new SendEmailJobArgs(
            emailsToSend,
            subject,
            emailModel
        );

        await _emailJobScheduler.ScheduleEmailAsync(emailArgs);

        return ObjectMapper.Map<PriceOfferMessage, MessageDto>(message);
    }

    private async Task<(List<string> ValidEmails, List<string> InvalidEmails)> NormalizeEmailListAsync(List<string> emails)
    {
        var allUsers = await _identityUserRepository.GetListAsync();

        // Create a lookup dictionary for O(1) email lookups
        var userLookup = allUsers
            .Where(u => !string.IsNullOrEmpty(u.UserName))
            .ToDictionary(u => u.UserName!.Trim().ToLowerInvariant(), u => u.UserName!, StringComparer.OrdinalIgnoreCase);

        var validEmails = new List<string>(emails.Count);
        var invalidEmails = new List<string>();

        foreach (var email in emails)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            if (userLookup.TryGetValue(normalizedEmail, out var actualUserName))
            {
                validEmails.Add(actualUserName);
            }
            else
            {
                _logger.LogWarning("Email {email} does not match any user in the system.", email);
                invalidEmails.Add(email);
            }
        }

        return (validEmails, invalidEmails);
    }

    public virtual async Task<PriceOfferDto> SubmitProjectResultAsync(Guid id, SubmitProjectResultDto input)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(input.Note))
        {
            throw new AbpValidationException("Note is required");
        }

        var validStatuses = new[] { QuoteFlowStatuses.PriceOffer.Won, QuoteFlowStatuses.PriceOffer.PreOrder, QuoteFlowStatuses.PriceOffer.Lost };
        if (!validStatuses.Contains(input.ResultStatus))
        {
            throw new AbpValidationException($"Invalid result status. Valid values are: {string.Join(", ", validStatuses)}");
        }

        var isWinOrPreOrder = input.ResultStatus == QuoteFlowStatuses.PriceOffer.Won || input.ResultStatus == QuoteFlowStatuses.PriceOffer.PreOrder;
        var isLost = input.ResultStatus == QuoteFlowStatuses.PriceOffer.Lost;

        if (isWinOrPreOrder && (input.WinningCustomers == null || input.WinningCustomers.Count == 0))
        {
            throw new AbpValidationException("Winning customers must be specified when submitting a win or pre-order");
        }

        if (isLost && input.WinningCustomers != null && input.WinningCustomers.Count != 0)
        {
            throw new AbpValidationException("Winning customers must not be specified when submitting a loss");
        }

        // Get price offer with customers
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(id);

        // Validate that each channel has at least one customer selected for wins and pre-orders
        if (isWinOrPreOrder)
        {
            var customerChannels = priceOffer.Customers.GroupBy(c => c.SaleChannel).ToList();
            var winningChannels = input.WinningCustomers!.Select(w => w.ChannelId).ToHashSet();

            foreach (var channelGroup in customerChannels)
            {
                if (!winningChannels.Contains(channelGroup.Key))
                {
                    throw new AbpValidationException($"Must select a winning customer for channel: {channelGroup.Key}");
                }
            }            // Check for duplicate channels in winning customers
            var channelCounts = input.WinningCustomers!.GroupBy(w => w.ChannelId).ToList();
            var duplicateChannels = channelCounts.Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicateChannels.Any())
            {
                throw new AbpValidationException($"Only one customer per channel is allowed. Duplicate channels: {string.Join(", ", duplicateChannels)}");
            }
        }

        // Convert winning customers to dictionary for business logic
        Dictionary<string, Guid>? winningCustomersByChannel = null;
        if (isWinOrPreOrder && input.WinningCustomers != null)
        {
            winningCustomersByChannel = input.WinningCustomers.ToDictionary(w => w.ChannelId, w => w.CustomerId);
        }

        // Record the action in history
        var actionDate = DateTime.Now;
        string historyAction = input.ResultStatus switch
        {
            QuoteFlowStatuses.PriceOffer.Won => HistoryActions.PriceOffer.SubmittedAsWin,
            QuoteFlowStatuses.PriceOffer.PreOrder => HistoryActions.PriceOffer.SubmittedAsPreOrder,
            QuoteFlowStatuses.PriceOffer.Lost => HistoryActions.PriceOffer.SubmittedAsLoss,
            _ => throw new ArgumentException($"Unknown result status: {input.ResultStatus}")
        };

        var actionHistory = new PriceOfferApprovalHistory(
            GuidGenerator.Create(),
            priceOffer.Id,
            new ApprovalHistoryCreateParams(
                "SalePIC",
                "SalePIC",
                _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
                _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
                historyAction,
                actionDate,
                input.Note,
                true // Last step
            )
        );
        priceOffer.RecordAction(actionHistory);

        // Submit sales outcome using business logic
        priceOffer.SubmitProjectResult(
            input.ResultStatus,
            input.Note,
            _currentUser.Id ?? CurrentUser.Id ?? Guid.Empty,
            _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
            _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
            winningCustomersByChannel);

        // If win or pre-order, set winning customers and soft delete others
        if (isWinOrPreOrder && winningCustomersByChannel != null)
        {
            foreach (var kvp in winningCustomersByChannel)
            {
                priceOffer.SetWinningCustomerForChannel(kvp.Key, kvp.Value);
            }
        }

        priceOffer.SetCurrentRoute(null);

        // Update the price offer
        var updatedPriceOffer = await _priceOfferRepository.UpdateAsync(priceOffer, autoSave: true);

        await _localEventBus.PublishAsync(
               new PriceOfferActionedEvent(
                   priceOffer.Id,
                   historyAction,
                   _currentUser.Username!,
                   actionDate,
                   ""),
               onUnitOfWorkComplete: false
           );

        return ObjectMapper.Map<PriceOffer, PriceOfferDto>(updatedPriceOffer);
    }

    public virtual async Task<PriceOfferDto> ConfirmPreOrderStatusAsync(Guid id, ConfirmPreOrderStatusDto input)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(input.Note))
        {
            throw new AbpValidationException("Note is required");
        }

        var validStatuses = new[] { QuoteFlowStatuses.PriceOffer.Won, QuoteFlowStatuses.PriceOffer.Lost };
        if (!validStatuses.Contains(input.ResultStatus))
        {
            throw new AbpValidationException($"Invalid result status. Valid values are: {string.Join(", ", validStatuses)}");
        }

        // Get price offer
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(id);

        // Record the action in history
        var actionDate = DateTime.Now;
        string historyAction = input.ResultStatus switch
        {
            QuoteFlowStatuses.PriceOffer.Won => HistoryActions.PriceOffer.SubmittedAsWin,
            QuoteFlowStatuses.PriceOffer.Lost => HistoryActions.PriceOffer.SubmittedAsLoss,
            _ => throw new ArgumentException($"Unknown result status: {input.ResultStatus}")
        };

        var actionHistory = new PriceOfferApprovalHistory(
            GuidGenerator.Create(),
            priceOffer.Id,
            new ApprovalHistoryCreateParams(
                "SalePIC",
                "SalePIC",
                _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
                _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
                historyAction,
                actionDate,
                input.Note,
                true // Last step
            )
        );
        priceOffer.RecordAction(actionHistory);

        // Remove previous submit project result record history for price offer details if exists
        //var previousSubmitHistory = priceOffer.Details.SelectMany(d => d.ApprovalHistories)
        //    .Where(h => h.Action == HistoryActions.PriceOffer.SubmittedAsPreOrder)
        //    .ToList();

        //foreach (var detail in priceOffer.Details)
        //{
        //    var previousHistory = detail.ApprovalHistories
        //        .Where(h => h.Action == HistoryActions.PriceOffer.SubmittedAsPreOrder)
        //        .ToList();

        //    foreach (var history in previousHistory)
        //    {
        //        detail.ApprovalHistories.Remove(history);
        //    }
        //}

        // Confirm pre-order status using business logic
        priceOffer.ConfirmPreOrderStatus(
            input.ResultStatus,
            input.Note,
            _currentUser.Id ?? CurrentUser.Id ?? Guid.Empty,
            _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
            _currentUser.FullName ?? CurrentUser.Name ?? string.Empty);

        // Update the price offer
        var updatedPriceOffer = await _priceOfferRepository.UpdateAsync(priceOffer, autoSave: true);

        await _localEventBus.PublishAsync(
              new PriceOfferActionedEvent(
                  priceOffer.Id,
                  historyAction,
                  _currentUser.Username!,
                  actionDate,
                  ""),
              onUnitOfWorkComplete: false
          );

        return ObjectMapper.Map<PriceOffer, PriceOfferDto>(updatedPriceOffer);
    }

    public virtual async Task<ExcelValidationResult<PriceOfferUpdateLandingCostImportDto>> ValidateAndParseUpdateLandingCostAsync(Guid priceOfferId, IRemoteStreamContent file)
    {
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);
        // this action can only be performed if the price offer is in progress, and is before step 3 (Current step sequence < 3)
        if (priceOffer.CurrentApprovalStepSequence >= 3 || priceOffer.ApprovalStatus != QuoteFlowStatuses.InProgress)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.InvalidPriceOfferStatusForLandingCostUpdate);
        }

        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.ParentEntityId, priceOfferId);
        var validator = _excelImportFactory.CreateValidator<PriceOfferUpdateLandingCostImportDto>(ExcelImporters.PriceOfferUpdateLandingCost);
        await using var stream = file.GetStream();
        return await validator.ValidateAsync(stream, file.FileName ?? "", context);
    }

    public virtual async Task<List<PriceOfferDetailDto>> ImportUpdateLandingCostAsync(Guid priceOfferId, ExcelValidationResult<PriceOfferUpdateLandingCostImportDto> validationResult)
    {
        // Record the action in history
        var actionDate = DateTime.Now;
        var actionHistory = new PriceOfferApprovalHistory(
            GuidGenerator.Create(),
            validationResult.ListData.FirstOrDefault()?.RowData?.PriceOfferId ?? Guid.Empty,
            new ApprovalHistoryCreateParams(
                "FAP",
                "FAP",
                _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
                _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
                HistoryActions.PriceOffer.LandingCostUpdated,
                actionDate,
                "Landing cost updated via import"
            )
        );

        var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<PriceOfferUpdateLandingCostImportDto, PriceOfferDetailUpdateLandingCostParams>(ExcelImporters.PriceOfferUpdateLandingCost);

        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);
        priceOffer.RecordAction(actionHistory);
        var updatedIds = validationResult.ListData
            .Where(x => x.RowData != null && !x.Errors.Any() && x.RowData!.PriceOfferDetailId.HasValue)
            .Select(x => x.RowData!.PriceOfferDetailId!.Value)
            .Distinct()
            .ToList();
        foreach (var detail in priceOffer.Details.Where(d => updatedIds.Contains(d.Id)))
        {
            var detailHistory = new PriceOfferDetailApprovalHistory(
                GuidGenerator.Create(),
                detail.Id,
                new ApprovalHistoryCreateParams(
                    "FAP",
                    "FAP",
                    _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
                    _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
                    HistoryActions.PriceOffer.LandingCostUpdated,
                    actionDate,
                    "Landing cost updated via import"
                )
            );
            detail.RecordAction(detailHistory);
        }

        var context = new ExcelImportContext();
        var updatedDetails = new List<PriceOfferDetail>();

        foreach (var rowResult in validationResult.ListData.Where(x => x.RowData != null && !x.Errors.Any()))
        {
            var updateParams = await createParamsConverters.ConvertToCreateParamsAsync(rowResult, context, default);

            if (updateParams != null)
            {
                var updatedDetail = await _priceOfferDetailManager.UpdateLandingCostAsync(updateParams);
                updatedDetails.Add(updatedDetail);
            }
        }

        await UnitOfWorkManager.Current!.SaveChangesAsync();

        // call stored procedure to update calculated values and regenerate approval routes
        priceOffer = await _priceOfferRepository.UpdateCalculatedFieldsAsync(priceOfferId);
        await _priceOfferRepository.GenerateApprovalRouteAsync(priceOfferId, priceOffer.PriceOfferCode);

        priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);
        return ObjectMapper.Map<List<PriceOfferDetail>, List<PriceOfferDetailDto>>([.. priceOffer.Details]);
    }

    public async Task<PriceOfferDto> AssignSpecialInputPrice(Guid id, AssignSpecialInputPriceDto input)
    {
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(id);
        priceOffer.ValidateConcurrencyStamp(input.ConcurrencyStamp);

        var speicalInputPrice = await _specialInputPriceRepository.GetAsync(input.SpecialInputPriceId);

        var accountNo = speicalInputPrice.AccountNo;

        priceOffer.AccountNo = accountNo;
        priceOffer.SpecialInputPriceAccountName = speicalInputPrice.AccountName;

        var note = accountNo + ": " + input.Note;

        if (priceOffer.ApprovalStatus != QuoteFlowStatuses.InProgress || priceOffer.CurrentApprovalStepSequence >= 3)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.InvalidPriceOfferStatusForLandingCostUpdate);
        }

        var history = new PriceOfferApprovalHistory(
            GuidGenerator.Create(),
            priceOffer.Id,
            new ApprovalHistoryCreateParams(
                "FAP",
                "FAP",
                _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
                _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
                HistoryActions.PriceOffer.SpecialInputPriceAssigned,
                DateTime.Now,
                note
            )
        );
        priceOffer.RecordAction(history);

        var specialInputPrice = await _specialInputPriceRepository.GetAsync(input.SpecialInputPriceId);
        priceOffer.AssignSpecialInputPrice(
            specialInputPrice,
            _currentUser.Id ?? Guid.Empty,
            _currentUser.Username ?? "N/A",
            _currentUser.FullName ?? "N/A",
            input.Note);

        //priceOffer = await _priceOfferRepository.UpdateAsync(priceOffer, true);
        priceOffer = await _priceOfferRepository.UpdateCalculatedFieldsAsync(id, true);

        // Track history for price offer details that had special input price applied
        await TrackSpecialInputPriceHistoryForDetailsAsync(id, input.SpecialInputPriceId, note);

        var updatedPriceOffer = await _priceOfferRepository.UpdateAsync(priceOffer, true);

        await _localEventBus.PublishAsync(
              new PriceOfferActionedEvent(
                  priceOffer.Id,
                  HistoryActions.PriceOffer.SpecialInputPriceAssigned,
                  _currentUser.Username!,
                  DateTime.Now,
                  ""),
              onUnitOfWorkComplete: false
          );

        return ObjectMapper.Map<PriceOffer, PriceOfferDto>(updatedPriceOffer);
    }




    // -- Private methods for validation and handling actions --
    private async Task ValidateAction(PriceOffer priceOffer, ActionDto input)
    {
        var validationResults = input.ValidateConcurrencyStamp(priceOffer.ConcurrencyStamp);
        if (validationResults.Any())
        {
            throw new AbpValidationException(validationErrors: validationResults.ToList());
        }

        validationResults = await _priceOfferManager.ValidateActionAsync(priceOffer);
        if (validationResults.Any())
        {
            throw new AbpValidationException(validationErrors: validationResults.ToList());
        }
    }

    private async Task<PriceOffer> HandleApprovalAsync(
        PriceOffer priceOffer,
        PriceOfferApprovalRoute currentApprovalStep,
        bool isLastStep,
        DateTime actionDate,
        string? approvalNote = null)
    {
        var isFirstApproval = priceOffer.IsAwaitingInitialApproval();
        var isPendingProjectOffer = priceOffer.IsPendingForSales();
        priceOffer.Approve(actionDate, approvalNote, isLastStep);


        var nextStep = _approvalRouteManager.GetNextApprovalStep(priceOffer.ApprovalRoutes, currentApprovalStep);
        if (isLastStep && isFirstApproval)
        {
            priceOffer.ApprovalRoutes.Clear();
            await _approvalRouteManager.RemoveRoutes(currentApprovalStep.InstanceId!.Value, EntityTypes.PriceOffer);

            if (isPendingProjectOffer)
            {
                await SendBackToSaleForConfirmProjectResult(priceOffer);
            }
            else
            {
                priceOffer.SetCurrentRoute(null);
            }
        }
        else if (isLastStep)
        {
            // This is the last step but not the first approval step
            // => Only the case import more items
            // => Auto-merge on approval
            //  - When an item is approved and a previous approved item with the same golfa +price + spoId exists, mark the
            //new one MERGED and add its qty to the existing item.
            // map id with its value
            var idMapToQty = priceOffer.Details
                .ToDictionary(x => x.Id, x => x.Qty);

            var newestImportGuid = priceOffer.Details
                .OrderByDescending(x => x.CreationTime)
                .Select(x => x.ImportGuid)
                .FirstOrDefault();
            var mergedIds = priceOffer.MergeDetails(newestImportGuid);
            // record histories for all these merged detail ids
            foreach (var detailId in mergedIds)
            {
                var previousQty = idMapToQty.GetValueOrDefault(detailId, 0);
                var currentQty = priceOffer.Details.FirstOrDefault(d => d.Id == detailId)?.Qty ?? 0;
                var mergedQty = currentQty - previousQty;
                var detailHistory = new PriceOfferDetailApprovalHistory(
                    GuidGenerator.Create(),
                    detailId,
                    new ApprovalHistoryCreateParams(
                        "System",
                        "System",
                        "System",
                        "System",
                        HistoryActions.PriceOfferDetail.QuantityMerged,
                        actionDate,
                        $"Merged quantity: added {mergedQty:N0} to original {previousQty:N0}, new total: {currentQty:N0}"
                    )
                );
                priceOffer.Details.FirstOrDefault(d => d.Id == detailId)?.RecordAction(detailHistory);
            }

            await _approvalRouteManager.RemoveRoutes(currentApprovalStep.InstanceId!.Value, EntityTypes.PriceOffer);
            if (isPendingProjectOffer)
            {
                await SendBackToSaleForConfirmProjectResult(priceOffer);
            }
            else
            {
                priceOffer.SetCurrentRoute(null);
            }
            priceOffer = await _priceOfferRepository.UpdateCalculatedFieldsAsync(priceOffer.Id);
        }
        else if (nextStep is not null)
        {
            priceOffer.SetCurrentRoute(new(
                nextStep.InstanceId,
                nextStep.StepSequence,
                nextStep.ApproverRoleCode,
                nextStep.ApproverRoleName
            ));
        }
        else
        {
            _logger.LogWarning("No next approval step found for PriceOffer ID: {id}", priceOffer.Id);
            throw new BusinessException(QuoteFlowDomainErrorCodes.NoApprovalRouteFound);
        }

        return priceOffer;
    }

    private async Task SendBackToSaleForConfirmProjectResult(PriceOffer priceOffer)
    {
        var salePics = await _salesAssignmentRepository.GetListAsync(new()
        {
            BuyerId = priceOffer.BuyerId,
            LocationId = priceOffer.LocationId,
            BuyerTypeId = priceOffer.BuyerTypeId,
            MaterialType = priceOffer.MaterialType,
        });
        var instanceId = GuidGenerator.Create();
        var stepSequence = PriceOfferConsts.SalePicStepSequence;
        var routes = salePics.Select(x => new PriceOfferApprovalRoute(
            priceOffer.Id,
            GuidGenerator.Create(),
            stepSequence,
            PriceOfferConsts.SalePICApproverRoleCode,
            PriceOfferConsts.SalePICApproverRoleName,
            instanceId,
            x.SaleUserName
        ));
        priceOffer.SetCurrentRoute(new(
            instanceId,
            stepSequence,
            PriceOfferConsts.SalePICApproverRoleCode,
            PriceOfferConsts.SalePICApproverRoleName
        ));
        await _approvalRouteRepository.InsertManyAsync(routes);
    }

    private async Task<PriceOffer> HandleRejectionAsync(PriceOffer priceOffer, PriceOfferApprovalRoute currentApprovalStep)
    {
        priceOffer.Reject();
        priceOffer.SetCurrentRoute(null);
        await _approvalRouteManager.RemoveRoutes(currentApprovalStep.InstanceId!.Value, EntityTypes.PriceOffer);

        priceOffer = await _priceOfferRepository.UpdateCalculatedFieldsAsync(priceOffer.Id);

        return priceOffer;
    }

    private async Task<PriceOffer> HandleCancellationAsync(PriceOffer priceOffer, PriceOfferApprovalRoute currentApprovalStep)
    {
        priceOffer.Cancel();

        // cancel when Price Offer Is Project Offer and Pending for Sales
        if (priceOffer.IsPendingForSales())
        {
            await SendBackToSaleForConfirmProjectResult(priceOffer);
        }
        else
        {
            priceOffer.SetCurrentRoute(null);
            await _approvalRouteManager.RemoveRoutes(currentApprovalStep.InstanceId!.Value, EntityTypes.PriceOffer);
        }

        priceOffer = await _priceOfferRepository.UpdateCalculatedFieldsAsync(priceOffer.Id);
        return priceOffer;
    }

    private async Task<PriceOffer> HandleClosureAsync(PriceOffer priceOffer, ActionDto input, DateTime actionDate)
    {
        var closePersonRole = "N/A";
        if (string.Equals(_currentUser.Username, priceOffer.CreatorUsername, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(_currentUser.Username, priceOffer.LastApprovalRouteCreatorUsername, StringComparison.OrdinalIgnoreCase))
        {
            closePersonRole = "Sale PIC";
        }
        else
        {
            closePersonRole = "FAP";
        }

        var baseHistoryCreateParams = new ApprovalHistoryCreateParams(
            closePersonRole,
            closePersonRole,
            _currentUser.Username,
            _currentUser.FullName,
            HistoryActions.Closed,
            actionDate,
            input.Comment,
            false
        );

        var history = new PriceOfferApprovalHistory(
            GuidGenerator.Create(),
            priceOffer.Id,
            baseHistoryCreateParams
        );
        priceOffer.RecordAction(history);

        foreach (var detail in priceOffer.Details)
        {
            if (!detail.IsApproved())
            {
                continue;
            }

            var detailHistory = new PriceOfferDetailApprovalHistory(
                GuidGenerator.Create(),
                detail.Id,
                baseHistoryCreateParams
            );

            detail.RecordAction(detailHistory);
        }

        priceOffer.Close();

        if (priceOffer.CurrentApprovalRouteInstanceId.HasValue)
        {
            await _approvalRouteManager.RemoveRoutes(priceOffer.CurrentApprovalRouteInstanceId.Value, EntityTypes.PriceOffer);
        }

        return priceOffer;
    }

    private async Task ValidateAddMoreItemsLimitAsync(PriceOffer priceOffer, decimal totalImpact)
    {
        if (priceOffer.IsKeyAccountPriceOffer())
        {
            return;
        }

        if (priceOffer.IsBuyerStockPriceOffer() && priceOffer.HasDPOUsed)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.CannotAddMoreItemsWhenDPOUsed)
                .WithData("reason", "Cannot add more items: This price offer's special pricing has already been applied to a DPO.");
        }

        // Get the allowance percentage from system configuration
        var allowanceConfigs = await _systemConfigurationRepository.GetListAsync(
            cfgKey: SystemConfigurationKeys.SpoAddItemAllowancePercent,
            maxResultCount: 1);

        var allowanceConfig = allowanceConfigs.FirstOrDefault();
        if (allowanceConfig == null)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.CannotAddMoreItemsWhenDPOUsed)
                .WithData("reason", "Allowance percentage configuration not found");
        }

        if (!decimal.TryParse(allowanceConfig.CfgValue, out var allowancePercent))
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.CannotAddMoreItemsWhenDPOUsed)
                .WithData("reason", "Invalid allowance percentage configuration");
        }

        // Calculate the allowable range based on the initial amount when DPO was first used
        var baseAmount = priceOffer.InitialTotalMEVNOfferAmount;
        var allowanceAmount = baseAmount * (allowancePercent / 100);
        var upperLimit = baseAmount + allowanceAmount;
        var lowerLimit = baseAmount - allowanceAmount;

        // Calculate what the new total would be after this import
        var currentAmount = priceOffer.TotalMEVNOfferAmount ?? 0;
        var newTotalAmount = currentAmount + totalImpact;

        // Validate that the new total stays within the allowable range
        if (newTotalAmount > upperLimit || newTotalAmount < lowerLimit)
        {
            var reason = newTotalAmount > upperLimit
                ? $"Adjusted amount {newTotalAmount:N0} exceeds the allowed upper limit {upperLimit:N0}. " +
                  $"Base {baseAmount:N0}, allowance {allowancePercent:N0}%, current {currentAmount:N0}, adjustment {totalImpact:N0}."
                : $"Adjusted amount {newTotalAmount:N0} is below the allowed lower limit {lowerLimit:N0}. " +
                  $"Base {baseAmount:N0}, allowance {allowancePercent:N0}%, current {currentAmount:N0}, adjustment {totalImpact:N0}.";

            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.CannotAddMoreItemsWhenDPOUsed)
                .WithData("reason", reason);
        }
    }

    private async Task CreatePriceOfferDetailsExcelWithSummaryAsync(List<PriceOfferDetailExcelDto> itemDtos, MemoryStream memoryStream)
    {
        await Task.CompletedTask; // Suppress async warning

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("ExportData");

        if (!itemDtos.Any())
        {
            workbook.SaveAs(memoryStream);
            return;
        }

        // Set up headers
        var headers = new[]
        {
            "No.", "Golfa Code", "Model Name", "Special Spec 1", "Special Spec 2",
            "SPO Qty", "Standard Price", "Standard Amount", "Buyer Price", "Requested Amount",
            "Requested Discount Ratio", "Price To Customer", "Price To Customer Amount",
            "MEVN Offer Price", "MEVN Offer Amount", "DPO Used Qty", "DPO Used Amount", "Competitor Brand", "Competitor Model",
            "Competitor Price", "Landed Cost", "Price Offer Detail Margin", "Account Code",
            "Note", "Approval Status", "Project Result", "Actual Discount Ratio"
        };

        // Add headers to row 1
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
        }

        // Format header row
        var headerRange = worksheet.Range(1, 1, 1, headers.Length);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        // Add data rows
        var dataStartRow = 2;
        for (int i = 0; i < itemDtos.Count; i++)
        {
            var item = itemDtos[i];
            var row = i + dataStartRow;

            worksheet.Cell(row, 1).Value = item.RowNo;
            worksheet.Cell(row, 2).Value = item.GolfaCode ?? "";
            worksheet.Cell(row, 3).Value = item.ModelName ?? "";
            worksheet.Cell(row, 4).Value = item.SpecialSpec1 ?? "";
            worksheet.Cell(row, 5).Value = item.SpecialSpec2 ?? "";
            worksheet.Cell(row, 6).Value = item.Qty;
            worksheet.Cell(row, 7).Value = item.StandardPrice;
            worksheet.Cell(row, 8).Value = item.StandardAmount;
            worksheet.Cell(row, 9).Value = item.BuyerPrice ?? 0;
            worksheet.Cell(row, 10).Value = item.RequestedAmount ?? 0;
            worksheet.Cell(row, 11).Value = item.RequestedDiscountRatio ?? 0;
            worksheet.Cell(row, 12).Value = item.PriceToCustomer ?? 0;
            worksheet.Cell(row, 13).Value = item.PriceToCustomerAmount;
            worksheet.Cell(row, 14).Value = item.MEVNOfferPrice;
            worksheet.Cell(row, 15).Value = item.MEVNOfferAmount ?? 0;
            worksheet.Cell(row, 16).Value = item.DpoUsed ?? 0;
            worksheet.Cell(row, 17).Value = item.DpoUsedAmount ?? 0;
            worksheet.Cell(row, 18).Value = item.CompetitorBrand ?? "";
            worksheet.Cell(row, 19).Value = item.CompetitorModel ?? "";
            worksheet.Cell(row, 20).Value = item.CompetitorPrice ?? 0;
            worksheet.Cell(row, 21).Value = item.LandingCost ?? 0;
            worksheet.Cell(row, 22).Value = item.PriceOfferDetailMargin ?? 0;
            worksheet.Cell(row, 23).Value = item.AccountCode ?? "";
            worksheet.Cell(row, 24).Value = item.Note ?? "";
            worksheet.Cell(row, 25).Value = item.Status ?? "";
            worksheet.Cell(row, 26).Value = item.ProjectResult ?? "";
            worksheet.Cell(row, 27).Value = item.ActualDiscountRatio ?? 0;
        }

        // Add summary row
        var summaryRow = itemDtos.Count + dataStartRow;
        worksheet.Cell(summaryRow, 1).Value = "TOTAL";
        worksheet.Cell(summaryRow, 1).Style.Font.Bold = true;

        // Calculate totals and ratios for numeric columns
        var totalQty = itemDtos.Sum(x => x.Qty);
        var totalStandardAmount = itemDtos.Sum(x => x.StandardAmount);
        var totalRequestedAmount = itemDtos.Sum(x => x.RequestedAmount ?? 0);
        var totalPriceToCustomerAmount = itemDtos.Sum(x => x.PriceToCustomerAmount);
        var totalMEVNOfferAmount = itemDtos.Sum(x => x.MEVNOfferAmount ?? 0);
        var totalDpoUsed = itemDtos.Sum(x => x.DpoUsed ?? 0);
        var totalDpoUsedAmount = itemDtos.Sum(x => x.DpoUsedAmount ?? 0);

        // Calculate weighted averages for ratio columns
        var avgRequestedDiscountRatio = totalRequestedAmount > 0 ?
            itemDtos.Where(x => x.RequestedAmount.HasValue && x.RequestedAmount > 0)
                    .Sum(x => (x.RequestedDiscountRatio ?? 0) * x.RequestedAmount.Value) / totalRequestedAmount : 0;

        var avgPriceOfferDetailMargin = totalMEVNOfferAmount > 0 ?
            itemDtos.Where(x => x.MEVNOfferAmount.HasValue && x.MEVNOfferAmount > 0)
                    .Sum(x => (x.PriceOfferDetailMargin ?? 0) * x.MEVNOfferAmount.Value) / totalMEVNOfferAmount : 0;

        var avgActualDiscountRatio = totalStandardAmount > 0 ?
            1 - (totalMEVNOfferAmount / totalStandardAmount) : 0;
        // Set summary values
        worksheet.Cell(summaryRow, 6).Value = totalQty; // Quantity
        worksheet.Cell(summaryRow, 8).Value = totalStandardAmount; // Standard Amount
        worksheet.Cell(summaryRow, 10).Value = totalRequestedAmount; // Requested Amount
        worksheet.Cell(summaryRow, 11).Value = avgRequestedDiscountRatio; // Requested Discount Ratio
        worksheet.Cell(summaryRow, 13).Value = totalPriceToCustomerAmount; // Price To Customer Amount
        worksheet.Cell(summaryRow, 15).Value = totalMEVNOfferAmount; // MEVN Offer Amount
        worksheet.Cell(summaryRow, 16).Value = totalDpoUsed; // DPO Used
        worksheet.Cell(summaryRow, 17).Value = totalDpoUsedAmount; // DPO Used Amount
        worksheet.Cell(summaryRow, 22).Value = avgPriceOfferDetailMargin; // Price Offer Detail Margin
        worksheet.Cell(summaryRow, 27).Value = avgActualDiscountRatio; // Actual Discount Ratio

        // Format summary row
        var summaryRange = worksheet.Range(summaryRow, 1, summaryRow, headers.Length);
        summaryRange.Style.Font.Bold = true;
        summaryRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        summaryRange.Style.Border.TopBorder = XLBorderStyleValues.Thick;

        // Apply column highlighting only to data rows (excluding header row and summary row)
        // MEVN Offer Price & MEVN Offer Amount: #ebf1de (light green)
        var mevnOfferPriceRange = worksheet.Range(dataStartRow, 14, summaryRow - 1, 14); // MEVN Offer Price
        var mevnOfferAmountRange = worksheet.Range(dataStartRow, 15, summaryRow - 1, 15); // MEVN Offer Amount
        mevnOfferPriceRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#ebf1de");
        mevnOfferAmountRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#ebf1de");

        // DPO Used & DPO Used Amount: #f2dcdb (light red)
        var dpoUsedRange = worksheet.Range(dataStartRow, 16, summaryRow - 1, 16); // DPO Used
        var dpoUsedAmountRange = worksheet.Range(dataStartRow, 17, summaryRow - 1, 17); // DPO Used Amount
        dpoUsedRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#f2dcdb");
        dpoUsedAmountRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#f2dcdb");

        // Apply number formatting to relevant columns
        var numberColumns = new[] { 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 17, 20, 21 }; // Amount columns  
        var percentageColumns = new[] { 11, 22, 27 }; // Ratio columns

        var dataRange = worksheet.Range(dataStartRow, 1, summaryRow, headers.Length);
        foreach (var col in numberColumns)
        {
            worksheet.Column(col).Style.NumberFormat.Format = "#,##0";
        }
        foreach (var col in percentageColumns)
        {
            worksheet.Column(col).Style.NumberFormat.Format = "#,##0.0%";
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        // Set column widths based on MiniExcel attributes (approximate)
        worksheet.Column(1).Width = 15; // No.
        worksheet.Column(2).Width = 25; // Golfa Code
        worksheet.Column(3).Width = 40; // Model Name
        worksheet.Column(4).Width = 25; // Special Spec 1
        worksheet.Column(5).Width = 25; // Special Spec 2
        worksheet.Column(23).Width = 40; // Note

        workbook.SaveAs(memoryStream);
    }


    public virtual async Task<List<PriceOfferDetailDto>> CancelPriceOfferDetailsAsync(Guid priceOfferId, PriceOfferDetailCancelDto input)
    {
        // Get the price offer with details to ensure it exists and get type info
        var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(priceOfferId);

        // Validate that it's an AP type price offer (only AP allows cancelling approved items)
        if (priceOffer.GetPriceOfferType() != PriceOfferTypes.PriceOfferAP)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.InvalidPriceOfferType)
                .WithData("expectedTypes", PriceOfferTypes.PriceOfferAP)
                .WithData("actualType", priceOffer.GetPriceOfferType());
        }

        // Get the price offer details to be cancelled
        var priceOfferDetailsToCancel = await _priceOfferDetailRepository.GetListAsync(
            detail => input.PriceOfferDetailIds.Contains(detail.Id) && detail.PriceOfferId == priceOfferId,
            includeDetails: true);

        if (priceOfferDetailsToCancel.Count != input.PriceOfferDetailIds.Count)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.PriceOfferDetailsNotFound)
                .WithData("message", "One or more price offer details not found or do not belong to the specified price offer.");
        }

        // Use the PriceOfferDetailManager to cancel the details
        var cancelledDetails = await _priceOfferDetailManager.CancelDetailsAsync(priceOfferDetailsToCancel, input.Note);

        // Record approval history for each cancelled detail
        var actionDate = DateTime.Now;
        var baseHistoryCreateParams = new ApprovalHistoryCreateParams(
            "User",
            "User",
            _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
            _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
            HistoryActions.Cancelled,
            actionDate,
            input.Note,
            true // Final action for this detail
        );

        foreach (var detail in cancelledDetails)
        {
            var detailHistory = new PriceOfferDetailApprovalHistory(
                GuidGenerator.Create(),
                detail.Id,
                baseHistoryCreateParams
            );

            detail.ApprovalHistories.Add(detailHistory);
            await _priceOfferDetailRepository.UpdateAsync(detail);
        }

        // Refresh the price offer to get updated totals
        priceOffer = await _priceOfferRepository.UpdateCalculatedFieldsAsync(priceOfferId);

        return ObjectMapper.Map<List<PriceOfferDetail>, List<PriceOfferDetailDto>>(cancelledDetails);
    }

    private async Task TrackSpecialInputPriceHistoryForDetailsAsync(Guid priceOfferId, Guid specialInputPriceId, string? note)
    {
        // Get the special input price with its details to find matching material codes
        var specialInputPriceQuery = await _specialInputPriceRepository.WithDetailsAsync(x => x.Details);
        var specialInputPrice = await AsyncExecuter.FirstOrDefaultAsync(
            specialInputPriceQuery.Where(x => x.Id == specialInputPriceId)
        );

        if (specialInputPrice == null || specialInputPrice.Details == null || !specialInputPrice.Details.Any())
        {
            return; // No details to match against
        }

        // Extract material codes from special input price details
        var materialCodes = specialInputPrice.Details
            .Where(d => !string.IsNullOrEmpty(d.MaterialCode))
            .Select(d => d.MaterialCode!)
            .ToHashSet();

        if (!materialCodes.Any())
        {
            return; // No material codes to match
        }

        var priceOfferDetailQuery = await _priceOfferDetailRepository.GetQueryableAsync();
        var matchedDetails = await AsyncExecuter.ToListAsync(
            priceOfferDetailQuery.Where(pod =>
                pod.PriceOfferId == priceOfferId &&
                materialCodes.Contains(pod.GolfaCode) &&
                pod.Status == QuoteFlowStatuses.InProgress
            )
        );

        if (!matchedDetails.Any())
        {
            return; // No matching details found
        }

        // Create history records for each matched detail
        var actionDate = DateTime.Now;
        var historyCreateParams = new ApprovalHistoryCreateParams(
            "FAP",
            "FAP",
            _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
            _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
            HistoryActions.PriceOfferDetail.SpecialInputPriceAssigned,
            actionDate,
            note
        );

        foreach (var detail in matchedDetails)
        {
            var detailHistory = new PriceOfferDetailApprovalHistory(
                GuidGenerator.Create(),
                detail.Id,
                historyCreateParams
            );

            // Use the RecordAction method to add history to the detail
            detail.RecordAction(detailHistory);
            await _priceOfferDetailRepository.UpdateAsync(detail);
        }
    }
}
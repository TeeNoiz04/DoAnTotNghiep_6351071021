using ClosedXML.Excel;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using QuoteFlow.BackgroundJobs.Emailing;
using QuoteFlow.BuyerAccess;
using QuoteFlow.Buyers;
using QuoteFlow.Cargos.CargoDatas;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using QuoteFlow.DPOs.DpoGkrUsages;
using QuoteFlow.DPOs.DPOMessages;
using QuoteFlow.DPOs.Events;
using QuoteFlow.DPOs.Models;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.Emailing;
using QuoteFlow.Emailing.EmailInfoModel;
using QuoteFlow.Emailing.EmailModels;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialStocks;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;
using QuoteFlow.Messages;
using QuoteFlow.Permissions;
using QuoteFlow.PriceOffers;
using QuoteFlow.PurchaseOrderDetails;
using QuoteFlow.PurchaseOrderLockShipments;
using QuoteFlow.PurchaseOrders;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SaleOrders;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.Flagging;
using QuoteFlow.Shared.Models;
using QuoteFlow.Shared.Utils;
using QuoteFlow.StockCategories;
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
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.FileManagement.Files;

namespace QuoteFlow.DPOs;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.MovingOrders.DPOs.DPODefault)]
public class DPOsAppService : QuoteFlowAppService, IDPOsAppService
{
    protected IDistributedCache<DPODownloadTokenCacheItem, string> _downloadTokenCache;
    protected IDistributedCache<DPODetailDownloadTokenCacheItem, string> _downloadTokenDPODetailCache;
    protected IDPORepository _dPORepository;
    protected readonly ILogger<DPOsAppService> _logger;
    protected readonly IIdentityUserRepository _identityUserRepository;
    protected IMaterialStockRepository _materialStockRepository;
    protected IMaterialStockLockStockRepository _materialStockLockStockRepository;
    protected IPurchaseOrderLockShipmentRepository _purchaseOrderLockShipmentRepository;
    protected IPurchaseOrderRepository _poRepository;
    protected ICargoDataRepository _cargoDataRepository;
    protected IPurchaseOrderDetailRepository _poDetailRepository;
    protected IDPODetailRepository _dPODetailRepository;
    protected IPriceOfferRepository _priceOfferRepository;
    protected DPOManager _dPOManager;
    protected DPODetailManager _dPODetailManager;
    protected IExcelImportFactory _excelImportFactory;
    protected ILocalEventBus _localEventBus;
    protected IEffectiveUserContext _currentUser;
    protected readonly IFlaggingService<DPO, DPOFlagsDto> _flaggingService;
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    private readonly FileDescriptorAppService _fileDescriptorAppService;
    protected readonly ISalesAssignmentRepository _salesAssignmentRepository;
    protected IMaterialStockLockShipmentRepository _materialStockLockShipmentRepository;
    protected IStockCategoryRepository _stockCategoryRepository;
    protected readonly IEmailJobScheduler _emailJobScheduler;
    protected IBuyerAccessService _buyerAccessService;
    protected IMaterialRepository _materialRepository;
    protected IBuyerRepository _buyerRepository;
    protected IDpoGkrUsageRepository _dpoGkrUsageRepository;


    public DPOsAppService(
        IDPORepository dPORepository,
        IDPODetailRepository dPODetailRepository,
        DPOManager dPOManager,
        IDistributedCache<DPODownloadTokenCacheItem, string> downloadTokenCache,
        IExcelImportFactory excelImportFactory,
        ILocalEventBus localEventBus,
        IEffectiveUserContext currentUser,
        IMaterialStockRepository materialStockRepository,
        IMaterialStockLockStockRepository materialStockLockStockRepository,
        IFlaggingService<DPO, DPOFlagsDto> flaggingService,
        IPurchaseOrderRepository poRepository,
        IPurchaseOrderDetailRepository poDetailRepository,
        ICargoDataRepository cargoDataRepository,
        IPurchaseOrderLockShipmentRepository purchaseOrderLockShipmentRepository,
        DPODetailManager dPODetailManager,
        IPriceOfferRepository priceOfferRepository,
        IRepository<FileDescriptor, Guid> fileDescriptorRepository,
        FileDescriptorAppService fileDescriptorAppService,
        ILogger<DPOsAppService> logger,
        IIdentityUserRepository identityUserRepository,
        ISalesAssignmentRepository salesAssignmentRepository,
        IMaterialStockLockShipmentRepository materialStockLockShipmentRepository,
        IDistributedCache<DPODetailDownloadTokenCacheItem, string> downloadTokenDPODetailCache,
        IStockCategoryRepository stockCategoryRepository,
        IBuyerAccessService buyerAccessService,
        IEmailJobScheduler emailJobScheduler,
        IMaterialRepository materialRepository,
        IBuyerRepository buyerRepository,
        IDpoGkrUsageRepository dpoGkrUsageRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _dPORepository = dPORepository;
        _dPODetailRepository = dPODetailRepository;
        _dPOManager = dPOManager;
        _excelImportFactory = excelImportFactory;
        _localEventBus = localEventBus;
        _currentUser = currentUser;
        _materialStockRepository = materialStockRepository;
        _materialStockLockStockRepository = materialStockLockStockRepository;
        _flaggingService = flaggingService;
        _poRepository = poRepository;
        _poDetailRepository = poDetailRepository;
        _cargoDataRepository = cargoDataRepository;
        _purchaseOrderLockShipmentRepository = purchaseOrderLockShipmentRepository;
        _dPODetailManager = dPODetailManager;
        _priceOfferRepository = priceOfferRepository;
        _fileDescriptorRepository = fileDescriptorRepository;
        _fileDescriptorAppService = fileDescriptorAppService;
        _logger = logger;
        _identityUserRepository = identityUserRepository;
        _salesAssignmentRepository = salesAssignmentRepository;
        _materialStockLockShipmentRepository = materialStockLockShipmentRepository;
        _downloadTokenDPODetailCache = downloadTokenDPODetailCache;
        _stockCategoryRepository = stockCategoryRepository;
        _emailJobScheduler = emailJobScheduler;
        _buyerAccessService = buyerAccessService;
        _materialRepository = materialRepository;
        _buyerRepository = buyerRepository;
        _dpoGkrUsageRepository = dpoGkrUsageRepository;
    }

    public virtual async Task<PagedResultDto<DPODto>> GetListAsync(GetDPOsInput input)
    {
        var filterParams = ObjectMapper.Map<GetDPOsInput, DPOFilterParams>(input);

        // Apply buyer access restrictions using centralized service
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        filterParams.ApplyBuyerRestrictions(buyerAccess);
        filterParams.ApplyMaterialTypeRestrictions(buyerAccess);

        var totalCount = await _dPORepository.GetCountAsync(filterParams);
        var items = await _dPORepository.GetListAsync(filterParams, input.Sorting, input.MaxResultCount, input.SkipCount);

        var flags = await _flaggingService.CreateBulkFlagsAsync(items);
        var itemDtos = ObjectMapper.Map<List<DPO>, List<DPODto>>(items);
        itemDtos.ForEach(i => i.Flags = flags[i.Id]);

        return new PagedResultDto<DPODto>
        {
            TotalCount = totalCount,
            Items = itemDtos
        };
    }

    public virtual async Task<DPODto> GetAsync(Guid id)
    {
        var dpo = await _dPORepository.GetAsync(id);
        var dto = ObjectMapper.Map<DPO, DPODto>(dpo);


        List<string> golfas = dto.Details
           .Select(x => x.GolfaCode)
           .Distinct()
           .ToList();

        // Get all materials whose GolfaCode is in the list of golfas
        var materials = await _materialRepository.GetListAsync(
            x => golfas.Contains(x.GolfaCode)
        );
        foreach (var item in dto.Details)
        {
            //get Spec1, Spec2 into Materils
            var material = materials.FirstOrDefault(m => m.GolfaCode == item.GolfaCode);
            if (material != null)
            {
                item.Spec1 = material.Spec1;
                item.Spec2 = material.Spec2;
            }
        }
        dto.Flags = await _flaggingService.CreateFlagsAsync(dpo);
        return dto;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var dpo = await _dPORepository.GetAsync(id);
        if (dpo.Details.Any(x => x.Delivered > 0 || x.LockStockSO > 0 || x.LockStock > 0 || x.LockShipment > 0))
        {
            throw new UserFriendlyException("DPO cannot be deleted due to delivered or locked items.");
        }

        // Prepare event data before deletion
        var eventDetails = dpo.Details
            .Where(x => !string.IsNullOrEmpty(x.SPOCode))
            .Select(detail => new DPODeletedEventDetail(
                detail.Id,
                detail.SPOCode!,
                detail.GolfaCode,
                detail.UnitPrice ?? 0,
                detail.Qty ?? 0
            ))
            .ToList();

        var dpoHistory = SetDPOApproval(id, HistoryActions.DPO.Deleted);
        dpo.RecordAction(dpoHistory);
        await _dPORepository.DeleteAsync(id);

        // Publish event after successful deletion
        if (eventDetails.Count != 0)
        {
            await _localEventBus.PublishAsync(new DPODeletedEvent(id, eventDetails), false);
        }
    }


    public virtual async Task<DPODto> CreateAsync(DPOCreateDto input)
    {
        var createParams = ObjectMapper.Map<DPOCreateDto, DPOCreateParams>(input);
        var dPO = await _dPOManager.CreateAsync(
        createParams
        );

        return ObjectMapper.Map<DPO, DPODto>(dPO);
    }


    public virtual async Task<DPODto> UpdateAsync(Guid id, DPOUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<DPOUpdateDto, DPOUpdateParams>(input);
        var dPO = await _dPOManager.UpdateAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<DPO, DPODto>(dPO);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(DPOExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var filterParams = ObjectMapper.Map<DPOExcelDownloadDto, DPOFilterParams>(input);

        // Apply buyer access restrictions using centralized service
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        filterParams.ApplyBuyerRestrictions(buyerAccess);
        filterParams.ApplyMaterialTypeRestrictions(buyerAccess);

        var items = await _dPORepository.GetListAsync(filterParams);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<DPO>, List<DPOExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "DPOs.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new DPODownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new QuoteFlow.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }

    public virtual async Task<ExcelValidationResult<ImportDPODto>> ValidateAndParseDPOAsync(IRemoteStreamContent file, ImportDPOInput input)
    {
        var validator = _excelImportFactory.CreateValidator<ImportDPODto>(ExcelImporters.DPO);

        var context = new ExcelImportContext();
        if (!string.IsNullOrEmpty(input.MaterialType))
        {
            context.SetData(ExcelImportContextKeys.DPO.MaterialType, input.MaterialType);
        }

        context.SetData(ExcelImportContextKeys.DPO.BuyerId, input.BuyerId);


        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);

        var dpo = result.ListData.FirstOrDefault()?.RowData;

        if (dpo != null)
        {
            dpo.MaterialType = input.MaterialType;
            dpo.BuyerId = input.BuyerId;
            dpo.BuyerTypeId = input.BuyerTypeId;
            dpo.ConfirmDate = input.ConfirmDate;
        }

        return result;
    }


    public virtual async Task<DPODto> ImportDPOAsync(ExcelValidationResult<ImportDPODto> data, bool force = false)
    {
        if (data.HasErrors)
        {
            throw new UserFriendlyException("Validation errors found in the import data. Please check the file and try again.");
        }

        var rowResult = data.ListData.FirstOrDefault()
            ?? throw new UserFriendlyException("No valid data found in the import file.");

        var dpo = rowResult.RowData;

        // Check for price mismatches when force=false
        if (!force && dpo?.Details?.ListData?.Any() == true)
        {
            await ValidatePriceMismatchesAsync(dpo.Details, dpo.BuyerId, dpo.MaterialType);
        }

        var converter = _excelImportFactory.CreateCreateParamsConverter<ImportDPODto, DPOCreateParams>(ExcelImporters.DPO);

        var context = new ExcelImportContext();
        // create context is list of spo codes linked with its accountNo (if any)
        var spoCodes = rowResult.RowData.Details.ListData
            .Select(x => x?.RowData?.SPOCode)
            .Where(x => x is not null)
            .ToList();
        var priceOffers = await _priceOfferRepository.GetListAsync(x => spoCodes.Contains(x.PriceOfferCode));
        var spoCodeAccountNoMap = priceOffers
            .ToDictionary(x => x.PriceOfferCode, x => x.AccountNo);
        context.SetData(ExcelImportContextKeys.DPO.SPOCodeAccountNoMap, spoCodeAccountNoMap);

        if (!string.IsNullOrEmpty(dpo?.MaterialType))
        {
            context.SetData("MaterialType", dpo.MaterialType);
        }

        var createParams = await converter.ConvertToCreateParamsAsync(rowResult, context, default)
            ?? throw new UserFriendlyException("Failed to convert import data to create parameters.");

        var createdDpo = await _dPOManager.CreateAsync(createParams);



        var dpoHistory = SetDPOApproval(createdDpo.Id, HistoryActions.DPO.Confirmed);
        createdDpo.RecordAction(dpoHistory);

        await CheckBypassConfirmationAsync(createdDpo);
        if (createdDpo.IsLockedStock)
        {
            var autoConfirmLockStockHistory = new DPOApprovalHistory(
                GuidGenerator.Create(),
                createdDpo.Id,
                new ApprovalHistoryCreateParams
                {
                    Action = HistoryActions.DPO.ConfirmedLockStock,
                    ActionDate = Clock.Now.AddSeconds(1),
                    ApproverUsername = "System",
                    ApproverFullName = "System",
                    ApproverRoleCode = "System Bypass Action",
                    ApproverRoleName = "System Bypass Action",
                    EntityType = EntityTypes.DPO,
                    IsLastApprovalInCurrentWorkflow = false,
                    Note = "Bypassed at the confirm lock stock step."
                }
            );

            createdDpo.RecordAction(autoConfirmLockStockHistory);
        }
        if (createdDpo.IsInProgress())
        {
            var autoConfirmLockStockHistory = new DPOApprovalHistory(
                GuidGenerator.Create(),
                createdDpo.Id,
                new ApprovalHistoryCreateParams
                {
                    Action = HistoryActions.DPO.ConfirmedLockStock,
                    ActionDate = Clock.Now.AddSeconds(1),
                    ApproverUsername = "System",
                    ApproverFullName = "System",
                    ApproverRoleCode = "System Bypass Action",
                    ApproverRoleName = "System Bypass Action",
                    EntityType = EntityTypes.DPO,
                    IsLastApprovalInCurrentWorkflow = false,
                    Note = "Bypassed at the confirm lock stock step."
                }
            );

            createdDpo.RecordAction(autoConfirmLockStockHistory);

            var autoConfirmLockOnOrderHistory = new DPOApprovalHistory(
                    GuidGenerator.Create(),
                    createdDpo.Id,
                    new ApprovalHistoryCreateParams
                    {
                        Action = HistoryActions.DPO.ConfirmedLockOnOrder,
                        ActionDate = Clock.Now.AddSeconds(2),
                        ApproverUsername = "System",
                        ApproverFullName = "System",
                        ApproverRoleCode = "System Bypass Action",
                        ApproverRoleName = "System Bypass Action",
                        EntityType = EntityTypes.DPO,
                        IsLastApprovalInCurrentWorkflow = false,
                        Note = "Bypassed at the confirm lock on order stock step."
                    }
                );

            createdDpo.RecordAction(autoConfirmLockOnOrderHistory);

        }

        await _localEventBus.PublishAsync(new DPOImportedEvent(createdDpo.Id), true);
        return ObjectMapper.Map<DPO, DPODto>(createdDpo);
    }

    public virtual async Task<List<DPODetailDto>> ConfirmNoteAsync(DPOConfirmNoteDto input)
    {
        var dpoDetails = new List<DPODetail>();

        foreach (var id in input.DPODetailIds)
        {
            var dpoDetail = await _dPODetailRepository.GetAsync(id);
            dpoDetail.ConfirmNoted = input.Note;
            await _dPODetailRepository.UpdateAsync(dpoDetail);
            dpoDetails.Add(dpoDetail);
        }

        return ObjectMapper.Map<List<DPODetail>, List<DPODetailDto>>(dpoDetails);
    }


    public virtual async Task<List<DPODetailDto>> AddExtraFeeAsync(DPOAddExtraFeeDto input)
    {
        var dpoDetails = new List<DPODetail>();

        foreach (var id in input.DPODetailIds)
        {
            var dpoDetail = await _dPODetailRepository.GetAsync(id);
            var usedExtraFee = (dpoDetail.Extrafee ?? 0) - (dpoDetail.ExtrafeeAvailable ?? 0);
            if (input.ExtraFee < dpoDetail.ExtrafeeUsedInSO)
            {
                throw new UserFriendlyException("The extra fee cannot be less than the used amount in Sale Orders.");
            }
            dpoDetail.Extrafee = input.ExtraFee;
            dpoDetail.ExtrafeeNote = input.ExtraFeeNote;
            dpoDetail.ExtrafeeAvailable = input.ExtraFee - usedExtraFee;
            dpoDetail.AmountIncludeExtraFee = (dpoDetail.Amount ?? 0) + (dpoDetail.Extrafee ?? 0);
            await _dPODetailRepository.UpdateAsync(dpoDetail);
            dpoDetails.Add(dpoDetail);
        }

        var dpo = await _dPORepository.GetAsync(dpoDetails.First().DPOId);
        dpo.TotalAmount = dpo.CalculateTotalAmount();
        dpo.TotalAmountIncludeExtraFee = dpo.CalculateTotalAmount(includeExtraFee: true);
        await _dPORepository.UpdateAsync(dpo);

        return ObjectMapper.Map<List<DPODetail>, List<DPODetailDto>>(dpoDetails);
    }

    public virtual async Task<DPODto> CancelAsync(Guid id, DPOCancelDto input)
    {
        // Get the DPO with its details
        var dpo = await _dPORepository.GetAsync(id);
        // Get the details to be cancelled
        var detailsToCancel = dpo.Details
            .Where(d => input.DPODetailIds.Contains(d.Id))
            .ToList();
        // Validate concurrency stamp
        var validationResults = dpo.ValidateConcurrencyStamp(input.ConcurrencyStamp);
        if (validationResults.Any())
        {
            throw new AbpValidationException(validationErrors: validationResults.ToList());
        }

        validationResults = await _dPODetailManager.ValidationCancelAsync(detailsToCancel);
        if (validationResults.Any())
        {
            throw new AbpValidationException(validationErrors: validationResults.ToList());
        }

        // Record history for DPO
        var actionDate = DateTime.Now;

        // Record history for each detail
        foreach (var detail in detailsToCancel)
        {
            var detailHistory = new DPODetailApprovalHistory(
                GuidGenerator.Create(),
                detail.Id,
                new(
                    "FA Team",
                    "FA Team",
                    _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
                    _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
                    HistoryActions.Cancelled,
                    actionDate,
                    input.Note,
                    false,
                    entityType: EntityTypes.DPODetail)
            );
            detail.RecordAction(detailHistory);
        }

        // Prepare event data before cancelling (only for details with SPOCode)
        var eventDetails = detailsToCancel
            .Where(x => !string.IsNullOrEmpty(x.SPOCode))
            .Select(detail => new DPODeletedEventDetail(
                detail.Id,
                detail.SPOCode!,
                detail.GolfaCode,
                detail.UnitPrice ?? 0,
                detail.Qty ?? 0
            ))
            .ToList();

        // Cancel the DPO (this will also cancel the specified details)
        dpo.Cancel(input.DPODetailIds);

        // Update the DPO
        await _dPORepository.UpdateAsync(dpo);
        if (dpo.IsCancelled)
        {
            var pdoHistory = SetDPOApproval(dpo.Id, HistoryActions.DPO.Cancelled, note: input.Note);
            dpo.RecordAction(pdoHistory);
        }

        if (dpo.IsClosed())
        {
            var pdoHistory = SetDPOApproval(dpo.Id, HistoryActions.DPO.Closed, note: input.Note);
            dpo.RecordAction(pdoHistory);
        }

        // Update the details
        foreach (var detail in detailsToCancel)
        {
            await _dPODetailRepository.UpdateAsync(detail);
        }

        // Publish event after successful cancellation to reduce DPO used amounts
        if (eventDetails.Count != 0)
        {
            await _localEventBus.PublishAsync(new DPOCanceledEvent(id, eventDetails), false);
        }

        return ObjectMapper.Map<DPO, DPODto>(dpo);
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockStock)]
    public virtual async Task LockStockAutoAsync(DPOLockStockAutoDto input)
    {
        await _dPORepository.LockStockAutoAsync(
            input.DPOId,
            input.StockCategoryId,
            _currentUser.Username ?? string.Empty,
            _currentUser.FullName ?? string.Empty
        );
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockStock)]
    public virtual async Task LockStockAutoV2Async(DPOLockStockAutoV2Dto input)
    {
        await _dPORepository.LockStockAutoV2Async(
            input.DPODetailIds,
            input.StockCategoryId,
            _currentUser.Username ?? string.Empty,
            _currentUser.FullName ?? string.Empty
        );
        foreach (var detailId in input.DPODetailIds)
        {
            var dpoDetail = await _dPODetailRepository.GetAsync(detailId);
            var autoReleaseLockStockHistory = new DPODetailApprovalHistory(
                    GuidGenerator.Create(),
                    dpoDetail.Id,
                    new ApprovalHistoryCreateParams
                    {
                        Action = HistoryActions.DPO.AutoLockStock,
                        ActionDate = Clock.Now.AddSeconds(2),
                        ApproverUsername = _currentUser.Username,
                        ApproverFullName = _currentUser.FullName,
                        ApproverRoleCode = "FA Team",
                        ApproverRoleName = "FA Team",
                        EntityType = EntityTypes.DPODetail,
                        IsLastApprovalInCurrentWorkflow = false,
                        Note = $"Auto lock stock for GolfaCode: {dpoDetail.GolfaCode}"

                    }
                );

            dpoDetail.RecordAction(autoReleaseLockStockHistory);
        }

    }

    public virtual async Task LockShipmentAutoAsync(DPOLockShipmentAutoDto input)
    {
        await _dPORepository.LockShipmentAutoAsync(
            input.DPODetailIds,
            input.Note,
            _currentUser.Username ?? string.Empty,
            _currentUser.FullName ?? string.Empty
        );
        foreach (var detailId in input.DPODetailIds)
        {
            var dpoDetail = await _dPODetailRepository.GetAsync(detailId);
            var autoReleaseLockStockHistory = new DPODetailApprovalHistory(
                    GuidGenerator.Create(),
                    dpoDetail.Id,
                    new ApprovalHistoryCreateParams
                    {
                        Action = HistoryActions.DPO.AutoLockOnOrder,
                        ActionDate = Clock.Now.AddSeconds(2),
                        ApproverUsername = _currentUser.Username,
                        ApproverFullName = _currentUser.FullName,
                        ApproverRoleCode = "FA Team",
                        ApproverRoleName = "FA Team",
                        EntityType = EntityTypes.DPODetail,
                        IsLastApprovalInCurrentWorkflow = false,
                        Note = $"{input.Note}"

                    }
                );

            dpoDetail.RecordAction(autoReleaseLockStockHistory);
        }
    }

    public virtual async Task<ListResultDto<MaterialStockLockStockDto>> GetLockStocksAsync(Guid dpoId, Guid detailId)
    {
        var dpoDetail = await _dPODetailRepository.GetAsync(detailId);
        if (dpoDetail.DPOId != dpoId)
        {
            throw new UserFriendlyException("The detail does not belong to the specified DPO.");
        }

        var lockStocks = await _materialStockLockStockRepository.GetListAsync(
            dPODetailId: detailId
        );

        return new ListResultDto<MaterialStockLockStockDto>(
            ObjectMapper.Map<List<MaterialStockLockStock>, List<MaterialStockLockStockDto>>(lockStocks)
        );
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockStock)]
    public virtual async Task<DPODto> ConfirmLockStockAsync(Guid id, NoteMetadataDto input)
    {
        var dpo = await _dPORepository.GetAsync(id);

        // Call domain method to change status
        dpo.ConfirmLockStock();

        // Record history
        var dpoHistory = SetDPOApproval(id, HistoryActions.DPO.ConfirmedLockStock, note: input.Note);
        dpo.RecordAction(dpoHistory);

        // Update the entity
        await _dPORepository.UpdateAsync(dpo);

        await CheckBypassLockOnOrderStockAsync(dpo);

        if (dpo.IsInProgress())
        {


            var autoConfirmLockOnOrderHistory = new DPOApprovalHistory(
                    GuidGenerator.Create(),
                    dpo.Id,
                    new ApprovalHistoryCreateParams
                    {
                        Action = HistoryActions.DPO.ConfirmedLockOnOrder,
                        ActionDate = Clock.Now.AddSeconds(1),
                        ApproverUsername = "System",
                        ApproverFullName = "System",
                        ApproverRoleCode = "System Bypass Action",
                        ApproverRoleName = "System Bypass Action",
                        EntityType = EntityTypes.DPO,
                        IsLastApprovalInCurrentWorkflow = false,
                        Note = "Bypassed at the confirm lock on order stock step."
                    }
                );

            dpo.RecordAction(autoConfirmLockOnOrderHistory);

        }
        // Return DTO with flags
        var dto = ObjectMapper.Map<DPO, DPODto>(dpo);
        dto.Flags = await _flaggingService.CreateFlagsAsync(dpo);
        return dto;
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockOnOrderStock)]
    public virtual async Task<DPODto> ConfirmLockOnOrderAsync(Guid id, NoteMetadataDto input)
    {
        var dpo = await _dPORepository.GetAsync(id);

        // Call domain method to change status
        dpo.ConfirmLockOnOrder();


        var dpoHistory = SetDPOApproval(id, HistoryActions.DPO.ConfirmedLockOnOrder, note: input.Note);
        dpo.RecordAction(dpoHistory);

        // Update the entity
        await _dPORepository.UpdateAsync(dpo);

        // Return DTO with flags
        var dto = ObjectMapper.Map<DPO, DPODto>(dpo);
        dto.Flags = await _flaggingService.CreateFlagsAsync(dpo);
        return dto;
    }

    public virtual async Task<ListResultDto<DPOListPOsDto>> GetListAvailablePOsAsync(Guid dpoId, Guid dpoDetailId, string? materialCode)
    {
        var models = await _dPORepository.GetListAvailablePOsAsync(dpoDetailId, materialCode);
        var itemDtos = ObjectMapper.Map<List<DPOListPOsModel>, List<DPOListPOsDto>>(models);
        itemDtos = itemDtos.OrderBy(x => x.PODate).ToList();
        return new ListResultDto<DPOListPOsDto>(itemDtos);
    }

    public virtual async Task<ListResultDto<DPOLockOnOrderStockDto>> GetListLockOnOrderStockAsync(Guid dpoId, Guid dpoDetailId)
    {
        var purchaseOrderLockShipments = await _purchaseOrderLockShipmentRepository.GetListAsync(
            dPODetailId: dpoDetailId);

        var poDetailIds = purchaseOrderLockShipments
            .Select(x => x.PODetailId)
            .Distinct()
            .ToList();

        if (purchaseOrderLockShipments.Count == 0)
        {
            return new ListResultDto<DPOLockOnOrderStockDto>([]);
        }

        var poDetails = await _poDetailRepository.GetListWithDetailsAsync(x => poDetailIds.Contains(x.Id));
        var allGolfaCodes = poDetails.Select(x => x.GolfaCode).Distinct().ToList();
        var cargoDataFull = await _cargoDataRepository.GetListAsync(y => allGolfaCodes.Contains(y.GolfaCode));

        var itemDtos = purchaseOrderLockShipments.Select(x =>
        {
            var poDetail = poDetails.FirstOrDefault(pd => pd.Id == x.PODetailId);
            var po = poDetail?.PurchaseOrder;
            var cargoData = cargoDataFull.FirstOrDefault(cd =>
                cd.GolfaCode.Equals(poDetail?.GolfaCode, StringComparison.OrdinalIgnoreCase) &&
                cd.PODetailId == poDetail.Id &&
                string.Equals(cd.PODetailCode, poDetail.PODetailCode, StringComparison.OrdinalIgnoreCase)
            );

            return new DPOLockOnOrderStockDto
            {
                Id = x.Id,
                PODetailId = x.PODetailId,
                PONo = po?.PONo ?? "",
                POQty = poDetail?.Qty ?? 0,
                PODate = po?.PODate,
                MachineNumber = cargoData?.MachineNumber,
                STCReply = cargoData?.STCReply,
                QtyLocked = x.Qty,
                QtyImported = x.QtyDisposed,
                QtyNeedImport = x.QtyNeed,
                Status = x.Qty == x.QtyDisposed ? QuoteFlowStatuses.Closed : QuoteFlowStatuses.InProgress,
                Note = x.Note,
                LastModifierUsername = x.LastModifierUsername
            };
        }).ToList();

        return new ListResultDto<DPOLockOnOrderStockDto>(itemDtos);
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockOnOrderStock)]
    public virtual async Task LockShipmentAsync(Guid dpoDetailId, DPOLockShipmentDto input)
    {
        var dpoDetails = new List<DPODetail>();
        var dpoDetail = await _dPODetailRepository.GetAsync(dpoDetailId);

        var totalLockQty = input.Items.Sum(x => x.Qty);
        if (totalLockQty > dpoDetail.NeedDelivery)
        {
            throw new UserFriendlyException($"The total qty to Lock ({totalLockQty}) exceeds the Need Order qty ({dpoDetail.NeedDelivery}).");
        }

        var errorMessages = new List<string>();
        foreach (var item in input.Items)
        {
            // Call stored procedure for each item
            var error = await _dPORepository.LockShipmentAsync(
                item.PODetailId,
                dpoDetailId,
                item.GolfaCode,
                item.Qty,
                item.Note,
                _currentUser.Username ?? "",
                _currentUser.FullName ?? ""
            );
            if (!string.IsNullOrWhiteSpace(error))
            {
                errorMessages.Add($"{item.GolfaCode}: {error}");
                continue;
            }

            // Retrieve the updated DPO detail
            dpoDetails.Add(dpoDetail);
        }
        if (errorMessages.Any())
        {
            throw new UserFriendlyException(string.Join("\n", errorMessages));
        }
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockOnOrderStock)]
    public virtual async Task UpdateLockShipmentAsync(Guid dpoDetailId, Guid poDetailId, DPOLockShipmentItemUpdateDto input)
    {
        var errorMessage = await _dPORepository.UpdateLockShipmentAsync(
            poDetailId,
            dpoDetailId,
            input.GolfaCode,
            input.Qty,
            input.Note,
            _currentUser.Username ?? string.Empty,
            _currentUser.FullName ?? string.Empty
        );

        if (!string.IsNullOrEmpty(errorMessage))
        {
            throw new UserFriendlyException(errorMessage);
        }
    }

    public virtual async Task<ListResultDto<DPOLockStockEtaEtdDto>> GetListLockStockEtaEtdAsync(Guid dpoDetailId, Guid poDetailId)
    {
        var models = await _dPORepository.GetListLockStockEtaEtdAsync(dpoDetailId, poDetailId);
        var itemsDto = ObjectMapper.Map<List<DPOLockStockEtaEtdModel>, List<DPOLockStockEtaEtdDto>>(models);
        return new ListResultDto<DPOLockStockEtaEtdDto>(itemsDto);
    }

    public virtual async Task UpdateLockStockDetailAsync(Guid dpoDetailId, DPODetailUpdateLockStockDto input)
    {
        // Get DPO detail to retrieve GolfaCode
        var dpoDetail = await _dPODetailRepository.GetAsync(dpoDetailId);

        // Call stored procedure instead of code logic
        var errorMessage = await _dPORepository.UpdateLockStockAsync(
            dpoDetailId,
            dpoDetail.GolfaCode,
            input.StockCategoryId,
            input.NewQty,
            input.Note,
            _currentUser.Username ?? string.Empty,
            _currentUser.FullName ?? string.Empty
        );

        // If stored procedure returns an error message, throw exception
        if (!string.IsNullOrEmpty(errorMessage))
        {
            throw new UserFriendlyException(errorMessage);
        }
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockStock)]
    public virtual async Task DeleteLockStockAsync(Guid dpoDetailId, Guid lockStockId)
    {
        var errorMes = await _dPORepository.DeleteDPOLockStockAsync(dpoDetailId, lockStockId, _currentUser.Username, _currentUser.FullName);
        if (!string.IsNullOrEmpty(errorMes))
        {
            throw new UserFriendlyException(errorMes);
        }
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockOnOrderStock)]
    public virtual async Task DeleteLockOnOrderStockAsync(Guid dpoDetailId, Guid poDetailId)
    {
        var errorMes = await _dPORepository.DeleteLockOnOrderStockAsync(poDetailId, dpoDetailId, _currentUser.Username, _currentUser.FullName);
        if (!string.IsNullOrEmpty(errorMes))
        {
            throw new UserFriendlyException(errorMes);
        }
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.ConfirmReject)]
    public virtual async Task<DPODto> ApproveAsync(Guid id, NoteMetadataDto input)
    {
        var dpo = await _dPORepository.GetAsync(id);

        // Validate concurrency stamp
        var validationResults = dpo.ValidateConcurrencyStamp(input.ConcurrencyStamp);
        if (validationResults.Any())
        {
            throw new AbpValidationException(validationErrors: validationResults.ToList());
        }

        // Call domain method to approve
        await CheckBypassConfirmationAsync(dpo);


        // Record history for DPO
        var actionDate = Clock.Now;
        var dpoHistory = SetDPOApproval(dpo.Id, HistoryActions.DPO.Confirmed, input.Note);

        dpo.RecordAction(dpoHistory);
        if (dpo.IsLockedStock)
        {
            var autoConfirmLockStockHistory = new DPOApprovalHistory(
                GuidGenerator.Create(),
                dpo.Id,
                new ApprovalHistoryCreateParams
                {
                    Action = HistoryActions.DPO.ConfirmedLockStock,
                    ActionDate = Clock.Now.AddSeconds(1),
                    ApproverUsername = "System",
                    ApproverFullName = "System",
                    ApproverRoleCode = "System Bypass Action",
                    ApproverRoleName = "System Bypass Action",
                    EntityType = EntityTypes.DPO,
                    IsLastApprovalInCurrentWorkflow = false,
                    Note = "Bypassed at the confirm lock stock step."
                }
            );

            dpo.RecordAction(autoConfirmLockStockHistory);
        }
        if (dpo.IsInProgress())
        {
            var autoConfirmLockStockHistory = new DPOApprovalHistory(
                GuidGenerator.Create(),
                dpo.Id,
                new ApprovalHistoryCreateParams
                {
                    Action = HistoryActions.DPO.ConfirmedLockStock,
                    ActionDate = Clock.Now.AddSeconds(1),
                    ApproverUsername = "System",
                    ApproverFullName = "System",
                    ApproverRoleCode = "System Bypass Action",
                    ApproverRoleName = "System Bypass Action",
                    EntityType = EntityTypes.DPO,
                    IsLastApprovalInCurrentWorkflow = false,
                    Note = "Bypassed at the confirm lock stock step."
                }
            );

            dpo.RecordAction(autoConfirmLockStockHistory);

            var autoConfirmLockOnOrderHistory = new DPOApprovalHistory(
                    GuidGenerator.Create(),
                    dpo.Id,
                    new ApprovalHistoryCreateParams
                    {
                        Action = HistoryActions.DPO.ConfirmedLockOnOrder,
                        ActionDate = Clock.Now.AddSeconds(2),
                        ApproverUsername = "System",
                        ApproverFullName = "System",
                        ApproverRoleCode = "System Bypass Action",
                        ApproverRoleName = "System Bypass Action",
                        EntityType = EntityTypes.DPO,
                        IsLastApprovalInCurrentWorkflow = false,
                        Note = "Bypassed at the confirm lock on order stock step."
                    }
                );

            dpo.RecordAction(autoConfirmLockOnOrderHistory);

        }
        // Update the entity
        await _dPORepository.UpdateAsync(dpo);

        await _localEventBus.PublishAsync(
             new DPOActionedEvent(
                 dpo.Id,
                 HistoryActions.DPO.Confirmed,
                 _currentUser.Username!,
                 actionDate,
                 ""),
             onUnitOfWorkComplete: false
         );

        // Return DTO with flags
        var dto = ObjectMapper.Map<DPO, DPODto>(dpo);
        dto.Flags = await _flaggingService.CreateFlagsAsync(dpo);
        return dto;
    }

    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.ConfirmReject)]
    public virtual async Task<DPODto> RejectAsync(Guid id, NoteMetadataDto input)
    {
        var dpo = await _dPORepository.GetAsync(id);

        // Validate concurrency stamp
        var validationResults = dpo.ValidateConcurrencyStamp(input.ConcurrencyStamp);
        if (validationResults.Any())
        {
            throw new AbpValidationException(validationErrors: validationResults.ToList());
        }

        // Prepare event data BEFORE calling domain method (for consistency with Cancel/Delete)
        var eventDetails = dpo.Details
            .Where(x => !string.IsNullOrEmpty(x.SPOCode))
            .Select(detail => new DPODeletedEventDetail(
                detail.Id,
                detail.SPOCode!,
                detail.GolfaCode,
                detail.UnitPrice ?? 0,
                detail.Qty ?? 0
            ))
            .ToList();

        // Call domain method to reject (this will also reject all details)
        dpo.Reject();

        // Record history for DPO
        var actionDate = Clock.Now;
        var dpoHistory = SetDPOApproval(dpo.Id, HistoryActions.Rejected, note: input.Note);

        dpo.RecordAction(dpoHistory);

        // Record history for each detail
        foreach (var detail in dpo.Details)
        {
            var detailHistory = new DPODetailApprovalHistory(
                GuidGenerator.Create(),
                detail.Id,
                new ApprovalHistoryCreateParams
                {
                    Action = HistoryActions.Rejected,
                    ActionDate = actionDate,
                    ApproverUsername = _currentUser.Username,
                    ApproverFullName = _currentUser.FullName,
                    ApproverRoleCode = "FA Team",
                    ApproverRoleName = "FA Team",
                    EntityType = EntityTypes.DPODetail,
                    IsLastApprovalInCurrentWorkflow = false,
                    Note = input.Note
                }
            );
            detail.RecordAction(detailHistory);
        }

        // Update the entity
        await _dPORepository.UpdateAsync(dpo);

        // Update all details
        foreach (var detail in dpo.Details)
        {
            await _dPODetailRepository.UpdateAsync(detail);
        }

        // Return DTO with flags
        var dto = ObjectMapper.Map<DPO, DPODto>(dpo);
        dto.Flags = await _flaggingService.CreateFlagsAsync(dpo);
        await _localEventBus.PublishAsync(
             new DPOActionedEvent(
                 dpo.Id,
                 HistoryActions.Rejected,
                 _currentUser.Username!,
                 actionDate,
                 ""),
             onUnitOfWorkComplete: false
         );

        // Publish event to reduce DPO used amounts in PriceOffers
        if (eventDetails.Count != 0)
        {
            await _localEventBus.PublishAsync(new DPORejectedEvent(dpo.Id, eventDetails), false);
        }

        return dto;
    }
    [Authorize(QuoteFlowPermissions.Reports.R24DPOProcessing)]
    public async Task<IRemoteStreamContent> GetListDPOProcessingReportAsync(GetDPOReportInputDto input)
    {
        // 1. Get data
        var fullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        var items = await _dPORepository.GetListDPOProcessingReportAsync(input.BuyerTypeId, input.BuyerId, input.FromDate, input.ToDate, fullBuyerAccess, _currentUser.Username ?? "");
        // 2. Get the template file
        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "DPOProcessingReport.xlsx")
            ?? throw new UserFriendlyException("Template Excel not found.");
        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);
        // 3. Copy the template to a temporary stream
        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;
        // 4. Load workbook
        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();
        int startRow = 7; // start from row 5
        int startCol = 1; // column A
        // 5. Insert additional rows if needed
        if (items.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(items.Count - 1);
        }
        // 6. Write data to the sheet
        int currentRow = startRow;

        foreach (var item in items)
        {
            int col = startCol;

            ws.Cell(currentRow, col++).Value = item.DPONo;
            ws.Cell(currentRow, col++).Value = item.GolfaCode;
            ws.Cell(currentRow, col++).Value = item.Model;
            ws.Cell(currentRow, col++).Value = item.Spec1;
            ws.Cell(currentRow, col++).Value = item.MaterialType;
            ws.Cell(currentRow, col++).Value = item.Material_Group;
            ws.Cell(currentRow, col++).Value = item.BuyerTypeDescription;
            ws.Cell(currentRow, col++).Value = item.BuyerShortName;
            ws.Cell(currentRow, col++).Value = item.OrderDate;
            ws.Cell(currentRow, col++).Value = item.RequestedETA;
            ws.Cell(currentRow, col++).Value = item.SPOCode;
            ws.Cell(currentRow, col++).Value = item.CustomerName;
            ws.Cell(currentRow, col++).Value = item.Status;
            ws.Cell(currentRow, col++).Value = item.Qty;
            ws.Cell(currentRow, col++).Value = item.UnitPrice;
            ws.Cell(currentRow, col++).Value = item.Extrafee;
            ws.Cell(currentRow, col++).Value = item.AmountIncludeExtrafee;
            ws.Cell(currentRow, col++).Value = item.Delivered;
            ws.Cell(currentRow, col++).Value = item.Delivery_Price;
            ws.Cell(currentRow, col++).Value = item.DeliveryOrder_Amount;
            ws.Cell(currentRow, col++).Value = item.Remain_Qty;
            ws.Cell(currentRow, col++).Value = item.Remain_Amount;

            currentRow++;
        }


        ws.Cell("A3").Value = DateTime.Now;
        ws.Cell("A3").Style.DateFormat.Format = "dd/MM/yyyy HH:mm:ss";

        ws.Cell("N6").Value = items.Sum(x => x.Qty);
        ws.Cell("Q6").Value = items.Sum(x => x.AmountIncludeExtrafee);
        ws.Cell("S6").Value = items.Sum(x => x.Delivery_Price);
        ws.Cell("T6").Value = items.Sum(x => x.DeliveryOrder_Amount);
        ws.Cell("U6").Value = items.Sum(x => x.Remain_Qty);
        ws.Cell("V6").Value = items.Sum(x => x.Remain_Amount);


        int sumRow = currentRow + 1;

        // Ghi tổng các cột
        ws.Cell(sumRow, 14).Value = items.Sum(x => x.Qty);
        ws.Cell(sumRow, 17).Value = items.Sum(x => x.AmountIncludeExtrafee);
        ws.Cell(sumRow, 19).Value = items.Sum(x => x.Delivery_Price);
        ws.Cell(sumRow, 20).Value = items.Sum(x => x.DeliveryOrder_Amount);
        ws.Cell(sumRow, 21).Value = items.Sum(x => x.Remain_Qty);
        ws.Cell(sumRow, 22).Value = items.Sum(x => x.Remain_Amount);

        var sumRange = ws.Range(sumRow, 14, sumRow, 22);
        sumRange.Style.Font.Bold = true;
        sumRange.Style.Font.FontColor = XLColor.Red;
        sumRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        sumRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;


        // 9. Save workbook to a new stream
        var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
        outputStream.Position = 0;

        // 9. Return the file to the client
        return new RemoteStreamContent(
            outputStream,
            "DPOProcessingReport.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }
    [Authorize(QuoteFlowPermissions.Reports.R25DPOReceivedByMaterialType)]
    public async Task<IRemoteStreamContent> GetListDPOReportAsync(GetDPOReportInputDto input)
    {
        // 1. Get data
        var fullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        var items = await _dPORepository.GetListDPOReportAsync(input.BuyerTypeId, input.BuyerId, input.FromDate, input.ToDate, fullBuyerAccess, _currentUser.Username ?? "");

        // 2. Get the template file
        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "DPOReport.xlsx")
            ?? throw new UserFriendlyException("Template Excel not found.");
        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        // 3. Copy the template to a temporary stream
        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        // 4. Load workbook
        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();
        int startRow = 5; // start from row 5
        int startCol = 1; // column A

        // 5. Group items by BuyerShortName to calculate subtotals
        var groupedItems = items.GroupBy(x => new { x.BuyerType, x.BuyerShortName }).ToList();

        // Calculate total rows needed (data rows + subtotal rows)
        int totalRowsNeeded = items.Count + groupedItems.Count;

        // 6. Insert additional rows if needed
        if (totalRowsNeeded > 1)
        {
            ws.Row(startRow).InsertRowsBelow(totalRowsNeeded - 1);
        }

        // 7. Calculate totals for FA and LVS by month
        var faItems = items.Where(x => x.MaterialType == "FA").ToList();
        var lvsItems = items.Where(x => x.MaterialType == "LVS").ToList();

        // Write FA totals to row 3 (D3:O3)
        for (int monthCol = 4; monthCol <= 15; monthCol++) // Columns D to O (January to December)
        {
            var faSum = faItems.Sum(item => GetMonthValue(item, monthCol - 4));
            ws.Cell(3, monthCol).Value = faSum;
        }

        // Write LVS totals to row 4 (D4:O4)  
        for (int monthCol = 4; monthCol <= 15; monthCol++) // Columns D to O (January to December)
        {
            var lvsSum = lvsItems.Sum(item => GetMonthValue(item, monthCol - 4));
            ws.Cell(4, monthCol).Value = lvsSum;
        }

        // 8. Write data to the sheet
        int currentRow = startRow;

        // 8. Write data to the sheet
        //int currentRow = startRow;

        foreach (var group in groupedItems)
        {
            var groupItems = group.ToList();
            int groupStartRow = currentRow;

            // Add subtotal row FIRST
            var subtotalRow = ws.Row(currentRow);
            subtotalRow.Cell(1).Value = group.Key.BuyerType;
            subtotalRow.Cell(2).Value = group.Key.BuyerShortName;
            subtotalRow.Cell(3).Value = ""; // Empty MaterialType

            // Calculate and write subtotals for each month
            for (int monthCol = 4; monthCol <= 15; monthCol++) // Columns D to O (January to December)
            {
                var sum = groupItems.Sum(item => GetMonthValue(item, monthCol - 4));
                subtotalRow.Cell(monthCol).Value = sum;
            }
            var totalSum = groupItems.Sum(item =>
           (item.January ?? 0) + (item.February ?? 0) + (item.March ?? 0) + (item.April ?? 0) +
           (item.May ?? 0) + (item.June ?? 0) + (item.July ?? 0) + (item.August ?? 0) +
           (item.September ?? 0) + (item.October ?? 0) + (item.November ?? 0) + (item.December ?? 0));
            subtotalRow.Cell(16).Value = totalSum; // Column P

            // Style the subtotal row - tô màu từ cột D đến P (cột 4-16)
            var subtotalColorRange = ws.Range(currentRow, 4, currentRow, 16);
            subtotalColorRange.Style.Font.Bold = true;
            subtotalColorRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

            // Add borders for subtotal row
            var subtotalBorderRange = ws.Range(currentRow, 1, currentRow, 16);
            subtotalBorderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            subtotalBorderRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            currentRow++;
            int dataStartRow = currentRow;

            // Write each item in the group AFTER subtotal
            for (int i = 0; i < groupItems.Count; i++)
            {
                var item = groupItems[i];
                var row = ws.Row(currentRow);
                int col = startCol;

                // String fields
                row.Cell(col++).Value = item.BuyerType;
                row.Cell(col++).Value = item.BuyerShortName;
                row.Cell(col++).Value = item.MaterialType;

                int firstMonthCol = col;

                // Decimal month fields
                row.Cell(col++).Value = item.January ?? 0;
                row.Cell(col++).Value = item.February ?? 0;
                row.Cell(col++).Value = item.March ?? 0;
                row.Cell(col++).Value = item.April ?? 0;
                row.Cell(col++).Value = item.May ?? 0;
                row.Cell(col++).Value = item.June ?? 0;
                row.Cell(col++).Value = item.July ?? 0;
                row.Cell(col++).Value = item.August ?? 0;
                row.Cell(col++).Value = item.September ?? 0;
                row.Cell(col++).Value = item.October ?? 0;
                row.Cell(col++).Value = item.November ?? 0;
                row.Cell(col++).Value = item.December ?? 0;
                int lastMonthCol = col - 1;

                // Total column (Excel SUM formula)
                row.Cell(col).FormulaA1 = $"SUM({row.Cell(firstMonthCol).Address}:{row.Cell(lastMonthCol).Address})";

                // Add borders for data row
                var dataRowRange = ws.Range(currentRow, 1, currentRow, 16);
                dataRowRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRowRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                currentRow++;
            }

            // Merge BuyerType and BuyerShortName cells for the entire group (including subtotal + data rows)
            int groupEndRow = currentRow - 1;

            // Merge BuyerType column (column 1) - từ subtotal row đến data rows cuối
            var buyerTypeRange = ws.Range(groupStartRow, 1, groupEndRow, 1);
            buyerTypeRange.Merge();
            buyerTypeRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            // Merge BuyerShortName column (column 2) - từ subtotal row đến data rows cuối
            var buyerShortNameRange = ws.Range(groupStartRow, 2, groupEndRow, 2);
            buyerShortNameRange.Merge();
            buyerShortNameRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        }

        // 9. Save workbook to a new stream
        var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
        outputStream.Position = 0;

        // 9. Return the file to the client
        return new RemoteStreamContent(
            outputStream,
            "DPOReport.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    // Helper method to get month value by index
    private decimal GetMonthValue(dynamic item, int monthIndex)
    {
        return monthIndex switch
        {
            0 => item.January ?? 0,
            1 => item.February ?? 0,
            2 => item.March ?? 0,
            3 => item.April ?? 0,
            4 => item.May ?? 0,
            5 => item.June ?? 0,
            6 => item.July ?? 0,
            7 => item.August ?? 0,
            8 => item.September ?? 0,
            9 => item.October ?? 0,
            10 => item.November ?? 0,
            11 => item.December ?? 0,
            _ => 0
        };
    }

    public async Task<List<DPODataReportDto>> GetDataDPOReportAsync(GetDPOReportInputDto input)
    {
        var fullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        var items = await _dPORepository.GetListDPOReportAsync(input.BuyerTypeId, input.BuyerId, input.FromDate, input.ToDate, fullBuyerAccess, _currentUser.Username ?? "");
        return ObjectMapper.Map<List<DPOReportDto>, List<DPODataReportDto>>(items);

    }

    public async Task<List<DPOProcessingReportDto>> GetDataDPOProcessingReportAsync(GetDPOReportInputDto input)
    {
        var fullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        var items = await _dPORepository.GetListDPOProcessingReportAsync(input.BuyerTypeId, input.BuyerId, input.FromDate, input.ToDate, fullBuyerAccess, _currentUser.Username ?? "");
        return ObjectMapper.Map<List<DPOProcessingReport>, List<DPOProcessingReportDto>>(items);
    }

    public async Task<List<DpoGkrAllocationDto>> GetGkrAllocationsAsync(Guid dpoId)
    {
        var gkrs = await _dPORepository.GetListAsync(x => x.LinkedDpoId == dpoId);
        var result = ObjectMapper.Map<List<DPO>, List<DpoGkrAllocationDto>>(gkrs);

        return result;
    }

    public async Task<List<DpoGkrAllocationDto>> GetAvailableGkrsForAllocationAsync(Guid dpoId)
    {
        var dpo = await _dPORepository.GetAsync(dpoId);
        List<DPO> gkrs = await GetListAvailableGKRsAsync(dpo);

        var gkrDetails = await _dPODetailRepository.GetListAsync(x => gkrs.Select(g => g.Id).Contains(x.DPOId));

        var result = ObjectMapper.Map<List<DPO>, List<DpoGkrAllocationDto>>(gkrs);

        // Populate details for each GKR
        var dpoDetailsMap = dpo.Details.ToDictionary(dd => (dd.GolfaCode.ToUpperInvariant(), dd.UnitPrice), dd => dd);

        foreach (var gkr in result)
        {
            gkr.AllocationDetails = gkrDetails
                .Where(x =>
                    x.DPOId == gkr.Id
                )
                .Select(x =>
                {
                    int keptQty = (x.Qty ?? 0) - (x.NeedDelivery ?? 0);
                    int dpoOrderQty = 0;
                    int takeQty = 0;

                    if (dpoDetailsMap.TryGetValue((x.GolfaCode.ToUpperInvariant(), x.UnitPrice ?? 0), out var dpod))
                    {
                        dpoOrderQty = dpod.NeedDelivery ?? 0;

                        // Determine takeQty based on keptQty and dpoOrderQty
                        // e.g., if keptQty = 10, dpoOrderQty = 15 => takeQty = 10; if keptQty = 20, dpoOrderQty = 15 => takeQty = 15
                        takeQty = Math.Min(dpoOrderQty, keptQty);
                    }

                    return new DpoGkrAllocationDetailDto
                    {
                        Id = x.Id,
                        GolfaCode = x.GolfaCode,
                        Model = x.Model!,
                        GkrQty = x.Qty ?? 0,
                        KeptQty = keptQty,
                        OrderQty = dpoOrderQty,
                        TakeQty = takeQty,
                        ReleaseQty = Math.Max(0, keptQty - takeQty) // max to avoid negative
                    };
                });
        }

        return result;
    }

    private async Task<List<DPO>> GetListAvailableGKRsAsync(DPO dpo)
    {
        var invalidGkrStatuses = new List<string> {
            QuoteFlowStatuses.Closed,
            QuoteFlowStatuses.Cancelled,
            QuoteFlowStatuses.GKR.Submitted,
            QuoteFlowStatuses.Rejected
        };
        var gkrs = await _dPORepository.GetListAsync(x =>
            (x.BuyerId == null || x.BuyerId == dpo.BuyerId)
            && x.MaterialType == dpo.MaterialType
            && !invalidGkrStatuses.Contains(x.Status!)
            && x.LinkedDpoId == null
            && x.DPOType == DPOTypes.GKR
            && x.ExpirationDate! >= DateTime.Now.Date
        );
        return gkrs;
    }

    [UnitOfWork(IsDisabled = true)]
    public async Task AllocateGkrToDpoAsync(DPOAllocateGKRDto input)
    {
        var gkr = await _dPORepository.GetAsync(input.GKRId);
        if (gkr.LinkedDpoId != null)
        {
            throw new UserFriendlyException("This GKR is already linked to another DPO.");
        }

        if (gkr.ExpirationDate! < DateTime.Now.Date)
        {
            throw new UserFriendlyException("This GKR has already expired and cannot be allocated.");
        }

        var dpo = await _dPORepository.GetAsync(input.DPOId);
        var validationResult = input.ValidateConcurrencyStamp(dpo.ConcurrencyStamp);
        if (validationResult.Any())
        {
            throw new AbpValidationException(validationErrors: validationResult.ToList());
        }

        var errorMsg = await _dPORepository.AllocateGkrToDpoAsync(input.DPOId, input.GKRId, input.Note, _currentUser.Username ?? "", _currentUser.FullName ?? "");
        if (!string.IsNullOrEmpty(errorMsg))
        {
            throw new UserFriendlyException(errorMsg);
        }
    }


    public virtual async Task<PagedResultDto<MessageDto>> GetListMessagesAsync(Guid dpoId, GetDPOMessagesInput input)
    {
        var totalCount = await _dPORepository.GetCountMessagesAsync(dpoId);
        var items = await _dPORepository.GetListMessagesAsync(dpoId, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<MessageDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<DPOMessage>, List<MessageDto>>(items)
        };
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
    public virtual async Task<MessageDto> SendMessageAsync(Guid dpoId, MessageCreateDto input)
    {
        using var uow = UnitOfWorkManager.Begin(new(true), requiresNew: true);
        var dpo = await _dPORepository.GetWithDetailsAsync(dpoId, x => x.Messages);
        var currentUserFullName = _currentUser.FullName ?? "N/A";
        var currentUserUsername = _currentUser.Username ?? "N/A";

        var comment = input.Comment?.Trim() ?? string.Empty;
        var inputEmails = input.SendToEmails?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? [];

        // Get sales pic emails for the buyer of this DPO
        var filterParams = new SalesAssignments.ParameterObjects.SalesAssignmentFilterParams { BuyerId = dpo.BuyerId };
        var salesAssignments = await _salesAssignmentRepository.GetListAsync(filterParams);
        var salePicEmails = salesAssignments.Select(sa => sa.SaleUserName).ToList();
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
                _logger.LogWarning("[DPO.SendMessageAsync] Invalid emails provided: {emails}", string.Join(", ", invalidEmails));
                throw new UserFriendlyException($"Some emails are invalid. Please check and try again. Invalid emails: {string.Join(", ", invalidEmails)}");
            }

            // All emails are valid, send to input list
            emailsToSend = validEmails;
        }
        else
        {
            // No input emails provided
            if (isCurrentUserSalePic)
            {
                _logger.LogWarning("[DPO.SendMessageAsync] Current user is sale pic but no recipients specified, throwing errors.");
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
            throw new UserFriendlyException($"Some emails are invalid. Please check and try again. Invalid emails: {string.Join(", ", finalInvalidEmails)}");
        }

        var message = new DPOMessage(
            GuidGenerator.Create(),
            dpoId,
            currentUserUsername,
            currentUserFullName,
            finalValidEmails,
            comment);

        dpo.AddMessage(message);
        await _dPORepository.UpdateAsync(dpo, autoSave: true);

        var emailModel = new DPODiscussionEmailModel(
            new DPOEmailInfo(
                dpo.Status ?? "",
                dpo.CreatorName!,
                dpo.DPONo!,
                dpo.MaterialType!,
                dpo.BuyerShortName!,
                dpo.Remark ?? "",
                dpo.OrderDate ?? DateTime.Now,
                dpo.TotalAmount


            ),
            inputEmails.Count > 1 ? $"Dear All" : $"Dear Mr/Ms {GetDisplayNameFromEmail(inputEmails.First())}",
            _currentUser.FullName,
            message.Comment ?? string.Empty,
            DateTime.Now
        );

        var subject = EmailSubjectHelper.Generate(
           currentUserUsername,
           HistoryActions.Discussion.MessageSent,
           NameHelper.ConvertClassNameToEntityName(nameof(PriceOffer)),
           EmailRecipientRole.NormalRecipient,
           dpo.DPONo
       );

        var emailArgs = new SendEmailJobArgs(
            inputEmails,
            subject,
            emailModel
        );

        await _emailJobScheduler.ScheduleEmailAsync(emailArgs);

        await uow.CompleteAsync();

        return ObjectMapper.Map<DPOMessage, MessageDto>(message);
    }

    private async Task<(List<string> ValidEmails, List<string> InvalidEmails)> NormalizeEmailListAsync(List<string> emails)
    {
        var allUsers = await _identityUserRepository.GetListAsync();

        // Create a lookup dictionary for O(1) email lookups for internal users
        var userLookup = allUsers
            .Where(u => !string.IsNullOrEmpty(u.UserName))
            .ToDictionary(
                u => u.UserName!.Trim().ToLowerInvariant(),
                u => u.UserName!,
                StringComparer.OrdinalIgnoreCase);

        var validEmails = new List<string>(emails.Count);
        var invalidEmails = new List<string>();

        // Email regex pattern for validation
        var emailRegex = new System.Text.RegularExpressions.Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            System.Text.RegularExpressions.RegexOptions.Compiled);

        foreach (var email in emails)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();

            // First, try to find internal user (normalize if possible)
            if (userLookup.TryGetValue(normalizedEmail, out var actualUserName))
            {
                validEmails.Add(actualUserName);
            }
            // If not internal user, validate as external email using regex
            else if (emailRegex.IsMatch(normalizedEmail))
            {
                validEmails.Add(email.Trim()); // Keep original casing for external emails
            }
            else
            {
                _logger.LogWarning("Email {email} is not a valid email format.", email);
                invalidEmails.Add(email);
            }
        }

        return (validEmails, invalidEmails);
    }

    public virtual async Task<IRemoteStreamContent> ExportDataAsExcelAsync(DPOExportDataInputDto input)
    {
        // Check full buyer permission
        var hasFullBuyerPermission = await _buyerAccessService.HasFullBuyerAccessAsync();
        var currentUserName = _currentUser.Username ?? CurrentUser.UserName ?? string.Empty;

        // Get data from stored procedure
        var exportData = await _dPORepository.GetExportDataAsync(
            input.DPONo,
            input.Status,
            input.GolfaCode,
            input.Model,
            input.PONo,
            input.CustomerName,
            input.FromDate,
            input.ToDate,
            input.BuyerTypeId,
            input.BuyerId,
            input.MaterialType,
            input.SupplierCode,
            input.SPOCode,
            input.TaxCode,
            input.MaterialGroup,
            currentUserName,
            hasFullBuyerPermission
        );

        // Convert to DTOs
        var exportDtos = ObjectMapper.Map<List<DPOExportDataModel>, List<DPOExportDataDto>>(exportData);

        // Create Excel file with styling and striped coloring
        var excelBytes = await CreateStyledExcelFileAsync(exportDtos);

        var memoryStream = new MemoryStream(excelBytes);
        return new RemoteStreamContent(
            memoryStream,
            $"DPO_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    private async Task<byte[]> CreateStyledExcelFileAsync(List<DPOExportDataDto> data)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("DPO Export Data");

        // 1. Header definitions
        var columnConfigs = new List<(string Header, string Color)>
    {
        ("DPONo", "#32cd32"),
        ("Golfa Code", "#32cd32"),
        ("Model", "#32cd32"),
        ("Spec1", "#32cd32"),
        ("Spec2", "#32cd32"),
        ("Material Type", "#32cd32"),
        ("Material Group", "#32cd32"),
        ("Qty", "#32cd32"),
        ("Price", "#32cd32"),
        ("Amount", "#32cd32"),
        ("Distributor", "#32cd32"),
        ("Order Date", "#32cd32"),
        ("Requested ETA", "#32cd32"),
        ("Project Code", "#32cd32"),
        ("Project Name", "#32cd32"),
        ("Customer", "#32cd32"),
        ("Status", "#32cd32"),

        ("SONo", "#6495ed"),
        ("SO Date", "#6495ed"),
        ("SO Qty", "#6495ed"),

        ("PONo", "#40e0d0"),
        ("PODate", "#40e0d0"),
        ("PO Qty", "#40e0d0"),
        ("Lockshipment Qty", "#40e0d0"),
        ("Lockshipment Qty Imported", "#40e0d0"),
        ("Machine Number", "#40e0d0"),
        ("STCReply", "#40e0d0"),

        ("Invoice No", "#fcc1cb"),
        ("Qty Allocation", "#fcc1cb"),
        ("CDNo", "#fcc1cb"),
        ("Bill No", "#fcc1cb"),
        ("ETD", "#fcc1cb"),
        ("ETA", "#fcc1cb"),
        ("Stock Date", "#fcc1cb"),

        ("SAP Code", "#ffa500"),
        ("POSAPNo", "#ffa500"),
        ("SOSAPNo", "#ffa500"),
        ("DOSAPNo", "#ffa500"),
        ("SAP Billling No", "#ffa500"),
        ("SAP Invoice No", "#ffa500"),
        ("SAP Invoice Date", "#ffa500"),
        ("Remark", "#ffa500")
    };

        int totalCols = columnConfigs.Count;

        // 2. Headers
        ws.Cell(1, 1).InsertData(new[] { columnConfigs.Select(c => c.Header).ToArray() });

        // Apply header style
        for (int i = 0; i < columnConfigs.Count; i++)
        {
            var cell = ws.Cell(1, i + 1);
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml(columnConfigs[i].Color);
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        // 3. Build data rows
        var rows = data.Select(d => new object[]
        {
            d.DPONo ?? "",
            d.GolfaCode ?? "",
            d.Model ?? "",
            d.Spec1 ?? "",
            d.Spec2 ?? "",
            d.MaterialType ?? "",
            d.MaterialGroup ?? "",
            d.Qty,
            d.Price,
            d.Amount,
            d.Distributor ?? "",
            d.OrderDate,
            d.RequestedETA,
            d.ProjectCode ?? "",
            d.ProjectName ?? "",
            d.Customer ?? "",
            d.Status ?? "",
            d.SONo ?? "",
            d.SODate,
            d.SOQty,
            d.PONo ?? "",
            d.PODate,
            d.POQty,
            d.LockshipmentQty,
            d.LockshipmentQtyImported,
            d.MachineNumber ?? "",
            d.STCReply ?? "",
            d.InvoiceNo ?? "",
            d.QtyAllocation,
            d.CDNo ?? "",
            d.BillNo ?? "",
            d.ETD,
            d.ETA,
            d.StockDate,
            d.SAPCode ?? "",
            d.POSAPNo ?? "",
            d.SAPSONo ?? "",
            d.DOSAPNo ?? "",
            d.SAPBillingNo ?? "",
            d.SAPInvoiceNo ?? "",
            d.SAPInvoiceDate,
            d.Remark ?? ""
        }).ToList();


        ws.Cell(2, 1).InsertData(rows);

        int lastRow = rows.Count + 1;

        var grouped = data.GroupBy(x => x.DPONo).ToList();
        int rowIdx = 2;
        bool isGray = true;

        foreach (var g in grouped)
        {
            var rowColor = isGray ? XLColor.LightGray : XLColor.White;
            int groupSize = g.Count();

            var range = ws.Range(rowIdx, 1, rowIdx + groupSize - 1, totalCols);
            range.Style.Fill.BackgroundColor = rowColor;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            rowIdx += groupSize;
            isGray = !isGray;
        }

        // 5. Apply formatting
        ApplyColumnFormatting(ws, totalCols, lastRow);

        // 6. Export
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private void ApplyColumnFormatting(IXLWorksheet worksheet, int totalColumns, int lastRow)
    {
        // Set all columns to default width 20
        for (int col = 1; col <= totalColumns; col++)
        {
            worksheet.Column(col).Width = 20;
        }

        // Define date columns (Order Date=12, Requested ETA=13, SO Date=18, PODate=21, ETD=31, ETA=32, Stock Date=33, SAP Invoice Date=40)
        var dateColumns = new[] { 12, 13, 19, 22, 32, 33, 34, 41 };

        // Define qty columns (Qty=8, SO Qty=19, PO Qty=22, Lockshipment Qty=23, Lockshipment Qty Imported=24, Qty Allocation=28)
        var qtyColumns = new[] { 8, 20, 23, 24, 25, 29 };

        // Define number columns (Price=9, Amount=10)
        var numberColumns = new[] { 9, 10 };

        // Set column widths
        // Date columns: width 11
        foreach (var col in dateColumns)
        {
            if (col <= totalColumns)
            {
                worksheet.Column(col).Width = 11;
            }
        }

        // Qty columns: width 6
        foreach (var col in qtyColumns)
        {
            if (col <= totalColumns)
            {
                worksheet.Column(col).Width = 6;
            }
        }

        // Number columns: width 16
        foreach (var col in numberColumns)
        {
            if (col <= totalColumns)
            {
                worksheet.Column(col).Width = 16;
            }
        }

        // Apply date formatting (dd/MM/yyyy)
        foreach (var col in dateColumns)
        {
            if (col <= totalColumns)
            {
                worksheet.Column(col).Style.NumberFormat.Format = "dd/MM/yyyy";
            }
        }

        // Apply number formatting (thousand separator, no decimal places) to both qty and number columns
        var allNumberColumns = qtyColumns.Concat(numberColumns).ToArray();
        foreach (var col in allNumberColumns)
        {
            if (col <= totalColumns)
            {
                worksheet.Column(col).Style.NumberFormat.Format = "#,##0";
            }
        }

        // Apply formatting to data range only (excluding headers)
        if (lastRow > 1)
        {
            var dataRange = worksheet.Range(2, 1, lastRow, totalColumns);

            // Apply date formatting to specific columns
            foreach (var col in dateColumns)
            {
                if (col <= totalColumns)
                {
                    var dateRange = dataRange.Column(col);
                    dateRange.Style.NumberFormat.Format = "dd/MM/yyyy";
                }
            }

            // Apply number formatting to specific columns
            foreach (var col in allNumberColumns)
            {
                if (col <= totalColumns)
                {
                    var numberRange = dataRange.Column(col);
                    numberRange.Style.NumberFormat.Format = "#,##0";
                }
            }
        }
    }

    // DPO Detail

    public virtual async Task<PagedResultDto<DPODetailDto>> GetListDPODetailAsync(GetDPODetailsInput input)
    {
        var totalCount = await _dPODetailRepository.GetCountAsync(input.FilterText, input.DPOId, input.Status, input.GolfaCode, input.Model, input.Spec1, input.Spec2, input.QtyMin, input.QtyMax, input.UnitPriceMin, input.UnitPriceMax, input.AmountMin, input.AmountMax, input.RequestedETAMin, input.RequestedETAMax, input.SPOId, input.SPOCode, input.CustomerTaxCode, input.CustomerName, input.LockStockMin, input.LockStockMax, input.LockStockSOMin, input.LockStockSOMax, input.LockShipmentMin, input.LockShipmentMax, input.DeliveredMin, input.DeliveredMax, input.NeedDeliveryMin, input.NeedDeliveryMax, input.Note);
        var items = await _dPODetailRepository.GetListAsync(input.FilterText, input.DPOId, input.Status, input.GolfaCode, input.Model, input.Spec1, input.Spec2, input.QtyMin, input.QtyMax, input.UnitPriceMin, input.UnitPriceMax, input.AmountMin, input.AmountMax, input.RequestedETAMin, input.RequestedETAMax, input.SPOId, input.SPOCode, input.CustomerTaxCode, input.CustomerName, input.LockStockMin, input.LockStockMax, input.LockStockSOMin, input.LockStockSOMax, input.LockShipmentMin, input.LockShipmentMax, input.DeliveredMin, input.DeliveredMax, input.NeedDeliveryMin, input.NeedDeliveryMax, input.Note, input.Sorting, input.MaxResultCount, input.SkipCount);
        var itemDtos = ObjectMapper.Map<List<DPODetail>, List<DPODetailDto>>(items);
        var allGolfaCodes = itemDtos.Select(x => x.GolfaCode).Distinct().ToList();

        var damagedOrFOCStock = await _stockCategoryRepository.GetListAsync(x =>
            x.DamagedStock == true || x.FOC == true
        );
        var materialStocks = await _materialStockRepository.GetListAsync(x =>
            allGolfaCodes.Contains(x.GolfaCode) && !damagedOrFOCStock.Select(stock => stock.Id).Contains(x.StockCategoryId)
        );
        var materialLockShipments = await _materialStockLockShipmentRepository.GetListAsync(x =>
            allGolfaCodes.Contains(x.GolfaCode)
        );
        var materialNotActive = await _materialRepository.GetListAsync(
            x => allGolfaCodes.Contains(x.GolfaCode) && x.MaterialStatus != MaterialStatuses.Active
        );

        var materialStatusLookup = materialNotActive
            .GroupBy(x => x.GolfaCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.First().MaterialStatus,
                StringComparer.OrdinalIgnoreCase
            );

        // Get all materials whose GolfaCode is in the list of golfas
        List<string> golfas = itemDtos
           .Select(x => x.GolfaCode)
           .Distinct()
           .ToList();
        var materials = await _materialRepository.GetListAsync(
            x => golfas.Contains(x.GolfaCode)
        );



        foreach (var dto in itemDtos)
        {

            //get Spec1, Spec2 into Materils
            var material = materials.FirstOrDefault(m => m.GolfaCode == dto.GolfaCode);
            if (material != null)
            {
                dto.Spec1 = material.Spec1;
                dto.Spec2 = material.Spec2;
            }
            //end
            dto.AvailableStockQty = materialStocks
                .Where(x => x.GolfaCode.Equals(dto.GolfaCode, StringComparison.OrdinalIgnoreCase))
                .Sum(x => x.Available_Qty) ?? 0;

            dto.OnOrderStockAvailable = materialLockShipments
                .Where(x => x.GolfaCode.Equals(dto.GolfaCode, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault()?.StockOnOrder ?? 0;

            if (materialStatusLookup.TryGetValue(dto.GolfaCode, out var status))
                dto.MaterialStatus = status;
            else
                dto.MaterialStatus = MaterialStatuses.Active;
        }



        return new PagedResultDto<DPODetailDto>
        {
            TotalCount = totalCount,
            Items = itemDtos
        };
    }

    public virtual async Task<DPODetailDto> GetDPODetailAsync(Guid id)
    {
        var item = await _dPODetailRepository.GetAsync(id);
        var itemDto = ObjectMapper.Map<DPODetail, DPODetailDto>(item);

        try
        {
            var materialStock = await _materialStockRepository.GetAsync(x => x.GolfaCode == itemDto.GolfaCode);
            var materialLockShipment = await _materialStockLockShipmentRepository.GetAsync(x => x.GolfaCode == itemDto.GolfaCode);

            itemDto.AvailableStockQty = materialStock?.Available_Qty ?? 0;
            itemDto.OnOrderStockAvailable = materialLockShipment?.StockOnOrder ?? 0;
        }
        catch (EntityNotFoundException ex)
        {
            // logs out and ignore if material stock or lock shipment not found
            if (ex.EntityType != typeof(MaterialStock) && ex.EntityType != typeof(MaterialStockLockShipment))
            {
                throw;
            }

            if (ex.EntityType == typeof(MaterialStock))
            {
                Logger.LogWarning($"Material stock not found for GolfaCode: {itemDto.GolfaCode}");
                itemDto.AvailableStockQty = 0;
            }

            if (ex.EntityType == typeof(MaterialStockLockShipment))
            {
                Logger.LogWarning($"Material lock shipment not found for GolfaCode: {itemDto.GolfaCode}");
                itemDto.OnOrderStockAvailable = 0;
            }
        }

        return itemDto;
    }

    public virtual async Task DeleteDPODetailAsync(Guid id)
    {
        await _dPODetailRepository.DeleteAsync(id);
    }

    public virtual async Task<DPODetailDto> CreateDPODetailAsync(DPODetailCreateDto input)
    {
        var createParams = ObjectMapper.Map<DPODetailCreateDto, DPODetailCreateParams>(input);
        var dPODetail = await _dPODetailManager.CreateAsync(
        createParams
        );

        return ObjectMapper.Map<DPODetail, DPODetailDto>(dPODetail);
    }

    public virtual async Task<DPODetailDto> UpdateAsync(Guid id, DPODetailUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<DPODetailUpdateDto, DPODetailUpdateParams>(input);
        var dPODetail = await _dPODetailManager.UpdateAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<DPODetail, DPODetailDto>(dPODetail);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(DPODetailExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenDPODetailCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var items = await _dPODetailRepository.GetListAsync(input.FilterText, input.DPOId, input.Status, input.GolfaCode, input.Model, input.Spec1, input.Spec2, input.QtyMin, input.QtyMax, input.UnitPriceMin, input.UnitPriceMax, input.AmountMin, input.AmountMax, input.RequestedETAMin, input.RequestedETAMax, input.SPOId, input.SPOCode, input.CustomerTaxCode, input.CustomerName, input.LockStockMin, input.LockStockMax, input.LockStockSOMin, input.LockStockSOMax, input.LockShipmentMin, input.LockShipmentMax, input.DeliveredMin, input.DeliveredMax, input.NeedDeliveryMin, input.NeedDeliveryMax, input.Note);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<DPODetail>, List<DPODetailExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "DPODetails.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<Shared.DownloadTokenResultDto> GetDownloadTokenDPODtailAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenDPODetailCache.SetAsync(
            token,
            new DPODetailDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }

    public virtual async Task LockStockAsync(Guid id, DPODetailLockStockDto input)
    {
        await _dPODetailRepository.LockStockAsync(
            id,
            input.GolfaCode,
            input.StockCategoryId,
            input.LockQty,
            _currentUser.Username ?? string.Empty,
            _currentUser.FullName ?? string.Empty,
            input.Note
        );
    }

    public async Task<List<SaleOrderListModalDPODto>> GetListSaleOrderModalDPOAsync(Guid dpoDetailId)
    {
        var items = await _dPODetailRepository.GetSaleOrderListModalDPOAsync(dpoDetailId);

        return ObjectMapper.Map<List<SaleOrderListModalDPO>, List<SaleOrderListModalDPODto>>(items);
    }

    public async Task<List<SaleOrderListModalDeliveryDto>> GetListSaleOrderModalDeliveryAsync(Guid dpoDetailId)
    {
        var items = await _dPODetailRepository.GetSaleOrderListModalDeliveryAsync(dpoDetailId);

        return ObjectMapper.Map<List<SaleOrderListModalDelivery>, List<SaleOrderListModalDeliveryDto>>(items);
    }

    private async Task CheckBypassConfirmationAsync(DPO dpo)
    {
        var dpoDetails = dpo.Details;
        var allGolfaCodes = dpoDetails.Select(x => x.GolfaCode).Distinct().ToList();

        var damagedOrFOCStock = await _stockCategoryRepository.GetListAsync(x =>
            x.DamagedStock == true || x.FOC == true
        );
        var materialStocks = await _materialStockRepository.GetListAsync(x =>
            allGolfaCodes.Contains(x.GolfaCode) && !damagedOrFOCStock.Select(stock => stock.Id).Contains(x.StockCategoryId)
        );

        var availableGKRs = await GetListAvailableGKRsAsync(dpo);

        if (materialStocks.All(x => (x.Available_Qty ?? 0) == 0) && availableGKRs.Count == 0)
        {
            // bypass lock stock
            // Check if should bypass lock on order stock as well
            await CheckBypassLockOnOrderStockAsync(dpo);
        }
        else
        {
            dpo.Status = QuoteFlowStatuses.DPO.Confirmed;
        }
    }

    private async Task CheckBypassLockOnOrderStockAsync(DPO dpo)
    {
        var allGolfaCodes = dpo.Details.Select(x => x.GolfaCode).Distinct().ToList();

        var materialLockShipments = await _materialStockLockShipmentRepository.GetListAsync(x =>
            allGolfaCodes.Contains(x.GolfaCode)
        );
        if (materialLockShipments.All(x => (x.StockOnOrder ?? 0) == 0)
            || dpo.Details.All(x => x.NeedDelivery == 0))
        {
            // bypass lock on order stock
            dpo.Status = QuoteFlowStatuses.InProgress;

        }
        else
        {
            dpo.Status = QuoteFlowStatuses.DPO.LockedStock;
        }
    }

    /// <summary>
    /// Validates price mismatches for DPO items without SPO Code.
    /// Throws a custom exception with formatted message if mismatches are found.
    /// </summary>
    private async Task ValidatePriceMismatchesAsync(ExcelValidationResult<ImportDPODetailDto> dpoDetails, Guid? buyerId, string? materialType)
    {
        if (dpoDetails?.ListData?.Any() != true || !buyerId.HasValue)
        {
            return; // Nothing to validate
        }

        // Get rows without SPO Code for price validation
        var rowsWithoutSPOCode = dpoDetails.ListData
            .Where(x => string.IsNullOrWhiteSpace(x.RowData.SPOCode) &&
                       !string.IsNullOrWhiteSpace(x.RowData.GolfaCode) &&
                       !string.IsNullOrWhiteSpace(x.RowData.Model) &&
                       x.RowData.UnitPrice.HasValue)
            .ToList();

        if (!rowsWithoutSPOCode.Any())
        {
            return; // No rows to validate
        }

        // Get buyer's applied price setting
        int appliedPrice = 0;
        try
        {
            var buyer = await _buyerRepository.GetAsync(buyerId.Value);
            if (buyer?.AppliedPrice.HasValue == true)
            {
                appliedPrice = buyer.AppliedPrice.Value;
            }
        }
        catch (Exception)
        {
            throw new UserFriendlyException("This is not a valid buyer. Please select a valid buyer.");
        }

        // Get material data for price validation
        var materialLookups = rowsWithoutSPOCode
            .Select(x => new { GolfaCode = x.RowData.GolfaCode!, Model = x.RowData.Model! })
            .Distinct()
            .ToList();

        var materials = await _materialRepository.GetListAsync(new(),
            x => new MaterialPriceProjection
            {
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                StandardPrice = x.Standard_Price,
                SellingPrice1 = x.SellingPrice1
            });

        var materialsByKey = new Dictionary<string, MaterialPriceProjection>();
        foreach (var lookup in materialLookups)
        {
            var material = materials.FirstOrDefault(m =>
                string.Equals(m.GolfaCode, lookup.GolfaCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Model, lookup.Model, StringComparison.OrdinalIgnoreCase));
            if (material != null)
            {
                var key = $"{lookup.GolfaCode}_{lookup.Model}";
                materialsByKey[key] = material;
            }
        }

        // Collect rows with price mismatches
        var mismatchRows = new List<int>();
        //var deactiveMaterialRows = new List<int>();
        foreach (var row in rowsWithoutSPOCode)
        {
            var materialKey = $"{row.RowData.GolfaCode}_{row.RowData.Model}";
            if (materialsByKey.TryGetValue(materialKey, out var material))
            {
                // Determine expected price based on buyer's applied price setting
                var expectedPrice = material.StandardPrice;
                switch (appliedPrice)
                {
                    case 0:
                        expectedPrice = material.StandardPrice;
                        break;
                    case 1:
                        expectedPrice = material.SellingPrice1 ?? material.StandardPrice;
                        break;
                }

                // Check for price mismatch
                if (row.RowData.UnitPrice != expectedPrice)
                {
                    mismatchRows.Add(row.RowData.RowNo ?? row.RowIndex);
                }
            }
        }

        // If price mismatches found, throw custom exception
        if (mismatchRows.Count != 0)
        {
            var rowNumbers = string.Join(",", mismatchRows.OrderBy(x => x));

            throw new BusinessException(
                QuoteFlowDomainErrorCodes.DPO.ImportPriceMismatchWarning
            ).WithData("rowNumbers", rowNumbers);
        }
    }


    private DPOApprovalHistory SetDPOApproval(Guid DPOId, string action, string? note = null)
    {
        var actionDate = DateTime.Now;
        return new DPOApprovalHistory(
            GuidGenerator.Create(),
            DPOId,
            new(
                "FA Team",
                "FA Team",
                _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
                _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
                action,
                actionDate,
                note)
        );
    }

    public virtual async Task UpdateDPODetailAsync(Guid dpoDetailId, int qty, string? confirmNote, string? note)
    {
        var errorMes = await _dPORepository.UpdateDPODetailAsync(dpoDetailId, qty, confirmNote, note, _currentUser.Username, _currentUser.FullName);
        if (errorMes != null)
        {
            throw new UserFriendlyException(errorMes);
        }
    }

    /// <summary>
    /// Batch auto-unlock stock for multiple DPO details with Server-Sent Events progress tracking
    /// </summary>
    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockStock)]
    [UnitOfWork(IsDisabled = true)]
    public virtual async IAsyncEnumerable<BatchUnlockProgressEventDto> BatchAutoUnlockStockAsync(BatchAutoUnlockStockDto input)
    {
        // Validation: At least one detail ID required
        if (input.DpoDetailIds == null || input.DpoDetailIds.Count == 0)
        {
            throw new UserFriendlyException("At least one DPO detail ID is required");
        }

        // Step 1: Validate all details belong to same DPO
        var firstDetail = await _dPODetailRepository.GetAsync(input.DpoDetailIds.First());
        var dpoId = firstDetail.DPOId;

        // Check if all details belong to the same DPO
        var allDetails = await _dPODetailRepository.GetListAsync(x => input.DpoDetailIds.Contains(x.Id));
        if (allDetails.Any(d => d.DPOId != dpoId))
        {
            throw new UserFriendlyException("All selected details must belong to the same DPO");
        }

        // Step 2: Validate DPO status
        var dpo = await _dPORepository.GetAsync(dpoId);
        var blockedStatuses = new HashSet<string>
        {
            QuoteFlowStatuses.Closed,
            QuoteFlowStatuses.Cancelled,
            QuoteFlowStatuses.Rejected,
            QuoteFlowStatuses.DPO.LockedStock
        };

        if (blockedStatuses.Contains(dpo.Status ?? string.Empty))
        {
            throw new UserFriendlyException($"Cannot unlock stock for this DPO. DPO status is {dpo.Status!.ToUpperFirstChar()}");
        }

        // Emit started event
        yield return new BatchUnlockProgressEventDto
        {
            EventType = "started",
            DpoId = dpoId,
            Total = input.DpoDetailIds.Count,
            Current = 0
        };

        // Step 3: Process each detail
        int current = 0;
        int succeeded = 0;
        int failed = 0;
        int totalUnlocked = 0;
        int totalSkipped = 0;

        foreach (var detailId in input.DpoDetailIds)
        {
            current++;

            // Emit processing event
            yield return new BatchUnlockProgressEventDto
            {
                EventType = "progress",
                DpoDetailId = detailId,
                Status = "processing",
                Current = current,
                Total = input.DpoDetailIds.Count
            };

            // Process this detail and get result
            var result = await UnlockDetailStockAsync(detailId);

            // Update counters
            if (result.Success)
            {
                succeeded++;
                totalUnlocked += result.UnlockedCount;
                totalSkipped += result.SkippedCount;

                yield return new BatchUnlockProgressEventDto
                {
                    EventType = "progress",
                    DpoDetailId = detailId,
                    Status = "success",
                    Current = current,
                    Total = input.DpoDetailIds.Count,
                    UnlockedCount = result.UnlockedCount,
                    SkippedCount = result.SkippedCount
                };
            }
            else
            {
                failed++;
                totalSkipped += result.SkippedCount;

                yield return new BatchUnlockProgressEventDto
                {
                    EventType = "progress",
                    DpoDetailId = detailId,
                    Status = "error",
                    Current = current,
                    Total = input.DpoDetailIds.Count,
                    ErrorMessage = result.ErrorMessage
                };
            }
        }

        // Emit complete event
        yield return new BatchUnlockProgressEventDto
        {
            EventType = "complete",
            TotalProcessed = input.DpoDetailIds.Count,
            Succeeded = succeeded,
            Failed = failed,
            TotalUnlocked = totalUnlocked,
            TotalSkipped = totalSkipped,
            Current = current,
            Total = input.DpoDetailIds.Count
        };
    }

    /// <summary>
    /// Helper method to unlock all lock stocks for a single DPO detail
    /// </summary>
    [UnitOfWork(IsDisabled = true)]
    private async Task<UnlockDetailResult> UnlockDetailStockAsync(Guid detailId)
    {
        try
        {
            // Get all unlockable lock stocks for this detail (ReleasedLock = 0)
            var lockStocks = await _materialStockLockStockRepository.GetUnlockableByDetailIdAsync(detailId);

            // Get all lock stocks to count skipped
            var allLockStocks = await _materialStockLockStockRepository.GetListAsync(dPODetailId: detailId);
            int skippedCount = allLockStocks.Count - lockStocks.Count;

            // If no unlockable records, return success with zero unlocked
            if (lockStocks.Count == 0)
            {
                return new UnlockDetailResult
                {
                    Success = true,
                    UnlockedCount = 0,
                    SkippedCount = skippedCount
                };
            }

            // Begin UOW for this detail
            using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
            {
                // Delete each lock stock record
                foreach (var lockStock in lockStocks)
                {
                    var errorMessage = await _dPORepository.DeleteDPOLockStockAsync(
                        detailId,
                        lockStock.Id,
                        _currentUser.Username ?? string.Empty,
                        _currentUser.FullName ?? string.Empty
                    );

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        // Rollback and return error
                        return new UnlockDetailResult
                        {
                            Success = false,
                            ErrorMessage = errorMessage,
                            SkippedCount = skippedCount
                        };
                    }
                }
                var dpoDetail = await _dPODetailRepository.GetAsync(detailId);
                var autoReleaseLockStockHistory = new DPODetailApprovalHistory(
                        GuidGenerator.Create(),
                        dpoDetail.Id,
                        new ApprovalHistoryCreateParams
                        {
                            Action = HistoryActions.DPO.ReleaseLockStock,
                            ActionDate = Clock.Now.AddSeconds(2),
                            ApproverUsername = _currentUser.Username,
                            ApproverFullName = _currentUser.FullName,
                            ApproverRoleCode = "FA Team",
                            ApproverRoleName = "FA Team",
                            EntityType = EntityTypes.DPODetail,
                            IsLastApprovalInCurrentWorkflow = false,
                            Note = $"Auto release lock stock for GolfaCode: {dpoDetail.GolfaCode}"

                        }
                    );

                dpoDetail.RecordAction(autoReleaseLockStockHistory);
                // Commit transaction
                await uow.CompleteAsync();
            }

            // Success
            return new UnlockDetailResult
            {
                Success = true,
                UnlockedCount = lockStocks.Count,
                SkippedCount = skippedCount
            };
        }
        catch (Exception ex)
        {
            return new UnlockDetailResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                SkippedCount = 0
            };
        }
    }
    [Authorize(QuoteFlowPermissions.MovingOrders.DPOs.LockOnOrderStock)]
    [UnitOfWork(IsDisabled = true)]
    public virtual async IAsyncEnumerable<BatchUnlockProgressEventDto> BatchAutoUnlockOnOrderStockAsync(BatchAutoUnlockStockDto input)
    {
        // Validation: At least one detail ID required
        if (input.DpoDetailIds == null || input.DpoDetailIds.Count == 0)
        {
            throw new UserFriendlyException("At least one DPO detail ID is required");
        }

        // Step 1: Validate all details belong to same DPO
        var firstDetail = await _dPODetailRepository.GetAsync(input.DpoDetailIds.First());
        var dpoId = firstDetail.DPOId;

        // Check if all details belong to the same DPO
        var allDetails = await _dPODetailRepository.GetListAsync(x => input.DpoDetailIds.Contains(x.Id));
        if (allDetails.Any(d => d.DPOId != dpoId))
        {
            throw new UserFriendlyException("All selected details must belong to the same DPO");
        }

        // Step 2: Validate DPO status
        var dpo = await _dPORepository.GetAsync(dpoId);
        var blockedStatuses = new HashSet<string>
        {
            QuoteFlowStatuses.Closed,
            QuoteFlowStatuses.Cancelled,
            QuoteFlowStatuses.Rejected,
            QuoteFlowStatuses.DPO.Confirmed,
        };

        if (blockedStatuses.Contains(dpo.Status ?? string.Empty))
        {
            throw new UserFriendlyException($"Cannot unlock on order stock for this DPO. DPO status is {dpo.Status!.ToUpperFirstChar()}");
        }

        // Emit started event
        yield return new BatchUnlockProgressEventDto
        {
            EventType = "started",
            DpoId = dpoId,
            Total = input.DpoDetailIds.Count,
            Current = 0
        };

        // Step 3: Process each detail
        int current = 0;
        int succeeded = 0;
        int failed = 0;
        int totalUnlocked = 0;
        int totalSkipped = 0;

        foreach (var detailId in input.DpoDetailIds)
        {
            current++;



            // Process this detail and get result
            var result = await UnlockDetailOnOrderStockAsync(detailId);

            // Update counters
            if (result.Success)
            {
                succeeded++;
                totalUnlocked += result.UnlockedCount;
                totalSkipped += result.SkippedCount;

                // Emit processing event
                yield return new BatchUnlockProgressEventDto
                {
                    EventType = "progress",
                    DpoDetailId = detailId,
                    Status = "processing",
                    Current = current,
                    Total = input.DpoDetailIds.Count
                };

                yield return new BatchUnlockProgressEventDto
                {
                    EventType = "progress",
                    DpoDetailId = detailId,
                    Status = "success",
                    Current = current,
                    Total = input.DpoDetailIds.Count,
                    UnlockedCount = result.UnlockedCount,
                    SkippedCount = result.SkippedCount
                };
            }
            else
            {
                failed++;
                totalSkipped += result.SkippedCount;

                yield return new BatchUnlockProgressEventDto
                {
                    EventType = "progress",
                    DpoDetailId = detailId,
                    Status = "error",
                    Current = current,
                    Total = input.DpoDetailIds.Count,
                    ErrorMessage = result.ErrorMessage
                };
            }
        }

        // Emit complete event
        yield return new BatchUnlockProgressEventDto
        {
            EventType = "complete",
            TotalProcessed = input.DpoDetailIds.Count,
            Succeeded = succeeded,
            Failed = failed,
            TotalUnlocked = totalUnlocked,
            TotalSkipped = totalSkipped,
            Current = current,
            Total = input.DpoDetailIds.Count
        };
    }
    /// <summary>
    /// Helper method to unlock all lock stocks for a single DPO detail
    /// </summary>
    [UnitOfWork(IsDisabled = true)]
    private async Task<UnlockDetailResult> UnlockDetailOnOrderStockAsync(Guid detailId)
    {
        try
        {
            var poDetails = await _purchaseOrderLockShipmentRepository.GetUnlockableByDetailIdAsync(detailId);
            int skippedCount = 0;

            // If no unlockable records, return success with zero unlocked
            if (poDetails.Count == 0)
            {
                return new UnlockDetailResult
                {
                    Success = true,
                    UnlockedCount = 0,
                    SkippedCount = skippedCount
                };
            }

            // Begin UOW for this detail
            using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
            {
                // Delete each lock stock record
                foreach (var poDetail in poDetails)
                {
                    var errorMessage = await _dPORepository.DeleteLockOnOrderStockAsync(
                        poDetail.PODetailId,
                        detailId,
                        _currentUser.Username ?? string.Empty,
                        _currentUser.FullName ?? string.Empty
                    );

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        // Rollback and return error
                        return new UnlockDetailResult
                        {
                            Success = false,
                            ErrorMessage = errorMessage,
                            SkippedCount = skippedCount
                        };
                    }
                }
                var dpoDetail = await _dPODetailRepository.GetAsync(detailId);
                var autoReleaseLockOnOrderHistory = new DPODetailApprovalHistory(
                        GuidGenerator.Create(),
                        dpoDetail.Id,
                        new ApprovalHistoryCreateParams
                        {
                            Action = HistoryActions.DPO.ReleaseLockOnOrderStock,
                            ActionDate = Clock.Now.AddSeconds(2),
                            ApproverUsername = _currentUser.Username,
                            ApproverFullName = _currentUser.FullName,
                            ApproverRoleCode = "FA Team",
                            ApproverRoleName = "FA Team",
                            EntityType = EntityTypes.DPODetail,
                            IsLastApprovalInCurrentWorkflow = false,
                            Note = $"Auto release lock on order stock for GolfaCode: {dpoDetail.GolfaCode}"

                        }
                    );

                dpoDetail.RecordAction(autoReleaseLockOnOrderHistory);
                // Commit transaction
                await uow.CompleteAsync();
            }

            // Success
            return new UnlockDetailResult
            {
                Success = true,
                UnlockedCount = poDetails.Count,
                SkippedCount = skippedCount
            };
        }
        catch (Exception ex)
        {
            return new UnlockDetailResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                SkippedCount = 0
            };
        }
    }
    /// <summary>
    /// Result of unlocking stock for a single detail
    /// </summary>
    private class UnlockDetailResult
    {
        public bool Success { get; set; }
        public int UnlockedCount { get; set; }
        public int SkippedCount { get; set; }
        public string? ErrorMessage { get; set; }
    }

    private class MaterialPriceProjection
    {
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public decimal StandardPrice { get; set; } = 0m;
        public decimal? SellingPrice1 { get; set; }
    }
}
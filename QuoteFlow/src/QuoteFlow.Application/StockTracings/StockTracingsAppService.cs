using QuoteFlow.Permissions;
using QuoteFlow.Shared.Excels;
using QuoteFlow.StockTracingDetails;
using QuoteFlow.StockTracingDetails.ParameterObjects;
using QuoteFlow.StockTracings.ParameterObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
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
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using Volo.FileManagement.Files;

namespace QuoteFlow.StockTracings;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.StockTracings.Default)]
public class StockTracingsAppService : QuoteFlowAppService, IStockTracingsAppService
{
    protected IDistributedCache<StockTracingDownloadTokenCacheItem, string> _downloadTokenCache;
    protected IStockTracingRepository _stockTracingRepository;
    protected StockTracingManager _stockTracingManager;
    protected StockTracingDetailManager _stockTracingDetailManager;
    protected IExcelImportFactory _excelImportFactory;
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    private readonly FileDescriptorAppService _fileDescriptorAppService;
    protected IStockTracingDetailRepository _stockTracingDetailRepository;
    protected IGuidGenerator _guidGenerator;
    protected IUnitOfWorkManager _unitOfWorkManager;

    public StockTracingsAppService(IStockTracingRepository stockTracingRepository, StockTracingManager stockTracingManager, IDistributedCache<StockTracingDownloadTokenCacheItem, string> downloadTokenCache, IExcelImportFactory excelImportFactory, StockTracingDetailManager stockTracingDetailManager, FileDescriptorAppService fileDescriptorAppService, IRepository<FileDescriptor, Guid> fileDescriptorRepository, IStockTracingDetailRepository stockTracingDetailRepository, IGuidGenerator guidGenerator, IUnitOfWorkManager unitOfWorkManager)
    {
        _downloadTokenCache = downloadTokenCache;
        _stockTracingRepository = stockTracingRepository;
        _stockTracingManager = stockTracingManager;
        _excelImportFactory = excelImportFactory;
        _stockTracingDetailManager = stockTracingDetailManager;
        _fileDescriptorAppService = fileDescriptorAppService;
        _fileDescriptorRepository = fileDescriptorRepository;
        _stockTracingDetailRepository = stockTracingDetailRepository;
        _guidGenerator = guidGenerator;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public virtual async Task<PagedResultDto<StockTracingDto>> GetListAsync(GetStockTracingsInput input)
    {
        var filterParams = ObjectMapper.Map<GetStockTracingsInput, StockTracingFilterParams>(input);
        var totalCount = await _stockTracingRepository.GetCountAsync(filterParams);
        var items = await _stockTracingRepository.GetListAsync(filterParams, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<StockTracingDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<StockTracing>, List<StockTracingDto>>(items)
        };
    }

    public virtual async Task<StockTracingDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<StockTracing, StockTracingDto>(await _stockTracingRepository.GetWithDetailsAsync(id));
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        var details = await _stockTracingDetailRepository.GetListAsync(x => x.StockTracingId == id);
        if (details.Any())
        {
            await _stockTracingDetailRepository.DeleteManyAsync(details);
        }

        await _stockTracingRepository.DeleteAsync(id);
    }


    public virtual async Task<StockTracingDto> CreateAsync(StockTracingCreateDto input)
    {
        var createParams = ObjectMapper.Map<StockTracingCreateDto, StockTracingCreateParams>(input);
        var stockTracing = await _stockTracingManager.CreateAsync(createParams);

        return ObjectMapper.Map<StockTracing, StockTracingDto>(stockTracing);
    }


    public virtual async Task<StockTracingDto> UpdateAsync(Guid id, StockTracingUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<StockTracingUpdateDto, StockTracingUpdateParams>(input);

        var stockTracing = await _stockTracingManager.UpdateAsync(id, updateParams);

        return ObjectMapper.Map<StockTracing, StockTracingDto>(stockTracing);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(StockTracingExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }
        var filterParams = ObjectMapper.Map<StockTracingExcelDownloadDto, StockTracingFilterParams>(input);
        var items = await _stockTracingRepository.GetListAsync(filterParams);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<StockTracing>, List<StockTracingExcelDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "StockTracings.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new StockTracingDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new QuoteFlow.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }

    public virtual async Task<ExcelValidationResult<StockTracingDeliveryImportDto>> ValidateAndStockTracingDeliveryAsync(IRemoteStreamContent file, DateTime fromDate, DateTime toDate, string? note)
    {
        var validator = _excelImportFactory.CreateValidator<StockTracingDeliveryImportDto>(ExcelImporters.StockTracingDelivery);

        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.StockTracing.FromDate, fromDate);
        context.SetData(ExcelImportContextKeys.StockTracing.ToDate, toDate);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);
        var fileName = file.FileName ?? "";
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var isFileExisted = await _stockTracingRepository.AnyAsync(x => x.FileName == fileName && x.ReportType == ReportType.Delivery);
            if (isFileExisted)
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.StockTracing.FileAlreadyExists).WithData("fileName", fileName);
            }
        }

        return result;
    }
    [UnitOfWork(IsDisabled = true)] // Disable default UoW to manage transactions manually for better performance
    public async Task<StockTracingDto> ImportStockTracingDeliveryAsync(ExcelValidationResult<StockTracingDeliveryImportDto> data, DateTime fromDate, DateTime toDate, string? note)
    {
        const int batchSize = 5000;
        StockTracing stockTracing;

        // Step 1: Create StockTracing in a separate UoW
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
        {
            var stockTracingCreateParams = new StockTracingCreateParams
            {
                FileName = data.FileName,
                Note = note,
                FromDate = fromDate,
                ToDate = toDate,
                ReportType = ReportType.Delivery
            };
            stockTracing = await _stockTracingManager.CreateAsync(stockTracingCreateParams);
            await uow.CompleteAsync();
        }

        // Step 2: Convert data to create params
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<StockTracingDeliveryImportDto, StockTracingDetailCreateParams>(ExcelImporters.StockTracingDelivery);

        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.ParentEntityId, stockTracing.Id);

        var createParams = new List<StockTracingDetailCreateParams>();
        foreach (var row in data.ListData)
        {
            var param = await dataObjects.ConvertToCreateParamsAsync(row, context, default);
            if (param != null)
            {
                param.ReportType = ReportType.Delivery;
                createParams.Add(param);
            }
        }

        // Step 3: Bulk insert details using SqlBulkCopy in batches
        var detailEntities = createParams.Select(cp =>
            new StockTracingDetail(_guidGenerator.Create(), cp)).ToList();

        var batches = detailEntities.Chunk(batchSize);
        foreach (var batch in batches)
        {
            await _stockTracingDetailRepository.BulkInsertAsync(batch.ToList());
        }

        // Step 4: Retrieve and return result
        var res = await _stockTracingRepository.GetAsync(stockTracing.Id);
        return ObjectMapper.Map<StockTracing, StockTracingDto>(res);
    }

    public async Task<ExcelValidationResult<StockTracingInventoryImportDto>> ValidateAndStockTracingInventoryAsync(IRemoteStreamContent file, DateTime dateEntered, string? note)
    {
        var validator = _excelImportFactory.CreateValidator<StockTracingInventoryImportDto>(ExcelImporters.StockTracingInventory);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");
        var fileName = file.FileName ?? "";
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var isFileExisted = await _stockTracingRepository.AnyAsync(x => x.FileName == fileName && x.ReportType == ReportType.Inventory);
            if (isFileExisted)
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.StockTracing.FileAlreadyExists).WithData("fileName", fileName);
            }
        }

        return result;
    }

    public async Task<ExcelValidationResult<StockTracingReceiptImportDto>> ValidateAndStockTracingReceiptAsync(IRemoteStreamContent file, DateTime fromDate, DateTime toDate, string? note)
    {
        var validator = _excelImportFactory.CreateValidator<StockTracingReceiptImportDto>(ExcelImporters.StockTracingReceipt);

        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.StockTracing.FromDate, fromDate);
        context.SetData(ExcelImportContextKeys.StockTracing.ToDate, toDate);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);

        var fileName = file.FileName ?? "";
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var isFileExisted = await _stockTracingRepository.AnyAsync(x => x.FileName == fileName && x.ReportType == ReportType.Receipt);
            if (isFileExisted)
            {
                throw new BusinessException(QuoteFlowDomainErrorCodes.StockTracing.FileAlreadyExists).WithData("fileName", fileName);
            }
        }

        return result;
    }

    [UnitOfWork(IsDisabled = true)] // Disable default UoW to manage transactions manually for better performance
    public async Task<StockTracingDto> ImportStockTracingInvantoryAsync(ExcelValidationResult<StockTracingInventoryImportDto> data, DateTime? entered, string note)
    {
        const int batchSize = 5000;
        StockTracing stockTracing;

        // Step 1: Create StockTracing in a separate UoW
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
        {
            var stockTracingCreateParams = new StockTracingCreateParams
            {
                FileName = data.FileName,
                Note = note,
                ReportType = ReportType.Inventory
            };
            stockTracing = await _stockTracingManager.CreateAsync(stockTracingCreateParams);
            await uow.CompleteAsync();
        }

        // Step 2: Convert data to create params
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<StockTracingInventoryImportDto, StockTracingDetailCreateParams>(ExcelImporters.StockTracingInventory);

        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.ParentEntityId, stockTracing.Id);

        var createParams = new List<StockTracingDetailCreateParams>();
        foreach (var row in data.ListData)
        {
            var param = await dataObjects.ConvertToCreateParamsAsync(row, context, default);
            if (param != null)
            {
                param.ReportType = ReportType.Inventory;
                param.DateEntered = entered;
                createParams.Add(param);
            }
        }

        // Step 3: Bulk insert details using SqlBulkCopy in batches
        var detailEntities = createParams.Select(cp =>
            new StockTracingDetail(_guidGenerator.Create(), cp)).ToList();

        var batches = detailEntities.Chunk(batchSize);
        foreach (var batch in batches)
        {
            await _stockTracingDetailRepository.BulkInsertAsync(batch.ToList());
        }

        // Step 4: Retrieve and return result
        var res = await _stockTracingRepository.GetNoDetailsAsync(stockTracing.Id);
        return ObjectMapper.Map<StockTracing, StockTracingDto>(res);
    }

    [UnitOfWork(IsDisabled = true)] // Disable default UoW to manage transactions manually for better performance
    public async Task<StockTracingDto> ImportStockTracingReceiptAsync(ExcelValidationResult<StockTracingReceiptImportDto> data, DateTime fromDate, DateTime toDate, string note)
    {
        const int batchSize = 5000;
        StockTracing stockTracing;

        // Step 1: Create StockTracing in a separate UoW
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
        {
            var stockTracingCreateParams = new StockTracingCreateParams
            {
                FileName = data.FileName,
                Note = note,
                FromDate = fromDate,
                ToDate = toDate,
                ReportType = ReportType.Receipt
            };
            stockTracing = await _stockTracingManager.CreateAsync(stockTracingCreateParams);
            await uow.CompleteAsync();
        }

        // Step 2: Convert data to create params
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<StockTracingReceiptImportDto, StockTracingDetailCreateParams>(ExcelImporters.StockTracingReceipt);

        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.ParentEntityId, stockTracing.Id);

        var createParams = new List<StockTracingDetailCreateParams>();
        foreach (var row in data.ListData)
        {
            var param = await dataObjects.ConvertToCreateParamsAsync(row, context, default);
            if (param != null)
            {
                param.ReportType = ReportType.Receipt;
                createParams.Add(param);
            }
        }

        // Step 3: Bulk insert details using SqlBulkCopy in batches
        var detailEntities = createParams.Select(cp =>
            new StockTracingDetail(_guidGenerator.Create(), cp)).ToList();

        var batches = detailEntities.Chunk(batchSize);
        foreach (var batch in batches)
        {
            await _stockTracingDetailRepository.BulkInsertAsync(batch.ToList());
        }

        // Step 4: Retrieve and return result
        var res = await _stockTracingRepository.GetAsync(stockTracing.Id);
        return ObjectMapper.Map<StockTracing, StockTracingDto>(res);
    }
}
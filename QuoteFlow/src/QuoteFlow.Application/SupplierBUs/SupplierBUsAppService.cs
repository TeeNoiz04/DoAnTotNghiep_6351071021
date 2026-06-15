using ClosedXML.Excel;
using QuoteFlow.Permissions;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SupplierBUs.ParameterObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
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
using Volo.FileManagement.Files;

namespace QuoteFlow.SupplierBUs;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.MasterDatas.SupplierBU)]
public class SupplierBUsAppService : QuoteFlowAppService, ISupplierBUsAppService
{
    protected IDistributedCache<SupplierBUDownloadTokenCacheItem, string> _downloadTokenCache;
    protected ISupplierBURepository _supplierBURepository;
    protected SupplierBUManager _supplierBUManager;
    protected IExcelImportFactory _excelImportFactory;
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    private readonly FileDescriptorAppService _fileDescriptorAppService;

    public SupplierBUsAppService(ISupplierBURepository supplierBURepository, SupplierBUManager supplierBUManager, IDistributedCache<SupplierBUDownloadTokenCacheItem, string> downloadTokenCache, IExcelImportFactory excelImportFactory, IRepository<FileDescriptor, Guid> fileDescriptorRepository, FileDescriptorAppService fileDescriptorAppService)
    {
        _downloadTokenCache = downloadTokenCache;
        _supplierBURepository = supplierBURepository;
        _supplierBUManager = supplierBUManager;
        _excelImportFactory = excelImportFactory;
        _fileDescriptorRepository = fileDescriptorRepository;
        _fileDescriptorAppService = fileDescriptorAppService;
    }

    public virtual async Task<PagedResultDto<SupplierBUDto>> GetListAsync(GetSupplierBUsInput input)
    {
        var filterParams = ObjectMapper.Map<GetSupplierBUsInput, SupplierBUFilterParams>(input);
        var totalCount = await _supplierBURepository.GetCountAsync(filterParams);
        var items = await _supplierBURepository.GetListAsync(filterParams);

        return new PagedResultDto<SupplierBUDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<SupplierBU>, List<SupplierBUDto>>(items)
        };
    }

    public virtual async Task<SupplierBUDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<SupplierBU, SupplierBUDto>(await _supplierBURepository.GetAsync(id));
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        //await _supplierBURepository.DeleteAsync(id);
        try
        {
            await _supplierBURepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.Category.RecordInUseCannotDelete);
        }
    }


    public virtual async Task<SupplierBUDto> CreateAsync(SupplierBUCreateDto input)
    {
        var createParams = ObjectMapper.Map<SupplierBUCreateDto, SupplierBUCreateParams>(input);
        var supplierBU = await _supplierBUManager.CreateAsync(
        createParams
        );

        return ObjectMapper.Map<SupplierBU, SupplierBUDto>(supplierBU);
    }


    public virtual async Task<SupplierBUDto> UpdateAsync(Guid id, SupplierBUUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<SupplierBUUpdateDto, SupplierBUUpdateParams>(input);
        var supplierBU = await _supplierBUManager.UpdateAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<SupplierBU, SupplierBUDto>(supplierBU);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(SupplierBUExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var filterParams = ObjectMapper.Map<SupplierBUExcelDownloadDto, SupplierBUFilterParams>(input);
        //1. get data
        var items = await _supplierBURepository.GetListAsync(filterParams);
        //2. get template
        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "SupplierBUReport.xlsx")
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

        int startRow = 3; // start from row 3
        int startCol = 1; // column A

        // 5. Insert additional rows if there is more than 1 record
        if (items.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(items.Count - 1);
        }

        // 6. Write data to the sheet
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var row = ws.Row(startRow + i);
            int col = startCol;

            row.Cell(col++).Value = i + 1;
            row.Cell(col++).Value = item.SupplierBUCode;
            row.Cell(col++).Value = item.SupplierBURemarks;
            row.Cell(col++).Value = item.OrderMethod;
            row.Cell(col++).Value = item.POTemplate;
            row.Cell(col++).Value = item.Contact;
            row.Cell(col++).Value = item.Email;
            row.Cell(col++).Value = item.INCOTerm;
            row.Cell(col++).Value = item.PaymentTermCode;
            row.Cell(col++).Value = item.PaymentDescription;
            row.Cell(col++).Value = item.Currency;
            row.Cell(col++).Value = item.MaterialType;
            row.Cell(col++).Value = item.Supplier?.SAPCode;
            row.Cell(col++).Value = item.SupplierShortName;
            row.Cell(col++).Value = item.SupplierAddress;
            row.Cell(col++).Value = item.FASCMVendorCode;
            row.Cell(col++).Value = item.FASCMBuyerCode;
            row.Cell(col++).Value = item.FASCMConsigneeCode;
            row.Cell(col++).Value = item.FASCMSectionCode;
            row.Cell(col++).Value = item.FASCMPaymentTerm;
            row.Cell(col++).Value = item.FASCMFreightMethod;
            row.Cell(col++).Value = item.FASCMDeliveryTerms;
            row.Cell(col++).Value = item.FASCMPlaceOfDeliveryTerms;
            row.Cell(col++).Value = item.FASCMShippingMarkCode;
        }
        // 7. Determine the last row
        int lastRow = startRow + items.Count;


        // 11. Draw borders for the whole table (from startRow to Total row)
        var tableRange = ws.Range(startRow, 1, lastRow, 24); // A -> H
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // 12. Save workbook to a new stream
        var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
        outputStream.Position = 0;

        // 13. Return the file to the client
        return new RemoteStreamContent(
            outputStream,
            "SupplierBU.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    public virtual async Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new SupplierBUDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new QuoteFlow.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
    public virtual async Task<ExcelValidationResult<SupplierBUImportDto>> ValidateAndParseSupplierBUAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<SupplierBUImportDto>(ExcelImporters.SupplierBU);
        var context = new ExcelImportContext();

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);

        return result;
    }
    public async Task<List<SupplierBUDto>> ImportSupplierBUAsync(ExcelValidationResult<SupplierBUImportDto> dataImport)
    {
        var dataCreateObjects = _excelImportFactory.CreateCreateParamsConverter<SupplierBUImportDto, SupplierBUCreateParams>(ExcelImporters.SupplierBU);
        var dataUpdateObjects = _excelImportFactory.CreateCreateParamsConverter<SupplierBUImportDto, SupplierBUImportUpdateParams>(ExcelImporters.SupplierBUUpdate);
        var context = new ExcelImportContext();
        //context.SetData(ExcelImportContextKeys.ParentEntityId, stockData.Id);

        var recordUpdate = new ExcelValidationResult<SupplierBUImportDto>(dataImport.SingleRow, dataImport.FileName)
        {
            Errors = dataImport.Errors,
            ListData = dataImport.ListData.Where(r => r.RowData?.IsUpdate == true).ToList()
        };
        var recordCreate = new ExcelValidationResult<SupplierBUImportDto>(dataImport.SingleRow, dataImport.FileName)
        {
            Errors = dataImport.Errors,
            ListData = dataImport.ListData.Where(r => r.RowData?.IsUpdate == false).ToList()
        };
        List<SupplierBUCreateParams> createParams = (await Task.WhenAll(
        recordCreate.ListData.Select(async x =>
        {
            var item = await dataCreateObjects.ConvertToCreateParamsAsync(x, context, default);
            return item;
        })
        )).Where(x => x != null)
        .ToList()!;

        List<SupplierBUImportUpdateParams> updateParams = (await Task.WhenAll(
        recordUpdate.ListData.Select(async x =>
        {
            var itemUpdate = await dataUpdateObjects.ConvertToCreateParamsAsync(x, context, default);
            return itemUpdate;
        })
        )).Where(x => x != null)
        .ToList()!;

        var result = await _supplierBUManager.CreateManyAsync(createParams);
        var resultUpdate = await _supplierBUManager.UpdateManyAsync(updateParams);
        var allResults = result.Concat(resultUpdate).ToList();

        return ObjectMapper.Map<List<SupplierBU>, List<SupplierBUDto>>(allResults);

    }

    public async Task ChangeDeactiveSupplierBUAsync(List<Guid> ids)
    {
        var supplierBUs = await _supplierBURepository.GetListAsync(x => ids.Contains(x.Id));
        foreach (var supplierBU in supplierBUs)
        {
            supplierBU.IsDeactive = !supplierBU.IsDeactive;
        }
        await _supplierBURepository.UpdateManyAsync(supplierBUs);
    }
}
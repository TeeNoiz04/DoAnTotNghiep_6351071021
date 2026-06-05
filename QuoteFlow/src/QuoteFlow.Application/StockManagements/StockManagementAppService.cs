using ClosedXML.Excel;
using QuoteFlow.BuyerAccess;
using QuoteFlow.HistoryTrackings;
using QuoteFlow.HistoryTrackings.ParameterObjects;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialStocks;
using QuoteFlow.Materials.MaterialStocks.ParameterObjects;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.MaterialStockUploadDetails.ParameterObjects;
using QuoteFlow.MaterialStockUploads;
using QuoteFlow.MaterialStockUploads.ParameterObjects;
using QuoteFlow.Permissions;
using QuoteFlow.RequesterContexts;
using QuoteFlow.Shared.Excels;
using QuoteFlow.StockCategories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Volo.FileManagement.Files;

namespace QuoteFlow.StockManagements;
[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.MaterialStocks.Default)]
public class StockManagementAppService : ApplicationService, IStockManagementAppService
{
    protected readonly IMaterialRepository _materialRepository;
    protected readonly IMaterialStockUploadRepository _materialStockUploadRepository;
    protected readonly MaterialStockUploadManager _materialStockUploadManager;
    protected readonly MaterialStockUploadDetailManager _materialStockUploadDetailManager;
    protected readonly IMaterialStockRepository _materialStockRepository;
    protected IExcelImportFactory _excelImportFactory;
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    private readonly FileDescriptorAppService _fileDescriptorAppService;
    protected readonly IEffectiveUserContext _currentUser;
    protected readonly IHistoryTrackingRepository _historyTrackingRepository;
    protected readonly IStockCategoryRepository _stockCategoriesRepository;
    protected readonly IBuyerAccessService _buyerAccessService;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public StockManagementAppService(
        IMaterialRepository materialRepository,
        IExcelImportFactory excelImportFactory,
        MaterialStockUploadDetailManager materialStockUploadDetailManager,
        IMaterialStockUploadRepository materialStockUploadRepository,
        MaterialStockUploadManager materialStockUploadManager,
        IMaterialStockRepository materialStockRepository,
        IRepository<FileDescriptor, Guid> fileDescriptorRepository,
        FileDescriptorAppService fileDescriptorAppService,
        IEffectiveUserContext currentUser,
        IHistoryTrackingRepository historyTrackingRepository,
        IStockCategoryRepository stockCategoriesRepository,
        IBuyerAccessService buyerAccessService,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _materialRepository = materialRepository;
        _excelImportFactory = excelImportFactory;
        _materialStockUploadDetailManager = materialStockUploadDetailManager;
        _materialStockUploadRepository = materialStockUploadRepository;
        _materialStockUploadManager = materialStockUploadManager;
        _materialStockRepository = materialStockRepository;
        _fileDescriptorRepository = fileDescriptorRepository;
        _fileDescriptorAppService = fileDescriptorAppService;
        _currentUser = currentUser;
        _historyTrackingRepository = historyTrackingRepository;
        _stockCategoriesRepository = stockCategoriesRepository;
        _buyerAccessService = buyerAccessService;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task<PagedResultDto<MaterialStockDto>> GetListAsync(GetMaterialStocksInput input)
    {
        var filterParams = ObjectMapper.Map<GetMaterialStocksInput, MaterialStockFilterParams>(input);
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync(_currentUser.Username!);
        filterParams.ApplyMaterialTypeRestrictions(buyerAccess);

        var totalCount = await _materialStockRepository.GetCountAsync(filterParams);
        var items = await _materialStockRepository.GetListAsync(filterParams);
        return new PagedResultDto<MaterialStockDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<MaterialStock>, List<MaterialStockDto>>(items)
        };

    }

    public async Task<PagedResultDto<StockManagementUploadDto>> GetListUploadAsync(GetStockManagementApprovalsInput input)
    {
        var filterParams = ObjectMapper.Map<GetStockManagementApprovalsInput, MaterialStockUploadFilterParams>(input);
        var totalCount = await _materialStockUploadRepository.GetCountAsync(filterParams);
        var items = await _materialStockUploadRepository.GetListAsync(filterParams);
        return new PagedResultDto<StockManagementUploadDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<MaterialStockUpload>, List<StockManagementUploadDto>>(items)
        };
    }
    public async Task<StockManagementUploadDto> GetUploadDetailAsync(Guid id)
    {
        return ObjectMapper.Map<MaterialStockUpload, StockManagementUploadDto>(await _materialStockUploadRepository.GetAsync(id));
    }

    [Authorize(QuoteFlowPermissions.MaterialStocks.Uploads.StockTransfer)]
    public virtual async Task<ExcelValidationResult<MaterialStockUploadDetailImportTransferDto>> ValidateAndParseStockTransferAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<MaterialStockUploadDetailImportTransferDto>(ExcelImporters.MaterialStockUploadDetailImportTransfer);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }

    public virtual async Task<ListResultDto<HistoryTrackingDto>> GetStockHistoryAsync(GetStockHistoriesInput input)
    {
        var stockCode = input.StockCode;
        var stock = await _stockCategoriesRepository.GetAsync(x => x.StockCode.ToUpper() == stockCode.ToUpper());
        var stockId = stock.Id;
        var filterParams = new HistoryTrackingFilterParams()
        {
            TrackingType = HistoryTrackingTypes.Stock,
            GolfaCode = input.GolfaCode,
            StockId = stockId,
            CreationTimeMin = input.ActionFrom.Date,
            CreationTimeMax = input.ActionTo?.Date,
            Note = input.Note
        };
        var histories = await _historyTrackingRepository.GetListAsync(filterParams);

        return new ListResultDto<HistoryTrackingDto>
        {
            Items = ObjectMapper.Map<List<HistoryTracking>, List<HistoryTrackingDto>>(histories)
        };
    }

    public virtual async Task<IRemoteStreamContent> GetStockHistoryAsExcelAsync(GetStockHistoriesInput input)
    {
        var stockCode = input.StockCode;
        var stock = await _stockCategoriesRepository.GetAsync(x => x.StockCode.ToUpper() == stockCode.ToUpper());
        var stockId = stock.Id;
        var filterParams = new HistoryTrackingFilterParams()
        {
            TrackingType = HistoryTrackingTypes.Stock,
            GolfaCode = input.GolfaCode,
            StockId = stockId,
            CreationTimeMin = input.ActionFrom.Date,
            CreationTimeMax = input.ActionTo?.Date,
            Note = input.Note,
        };

        var histories = await _historyTrackingRepository.GetListAsync(filterParams);
        var historyDtos = ObjectMapper.Map<List<HistoryTracking>, List<HistoryTrackingDto>>(histories);

        var excelBytes = await CreateStockHistoryExcelFileAsync(historyDtos);

        var memoryStream = new MemoryStream(excelBytes);
        return new RemoteStreamContent(
            memoryStream,
            $"StockHistory_{stockCode}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    private async Task<byte[]> CreateStockHistoryExcelFileAsync(List<HistoryTrackingDto> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Stock History");

        // Headers
        var headers = new[]
        {
            "Row No", "Action", "Previous Value", "Qty", "Next Value", "Storage", "Action At", "Note"
        };

        // Add headers with basic styling
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        // Add data rows
        for (int row = 0; row < data.Count; row++)
        {
            var item = data[row];
            var excelRow = row + 2;

            // "Row No"
            worksheet.Cell(excelRow, 1).Value = row + 1;

            // "Action"
            worksheet.Cell(excelRow, 2).Value = item.Action ?? "";

            // "Qty" with no decimals
            if (item.Qty.HasValue)
            {
                worksheet.Cell(excelRow, 4).Value = (double)item.Qty.Value;
                worksheet.Cell(excelRow, 4).Style.NumberFormat.Format = "0";
            }

            // "Previous Value"
            if (item.PreviousValue.HasValue)
            {
                worksheet.Cell(excelRow, 3).Value = (double)item.PreviousValue.Value;
                worksheet.Cell(excelRow, 3).Style.NumberFormat.Format = "0";
            }

            // "Next Value"
            if (item.NextValue.HasValue)
            {
                worksheet.Cell(excelRow, 5).Value = (double)item.NextValue.Value;
                worksheet.Cell(excelRow, 5).Style.NumberFormat.Format = "0";
            }

            // "Storage" (Stock Name)
            worksheet.Cell(excelRow, 6).Value = item.StockName ?? "";

            // "Action At" with custom format
            worksheet.Cell(excelRow, 7).Value = item.CreationTime.ToString("dd/MM/yyyy HH:mm");

            // "Note"
            worksheet.Cell(excelRow, 8).Value = item.Note ?? "";
        }

        // Define custom widths for each column
        var columnWidths = new Dictionary<int, double>
        {
            { 1, 10 },  // Row No
            { 2, 20 },  // Action
            { 3, 15 },  // Qty
            { 4, 20 },  // Previous Value
            { 5, 20 },  // Next Value
            { 6, 25 },  // Storage
            { 7, 20 },  // Action At
            { 8, 30 }   // Note
        };

        // Set each column's width using the dictionary
        foreach (var kvp in columnWidths)
        {
            worksheet.Column(kvp.Key).Width = kvp.Value;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    [Authorize(QuoteFlowPermissions.MaterialStocks.Uploads.StockTransfer)]
    [UnitOfWork(IsDisabled = true)]
    public async Task ImportMaterialStockTransferAsync(ExcelValidationResult<MaterialStockUploadDetailImportTransferDto> data, string? note)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
        {
            var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<MaterialStockUploadDetailImportTransferDto, MaterialStockUploadDetailCreateParams>(ExcelImporters.MaterialStockUploadDetailImportTransfer);
            var createParamStockUploads = new MaterialStockUploadCreateParams()
            {
                Note = note,
                FileName = data.FileName,
                ImportType = "Stock.Transfer"
            };

            var stockUpload = await _materialStockUploadManager.CreateAsync(createParamStockUploads);
            var context = new ExcelImportContext();
            context.SetData(ExcelImportContextKeys.ParentEntityId, stockUpload.Id);

            List<MaterialStockUploadDetailCreateParams> createParams = (await Task.WhenAll(
                data.ListData
                    .Select(x => createParamsConverters.ConvertToCreateParamsAsync(x, context, default))
            )).Where(x => x != null)
            .ToList()!;


            await _materialStockUploadDetailManager.CreateBatchAsync(createParams);

            var err = await _materialRepository.ValidationTransferAsync(stockUpload.Id, _currentUser.Username!, _currentUser.FullName!);

            if (!string.IsNullOrWhiteSpace(err))
            {
                throw new UserFriendlyException($"{err}");
            }
        }
    }

    [Authorize(QuoteFlowPermissions.MaterialStocks.Uploads.StockInventory)]
    public virtual async Task<ExcelValidationResult<MaterialStockUploadDetailImportInventoryDto>> ValidateAndParseStockInventoryAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<MaterialStockUploadDetailImportInventoryDto>(ExcelImporters.MaterialStockUploadDetailImportInventory);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }

    [Authorize(QuoteFlowPermissions.MaterialStocks.Uploads.StockInventory)]
    [UnitOfWork(IsDisabled = true)]
    public async Task ImportMaterialStockInventoryAsync(ExcelValidationResult<MaterialStockUploadDetailImportInventoryDto> data, string? note)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
        {
            var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<MaterialStockUploadDetailImportInventoryDto, MaterialStockUploadDetailCreateParams>(ExcelImporters.MaterialStockUploadDetailImportInventory);
            var createParamStockUploads = new MaterialStockUploadCreateParams()
            {
                Note = note,
                FileName = data.FileName,
                ImportType = "Stock.Inventory"
            };

            var stockUpload = await _materialStockUploadManager.CreateAsync(createParamStockUploads);

            var context = new ExcelImportContext();
            context.SetData(ExcelImportContextKeys.ParentEntityId, stockUpload.Id);

            List<MaterialStockUploadDetailCreateParams> createParams = (await Task.WhenAll(
                data.ListData
                    .Select(x => createParamsConverters.ConvertToCreateParamsAsync(x, context, default))
            )).Where(x => x != null)
            .ToList()!;


            await _materialStockUploadDetailManager.CreateBatchAsync(createParams);

            var err = await _materialRepository.ValidationInventoryAsync(stockUpload.Id, _currentUser.Username!, _currentUser.FullName!);

            if (!string.IsNullOrWhiteSpace(err))
            {
                throw new UserFriendlyException($"{err}");
            }
        }


    }

    public async Task<PagedResultDto<StockManagementListDto>> GetListStockManagementAsync(GetStockManagementsListInput input)
    {
        var filterParams = ObjectMapper.Map<GetStockManagementsListInput, StockManagementFilterParams>(input);
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync(_currentUser.Username!);
        filterParams.ApplyMaterialTypeRestrictions(buyerAccess);

        var items = await _materialRepository.GetStockManagementListAsync(filterParams, false);
        var result = items.Skip(filterParams.SkipCount).Take(filterParams.MaxResultCount).ToList();

        return new PagedResultDto<StockManagementListDto>
        {
            TotalCount = items.Count,
            Items = ObjectMapper.Map<List<StockManagementList>, List<StockManagementListDto>>(result)
        };
    }

    public async Task<IRemoteStreamContent> GetListStockManagementExcelAsync(GetStockManagementsListInput input)
    {
        var filterParams = ObjectMapper.Map<GetStockManagementsListInput, StockManagementFilterParams>(input);
        var items = await _materialRepository.GetStockManagementListAsync(filterParams, true);

        // 2. Get the template file
        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "Template_StockManagement_Export.xlsx")
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

        int startRow = 2; // start from row 3
        int startCol = 1; // column A
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

            row.Cell(col++).Value = item.GolfaCode;            // A: Golfa Code
            row.Cell(col++).Value = item.Model;                // B: Model
            row.Cell(col++).Value = item.Spec1;                // C: Spec1
            row.Cell(col++).Value = item.MaterialStatus;       // D: Material Status
            row.Cell(col++).Value = item.StockCode;            // E: Stock Code
            row.Cell(col++).Value = item.StockName;            // F: Stock Name
            row.Cell(col++).Value = item.Standard_Price;       // G: Standard Price
            row.Cell(col++).Value = item.ReferenceLeadTime;    // H: Reference Lead Time
            row.Cell(col++).Value = item.CountryOfOrigin;      // I: Country of Origin
            row.Cell(col++).Value = item.WarrantyTime;         // J: Warranty Time
            row.Cell(col++).Value = item.Maxlot;               // K: Maxlot
            row.Cell(col++).Value = item.MaterialType;         // L: Material Type
            row.Cell(col++).Value = item.SAP_Code;             // M: SAP Code
            row.Cell(col++).Value = item.Material_Group;       // N: Material Group
            row.Cell(col++).Value = item.Stock_Qty;            // O: Stock Qty
            row.Cell(col++).Value = item.Locked_Qty;           // P: Locked Qty
            row.Cell(col++).Value = item.LockStockSO_Qty;      // Q: LockStockSO Qty
            row.Cell(col++).Value = item.Available_Qty;        // R: Available Qty
            row.Cell(col++).Value = item.Lockshipment_Qty;     // S: Lockshipment Qty
            row.Cell(col++).Value = item.OnOderStock;          // T: On Order Stock
        }
        var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
        outputStream.Position = 0;


        return new RemoteStreamContent(
            outputStream,
            "InventoryReport.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    public async Task<MaterialDto> GetAsync(string golfaCode)
    {
        return ObjectMapper.Map<Material, MaterialDto>(await _materialRepository.GetWithDetailAsync(golfaCode));
    }

    public async Task<List<StockQtyDto>> GetStockQtyAsync(string? materialCode, Guid? stockId)
    {
        return ObjectMapper.Map<List<StockQty>, List<StockQtyDto>>(await _materialRepository.StockQtyAsync(materialCode, stockId));
    }

    public async Task<List<LockedDto>> GetLockedAsync(string? materialCode)
    {
        return ObjectMapper.Map<List<Locked>, List<LockedDto>>(await _materialRepository.LockedAsync(materialCode));
    }

    public async Task<List<StockOfSODto>> GetStockOfSOAsync(string? materialCode, Guid? stockId)
    {
        return ObjectMapper.Map<List<StockOfSO>, List<StockOfSODto>>(await _materialRepository.StockOfSOAsync(materialCode, stockId));
    }

    public async Task<List<LockShipmentDto>> GetLockShipmentAsync(string? materialCode)
    {
        return ObjectMapper.Map<List<LockShipment>, List<LockShipmentDto>>(await _materialRepository.GetLockShipmentAsync(materialCode));
    }

    public async Task<List<OnOrderStockDto>> GetOnOrderStockAsync(string? materialCode)
    {
        return ObjectMapper.Map<List<OnOrderStock>, List<OnOrderStockDto>>(await _materialRepository.OnOrderStockAsync(materialCode));
    }

    public async Task<IRemoteStreamContent> GetListOverallStockReportAsync()
    {
        // 1. Get data
        var items = await _materialRepository.GetListStockOverallAsync();

        // 2. Get the template file
        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "OverallStockReport.xlsx")
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

            row.Cell(col++).Value = item.MaterialType;
            row.Cell(col++).Value = item.Material_Group;
            row.Cell(col).Value = item.Available_Stock;
            row.Cell(col).Style.NumberFormat.SetFormat("#,##0");
            col++;

            row.Cell(col).Value = item.Keeping_Stock;
            row.Cell(col).Style.NumberFormat.SetFormat("#,##0");
            col++;

            row.Cell(col).Value = item.On_Order_Stock;
            row.Cell(col).Style.NumberFormat.SetFormat("#,##0");
            col++;

            row.Cell(col).Value = item.StockWarning;
            row.Cell(col).Style.NumberFormat.SetFormat("#,##0");
            col++;


            row.Cell(col).Value = item.Available_Stock + item.Keeping_Stock + item.On_Order_Stock + item.StockWarning;
            row.Cell(col).Style.NumberFormat.SetFormat("#,##0");
        }

        int lastRow = startRow + items.Count;


        ws.Range(startRow, 7, lastRow - 1, 7)
            .Style.Fill.BackgroundColor = XLColor.LightYellow;


        var totalRow = ws.Row(lastRow);
        totalRow.Cell(2).Value = "Total";


        for (int col = 3; col <= 7; col++)
        {
            string colLetter = ws.Column(col).ColumnLetter();
            totalRow.Cell(col).FormulaA1 = $"SUM({colLetter}{startRow}:{colLetter}{lastRow - 1})";
            totalRow.Cell(col).Style.NumberFormat.SetFormat("#,##0");
        }


        ws.Range(lastRow, 1, lastRow, 8)
            .Style.Fill.BackgroundColor = XLColor.LightGray;
        ws.Range(lastRow, 1, lastRow, 8)
            .Style.Font.Bold = true;


        var tableRange = ws.Range(startRow, 1, lastRow, 8);
        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;


        var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
        outputStream.Position = 0;


        return new RemoteStreamContent(
            outputStream,
            "OverallStockReport.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    [Authorize(QuoteFlowPermissions.Reports.R21OverallStock)]
    public async Task<List<DataMaterialOverallStockReportDto>> GeDataOverallStockReportAsync()
    {
        var items = await _materialRepository.GetListStockOverallAsync();
        return ObjectMapper.Map<List<MaterialOverallStockReport>, List<DataMaterialOverallStockReportDto>>(items);
    }

    [Authorize(QuoteFlowPermissions.Reports.R15Inventory)]
    public async Task<PagedResultDto<InventoryReportDto>> GetListInventoryReportAsync(GetInventoryReportsInput input)
    {
        var filterParams = ObjectMapper.Map<GetInventoryReportsInput, ExcelInventoryReportFilterParams>(input);
        filterParams.Export = false;
        var items = await _materialRepository.GetInventoryReportAsync(filterParams);
        var count = await _materialRepository.GetCountInventoryReportAsync(filterParams);

        return new PagedResultDto<InventoryReportDto>
        {
            TotalCount = count,
            Items = ObjectMapper.Map<List<InventoryReport>, List<InventoryReportDto>>(items)
        };
    }

    public async Task<IRemoteStreamContent> GetListExcelInventoryReportAsync(GetInventoryReportsInput input)
    {
        var filterParams = ObjectMapper.Map<GetInventoryReportsInput, ExcelInventoryReportFilterParams>(input);
        filterParams.Export = true;
        var items = await _materialRepository.GetInventoryReportAsync(filterParams);

        // 2. Get the template file
        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "InventoryReport.xlsx")
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

        int startRow = 5; // start from row 3
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
            row.Cell(col++).Value = item.GolfaCode;
            row.Cell(col++).Value = item.Model;
            row.Cell(col++).Value = item.Spec1;
            row.Cell(col++).Value = item.SAP_Code;
            row.Cell(col++).Value = item.InventoryCategory;
            row.Cell(col++).Value = item.Material_Group;
            row.Cell(col++).Value = item.Standard_Price;
            row.Cell(col++).Value = item.LandedCost;
            row.Cell(col++).Value = item.Inventory_Qty;
            row.Cell(col++).Value = item.Inventory_Amount;
            row.Cell(col++).Value = item.AvailableStock_Qty;
            row.Cell(col++).Value = item.AvailableStock_Amount;
            row.Cell(col++).Value = item.GKR_Qty;
            row.Cell(col++).Value = item.GKR_Amount;
            row.Cell(col++).Value = item.LockedStock_Qty;
            row.Cell(col++).Value = item.LockedStock_Amount;
            row.Cell(col++).Value = item.MEVNBackOrder_Qty;
            row.Cell(col++).Value = item.MEVNBackOrder_Amount;
            row.Cell(col++).Value = item.MEVNBackOrder_OnOrder_Qty;
            row.Cell(col++).Value = item.MEVNBackOrder_OnOrder_Amount;
            row.Cell(col++).Value = item.MEVNBackOrder_Locked_Qty;
            row.Cell(col++).Value = item.MEVNBackOrder_Locked_Amount;
            row.Cell(col++).Value = item.StockWarning_Qty;
            row.Cell(col++).Value = item.StockWarning_Amount;



        }



        var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
        outputStream.Position = 0;


        return new RemoteStreamContent(
            outputStream,
            "InventoryReport.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }
}

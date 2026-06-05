using QuoteFlow.BuyerAccess;
using QuoteFlow.Buyers;
using QuoteFlow.GICs;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;
using QuoteFlow.Permissions;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrders.ParameterObjects;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.SaleOrders.SaleOrderDetails.ParameterObjects;
using QuoteFlow.SaleOrdersSapImports;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Volo.FileManagement.Files;

namespace QuoteFlow.SaleOrders;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.SaleOrders.Default)]
public class SaleOrdersAppService : QuoteFlowAppService, ISaleOrdersAppService
{
    protected ISaleOrderRepository _saleOrderRepository;
    protected SaleOrderManager _saleOrderManager;
    protected SaleOrderDetailManager _saleOrderDetailManager;
    protected SaleOrdersSapImportManager _saleOrdersSapImportManager;
    protected ISaleOrderDetailRepository _saleOrderDetailRepository;
    protected IMaterialStockLockStockRepository _materialStockLockStockRepository;
    protected readonly IEffectiveUserContext _currentUser;
    protected IExcelImportFactory _excelImportFactory;
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    private readonly FileDescriptorAppService _fileDescriptorAppService;
    private readonly SaleOrderFlaggingService _saleOrderFlaggingService;
    private readonly IBuyerAccessService _buyerAccessService;
    private readonly IBuyerRepository _buyerRepository;

    private readonly IUnitOfWorkManager _unitOfWorkManager;
    public SaleOrdersAppService(
        ISaleOrderRepository saleOrderRepository,
        SaleOrderManager saleOrderManager,
        IMaterialStockLockStockRepository materialStockLockStockRepository,
        SaleOrderDetailManager saleOrderDetailManager,
        ISaleOrderDetailRepository saleOrderDetailRepository,
        IEffectiveUserContext currentUser,
        IExcelImportFactory excelImportFactory,
        SaleOrdersSapImportManager saleOrdersSapImportManager,
        FileDescriptorAppService fileDescriptorAppService,
        IRepository<FileDescriptor, Guid> fileDescriptorRepository,
        SaleOrderFlaggingService saleOrderFlaggingService,
        IEffectiveUserContext currentFullUser,
        IBuyerAccessService buyerAccessService,
        IBuyerRepository buyerRepository,
        IUnitOfWorkManager unitOfWorkManager)
    {

        _saleOrderRepository = saleOrderRepository;
        _saleOrderManager = saleOrderManager;
        _materialStockLockStockRepository = materialStockLockStockRepository;
        _saleOrderDetailManager = saleOrderDetailManager;
        _saleOrderDetailRepository = saleOrderDetailRepository;
        _currentUser = currentUser;
        _excelImportFactory = excelImportFactory;
        _saleOrdersSapImportManager = saleOrdersSapImportManager;
        _fileDescriptorAppService = fileDescriptorAppService;
        _fileDescriptorRepository = fileDescriptorRepository;
        _saleOrderFlaggingService = saleOrderFlaggingService;
        _buyerAccessService = buyerAccessService;
        _buyerRepository = buyerRepository;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public virtual async Task<PagedResultDto<SaleOrderDto>> GetListAsync(GetSaleOrdersInput input)
    {
        var filterParams = ObjectMapper.Map<GetSaleOrdersInput, SaleOrderFilterParams>(input);
        if (!string.IsNullOrEmpty(input.StatusCode) && input.StatusCode == QuoteFlowStatuses.InProgress)
        {
            filterParams.StatusCodes = [input.StatusCode, QuoteFlowStatuses.Draft];
        }
        else
        {
            filterParams.StatusCodes = [input.StatusCode ?? ""];
        }

        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        filterParams.ApplyBuyerRestrictions(buyerAccess);
        filterParams.ApplyMaterialTypeRestrictions(buyerAccess);

        var totalCount = await _saleOrderRepository.GetCountAsync(filterParams);
        var items = await _saleOrderRepository.GetListAsync(filterParams);

        var saleOrderDtos = ObjectMapper.Map<List<SaleOrder>, List<SaleOrderDto>>(items);

        // Add flags to each sale order
        var flags = await _saleOrderFlaggingService.CreateBulkFlagsAsync(items);
        foreach (var dto in saleOrderDtos)
        {
            if (flags.TryGetValue(dto.Id, out var flag))
            {
                dto.Flags = flag;
            }
        }

        return new PagedResultDto<SaleOrderDto>
        {
            TotalCount = totalCount,
            Items = saleOrderDtos
        };
    }

    public virtual async Task<SaleOrderDto> GetAsync(Guid id)
    {
        var saleOrder = await _saleOrderRepository.GetAsync(id);

        if (saleOrder.IsDeleted)
        {
            throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(SaleOrder), id);
        }
       
        var saleOrderDto = ObjectMapper.Map<SaleOrder, SaleOrderDto>(saleOrder);
        // Check permission see SAP landing cost or amount
        bool canViewSAPLandingCost = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.SaleOrders.SAPLandingCost);
        if (!canViewSAPLandingCost && saleOrderDto.SaleOrderDetails != null  )
        {
            foreach (var item in saleOrderDto.SaleOrderDetails)
            {
                item.SAPLandingCost = 0;
                item.Amount = 0;
            }
        }
        // Add flags to the sale order
        var flags = await _saleOrderFlaggingService.CreateFlagsAsync(saleOrder);
        saleOrderDto.Flags = flags;

        return saleOrderDto;
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var error = await _saleOrderRepository.DeleteSOAsync(id, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.Create)]
    public virtual async Task<SaleOrderDto> CreateAsync(SaleOrderCreateDto input)
    {
        var createParams = ObjectMapper.Map<SaleOrderCreateDto, SaleOrderCreateParams>(input);

        var buyer = await _buyerRepository.GetWithDetailsAsync(createParams.BuyerId ?? Guid.Empty);

        createParams.BuyerCode = buyer?.BuyerCode;
        createParams.BuyerType = buyer?.BuyerType?.Code;
        createParams.BuyerName = buyer?.ShortName;

        var saleOrder = await _saleOrderManager.CreateAsync(createParams);

        return ObjectMapper.Map<SaleOrder, SaleOrderDto>(saleOrder);
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.Edit)]
    public virtual async Task<SaleOrderDto> UpdateAsync(Guid id, SaleOrderUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<SaleOrderUpdateDto, SaleOrderUpdateParams>(input);

        var buyer = await _buyerRepository.GetWithDetailsAsync(updateParams.BuyerId ?? Guid.Empty);

        updateParams.BuyerCode = buyer?.BuyerCode;
        updateParams.BuyerType = buyer?.BuyerType?.Code;
        updateParams.BuyerName = buyer?.ShortName;

        var saleOrder = await _saleOrderManager.UpdateAsync(id, updateParams);

        return ObjectMapper.Map<SaleOrder, SaleOrderDto>(saleOrder);
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.Edit)]
    public virtual async Task<SaleOrderDetailDto> UpdateDetailAsync(Guid id, SaleOrderDetailUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<SaleOrderDetailUpdateDto, SaleOrderDetailUpdateParams>(input);
        var saleOrder = await _saleOrderDetailManager.UpdatePriceAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<SaleOrderDetail, SaleOrderDetailDto>(saleOrder);
    }

    public virtual async Task<SaleOrderDetailDto> UpdateNoteAsync(Guid id, SaleOrderDetailUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<SaleOrderDetailUpdateDto, SaleOrderDetailUpdateParams>(input);
        var saleOrder = await _saleOrderDetailManager.UpdateNoteAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<SaleOrderDetail, SaleOrderDetailDto>(saleOrder);
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.DeleteItem)]
    public virtual async Task DeleteDetailAsync(Guid id)
    {
        var error = await _saleOrderRepository.DeleteSODetailAsync(id, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.Create)]
    [UnitOfWork(IsDisabled = true)]
    public async Task CreateDetailDPOAsync(List<SaleOrderAddedDetailDPODto> input)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
        {
            var filterParams = ObjectMapper.Map<List<SaleOrderAddedDetailDPODto>, List<SaleOrderAddedDetailDPOParams>>(input);
            foreach (var item in filterParams)
            {
                item.UserName = _currentUser.Username!;
                item.UserFullName = _currentUser.FullName!;
            }
            var errorMes = await _saleOrderRepository.SaveSODetailAsync(filterParams);
            if (!string.IsNullOrWhiteSpace(errorMes))
            {
                throw new UserFriendlyException(errorMes);
            }

            await _saleOrderManager.UpdateStatusInProgressAsync(input[0].PrSOId);
        }
    }

    public virtual async Task<PagedResultDto<SaleOrderListDetailDPODto>> GetListDetailDPOAsync(GetSaleOrderListDetailDPOsInput input)
    {
        var filterParams = ObjectMapper.Map<GetSaleOrderListDetailDPOsInput, SaleOrderGetListDetailDPOParams>(input);
        var totalCount = await _saleOrderRepository.GetListAddDetailDPOCountAsync(filterParams);
        var items = await _saleOrderRepository.GetListAddDetailDPOAsync(filterParams);

        return new PagedResultDto<SaleOrderListDetailDPODto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<SaleOrderListDetailDPO>, List<SaleOrderListDetailDPODto>>(items)
        };
    }

    public virtual async Task<PagedResultDto<SaleOrderListDetailGICDto>> GetListDetailGICAsync(GetSaleOrderListDetailGICsInput input)
    {
        var filterParams = ObjectMapper.Map<GetSaleOrderListDetailGICsInput, SaleOrderGetListDetailGICParams>(input);
        var items = await _saleOrderRepository.GetListAddDetailGICAsync(filterParams);
        var totalCount = items.Count;

        return new PagedResultDto<SaleOrderListDetailGICDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<SaleOrderListDetailGIC>, List<SaleOrderListDetailGICDto>>(items)
        };
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.Reopen)]
    public virtual async Task ReOpenSOAsync(Guid id)
    {
        var error = await _saleOrderRepository.ReOpenSO(id, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
        var history = SetSOHistory(id, HistoryActions.SaleOrder.Repoen, "Reopen");
        var saleOrder = await _saleOrderRepository.GetAsync(id);
        saleOrder.RecordAction(history);
    }

    private SOHistory SetSOHistory(Guid soId, string action, string? note = null)
    {
        var actionDate = DateTime.Now;
        //var rolePriority = new[]
        //{
        //    "Admin_FA",
        //    "PlanningManager",
        //};

        var role = "FA Team";
        //rolePriority
        //.FirstOrDefault(p =>
        //    _currentUser.Roles?.Any(r =>
        //        string.Equals(r, p, StringComparison.OrdinalIgnoreCase)
        //    ) == true
        //) ?? string.Empty;

        return new SOHistory(
            GuidGenerator.Create(),
            soId,
            new(
                role,
                role,
                _currentUser.Username ?? CurrentUser.UserName ?? string.Empty,
                _currentUser.FullName ?? CurrentUser.Name ?? string.Empty,
                action,
                actionDate,
                note)
        );
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ConfirmDelivery)]
    public virtual async Task ConfirmDelivery(Guid id)
    {
        var saleOrder = await _saleOrderRepository.GetAsync(id);
        if (saleOrder.StatusCode != QuoteFlowStatuses.InProgress)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.SalesOrder.OnlyInProgressCanConfirmDelivery);
        }

        var error = await _saleOrderRepository.ConfirmDelivery(id, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
        var history = SetSOHistory(id, HistoryActions.SaleOrder.ConfirmedDelivery, "Closed by Confrimed Delivery");
        saleOrder.RecordAction(history);
    }

    public virtual async Task ConfirmDeliveryGIC(Guid id)
    {
        var saleOrder = await _saleOrderRepository.GetAsync(id);
        if (saleOrder.StatusCode != QuoteFlowStatuses.InProgress)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.SalesOrder.OnlyInProgressCanConfirmDelivery);
        }

        var error = await _saleOrderRepository.ConfirmDeliveryGIC(id, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
        var history = SetSOHistory(id, HistoryActions.SaleOrder.ConfirmedDelivery, "Closed by Confrimed Delivery");
        saleOrder.RecordAction(history);
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public virtual async Task<ExcelValidationResult<SaleOrderExcelDto>> ValidateAndParseAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<SaleOrderExcelDto>(ExcelImporters.SaleOrders);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public async Task ImportSOAsync(ExcelValidationResult<SaleOrderExcelDto> data)
    {
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<SaleOrderExcelDto, SaleOrderSapImportCreateParams>(ExcelImporters.SaleOrders);

        var context = new ExcelImportContext();
        var createdGuid = GuidGenerator.Create();

        List<SaleOrderSapImportCreateParams> createParams = (await Task.WhenAll(
        data.ListData.Select(async x =>
        {
            var item = await dataObjects.ConvertToCreateParamsAsync(x, context, default);
            if (item != null)
            {
                item.ImportKey = createdGuid;
                item.FileName = data.FileName;

            }
            return item;
        })
        )).Where(x => x != null)
        .ToList()!;

        await _saleOrdersSapImportManager.CreateBatchAsync(createParams);
        await WriteHistoryAsync(createParams);
        await UnitOfWorkManager.Current!.SaveChangesAsync();
        var error = await _saleOrderRepository.ImportSAPDataAsync(createdGuid, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
    }
    public async Task<IRemoteStreamContent> GetListGICAsExcelFileAsync(GetSaleOrdersInput input)
    {
        string fileName, sheetName;
        Dictionary<int, string> columnMapping;

        //if (string.IsNullOrWhiteSpace(input.GicType))
        //{
        //    throw new UserFriendlyException("Please select GIC type.");
        //}
        //check same GIC Type
        var type = await _saleOrderManager.CheckSameGICTypeAsync(input.LstSO, exportSAP: true);
        input.GicType = type;

        // Xác định file template và column mapping dựa vào GIC Type
        switch (type)
        {
            case GICTypeCodes.Internal:
                fileName = "SO_SAP_Export_GIC_Internal.xlsx";
                sheetName = "Data_IU";
                columnMapping = GetInternalColumnMapping();
                break;

            case GICTypeCodes.GivingSponsor:
                fileName = "SO_SAP_Export_GIC_Sponsor.xlsx";
                sheetName = "Data_FOC";
                columnMapping = GetSponsorColumnMapping();
                break;

            case GICTypeCodes.Warranty:
                fileName = "SO_SAP_Export_GIC_Warranty.xlsx";
                sheetName = "Data_WR";
                columnMapping = GetWarrantyColumnMapping();
                break;

            case GICTypeCodes.WriteOff:
                fileName = "SO_SAP_Export_GIC_WriteOff.xlsx";
                sheetName = "Data_WO";
                columnMapping = GetWriteOffColumnMapping();
                break;

            default:
                throw new InvalidOperationException($"Unknown GIC type: {input.GicType}");
        }

        var filterParams = ObjectMapper.Map<GetSaleOrdersInput, SaleOrderListExportSAPDataParams>(input);
        filterParams.Username = _currentUser.Username;
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        var items = await _saleOrderRepository.ExportSAPGICDataAsync(filterParams);

        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("Template Excel not found.");

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheet(sheetName);

        int startRow = 3;
        if (items.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(items.Count - 1);
        }
        bool canViewLanding = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.SaleOrders.SAPLandingCost);
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var row = ws.Row(startRow + i);

            foreach (var mapping in columnMapping)
            {
                int columnIndex = mapping.Key;
                string propertyName = mapping.Value;
                if(canViewLanding == false && (propertyName == nameof(SaleOrderExportSAPGICData.SAPLandingCost) || propertyName == nameof(SaleOrderExportSAPGICData.AmountInLandingCost)))
                {
                    // write into cell
                    var celll = row.Cell(columnIndex);
                    celll.Value = 0;
                    continue; // Skip these columns if user doesn't have permission
                }
                // get value from property
                var value = GetPropertyValue(item, propertyName);

                // write into cell
                var cell = row.Cell(columnIndex);

                // Format Date
                if (value is DateTime dateValue)
                {
                    cell.Value = dateValue;
                    //cell.Style.DateFormat.Format = "dd/MM/yyyy";
                }
                // Format for number
                else if (value is decimal || value is double || value is float)
                {
                    if (propertyName == "VAT")
                    {
                        cell.Value = Convert.ToDouble(value);
                        //cell.Style.NumberFormat.Format = "0%";
                    }
                    else
                    {
                        cell.Value = Convert.ToDouble(value);
                        //cell.Style.NumberFormat.Format = "#,##0";
                    }
                }
                else
                {
                    cell.Value = value?.ToString() ?? string.Empty;
                }
            }
        }

        foreach (var worksheet in workbook.Worksheets)
        {
            worksheet.CellsUsed().Style.Font.FontName = "Arial";
        }

        // save and return to file
        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;

        return new RemoteStreamContent(
            output,
            fileName,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }
    [Authorize(QuoteFlowPermissions.SaleOrders.ImportInternalUseChange)]
    public async Task<IRemoteStreamContent> GetListGICInternalUseChangeAsExcelFileAsync(GetSaleOrdersInput input)
    {
        string fileName, sheetName;
        Dictionary<int, string> columnMapping;

        //if (string.IsNullOrWhiteSpace(input.GicType))
        //{
        //    throw new UserFriendlyException("Please select GIC type.");
        //}
        //check same GIC Type
        var type = await _saleOrderManager.CheckSameGICTypeAsync(input.LstSO);
        input.GicType = type;

        fileName = "SO_SAP_Export_GIC_Internal_Use_Change.xlsx";
        sheetName = "Data_IU_Change";
        columnMapping = GetInternalUseChangeColumnMapping();

        var filterParams = ObjectMapper.Map<GetSaleOrdersInput, SaleOrderListExportSAPDataParams>(input);
        filterParams.Username = _currentUser.Username;
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        var items = await _saleOrderRepository.ExportSAPGICDataAsync(filterParams, isExport: true);

        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == fileName)
            ?? throw new UserFriendlyException("Template Excel not found.");

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheet(sheetName);

        int startRow = 3;
        if (items.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(items.Count - 1);
        }

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var row = ws.Row(startRow + i);

            foreach (var mapping in columnMapping)
            {
                int columnIndex = mapping.Key;
                string propertyName = mapping.Value;

                // get value from property
                var value = GetPropertyValue(item, propertyName);

                // write into cell
                var cell = row.Cell(columnIndex);

                // Format Date
                if (value is DateTime dateValue)
                {
                    cell.Value = dateValue;
                    cell.Style.DateFormat.Format = "dd/MM/yyyy";
                }
                // Format for number
                else if (value is decimal || value is double || value is float)
                {
                    cell.Value = Convert.ToDouble(value);
                    cell.Style.NumberFormat.Format = "#,##0";
                }
                else
                {
                    cell.Value = value?.ToString() ?? string.Empty;
                }
            }
        }

        // save and return to file
        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;

        return new RemoteStreamContent(
            output,
            fileName,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    // Helper method auto get value property
    private object GetPropertyValue(object obj, string propertyName)
    {
        if (obj == null) return null;

        var propertyInfo = obj.GetType().GetProperty(propertyName);
        if (propertyInfo == null) return null;

        return propertyInfo.GetValue(obj);
    }

    // Column Mapping for Warranty
    private Dictionary<int, string> GetWarrantyColumnMapping()
    {
        return new Dictionary<int, string>
        {
            { 2,  nameof(SaleOrderExportSAPGICData.Process) },               // B - Warranty Process
            { 3,  nameof(SaleOrderExportSAPGICData.BuyerType) },             // C - Buyer Type
            { 4,  nameof(SaleOrderExportSAPGICData.BuyerShortName) },        // D - Buyer Short Name
            { 5,  nameof(SaleOrderExportSAPGICData.GICNo) },                 // E - GIC No
            { 6,  nameof(SaleOrderExportSAPGICData.GICDate) },               // F - GIC Date
            { 7,  nameof(SaleOrderExportSAPGICData.CostCenter) },            // G - Cost Center
            { 8,  nameof(SaleOrderExportSAPGICData.RefDoc) },                // H - Ref doc (WR No)
            { 9,  nameof(SaleOrderExportSAPGICData.MaterialType) },          // I - Material Type
            { 10, nameof(SaleOrderExportSAPGICData.ItemNo) },                // J - No
            { 11, nameof(SaleOrderExportSAPGICData.MaterialCode) },          // K - Material Code
            { 12, nameof(SaleOrderExportSAPGICData.Model) },                 // L - Model name
            { 13, nameof(SaleOrderExportSAPGICData.SAP_Code) },              // M - SAP Code
            { 14, nameof(SaleOrderExportSAPGICData.VAT) },                   // N - VAT
            { 15, nameof(SaleOrderExportSAPGICData.Spec1) },                 // O - Spec1
            { 16, nameof(SaleOrderExportSAPGICData.Qty) },                   // P - Qty
            { 17, nameof(SaleOrderExportSAPGICData.CustomerName) },          // Q - Customer Name
            { 18, nameof(SaleOrderExportSAPGICData.DamagedProduct) },        // R - Damaged Product
            { 19, nameof(SaleOrderExportSAPGICData.ProductSerialNo) },       // S - Product Serial No
            { 20, nameof(SaleOrderExportSAPGICData.MEVNSellingInvoiceNo) },  // T - MEVN selling invoice no
            { 21, nameof(SaleOrderExportSAPGICData.SONo) },                  // U - SO No
            { 22, nameof(SaleOrderExportSAPGICData.DPOQty) },                // V - SO Qty
            { 23, nameof(SaleOrderExportSAPGICData.UnitPrice) },            // W - Unit Price
            { 24, nameof(SaleOrderExportSAPGICData.Amount) },               // X - Amount
            { 25, nameof(SaleOrderExportSAPGICData.Note) },                 // Y - SO Item Note
            { 26, nameof(SaleOrderExportSAPGICData.SAPSONo) },              // Z - SAP SO No
            { 27, nameof(SaleOrderExportSAPGICData.SAPDONo) },              // AA - SAP DO No
            { 28, nameof(SaleOrderExportSAPGICData.SAPBillingNo) },         // AB - SAP Billing No
            { 29, nameof(SaleOrderExportSAPGICData.GIVNo) },                // AC - GIV No
            { 30, nameof(SaleOrderExportSAPGICData.GIVDate) },              // AD - GIV Date
            { 31, nameof(SaleOrderExportSAPGICData.InvoiceNo) },            // AE - Invoice No
            { 32, nameof(SaleOrderExportSAPGICData.InvoiceDate) },          // AF - Invoice Date
            { 33, nameof(SaleOrderExportSAPGICData.SAPLandingCost) },       // AG - SAP Landing Cost
            { 34, nameof(SaleOrderExportSAPGICData.AmountInLandingCost) },  // AH - Amount in SAP Landing Cost
            { 35, nameof(SaleOrderExportSAPGICData.PORNo) },                // AI - POR No
            { 36, nameof(SaleOrderExportSAPGICData.PRNo) },                 // AJ - PR No
            { 37, nameof(SaleOrderExportSAPGICData.ChangeNote) },           // AK - Change Note
        };
    }



    // Column Mapping for Internal
    private Dictionary<int, string> GetInternalColumnMapping()
    {
        return new Dictionary<int, string>
        {
            { 2,  nameof(SaleOrderExportSAPGICData.GICNo) },                 // B - GIC No
            { 3,  nameof(SaleOrderExportSAPGICData.GICDate) },               // C - GIC Date
            { 4,  nameof(SaleOrderExportSAPGICData.Process) },               // D - Process
            { 5,  nameof(SaleOrderExportSAPGICData.CostCenter) },            // E - Cost Center
            { 6,  nameof(SaleOrderExportSAPGICData.RefDoc) },                // F - Ref doc (EMG)
            { 7,  nameof(SaleOrderExportSAPGICData.MaterialType) },          // G - Material Type
            { 8,  nameof(SaleOrderExportSAPGICData.ItemNo) },                // H - Item No
            { 9,  nameof(SaleOrderExportSAPGICData.MaterialCode) },          // I - Material Code
            { 10, nameof(SaleOrderExportSAPGICData.MaterialGroup) },         // J - Material Group
            { 11, nameof(SaleOrderExportSAPGICData.Model) },                 // K - Model name
            { 12, nameof(SaleOrderExportSAPGICData.SAP_Code) },              // L - Material SAP Code
            { 13, nameof(SaleOrderExportSAPGICData.Spec1) },                 // M - Spec1
            { 14, nameof(SaleOrderExportSAPGICData.DPOQty) },                // N - GIC Qty
            { 15, nameof(SaleOrderExportSAPGICData.SONo) },                  // O - SO No
            { 16, nameof(SaleOrderExportSAPGICData.Qty) },                   // P - SO Qty
            { 17, nameof(SaleOrderExportSAPGICData.Note) },                  // Q - SO Item Note
            { 18, nameof(SaleOrderExportSAPGICData.GIVNo) },                 // R - GIV No
            { 19, nameof(SaleOrderExportSAPGICData.GIVDate) },               // S - GIV Date
            { 20, nameof(SaleOrderExportSAPGICData.ReservationNo) },         // T - Reservation No
            { 21, nameof(SaleOrderExportSAPGICData.SAPLandingCost) },        // U - SAP Landing Cost
            { 22, nameof(SaleOrderExportSAPGICData.AmountInLandingCost) },   // V - Amount in SAP Landing Cost
            { 23, nameof(SaleOrderExportSAPGICData.PORNo) },                 // W - POR No
            { 24, nameof(SaleOrderExportSAPGICData.PRNo) },                  // X - PR No
            { 25, nameof(SaleOrderExportSAPGICData.Location) },              // Y - Location (typo như trong SP)
            { 26, nameof(SaleOrderExportSAPGICData.SalePIC) },               // Z - Sales PIC
            { 27, nameof(SaleOrderExportSAPGICData.AssetClass) },            // AA - Asset Class
            { 28, nameof(SaleOrderExportSAPGICData.MainAssetCode) },         // AB - Main asset code
            { 29, nameof(SaleOrderExportSAPGICData.SubAssetCode) },          // AC - Sub-asset code
            { 30, nameof(SaleOrderExportSAPGICData.AssetName) },             // AD - Asset Name
        };
    }

    // Column Mapping for Internal
    private Dictionary<int, string> GetInternalUseChangeColumnMapping()
    {
        return new Dictionary<int, string>
        {
            { 2,  nameof(SaleOrderExportSAPGICData.GICNo) },                 // B - GIC No (*)
            { 3,  nameof(SaleOrderExportSAPGICData.GICDate) },               // C - GIC Date
            { 4,  nameof(SaleOrderExportSAPGICData.Process) },               // D - Process
            { 5,  nameof(SaleOrderExportSAPGICData.CostCenter) },            // E - Cost Center
            { 6,  nameof(SaleOrderExportSAPGICData.RefDoc) },                // F - Ref doc (SSG)
            { 7,  nameof(SaleOrderExportSAPGICData.MaterialType) },          // G - Material Type
            { 8,  nameof(SaleOrderExportSAPGICData.ItemNo) },                // H - No
            { 9,  nameof(SaleOrderExportSAPGICData.MaterialCode) },          // I - Material Code (*)
            { 10, nameof(SaleOrderExportSAPGICData.MaterialGroup) },         // J - Material Group
            { 11, nameof(SaleOrderExportSAPGICData.Model) },                 // K - Model name
            { 12, nameof(SaleOrderExportSAPGICData.SAP_Code) },              // L - Material SAP Code
            { 13, nameof(SaleOrderExportSAPGICData.Spec1) },                 // M - Spec1
            { 14, nameof(SaleOrderExportSAPGICData.DPOQty) },                // N - GIC Qty
            { 15, nameof(SaleOrderExportSAPGICData.SONo) },                  // O - SO No (*)
            { 16, nameof(SaleOrderExportSAPGICData.Qty) },                   // P - SO Qty
            { 17, nameof(SaleOrderExportSAPGICData.Note) },                  // Q - SO Item Note
            { 18, nameof(SaleOrderExportSAPGICData.GIVNo) },                 // R - GIV No
            { 19, nameof(SaleOrderExportSAPGICData.GIVDate) },               // S - GIV Date
            { 20, nameof(SaleOrderExportSAPGICData.ReservationNo) },         // T - Reservation No
            { 21, nameof(SaleOrderExportSAPGICData.SAPLandingCost) },        // U - SAP Landing Cost
            { 22, nameof(SaleOrderExportSAPGICData.AmountInLandingCost) },   // V - Amount in SAP Landing Cost
            { 23, nameof(SaleOrderExportSAPGICData.PORNo) },                 // W - POR No (*)
            { 24, nameof(SaleOrderExportSAPGICData.PRNo) },                  // X - PR No (*)
            { 25, nameof(SaleOrderExportSAPGICData.Location) },              // Y - Location (*)
            { 26, nameof(SaleOrderExportSAPGICData.SalePIC) },               // Z - Sale PIC (*)
            
        };
    }


    private Dictionary<int, string> GetSponsorColumnMapping()
    {
        return new Dictionary<int, string>
        {
            { 2,  nameof(SaleOrderExportSAPGICData.GICNo) },                 // B - GIC No
            { 3,  nameof(SaleOrderExportSAPGICData.GICDate) },               // C - GIC Date
            { 4,  nameof(SaleOrderExportSAPGICData.CostCenter) },            // D - Cost Center
            { 5,  nameof(SaleOrderExportSAPGICData.SalesOrg) },              // E - Sales org
            { 6,  nameof(SaleOrderExportSAPGICData.BuyerShortName) },        // F - Buyer
            { 7,  nameof(SaleOrderExportSAPGICData.BuyerSAPCode) },          // G - SAP Buyer code
            { 8,  nameof(SaleOrderExportSAPGICData.RefDoc) },                // H - Ref doc (Agreement No)
            { 9,  nameof(SaleOrderExportSAPGICData.MaterialType) },          // I - Material Type
            { 10, nameof(SaleOrderExportSAPGICData.ItemNo) },                // J - No
            { 11, nameof(SaleOrderExportSAPGICData.MaterialCode) },          // K - Material Code
            { 12, nameof(SaleOrderExportSAPGICData.Model) },                 // L - Model name
            { 13, nameof(SaleOrderExportSAPGICData.SAP_Code) },              // M - SAP Code
            { 14, nameof(SaleOrderExportSAPGICData.VAT) },                   // N - VAT
            { 15, nameof(SaleOrderExportSAPGICData.Spec1) },                 // O - Spec1
            { 16, nameof(SaleOrderExportSAPGICData.DPOQty) },                // P - Qty
            { 17, nameof(SaleOrderExportSAPGICData.OrderReason) },           // Q - Order reason
            { 18, nameof(SaleOrderExportSAPGICData.SONo) },                  // R - SO No
            { 19, nameof(SaleOrderExportSAPGICData.Qty) },                   // S - SO Qty
            { 20, nameof(SaleOrderExportSAPGICData.UnitPrice) },             // T - Unit Price
            { 21, nameof(SaleOrderExportSAPGICData.Amount) },                // U - Amount
            { 22, nameof(SaleOrderExportSAPGICData.Note) },                  // V - SO Item Note
            { 23, nameof(SaleOrderExportSAPGICData.SAPSONo) },               // W - SAP SO No
            { 24, nameof(SaleOrderExportSAPGICData.SAPDONo) },               // X - SAP DO No
            { 25, nameof(SaleOrderExportSAPGICData.SAPBillingNo) },          // Y - SAP Billing No
            { 26, nameof(SaleOrderExportSAPGICData.GIVNo) },                 // Z - GIV No
            { 27, nameof(SaleOrderExportSAPGICData.InvoiceNo) },             // AA - Invoice No
            { 28, nameof(SaleOrderExportSAPGICData.InvoiceDate) },           // AB - Invoice Date
            { 29, nameof(SaleOrderExportSAPGICData.SAPLandingCost) },        // AC - SAP Landing Cost
            { 30, nameof(SaleOrderExportSAPGICData.AmountInLandingCost) },   // AD - Amount in SAP Landing Cost
            { 31, nameof(SaleOrderExportSAPGICData.PORNo) },                 // AE - POR No
            { 32, nameof(SaleOrderExportSAPGICData.PRNo) },                  // AF - PR No
            { 33, nameof(SaleOrderExportSAPGICData.ChangeNote) },            // AG - Change Note
        };
    }


    private Dictionary<int, string> GetWriteOffColumnMapping()
    {
        return new Dictionary<int, string>
        {
            { 2,  nameof(SaleOrderExportSAPGICData.GICNo) },                 // B - GIC-WO No
            { 3,  nameof(SaleOrderExportSAPGICData.GICDate) },               // C - GIC-WO Date
            { 4,  nameof(SaleOrderExportSAPGICData.CostCenter) },            // D - Cost Center
            { 5,  nameof(SaleOrderExportSAPGICData.MaterialType) },          // E - Material Type
            { 6,  nameof(SaleOrderExportSAPGICData.ItemNo) },                // F - No
            { 7,  nameof(SaleOrderExportSAPGICData.MaterialCode) },          // G - Material Code
            { 8,  nameof(SaleOrderExportSAPGICData.Model) },                 // H - Model name
            { 9,  nameof(SaleOrderExportSAPGICData.SAP_Code) },              // I - SAP Code
            { 10, nameof(SaleOrderExportSAPGICData.Spec1) },                 // J - Spec1
            { 11, nameof(SaleOrderExportSAPGICData.DPOQty) },                // K - Qty
            { 12, nameof(SaleOrderExportSAPGICData.SONo) },                  // L - SO No
            { 13, nameof(SaleOrderExportSAPGICData.Qty) },                   // M - SO Qty
            { 14, nameof(SaleOrderExportSAPGICData.Note) },                  // N - SO Item Note
            { 15, nameof(SaleOrderExportSAPGICData.SOType) },                // O - SOType (Z2WO)
            { 16, nameof(SaleOrderExportSAPGICData.GIVNo) },                 // P - GIV No
            { 17, nameof(SaleOrderExportSAPGICData.GIVDate) },               // Q - GIV Date
            { 18, nameof(SaleOrderExportSAPGICData.SAPLandingCost) },        // R - SAP Landing Cost
            { 19, nameof(SaleOrderExportSAPGICData.AmountInLandingCost) },   // S - Amount in SAP Landing Cost
            { 20, nameof(SaleOrderExportSAPGICData.Disposed) },              // T - Disposed
            { 21, nameof(SaleOrderExportSAPGICData.DeliveryRemarks) },       // U - Delivery remarks
        };
    }
    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public virtual async Task<ExcelValidationResult<SaleOrderGICWriteOffExcelDto>> ValidateAndParseGICWriteOffAsync(IRemoteStreamContent file, string gicType)
    {
        var validator = _excelImportFactory.CreateValidator<SaleOrderGICWriteOffExcelDto>(ExcelImporters.SaleOrderGICWriteOff);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        foreach (var item in result.ListData)
        {
            item.RowData.SOType = gicType;
        }
        return result;
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public async Task ImportSOGICWriteOffAsync(ExcelValidationResult<SaleOrderGICWriteOffExcelDto> data)
    {
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<SaleOrderGICWriteOffExcelDto, SaleOrderSapImportCreateParams>(ExcelImporters.SaleOrderGICWriteOff);

        var context = new ExcelImportContext();
        var createdGuid = GuidGenerator.Create();

        List<SaleOrderSapImportCreateParams> createParams = (await Task.WhenAll(
        data.ListData.Select(async x =>
        {
            var item = await dataObjects.ConvertToCreateParamsAsync(x, context, default);
            if (item != null)
            {
                item.ImportKey = createdGuid;
                item.FileName = data.FileName;

            }
            return item;
        })
        )).Where(x => x != null)
        .ToList()!;

        await _saleOrdersSapImportManager.CreateBatchAsync(createParams);
        await WriteHistoryAsync(createParams);
        await UnitOfWorkManager.Current!.SaveChangesAsync();

        var error = await _saleOrderRepository.ImportSAPDataGICAsync(createdGuid, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }


    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public virtual async Task<ExcelValidationResult<SaleOrderGICWarrantyExcelDto>> ValidateAndParseGICWarrantyAsync(IRemoteStreamContent file, string gicType)
    {
        var validator = _excelImportFactory.CreateValidator<SaleOrderGICWarrantyExcelDto>(ExcelImporters.SaleOrderGICWarranty);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        foreach (var item in result.ListData)
        {
            item.RowData.SOType = gicType;
        }
        return result;
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public async Task ImportSOGICWarrantyAsync(ExcelValidationResult<SaleOrderGICWarrantyExcelDto> data)
    {
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<SaleOrderGICWarrantyExcelDto, SaleOrderSapImportCreateParams>(ExcelImporters.SaleOrderGICWarranty);

        var context = new ExcelImportContext();
        var createdGuid = GuidGenerator.Create();

        List<SaleOrderSapImportCreateParams> createParams = (await Task.WhenAll(
        data.ListData.Select(async x =>
        {
            var item = await dataObjects.ConvertToCreateParamsAsync(x, context, default);
            if (item != null)
            {
                item.ImportKey = createdGuid;
                item.FileName = data.FileName;

            }
            return item;
        })
        )).Where(x => x != null)
        .ToList()!;

        await _saleOrdersSapImportManager.CreateBatchAsync(createParams);
        await UnitOfWorkManager.Current!.SaveChangesAsync();
        await WriteHistoryAsync(createParams);

        var error = await _saleOrderRepository.ImportSAPDataGICAsync(createdGuid, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }

    }

    private async Task WriteHistoryAsync(List<SaleOrderSapImportCreateParams> createParams, bool? isUIChange = false, bool? isUI = false)
    {
        var soNoDistinct = createParams
                    .Where(x => !string.IsNullOrWhiteSpace(x.SONo))
                    .Select(x => x.SONo)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

        var saleOrders = await _saleOrderRepository.GetListAsync(
            x => soNoDistinct.Contains(x.SONo) && x.IsDeleted == false
        );

        var saleOrderDict = saleOrders.ToDictionary(
            x => x.SONo,
            StringComparer.OrdinalIgnoreCase
        );

        foreach (var soNo in soNoDistinct)
        {
            if (!saleOrderDict.TryGetValue(soNo, out var saleOrder))
                continue;

            if (isUIChange == true)
            {
                var status = HistoryActions.SaleOrder.ImportIUChange;
                var history = SetSOHistory(saleOrder.Id, status, "Info updated by IU Change");
                saleOrder.RecordAction(history);
            }
            else if (isUI == true)
            {
                var status = saleOrder.StatusCode == QuoteFlowStatuses.InProgress
                ? HistoryActions.SaleOrder.Update
                : HistoryActions.SaleOrder.ImportSAPData;
                string note = saleOrder.StatusCode == QuoteFlowStatuses.InProgress
                    ? "Update SO"
                    : "SO Closed by SAP Import";

                var history = SetSOHistory(saleOrder.Id, status, note);
                saleOrder.RecordAction(history);
            }
            else
            {
                var status = saleOrder.StatusCode == QuoteFlowStatuses.Closed
                ? HistoryActions.SaleOrder.Update
                : HistoryActions.SaleOrder.ImportSAPData;
                string note = saleOrder.StatusCode == QuoteFlowStatuses.Closed
                    ? "Update SO"
                    : "SO Closed by SAP Import";

                var history = SetSOHistory(saleOrder.Id, status, note);
                saleOrder.RecordAction(history);
            }
        }
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public virtual async Task<ExcelValidationResult<SaleOrderGICInternalUseExcelDto>> ValidateAndParseGICInternalUseAsync(IRemoteStreamContent file, string gicType)
    {
        var validator = _excelImportFactory.CreateValidator<SaleOrderGICInternalUseExcelDto>(ExcelImporters.SaleOrderGICInternalUse);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");
        foreach (var item in result.ListData)
        {
            item.RowData.SOType = gicType;
        }
        return result;
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public async Task ImportSOGICInternalUseAsync(ExcelValidationResult<SaleOrderGICInternalUseExcelDto> data)
    {
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<SaleOrderGICInternalUseExcelDto, SaleOrderSapImportCreateParams>(ExcelImporters.SaleOrderGICInternalUse);

        var context = new ExcelImportContext();
        var createdGuid = GuidGenerator.Create();

        List<SaleOrderSapImportCreateParams> createParams = (await Task.WhenAll(
        data.ListData.Select(async x =>
        {
            var item = await dataObjects.ConvertToCreateParamsAsync(x, context, default);
            if (item != null)
            {
                item.ImportKey = createdGuid;
                item.FileName = data.FileName;

            }
            return item;
        })
        )).Where(x => x != null)
        .ToList()!;

        await _saleOrdersSapImportManager.CreateBatchAsync(createParams);
        await UnitOfWorkManager.Current!.SaveChangesAsync();
        if (data.ListData.Select(x => x.RowData.GICProcess).Distinct().Count() == 1)
        {

            if (data.ListData.First().RowData.GICProcess == GICProcessCodes.ReservationNo)
            {


                var error = await _saleOrderRepository.ImportSAPDataGICAsync(createdGuid, _currentUser.Username!, _currentUser.FullName!);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    throw new UserFriendlyException($"{error}");
                }
            }
            else if (data.ListData.First().RowData.GICProcess == GICProcessCodes.Asset || data.ListData.First().RowData.GICProcess == GICProcessCodes.Tool)
            {

                var error = await _saleOrderRepository.ImportSAPDataGICInternalUseAsync(createdGuid, _currentUser.Username!, _currentUser.FullName!);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    throw new UserFriendlyException($"{error}");
                }
            }
        }
        else
        {

            throw new UserFriendlyException("Please ensure all rows have the same GIC Process.");
        }
        await WriteHistoryAsync(createParams, isUI: true);
    }
    [Authorize(QuoteFlowPermissions.SaleOrders.ImportInternalUseChange)]
    public virtual async Task<ExcelValidationResult<SaleOrderGICInternalUseChangeExcelDto>> ValidateAndParseGICInternalUseChangeAsync(IRemoteStreamContent file, string gicType)
    {
        var validator = _excelImportFactory.CreateValidator<SaleOrderGICInternalUseChangeExcelDto>(ExcelImporters.SaleOrderGICInternalUseChange);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");
        foreach (var item in result.ListData)
        {
            item.RowData.SOType = gicType;
        }
        return result;
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportInternalUseChange)]
    public async Task ImportSOGICInternalUseChangeAsync(ExcelValidationResult<SaleOrderGICInternalUseChangeExcelDto> data)
    {
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<SaleOrderGICInternalUseChangeExcelDto, SaleOrderSapImportCreateParams>(ExcelImporters.SaleOrderGICInternalUseChange);

        var context = new ExcelImportContext();
        var createdGuid = GuidGenerator.Create();

        List<SaleOrderSapImportCreateParams> createParams = (await Task.WhenAll(
        data.ListData.Select(async x =>
        {
            var item = await dataObjects.ConvertToCreateParamsAsync(x, context, default);
            if (item != null)
            {
                item.ImportKey = createdGuid;
                item.FileName = data.FileName;

            }
            return item;
        })
        )).Where(x => x != null)
        .ToList()!;

        await _saleOrdersSapImportManager.CreateBatchAsync(createParams);
        await WriteHistoryAsync(createParams, isUIChange: true);
        await UnitOfWorkManager.Current!.SaveChangesAsync();
        var error = await _saleOrderRepository.ImportInternalUseChangeDataGICAsync(createdGuid, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public virtual async Task<ExcelValidationResult<SaleOrderGICFOCExcelDto>> ValidateAndParseGICFOCAsync(IRemoteStreamContent file, string gicType)
    {
        var validator = _excelImportFactory.CreateValidator<SaleOrderGICFOCExcelDto>(ExcelImporters.SaleOrderGICFOC);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");
        foreach (var item in result.ListData)
        {
            item.RowData.SOType = gicType;
        }
        return result;
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ImportSAPSO)]
    public async Task ImportSOGICFOCAsync(ExcelValidationResult<SaleOrderGICFOCExcelDto> data)
    {
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<SaleOrderGICFOCExcelDto, SaleOrderSapImportCreateParams>(ExcelImporters.SaleOrderGICFOC);

        var context = new ExcelImportContext();
        var createdGuid = GuidGenerator.Create();

        List<SaleOrderSapImportCreateParams> createParams = (await Task.WhenAll(
        data.ListData.Select(async x =>
        {
            var item = await dataObjects.ConvertToCreateParamsAsync(x, context, default);
            if (item != null)
            {
                item.ImportKey = createdGuid;
                item.FileName = data.FileName;

            }
            return item;
        })
        )).Where(x => x != null)
        .ToList()!;

        await _saleOrdersSapImportManager.CreateBatchAsync(createParams);
        await WriteHistoryAsync(createParams);
        await UnitOfWorkManager.Current!.SaveChangesAsync();
        var error = await _saleOrderRepository.ImportSAPDataGICAsync(createdGuid, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
    }



    [Authorize(QuoteFlowPermissions.SaleOrders.ExportSAPData)]
    public async Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetSaleOrdersInput input)
    {
        var filterParams = ObjectMapper.Map<GetSaleOrdersInput, SaleOrderListExportSAPDataParams>(input);
        filterParams.Username = _currentUser.Username;
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        var items = await _saleOrderRepository.ExportSAPDataAsync(filterParams);


        var fileDescriptor = await _fileDescriptorRepository
           .FirstOrDefaultAsync(fd => fd.Name == "Template_SO_SAP.xlsx")
           ?? throw new UserFriendlyException("Template Excel not found.");

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        using var originalStream = new MemoryStream(templateBytes);


        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheet("Upload");

        int startRow = 5;


        for (int i = 0; i < items.Count; i++)
        {
            var d = items[i];
            var row = ws.Row(startRow + i);

            // --- Sale Order Information ---
            row.Cell(1).Value = d.SONo;
            row.Cell(2).Value = d.SaleOrderType;
            row.Cell(3).Value = d.SalesOrg;
            row.Cell(4).Value = d.DistributionChannel;
            row.Cell(5).Value = d.SoldTo;
            row.Cell(6).Value = d.ShipTo;
            row.Cell(7).Value = d.SoldToName1;
            row.Cell(8).Value = d.PartnerFunction;
            row.Cell(9).Value = d.SalesEmployeeCode1;
            row.Cell(10).Value = d.SalesEmployeeCode2;
            row.Cell(11).Value = d.SalesEmployeeCode3;
            row.Cell(12).Value = d.DPONo;
            row.Cell(13).Value = d.CustomerRefDate;
            row.Cell(14).Value = d.Incoterm1;
            row.Cell(15).Value = d.PricingDate;
            row.Cell(16).Value = d.RequestedDeliveryDate;
            row.Cell(17).Value = d.OrderReason;
            row.Cell(18).Value = d.DocumentCurrency;
            row.Cell(19).Value = d.ExchangeRate;
            row.Cell(20).Value = d.HeaderText01;
            row.Cell(21).Value = d.HeaderText02;
            row.Cell(22).Value = d.HeaderText03;

            // --- Sale Order Items information ---
            row.Cell(23).Value = d.ItemNo;
            row.Cell(24).Value = d.MaterialNo;
            row.Cell(25).Value = d.Qty;
            row.Cell(26).Value = ChangeMaterialTaxRate(d.MaterialTaxRate);
            row.Cell(27).Value = d.SalesUnit;
            row.Cell(28).Value = d.Description;
            row.Cell(29).Value = d.Plant;
            row.Cell(30).Value = d.StockName;
            row.Cell(31).Value = d.ValuationType;
            row.Cell(32).Value = d.CustomerReferenceitem;
            row.Cell(33).Value = d.ItemText01;
            row.Cell(34).Value = d.Amount1;
            row.Cell(35).Value = d.Amount2;
        }
        var worksheetToProcess = workbook.Worksheet("Import_SAP_SO");


        if (worksheetToProcess != null)
        {
            foreach (var cell in worksheetToProcess.CellsUsed().Where(c => c.HasFormula))
            {
                var formula = cell.FormulaA1;
                if (formula.StartsWith("@") && formula.Contains("UNIQUE"))
                {
                    cell.FormulaA1 = formula.Substring(1);
                }
            }
        }
        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;

        return new RemoteStreamContent(
            output,
            "SO_SAP_Export.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }
    protected virtual string? ChangeMaterialTaxRate(string? taxRate)
    {
        if (taxRate.IsNullOrWhiteSpace())
            return "0";
        if (taxRate == "0.00%")
            return "1";
        if (taxRate == "5.00%")
            return "2";
        if (taxRate == "8.00%")
            return "3";
        else if (taxRate == "10.00%")
            return "4";
        return null;


    }

    [Authorize(QuoteFlowPermissions.SaleOrders.AdjustDetailExtraFee)]
    public virtual async Task UpdateSODetailExtrafeeAsync(SODetailExtrafeeUpdateInput input)
    {
        var updateParams = ObjectMapper.Map<SODetailExtrafeeUpdateInput, SODetailExtrafeeUpdateParams>(input);
        updateParams.UserName = _currentUser.Username;
        updateParams.UserFullName = _currentUser.FullName;
        var error = await _saleOrderRepository.UpdateSODetailExtrafeeAsync(updateParams);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
    }

    [Authorize(QuoteFlowPermissions.SaleOrders.ExportReportData)]
    public async Task<IRemoteStreamContent> GetListSODataAsExcelFileAsync(GetSaleOrdersInput input)
    {
        var filterParams = ObjectMapper.Map<GetSaleOrdersInput, SaleOrderListExportSAPDataParams>(input);
        filterParams.Username = _currentUser.Username;
        filterParams.HasFullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();

        var items = await _saleOrderRepository.ExportSODataAsync(filterParams);

        var fileDescriptor = await _fileDescriptorRepository
           .FirstOrDefaultAsync(fd => fd.Name == "Template_SO_ExportData.xlsx")
           ?? throw new UserFriendlyException("Template Excel not found.");
        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);
        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;
        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheet("SO Report Template");

        int startRow = 2; // Dữ liệu bắt đầu từ A5
        int startCol = 1; // Cột A = 1

        // Thêm dòng nếu dữ liệu > 1
        if (items.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(items.Count - 1);
        }

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var row = ws.Row(startRow + i);
            int col = startCol;

            // --- SO Info ---
            row.Cell(col++).Value = item.SONo;                    // SO No.
            row.Cell(col++).Value = item.BuyerType;               // Buyer Type
            row.Cell(col++).Value = item.BuyerName;               // Buyer Name
            row.Cell(col++).Value = item.StockName;         // Storage location

            // --- Material Info ---
            row.Cell(col++).Value = item.GolfaCode;            // Material code
            row.Cell(col++).Value = item.SAP_Code;                 // SAP Code
            row.Cell(col++).Value = item.Model;               // Model name
            row.Cell(col++).Value = item.MaterialType;            // Material type
            row.Cell(col++).Value = item.Material_Group;           // Material Group
            row.Cell(col++).Value = item.Spec1;                   // Spec1
            row.Cell(col++).Value = item.Spec2;                   // Spec2
            row.Cell(col++).Value = item.Spec3;                   // Spec3
            row.Cell(col++).Value = item.SO_VAT.HasValue ? ((item.SO_VAT * 100).Value.ToString("0") + "%") : "KCT";                     // VAT
            row.Cell(col++).Value = item.Qty;                     // Qty
            row.Cell(col++).Value = item.Price;               // Unit Price (VND)
            row.Cell(col++).Value = item.ExtraFee;
            row.Cell(col++).Value = item.Amount;                  // Amount (VND)

            // --- DPO Info ---
            row.Cell(col++).Value = item.DPONo;                   // DPO No
            row.Cell(col++).Value = item.DPO_Date;       // DPO Date
            row.Cell(col++).Value = item.RequestedETA;  // Requested ETA

            // --- Customer Info ---
            row.Cell(col++).Value = item.CustomerTaxCode;         // Customer Tax code
            row.Cell(col++).Value = item.CustomerName;            // Customer Name

            // --- SPO Info ---
            row.Cell(col++).Value = item.SPOCode;                 // SPO Code
            row.Cell(col++).Value = item.SPODate;       // SPO Date
            row.Cell(col++).Value = item.SPOType;                 // SPO Type
            row.Cell(col++).Value = item.SPOName;                 // SPO Name
            row.Cell(col++).Value = item.PanelBuilder;            // Panel Builder
            row.Cell(col++).Value = item.MEContractor;            // M&E contractor
            row.Cell(col++).Value = item.MainContractor;          // Main contractor
            row.Cell(col++).Value = item.SIOEM;                   // SI/OEM
            row.Cell(col++).Value = item.InvestorEndUser;         // Investor/ End-user
            row.Cell(col++).Value = item.Trading;                 // Trading
            row.Cell(col++).Value = item.EUIndustry;                // Industry
            row.Cell(col++).Value = item.KeyAccount;              // Key account
            row.Cell(col++).Value = item.KeyAccountClassBuyer;         // Key account class
            row.Cell(col++).Value = item.SONote;                  // SO Note

            // --- SAP References ---
            row.Cell(col++).Value = item.SAPSONo;                 // SAP SO No
            row.Cell(col++).Value = item.SAPDONo;                 // SAP DO No
            row.Cell(col++).Value = item.SAPBillingNo;            // SAP Billing No
            row.Cell(col++).Value = item.SAPInvoice;            // SAP INV
            row.Cell(col++).Value = item.SAPInvoiceDate; // SAP INV date
        }

        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;
        return new RemoteStreamContent(
            output,
            "SO_SAP_Export.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

}
using QuoteFlow.Permissions;
using QuoteFlow.PSIs;
using QuoteFlow.PurchaseOrderDetails;
using QuoteFlow.PurchaseOrderDetails.ParameterObjects;
using QuoteFlow.PurchaseOrders.ParameterObjects;
using QuoteFlow.PurchaseOrders.PurchaseOrderDetails;
using QuoteFlow.PurchaseOrdersSapImports;
using QuoteFlow.PurchaseOrdersSapImports.Excel;
using QuoteFlow.PurchaseOrdersSapImports.ParameterObject;
using QuoteFlow.RequesterContexts;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using QuoteFlow.StockImportAllocations;
using QuoteFlow.SystemConfigurations;
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

namespace QuoteFlow.PurchaseOrders;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.PurchaseOrders.Default)]
public class PurchaseOrdersAppService : QuoteFlowAppService, IPurchaseOrdersAppService
{
    protected IPurchaseOrderRepository _purchaseOrderRepository;
    protected PurchaseOrderManager _purchaseOrderManager;
    protected PurchaseOrderDetailManager _purchaseOrderDetailManager;
    protected PurchaseOrdersSapImportManager _purchaseOrdersSapImportManager;
    protected IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
    protected readonly IEffectiveUserContext _currentUser;
    protected IStockImportAllocationRepository _stockImportAllocationRepository;
    protected IExcelImportFactory _excelImportFactory;
    protected ISystemConfigurationRepository _systemConfigurationRepository;
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    private readonly FileDescriptorAppService _fileDescriptorAppService;
    private readonly IPSIRepository _pSIRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public PurchaseOrdersAppService(
        IPurchaseOrderRepository purchaseOrderRepository,
        PurchaseOrderManager purchaseOrderManager,
        PurchaseOrderDetailManager purchaseOrderDetailManager,
        IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
        IEffectiveUserContext currentUser,
        IStockImportAllocationRepository stockImportAllocationRepository,
        IExcelImportFactory excelImportFactory,
        PurchaseOrdersSapImportManager purchaseOrdersSapImportManager,
        FileDescriptorAppService fileDescriptorAppService,
        IRepository<FileDescriptor, Guid> fileDescriptorRepository,
        ISystemConfigurationRepository systemConfigurationRepository,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderManager = purchaseOrderManager;
        _purchaseOrderDetailManager = purchaseOrderDetailManager;
        _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
        _currentUser = currentUser;
        _stockImportAllocationRepository = stockImportAllocationRepository;
        _purchaseOrdersSapImportManager = purchaseOrdersSapImportManager;
        _excelImportFactory = excelImportFactory;
        _fileDescriptorAppService = fileDescriptorAppService;
        _fileDescriptorRepository = fileDescriptorRepository;
        _systemConfigurationRepository = systemConfigurationRepository;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public virtual async Task<PagedResultDto<PurchaseOrderDto>> GetListAsync(GetPurchaseOrdersInput input)
    {
        var filterParams = ObjectMapper.Map<GetPurchaseOrdersInput, PurchaseOrderFilterParams>(input);
        if (!string.IsNullOrEmpty(input.StatusCode) && input.StatusCode == QuoteFlowStatuses.InProgress)
        {
            filterParams.StatusCodes = [input.StatusCode, QuoteFlowStatuses.Draft];
        }
        else
        {
            filterParams.StatusCodes = [input.StatusCode ?? ""];
        }

        var totalCount = await _purchaseOrderRepository.GetCountAsync(filterParams);
        var items = await _purchaseOrderRepository.GetListAsync(filterParams);

        return new PagedResultDto<PurchaseOrderDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<PurchaseOrder>, List<PurchaseOrderDto>>(items)
        };
    }

    public virtual async Task<PurchaseOrderDto> GetAsync(Guid id)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetAsync(id);

        if (purchaseOrder.IsDeleted == true)
        {
            throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(PurchaseOrder), id);
        }

        return ObjectMapper.Map<PurchaseOrder, PurchaseOrderDto>(purchaseOrder);
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var error = await _purchaseOrderRepository.DeletePOAsync(id, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.DeleteItem)]
    public virtual async Task DeleteDetailAsync(List<Guid> id)
    {
        var error = await _purchaseOrderRepository.DeletePODetailAsync(id, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Create)]
    public virtual async Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto input)
    {
        var createParams = ObjectMapper.Map<PurchaseOrderCreateDto, PurchaseOrderCreateParams>(input);
        var purchaseOrder = await _purchaseOrderManager.CreateAsync(
        createParams
        );

        return ObjectMapper.Map<PurchaseOrder, PurchaseOrderDto>(purchaseOrder);
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Edit)]
    public virtual async Task<PurchaseOrderDto> UpdateAsync(Guid id, PurchaseOrderUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<PurchaseOrderUpdateDto, PurchaseOrderUpdateParams>(input);
        var purchaseOrder = await _purchaseOrderManager.UpdateAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<PurchaseOrder, PurchaseOrderDto>(purchaseOrder);
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Edit)]
    public virtual async Task UpdateSendToSupplierAsync(Guid id, bool sendToSupplier, string? concurrencyStamp = null)
    {
        await _purchaseOrderManager.UpdateSendToSupplierAsync(
            id,
            sendToSupplier,
            concurrencyStamp
        );
    }

    public virtual async Task<decimal?> GetNewPriceAsync(string golfaCode, string model, string? accountNo)
    {
        var getNewPrice = await _purchaseOrderDetailRepository.GetNewPriceAsync(golfaCode, model, accountNo);
        return getNewPrice;
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Edit)]
    public virtual async Task<PurchaseOrderDetailDto> UpdateDetailAsync(Guid id, PurchaseOrderDetailUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<PurchaseOrderDetailUpdateDto, PurchaseOrderDetailUpdateParams>(input);
        var purchaseOrder = await _purchaseOrderDetailManager.UpdateQtyAsync(
            id,
            updateParams
        );

        return ObjectMapper.Map<PurchaseOrderDetail, PurchaseOrderDetailDto>(purchaseOrder);
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Create)]
    public virtual async Task<List<PurchaseOrderListDetailDPODto>> GetListDetailDPOAsync(GetListDetailDPOsInput input)
    {
        var filterParams = ObjectMapper.Map<GetListDetailDPOsInput, PurchaseOrderGetListDetailDPOParams>(input);
        var items = await _purchaseOrderRepository.GetListAddDetailDPOAsync(filterParams);
        return ObjectMapper.Map<List<PurchaseOrderListDetailDPO>, List<PurchaseOrderListDetailDPODto>>(items);
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Create)]
    public virtual async Task<List<PurchaseOrderListDetailWarningStockDto>> GetListDetailWarningStockAsync(GetListDetailDPOsInput input)
    {
        var filterParams = ObjectMapper.Map<GetListDetailDPOsInput, PurchaseOrderGetListDetailDPOParams>(input);
        var items = await _purchaseOrderRepository.GetListAddDetailWarningStockAsync(filterParams);
        return ObjectMapper.Map<List<PurchaseOrderListDetailWarningStock>, List<PurchaseOrderListDetailWarningStockDto>>(items);

    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Create)]
    public virtual async Task<List<PurchaseOrderListDetailFOCDto>> GetListDetailFOCAsync(GetListDetailDPOsInput input)
    {
        var filterParams = ObjectMapper.Map<GetListDetailDPOsInput, PurchaseOrderGetListDetailDPOParams>(input);
        var items = await _purchaseOrderRepository.GetListAddDetailFOCAsync(filterParams);
        return ObjectMapper.Map<List<PurchaseOrderListDetailFOC>, List<PurchaseOrderListDetailFOCDto>>(items);
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Create)]
    public async Task CreateDetailDPOASync(List<PurchaseOrderAddedDetailDPODto> input)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
        {
            var filterParams = ObjectMapper.Map<List<PurchaseOrderAddedDetailDPODto>, List<PurchaseOrderAddedDetailDPOParams>>(input);
            foreach (var item in filterParams)
            {
                item.UserFullName = _currentUser.FullName!;
                item.UserName = _currentUser.Username!;
            }

            var errorMes = await _purchaseOrderRepository.CreatePODetailFromDPOAsync(filterParams);
            if (!string.IsNullOrWhiteSpace(errorMes))
            {
                throw new UserFriendlyException(errorMes);
            }
            // await _purchaseOrderManager.UpdateStatusInProcessAsync(input[0].POId);
        }
    }

    public async Task<PagedResultDto<PurchaseOrderLinkedDPODto>> GetListLinkedDPOAsync(Guid poDetailId)
    {
        var items = await _purchaseOrderRepository.GetListLinkedlDPOAsync(poDetailId);

        return new PagedResultDto<PurchaseOrderLinkedDPODto>
        {
            TotalCount = items.Count,
            Items = ObjectMapper.Map<List<PurchaseOrderLinkedDPO>, List<PurchaseOrderLinkedDPODto>>(items)
        };
    }

    public async Task<PagedResultDto<PurchaseOrderListQtyImportedDto>> GetListQtyImportedAsync(Guid pODetailId, bool? isReceipt)
    {
        var items = await _stockImportAllocationRepository.GetAllocationsByPODetailIdAsync(pODetailId, isReceipt);

        return new PagedResultDto<PurchaseOrderListQtyImportedDto>
        {
            TotalCount = items.Count,
            Items = ObjectMapper.Map<List<PurchaseOrderListQtyImported>, List<PurchaseOrderListQtyImportedDto>>(items)
        };
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.ImportSAPPO)]
    public virtual async Task<ExcelValidationResult<PurchaseOrdersSapImportsExcelDto>> ValidateAndParseAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<PurchaseOrdersSapImportsExcelDto>(ExcelImporters.PurchaseOrders);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");
        var fileName = file.FileName ?? "";
        return result;
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.ImportSAPPO)]
    public async Task ImportPOAsync(ExcelValidationResult<PurchaseOrdersSapImportsExcelDto> data)
    {
        var dataObjects = _excelImportFactory.CreateCreateParamsConverter<PurchaseOrdersSapImportsExcelDto, PurchaseOrderSapImportCreateParams>(ExcelImporters.PurchaseOrders);

        var context = new ExcelImportContext();
        var createdGuid = GuidGenerator.Create();

        List<PurchaseOrderSapImportCreateParams> createParams = (await Task.WhenAll(
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

        await _purchaseOrdersSapImportManager.CreateBatchAsync(createParams);
        await UnitOfWorkManager.Current!.SaveChangesAsync();
        var error = await _purchaseOrderRepository.ImportSAPDataAsync(createdGuid, _currentUser.Username!, _currentUser.FullName!);
        if (!string.IsNullOrWhiteSpace(error))
        {
            throw new UserFriendlyException($"{error}");
        }
    }

    public async Task<PagedResultDto<PurchaseOrderListDetailDto>> GetListDetailAsync(Guid pOId)
    {
        var items = await _purchaseOrderRepository.GetListDetailAsync(pOId);
        return new PagedResultDto<PurchaseOrderListDetailDto>
        {
            TotalCount = items.Count,
            Items = ObjectMapper.Map<List<PurchaseOrderListDetail>, List<PurchaseOrderListDetailDto>>(items)
        };

    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.Edit)]
    public async Task<PurchaseOrderDetailDto> UpdateRequestInfoAsync(Guid id, PurchaseOrderDetailUpdateRequestInfoDto input)
    {
        var update = await _purchaseOrderDetailManager.UpdateRequestInfoAsync(id, input.RequestETA, input.Customer, input.Urgent, input.ConcurrencyStamp);

        return ObjectMapper.Map<PurchaseOrderDetail, PurchaseOrderDetailDto>(update);
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.ExportPOSAP)]
    public async Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetPurchaseOrdersInput input)
    {

        var filterParams = ObjectMapper.Map<GetPurchaseOrdersInput, PurchaseOrderFilterParams>(input);
        if (!string.IsNullOrEmpty(input.StatusCode) && input.StatusCode == QuoteFlowStatuses.InProgress)
        {
            filterParams.StatusCodes = [input.StatusCode, QuoteFlowStatuses.Draft];
        }
        else
        {
            filterParams.StatusCodes = [input.StatusCode ?? ""];
        }
        filterParams.Username = _currentUser.Username;
        var items = await _purchaseOrderRepository.GetListSAPDataAsync(filterParams);


        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "PO_SAP_Export.xlsx")
            ?? throw new UserFriendlyException("Template Excel not found.");

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);


        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();
        int startRow = 7;


        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var row = ws.Row(startRow + i);

            row.Cell(2).Value = item.SupplierCode;
            row.Cell(3).Value = item.PONo;
            row.Cell(4).Value = item.CompanyCode;
            row.Cell(5).Value = item.Plant;
            row.Cell(6).Value = item.PurchasingOrg;
            row.Cell(7).Value = item.PurchasingGroup;
            row.Cell(8).Value = item.Currency;
            row.Cell(9).Value = item.OurRef;
            row.Cell(10).Value = item.YourReference;
            row.Cell(11).Value = item.Incoterm;
            row.Cell(12).Value = item.IncoLocation;
            row.Cell(13).Value = item.PaymentTerm;
            row.Cell(14).Value = item.HeaderNote;
            row.Cell(15).Value = item.ItemNo;
            row.Cell(16).Value = item.GolfaCode;
            row.Cell(17).Value = item.MaterialDescription;
            row.Cell(18).Value = item.Qty;
            row.Cell(19).Value = item.UoM;
            row.Cell(20).Value = item.Price;
            row.Cell(21).Value = item.RequestETA;
            row.Cell(22).Value = item.TrackingNumber;
            row.Cell(23).Value = item.ValuationType;
            row.Cell(24).Value = item.TaxCode;
            row.Cell(25).Value = "1051"; // StorageLocation
            row.Cell(26).Value = item.IssuingStorageLocation;
            row.Cell(27).Value = item.Spec1;

            row.Cell(28).Value = item.ExternalPONo;
            row.Cell(29).Value = item.DeliveryText;
        }

        var worksheetToProcess = workbook.Worksheet("Import_SAP_PO");


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
            "SAP_Data.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.ExportStandard)]
    public async Task<IRemoteStreamContent> GetStandardPricePOFileAsync(Guid poId)
    {
        var item = await _purchaseOrderRepository.GetWithDetailsNoTrackingAsync(poId);
        var lvsDaysCfg = await _systemConfigurationRepository.FirstOrDefaultAsync(x => x.CfgKey == "PO.LVS_RequestedETA_Days");
        var faDaysCfg = await _systemConfigurationRepository.FirstOrDefaultAsync(x => x.CfgKey == "PO.FA_RequestedETA_Days");
        var faWeekDayCfg = await _systemConfigurationRepository.FirstOrDefaultAsync(x => x.CfgKey == "PO.FA_RequestedETA_WeekDay");
        var lvsWeekDayCfg = await _systemConfigurationRepository.FirstOrDefaultAsync(x => x.CfgKey == "PO.LVS_RequestedETA_WeekDay");
        var poDetails = item.PurchaseOrderDetails.Select(d => new
        {
            d.GolfaCode,
            d.Model,
            d.Qty,
            d.Price,
            d.Spec1,
            d.AccountNo,
            d.Amount,
            d.RequestETA,
            d.Note,
            d.Customer,
            d.Urgent,
            d.PODetailCode
        }).OrderBy(x => int.TryParse(x.PODetailCode, out var num) ? num : int.MaxValue).ToList();

        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "PO_Standard_Export.xlsx")
            ?? throw new UserFriendlyException("Template Excel not found.");

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        using var originalStream = new MemoryStream(templateBytes);


        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();
        ws.Cell("C8").Value = item.SupplierBU!.Supplier.FullName;
        ws.Cell("C9").Value = item.SupplierBU!.SupplierAddress;
        ws.Cell("C13").Value = item.SupplierBU!.Contact;



        ws.Cell("I8").Value = item.PONo;
        ws.Cell("I9").Value = item.PODate;
        ws.Cell("I11").Value = item.Currency;
        ws.Cell("I12").Value = item.SupplierBUCode;

        int startRow = 19;


        if (poDetails.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(poDetails.Count - 1);
        }

        for (int i = 0; i < poDetails.Count; i++)
        {

            var d = poDetails[i];
            // var eta = GetEta(item.PODate, item.MaterialType!, d.RequestETA);
            var row = ws.Row(startRow + i);
            row.Cell(2).Value = d.PODetailCode;
            row.Cell(3).Value = d.GolfaCode;    // C
            row.Cell(4).Value = d.Model;        // D
            row.Cell(5).Value = d.Spec1;       // E
            row.Cell(6).Value = d.AccountNo;
            row.Cell(7).Value = d.Qty;          // G
            row.Cell(8).Value = d.Price;        // H
            row.Cell(9).Value = d.Amount;       // I
            //row.Cell(10).Value = GetEta(
            //        item.PODate,
            //        item.MaterialType!,
            //        int.Parse(faDaysCfg.CfgValue),
            //        int.Parse(lvsDaysCfg.CfgValue),
            //        int.Parse(faWeekDayCfg.CfgValue),
            //        int.Parse(lvsWeekDayCfg.CfgValue)
            //    );
            row.Cell(10).Value = d.RequestETA;
            row.Cell(11).Value = d.Customer;
            row.Cell(12).Value = d.Urgent == true ? "URG" : "";
        }

        var totalQty = poDetails.Sum(d => d.Qty);
        var totalAmount = poDetails.Sum(d => d.Amount);

        int totalRow = startRow + poDetails.Count + 1;


        ws.Cell(totalRow, 7).Value = totalQty;

        ws.Cell(totalRow, 9).Value = totalAmount;

        ws.Cell(startRow + poDetails.Count + 4, 4).Value = item.SupplierBU.PaymentDescription;
        ws.Cell(startRow + poDetails.Count + 5, 4).Value = item.SupplierBU.INCOTerm;

        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;

        return new RemoteStreamContent(
            output,
            "PO_Standard_Export.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.ExportListPO)]
    public async Task<IRemoteStreamContent> GetListPOExportAsync(GetPurchaseOrdersInput input)
    {
        var filterParams = ObjectMapper.Map<GetPurchaseOrdersInput, PurchaseOrderFilterParams>(input);
        if (!string.IsNullOrEmpty(input.StatusCode) && input.StatusCode == QuoteFlowStatuses.InProgress)
        {
            filterParams.StatusCodes = [input.StatusCode, QuoteFlowStatuses.Draft];
        }
        else
        {
            filterParams.StatusCodes = [input.StatusCode ?? null];
        }
        filterParams.Username = _currentUser.Username;

        // Lấy danh sách dữ liệu
        var items = await _purchaseOrderRepository.GetListPOAsync(filterParams);

        // Lấy template file Excel
        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "PO_List_Export.xlsx")
            ?? throw new UserFriendlyException("Template Excel not found.");

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();

        int startRow = 5; // Dữ liệu bắt đầu từ A5
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

            // --- Materials information ---
            row.Cell(col++).Value = item.SupplierCode;
            row.Cell(col++).Value = item.SupplierBUCode;
            row.Cell(col++).Value = item.MaterialType;
            row.Cell(col++).Value = item.Material_Group;

            // --- PO information ---
            row.Cell(col++).Value = item.StatusCode;
            row.Cell(col++).Value = item.ItemStatus;
            row.Cell(col++).Value = item.POType;
            row.Cell(col++).Value = item.PODate;
            row.Cell(col++).Value = item.PONo;
            row.Cell(col++).Value = item.PODetailCode;
            row.Cell(col++).Value = item.GolfaCode;
            row.Cell(col++).Value = item.Model;
            row.Cell(col++).Value = item.Spec1;
            row.Cell(col++).Value = item.POSAPNo;
            row.Cell(col++).Value = item.POSAPDate;

            // --- Quantity information ---
            row.Cell(col++).Value = item.Qty;
            row.Cell(col++).Value = item.QtyLocked;
            row.Cell(col++).Value = item.FreeQty;
            row.Cell(col++).Value = item.ReceiptQty;
            row.Cell(col++).Value = item.Remaining;

            // --- Price information ---
            row.Cell(col++).Value = item.AccountNo;
            row.Cell(col++).Value = item.Price;
            row.Cell(col++).Value = item.AmountQty;
            row.Cell(col++).Value = item.AmountLocked;

            row.Cell(col++).Value = item.AmountFreeQty;
            row.Cell(col++).Value = item.AmountRecipt;
            row.Cell(col++).Value = item.AmountRemaining;
            row.Cell(col++).Value = item.Currency;

            // --- Cargo information ---
            row.Cell(col++).Value = item.ReferenceLeadTime;
            row.Cell(col++).Value = item.DeliveryDueDate;
            row.Cell(col++).Value = item.MachineNumber;
            row.Cell(col++).Value = item.MEVNRequest;
            row.Cell(col++).Value = item.MEVNAddedRequest;
            row.Cell(col++).Value = item.STCReply;
            row.Cell(col++).Value = item.Note;
        }

        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;

        return new RemoteStreamContent(
            output,
            "PO_List_Export.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.PODataReport)]
    public async Task<IRemoteStreamContent> GetListPODataReportAsync(GetPurchaseOrdersInput input)
    {
        var filterParams = ObjectMapper.Map<GetPurchaseOrdersInput, PurchaseOrderFilterParams>(input);
        if (!string.IsNullOrEmpty(input.StatusCode) && input.StatusCode == QuoteFlowStatuses.InProgress)
        {
            filterParams.StatusCodes = [input.StatusCode, QuoteFlowStatuses.Draft];
        }
        else
        {
            filterParams.StatusCodes = [input.StatusCode ?? null];
        }
        filterParams.Username = _currentUser.Username;

        // Lấy dữ liệu
        var items = await _purchaseOrderRepository.GetListPODataReportAsync(filterParams);

        // Lấy file template
        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "PO_Data_Export.xlsx")
            ?? throw new UserFriendlyException("Template Excel not found.");

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();

        int startRow = 5; // bắt đầu từ A5
        int startCol = 1; // cột A

        if (items.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(items.Count - 1);
        }

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var row = ws.Row(startRow + i);
            int col = startCol;

            // --- Materials information ---
            row.Cell(col++).Value = item.SupplierCode;
            row.Cell(col++).Value = item.SupplierBUCode;
            row.Cell(col++).Value = item.MaterialType;
            row.Cell(col++).Value = item.Material_Group;

            // --- PO information ---
            row.Cell(col++).Value = item.StatusCode;
            row.Cell(col++).Value = item.ItemStatus;
            row.Cell(col++).Value = item.POType;
            row.Cell(col++).Value = item.PODate;
            row.Cell(col++).Value = item.PONo;
            row.Cell(col++).Value = item.PODetailCode;
            row.Cell(col++).Value = item.GolfaCode;
            row.Cell(col++).Value = item.Model;
            row.Cell(col++).Value = item.Spec1;
            row.Cell(col++).Value = item.POSAPNo;


            // --- Quantity information ---
            row.Cell(col++).Value = item.Qty;
            row.Cell(col++).Value = item.ReceiptQty;
            row.Cell(col++).Value = item.Remaining;

            // --- Price information ---
            row.Cell(col++).Value = item.AccountNo;
            row.Cell(col++).Value = item.Price;
            row.Cell(col++).Value = item.AmountQty;
            row.Cell(col++).Value = item.Currency;
            row.Cell(col++).Value = item.AmountVNDEquivalent;

            // --- Cargo information ---
            row.Cell(col++).Value = item.ReferenceLeadTime;
            row.Cell(col++).Value = item.MachineNumber;

            // --- Invoice information ---
            row.Cell(col++).Value = item.InvoiceNo;
            row.Cell(col++).Value = item.Invoice_Qty;
            row.Cell(col++).Value = item.ATA;
            row.Cell(col++).Value = item.ATD;
            row.Cell(col++).Value = item.WHArrivalDate;
            row.Cell(col++).Value = item.StockConfirmedDate;

            // --- PO  Item note ---
            row.Cell(col++).Value = item.Note;
        }

        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;

        return new RemoteStreamContent(
            output,
            "PO_Data_Export.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    public async Task<List<PurchaseOrderDpoNoteDto>> GetDpoNotesAsync(Guid poDetailId)
    {
        return await _purchaseOrderRepository.GetDpoNotesAsync(poDetailId);
    }

    [Authorize(QuoteFlowPermissions.PurchaseOrders.ExportFASCM)]
    public async Task<IRemoteStreamContent> GetFASCMPOFileAsync(Guid poId)
    {
        var item = await _purchaseOrderRepository.GetWithDetailsNoTrackingAsync(poId);

        var poDetails = item.PurchaseOrderDetails.Select(d => new
        {
            d.PODetailCode,
            d.GolfaCode,
            d.Model,
            d.Spec1,
            d.RequestETA,
            d.AccountNo,
            d.Qty,
            d.Price,
            d.Amount,
            d.Urgent,
            d.Customer
        }).OrderBy(x => int.TryParse(x.PODetailCode, out var code) ? code : int.MaxValue).ToList();

        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "PO_FASCM_Export.xlsx")
            ?? throw new UserFriendlyException("Template Excel not found.");

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        using var originalStream = new MemoryStream(templateBytes);


        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();
        ws.Cell("B14").Value = "1";
        var poNo = item.PONo;
        var poDate = item.PODate;
        if (poNo.StartsWith("VNFS-"))
        {
            poNo = poNo.Substring(5);
        }
        ws.Cell("C14").Value = poNo;
        ws.Cell("D14").Value = item.SupplierBU!.FASCMBuyerCode;
        ws.Cell("E14").Value = item.SupplierBU!.FASCMVendorCode;
        ws.Cell("F14").Value = item.Currency;
        ws.Cell("H14").Value = item.SupplierBU.FASCMSectionCode;
        ws.Cell("I14").Value = item.SupplierBU.FASCMPaymentTerm;

        int startRow = 20;

        if (poDetails.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(poDetails.Count - 1);
        }

        for (int i = 0; i < poDetails.Count; i++)
        {

            var d = poDetails[i];
            var row = ws.Row(startRow + i);
            row.Cell(2).Value = "1";
            row.Cell(3).Value = int.Parse(d.PODetailCode); ;    // C
            row.Cell(4).Value = poDate?.ToString("yyyyMMdd");        // D
            row.Cell(5).Value = item.SupplierBU.FASCMConsigneeCode;
            row.Cell(6).Value = d.GolfaCode;          // G
            row.Cell(7).Value = d.Model;        // H
            row.Cell(8).Value = d.Spec1;
            row.Cell(11).Value = d.RequestETA?.ToString("yyyyMMdd");       // I
            row.Cell(12).Value = d.AccountNo;
            row.Cell(13).Value = d.Qty;
            row.Cell(14).Value = d.Price;
            row.Cell(15).Value = d.Amount;
            row.Cell(16).Value = item.SupplierBU.FASCMFreightMethod;
            row.Cell(17).Value = item.SupplierBU.FASCMDeliveryTerms;
            row.Cell(18).Value = item.SupplierBU.FASCMPlaceOfDeliveryTerms;
            row.Cell(19).Value = item.SupplierBU.FASCMShippingMarkCode;
            //row.Cell(20).Value = item.SupplierBU.FASCMPlaceOfDeliveryTerms;
        }

        var output = new MemoryStream();
        workbook.SaveAs(output);
        output.Position = 0;

        return new RemoteStreamContent(
            output,
            "PO_FASCM_Export.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }
}
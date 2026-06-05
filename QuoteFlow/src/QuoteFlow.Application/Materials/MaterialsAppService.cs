using ClosedXML.Excel;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using QuoteFlow.ApprovalRoutes;
using QuoteFlow.BuyerAccess;
using QuoteFlow.HistoryTrackings;
using QuoteFlow.HistoryTrackings.ParameterObjects;
using QuoteFlow.Materials.Events;
using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
using QuoteFlow.Materials.MaterialImport.MaterialFactory;
using QuoteFlow.Materials.MaterialImport.MaterialSAP;
using QuoteFlow.Materials.MaterialImport.MaterialStatus;
using QuoteFlow.Materials.MaterialStocks;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;
using QuoteFlow.Materials.MaterialStocks.ParameterObjects;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.Permissions;
using QuoteFlow.RequesterContexts;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.Flagging;
using QuoteFlow.Shared.Models;
using QuoteFlow.Shared.Utils;
using QuoteFlow.StockCategories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.Abp.Validation;
using Volo.FileManagement.Files;

namespace QuoteFlow.Materials;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.Materials.Default)]
public class MaterialsAppService : QuoteFlowAppService, IMaterialsAppService
{
    protected IMaterialRepository _materialRepository;
    protected MaterialManager _materialManager;
    protected IMaterialApprovalRequestRepository _materialApprovalRequestRepository;
    protected IHistoryTrackingRepository _historyTrackingRepository;
    protected MaterialApprovalRequestManager _materialApprovalRequestManager;
    protected IMaterialApprovalRequestDetailRepository _materialApprovalRequestDetailRepository;
    protected MaterialApprovalRequestDetailManager _materialApprovalRequestDetailManager;
    protected IExcelImportFactory _excelImportFactory;
    protected readonly ICurrentUser _currentUserFromToken;
    protected readonly IRequesterContext _currentUserFromHeader;
    protected readonly IIdentityUserRepository _identityUserRepository;
    protected IdentityUser? _currentUser;
    protected ILocalEventBus _localEventBus;
    protected IApprovalRouteRepository _approvalRouteRepository;
    protected IGuidGenerator _guidGenerator;
    protected readonly IFlaggingService<MaterialApprovalRequest, MaterialFlagsDto> _flaggingService;
    protected IEffectiveUserContext _currentFullUser;
    protected readonly ApprovalRouteManager _approvalRouteManager;
    protected readonly IStockCategoryRepository _stockCategoryRepository;
    protected readonly MaterialStockManager _materialStockManager;
    protected readonly MaterialStockLockShipmentManager _materialStockLockShipmentManager;
    private readonly FileDescriptorAppService _fileDescriptorAppService;
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    protected readonly IBuyerAccessService _buyerAccessService;
    protected readonly IUnitOfWorkManager _unitOfWorkManager;

    public MaterialsAppService(IMaterialRepository materialRepository, MaterialManager materialManager, IExcelImportFactory excelValidatorFactory, IMaterialApprovalRequestRepository materialApprovalRequestRepository, IMaterialApprovalRequestDetailRepository materialApprovalRequestDetailRepository, MaterialApprovalRequestManager materialApprovalRequestManager, MaterialApprovalRequestDetailManager materialApprovalRequestDetailManager, IIdentityUserRepository identityUserRepository, IRequesterContext currentUserFromHeader, ICurrentUser currentUserFromToken, IApprovalRouteRepository approvalRouteRepository, ILocalEventBus localEventBus, IFlaggingService<MaterialApprovalRequest, MaterialFlagsDto> flaggingService, IGuidGenerator guidGenerator, IEffectiveUserContext currentFullUser, ApprovalRouteManager approvalRouteManager, IStockCategoryRepository stockCategoryRepository, MaterialStockManager materialStockManager, FileDescriptorAppService fileDescriptorAppService, IRepository<FileDescriptor, Guid> fileDescriptorRepository, MaterialStockLockShipmentManager materialStockLockShipmentManager, IBuyerAccessService buyerAccessService, IHistoryTrackingRepository historyTrackingRepository, IUnitOfWorkManager unitOfWorkManager)
    {

        _materialRepository = materialRepository;
        _materialManager = materialManager;
        _excelImportFactory = excelValidatorFactory;
        _materialApprovalRequestRepository = materialApprovalRequestRepository;
        _materialApprovalRequestDetailRepository = materialApprovalRequestDetailRepository;
        _materialApprovalRequestManager = materialApprovalRequestManager;
        _materialApprovalRequestDetailManager = materialApprovalRequestDetailManager;

        _identityUserRepository = identityUserRepository;
        _currentUserFromHeader = currentUserFromHeader;
        _currentUserFromToken = currentUserFromToken;
        _approvalRouteRepository = approvalRouteRepository;
        _localEventBus = localEventBus;
        _flaggingService = flaggingService;
        _guidGenerator = guidGenerator;
        _currentFullUser = currentFullUser;
        _approvalRouteManager = approvalRouteManager;
        _stockCategoryRepository = stockCategoryRepository;
        _materialStockManager = materialStockManager;
        _fileDescriptorAppService = fileDescriptorAppService;
        _fileDescriptorRepository = fileDescriptorRepository;
        _materialStockLockShipmentManager = materialStockLockShipmentManager;
        _buyerAccessService = buyerAccessService;
        _historyTrackingRepository = historyTrackingRepository;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public virtual async Task<PagedResultDto<MaterialDto>> GetListAsync(GetMaterialsInput input)
    {
        var filterParams = ObjectMapper.Map<GetMaterialsInput, MaterialFilterParams>(input);
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        filterParams.ApplyMaterialTypeRestrictions(buyerAccess);

        var totalCount = await _materialRepository.GetCountAsync(filterParams);
        var items = await _materialRepository.GetListAsync(filterParams);
        return new PagedResultDto<MaterialDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Material>, List<MaterialDto>>(items)
        };
    }

    public virtual async Task<MaterialDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Material, MaterialDto>(await _materialRepository.GetAsync(id));
    }

    public virtual async Task<PagedResultDto<MaterialApprovalRequestDto>> GetListApprovalAsync(GetMaterialsApprovalInput input)
    {
        //_currentUser ??= await GetCurrentUserAsync();
        //var currenUser = _currentUser.UserName;
        var filterParams = ObjectMapper.Map<GetMaterialsApprovalInput, MaterialApprovalRequestFilterParams>(input);
        var items = await _materialApprovalRequestRepository.GetListAsync(filterParams);
        var count = await _materialApprovalRequestRepository.GetCountAsync(filterParams);
        var flags = await _flaggingService.CreateBulkFlagsAsync(items);
        var itemDtos = ObjectMapper.Map<List<MaterialApprovalRequest>, List<MaterialApprovalRequestDto>>(items);
        itemDtos.ForEach(i => i.Flags = flags[i.Id]);

        return new PagedResultDto<MaterialApprovalRequestDto>
        {
            TotalCount = count,
            Items = itemDtos
        };
    }

    public async Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetMaterialsInput input)
    {
        input.MaxResultCount = int.MaxValue; // Limit the number of records to export
        input.SkipCount = 0;
        var filterParams = ObjectMapper.Map<GetMaterialsInput, MaterialFilterParams>(input);
        //var totalCount = await _materialRepository.GetCountAsync(filterParams);
        var items = await _materialRepository.GetListAsync(filterParams);

        // 2. Get the template file
        var fileDescriptor = await _fileDescriptorRepository
            .FirstOrDefaultAsync(fd => fd.Name == "Template_M1U_Report.xlsx")
            ?? throw new UserFriendlyException("Template Excel not found.");

        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);
        // 3. Copy the template to a temporary stream
        using var originalStream = new MemoryStream(templateBytes);
        var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        // 4. Load workbook
        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.ElementAt(1);
        ws.Cell("A1").Clear();
        ws.Cell("A1").Value = "Export Material";
        ws.Cell("A1").Style.Font.FontSize = 18;
        ws.Cell("A1").Style.Font.FontColor = XLColor.Red;
        ws.Cell("A1").Style.Font.Bold = true;
        ws.Cell("A2").Value = DateTime.Now;

        int startRow = 4; // start from row 4
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

            row.Cell(col++).Value = item.MaterialStatus;

            row.Cell(col++).Value = (item.RegistrationDate == DateTime.MinValue || (item.RegistrationDate.ToString().Contains("9999"))) ? "" : item.RegistrationDate;
            row.Cell(col++).Value = (item.ValidFrom == DateTime.MinValue || (item.ValidFrom.ToString().Contains("9999"))) ? "" : item.ValidFrom;
            row.Cell(col++).Value = (item.ValidTo == DateTime.MinValue || (item.ValidTo.ToString().Contains("9999"))) ? "" : item.ValidTo;

            row.Cell(col++).Value = item.GolfaCode;
            row.Cell(col++).Value = item.Model;
            row.Cell(col++).Value = item.SAP_Code;
            row.Cell(col++).Value = item.Spec1;
            row.Cell(col++).Value = item.Spec2;
            row.Cell(col++).Value = item.Spec3;
            row.Cell(col++).Value = item.Spec4;
            row.Cell(col++).Value = item.Description_EN;
            row.Cell(col++).Value = item.Description_VN;
            row.Cell(col++).Value = item.MaterialType;
            row.Cell(col++).Value = item.Unit;
            row.Cell(col++).Value = item.MaterialClass;
            row.Cell(col++).Value = item.Material_SEC_Classification;
            row.Cell(col++).Value = item.Material_Group;
            row.Cell(col++).Value = item.SAPMatGroup;
            row.Cell(col++).Value = item.Product_Hierarchy;
            row.Cell(col++).Value = item.ProductHierarchyDescription;
            row.Cell(col++).Value = item.CountryOfOrigin;
            row.Cell(col++).Value = item.ReferenceLeadTime;
            row.Cell(col++).Value = item.WarrantyTime;
            row.Cell(col++).Value = item.InventoryCategory;
            row.Cell(col++).Value = item.CargoNote;
            row.Cell(col++).Value = item.Weight;
            row.Cell(col++).Value = item.Size;
            row.Cell(col++).Value = item.QRCode;
            row.Cell(col++).Value = item.Maxlot;
            row.Cell(col++).Value = item.StockWarning;
            row.Cell(col++).Value = item.VAT;
            row.Cell(col++).Value = item.HS_Code;
            row.Cell(col++).Value = item.SupplierCode;
            row.Cell(col++).Value = item.SupplierBUCode;
            row.Cell(col++).Value = item.Factory_Text;
            row.Cell(col++).Value = item.Input_Price;
            row.Cell(col++).Value = item.InputCurrency;
            row.Cell(col++).Value = item.INCOTERMS;
            row.Cell(col++).Value = item.EPA;
            row.Cell(col++).Value = item.ImportDuty;
            row.Cell(col++).Value = item.AppliedExchangeRate;
            row.Cell(col++).Value = item.LandedCost;
            row.Cell(col++).Value = item.MaxSalesOfferPrice;
            row.Cell(col++).Value = item.MaxMangerOfferPrice;
            row.Cell(col++).Value = item.Standard_Price;
            row.Cell(col++).Value = item.SellingPrice1;
            row.Cell(col++).Value = item.SellingPrice2;
            row.Cell(col++).Value = item.SellingPrice3;
            row.Cell(col++).Value = item.SellingPrice4;
            row.Cell(col++).Value = item.SellingPrice5;
        }


        var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
        outputStream.Position = 0;


        return new RemoteStreamContent(
            outputStream,
            "MaterialReport.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );

        //var memoryStream = new MemoryStream();
        //await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Material>, List<MaterialExportExcelDto>>(items));
        //memoryStream.Seek(0, SeekOrigin.Begin);

        //return new RemoteStreamContent(memoryStream, "MaterialReport.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public async Task<PagedResultDto<MaterialApprovalRequestDto>> GetListMyApprovalAsync(GetMaterialsApprovalInput input)
    {
        var filterParams = ObjectMapper.Map<GetMaterialsApprovalInput, MaterialApprovalRequestFilterParams>(input);
        var currentApprover = await GetCurrentUserAsync();
        var items = await _materialApprovalRequestRepository.GetListPendingAsync(filterParams, currentApprover.UserName);
        return new PagedResultDto<MaterialApprovalRequestDto>
        {
            TotalCount = items.Count,
            Items = ObjectMapper.Map<List<MaterialApprovalRequest>, List<MaterialApprovalRequestDto>>(items)
        };
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        await _materialRepository.DeleteAsync(id);
    }


    public virtual async Task<MaterialDto> CreateAsync(MaterialCreateDto input)
    {
        var createParams = ObjectMapper.Map<MaterialCreateDto, MaterialCreateParams>(input);
        var material = await _materialManager.CreateAsync(
        createParams
        );

        return ObjectMapper.Map<Material, MaterialDto>(material);
    }


    public virtual async Task<MaterialDto> UpdateAsync(Guid id, MaterialUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<MaterialUpdateDto, MaterialUpdateParams>(input);

        var material = await _materialManager.UpdateAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<Material, MaterialDto>(material);
    }

    public async Task<MaterialApprovalRequestDto> GetMaterialApprovalDetailAsync(Guid approvalId)
    {
        var item = await _materialApprovalRequestRepository.GetWithDetailAsync(approvalId);
        var dto = ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(item);
        if (dto.ImportType == MaterialImportType.M5U)
        {
            var materials = await _materialRepository.GetListWithDeactiveAsync(
            new(),
            x => new MaterialSupportInfo(x.Id)
            {
                ConcurrencyStamp = x.ConcurrencyStamp,
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                MaterialStatus = x.MaterialStatus,
                StockWarning = x.StockWarning
            });

            var materialLookup = materials
                .GroupBy(x => new { x.GolfaCode, x.Model }) // 1. Find duplicates
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(m => m.MaterialStatus == MaterialStatuses.Active || m.MaterialStatus == MaterialStatuses.Discontinued) // 2. Prioritize 'Active'
                          .First() // 3. Pick the best one
                );

            dto.MaterialApprovalDetails!.ForEach(mad =>
            {
                if (materialLookup.TryGetValue(new { mad.GolfaCode, mad.Model }, out var material))
                {
                    mad.CurrentStockWarning = material.StockWarning;
                }
            });
        }

        dto.Flags = await _flaggingService.CreateFlagsAsync(item);
        return dto;
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.UpdatePrice)]
    public virtual async Task<ExcelValidationResult<MaterialUpdatePriceImportDto>> ValidateAndParseUpdatePriceAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<MaterialUpdatePriceImportDto>(ExcelImporters.MaterialUpdatePrice);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.NewMaterial)]
    public virtual async Task<ExcelValidationResult<MaterialNewRegistrationImportDto>> ValidateAndParseNewRegistrationAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<MaterialNewRegistrationImportDto>(ExcelImporters.MaterialNewRegistration);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.UpdateMaterialWithoutPrice)]
    public virtual async Task<ExcelValidationResult<MaterialUpdateWithoutPriceImportDto>> ValidateAndParseUpdateWithoutPriceAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<MaterialUpdateWithoutPriceImportDto>(ExcelImporters.MaterialUpdateWithoutPrice);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.InventoryPlanning)]
    public virtual async Task<ExcelValidationResult<MaterialUpdateInventoryPlanImportDto>> ValidateAndParseUpdateInventoryPlanAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<MaterialUpdateInventoryPlanImportDto>(ExcelImporters.MaterialUpdateInventoryPlan);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.NewMaterial)]
    public async Task<MaterialApprovalRequestDto> ImportMaterialNewRegistrationAsync(ExcelValidationResult<MaterialNewRegistrationImportDto> data, string? note = null)
    {
        var approvalRequestCreateParams = ObjectMapper.Map<ExcelValidationResult<MaterialNewRegistrationImportDto>, MaterialApprovalRequestCreateParams>(data);
        approvalRequestCreateParams.Note = note;
        approvalRequestCreateParams.ImportType = MaterialImportType.M1U;
        var approvalRequestData = ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(await _materialApprovalRequestManager.CreateAsync(approvalRequestCreateParams));

        var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<MaterialNewRegistrationImportDto, MaterialApprovalRequestDetailCreateParams>(ExcelImporters.MaterialNewRegistration);

        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.ParentEntityId, approvalRequestData.Id);


        List<MaterialApprovalRequestDetailCreateParams> createParams = (await Task.WhenAll(
            data.ListData.Select(async x =>
            {
                var param = await createParamsConverters.ConvertToCreateParamsAsync(x, context, default);
                if (param != null)
                {
                    param.MaterialStatus = QuoteFlowStatuses.InProgress;
                    param.MaterialClass = x.RowData.MaterialClass;
                    param.SupplierBUId = x.RowData.SupplierBUId;
                }
                return param;
            })
        )).Where(x => x != null).ToList()!;


        await _materialApprovalRequestDetailManager.CreateBatchInProgressAsync(createParams);

        await _localEventBus.PublishAsync(new MaterialApprovalCreatedEvent(approvalRequestData.Id, note), onUnitOfWorkComplete: false);

        await UnitOfWorkManager.Current!.SaveChangesAsync();

        var res = await _materialApprovalRequestRepository.GetWithDetailAsync(approvalRequestData.Id);



        return ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(res);
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.UpdatePrice)]
    public async Task<MaterialApprovalRequestDto> ImportMaterialUpdatePriceAsync(
    ExcelValidationResult<MaterialUpdatePriceImportDto> data,
    string? note = null)
    {
        var approvalRequestCreateParams = ObjectMapper.Map<ExcelValidationResult<MaterialUpdatePriceImportDto>, MaterialApprovalRequestCreateParams>(data);
        approvalRequestCreateParams.Note = note;
        approvalRequestCreateParams.ImportType = MaterialImportType.M2U;

        var materialApprovalRequest = await _materialApprovalRequestManager.CreateAsync(approvalRequestCreateParams);

        var context = new ExcelImportContext();
        var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<MaterialUpdatePriceImportDto, ExcelMaterialUpdatePriceParams>(ExcelImporters.MaterialUpdatePrice);

        var updateParams = (await Task.WhenAll(
                data.ListData.Select(x => createParamsConverters.ConvertToCreateParamsAsync(x, context))
            ))
            .OfType<ExcelMaterialUpdatePriceParams>()
            .ToList();

        //var materialIds = updateParams.Select(x => x.Id).Where(x => x.HasValue).Select(x => x.Value).Distinct();
        //var materials = await _materialRepository.GetListAsync(m => materialIds.Contains(m.Id));

        var materialApprovalDetailCreateParamsList = updateParams.Select(x =>
        {
            var materialApprovalRequestId = materialApprovalRequest.Id;
            return new MaterialApprovalRequestDetailCreateParams(x, materialApprovalRequestId);
        }).ToList();
        //var materialApprovalDetailCreateParamsList = (
        //    from material in materials
        //    join update in updateParams
        //        on new { material.GolfaCode, material.Model }
        //                equals new { update.GolfaCode, update.Model }
        //    select new MaterialApprovalRequestDetailCreateParams(update, materialApprovalRequest.Id)
        //).ToList();


        await _materialApprovalRequestDetailManager.CreateBatchInProgressAsync(materialApprovalDetailCreateParamsList);

        await _localEventBus.PublishAsync(new MaterialApprovalCreatedEvent(materialApprovalRequest.Id, note), onUnitOfWorkComplete: false);

        return ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(materialApprovalRequest);
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.UpdateMaterialWithoutPrice)]
    [UnitOfWork(IsDisabled = true)]
    public async Task<MaterialApprovalRequestDto> ImportMaterialUpdateWithoutPriceAsync(ExcelValidationResult<MaterialUpdateWithoutPriceImportDto> data, string? note = null)
    {
        const int batchSize = 1000;
        MaterialApprovalRequest materialApprovalRequest;

        // Step 1: Create approval request in a separate UoW (Transaction)
        // This ensures the header is committed before we start the heavy data processing
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
        {
            var approvalRequestCreateParams = ObjectMapper.Map<ExcelValidationResult<MaterialUpdateWithoutPriceImportDto>, MaterialApprovalRequestCreateParams>(data);
            approvalRequestCreateParams.Note = note;
            approvalRequestCreateParams.ImportType = MaterialImportType.M3U;

            materialApprovalRequest = await _materialApprovalRequestManager.CreateAsync(approvalRequestCreateParams);
            materialApprovalRequest.Status = QuoteFlowStatuses.Approved;

            // Record history immediately within the same transaction
            materialApprovalRequest.RecordAction(new MaterialApprovalRequestHistory(
                _guidGenerator.Create(),
                materialApprovalRequest.Id,
                new(
                    null,
                    null,
                    _currentFullUser.Username,
                    _currentFullUser.FullName,
                    HistoryActions.Submitted,
                    DateTime.Now,
                    note
                )
            ));

            await uow.CompleteAsync();
        }

        // Step 2: Convert data to update params
        // Using a loop instead of Task.WhenAll to reduce Task allocation overhead for large datasets
        var context = new ExcelImportContext();
        var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<MaterialUpdateWithoutPriceImportDto, ExcelMaterialUpdateWithoutPrriceParams>(ExcelImporters.MaterialUpdateWithoutPrice);

        var updateParams = new List<ExcelMaterialUpdateWithoutPrriceParams>();
        foreach (var row in data.ListData)
        {
            var param = await createParamsConverters.ConvertToCreateParamsAsync(row, context);
            if (param is ExcelMaterialUpdateWithoutPrriceParams validParam)
            {
                updateParams.Add(validParam);
            }
        }

        // Step 3: Bulk update materials
        // Assuming this method uses optimized SQL/Bulk operations internally
        //await _materialManager.UpdateListM3UFromExcelAsync(updateParams);
        await _materialRepository.BulkUpdateMaterialWithOutPriceAsync(updateParams);

        // Step 4: Create detail records using BulkInsert in batches
        // Construct the Entities directly to prepare for Repository BulkInsert
        var detailEntities = updateParams.Select(param => new MaterialApprovalRequestDetail(
            _guidGenerator.Create(),
            new MaterialApprovalRequestDetailCreateParams(param, materialApprovalRequest.Id)
        )).ToList();

        // Insert details in batches to prevent lock escalation
        var batches = detailEntities.Chunk(batchSize);
        foreach (var batch in batches)
        {
            // Note: You must ensure _materialApprovalRequestDetailRepository is injected into this class
            await _materialApprovalRequestDetailRepository.BulkInsertM3UAsync(batch.ToList());
        }

        return ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(materialApprovalRequest);
    }
    //public async Task<MaterialApprovalRequestDto> ImportMaterialUpdateWithoutPriceAsync(ExcelValidationResult<MaterialUpdateWithoutPriceImportDto> data, string? note = null)
    //{
    //    var approvalRequestCreateParams = ObjectMapper.Map<ExcelValidationResult<MaterialUpdateWithoutPriceImportDto>, MaterialApprovalRequestCreateParams>(data);
    //    approvalRequestCreateParams.Note = note;
    //    approvalRequestCreateParams.ImportType = MaterialImportType.M3U;

    //    var materialApprovalRequest = await _materialApprovalRequestManager.CreateAsync(approvalRequestCreateParams);
    //    materialApprovalRequest.Status = QuoteFlowStatuses.Approved;


    //    var context = new ExcelImportContext();
    //    //context.SetData(ExcelImportContextKeys.ParentEntityId, materialApprovalRequest.Id);
    //    var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<MaterialUpdateWithoutPriceImportDto, ExcelMaterialUpdateWithoutPrriceParams>(ExcelImporters.MaterialUpdateWithoutPrice);
    //    var updateParams = (await Task.WhenAll(
    //            data.ListData.Select(x => createParamsConverters.ConvertToCreateParamsAsync(x, context))
    //        ))
    //        .OfType<ExcelMaterialUpdateWithoutPrriceParams>()
    //        .ToList();

    //await _materialManager.UpdateListM3UFromExcelAsync(updateParams);


    //    //var materialIds = data.ListData.Select(row => row.RowData.Id);
    //    //var materials = await _materialRepository.GetListAsync(m => materialIds.Contains(m.Id));
    //    var materialApprovalDetailCreateParamsList = updateParams.Select(m => new MaterialApprovalRequestDetailCreateParams(m, materialApprovalRequest.Id));
    //    await _materialApprovalRequestDetailManager.CreateBatchAsync(materialApprovalDetailCreateParamsList);
    //    materialApprovalRequest.RecordAction(new MaterialApprovalRequestHistory(
    //        _guidGenerator.Create(),
    //        materialApprovalRequest.Id,
    //        new(
    //            null,
    //            null,
    //            _currentFullUser.Username,
    //            _currentFullUser.FullName,
    //            HistoryActions.Submitted,
    //            DateTime.Now,
    //            note
    //        )
    //    ));

    //    return ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(materialApprovalRequest);
    //}

    //public async Task<MaterialApprovalRequestDto> SubmitAsync(Guid id, MaterialApprovalRequestSubmitDto input)
    //{
    //    var materialApproval = await _materialApprovalRequestRepository.GetWithDetailAsync(id);
    //    await ValidateMaterialApprovalSubmissionAsync(materialApproval, input);
    //    await ProcessMaterialApprovalSubmissionAsync(materialApproval);
    //    var result = ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(materialApproval);


    //    return result;
    //}

    public async Task<MaterialApprovalRequestDto> PerformActionAsync(Guid id, ActionDto input)
    {
        var materialApproval = await _materialApprovalRequestRepository.GetWithDetailAsync(id);
        await ValidateRequestAction(materialApproval, input);

        await ProcessActionAsync(materialApproval, input);

        //if (input.Action == HistoryActions.Cancelled)
        //{
        //    await _materialApprovalRequestDetailRepository.ActionAsync(id, QuoteFlowStatuses.Cancelled);
        //}
        //else if (input.Action == HistoryActions.Rejected)
        //{
        //    await _materialApprovalRequestDetailRepository.ActionAsync(id, QuoteFlowStatuses.Rejected);
        //}
        var result = ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(materialApproval);


        return result;
    }

    private async Task ProcessActionAsync(MaterialApprovalRequest materialApproval, ActionDto input)
    {
        _currentUser ??= await GetCurrentUserAsync();

        var currentRoutes = (materialApproval.MaterialRoutes ?? []).ToList();
        var currentApproverRoute = currentRoutes!.FirstOrDefault(x => x.Approver != null && x.Approver.Equals(_currentUser.Email, StringComparison.InvariantCultureIgnoreCase));

        var allNextApprovalRoutes = currentRoutes!.Where(x => x.StepSequence == (materialApproval.CurrentApprovalStepSequence + 1) &&
                                                        x.MaterialApprovalRequestId == materialApproval.Id
                                                        && x.InstanceId == materialApproval.CurrentApprovalRouteInstanceId)
                                                .OrderBy(p => p.StepSequence)
                                                .ToList();
        var nextApprovalRoutes = allNextApprovalRoutes is null ? null : GetNextApprovalRoute(materialApproval, allNextApprovalRoutes);
        var isCurrentRouteValid = currentApproverRoute is not null;
        var isRequester = IsRequestByCurrentUser(materialApproval, _currentUser.UserName);
        bool isRequesterCancelled = isRequester && input.Action == HistoryActions.Cancelled && materialApproval.Status == QuoteFlowStatuses.InProgress;
        if (isRequesterCancelled)
        {
            var historyCreateParams = new ApprovalHistoryCreateParams()
            {

                Action = input.Action,
                ActionDate = DateTime.Now,
                IsLastApprovalInCurrentWorkflow = false,
                EntityType = EntityTypes.MaterialApprovalRequest,
                ApproverRoleCode = null,
                ApproverRoleName = null,
                ApproverUsername = _currentUserFromHeader.Username ?? _currentUserFromToken.UserName ?? "N/A",
                ApproverFullName = _currentUserFromHeader.FullName ?? UserHelper.GetFullName(_currentUserFromToken.Name, _currentUserFromToken.SurName),
                Note = input.Comment
            };

            await _materialApprovalRequestManager.RecordActionAsync(materialApproval, historyCreateParams);
            if (input.Action == HistoryActions.Cancelled)
            {
                await HandleCancelAsync(materialApproval, _currentUser);
            }

        }
        else if (isCurrentRouteValid && (input.Action == HistoryActions.Rejected || input.Action == HistoryActions.Approved))
        {
            var historyCreateParams = new ApprovalHistoryCreateParams()
            {
                Action = input.Action,
                ActionDate = DateTime.Now,
                IsLastApprovalInCurrentWorkflow = false,
                EntityType = EntityTypes.MaterialApprovalRequest,
                ApproverRoleCode = materialApproval.CurrentApproverRoleCode,
                ApproverRoleName = materialApproval.CurrentApproverRoleName,
                ApproverUsername = _currentUserFromHeader.Username ?? _currentUserFromToken.UserName ?? "N/A",
                ApproverFullName = _currentUserFromHeader.FullName ?? UserHelper.GetFullName(_currentUserFromToken.Name, _currentUserFromToken.SurName),
                Note = input.Comment
            };
            await _materialApprovalRequestManager.RecordActionAsync(materialApproval, historyCreateParams);

            if (input.Action == HistoryActions.Rejected)
            {
                await HandleRejectionAsync(materialApproval, null);
            }
            if (input.Action == HistoryActions.Approved)
            {
                await HandleApprovalAsync(materialApproval, nextApprovalRoutes);
            }
        }
        else
        {
            throw new UserFriendlyException("You are not authorized to do this action!");
        }
        await _materialApprovalRequestRepository.UpdateAsync(materialApproval, autoSave: true);
        await _localEventBus.PublishAsync(new MaterialActionedEvent(materialApproval.Id, input.Action, CurrentUser.UserName, DateTime.Now), onUnitOfWorkComplete: false);
    }
    protected async Task HandleApprovalAsync(MaterialApprovalRequest materialApproval, List<MaterialApprovalRequestRoute>? nextApprovalRoutes)
    {

        var currentRoute = (nextApprovalRoutes ?? []).FirstOrDefault();
        bool lastStep = currentRoute is null;

        materialApproval.Approved(lastStep, DateTime.Now, null);
        if (currentRoute is null)
        {

            await _approvalRouteRepository.DeleteDirectAsync(x => x.InstanceId == materialApproval.CurrentApprovalRouteInstanceId && x.EntityType == EntityTypes.MaterialApprovalRequest);
            materialApproval.UpdateCurrentApprovalRoute();

            if (materialApproval.ImportType == MaterialImportType.M2U)
            {
                await _materialApprovalRequestDetailRepository.ActionAsync(materialApproval.Id, QuoteFlowStatuses.Approved);
                var updateParams = await _materialApprovalRequestDetailRepository.GetListAsync(x => x.MaterialApprovalId == materialApproval.Id);
                //var updateParams = ObjectMapper.Map<List<MaterialApprovalRequestDetail>,List<Mater>>
                await _materialManager.UpdateListM2UFromExcelAsync(updateParams);
            }
            else if (materialApproval.ImportType == MaterialImportType.M1U)
            {
                await _materialApprovalRequestDetailRepository.ActionAsync(materialApproval.Id, QuoteFlowStatuses.Approved);
                var materialDetail = await _materialApprovalRequestDetailRepository.GetListAsync(x => x.MaterialApprovalId == materialApproval.Id);
                var materialCreateParamList = materialDetail.Select(m => new MaterialCreateParams(m)).ToList();
                var materialNew = await _materialManager.CreateListAsync(materialCreateParamList);

                var stock = await _stockCategoryRepository.FirstOrDefaultAsync(x => x.MainStock == true);
                var materialStockCreateParamList = materialNew
                    .Select(detail => new MaterialStockCreateParams
                    {
                        MaterialId = detail.Id,
                        StockCategoryId = stock!.Id,
                        GolfaCode = detail.GolfaCode,
                        Model = detail.Model,
                        Qty = 0,
                        Available_Qty = 0,
                        Locked = 0,
                        LockStockKeeping = 0,
                        LockStockSO = 0,
                        Note = null
                    })
                    .ToList();
                await _materialStockManager.CreateListAsync(materialStockCreateParamList);

                var materialStockLockShipmentCreateParamsList = materialNew.Select(detail => new MaterialStockLockShipment(GuidGenerator.Create(), detail.GolfaCode, 0, 0)).ToList();

                await _materialStockLockShipmentManager.CreateListAsync(materialStockLockShipmentCreateParamsList);


            }
            else if (materialApproval.ImportType == MaterialImportType.M5U)
            {
                await _materialApprovalRequestDetailRepository.ActionAsync(materialApproval.Id, QuoteFlowStatuses.Approved);
                var materialDetail = await _materialApprovalRequestDetailRepository.GetListAsync(x => x.MaterialApprovalId == materialApproval.Id);
                var updateParams = materialDetail.Select(m => new ExcelMaterialUpdateInventoryPlanUpdateParams(m)).ToList();
                await _materialManager.UpdateListM5UFromExcelAsync(updateParams);
            }


        }
        else
        {
            materialApproval.UpdateCurrentApprovalRoute(currentRoute.InstanceId, currentRoute.StepSequence, currentRoute.ApproverRoleCode, currentRoute.ApproverRoleName);
        }


    }

    protected async Task HandleCancelAsync(MaterialApprovalRequest request, IdentityUser currentUser)
    {
        request.Cancel();
        if (request.CurrentApprovalRouteInstanceId != null)
        {
            await _approvalRouteRepository.DeleteDirectAsync(x => x.InstanceId == request.CurrentApprovalRouteInstanceId && x.EntityType == EntityTypes.MaterialApprovalRequest);
        }
        request.UpdateCurrentApprovalRoute();

        await _materialApprovalRequestDetailRepository.ActionAsync(request.Id, QuoteFlowStatuses.Cancelled);


    }

    protected async Task HandleRejectionAsync(MaterialApprovalRequest request, MaterialApprovalRequestRoute? currentApproverRoute)
    {
        request.Reject();
        if (request.CurrentApprovalRouteInstanceId != null)
        {
            await _approvalRouteRepository.DeleteDirectAsync(x => x.InstanceId == request.CurrentApprovalRouteInstanceId && x.EntityType == EntityTypes.MaterialApprovalRequest);
        }

        request.UpdateCurrentApprovalRoute();

        await _materialApprovalRequestDetailRepository.ActionAsync(request.Id, QuoteFlowStatuses.Rejected);
    }

    protected bool IsRequestByCurrentUser(MaterialApprovalRequest request, string currentUsername)
    {
        var creatorUsername = request.CreatorUsername;
        return creatorUsername is not null && creatorUsername.Equals(currentUsername, StringComparison.OrdinalIgnoreCase);
    }
    public virtual List<MaterialApprovalRequestRoute>? GetNextApprovalRoute(MaterialApprovalRequest request, List<MaterialApprovalRequestRoute>? allNextApprovalRoutes)
    {
        if (allNextApprovalRoutes == null)
            return null;

        var nextStep = request.CurrentApprovalStepSequence + 1;
        var nextNearestStepSequence = allNextApprovalRoutes
            .Where(ar => ar.MaterialApprovalRequestId == request.Id && ar.InstanceId == request.CurrentApprovalRouteInstanceId && ar.StepSequence >= nextStep)
            .OrderBy(ar => ar.StepSequence)
            .Select(ar => ar.StepSequence)
            .FirstOrDefault();

        var nextRoutes = allNextApprovalRoutes
            .Where(ar => ar.MaterialApprovalRequestId == request.Id && ar.StepSequence == nextNearestStepSequence && !ar.IsApproved)
            .ToList();
        return nextRoutes;
    }

    private async Task ValidateRequestAction(MaterialApprovalRequest request, ActionDto input)
    {
        var validationResults = input.ValidateConcurrencyStamp(request.ConcurrencyStamp);
        if (validationResults.Any())
        {
            throw new AbpValidationException(validationErrors: validationResults.ToList());
        }

        validationResults = await _materialApprovalRequestManager.ValidateRequestActionAsync(request);
        if (validationResults.Any())
        {
            throw new AbpValidationException(validationErrors: validationResults.ToList());
        }
    }
    //public async Task ProcessMaterialApprovalSubmissionAsync(MaterialApprovalRequest request)
    //{
    //    _currentUser ??= await GetCurrentUserAsync();

    //    // Save history of the request pre-action
    //    var historyCreateParams = new ApprovalHistoryCreateParams()
    //    {

    //        Action = HistoryActions.Submitted,
    //        ActionDate = DateTime.Now,
    //        IsLastApprovalInCurrentWorkflow = false,
    //        EntityType = EntityTypes.MaterialApprovalRequest,
    //        ApproverRoleCode = null,
    //        ApproverRoleName = null,
    //        ApproverUsername = _currentUserFromHeader.Username ?? _currentUserFromToken.UserName ?? "N/A",
    //        ApproverFullName = _currentUserFromHeader.FullName ?? UserHelper.GetFullName(_currentUserFromToken.Name, _currentUserFromToken.SurName)

    //    };
    //    await _materialApprovalRequestManager.RecordActionAsync(request, historyCreateParams);

    //    // Submit the request with the new request number

    //    var generatedApprovalRoutes = await GenerateApprovalRoutesAsync(request);



    //    request.Submit();
    //    var startRoute = generatedApprovalRoutes.Where(x => !x.IsApproved).OrderBy(p => p.StepSequence).FirstOrDefault();

    //    if (startRoute is null) // No approval route found
    //    {
    //        // TODO[Kien] [AfterDemo]: Uncomment this line after the demo
    //        throw new BusinessException(QuoteFlowDomainErrorCodes.NoApprovalRouteFound)
    //            .WithData("entityId", request.Id);
    //        //historyCreateParams = new ApprovalHistoryCreateParams(
    //        //    request.Id,
    //        //    request.RequestType.ToString(),
    //        //    RequestHistoryAction.Approved,
    //        //    RequestConsts.SystemRoleCode,
    //        //    RequestConsts.SystemRoleName,
    //        //    RequestConsts.SystemUsername,
    //        //    RequestConsts.SystemFullName,
    //        //    "Auto approved due to no configured approval steps were found",
    //        //    true
    //        //);

    //        //await _approvalHistoryManager.SaveRequestHistoryAsync(historyCreateParams);
    //        //request.UpdateCurrentApprovalRoute(null);
    //        //request.Approve(true);
    //    }

    //    else // Set the first approval route
    //    {

    //        request.UpdateCurrentApprovalRoute(startRoute.InstanceId!.Value, startRoute.StepSequence, startRoute.ApproverRoleCode, startRoute.ApproverRoleName);
    //    }
    //    //// Publish this event to send email
    //    //await _localEventBus.PublishAsync(new CarRequestSendMailEvent(
    //    //   car,
    //    //   RequestHistoryAction.Submitted,
    //    //   _currentUser.UserName,
    //    //   car.RequestStatus!,
    //    //   true), onUnitOfWorkComplete: false);

    //    // Update everything -- call this to get latest concurrency stamp
    //    await _materialApprovalRequestRepository.UpdateAsync(request, autoSave: true);
    //}

    //protected virtual async Task<List<MaterialApprovalRequestRouteDto>> GenerateApprovalRoutesAsync(MaterialApprovalRequest request)
    //{

    //    var filterParams = new MaterialApprovalWFRouteFilterParams()
    //    {
    //        ImportType = request.ImportType,
    //        MaterialId = request.Id,
    //        Note = ""
    //    };
    //    await _materialApprovalRequestRepository.CreateApprovalRoute(filterParams);


    //    var approvalRequest = await _materialApprovalRequestRepository.GetWithDetailAsync(request.Id);
    //    var approvalRoutes = ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(approvalRequest);

    //    return (approvalRoutes.MaterialRoutes ?? []);

    //}
    protected async Task<IdentityUser> GetCurrentUserAsync()
    {
        var currentUserName = _currentUserFromHeader.Username ?? _currentUserFromToken.UserName;

        var result = (await _identityUserRepository.GetListAsync(userName: currentUserName)).FirstOrDefault()
            ?? throw new EntityNotFoundException($"Cannot find user with username {currentUserName}");

        return result;
    }

    //private async Task ValidateMaterialApprovalSubmissionAsync(MaterialApprovalRequest request, MaterialApprovalRequestSubmitDto input)
    //{
    //    var validationResults = input.ValidateConcurrencyStamp(request.ConcurrencyStamp);
    //    if (validationResults.Any())
    //    {
    //        throw new AbpValidationException(validationErrors: validationResults.ToList());
    //    }



    //    var submitParams = ObjectMapper.Map<MaterialApprovalRequestSubmitDto, MaterialApprovalRequestSubmitParams>(input);
    //    validationResults = await _materialApprovalRequestManager.ValidateMaterialApprovalRequestSubmissionAsync(submitParams, request);

    //    if (validationResults.Any())
    //    {
    //        throw new AbpValidationException(validationErrors: validationResults.ToList());
    //    }
    //}

    [Authorize(QuoteFlowPermissions.Materials.Uploads.SapCode)]
    public virtual async Task<ExcelValidationResult<MaterialSAPUpdateExcelDto>> ValidateAndParseSAPAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<MaterialSAPUpdateExcelDto>(ExcelImporters.MaterialSAP);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }


    [Authorize(QuoteFlowPermissions.Materials.Uploads.SapCode)]
    public async Task UpdateMaterialSAPAsync(ExcelValidationResult<MaterialSAPUpdateExcelDto> data, string? note)
    {
        _currentUser ??= await GetCurrentUserAsync();
        var materialApprovalParams = new MaterialApprovalRequestCreateParams();
        materialApprovalParams.FileName = data.FileName;
        materialApprovalParams.Note = note;
        materialApprovalParams.ImportType = MaterialImportType.M7U;
        MaterialApprovalRequest materialApprovalRequest = await _materialApprovalRequestManager.CreateAsync(materialApprovalParams);
        materialApprovalRequest.Status = QuoteFlowStatuses.Approved;

        var createParamsConverter = _excelImportFactory
            .CreateCreateParamsConverter<MaterialSAPUpdateExcelDto, ExcelMaterialUpdateParams>(ExcelImporters.MaterialSAP);
        var context = new ExcelImportContext();
        var updateParams = (await Task.WhenAll(
                data.ListData.Select(x => createParamsConverter.ConvertToCreateParamsAsync(x, context))
            ))
            .OfType<ExcelMaterialUpdateParams>()
            .ToList();

        await _materialManager.UpdateBatchFromExcelAsync(updateParams);

        var detailParamsList = data.ListData.Select(row =>
        {

            return new MaterialApprovalRequestDetailCreateParams(
                materialRequestId: materialApprovalRequest.Id,
                golfaCode: row.RowData.GolfaCode!,
                model: row.RowData.Model!,
                sapCode: row.RowData.SAPCode,
                description: row.RowData.DescriptionVN,
                productHierarchy: row.RowData.ProductHiearchy,
                vat: row.RowData.VAT
            );
        }).ToList();
        await _materialApprovalRequestDetailManager.CreateBatchAsync(detailParamsList);

        materialApprovalRequest.RecordAction(new MaterialApprovalRequestHistory(
            _guidGenerator.Create(),
            materialApprovalRequest.Id,
            new(
                null,
                null,
                _currentFullUser.Username,
                _currentFullUser.FullName,
                HistoryActions.Submitted,
                DateTime.Now,
                note
            )
        ));

        //await _materialManager.UpdateBatchAsync(updateParams);
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.Leadtime)]
    public async Task<ExcelValidationResult<MaterialFactoryUpdateExcelDto>> ValidateAndParseFactoryAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<MaterialFactoryUpdateExcelDto>(ExcelImporters.MaterialFactory);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.Leadtime)]
    [UnitOfWork(IsDisabled = true)] // Disable default UoW to manage transactions manually for better performance
    public async Task UpdateMaterialFactoryAsync(ExcelValidationResult<MaterialFactoryUpdateExcelDto> data, string? note)
    {
        const int batchSize = 1000;
        MaterialApprovalRequest materialApprovalRequest;

        // Step 1: Create approval request in a separate UoW
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
        {
            var materialApprovalParams = new MaterialApprovalRequestCreateParams
            {
                FileName = data.FileName,
                Note = note,
                ImportType = MaterialImportType.M6U
            };
            materialApprovalRequest = await _materialApprovalRequestManager.CreateAsync(materialApprovalParams);
            materialApprovalRequest.Status = QuoteFlowStatuses.Approved;

            materialApprovalRequest.RecordAction(new MaterialApprovalRequestHistory(
                _guidGenerator.Create(),
                materialApprovalRequest.Id,
                new(
                    null,
                    null,
                    _currentFullUser.Username,
                    _currentFullUser.FullName,
                    HistoryActions.Submitted,
                    DateTime.Now,
                    note
                )
            ));

            await uow.CompleteAsync();
        }

        // Step 2: Convert data to update params (sync, no async overhead)
        var dataObjects = _excelImportFactory
            .CreateCreateParamsConverter<MaterialFactoryUpdateExcelDto, ExcelMaterialFactoryUpdateParams>(ExcelImporters.MaterialFactory);

        var context = new ExcelImportContext();
        var updateParams = new List<ExcelMaterialFactoryUpdateParams>();
        foreach (var row in data.ListData)
        {
            var param = await dataObjects.ConvertToCreateParamsAsync(row, context, default);
            if (param is ExcelMaterialFactoryUpdateParams factoryParam)
            {
                updateParams.Add(factoryParam);
            }
        }

        // Step 3: Bulk update materials using SqlBulkCopy + MERGE (no EF tracking overhead)
        await _materialRepository.BulkUpdateFactoryAsync(updateParams);

        // Step 4: Create detail records using SqlBulkCopy in batches
        var detailEntities = data.ListData.Select(row => new MaterialApprovalRequestDetail(
            _guidGenerator.Create(),
            new MaterialApprovalRequestDetailCreateParams(
                materialRequestId: materialApprovalRequest.Id,
                golfaCode: row.RowData.GolfaCode!,
                model: row.RowData.Model!,
                referenceLeadTime: row.RowData.ReferenceLeadTime,
                country: row.RowData.CountryOfOrigin,
                maxlot: row.RowData.Maxlot
            )
        )).ToList();

        // Insert details in batches to prevent lock escalation
        var batches = detailEntities.Chunk(batchSize);
        foreach (var batch in batches)
        {
            await _materialApprovalRequestDetailRepository.BulkInsertAsync(batch.ToList());
        }
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.MaterialStatus)]
    public async Task<ExcelValidationResult<MaterialStatusUpdateExcelDto>> ValidateAndParseStatusAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<MaterialStatusUpdateExcelDto>(ExcelImporters.MaterialStatus);

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "");

        return result;
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.MaterialStatus)]
    public async Task<MaterialApprovalRequestDto> UpdateMaterialStatusAsync(ExcelValidationResult<MaterialStatusUpdateExcelDto> data, string? note)
    {
        var approvalRequestCreateParams = ObjectMapper.Map<ExcelValidationResult<MaterialStatusUpdateExcelDto>, MaterialApprovalRequestCreateParams>(data);
        approvalRequestCreateParams.Note = note;
        approvalRequestCreateParams.ImportType = MaterialImportType.M4U;

        var materialApprovalRequest = await _materialApprovalRequestManager.CreateAsync(approvalRequestCreateParams);
        materialApprovalRequest.Status = QuoteFlowStatuses.Approved;


        var context = new ExcelImportContext();
        //context.SetData(ExcelImportContextKeys.ParentEntityId, materialApprovalRequest.Id);
        //var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<MaterialStatusUpdateExcelDto, ExcelMaterialStatusUpdateParams>(ExcelImporters.MaterialStatus);
        var detailParamsList = data.ListData.Select(row =>
        {

            return new MaterialApprovalRequestDetailCreateParams(
                materialRequestId: materialApprovalRequest.Id,
                golfaCode: row.RowData.GolfaCode!,
                model: row.RowData.Model!,
                finalDPOAcceptanceDate: row.RowData.AcceptanceDate,
                actionDate: row.RowData.ActiveDate,
                action: row.RowData.Action,
                source: row.RowData.Source,
                reason: row.RowData.Reason,
                factoryRefDoc: row.RowData.FactoryRefDoc
            );
        }).ToList();
        await _materialApprovalRequestDetailManager.CreateBatchAsync(detailParamsList);
        materialApprovalRequest.RecordAction(new MaterialApprovalRequestHistory(
            _guidGenerator.Create(),
            materialApprovalRequest.Id,
            new(
                null,
                null,
                _currentFullUser.Username,
                _currentFullUser.FullName,
                HistoryActions.Submitted,
                DateTime.Now,
                note
            )
        ));

        await _materialRepository.UpdateStatusAsync(materialApprovalRequest.Id);

        return ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(materialApprovalRequest);
    }

    [Authorize(QuoteFlowPermissions.Materials.Uploads.InventoryPlanning)]
    public async Task<MaterialApprovalRequestDto> MaterialUpdateInventoryPlanAsync(ExcelValidationResult<MaterialUpdateInventoryPlanImportDto> data, string? note)
    {
        var approvalRequestCreateParams = ObjectMapper.Map<ExcelValidationResult<MaterialUpdateInventoryPlanImportDto>, MaterialApprovalRequestCreateParams>(data);
        approvalRequestCreateParams.Note = note;
        approvalRequestCreateParams.ImportType = MaterialImportType.M5U;

        var materialApprovalRequest = await _materialApprovalRequestManager.CreateAsync(approvalRequestCreateParams);



        var context = new ExcelImportContext();
        ////context.SetData(ExcelImportContextKeys.ParentEntityId, materialApprovalRequest.Id);
        //var createParamsConverters = _excelImportFactory.CreateCreateParamsConverter<MaterialUpdateInventoryPlanImportDto, ExcelMaterialUpdateInventoryPlanUpdateParams>(ExcelImporters.MaterialUpdateInventoryPlan);
        //var updateParams = (await Task.WhenAll(
        //        data.ListData.Select(x => createParamsConverters.ConvertToCreateParamsAsync(x, context))
        //    ))
        //    .OfType<ExcelMaterialUpdateInventoryPlanUpdateParams>()
        //    .ToList();
        //await _materialManager.UpdateListM5UFromExcelAsync(updateParams);




        //var materialIds = data.ListData.Select(row => row.RowData.Id);
        //var materials = await _materialRepository.GetListAsync(m => materialIds.Contains(m.Id));
        //var materialApprovalDetailCreateParamsList = createParamsConverters.Select(m => new MaterialApprovalRequestDetailCreateParams(m, materialApprovalRequest.Id));
        var detailParamsList = data.ListData.Select(row =>
        {

            return new MaterialApprovalRequestDetailCreateParams(
                materialRequestId: materialApprovalRequest.Id,
                golfaCode: row.RowData.GolfaCode!,
                model: row.RowData.Model!,
                inventoryCategory: row.RowData.InventoryCategory,
                stockWarning: row.RowData.StockWarning

            );
        }).ToList();
        await _materialApprovalRequestDetailManager.CreateBatchAsync(detailParamsList);

        await _localEventBus.PublishAsync(new MaterialApprovalCreatedEvent(materialApprovalRequest.Id, note), onUnitOfWorkComplete: false);

        return ObjectMapper.Map<MaterialApprovalRequest, MaterialApprovalRequestDto>(materialApprovalRequest);
    }

    public async Task<PagedResultDto<MaterialApprovalRequestDetailDto>> GetListByApprovalIdAsync(Guid id)
    {
        var items = await _materialApprovalRequestDetailRepository.GetListByApprovalIdAsync(id);
        var total = items.Count();
        return new PagedResultDto<MaterialApprovalRequestDetailDto>
        {
            TotalCount = total,
            Items = ObjectMapper.Map<List<MaterialApprovalRequestDetail>, List<MaterialApprovalRequestDetailDto>>(items)
        };
    }

    public virtual async Task<List<ApproverDto>> GetListApproversAsync(Guid materialId)
    {
        var material = await _materialApprovalRequestRepository.GetWithDetailAsync(materialId);

        if (material.CurrentApprovalRouteInstanceId is null)
        {
            return [];
        }

        var pendingApprovalRoutes = _approvalRouteManager.GetLatestUnapprovedSteps(material.MaterialRoutes, material.Id);
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

    public virtual async Task<List<HistoryTrackingDto>> GetListMaterialHistoryAsync(string golfaCode)
    {
        var items = await _historyTrackingRepository.GetListAsync(x => x.TrackingType == "Material" && x.GolfaCode == golfaCode);
        items = items.OrderByDescending(x => x.CreationTime).ToList();
        return ObjectMapper.Map<List<HistoryTracking>, List<HistoryTrackingDto>>(items);
    }

    public virtual async Task<IRemoteStreamContent> GetMaterialHistoryAsExcelAsync(string golfaCode)
    {
        // Get material info for filename
        var material = await _materialRepository.FirstOrDefaultAsync(x => x.GolfaCode == golfaCode);

        var filterParams = new HistoryTrackingFilterParams()
        {
            TrackingType = "Material",
            GolfaCode = golfaCode,
        };

        var histories = await _historyTrackingRepository.GetListAsync(filterParams);
        var historyDtos = ObjectMapper.Map<List<HistoryTracking>, List<HistoryTrackingDto>>(histories);

        var excelBytes = await CreateMaterialHistoryExcelFileAsync(historyDtos, material);
        var memoryStream = new MemoryStream(excelBytes);

        var fileName = material != null
            ? $"MaterialHistory_{golfaCode}_{material.Model}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            : $"MaterialHistory_{golfaCode}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        return new RemoteStreamContent(
            memoryStream,
            fileName,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    private async Task<byte[]> CreateMaterialHistoryExcelFileAsync(
        List<HistoryTrackingDto> data,
        Material? material)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Material History");

        // Add title with material info
        if (material != null)
        {
            worksheet.Cell(1, 1).Value = $"Material History - Code: {material.GolfaCode} - Model: {material.Model}";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 14;
            worksheet.Range(1, 1, 1, 7).Merge();

            // Start headers from row 3
            var headerRow = 3;
            AddHeaders(worksheet, headerRow);
            AddDataRows(worksheet, data, headerRow + 1);
        }
        else
        {
            // No material info, start from row 1
            AddHeaders(worksheet, 1);
            AddDataRows(worksheet, data, 2);
        }

        // Set column widths
        SetColumnWidths(worksheet);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private void AddHeaders(IXLWorksheet worksheet, int row)
    {
        var headers = new[]
        {
        "No",
        "Action",
        "Previous Value",
        "Next Value",
        "Before Change",
        "After Change",
        "Creator",
        "Date"
    };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(row, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }
    }

    private void AddDataRows(IXLWorksheet worksheet, List<HistoryTrackingDto> data, int startRow)
    {
        for (int i = 0; i < data.Count; i++)
        {
            var item = data[i];
            var excelRow = startRow + i;

            // No
            worksheet.Cell(excelRow, 1).Value = i + 1;
            worksheet.Cell(excelRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Action
            worksheet.Cell(excelRow, 2).Value = item.Action ?? "";

            // Previous Value (with number format if numeric)
            if (item.PreviousValue.HasValue)
            {
                worksheet.Cell(excelRow, 3).Value = (double)item.PreviousValue.Value;
                worksheet.Cell(excelRow, 3).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Cell(excelRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            }
            else
            {
                worksheet.Cell(excelRow, 3).Value = "";
            }

            // Next Value (with number format if numeric)
            if (item.NextValue.HasValue)
            {
                worksheet.Cell(excelRow, 4).Value = (double)item.NextValue.Value;
                worksheet.Cell(excelRow, 4).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Cell(excelRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            }
            else
            {
                worksheet.Cell(excelRow, 4).Value = "";
            }

            // Before Change
            worksheet.Cell(excelRow, 5).Value = item.BeforeChange ?? "";

            // After Change
            worksheet.Cell(excelRow, 6).Value = item.AfterChange ?? "";

            // Creator
            worksheet.Cell(excelRow, 7).Value = item.CreatorUsername ?? "";

            // Date (with custom format)
            if (item.CreationTime != default)
            {
                worksheet.Cell(excelRow, 8).Value = item.CreationTime.ToString("dd/MM/yyyy HH:mm:ss");
            }
            else
            {
                worksheet.Cell(excelRow, 8).Value = "";
            }

            // Add borders to all cells in row
            worksheet.Range(excelRow, 1, excelRow, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }
    }

    private void SetColumnWidths(IXLWorksheet worksheet)
    {
        var columnWidths = new Dictionary<int, double>
    {
        { 1, 8 },   // No
        { 2, 15 },  // Action
        { 3, 15 },  // Previous Value
        { 4, 15 },  // Next Value
        { 5, 20 },  // Before Change
        { 6, 20 },  // After Change
        { 7, 25 },  // Creator
        { 8, 20 }   // Date
    };

        foreach (var kvp in columnWidths)
        {
            worksheet.Column(kvp.Key).Width = kvp.Value;
        }
    }

    private class MaterialSupportInfo : Entity<Guid>, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; } = null!;
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string MaterialStatus { get; set; } = null!;
        public int? StockWarning { get; set; }

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}

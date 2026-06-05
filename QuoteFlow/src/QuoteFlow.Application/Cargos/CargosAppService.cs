using QuoteFlow.Cargos.CargoDatas;
using QuoteFlow.Cargos.CargoDatas.ParameterObjects;
using QuoteFlow.Cargos.ParameterObjects;
using QuoteFlow.Permissions;
using QuoteFlow.PurchaseOrderDetails;
using QuoteFlow.Shared.Excels;
using Microsoft.AspNetCore.Authorization;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace QuoteFlow.Cargos;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.CargoDatas.Default)]
public class CargosAppService : QuoteFlowAppService, ICargosAppService
{
    protected ICargoDataRepository _cargoDataRepository;
    protected ICargoRepository _cargoRepository;
    protected IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
    protected PurchaseOrderDetailManager _purchaseOrderDetailManager;
    protected CargoManager _cargoManager;
    protected IExcelImportFactory _excelImportFactory;
    protected CargoDataManager _cargoDataManager;

    public CargosAppService(
        ICargoRepository cargoRepository,
        CargoManager cargoManager,
        ICargoDataRepository cargoDataRepository,
        IExcelImportFactory excelImportFactory,
        CargoDataManager cargoDataManager,
        IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
        PurchaseOrderDetailManager purchaseOrderDetailManager)
    {
        _cargoRepository = cargoRepository;
        _cargoManager = cargoManager;
        _cargoDataRepository = cargoDataRepository;
        _excelImportFactory = excelImportFactory;
        _cargoDataManager = cargoDataManager;
        _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
        _purchaseOrderDetailManager = purchaseOrderDetailManager;
    }

    public virtual async Task<PagedResultDto<CargoDto>> GetListAsync(GetCargosInput input)
    {
        var filterParams = ObjectMapper.Map<GetCargosInput, CargoFilterParams>(input);
        var totalCount = await _cargoRepository.GetCountAsync(filterParams);
        var items = await _cargoRepository.GetListAsync(filterParams);

        return new PagedResultDto<CargoDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Cargo>, List<CargoDto>>(items)
        };
    }

    public virtual async Task<PagedResultDto<CargoDataDto>> GetListCargoDataAsync(GetCargoDataInput input)
    {
        var filterParams = ObjectMapper.Map<GetCargoDataInput, CargoDataFilterParams>(input);
        var totalCount = await _cargoDataRepository.GetCountAsync(filterParams);
        var items = await _cargoDataRepository.GetListAsync(filterParams);

        return new PagedResultDto<CargoDataDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<CargoData>, List<CargoDataDto>>(items)
        };
    }
    public virtual async Task<CargoDto> GetDetailAsync(Guid id)
    {
        var cargo = await _cargoRepository.GetDetailAsync(id);
        return ObjectMapper.Map<Cargo, CargoDto>(cargo);
    }
    public virtual async Task<PagedResultDto<CargoDataDto>> GetListDetailAsync(GetCargoDataInput input)
    {
        //var items = await _cargoDataRepository.GetListAsync(x => x.CargoId == id);
        //return new PagedResultDto<CargoDataDto>
        //{
        //    TotalCount = items.Count,
        //    Items = ObjectMapper.Map<List<CargoData>, List<CargoDataDto>>(items)
        //};
        input.MaxResultCount = 10000;
        var filterParams = ObjectMapper.Map<GetCargoDataInput, CargoDataFilterParams>(input);
        var totalCount = await _cargoDataRepository.GetCountAsync(filterParams);
        var items = await _cargoDataRepository.GetListAsync(filterParams);

        return new PagedResultDto<CargoDataDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<CargoData>, List<CargoDataDto>>(items)
        };
    }
    public virtual async Task<CargoDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Cargo, CargoDto>(await _cargoRepository.GetAsync(id));
    }

    public virtual async Task DeleteCargoDataAsync(Guid cargoDataId, Guid cargoId)
    {
        var detail = await _cargoDataRepository.GetAsync(cargoDataId);

        await _cargoDataRepository.DeleteAsync(cargoDataId);

        await _purchaseOrderDetailManager
            .UpdateRemoveCargoAsync(detail.PODetailId!.Value);

    }

    public virtual async Task<CargoDto> CreateAsync(CargoCreateDto input)
    {
        var createParams = ObjectMapper.Map<CargoCreateDto, CargoCreateParams>(input);
        var cargo = await _cargoManager.CreateAsync(
            createParams
        );

        return ObjectMapper.Map<Cargo, CargoDto>(cargo);
    }

    public virtual async Task<CargoDto> UpdateAsync(Guid id, CargoUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<CargoUpdateDto, CargoUpdateParams>(input);
        var cargo = await _cargoManager.UpdateAsync(
        id,
        updateParams
        );

        return ObjectMapper.Map<Cargo, CargoDto>(cargo);
    }

    public async Task<PagedResultDto<CargoReportDto>> GetListCargoReportAsync(GetCargoReportsInput input)
    {
        var filterParams = ObjectMapper.Map<GetCargoReportsInput, CargoReportFilterParams>(input);
        var count = await _cargoRepository.GetCountListCargoReportAsync(filterParams);
        var items = await _cargoRepository.GetListCargoReportAsync(filterParams);

        return new PagedResultDto<CargoReportDto>
        {
            TotalCount = count,
            Items = ObjectMapper.Map<List<CargoReport>, List<CargoReportDto>>(items)
        };
    }

    public async Task<IRemoteStreamContent> GetListAsExcelAsync(GetCargoReportsInput input)
    {
        var filterParams = ObjectMapper.Map<GetCargoReportsInput, CargoReportFilterParams>(input);
        filterParams.SkipCount = 0; //reset skip count
        filterParams.MaxResultCount = int.MaxValue;
        var items = await _cargoRepository.GetListCargoReportAsync(filterParams);

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(ObjectMapper.Map<List<CargoReport>, List<CargoReportDto>>(items));
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "CargoReport.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<ExcelValidationResult<CargoImportDto>> ValidateAndParseCargoAsync(IRemoteStreamContent file, CargoExcelInput input)
    {
        var validator = _excelImportFactory.CreateValidator<CargoImportDto>(ExcelImporters.Cargo);
        var context = new ExcelImportContext();
        context.SetData(ExcelImportContextKeys.Cargo.MaterialType, input.MaterialType);
        context.SetData(ExcelImportContextKeys.Cargo.SupplierCode, input.SupplierCode);


        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);

        return result;
    }

    //public async Task<CargoDto> ImportCargoAsync(ImportCargoRequestDto request)
    //{
    //    var data = request.Data;
    //    var input = request.Input;
    //    var cargoParams = new CargoCreateParams();
    //    cargoParams.FileName = data.FileName;
    //    cargoParams.Note = input.Note;
    //    cargoParams.MaterialType = input.MaterialType;
    //    cargoParams.SupplierCode = input.SupplierCode;
    //    //var approvalRequestData = ObjectMapper.Map<Cargo, CargoDto>(await _cargoManager.CreateAsync(cargoParams));

    //    var cargoDataObjects = _excelImportFactory.CreateCreateParamsConverter<CargoImportDto, CargoDataCreateParams>(ExcelImporters.Cargo);

    //    var context = new ExcelImportContext();
    //    //context.SetData(ExcelImportContextKeys.ParentEntityId, approvalRequestData.Id);

    //    List<CargoDataCreateParams> createParams = (await Task.WhenAll(
    //        data.ListData
    //            .Select(x => cargoDataObjects.ConvertToCreateParamsAsync(x, context, default))
    //        )).Where(x => x != null)
    //        .ToList()!;

    //    var relatedPurchaseOrderDetailIds = createParams
    //        .Where(x => x.PODetailId.HasValue)
    //        .Select(x => x.PODetailId!.Value)
    //        .Distinct()
    //        .ToList();

    //    var relatedPurchaseOrderDetails = await _purchaseOrderDetailRepository.GetListAsync(x => relatedPurchaseOrderDetailIds.Contains(x.Id));
    //    var cargoDatas = await _cargoDataRepository.GetListAsync();

    //    var idCargoDataDelete = new List<Guid>();

    //    foreach (var cargoDataCreateParams in createParams)
    //    {
    //        cargoDataCreateParams.FileName = data.FileName;
    //        cargoDataCreateParams.SupplierCode = input.SupplierCode;
    //        cargoDataCreateParams.MaterialType = input.MaterialType;
    //        cargoDataCreateParams.Note = input.Note;

    //        var cargoDelete = cargoDatas.FirstOrDefault(x =>
    //            x.InvoiceNo == cargoDataCreateParams.InvoiceNo
    //            && x.PODetailCode == cargoDataCreateParams.PODetailCode
    //            && x.PORef == cargoDataCreateParams.PORef
    //            && x.GolfaCode.Equals(cargoDataCreateParams.GolfaCode, StringComparison.OrdinalIgnoreCase));

    //        if (cargoDelete is not null)
    //        {
    //            idCargoDataDelete.Add(cargoDelete.Id);
    //        }

    //        if (cargoDataCreateParams.PODetailId.HasValue)
    //        {
    //            var relatedDetail = relatedPurchaseOrderDetails.FirstOrDefault(x => x.Id == cargoDataCreateParams.PODetailId!.Value);
    //            if (relatedDetail != null)
    //            {
    //                relatedDetail.MachineNumber = cargoDataCreateParams.MachineNumber;
    //                relatedDetail.MEVNAddedRequest = cargoDataCreateParams.MEVNAddedRequest;
    //                relatedDetail.MEVNRequest = cargoDataCreateParams.MEVNRequest;
    //                relatedDetail.STCReply = cargoDataCreateParams.STCReply;
    //            }
    //        }
    //    }

    //    if (idCargoDataDelete.Any())
    //    {
    //        await _cargoDataManager.DeleteManyAsync(idCargoDataDelete);
    //    }

    //    await _cargoDataManager.CreateBatchAsync(createParams);
    //    await UnitOfWorkManager.Current!.SaveChangesAsync();

    //    await _purchaseOrderDetailRepository.UpdateManyAsync(relatedPurchaseOrderDetails);
    //    await UnitOfWorkManager.Current!.SaveChangesAsync();

    //    //var res = await _cargoRepository.GetAsync(approvalRequestData.Id);
    //    //return ObjectMapper.Map<Cargo, CargoDto>(res);
    //    return null;
    //}
    public async Task<CargoDto> ImportCargoAsync(ImportCargoRequestDto request)
    {
        var data = request.Data;
        var input = request.Input;

        // 1. Convert dữ liệu Excel
        var cargoDataObjects = _excelImportFactory.CreateCreateParamsConverter<CargoImportDto, CargoDataCreateParams>(ExcelImporters.Cargo);
        var context = new ExcelImportContext();

        List<CargoDataCreateParams> createParams = (await Task.WhenAll(
            data.ListData
                .Select(x => cargoDataObjects.ConvertToCreateParamsAsync(x, context, default))
            )).Where(x => x != null)
            .ToList()!;

        // 2. Tối ưu load PurchaseOrderDetail (Logic giữ nguyên vì dùng ID là Unique)
        var relatedPurchaseOrderDetailIds = createParams
            .Where(x => x.PODetailId.HasValue)
            .Select(x => x.PODetailId!.Value)
            .Distinct()
            .ToList();

        var relatedPurchaseOrderDetails = new List<PurchaseOrderDetail>();
        if (relatedPurchaseOrderDetailIds.Any())
        {
            relatedPurchaseOrderDetails = await _purchaseOrderDetailRepository
                .GetListAsync(x => relatedPurchaseOrderDetailIds.Contains(x.Id));
        }
        // Dùng Dictionary cho PO Detail là an toàn vì ID là duy nhất
        var poDetailMap = relatedPurchaseOrderDetails.ToDictionary(x => x.Id);

        // 3. TỐI ƯU LOAD CARGO DATA (SỬA LẠI CHO ĐÚNG LOGIC)

        // Lấy danh sách InvoiceNo để lọc bớt dữ liệu rác từ DB
        var invoiceNos = createParams
            .Select(x => x.InvoiceNo)
            .Distinct()
            .ToList();

        List<CargoData> potentialDuplicates = new List<CargoData>();
        if (invoiceNos.Any())
        {
            // Chỉ tải những dòng có InvoiceNo liên quan.
            // Đây là bước tối ưu RAM quan trọng nhất.
            potentialDuplicates = await _cargoDataRepository
                .GetListAsync(x => invoiceNos.Contains(x.InvoiceNo));
        }

        // [QUAN TRỌNG]: Dùng ToLookup thay vì ToDictionary.
        // ToLookup cho phép 1 InvoiceNo có nhiều dòng dữ liệu (như DB gốc).
        // Key là InvoiceNo để tra cứu nhanh O(1).
        var cargoLookup = potentialDuplicates.ToLookup(x => x.InvoiceNo);

        var idCargoDataDelete = new HashSet<Guid>();

        // 4. Vòng lặp xử lý logic
        foreach (var cargoDataCreateParams in createParams)
        {
            // Gán thông tin (Logic gốc)
            cargoDataCreateParams.FileName = data.FileName;
            cargoDataCreateParams.SupplierCode = input.SupplierCode;
            cargoDataCreateParams.MaterialType = input.MaterialType;
            cargoDataCreateParams.Note = input.Note;

            // [LOGIC CHUẨN]: 
            // Bước 1: Lấy nhóm các dòng trong DB có cùng InvoiceNo (Tra cứu cực nhanh từ RAM)
            var candidates = cargoLookup[cargoDataCreateParams.InvoiceNo];

            // Bước 2: Tìm chính xác bằng FirstOrDefault như code gốc
            // Giữ nguyên so sánh OrdinalIgnoreCase và KHÔNG Trim() nếu code gốc không Trim
            var cargoDelete = candidates.FirstOrDefault(x =>
                x.PODetailCode == cargoDataCreateParams.PODetailCode
                && x.PORef == cargoDataCreateParams.PORef
                && string.Equals(x.GolfaCode, cargoDataCreateParams.GolfaCode, StringComparison.OrdinalIgnoreCase));
            // Lưu ý: dùng string.Equals static method để tránh lỗi NullReferenceException nếu x.GolfaCode null

            if (cargoDelete is not null)
            {
                idCargoDataDelete.Add(cargoDelete.Id);
            }

            // Update PO Detail (Logic gốc)
            if (cargoDataCreateParams.PODetailId.HasValue &&
                poDetailMap.TryGetValue(cargoDataCreateParams.PODetailId.Value, out var relatedDetail))
            {
                //if (relatedDetail.StatusCode != QuoteFlowStatuses.Closed)
                //{
                relatedDetail.MachineNumber = cargoDataCreateParams.MachineNumber;
                relatedDetail.MEVNAddedRequest = cargoDataCreateParams.MEVNAddedRequest;
                relatedDetail.MEVNRequest = cargoDataCreateParams.MEVNRequest;
                relatedDetail.STCReply = cargoDataCreateParams.STCReply;
                //}
            }
        }

        // 5. Lưu xuống DB
        if (idCargoDataDelete.Any())
        {
            // HashSet giúp tránh việc gửi ID trùng lặp xuống SQL gây lỗi hoặc chậm
            await _cargoDataManager.DeleteManyAsync(idCargoDataDelete.ToList());
        }

        if (createParams.Any())
        {
            await _cargoDataManager.CreateBatchAsync(createParams);
        }

        if (relatedPurchaseOrderDetails.Any())
        {
            await _purchaseOrderDetailRepository.UpdateManyAsync(relatedPurchaseOrderDetails);
        }

        await UnitOfWorkManager.Current!.SaveChangesAsync();

        return null;
    }
}

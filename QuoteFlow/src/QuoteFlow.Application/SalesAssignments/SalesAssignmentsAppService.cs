using QuoteFlow.BuyerAccess;
using QuoteFlow.Permissions;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SalesAssignments.ParameterObjects;
using QuoteFlow.Shared.Utils;
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
using Volo.Abp.Identity;
using Volo.FileManagement.Files;

namespace QuoteFlow.SalesAssignments;

[RemoteService(IsEnabled = false)]

public class SalesAssignmentsAppService : QuoteFlowAppService, ISalesAssignmentsAppService
{
    protected ISalesAssignmentRepository _salesAssignmentRepository;
    protected IIdentityUserRepository _identityUserRepository;
    protected readonly IEffectiveUserContext _currentUser;
    private readonly IRepository<FileDescriptor, Guid> _fileDescriptorRepository;
    private readonly FileDescriptorAppService _fileDescriptorAppService;
    private readonly IBuyerAccessService _buyerAccessService;

    protected SalesAssignmentManager _salesAssignmentManager;
    public SalesAssignmentsAppService(ISalesAssignmentRepository SalesAssignmentRepository, SalesAssignmentManager SalesAssignmentManager, IIdentityUserRepository identityUserRepository, IEffectiveUserContext currentUser, IRepository<FileDescriptor, Guid> fileDescriptorRepository, FileDescriptorAppService fileDescriptorAppService, IBuyerAccessService buyerAccessService)
    {
        _salesAssignmentRepository = SalesAssignmentRepository;
        _salesAssignmentManager = SalesAssignmentManager;
        _identityUserRepository = identityUserRepository;
        _currentUser = currentUser;
        _fileDescriptorRepository = fileDescriptorRepository;
        _fileDescriptorAppService = fileDescriptorAppService;
        _buyerAccessService = buyerAccessService;
    }

    public async Task<List<SalesAssignmentDto>> CreateAsync(SalesAssignmentCreateDto input)
    {
        var dtoList = new List<SalesAssignmentCreateParams>();
        //var result = new List<SalesAssignmentDto>();

        // First loop: build the list of properly structured DTOs
        foreach (var user in input.Users)
        {
            var materialTypes = input.MaterialType?.Split(',') ?? new[] { input.MaterialType };

            foreach (var materialType in materialTypes)
            {
                var dto = new SalesAssignmentCreateParams
                {
                    BuyerId = input.BuyerId,
                    BuyerTypeId = input.BuyerTypeId,
                    LocationId = input.LocationId,
                    SaleUserName = user.UserName,
                    SaleFullName = user.FullName,
                    BuyerShortName = input.BuyerShortName,
                    MaterialType = materialType.Trim()
                };
                dtoList.Add(dto);
            }
        }
        var result = await _salesAssignmentManager.CreateManyAsync(dtoList);

        return ObjectMapper.Map<List<SalesAssignment>, List<SalesAssignmentDto>>(result);
    }

    public async Task DeleteAsync(Guid id)
    {
        //await _salesAssignmentRepository.DeleteAsync(id);
        try
        {
            await _salesAssignmentRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.Category.RecordInUseCannotDelete);
        }
    }

    public Task<SalesAssignmentDto> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Shared.UserLookupDto>> GetListUserLookup(string name)
    {
        var users = await _identityUserRepository.GetListAsync();

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

    public async Task<PagedResultDto<SalesAssignmentDto>> GetListAsync(GetSalesAssignmentInput input)
    {
        var filterParams = ObjectMapper.Map<GetSalesAssignmentInput, SalesAssignmentFilterParams>(input);
        var totalCount = await _salesAssignmentRepository.GetCountAsync(filterParams);
        var items = await _salesAssignmentRepository.GetListAsync(filterParams, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<SalesAssignmentDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<SalesAssignment>, List<SalesAssignmentDto>>(items)
        };
    }

    [Authorize(QuoteFlowPermissions.Reports.CustomerSaleReportDetail)]
    public async Task<PagedResultDto<SaleReportByCustomerDto>> GetListSaleReportDetailAsync(SaleReportInput input)
    {
        var filterParams = ObjectMapper.Map<SaleReportInput, SaleReportFillterParams>(input);

        filterParams.HasFullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        filterParams.HasStrategicPriceAccess = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.Materials.ViewStrategicPrice);
        filterParams.UserName = _currentUser.Username;

        var items = await _salesAssignmentRepository.ExportSaleReportAsync(filterParams);

        var itemDtos = ObjectMapper.Map<List<SaleReportByCustomer>, List<SaleReportByCustomerDto>>([.. items.Skip(input.SkipCount).Take(input.MaxResultCount)]);
        var totalCount = items.Count;
        return new PagedResultDto<SaleReportByCustomerDto>
        {
            TotalCount = totalCount,
            Items = itemDtos
        };
    }

    [Authorize(QuoteFlowPermissions.Reports.CustomerSaleReportDetail)]
    public async Task<IRemoteStreamContent> GetListSaleReportDetailAsExcelAsync(SaleReportInput input)
    {
        // 1. Get Data
        var filterParams = ObjectMapper.Map<SaleReportInput, SaleReportFillterParams>(input);
        filterParams.MaxResultCount = int.MaxValue;
        filterParams.SkipCount = 0;
        filterParams.HasFullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        filterParams.HasStrategicPriceAccess = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.Materials.ViewStrategicPrice);
        filterParams.UserName = _currentUser.Username;

        var items = await _salesAssignmentRepository.ExportSaleReportAsync(filterParams);
        var itemDtos = ObjectMapper.Map<List<SaleReportByCustomer>, List<SaleReportByCustomerDto>>(items);

        // 2. Get the template file (Make sure you have a file named SaleReport.xlsx in DB)
        var fileDescriptor = await _fileDescriptorRepository
             .FirstOrDefaultAsync(fd => fd.Name == "R06_SalesReportByCustomer_Detail.xlsx")
             ?? throw new UserFriendlyException("Template Excel not found.");
        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        // 3. Load into MemoryStream
        using var originalStream = new MemoryStream(templateBytes);
        using var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        // 4. Open Workbook
        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();

        // CONFIGURATION
        int startRow = 2; // The row where data starts printing
        int startCol = 1; // Column A

        // 5. Insert rows to preserve template formatting (borders, styles)
        if (itemDtos.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(itemDtos.Count - 1);
        }

        // 6. Write data loop
        int currentRow = startRow;
        foreach (var item in itemDtos)
        {
            int col = startCol; // Assuming startCol is 1

            // --- 1. Index & Customer Info ---
            ws.Cell(currentRow, col++).Value = currentRow - 1;                  // 1. No
            ws.Cell(currentRow, col++).Value = item.CustomerTaxCode;            // 2. Tax code
            ws.Cell(currentRow, col++).Value = item.CustomerName;               // 3. Customer name
            ws.Cell(currentRow, col++).Value = item.Nationality;                // 4. Nationality
            ws.Cell(currentRow, col++).Value = item.CustomerType;               // 5. Customer type
            ws.Cell(currentRow, col++).Value = item.Industry;                   // 6. Industry
            ws.Cell(currentRow, col++).Value = item.TypeOfBusiness;             // 7. Type of business

            // --- 2. Key Account Info ---
            ws.Cell(currentRow, col++).Value = item.KAType;                     // 8. KA type
            ws.Cell(currentRow, col++).Value = item.KAClass;                    // 9. KA Class

            // --- 3. DPO Info ---
            ws.Cell(currentRow, col++).Value = item.DPONo;                      // 10. DPO No.
            ws.Cell(currentRow, col++).Value = item.Distributor;                // 11. Distributor
            ws.Cell(currentRow, col++).Value = item.DPODate;                    // 12. DPO date
            ws.Cell(currentRow, col++).Value = item.SPOCode;                    // 13. SPOCode

            // --- 4. Product Info ---
            ws.Cell(currentRow, col++).Value = item.GolfaCode;                  // 14. Golfa code
            ws.Cell(currentRow, col++).Value = item.Model;                      // 15. Model
            ws.Cell(currentRow, col++).Value = item.MaterialType;               // 16. Material Type
            ws.Cell(currentRow, col++).Value = item.MaterialGroup;              // 17. Material Group

            // --- 5. DPO Metrics ---
            ws.Cell(currentRow, col++).Value = item.DPOQty;                    // 18. DPO Qty
            ws.Cell(currentRow, col++).Value = item.UnitStandardPrice;          // 19. Unit standard price (VND)
            ws.Cell(currentRow, col++).Value = item.DPOUnitPrice;              // 20. DPO Unit Price (VND)
            ws.Cell(currentRow, col++).Value = item.DPO_Amount;                 // 21. DPO Amount (VND)
            ws.Cell(currentRow, col++).Value = item.DiscountPercent;            // 22. Discount(%)

            // --- 6. Invoice / SO Info ---
            ws.Cell(currentRow, col++).Value = item.InvoiceNo;                  // 23. Invoice No.
            ws.Cell(currentRow, col++).Value = item.InvoiceDate;                // 24. Invoice Date
            ws.Cell(currentRow, col++).Value = item.InvoiceQty;                 // 25. Invoice Qty
            ws.Cell(currentRow, col++).Value = item.AmountVATInvoice;           // 26. Amount VAT invoice (VND)

            // --- 7. Margin & Cost ---
            ws.Cell(currentRow, col++).Value = item.UnitLandedCost;             // 27. Unit landed cost (VND)
            ws.Cell(currentRow, col++).Value = item.GPPercent;                  // 28. GP (%)

            currentRow++;
        }

        var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
        outputStream.Position = 0;

        return new RemoteStreamContent(
            outputStream,
            $"Sale_Report_{DateTime.Now:yyyyMMdd}.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    [Authorize(QuoteFlowPermissions.Reports.CustomerSaleReportGeneral)]
    public async Task<PagedResultDto<SaleReportByCustomerR05Dto>> GetListSaleReportGeneralAsync(SaleReportInput input)
    {
        var filterParams = ObjectMapper.Map<SaleReportInput, SaleReportFillterParams>(input);

        filterParams.HasFullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        filterParams.HasStrategicPriceAccess = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.Materials.ViewStrategicPrice);
        filterParams.UserName = _currentUser.Username;

        var items = await _salesAssignmentRepository.ExportSaleReportR05Async(filterParams);

        var itemDtos = ObjectMapper.Map<List<SaleReportByCustomerR05>, List<SaleReportByCustomerR05Dto>>(
            items.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()
        );

        var totalCount = items.Count;

        return new PagedResultDto<SaleReportByCustomerR05Dto>
        {
            TotalCount = totalCount,
            Items = itemDtos
        };
    }

    public async Task<IRemoteStreamContent> GetListSaleReportGeneralAsExcelAsync(SaleReportInput input)
    {
        // 1. Get Data
        var filterParams = ObjectMapper.Map<SaleReportInput, SaleReportFillterParams>(input);
        filterParams.HasFullBuyerAccess = await _buyerAccessService.HasFullBuyerAccessAsync();
        filterParams.HasStrategicPriceAccess = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.Materials.ViewStrategicPrice);
        filterParams.UserName = _currentUser.Username;

        var items = await _salesAssignmentRepository.ExportSaleReportR05Async(filterParams);
        var itemDtos = ObjectMapper.Map<List<SaleReportByCustomerR05>, List<SaleReportByCustomerR05Dto>>(items);

        // 2. Get the template file (Make sure you have a file named SaleReport.xlsx in DB)
        var fileDescriptor = await _fileDescriptorRepository
             .FirstOrDefaultAsync(fd => fd.Name == "R05_SalesReportByCustomer_General.xlsx")
             ?? throw new UserFriendlyException("Template Excel not found.");
        var templateBytes = await _fileDescriptorAppService.GetContentAsync(fileDescriptor.Id);

        // 3. Load into MemoryStream
        using var originalStream = new MemoryStream(templateBytes);
        using var tempStream = new MemoryStream();
        await originalStream.CopyToAsync(tempStream);
        tempStream.Position = 0;

        // 4. Open Workbook
        using var workbook = new ClosedXML.Excel.XLWorkbook(tempStream);
        var ws = workbook.Worksheets.First();

        // CONFIGURATION
        int startRow = 2; // The row where data starts printing
        int startCol = 1; // Column A

        // 5. Insert rows to preserve template formatting (borders, styles)
        if (itemDtos.Count > 1)
        {
            ws.Row(startRow).InsertRowsBelow(itemDtos.Count - 1);
        }

        // 6. Write data loop
        int currentRow = startRow;
        foreach (var item in itemDtos)
        {
            int col = startCol;

            // --- 1. Customer Info ---
            ws.Cell(currentRow, col++).Value = currentRow - 1;              // 1. No
            ws.Cell(currentRow, col++).Value = item.CustomerTaxCode;        // 2. Tax code
            ws.Cell(currentRow, col++).Value = item.CustomerName;           // 3. Customer name
            ws.Cell(currentRow, col++).Value = item.Nationality;            // 4. Nationality
            ws.Cell(currentRow, col++).Value = item.CustomerType;           // 5. Customer type
            ws.Cell(currentRow, col++).Value = item.Industry;               // 6. Industry
            ws.Cell(currentRow, col++).Value = item.TypeOfBusiness;         // 7. Type of business
            ws.Cell(currentRow, col++).Value = item.Buyer;                  // 7.1 Buyer

            // --- 2. Key Account Info ---
            ws.Cell(currentRow, col++).Value = item.KAType;                 // 8. KA Type
            ws.Cell(currentRow, col++).Value = item.KACode;                 // 9. KA Code
            ws.Cell(currentRow, col++).Value = item.KAClass;                // 10. KA Class

            // --- 3. Material Info ---
            ws.Cell(currentRow, col++).Value = item.MaterialType;           // 11. Material Type

            // --- 4. SPO Metrics ---
            ws.Cell(currentRow, col++).Value = item.SPOStandardAmount;      // 12. Standard amount (SPO)
            ws.Cell(currentRow, col++).Value = item.SPOSaleOfferAmount;    // 13. Sales Offer amount (SPO)
            ws.Cell(currentRow, col++).Value = item.DiscountPercent;        // 14. % Discount
            ws.Cell(currentRow, col++).Value = item.SPOLandedCostAmount;    // 15. Landed cost amount (SPO)
            ws.Cell(currentRow, col++).Value = item.SPOGP;                  // 16. % GP (SPO)

            // --- 5. DPO & Sales Metrics ---
            ws.Cell(currentRow, col++).Value = item.DPOAmount;              // 17. DPO Amount
            ws.Cell(currentRow, col++).Value = item.SOAmount;               // 18. Sale Amount (VND)
            ws.Cell(currentRow, col++).Value = item.DPOGP;                  // 19. % GP (DPO)
            ws.Cell(currentRow, col++).Value = item.SOGP;                   // 20. % GP (SO)

            currentRow++;
        }

        // 9. Save and Return
        var outputStream = new MemoryStream();
        workbook.SaveAs(outputStream);
        outputStream.Position = 0;

        return new RemoteStreamContent(
            outputStream,
            $"Sale_Report_{DateTime.Now:yyyyMMdd}.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    public async Task<SalesAssignmentDto> UpdateAsync(Guid id, SalesAssignmentUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<SalesAssignmentUpdateDto, SalesAssignmentUpdateParams>(input);
        var SalesAssignment = await _salesAssignmentManager.UpdateAsync(id, updateParams);
        return ObjectMapper.Map<SalesAssignment, SalesAssignmentDto>(SalesAssignment);
    }

}

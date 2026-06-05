using ClosedXML.Excel;
using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SystemCategories;
using QuoteFlow.SystemCategories.ParameterObjects;
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

namespace QuoteFlow.Customers;

[RemoteService(IsEnabled = false)]

public class CustomersAppService : QuoteFlowAppService, ICustomersAppService
{
    protected IDistributedCache<CustomerDownloadTokenCacheItem, string> _downloadTokenCache;
    protected ICustomerRepository _customerRepository;
    protected CustomerManager _customerManager;
    protected IExcelImportFactory _excelImportFactory;
    protected ISystemCategoryRepository _systemCategoryRepository;

    public CustomersAppService(ICustomerRepository customerRepository, CustomerManager customerManager, IDistributedCache<CustomerDownloadTokenCacheItem, string> downloadTokenCache, IExcelImportFactory excelImportFactory, ISystemCategoryRepository systemCategoryRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _customerRepository = customerRepository;
        _customerManager = customerManager;
        _excelImportFactory = excelImportFactory;
        _systemCategoryRepository = systemCategoryRepository;
    }

    public virtual async Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomersInput input)
    {
        var filterParams = ObjectMapper.Map<GetCustomersInput, CustomerFilterParams>(input);
        var totalCount = await _customerRepository.GetCountAsync(filterParams);
        var items = await _customerRepository.GetListAsync(filterParams, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<CustomerDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Customer>, List<CustomerDto>>(items)
        };
    }

    public virtual async Task<CustomerDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Customer, CustomerDto>(await _customerRepository.GetAsync(id));
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        await _customerRepository.DeleteAsync(id);
    }


    public virtual async Task<CustomerDto> CreateAsync(CustomerCreateDto input)
    {
        var createParams = ObjectMapper.Map<CustomerCreateDto, CustomerCreateParams>(input);
        var customer = await _customerManager.CreateAsync(createParams);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    public virtual async Task<ExcelValidationResult<CustomerImportDto>> ValidateAndParseCustomerAsync(IRemoteStreamContent file)
    {
        var validator = _excelImportFactory.CreateValidator<CustomerImportDto>(ExcelImporters.Customers);
        var context = new ExcelImportContext();

        await using var stream = file.GetStream();
        var result = await validator.ValidateAsync(stream, file.FileName ?? "", context);

        return result;
    }


    public async Task<List<CustomerDto>> ImportCustomerAsync(ExcelValidationResult<CustomerImportDto> dataImport)
    {
        var dataCreateObjects = _excelImportFactory.CreateCreateParamsConverter<CustomerImportDto, CustomerCreateParams>(ExcelImporters.Customers);
        var dataUpdateObjects = _excelImportFactory.CreateCreateParamsConverter<CustomerImportDto, CustomerUpdateParams>(ExcelImporters.CustomersUpdate);
        var context = new ExcelImportContext();
        //context.SetData(ExcelImportContextKeys.ParentEntityId, stockData.Id);

        var recordUpdate = new ExcelValidationResult<CustomerImportDto>(dataImport.SingleRow, dataImport.FileName)
        {
            Errors = dataImport.Errors,
            ListData = dataImport.ListData.Where(r => r.RowData?.IsUpdate == true).ToList()
        };
        var recordCreate = new ExcelValidationResult<CustomerImportDto>(dataImport.SingleRow, dataImport.FileName)
        {
            Errors = dataImport.Errors,
            ListData = dataImport.ListData.Where(r => r.RowData?.IsUpdate == false).ToList()
        };
        List<CustomerCreateParams> createParams = (await Task.WhenAll(
        recordCreate.ListData.Select(async x =>
        {
            var item = await dataCreateObjects.ConvertToCreateParamsAsync(x, context, default);
            return item;
        })
        )).Where(x => x != null)
        .ToList()!;

        List<CustomerUpdateParams> updateParams = (await Task.WhenAll(
        recordUpdate.ListData.Select(async x =>
        {
            var itemUpdate = await dataUpdateObjects.ConvertToCreateParamsAsync(x, context, default);
            return itemUpdate;
        })
        )).Where(x => x != null)
        .ToList()!;

        var result = await _customerManager.CreateManyAsync(createParams);
        var resultUpdate = await _customerManager.UpdateManyAsync(updateParams);
        var allResults = result.Concat(resultUpdate).ToList();

        return ObjectMapper.Map<List<Customer>, List<CustomerDto>>(allResults);

    }


    public virtual async Task<CustomerDto> UpdateAsync(Guid id, CustomerUpdateDto input)
    {
        var updateParams = ObjectMapper.Map<CustomerUpdateDto, CustomerUpdateParams>(input);
        var customer = await _customerManager.UpdateAsync(id, updateParams);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CustomerExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var filterParams = ObjectMapper.Map<CustomerExcelDownloadDto, CustomerFilterParams>(input);
        var items = await _customerRepository.GetListAsync(filterParams);

        // Map the entities to the DTOs first
        var customerDtos = ObjectMapper.Map<List<Customer>, List<CustomerExcelDto>>(items);

        // Call the custom Excel creation method
        var excelBytes = await CreateCustomerExcelFileAsync(customerDtos);

        var memoryStream = new MemoryStream(excelBytes);
        // memoryStream.Seek(0, SeekOrigin.Begin); // Not needed since we create a new MemoryStream from the bytes

        return new RemoteStreamContent(
            memoryStream,
            $"Customers_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", // Added timestamp to filename for uniqueness
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

    private async Task<byte[]> CreateCustomerExcelFileAsync(List<CustomerExcelDto> data)
    {
        // Fetch dropdown data from database
        var customerTypes = await _systemCategoryRepository.GetListAsync(
            new SystemCategoryFilterParams { CategoryType = CategoryTypes.CustomerType, IsDeactive = false }
        );
        var customerIndustries = await _systemCategoryRepository.GetListAsync(
            new SystemCategoryFilterParams { CategoryType = CategoryTypes.EUIndustry, IsDeactive = false }
        );

        var nationality = await _systemCategoryRepository.GetListAsync(
            new SystemCategoryFilterParams { CategoryType = CategoryTypes.Nationality, IsDeactive = false }
        );

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Customers");

        // Define headers (must match property order in CustomerExcelDto for simple mapping)
        var headers = new[]
        {
        "Tax Code", "Customer Name", "Short Name", "Address", "Phone",
        "Country", "Province", "Website", "Customer Type", "Customer Industry", "Note"
    };

        // Define column widths based on CustomerExcelDto attributes (if available) or a default set
        // Note: Since we don't have direct access to the [ExcelColumnWidth] attribute values here
        // without reflection, we'll use a hardcoded set based on your reference code's style.
        var columnWidths = new Dictionary<int, double>
    {
        { 1, 15 }, // Tax Code
        { 2, 40 }, // Customer Name
        { 3, 25 }, // Short Name
        { 4, 40 }, // Address
        { 5, 18 }, // Phone
        { 6, 15 }, // Country
        { 7, 18 }, // Province
        { 8, 25 }, // Website
        { 9, 20 }, // Customer Type
        { 10, 20 },// Customer Industry
        { 11, 30 } // Note
    };

        // --- 1. Add Headers with Styling ---
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightSkyBlue;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        // --- 2. Add Data Rows ---
        for (int row = 0; row < data.Count; row++)
        {
            var item = data[row];
            var excelRow = row + 2;
            int col = 1;

            worksheet.Cell(excelRow, col++).Value = item.TaxCode ?? "";
            worksheet.Cell(excelRow, col++).Value = item.CustomerName ?? "";
            worksheet.Cell(excelRow, col++).Value = item.CustomerShortName ?? "";
            worksheet.Cell(excelRow, col++).Value = item.Address ?? "";
            worksheet.Cell(excelRow, col++).Value = item.Phone ?? "";
            worksheet.Cell(excelRow, col++).Value = item.Country ?? "";
            worksheet.Cell(excelRow, col++).Value = item.Province ?? "";
            worksheet.Cell(excelRow, col++).Value = item.Website ?? "";
            worksheet.Cell(excelRow, col++).Value = item.CustomerType ?? "";
            worksheet.Cell(excelRow, col++).Value = item.CustomerIndustry ?? "";
            worksheet.Cell(excelRow, col++).Value = item.Note ?? "";
        }

        // --- 3. Set Column Widths ---
        foreach (var kvp in columnWidths)
        {
            worksheet.Column(kvp.Key).Width = kvp.Value;
        }

        // --- 4. Create Category Sheet with Dropdown Values ---
        var categorySheet = workbook.Worksheets.Add("Category");

        // Add Customer Type values to column A
        categorySheet.Cell(1, 1).Value = "Customer Type";
        categorySheet.Cell(1, 1).Style.Font.Bold = true;
        for (int i = 0; i < customerTypes.Count; i++)
        {
            categorySheet.Cell(i + 2, 1).Value = customerTypes[i].Description ?? "";
        }

        // Add Customer Industry values to column B
        categorySheet.Cell(1, 2).Value = "Customer Industry";
        categorySheet.Cell(1, 2).Style.Font.Bold = true;
        for (int i = 0; i < customerIndustries.Count; i++)
        {
            categorySheet.Cell(i + 2, 2).Value = customerIndustries[i].Description ?? "";
        }

        // Add Nationality values to column C
        categorySheet.Cell(1, 3).Value = "Nationality";
        categorySheet.Cell(1, 3).Style.Font.Bold = true;
        for (int i = 0; i < nationality.Count; i++)
        {
            categorySheet.Cell(i + 2, 3).Value = nationality[i].Description ?? "";
        }

        // --- 5. Apply Data Validation to Customer Type Column (Column 9) ---
        if (customerTypes.Count > 0)
        {
            var customerTypeRange = $"Category!$A$2:$A${customerTypes.Count + 1}";
            worksheet.Column(9).CreateDataValidation().List(customerTypeRange, true);
        }

        // --- 6. Apply Data Validation to Customer Industry Column (Column 10) ---
        if (customerIndustries.Count > 0)
        {
            var customerIndustryRange = $"Category!$B$2:$B${customerIndustries.Count + 1}";
            worksheet.Column(10).CreateDataValidation().List(customerIndustryRange, true);
        }

        // --- 7. Apply Data Validation to Country Column (Column 6) ---
        if (nationality.Count > 0)
        {
            var nationalityRange = $"Category!$C$2:$C${nationality.Count + 1}";
            worksheet.Column(6).CreateDataValidation().List(nationalityRange, true);
        }

        // --- 8. Hide Category Sheet ---
        categorySheet.Visibility = XLWorksheetVisibility.VeryHidden;

        // --- 9. Save to Memory Stream ---
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public virtual async Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new CustomerDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new QuoteFlow.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }
}
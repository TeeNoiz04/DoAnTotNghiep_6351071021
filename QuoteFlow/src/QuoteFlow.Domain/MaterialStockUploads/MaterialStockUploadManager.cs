using QuoteFlow.MaterialStockUploads.ParameterObjects;
using QuoteFlow.Shared.Utils;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.MaterialStockUploads;

public class MaterialStockUploadManager : DomainService
{
    protected IMaterialStockUploadRepository _materialStockUploadRepository;

    public MaterialStockUploadManager(IMaterialStockUploadRepository materialStockUploadRepository)
    {
        _materialStockUploadRepository = materialStockUploadRepository;
    }
    public async Task<string> GenerateNewCodeAsync(string prefix)
    {
        Check.NotNullOrWhiteSpace(prefix, nameof(prefix));

        var latestCode = await _materialStockUploadRepository.GetLatestCodeAsync(prefix);
        if (latestCode == null)
        {
            // code 001
            return GenerateCode(prefix, 1);
        }
        else
        {
            // Extract the numeric suffix from the latest code
            var numericSuffix = CodeHelper.ExtractNumericSuffix(latestCode);
            var (year, month) = ExtractYearMonthFromCode(latestCode);
            if (numericSuffix == -1 || year != DateTime.Now.Year || month != DateTime.Now.Month)
            {
                return GenerateCode(prefix, 1);
            }

            // Increment the count for the new code
            var newCount = numericSuffix + 1;
            return GenerateCode(prefix, newCount);
        }
    }
    private string GenerateCode(string prefix, int count)
    {
        var dateTime = DateTime.Now; // Use current date and time
        var year = dateTime.Year.ToString().Substring(2, 2); // Last two digits of the year
        var month = dateTime.Month.ToString("D2");

        if (count <= 0)
        {
            // draft code
            return $"{prefix}_{year}.{month}_xxx";
        }

        return $"{prefix}_{year}.{month}_{count:D3}";
    }

    private (int year, int month) ExtractYearMonthFromCode(string code)
    {
        Check.NotNullOrWhiteSpace(code, nameof(code));
        var parts = code.Split('_');
        if (parts.Length < 2)
        {
            throw new AbpException($"Invalid code format: '{code}'");
        }
        var datePart = parts[1]; // Get the part after the first underscore
        var dateParts = datePart.Split('.');
        if (dateParts.Length < 2)
        {
            throw new AbpException($"Invalid date format in code: '{code}'");
        }
        var year = int.Parse(dateParts[0]) + 2000;
        var month = int.Parse(dateParts[1]);
        return (year, month);
    }
    public virtual async Task<MaterialStockUpload> CreateAsync(
     MaterialStockUploadCreateParams createParams)
    {
        string newRequestNo = await GenerateNewCodeAsync(createParams.ImportType!);
        var materialStockUpload = new MaterialStockUpload(
            GuidGenerator.Create(),
            newRequestNo,
            createParams.ImportType,
            createParams.FileName,
            createParams.Note,
            createParams.Status
        );

        return await _materialStockUploadRepository.InsertAsync(materialStockUpload, autoSave: true);
    }
    public virtual async Task<MaterialStockUpload> UpdateAsync(
    Guid id,
    MaterialStockUploadUpdateParams updateParams)
    {
        var materialStockUpload = await _materialStockUploadRepository.GetAsync(id);

        materialStockUpload.RequestNo = updateParams.RequestNo;
        materialStockUpload.ImportType = updateParams.ImportType;
        materialStockUpload.FileName = updateParams.FileName;
        materialStockUpload.Note = updateParams.Note;
        materialStockUpload.Status = updateParams.Status;

        materialStockUpload.SetConcurrencyStampIfNotNull(updateParams.ConcurrencyStamp);

        return await _materialStockUploadRepository.UpdateAsync(materialStockUpload, autoSave: true);
    }


}
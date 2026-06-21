using QuoteFlow.Shared.Utils;
using QuoteFlow.SpoBatchRequests.ParameterObject;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SpoBatchRequests;

public class SpoBatchRequestManager : DomainService
{
    protected readonly ISpoBatchRequestRepository _spoBatchRequestRepository;

    public SpoBatchRequestManager(ISpoBatchRequestRepository spoBatchRequestRepository)
    {
        _spoBatchRequestRepository = spoBatchRequestRepository;
    }
    public async Task<string> GenerateNewCodeAsync(string prefix)
    {
        Check.NotNullOrWhiteSpace(prefix, nameof(prefix));

        var latestCode = await _spoBatchRequestRepository.GetLatestCodeAsync(prefix);
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
    public virtual async Task<SpoBatchRequest> CreateAsync(SpoBatchRequestCreateParams input)
    {
        string newRequestNo = await GenerateNewCodeAsync("SPO.BR");
        var spoBatchRequest = new SpoBatchRequest(
             GuidGenerator.Create(),
             newRequestNo,
             input
         );



        return await _spoBatchRequestRepository.InsertAsync(spoBatchRequest);
    }

    public virtual async Task<SpoBatchRequest> UpdateAsync(Guid id, SpoBatchRequestUpdateParams input)
    {


        var spoBatchRequest = await _spoBatchRequestRepository.GetAsync(id);

        spoBatchRequest.RequestNo = input.RequestNo;
        spoBatchRequest.ImportType = input.ImportType;
        spoBatchRequest.FileName = input.FileName;
        spoBatchRequest.Note = input.Note;
        spoBatchRequest.Status = input.Status;

        spoBatchRequest.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        return await _spoBatchRequestRepository.UpdateAsync(spoBatchRequest);
    }
}

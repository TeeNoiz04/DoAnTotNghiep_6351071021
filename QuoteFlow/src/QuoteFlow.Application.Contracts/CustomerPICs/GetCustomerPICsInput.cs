using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.CustomerPICs;

public class GetCustomerPICsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? KeyAccountId { get; set; }
    public string? PICName { get; set; }
    public string? PICPhone { get; set; }
    public string? PICEmail { get; set; }
    public string? PICJobTitle { get; set; }
    public string? Remark { get; set; }

    public GetCustomerPICsInput()
    {

    }
}
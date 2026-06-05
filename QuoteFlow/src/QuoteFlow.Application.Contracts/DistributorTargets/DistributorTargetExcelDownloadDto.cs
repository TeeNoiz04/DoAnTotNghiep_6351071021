using System;

namespace QuoteFlow.DistributorTargets;

public class DistributorTargetExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;

    public string? FilterText { get; set; }

    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerCode { get; set; }
    public string? MaterialType { get; set; }
    public int? FinanceYearMin { get; set; }
    public int? FinanceYearMax { get; set; }
    public decimal? FirstFYTargetMin { get; set; }
    public decimal? FirstFYTargetMax { get; set; }
    public decimal? SecondFYTargetMin { get; set; }
    public decimal? SecondFYTargetMax { get; set; }
    public string? Note { get; set; }

    public DistributorTargetExcelDownloadDto()
    {

    }
}
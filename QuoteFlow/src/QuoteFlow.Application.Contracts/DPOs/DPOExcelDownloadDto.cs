using System;

namespace QuoteFlow.DPOs;

public class DPOExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;

    public string? FilterText { get; set; }

    public string? DPONo { get; set; }
    public string? DPOType { get; set; }
    public string? DPOSubType { get; set; }
    public string? MaterialType { get; set; }
    public string? CostCenter { get; set; }
    public string? Status { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerShortName { get; set; }

    public string? BuyerDescription { get; set; }
    public DateTime? OrderDateMin { get; set; }
    public DateTime? OrderDateMax { get; set; }
    public decimal? TotalAmountMin { get; set; }
    public decimal? TotalAmountMax { get; set; }
    public string? Remark { get; set; }
    public string? FileName { get; set; }

    public DPOExcelDownloadDto()
    {

    }
}
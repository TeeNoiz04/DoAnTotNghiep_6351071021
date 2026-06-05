using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.Shared.Excels;
using System;

namespace QuoteFlow.DPOs;

public class ImportDPODto
{
    public string? MaterialType { get; set; }
    public string? DPONo { get; set; }
    public string? Remark { get; set; }
    public string? FileName { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }
    public DateTime ConfirmDate { get; set; }
    public ExcelValidationResult<ImportDPODetailDto> Details { get; set; } = new(false, "");
}
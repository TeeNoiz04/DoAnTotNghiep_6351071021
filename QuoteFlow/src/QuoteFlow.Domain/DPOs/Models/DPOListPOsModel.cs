using System;

namespace QuoteFlow.DPOs.Models;

public class DPOListPOsModel
{
    public Guid PODetailId { get; set; }
    public string PONo { get; set; } = null!;
    public string GolfaCode { get; set; } = null!;
    public DateTime PODate { get; set; }
    public decimal QtyAvailable { get; set; }
    public string? MachineNumber { get; set; }
    public string? STCReply { get; set; }
}
using System;
using System.Collections.Generic;

namespace QuoteFlow.DPOs;
public class GicGkrAllocationDto
{
    public Guid Id { get; set; }
    public string? MaterialType { get; set; }
    public string DPONo { get; set; } = null!;
    public Guid? BuyerId { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public string? BuyerTypeDescription { get; set; }
    public string? BuyerShortName { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Remark { get; set; }
    public string? LinkedNote { get; set; }
    public IEnumerable<GicGkrAllocationDetailDto> AllocationDetails { get; set; } = [];
}

public class GicGkrAllocationDetailDto
{
    public Guid Id { get; set; }
    public string GolfaCode { get; set; } = null!;
    public string Model { get; set; } = null!;

    public int GkrQty { get; set; }
    public int KeptQty { get; set; }
    public int OrderQty { get; set; }
    public int TakeQty { get; set; }
    public int ReleaseQty { get; set; }

    public string? Note { get; set; }
}

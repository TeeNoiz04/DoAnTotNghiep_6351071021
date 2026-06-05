using QuoteFlow.BuyerAccess;
using System;
using System.Collections.Generic;

namespace QuoteFlow.SaleOrders.ParameterObjects;
public class SaleOrderFilterParams : IBuyerRestrictable, IMaterialTypeRestrictable
{
    public string? SONo { get; set; }
    public string? SOSAPNo { get; set; }
    public string? MaterialType { get; set; }

    public string? DPONo { get; set; }
    public string? MaterialCode { get; set; }
    public string? InvoiceNo { get; set; }

    public string? BuyerType { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerCode { get; set; }
    public string? BuyerName { get; set; }
    public DateTime? OrderDateMin { get; set; }
    public DateTime? OrderDateMax { get; set; }
    public string? Model { get; set; }
    public string? TaxCode { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public DateTime? SODateFrom { get; set; }
    public DateTime? SODateTo { get; set; }
    public DateTime? VATDateFrom { get; set; }
    public DateTime? VATDateTo { get; set; }
    public string? MaterialGroup { get; set; }

    //public string? StatusCode { get; set; }
    public IEnumerable<string> StatusCodes { get; set; } = [];
    public string? SOType { get; set; }
    public string? GicType { get; set; }
    public string? GicProcess { get; set; }

    public virtual decimal? SAPGICLandingCost { get; set; }
    public virtual decimal? SAPGICAmountLandingCost { get; set; }
    public virtual string? SAPGICPORNo { get; set; }
    public virtual string? SAPGICPRNo { get; set; }
    public virtual string? SAPGICGivNo { get; set; }
    public virtual DateTime? SAPGICGivDate { get; set; }
    public virtual string? SAPGICSalesPIC { get; set; }
    public virtual string? SAPGICLocation { get; set; }
    public virtual string? SAPGICReservationNo { get; set; }
    public virtual string? SAPGICAssetClass { get; set; }
    public virtual string? SAPGICMainAssetCode { get; set; }
    public virtual string? SAPGICSubAssetCode { get; set; }
    public virtual string? SAPGICAssetName { get; set; }

    public virtual bool? CompletelyClosed { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
    public List<Guid> RestrictedBuyerIds { get; set; } = [];
    public List<string> RestrictedMaterialTypes { get; set; } = [];
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuoteFlow.SaleOrders;

public class SaleOrderCreateDto
{

    [StringLength(SaleOrderConsts.SOSAPNoMaxLength)]
    public string? SOSAPNo { get; set; }
    [StringLength(SaleOrderConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }

    [StringLength(SaleOrderConsts.MaterialTypeMaxLength)]
    public string? BuyerType { get; set; }
    public Guid? BuyerId { get; set; }
    [StringLength(SaleOrderConsts.BuyerCodeMaxLength)]
    public string? BuyerCode { get; set; }
    [StringLength(SaleOrderConsts.BuyerNameMaxLength)]
    public string? BuyerName { get; set; }
    public DateTime? OrderDate { get; set; }
    public Guid? StockCategoryId { get; set; }
    [JsonPropertyName("sO_VAT")]
    public decimal? SO_VAT { get; set; }
    [StringLength(SaleOrderConsts.SOSAPNoMaxLength)]
    public virtual string? SAPDONo { get; set; }
    [StringLength(SaleOrderConsts.SOSAPNoMaxLength)]
    public virtual string? SAPBillingNo { get; set; }
    [StringLength(SaleOrderConsts.SOSAPNoMaxLength)]
    public virtual string? SAPInvoice { get; set; }
    public virtual DateTime? SAPInvoiceDate { get; set; }
    public virtual bool? DeliveryConfirmed { get; set; }
    public virtual DateTime? SAPDeliveryDate { get; set; }
    [StringLength(SaleOrderConsts.NoteMaxLength)]
    public string? Note { get; set; }

    [StringLength(SaleOrderConsts.SOTypeMaxLength)]
    public virtual string? SOType { get; set; }

    [StringLength(SaleOrderConsts.GICTypeMaxLength)]
    public virtual string? GICType { get; set; }

    [StringLength(SaleOrderConsts.GICProcessMaxLength)]
    public virtual string? GICProcess { get; set; }


    public virtual decimal? SAPGICLandingCost { get; set; }


    [StringLength(SaleOrderConsts.SOSAPNoMaxLength)]
    public virtual string? GICGivNo { get; set; }
    public virtual DateTime? GICGivDate { get; set; }
    public virtual bool? CompletelyClosed { get; set; }

}
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.Shared;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SaleOrders;

public class SaleOrderDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string SONo { get; set; } = null!;
    public string? SOSAPNo { get; set; }
    public string? MaterialType { get; set; }
    public Guid? BuyerId { get; set; }

    public string? BuyerType { get; set; }
    public string? BuyerCode { get; set; }
    public string? BuyerName { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? StatusCode { get; set; }
    public Guid? StockCategoryId { get; set; }
    [JsonPropertyName("sO_VAT")]
    public decimal? SO_VAT { get; set; }
    public string? Note { get; set; }
    public virtual List<SaleOrderDetailDto>? SaleOrderDetails { get; set; }

    public virtual string? SAPDONo { get; set; }

    public virtual string? SAPBillingNo { get; set; }

    public virtual string? SAPInvoice { get; set; }
    public virtual DateTime? SAPInvoiceDate { get; set; }
    public virtual bool? DeliveryConfirmed { get; set; }
    public virtual DateTime? SAPDeliveryDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;
    public SaleOrderFlagsDto? Flags { get; set; }

    public virtual string? SOType { get; set; }
    public virtual string? GICType { get; set; }
    public virtual string? GICProcess { get; set; }



    public virtual string? GICGivNo { get; set; }
    public virtual DateTime? GICGivDate { get; set; }
    public virtual bool? CompletelyClosed { get; set; }

}
using QuoteFlow.Shared;
using QuoteFlow.SystemCategories;
using MiniExcelLibs.Attributes;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Buyers;


public class BuyerDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    [ExcelIgnore]
    public Guid BuyerTypeId { get; set; } = Guid.Empty!;
    public string? BuyerTypeCode { get; set; }
    [ExcelColumnWidth(40)]
    public string BuyerCode { get; set; } = null!;
    [ExcelColumnWidth(40)]
    public string? ShortName { get; set; }
    [ExcelColumnWidth(40)]
    public string? FullName { get; set; }
    [ExcelColumnWidth(40)]
    public string? TaxCode { get; set; }
    [ExcelColumnWidth(40)]
    public string? Address { get; set; }
    [ExcelColumnWidth(40)]
    public string? ContactPerson { get; set; }
    [ExcelColumnWidth(40)]
    public string? ContactEmail { get; set; }
    [ExcelColumnWidth(40)]
    public string? ContactPhoneNumber { get; set; }
    [ExcelColumnWidth(40)]
    public string? PaymentTermCode { get; set; }
    [ExcelColumnWidth(40)]
    public string? PaymentTermDescription { get; set; }
    [ExcelColumnWidth(40)]
    public decimal? CreditLimit { get; set; }
    [ExcelColumnWidth(40)]
    public decimal? CreditExposure { get; set; }
    [ExcelColumnWidth(40)]
    public decimal? AvailableCredit { get; set; }
    [ExcelColumnWidth(40)]
    public int? AppliedPrice { get; set; }
    [ExcelIgnore]
    public bool? Deactive { get; set; }

    [ExcelColumnWidth(40)]
    public string? Note { get; set; }
    [ExcelIgnore]
    public string ConcurrencyStamp { get; set; } = null!;

    [ExcelIgnore]
    public SystemCategoryListDto? BuyerType { get; set; }
}

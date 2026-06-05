using QuoteFlow.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SalesAssignments.ParameterObjects;

public class SalesAssignmentUpdateParams : BaseUpdateParams
{
    [Required]
    [StringLength(SalesAssignmentConsts.SaleUserNameMaxLength)]
    public string SaleUserName { get; set; } = null!;

    [StringLength(SalesAssignmentConsts.SaleFullNameMaxLength)]
    public string? SaleFullName { get; set; }

    [Required]
    [StringLength(SalesAssignmentConsts.MaterialTypeMaxLength)]
    public string MaterialType { get; set; } = null!;

    [Required]
    public Guid LocationId { get; set; }

    [Required]
    public Guid BuyerId { get; set; }

    [Required]
    public Guid BuyerTypeId { get; set; }

    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [StringLength(SalesAssignmentConsts.BuyerShortNameMaxLength)]
    public string? BuyerShortName { get; set; }
}
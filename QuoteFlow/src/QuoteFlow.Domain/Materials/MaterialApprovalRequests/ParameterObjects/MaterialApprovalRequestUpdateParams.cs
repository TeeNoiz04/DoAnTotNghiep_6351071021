using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
public class MaterialApprovalRequestUpdateParams
{
    [Required]
    [StringLength(MaterialApprovalRequestConsts.ImportTypeMaxLength)]
    public string ImportType { get; set; } = null!;
    [StringLength(MaterialApprovalRequestConsts.FileNameMaxLength)]
    public string? FileName { get; set; }
    [StringLength(MaterialApprovalRequestConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [StringLength(MaterialApprovalRequestConsts.StatusMaxLength)]
    public string? Status { get; set; }
    [Required]
    [StringLength(MaterialApprovalRequestConsts.RequestNoMaxLength)]
    public string RequestNo { get; set; } = null!;

    public string ConcurrencyStamp { get; set; } = null!;
}

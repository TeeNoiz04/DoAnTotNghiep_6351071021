using QuoteFlow.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SpoBatchRequests.ParameterObject;

public class SpoBatchRequestUpdateParams : BaseUpdateParams
{
    [Required]
    [StringLength(SpoBatchRequestConsts.RequestNoMaxLength)]
    public string RequestNo { get; set; } = null!;
    [Required]
    [StringLength(SpoBatchRequestConsts.ImportTypeMaxLength)]
    public string ImportType { get; set; } = null!;
    [StringLength(SpoBatchRequestConsts.FileNameMaxLength)]
    public string? FileName { get; set; }
    [StringLength(SpoBatchRequestConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [StringLength(SpoBatchRequestConsts.StatusMaxLength)]
    public string? Status { get; set; }

}

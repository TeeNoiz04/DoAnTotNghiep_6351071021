using QuoteFlow.Shared.Models;
using QuoteFlow.SpoBatchRequestDetails;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.Params;

public class SpoBatchRequestDetailUpdateParams : BaseUpdateParams
{
    [Required]
    public Guid RequestId { get; set; }
    [StringLength(SpoBatchRequestDetailConsts.SPOCodeMaxLength)]
    public string? SPOCode { get; set; }
    [StringLength(SpoBatchRequestDetailConsts.GolfaCodeMaxLength)]
    public string? GolfaCode { get; set; }
    [StringLength(SpoBatchRequestDetailConsts.ActionMaxLength)]
    public string? Action { get; set; }
    public DateTime? ActionDate { get; set; }
    [StringLength(SpoBatchRequestDetailConsts.NoteMaxLength)]
    public string? Note { get; set; }
}

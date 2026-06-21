using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using QuoteFlow.SpoBatchRequestDetails;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails
{
    public class SpoBatchRequestDetailUpdateDto : IHasConcurrencyStamp
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

        public string ConcurrencyStamp { get; set; } = null!;
    }
}
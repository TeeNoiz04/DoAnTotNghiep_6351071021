using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QuoteFlow.SpoBatchRequests
{
    public class SpoBatchRequestCreateDto
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
}
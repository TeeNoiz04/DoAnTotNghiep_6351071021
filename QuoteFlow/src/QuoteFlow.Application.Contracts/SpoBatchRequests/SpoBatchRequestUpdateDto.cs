using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SpoBatchRequests
{
    public class SpoBatchRequestUpdateDto : IHasConcurrencyStamp
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

        public string ConcurrencyStamp { get; set; } = null!;
    }
}
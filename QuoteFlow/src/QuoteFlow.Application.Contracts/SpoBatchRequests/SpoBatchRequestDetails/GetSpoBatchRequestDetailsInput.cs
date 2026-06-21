using Volo.Abp.Application.Dtos;
using System;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails
{
    public class GetSpoBatchRequestDetailsInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public Guid? RequestId { get; set; }
        public string? SPOCode { get; set; }
        public string? GolfaCode { get; set; }
        public string? Action { get; set; }
        public DateTime? ActionDateMin { get; set; }
        public DateTime? ActionDateMax { get; set; }
        public string? Note { get; set; }

        public GetSpoBatchRequestDetailsInput()
        {

        }
    }
}
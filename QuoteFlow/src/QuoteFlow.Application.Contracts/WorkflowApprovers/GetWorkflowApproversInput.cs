using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.WorkflowApprovers;

public class GetWorkflowApproversInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? WFId { get; set; }
    public string? Approver { get; set; }
    public string? Note { get; set; }

    public GetWorkflowApproversInput()
    {

    }
}
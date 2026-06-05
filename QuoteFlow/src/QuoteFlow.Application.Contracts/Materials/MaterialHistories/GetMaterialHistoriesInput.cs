using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Materials.MaterialHistories;

public class GetMaterialHistoriesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? MaterialId { get; set; }
    public string? Action { get; set; }
    public string? Note { get; set; }

    public GetMaterialHistoriesInput()
    {

    }
}
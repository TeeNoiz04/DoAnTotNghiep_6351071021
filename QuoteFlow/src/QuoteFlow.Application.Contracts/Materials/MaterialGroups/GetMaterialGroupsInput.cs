using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Materials.MaterialGroups;

public class GetMaterialGroupsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? Code { get; set; }
    public string? Name { get; set; }
    public Guid? Parent { get; set; }
    public int? SortOrderMin { get; set; }
    public int? SortOrderMax { get; set; }
    public string? Note { get; set; }
    public bool? IsDeActive { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialGroupPSI { get; set; }
    public bool? AllowKeyAccount { get; set; }

    public GetMaterialGroupsInput()
    {

    }
}
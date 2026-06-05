using Asp.Versioning;
using QuoteFlow.Seeders;
using QuoteFlow.SystemCategories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Controllers.SystemCategories;

[RemoteService]
[Area("app")]
[ControllerName("SystemCategory")]
[Route("api/app/system-categories")]

public class SystemCategoryController : AbpController, ISystemCategoriesAppService
{
    protected ISystemCategoriesAppService _systemCategoriesAppService;
    protected readonly List<SystemCategoryDto> _systemCategoryData;

    public SystemCategoryController(ISystemCategoriesAppService systemCategoriesAppService)
    {
        _systemCategoriesAppService = systemCategoriesAppService;

        var seeder = new SystemCategorySeeder();
        var seed = 1234;
        var count = 1000;
        _systemCategoryData = seeder.Generate(count, seed); // Generate 1000 system categories with a fixed seed
    }

    [HttpGet]
    public virtual Task<PagedResultDto<SystemCategoryDto>> GetListAsync(GetSystemCategoriesInput input)
    {
        return _systemCategoriesAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<SystemCategoryDto> GetAsync(Guid id)
    {
        //return _systemCategoriesAppService.GetAsync(id);

        // Simulate with in-memory data
        var systemCategory = _systemCategoryData.FirstOrDefault(c => c.Id == id)
            ?? throw new EntityNotFoundException(typeof(SystemCategoryDto), id);

        return Task.FromResult(systemCategory);
    }

    [HttpPost]
    public virtual Task<SystemCategoryDto> CreateAsync(SystemCategoryCreateDto input)
    {
        return _systemCategoriesAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<SystemCategoryDto> UpdateAsync(Guid id, SystemCategoryUpdateDto input)
    {
        return _systemCategoriesAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _systemCategoriesAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(SystemCategoryExcelDownloadDto input)
    {
        return _systemCategoriesAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _systemCategoriesAppService.GetDownloadTokenAsync();
    }

}
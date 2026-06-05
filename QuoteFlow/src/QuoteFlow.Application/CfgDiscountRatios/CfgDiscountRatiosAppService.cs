using QuoteFlow.CfgDiscountRatios.ParameterObjects;
using QuoteFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.CfgDiscountRatios;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.FAAdmins.CfgDiscountRatio)]
public class CfgDiscountRatiosAppService : QuoteFlowAppService, ICfgDiscountRatiosAppService
{

    protected ICfgDiscountRatioRepository _cfgDiscountRatioRepository;
    protected CfgDiscountRatioManager _cfgDiscountRatioManager;

    public CfgDiscountRatiosAppService(ICfgDiscountRatioRepository cfgDiscountRatioRepository, CfgDiscountRatioManager cfgDiscountRatioManager)
    {

        _cfgDiscountRatioRepository = cfgDiscountRatioRepository;
        _cfgDiscountRatioManager = cfgDiscountRatioManager;

    }

    public virtual async Task<PagedResultDto<CfgDiscountRatioDto>> GetListAsync(GetCfgDiscountRatiosInput input)
    {
        var filterParams = ObjectMapper.Map<GetCfgDiscountRatiosInput, CfgDiscountRatioFilterParams>(input);
        var totalCount = await _cfgDiscountRatioRepository.GetCountAsync(filterParams);
        var items = await _cfgDiscountRatioRepository.GetListAsync(filterParams);

        return new PagedResultDto<CfgDiscountRatioDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<CfgDiscountRatio>, List<CfgDiscountRatioDto>>(items)
        };
    }

    public virtual async Task<CfgDiscountRatioDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<CfgDiscountRatio, CfgDiscountRatioDto>(await _cfgDiscountRatioRepository.GetAsync(id));
    }

    [Authorize(QuoteFlowPermissions.FAAdmins.DeleteCfgDiscountRatio)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _cfgDiscountRatioRepository.DeleteAsync(id);
    }

    [Authorize(QuoteFlowPermissions.FAAdmins.CreateCfgDiscountRatio)]
    public virtual async Task<CfgDiscountRatioDto> CreateAsync(CfgDiscountRatioCreateDto input)
    {

        var cfgDiscountRatio = await _cfgDiscountRatioManager.CreateAsync(
        input.Approval_Type, input.Product_Type, input.AccountClassify, input.Value_Min, input.Value_Max, input.DiscountRatio, input.Note
        );

        return ObjectMapper.Map<CfgDiscountRatio, CfgDiscountRatioDto>(cfgDiscountRatio);
    }

    [Authorize(QuoteFlowPermissions.FAAdmins.EditCfgDiscountRatio)]
    public virtual async Task<CfgDiscountRatioDto> UpdateAsync(Guid id, CfgDiscountRatioUpdateDto input)
    {
        var updateParam = ObjectMapper.Map<CfgDiscountRatioUpdateDto, CfgDiscountRatioUpdateParams>(input);
        var cfgDiscountRatio = await _cfgDiscountRatioManager.UpdateAsync(id, updateParam);

        return ObjectMapper.Map<CfgDiscountRatio, CfgDiscountRatioDto>(cfgDiscountRatio);
    }
}
using Asp.Versioning;
using QuoteFlow.CfgDiscountRatios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.CfgDiscountRatios
{
    [RemoteService]
    [Area("app")]
    [ControllerName("CfgDiscountRatio")]
    [Route("api/app/cfg-discount-ratios")]

    public class CfgDiscountRatioController : AbpController, ICfgDiscountRatiosAppService
    {
        protected ICfgDiscountRatiosAppService _cfgDiscountRatiosAppService;

        public CfgDiscountRatioController(ICfgDiscountRatiosAppService cfgDiscountRatiosAppService)
        {
            _cfgDiscountRatiosAppService = cfgDiscountRatiosAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<CfgDiscountRatioDto>> GetListAsync(GetCfgDiscountRatiosInput input)
        {
            return _cfgDiscountRatiosAppService.GetListAsync(input);
        }

        //[HttpGet]
        //[Route("{id}")]
        //public virtual Task<CfgDiscountRatioDto> GetAsync(Guid id)
        //{
        //    return _cfgDiscountRatiosAppService.GetAsync(id);
        //}

        //[HttpPost]
        //public virtual Task<CfgDiscountRatioDto> CreateAsync(CfgDiscountRatioCreateDto input)
        //{
        //    return _cfgDiscountRatiosAppService.CreateAsync(input);
        //}

        [HttpPut]
        [Route("{id}")]
        public virtual Task<CfgDiscountRatioDto> UpdateAsync(Guid id, CfgDiscountRatioUpdateDto input)
        {
            return _cfgDiscountRatiosAppService.UpdateAsync(id, input);
        }

        //[HttpDelete]
        //[Route("{id}")]
        //public virtual Task DeleteAsync(Guid id)
        //{
        //    return _cfgDiscountRatiosAppService.DeleteAsync(id);
        //}
    }
}
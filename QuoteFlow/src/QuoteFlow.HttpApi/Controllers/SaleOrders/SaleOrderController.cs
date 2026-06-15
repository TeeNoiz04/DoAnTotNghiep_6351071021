using Asp.Versioning;
using QuoteFlow.SaleOrders;
using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.Shared.Excels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.SaleOrders;

[RemoteService]
[Area("app")]
[ControllerName("SaleOrder")]
[Route("api/app/sale-orders")]

public class SaleOrderController : AbpController, ISaleOrdersAppService
{
    protected ISaleOrdersAppService _saleOrdersAppService;

    public SaleOrderController(ISaleOrdersAppService saleOrdersAppService)
    {
        _saleOrdersAppService = saleOrdersAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<SaleOrderDto>> GetListAsync(GetSaleOrdersInput input)
    {
        return _saleOrdersAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<SaleOrderDto> GetAsync(Guid id)
    {
        return _saleOrdersAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<SaleOrderDto> CreateAsync(SaleOrderCreateDto input)
    {
        return _saleOrdersAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<SaleOrderDto> UpdateAsync(Guid id, SaleOrderUpdateDto input)
    {
        return _saleOrdersAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _saleOrdersAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("list-detail-dpo")]
    public Task<PagedResultDto<SaleOrderListDetailDPODto>> GetListDetailDPOAsync(GetSaleOrderListDetailDPOsInput input)
    {
        return _saleOrdersAppService.GetListDetailDPOAsync(input);
    }

    [HttpGet]
    [Route("list-detail-gic")]
    public Task<PagedResultDto<SaleOrderListDetailGICDto>> GetListDetailGICAsync(GetSaleOrderListDetailGICsInput input)
    {
        return _saleOrdersAppService.GetListDetailGICAsync(input);
    }

    [HttpPut]
    [Route("update-detail")]
    public Task<SaleOrderDetailDto> UpdateDetailAsync(Guid id, SaleOrderDetailUpdateDto input)
    {
        return _saleOrdersAppService.UpdateDetailAsync(id, input);
    }

    [HttpPut]
    [Route("update-note")]
    public Task<SaleOrderDetailDto> UpdateNoteAsync(Guid id, SaleOrderDetailUpdateDto input)
    {
        return _saleOrdersAppService.UpdateNoteAsync(id, input);
    }

    [HttpDelete]
    [Route("detail/{id}")]
    public Task DeleteDetailAsync(Guid id)
    {
        return _saleOrdersAppService.DeleteDetailAsync(id);
    }

    [HttpPost]
    [Route("added-list-detail-dpo")]
    public Task CreateDetailDPOAsync(List<SaleOrderAddedDetailDPODto> input)
    {
        return _saleOrdersAppService.CreateDetailDPOAsync(input);
    }


    [HttpPost]
    [Route("re-open-so")]
    public Task ReOpenSOAsync(Guid id)
    {
        return _saleOrdersAppService.ReOpenSOAsync(id);
    }

    [HttpPost]
    [Route("confirm-delivery/{id}")]
    public Task ConfirmDelivery(Guid id)
    {
        return _saleOrdersAppService.ConfirmDelivery(id);
    }

    [HttpPost]
    [Route("confirm-delivery-gic/{id}")]
    public Task ConfirmDeliveryGIC(Guid id)
    {
        return _saleOrdersAppService.ConfirmDeliveryGIC(id);
    }

    [HttpPost]
    [Route("validate-and-parse")]
    public Task<ExcelValidationResult<SaleOrderExcelDto>> ValidateAndParseAsync(IRemoteStreamContent file)
    {
        return _saleOrdersAppService.ValidateAndParseAsync(file);
    }
    [HttpPost]
    [Route("import")]
    public Task ImportSOAsync(ExcelValidationResult<SaleOrderExcelDto> data)
    {
        return _saleOrdersAppService.ImportSOAsync(data);
    }
    [HttpPost]
    [Route("export-so-gic-data")]
    public Task<IRemoteStreamContent> GetListGICAsExcelFileAsync(GetSaleOrdersInput input)
    {
        return _saleOrdersAppService.GetListGICAsExcelFileAsync(input);
    }

    [HttpPost]
    [Route("as-list-excel/export-sap-so")]
    public Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetSaleOrdersInput input)
    {
        return _saleOrdersAppService.GetListAsExcelFileAsync(input);
    }

    [HttpPost]
    [Route("as-list-excel/export-internal-use-change")]
    public Task<IRemoteStreamContent> GetListGICInternalUseChangeAsExcelFileAsync(GetSaleOrdersInput input)
    {
        return _saleOrdersAppService.GetListGICInternalUseChangeAsExcelFileAsync(input);
    }

    [HttpPut]
    [Route("so-detail-extrafee")]
    public Task UpdateSODetailExtrafeeAsync(SODetailExtrafeeUpdateInput input)
    {
        return _saleOrdersAppService.UpdateSODetailExtrafeeAsync(input);
    }

    //[HttpGet]
    //[Route("export-sap-data")]
    //public Task<List<SAPDataDto>> GetExpportSAPDataAsync(GetSaleOrdersInput input)
    //{
    //    return _saleOrdersAppService.GetExpportSAPDataAsync(input);
    //}
    [HttpPost]
    [Route("export-so-data")]
    public Task<IRemoteStreamContent> GetListSODataAsExcelFileAsync(GetSaleOrdersInput input)
    {
        return _saleOrdersAppService.GetListSODataAsExcelFileAsync(input);
    }
}
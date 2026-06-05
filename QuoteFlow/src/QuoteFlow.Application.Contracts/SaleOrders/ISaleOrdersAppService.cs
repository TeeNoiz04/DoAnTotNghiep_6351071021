using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace QuoteFlow.SaleOrders;
public interface ISaleOrdersAppService
{
    Task<PagedResultDto<SaleOrderDto>> GetListAsync(GetSaleOrdersInput input);
    Task<SaleOrderDto> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task DeleteDetailAsync(Guid id);
    Task<SaleOrderDto> CreateAsync(SaleOrderCreateDto input);
    Task<SaleOrderDto> UpdateAsync(Guid id, SaleOrderUpdateDto input);
    Task<PagedResultDto<SaleOrderListDetailDPODto>> GetListDetailDPOAsync(GetSaleOrderListDetailDPOsInput input);
    Task<PagedResultDto<SaleOrderListDetailGICDto>> GetListDetailGICAsync(GetSaleOrderListDetailGICsInput input);
    Task<SaleOrderDetailDto> UpdateDetailAsync(Guid id, SaleOrderDetailUpdateDto input);
    Task<SaleOrderDetailDto> UpdateNoteAsync(Guid id, SaleOrderDetailUpdateDto input);
    Task CreateDetailDPOAsync(List<SaleOrderAddedDetailDPODto> input);

    Task ReOpenSOAsync(Guid id);
    Task ConfirmDelivery(Guid id);
    Task ConfirmDeliveryGIC(Guid id);
    Task<ExcelValidationResult<SaleOrderExcelDto>> ValidateAndParseAsync(IRemoteStreamContent file);
    Task<IRemoteStreamContent> GetListGICAsExcelFileAsync(GetSaleOrdersInput input);
    Task<IRemoteStreamContent> GetListGICInternalUseChangeAsExcelFileAsync(GetSaleOrdersInput input);
    Task ImportSOAsync(ExcelValidationResult<SaleOrderExcelDto> data);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetSaleOrdersInput input);
    Task UpdateSODetailExtrafeeAsync(SODetailExtrafeeUpdateInput input);
    //Task<List<SAPDataDto>> GetExpportSAPDataAsync(GetSaleOrdersInput input);
    Task<IRemoteStreamContent> GetListSODataAsExcelFileAsync(GetSaleOrdersInput input);
    Task<ExcelValidationResult<SaleOrderGICWriteOffExcelDto>> ValidateAndParseGICWriteOffAsync(IRemoteStreamContent file, string gicType);

    Task ImportSOGICWriteOffAsync(ExcelValidationResult<SaleOrderGICWriteOffExcelDto> data);
    Task<ExcelValidationResult<SaleOrderGICWarrantyExcelDto>> ValidateAndParseGICWarrantyAsync(IRemoteStreamContent file, string gicType);

    Task ImportSOGICWarrantyAsync(ExcelValidationResult<SaleOrderGICWarrantyExcelDto> data);

    Task<ExcelValidationResult<SaleOrderGICInternalUseExcelDto>> ValidateAndParseGICInternalUseAsync(IRemoteStreamContent file, string gicType);

    Task ImportSOGICInternalUseAsync(ExcelValidationResult<SaleOrderGICInternalUseExcelDto> data);

    Task<ExcelValidationResult<SaleOrderGICInternalUseChangeExcelDto>> ValidateAndParseGICInternalUseChangeAsync(IRemoteStreamContent file, string gicType);

    Task ImportSOGICInternalUseChangeAsync(ExcelValidationResult<SaleOrderGICInternalUseChangeExcelDto> data);

    Task<ExcelValidationResult<SaleOrderGICFOCExcelDto>> ValidateAndParseGICFOCAsync(IRemoteStreamContent file, string gicType);

    Task ImportSOGICFOCAsync(ExcelValidationResult<SaleOrderGICFOCExcelDto> data);


}

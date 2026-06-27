
using QuoteFlow.Customers;
using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.DPOs;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
using QuoteFlow.Materials.MaterialImport.MaterialFactory;
using QuoteFlow.Materials.MaterialImport.MaterialSAP;
using QuoteFlow.Materials.MaterialImport.MaterialStatus;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.MaterialStockUploadDetails.ParameterObjects;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.ParameterObject;
using QuoteFlow.StockTracingDetails.ParameterObjects;
using QuoteFlow.StockTracings;
using QuoteFlow.SupplierBUs;
using QuoteFlow.SupplierBUs.ParameterObjects;
using Microsoft.Extensions.DependencyInjection;
using System;
using static QuoteFlow.ServiceKeys;

namespace QuoteFlow.Shared.Excels;

public class ExcelImportFactory : IExcelImportFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ExcelImportFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IExcelValidator<T> CreateValidator<T>(string validationType)
    {
        return validationType switch
        {
            ExcelImporters.PriceOfferPP => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PriceOfferImportDto>>(ExcelValidatorKeys.PriceOffers.PP),
            ExcelImporters.PriceOfferAddMoreItemDetail => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PriceOfferDetailImportDto>>(ExcelValidatorKeys.PriceOffers.DT),
            ExcelImporters.PriceOfferUpdateLandingCost => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PriceOfferUpdateLandingCostImportDto>>(ExcelValidatorKeys.PriceOffers.ULC),
            ExcelImporters.MaterialUpdatePrice => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialUpdatePriceImportDto>>(ExcelValidatorKeys.Materials.UP),
            ExcelImporters.MaterialNewRegistration => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialNewRegistrationImportDto>>(ExcelValidatorKeys.Materials.NR),
            ExcelImporters.MaterialUpdateWithoutPrice => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialUpdateWithoutPriceImportDto>>(ExcelValidatorKeys.Materials.WUP),
            ExcelImporters.MaterialUpdateInventoryPlan => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialUpdateInventoryPlanImportDto>>(ExcelValidatorKeys.Materials.UIP),
            ExcelImporters.StockTracingDelivery => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<StockTracingDeliveryImportDto>>(ExcelValidatorKeys.StocTracings.STD),
            ExcelImporters.StockTracingInventory => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<StockTracingInventoryImportDto>>(ExcelValidatorKeys.StocTracings.STI),
            ExcelImporters.StockTracingReceipt => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<StockTracingReceiptImportDto>>(ExcelValidatorKeys.StocTracings.STR),
            ExcelImporters.SupplierBU => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SupplierBUImportDto>>(ExcelValidatorKeys.SupplierBU.SBU),
            ExcelImporters.Customers => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<CustomerImportDto>>(ExcelValidatorKeys.Customer.CU),
            ExcelImporters.MaterialStockUploadDetailImportTransfer => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialStockUploadDetailImportTransferDto>>(ExcelValidatorKeys.StockManagement.TR),
            ExcelImporters.MaterialStockUploadDetailImportInventory => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialStockUploadDetailImportInventoryDto>>(ExcelValidatorKeys.StockManagement.IN),
            ExcelImporters.MaterialSAP => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialSAPUpdateExcelDto>>(ExcelValidatorKeys.Materials.SAP),
            ExcelImporters.MaterialFactory => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialFactoryUpdateExcelDto>>(ExcelValidatorKeys.Materials.FAC),
            ExcelImporters.MaterialStatus => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialStatusUpdateExcelDto>>(ExcelValidatorKeys.Materials.STA),
            ExcelImporters.DPO => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<ImportDPODto>>(ExcelValidatorKeys.DPOs.DPO),
            ExcelImporters.SaleOrders => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SaleOrderExcelDto>>(ExcelValidatorKeys.SaleOrders.SO),
            ExcelImporters.BatchRequest => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SpoBatchRequestDetailImportDto>>(ExcelValidatorKeys.PriceOffers.BR),
            _ => throw new ArgumentException($"Unknown validation type: {validationType}")
        };
    }

    public IExcelDtoConverter<TImportDto, TCreateParams> CreateCreateParamsConverter<TImportDto, TCreateParams>(string validationType)
        where TImportDto : class
        where TCreateParams : class
    {
        return validationType switch
        {
            ExcelImporters.PriceOfferPP => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>>(ExcelValidatorKeys.PriceOffers.PP),
            ExcelImporters.PriceOfferPPDetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>>(ExcelValidatorKeys.PriceOffers.PP),
            ExcelImporters.PriceOfferPPCustomer => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferCustomerImportDto, PriceOfferCustomerCreateParams>>(ExcelValidatorKeys.PriceOffers.PP),
            ExcelImporters.PriceOfferNBDetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>>(ExcelValidatorKeys.PriceOffers.NB),
            ExcelImporters.PriceOfferNBCustomer => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferCustomerImportDto, PriceOfferCustomerCreateParams>>(ExcelValidatorKeys.PriceOffers.NB),
            ExcelImporters.PriceOfferAddMoreItemDetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>>(ExcelValidatorKeys.PriceOffers.DT),
            ExcelImporters.PriceOfferUpdateLandingCost => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferUpdateLandingCostImportDto, PriceOfferDetailUpdateLandingCostParams>>(ExcelValidatorKeys.PriceOffers.ULC),
            ExcelImporters.MaterialNewRegistration => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<MaterialNewRegistrationImportDto, MaterialApprovalRequestDetailCreateParams>>(ExcelValidatorKeys.Materials.NR),
            ExcelImporters.MaterialUpdatePrice => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<MaterialUpdatePriceImportDto, ExcelMaterialUpdatePriceParams>>(ExcelValidatorKeys.Materials.UP),
            ExcelImporters.MaterialUpdateInventoryPlan => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<MaterialUpdateInventoryPlanImportDto, ExcelMaterialUpdateInventoryPlanUpdateParams>>(ExcelValidatorKeys.Materials.UIP),
            ExcelImporters.MaterialUpdateWithoutPrice => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<MaterialUpdateWithoutPriceImportDto, ExcelMaterialUpdateWithoutPrriceParams>>(ExcelValidatorKeys.Materials.WUP),
            ExcelImporters.SupplierBU => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SupplierBUImportDto, SupplierBUCreateParams>>(ExcelValidatorKeys.SupplierBU.SBU),
            ExcelImporters.Customers => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<CustomerImportDto, CustomerCreateParams>>(ExcelValidatorKeys.Customer.CU),
            ExcelImporters.SupplierBUUpdate => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SupplierBUImportDto, SupplierBUImportUpdateParams>>(ExcelValidatorKeys.SupplierBU.SBUU),
            ExcelImporters.CustomersUpdate => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<CustomerImportDto, CustomerUpdateParams>>(ExcelValidatorKeys.Customer.CUU),
            ExcelImporters.MaterialStockUploadDetailImportTransfer => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<MaterialStockUploadDetailImportTransferDto, MaterialStockUploadDetailCreateParams>>(ExcelValidatorKeys.StockManagement.TR),
            ExcelImporters.MaterialStockUploadDetailImportInventory => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<MaterialStockUploadDetailImportInventoryDto, MaterialStockUploadDetailCreateParams>>(ExcelValidatorKeys.StockManagement.IN),
            ExcelImporters.StockTracingDelivery => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<StockTracingDeliveryImportDto, StockTracingDetailCreateParams>>(ExcelValidatorKeys.StocTracings.STD),
            ExcelImporters.StockTracingInventory => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<StockTracingInventoryImportDto, StockTracingDetailCreateParams>>(ExcelValidatorKeys.StocTracings.STI),
            ExcelImporters.StockTracingReceipt => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<StockTracingReceiptImportDto, StockTracingDetailCreateParams>>(ExcelValidatorKeys.StocTracings.STR),

            ExcelImporters.MaterialSAP => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<MaterialSAPUpdateExcelDto, ExcelMaterialUpdateParams>>(ExcelValidatorKeys.Materials.SAP),
            ExcelImporters.MaterialFactory => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<MaterialFactoryUpdateExcelDto, ExcelMaterialFactoryUpdateParams>>(ExcelValidatorKeys.Materials.FAC),
            ExcelImporters.MaterialStatus => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<MaterialStatusUpdateExcelDto, ExcelMaterialStatusUpdateParams>>(ExcelValidatorKeys.Materials.STA),
            ExcelImporters.DPO => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<ImportDPODto, DPOCreateParams>>(ExcelValidatorKeys.DPOs.DPO),
            ExcelImporters.DPODetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<ImportDPODetailDto, DPODetailCreateParams>>(ExcelValidatorKeys.DPOs.DPO),
            ExcelImporters.SaleOrders => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SaleOrderExcelDto, SaleOrderSapImportCreateParams>>(ExcelValidatorKeys.SaleOrders.SO),
            ExcelImporters.BatchRequest => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SpoBatchRequestDetailImportDto, SpoBatchRequestDetailCreateParams>>(ExcelValidatorKeys.PriceOffers.BR),
         };
    }
}

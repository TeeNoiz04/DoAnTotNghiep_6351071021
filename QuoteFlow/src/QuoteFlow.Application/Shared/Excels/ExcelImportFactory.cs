using QuoteFlow.AssetRequestDetails.ParametersObjec;
using QuoteFlow.AssetRequests;
using QuoteFlow.Assets;
using QuoteFlow.Assets.ParameterObjects;
using QuoteFlow.Cargos;
using QuoteFlow.Cargos.CargoDatas.ParameterObjects;
using QuoteFlow.Customers;
using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.DPOs;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.GICs;
using QuoteFlow.GICs.GICDetails;
using QuoteFlow.GKRs;
using QuoteFlow.GKRs.GKRDetails;
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
using QuoteFlow.PSIs;
using QuoteFlow.PSIs.ParameterObjects;
using QuoteFlow.PSIs.PSIDetails;
using QuoteFlow.PSIs.PSIDetails.ParameterObject;
using QuoteFlow.PurchaseOrdersSapImports.Excel;
using QuoteFlow.PurchaseOrdersSapImports.ParameterObject;
using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.SpecialInputPrices;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails.ParameterObject;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.ParameterObject;
using QuoteFlow.StockImportDetails.ParameterObject;
using QuoteFlow.StockImportPriorities.Excel;
using QuoteFlow.StockImportPriorities.ParameterObjects;
using QuoteFlow.StockImports.Excel;
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
            ExcelImporters.PriceOfferAP => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PriceOfferImportDto>>(ExcelValidatorKeys.PriceOffers.AP),
            ExcelImporters.PriceOfferDS => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PriceOfferImportDto>>(ExcelValidatorKeys.PriceOffers.DS),
            ExcelImporters.PriceOfferNB => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PriceOfferImportDto>>(ExcelValidatorKeys.PriceOffers.NB),
            ExcelImporters.PriceOfferAddMoreItemDetail => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PriceOfferDetailImportDto>>(ExcelValidatorKeys.PriceOffers.DT),
            ExcelImporters.PriceOfferUpdateLandingCost => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PriceOfferUpdateLandingCostImportDto>>(ExcelValidatorKeys.PriceOffers.ULC),
            ExcelImporters.MaterialUpdatePrice => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialUpdatePriceImportDto>>(ExcelValidatorKeys.Materials.UP),
            ExcelImporters.MaterialNewRegistration => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialNewRegistrationImportDto>>(ExcelValidatorKeys.Materials.NR),
            ExcelImporters.MaterialUpdateWithoutPrice => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialUpdateWithoutPriceImportDto>>(ExcelValidatorKeys.Materials.WUP),
            ExcelImporters.MaterialUpdateInventoryPlan => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialUpdateInventoryPlanImportDto>>(ExcelValidatorKeys.Materials.UIP),
            ExcelImporters.StockTracingDelivery => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<StockTracingDeliveryImportDto>>(ExcelValidatorKeys.StocTracings.STD),
            ExcelImporters.StockTracingInventory => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<StockTracingInventoryImportDto>>(ExcelValidatorKeys.StocTracings.STI),
            ExcelImporters.StockTracingReceipt => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<StockTracingReceiptImportDto>>(ExcelValidatorKeys.StocTracings.STR),
            ExcelImporters.Cargo => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<CargoImportDto>>(ExcelValidatorKeys.Cargos.CA),
            ExcelImporters.Asset => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<AssetImportDto>>(ExcelValidatorKeys.Assets.AS),
            ExcelImporters.AssetAudit => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<AssetAuditImportDto>>(ExcelValidatorKeys.Assets.ASA),
            ExcelImporters.SupplierBU => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SupplierBUImportDto>>(ExcelValidatorKeys.SupplierBU.SBU),
            ExcelImporters.Customers => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<CustomerImportDto>>(ExcelValidatorKeys.Customer.CU),
            ExcelImporters.MaterialStockUploadDetailImportTransfer => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialStockUploadDetailImportTransferDto>>(ExcelValidatorKeys.StockManagement.TR),
            ExcelImporters.MaterialStockUploadDetailImportInventory => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialStockUploadDetailImportInventoryDto>>(ExcelValidatorKeys.StockManagement.IN),
            ExcelImporters.MaterialSAP => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialSAPUpdateExcelDto>>(ExcelValidatorKeys.Materials.SAP),
            ExcelImporters.MaterialFactory => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialFactoryUpdateExcelDto>>(ExcelValidatorKeys.Materials.FAC),
            ExcelImporters.MaterialStatus => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<MaterialStatusUpdateExcelDto>>(ExcelValidatorKeys.Materials.STA),
            ExcelImporters.GICWarranty => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<GICImportDto>>(ExcelValidatorKeys.GIC.WR),
            ExcelImporters.GICSponsor => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<GICImportDto>>(ExcelValidatorKeys.GIC.SP),
            ExcelImporters.GICInternal => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<GICImportDto>>(ExcelValidatorKeys.GIC.IN),
            ExcelImporters.GICWriteOff => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<GICImportDto>>(ExcelValidatorKeys.GIC.WO),
            ExcelImporters.GICWriteOffDetail => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<GICDetailImportDto>>(ExcelValidatorKeys.GIC.WO),
            ExcelImporters.DPO => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<ImportDPODto>>(ExcelValidatorKeys.DPOs.DPO),
            ExcelImporters.GKR => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<GKRImportDto>>(ExcelValidatorKeys.GKRs.GKR),
            ExcelImporters.PSI_FA => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PSIImportDto>>(ExcelValidatorKeys.PSI.FA),
            ExcelImporters.PSI_LVS => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PSIImportDto>>(ExcelValidatorKeys.PSI.LVS),
            ExcelImporters.StockImport => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<StockImportExcelDto>>(ExcelValidatorKeys.StokImport.SI),
            ExcelImporters.SpecialInputPrice => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SpecialInputPriceDetailImportDto>>(ExcelValidatorKeys.Special.SIP),
            ExcelImporters.StockImportPriority => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<StockImportPriorityExcelDto>>(ExcelValidatorKeys.StokImportPriority.SIP),
            ExcelImporters.SaleOrders => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SaleOrderExcelDto>>(ExcelValidatorKeys.SaleOrders.SO),
            ExcelImporters.SaleOrderGICWarranty => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SaleOrderGICWarrantyExcelDto>>(ExcelValidatorKeys.SaleOrders.WR),
            ExcelImporters.SaleOrderGICInternalUse => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SaleOrderGICInternalUseExcelDto>>(ExcelValidatorKeys.SaleOrders.IU),
            ExcelImporters.SaleOrderGICFOC => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SaleOrderGICFOCExcelDto>>(ExcelValidatorKeys.SaleOrders.FOC),
            ExcelImporters.SaleOrderGICWriteOff => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SaleOrderGICWriteOffExcelDto>>(ExcelValidatorKeys.SaleOrders.WO),
            ExcelImporters.PurchaseOrders => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<PurchaseOrdersSapImportsExcelDto>>(ExcelValidatorKeys.PurchaseOrders.PO),
            ExcelImporters.BatchRequest => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SpoBatchRequestDetailImportDto>>(ExcelValidatorKeys.PriceOffers.BR),
            ExcelImporters.SaleOrderGICInternalUseChange => (IExcelValidator<T>)_serviceProvider.GetRequiredKeyedService<IExcelValidator<SaleOrderGICInternalUseChangeExcelDto>>(ExcelValidatorKeys.SaleOrders.IUC),
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
            ExcelImporters.PriceOfferAP => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>>(ExcelValidatorKeys.PriceOffers.AP),
            ExcelImporters.PriceOfferDS => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>>(ExcelValidatorKeys.PriceOffers.DS),
            ExcelImporters.PriceOfferNB => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>>(ExcelValidatorKeys.PriceOffers.NB),
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
            ExcelImporters.Cargo => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<CargoImportDto, CargoDataCreateParams>>(ExcelValidatorKeys.Cargos.CA),
            ExcelImporters.Asset => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<AssetImportDto, AssetCreateParams>>(ExcelValidatorKeys.Assets.AS),
            ExcelImporters.AssetUpdate => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<AssetImportDto, AssetUpdateParams>>(ExcelValidatorKeys.Assets.ASU),
            ExcelImporters.AssetAudit => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<AssetAuditImportDto, AssetRequestDetailUpdateParams>>(ExcelValidatorKeys.Assets.ASA),
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
            ExcelImporters.GICWarranty => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GICImportDto, GICCreateParams>>(ExcelValidatorKeys.GIC.WR),
            ExcelImporters.GICWarrantyDetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GICDetailImportDto, DPODetailCreateParams>>(ExcelValidatorKeys.GIC.WR),
            ExcelImporters.GICSponsor => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GICImportDto, GICCreateParams>>(ExcelValidatorKeys.GIC.SP),
            ExcelImporters.GICSponsorDetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GICDetailImportDto, DPODetailCreateParams>>(ExcelValidatorKeys.GIC.SP),
            ExcelImporters.GICInternal => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GICImportDto, GICCreateParams>>(ExcelValidatorKeys.GIC.IN),
            ExcelImporters.GICInternalDetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GICDetailImportDto, DPODetailCreateParams>>(ExcelValidatorKeys.GIC.IN),
            ExcelImporters.GICWriteOff => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GICImportDto, GICCreateParams>>(ExcelValidatorKeys.GIC.WO),
            ExcelImporters.GICWriteOffDetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GICDetailImportDto, DPODetailCreateParams>>(ExcelValidatorKeys.GIC.WO),
            ExcelImporters.DPO => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<ImportDPODto, DPOCreateParams>>(ExcelValidatorKeys.DPOs.DPO),
            ExcelImporters.DPODetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<ImportDPODetailDto, DPODetailCreateParams>>(ExcelValidatorKeys.DPOs.DPO),

            ExcelImporters.GKR => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GKRImportDto, GKRCreateParams>>(ExcelValidatorKeys.GKRs.GKR),
            ExcelImporters.GKRDetail => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<GKRDetailImportDto, DPODetailCreateParams>>(ExcelValidatorKeys.GKRs.GKR),

            ExcelImporters.PSIDetail_FA => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PSIDetailImportDto, PSIDetailsCreateParams>>(ExcelValidatorKeys.PSI.FA),
            ExcelImporters.PSIDetail_LVS => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PSIDetailImportDto, PSIDetailsCreateParams>>(ExcelValidatorKeys.PSI.LVS),
            ExcelImporters.PSI_FA => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PSIImportDto, PSICreateParams>>(ExcelValidatorKeys.PSI.FA),
            ExcelImporters.PSI_LVS => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PSIImportDto, PSICreateParams>>(ExcelValidatorKeys.PSI.LVS),

            ExcelImporters.StockImport => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<StockImportExcelDto, StockImportDetailCreateParams>>(ExcelValidatorKeys.StokImport.SI),
            ExcelImporters.SpecialInputPrice => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SpecialInputPriceDetailImportDto, SpecialInputPriceDetailCreateParams>>(ExcelValidatorKeys.Special.SIP),
            ExcelImporters.StockImportPriority => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<StockImportPriorityExcelDto, StockImportPriorityCreateParams>>(ExcelValidatorKeys.StokImportPriority.SIP),
            ExcelImporters.SaleOrders => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SaleOrderExcelDto, SaleOrderSapImportCreateParams>>(ExcelValidatorKeys.SaleOrders.SO),
            ExcelImporters.SaleOrderGICWarranty => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SaleOrderGICWarrantyExcelDto, SaleOrderSapImportCreateParams>>(ExcelValidatorKeys.SaleOrders.WR),
            ExcelImporters.SaleOrderGICInternalUse => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SaleOrderGICInternalUseExcelDto, SaleOrderSapImportCreateParams>>(ExcelValidatorKeys.SaleOrders.IU),
            ExcelImporters.SaleOrderGICFOC => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SaleOrderGICFOCExcelDto, SaleOrderSapImportCreateParams>>(ExcelValidatorKeys.SaleOrders.FOC),
            ExcelImporters.SaleOrderGICWriteOff => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SaleOrderGICWriteOffExcelDto, SaleOrderSapImportCreateParams>>(ExcelValidatorKeys.SaleOrders.WO),
            ExcelImporters.PurchaseOrders => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<PurchaseOrdersSapImportsExcelDto, PurchaseOrderSapImportCreateParams>>(ExcelValidatorKeys.PurchaseOrders.PO),
            ExcelImporters.BatchRequest => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SpoBatchRequestDetailImportDto, SpoBatchRequestDetailCreateParams>>(ExcelValidatorKeys.PriceOffers.BR),
            ExcelImporters.SaleOrderGICInternalUseChange => (IExcelDtoConverter<TImportDto, TCreateParams>)_serviceProvider.GetRequiredKeyedService<IExcelDtoConverter<SaleOrderGICInternalUseChangeExcelDto, SaleOrderSapImportCreateParams>>(ExcelValidatorKeys.SaleOrders.IUC),
        };
    }
}

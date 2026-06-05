using AutoMapper;
using QuoteFlow.AddMoreItemHistories;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalRoutes;
using QuoteFlow.AssetRequestDetails;
using QuoteFlow.AssetRequestDetails.ParametersObjec;
using QuoteFlow.AssetRequests;
using QuoteFlow.AssetRequests.ParameterObjects;
using QuoteFlow.Assets;
using QuoteFlow.Assets.ParameterObjects;
using QuoteFlow.Attachments;
using QuoteFlow.Buyers;
using QuoteFlow.Buyers.ParameterObjects;
using QuoteFlow.Cargos;
using QuoteFlow.Cargos.CargoDatas;
using QuoteFlow.Cargos.CargoDatas.ParameterObjects;
using QuoteFlow.Cargos.ParameterObjects;
using QuoteFlow.CfgDiscountRatios;
using QuoteFlow.CfgDiscountRatios.ParameterObjects;
using QuoteFlow.CustomerPICs;
using QuoteFlow.Customers;
using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.Dashboards;
using QuoteFlow.DistributorTargets;
using QuoteFlow.DistributorTargets.ParameterObjects;
using QuoteFlow.DPOs;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.Models;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.GICs;
using QuoteFlow.GICs.GICDetails;
using QuoteFlow.GKRs;
using QuoteFlow.GKRs.GKRDetails;
using QuoteFlow.HistoryTrackings;
using QuoteFlow.KeyAccountEvaluations;
using QuoteFlow.KeyAccounts;
using QuoteFlow.KeyAccounts.KeyAccountDetailReports;
using QuoteFlow.KeyAccounts.KeyAccountDetailReports.ParameterObjects;
using QuoteFlow.KeyAccounts.KeyAccountGeneralReports;
using QuoteFlow.KeyAccounts.KeyAccountGeneralReports.ParameterObjects;
using QuoteFlow.KeyAccounts.KeyAccountUpgrades;
using QuoteFlow.KeyAccounts.KeyAccountUpgrades.ParameterObjects;
using QuoteFlow.KeyAccounts.ParameterObjects;
using QuoteFlow.MaterialGroupBuyers;
using QuoteFlow.MaterialGroupBuyers.ParameterObjects;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.Materials.MaterialGroups.ParameterObject;
using QuoteFlow.Materials.MaterialHistories;
using QuoteFlow.Materials.MaterialImport.MaterialStatus;
using QuoteFlow.Materials.MaterialStocks;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks.ParameterObjects;
using QuoteFlow.Materials.MaterialStocks.ParameterObjects;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.MaterialStockUploads;
using QuoteFlow.MaterialStockUploads.ParameterObjects;
using QuoteFlow.Messages;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferReportDetails;
using QuoteFlow.PriceOffers.PriceOfferReportDetails.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals.ParameterObjects;
using QuoteFlow.PSIs;
using QuoteFlow.PSIs.ParameterObjects;
using QuoteFlow.PSIs.PSIDetails;
using QuoteFlow.PSIs.PSIDetails.ParameterObject;
using QuoteFlow.PurchaseOrderDetails;
using QuoteFlow.PurchaseOrderDetails.ParameterObjects;
using QuoteFlow.PurchaseOrderLockShipments;
using QuoteFlow.PurchaseOrders;
using QuoteFlow.PurchaseOrders.ParameterObjects;
using QuoteFlow.PurchaseOrders.PurchaseOrderDetails;
using QuoteFlow.PurchaseOrdersSapImports;
using QuoteFlow.PurchaseOrdersSapImports.ParameterObject;
using QuoteFlow.SaleOrders;
using QuoteFlow.SaleOrders.ParameterObjects;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.SaleOrders.SaleOrderDetails.ParameterObjects;
using QuoteFlow.SaleOrdersSapImports;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.SalesAssignments;
using QuoteFlow.SalesAssignments.ParameterObjects;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SpecialInputPrices;
using QuoteFlow.SpecialInputPrices.ParameterObject;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails.ParameterObject;
using QuoteFlow.SpoBatchRequests;
using QuoteFlow.SpoBatchRequests.ParameterObject;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using QuoteFlow.StockCategories;
using QuoteFlow.StockCategories.ParameterObjects;
using QuoteFlow.StockImportAllocations;
using QuoteFlow.StockImportDetails;
using QuoteFlow.StockImportDetails.ParameterObject;
using QuoteFlow.StockImportPriorities;
using QuoteFlow.StockImportPriorities.ParameterObjects;
using QuoteFlow.StockImports;
using QuoteFlow.StockImports.ParameterObjects;
using QuoteFlow.StockManagements;
using QuoteFlow.StockTracingDetails;
using QuoteFlow.StockTracingDetails.ParameterObjects;
using QuoteFlow.StockTracings;
using QuoteFlow.StockTracings.ParameterObjects;
using QuoteFlow.SupplierBUs;
using QuoteFlow.SupplierBUs.ParameterObjects;
using QuoteFlow.Suppliers;
using QuoteFlow.Suppliers.ParameterObject;
using QuoteFlow.SystemCategories;
using QuoteFlow.SystemCategories.ParameterObjects;
using QuoteFlow.SystemConfigurations;
using QuoteFlow.WorkflowApprovers;
using QuoteFlow.WorkflowConfigurations;
using QuoteFlow.WorkflowConfigurations.ParameterObject;
using System.Linq;

namespace QuoteFlow;

public class QuoteFlowApplicationAutoMapperProfile : Profile
{
    public QuoteFlowApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Buyer, BuyerDto>();
        CreateMap<Buyer, BuyerListDto>();
        CreateMap<BuyerCreateDto, BuyerCreateParams>();
        CreateMap<GetBuyersInput, BuyerFilterParams>();
        CreateMap<BuyerUpdateDto, BuyerUpdateParams>();

        CreateMap<SalesAssignment, SalesAssignmentDto>();
        //CreateMap<SalesAssignment, BuyerExcelDto>();
        CreateMap<SalesAssignmentCreateDto, SalesAssignmentCreateParams>();
        CreateMap<GetSalesAssignmentInput, SalesAssignmentFilterParams>();
        //CreateMap<SalesAssignmentExcelDownloadDto, SalesAssignmentFilterParams>();
        CreateMap<SalesAssignmentUpdateDto, SalesAssignmentUpdateParams>();

        CreateMap<KeyAccount, KeyAccountDto>()
             .ForMember(dest => dest.KeyAccountEvaluationFinancial, opt => opt.MapFrom(src => src.KeyAccountEvaluation.Where(x => x.EvaluationType == EvaluationTypes.Financial).ToList()))
             .ForMember(dest => dest.KeyAccountEvaluationProduct, opt => opt.MapFrom(src => src.KeyAccountEvaluation.Where(x => x.EvaluationType == EvaluationTypes.Product).ToList()));

        CreateMap<KeyAccount, KeyAccountListDto>();
        CreateMap<KeyAccountApprovalHistory, ApprovalHistoryDto>();
        CreateMap<KeyAccount, KeyAccountWithNavigationListDto>();
        CreateMap<KeyAccount, KeyAccountExcelDto>();
        CreateMap<KeyAccountCreateDto, KeyAccountCreateParams>();
        CreateMap<GetKeyAccountsInput, KeyAccountFilterParams>();
        CreateMap<KeyAccountExcelDownloadDto, KeyAccountFilterParams>();
        CreateMap<KeyAccountUpdateDto, KeyAccountUpdateParams>();
        CreateMap<KeyAccountEvaluationDto, KeyAccountEvaluation>();
        CreateMap<KeyAccountEvaluation, KeyAccountEvaluationDto>();

        CreateMap<SystemCategory, SystemCategoryDto>();
        CreateMap<SystemCategory, SystemCategoryListDto>();
        CreateMap<SystemCategory, SystemCategoryExcelDto>();
        CreateMap<SystemCategoryCreateDto, SystemCategoryCreateParams>();
        CreateMap<GetSystemCategoriesInput, SystemCategoryFilterParams>();
        CreateMap<SystemCategoryExcelDownloadDto, SystemCategoryFilterParams>();
        CreateMap<SystemCategoryUpdateDto, SystemCategoryUpdateParams>();

        CreateMap<StockTracing, StockTracingDto>();
        CreateMap<StockTracing, StockTracingExcelDto>();
        CreateMap<StockTracingCreateDto, StockTracingCreateParams>();
        CreateMap<GetStockTracingsInput, StockTracingFilterParams>();
        CreateMap<StockTracingExcelDownloadDto, StockTracingFilterParams>();
        CreateMap<StockTracingUpdateDto, StockTracingUpdateParams>();

        CreateMap<StockTracingDetail, StockTracingDetailDto>();
        CreateMap<StockTracingDetail, StockTracingDetailExcelDto>();
        CreateMap<StockTracingDetailCreateDto, StockTracingDetailCreateParams>();
        CreateMap<GetStockTracingDetailsInput, StockTracingDetailFilterParams>();
        CreateMap<StockTracingDetailExcelDownloadDto, StockTracingDetailFilterParams>();
        CreateMap<StockTracingDetailUpdateDto, StockTracingDetailUpdateParams>();

        CreateMap<PSI, PSIDto>();
        CreateMap<PSI, PSIExcelDto>();
        CreateMap<PSICreateDto, PSICreateParams>();
        CreateMap<GetPSIsInput, PSIFilterParams>();
        CreateMap<PSIExcelDownloadDto, PSIFilterParams>();
        CreateMap<PSIUpdateDto, PSIUpdateParams>();
        CreateMap<GetPSIReportsInput, PSIReportFilterParams>();
        CreateMap<PSIReport, PSIReportDto>();
        CreateMap<PSIExportData, PSIExportDto>();
        CreateMap<PSIExportData, PSIExportDataDto>();

        CreateMap<GetMaterialsInput, MaterialFilterParams>();
        CreateMap<GetMaterialsApprovalInput, MaterialApprovalRequestFilterParams>();
        CreateMap<MaterialApprovalRequestRouteDto, MaterialApprovalRequestRoute>();
        CreateMap<MaterialApprovalRequestCreateDto, MaterialApprovalRequestCreateParams>();
        CreateMap<MaterialApprovalRequestUpdateDto, MaterialApprovalRequestUpdateParams>();
        CreateMap<MaterialApprovalRequestSubmitDto, MaterialApprovalRequestSubmitParams>();
        CreateMap<MaterialApprovalRequest, MaterialApprovalRequestDto>();
        CreateMap<ExcelValidationResult<MaterialNewRegistrationImportDto>, MaterialApprovalRequestCreateParams>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName));
        CreateMap<ExcelValidationResult<MaterialUpdatePriceImportDto>, MaterialApprovalRequestCreateParams>()
           .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName));
        CreateMap<ExcelValidationResult<MaterialUpdateWithoutPriceImportDto>, MaterialApprovalRequestCreateParams>()
           .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName));
        CreateMap<ExcelValidationResult<MaterialStatusUpdateExcelDto>, MaterialApprovalRequestCreateParams>()
          .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName));
        CreateMap<ExcelValidationResult<MaterialUpdateInventoryPlanImportDto>, MaterialApprovalRequestCreateParams>()
         .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName));
        CreateMap<GetMaterialsInput, MaterialFilterParams>();
        CreateMap<Material, MaterialExportExcelDto>();

        CreateMap<MaterialApprovalRequestDetailCreateDto, MaterialApprovalRequestDetailCreateParams>();

        CreateMap<MaterialApprovalRequestDetailUpdateDto, MaterialApprovalRequestDetailUpdateParams>();
        CreateMap<MaterialApprovalRequestDetail, MaterialApprovalRequestDetailDto>();
        CreateMap<MaterialCreateDto, MaterialCreateParams>();
        CreateMap<MaterialUpdateDto, MaterialUpdateParams>();
        CreateMap<Material, MaterialDto>();
        CreateMap<Material, MaterialExcelDto>();

        CreateMap<KeyAccountEvaluation, KeyAccountEvaluationDto>();
        CreateMap<KeyAccountEvaluation, KeyAccountEvaluationListDto>();
        CreateMap<KeyAccountEvaluation, KeyAccountEvaluationExcelDto>();
        CreateMap<KeyAccountDetailReport, KeyAccountDetailReportDto>()
        .ForMember(dest => dest.MaterialGroup, opt => opt.MapFrom(src => src.MaterialGroupName))
        .ForMember(dest => dest.DPOQty, opt => opt.MapFrom(src => src.DPO_Qty))
        .ForMember(dest => dest.DPOUnitPrice, opt => opt.MapFrom(src => src.DPO_UnitPrice))
        .ForMember(dest => dest.DPOAmount, opt => opt.MapFrom(src => src.DPO_Amount))
        .ForMember(dest => dest.AmountDO, opt => opt.MapFrom(src => src.DO_Amount))
        .ForMember(dest => dest.QtyDO, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.DO_Qty) ? 0 : int.Parse(src.DO_Qty)))
        .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.RowNo));
        CreateMap<GetKeyAccountDetailReportsInput, KeyAccountDetailReportFilterParams>();
        CreateMap<KeyAccountGeneralReport, KeyAccountGeneralReportDto>()
        .ForMember(dest => dest.DPOAmount, opt => opt.MapFrom(src => src.DPO_Amount))
        .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.RowNo));
        CreateMap<GetKeyAccountUpgradesInput, KeyAccountUpgradeFilterParams>();
        CreateMap<KeyAccountUpgrade, KeyAccountUpgradeDto>();
        //.ForMember(dest => dest.ClassSuggestion, opt => opt.MapFrom(src => src.Class_Suggestion))
        // .ForMember(dest => dest.KeyAccountClassCode, opt => opt.MapFrom(src => src.KeyAccount_Class_Code))
        //  .ForMember(dest => dest.KeyAccountClassName, opt => opt.MapFrom(src => src.KeyAccount_Class_Name))
        //   .ForMember(dest => dest.KeyAccountTypeName, opt => opt.MapFrom(src => src.KeyAccount_Type_Name))
        //    .ForMember(dest => dest.KeyAccountTypeCode, opt => opt.MapFrom(src => src.KeyAccount_Type_Code))
        //    .ForMember(dest => dest.SalePIC, opt => opt.MapFrom(src => src.MEVNSalePIC));

        CreateMap<GetKeyAccountGeneralReportsInput, KeyAccountGeneralReportFilterParams>();
        CreateMap<PriceOfferApprovalHistory, ApprovalHistoryDto>();
        CreateMap<PriceOfferDetailApprovalHistory, ApprovalHistoryDto>();
        CreateMap<PriceOffer, PriceOfferDto>();
        CreateMap<PriceOffer, PriceOfferListDto>();
        CreateMap<PriceOffer, PriceOfferWithNavigationListDto>()
            .IncludeBase<PriceOffer, PriceOfferListDto>();
        CreateMap<GetPriceOffersInput, PriceOfferFilterParams>();
        CreateMap<PriceOffer, PriceOfferExcelDto>();
        CreateMap<PriceOfferCustomer, PriceOfferCustomerDto>();
        CreateMap<PriceOfferCustomerCreateDto, PriceOfferCustomerCreateParams>();
        CreateMap<GetPriceOfferCustomersInput, PriceOfferCustomerFilterParams>();
        CreateMap<PriceOfferCustomerUpdateDto, PriceOfferCustomerUpdateParams>();

        CreateMap<PriceOfferCreateDto, PriceOfferCreateParams>();
        CreateMap<PriceOfferUpdateDto, PriceOfferUpdateParams>();

        CreateMap<PriceOfferDetail, PriceOfferDetailDto>();
        CreateMap<PriceOfferDetail, PriceOfferDetailExcelDto>();
        CreateMap<GetPriceOfferDetailsInput, PriceOfferDetailFilterParams>();
        CreateMap<GetPriceOfferReportDetailsInput, PriceOfferReportDetailFilterParams>();
        CreateMap<GetPriceOfferReportGeneralsInput, PriceOfferReportGeneralFilterParams>();
        CreateMap<PriceOfferReportGeneral, PriceOfferReportGeneralDto>();
        CreateMap<PriceOfferReportDetail, PriceOfferReportDetailDto>();

        CreateMap<GetCargosInput, CargoFilterParams>();
        CreateMap<CargoCreateDto, CargoCreateParams>();
        CreateMap<CargoUpdateDto, CargoUpdateParams>();
        CreateMap<GetCargoReportsInput, CargoReportFilterParams>();
        CreateMap<CargoReport, CargoReportDto>();
        CreateMap<Cargo, CargoDto>();
        CreateMap<CargoData, CargoDataDto>();
        CreateMap<GetCargoDataInput, CargoDataFilterParams>();
        CreateMap<Cargo, CargoImportDto>();

        CreateMap<PSIDetail, PSIDetailDto>();
        CreateMap<PSIDetailCreateDto, PSIDetailsCreateParams>();
        CreateMap<GetPSIDetailsInput, PSIDetailsFilterParams>();
        CreateMap<PSIDetailUpdateDto, PSIDetailsUpdateParams>();

        CreateMap<SupplierBUExcelDownloadDto, SupplierBUFilterParams>();

        CreateMap<Customer, CustomerDto>();
        CreateMap<Customer, CustomerExcelDto>();
        CreateMap<GetCustomersInput, CustomerFilterParams>();
        CreateMap<CustomerExcelDownloadDto, CustomerFilterParams>();
        CreateMap<CustomerCreateDto, CustomerCreateParams>();
        CreateMap<CustomerUpdateDto, CustomerUpdateParams>();

        CreateMap<MaterialHistory, MaterialHistoryDto>();

        CreateMap<MaterialStock, MaterialStockDto>();

        CreateMap<StockCategory, StockCategoryDto>();

        CreateMap<ApprovalHistory, ApprovalHistoryDto>();
        CreateMap<MaterialApprovalRequestHistory, MaterialApprovalRequestHistoryDto>();

        CreateMap<ApprovalRoute, ApprovalRouteDto>();
        CreateMap<MaterialApprovalRequestRoute, MaterialApprovalRequestRouteDto>();

        CreateMap<Buyer, BuyerListDto>();

        CreateMap<DPO, DPODto>();
        CreateMap<DPO, DPOExcelDto>();
        CreateMap<DPOListPOsModel, DPOListPOsDto>();
        CreateMap<DPOListPOsModel, GICListPOsDto>();
        CreateMap<DPOLockStockEtaEtdModel, DPOLockStockEtaEtdDto>();
        CreateMap<DPOExportDataModel, DPOExportDataDto>()
            .ForMember(dest => dest.MaterialGroup, opt => opt.MapFrom(src => src.Material_Group))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))

            .ForMember(dest => dest.Distributor, opt => opt.MapFrom(src => src.BuyerShortName))
            .ForMember(dest => dest.ProjectCode, opt => opt.MapFrom(src => src.SPOCode))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.SODate, opt => opt.MapFrom(src => src.SODate))
            .ForMember(dest => dest.LockshipmentQty, opt => opt.MapFrom(src => src.LockShipmentQty))
            .ForMember(dest => dest.LockshipmentQtyImported, opt => opt.MapFrom(src => src.LockShipmentImportedQty));
        CreateMap<DPOExcelDownloadDto, DPOFilterParams>();
        CreateMap<GetDPOsInput, DPOFilterParams>();

        CreateMap<DPODetail, DPODetailDto>()
            .ForMember(x => x.DpoNo, option => option.MapFrom(y => y.DPO.DPONo))
            .ForMember(x => x.DpoRemark, option => option.MapFrom(y => y.DPO.Remark))
            .ForMember(x => x.DpoOrderDate, option => option.MapFrom(y => y.DPO.OrderDate));
        CreateMap<DPODetail, DPODetailExcelDto>();
        CreateMap<DPOProcessingReport, DPOProcessingReportDto>();
        CreateMap<DPO, DpoGkrAllocationDto>();
        CreateMap<DPO, GicGkrAllocationDto>();
        CreateMap<Material, StockManagementDto>();
        CreateMap<MaterialStockUpload, StockManagementUploadDto>();
        CreateMap<MaterialStockUploadDetail, MaterialStockUploadDetailDto>();
        CreateMap<GetStockManagementsInput, MaterialFilterParams>();
        CreateMap<GetStockManagementApprovalsInput, MaterialStockUploadFilterParams>();
        CreateMap<SpecialInputPrice, SpecialInputPriceDto>();
        CreateMap<SpecialInputPriceCreateDto, SpecialInputPriceCreateParams>();
        CreateMap<SpecialInputPrice, SpecialInputPriceWithDetailDto>();
        CreateMap<SpecialInputPriceUpdateDto, SpecialInputPriceUpdateParams>();
        CreateMap<GetSpecialInputPricesInput, SpecialInputPriceFilterParams>();

        CreateMap<SpecialInputPriceDetail, SpecialInputPriceDetailDto>();
        CreateMap<SpecialInputPriceDetailCreateDto, SpecialInputPriceDetailCreateParams>();

        CreateMap<WorkflowApprover, WorkflowApproverDto>();
        CreateMap<GetWorkflowConfigurationsInput, WorkflowFilterParams>();

        CreateMap<WorkflowConfiguration, WorkflowConfigurationDto>();

        CreateMap<CustomerPIC, CustomerPICDto>()
        .ForMember(dest => dest.PICPhone, opt => opt.MapFrom(src => src.PIC_Phone))
        .ForMember(dest => dest.PICEmail, opt => opt.MapFrom(src => src.PIC_Email))
        .ForMember(dest => dest.PICJobTitle, opt => opt.MapFrom(src => src.PIC_JobTitle));

        CreateMap<Attachment, AttachmentDto>();
        CreateMap<KeyAccountAttachment, AttachmentDto>();

        CreateMap<DPO, GICDto>();
        CreateMap<DPODetail, GICDetailDto>();
        CreateMap<GetGICsInput, GICFilterParams>();
        CreateMap<PriceOfferAttachment, AttachmentDto>();

        CreateMap<Supplier, SupplierDto>();
        CreateMap<SupplierCreateDto, SupplierCreateParams>();
        CreateMap<GetSystemCategoriesInput, SupplierFilterParams>();
        CreateMap<GetSuppliersInput, SupplierFilterParams>();
        //CreateMap<SupplierExcelDownloadDto, SupplierFilterParams>();
        CreateMap<SupplierUpdateDto, SupplierUpdateParams>();

        CreateMap<Message, MessageDto>();
        CreateMap<PriceOfferMessage, MessageDto>();
        CreateMap<DPOMessage, MessageDto>();

        CreateMap<SystemConfiguration, SystemConfigurationDto>();

        CreateMap<SupplierBU, SupplierBUDto>();
        CreateMap<GetSupplierBUsInput, SupplierBUFilterParams>();
        CreateMap<SupplierBUCreateDto, SupplierBUCreateParams>();
        CreateMap<SupplierBUUpdateDto, SupplierBUUpdateParams>();
        CreateMap<SupplierBU, SupplierBUExcelDto>();

        CreateMap<MaterialGroupBuyer, MaterialGroupBuyerDto>();
        CreateMap<MaterialGroup, MaterialGroupDto>();

        CreateMap<MaterialGroupBuyerCreateDto, MaterialGroupBuyerCreateParams>();
        CreateMap<MaterialGroupBuyerUpdateDto, MaterialGroupBuyerUpdateParams>();
        CreateMap<GetMaterialGroupBuyersInput, MaterialGroupBuyerFilterParams>();

        CreateMap<GetMaterialStocksInput, MaterialStockFilterParams>();
        CreateMap<MaterialStock, MaterialStockDto>();

        CreateMap<StockImport, StockImportDto>();
        CreateMap<GetStockImportsInput, StockImportFilterParams>();
        CreateMap<StockImportCreateDto, StockImportCreateParams>();
        CreateMap<StockImportUpdateDto, StockImportUpdateParams>();

        CreateMap<StockImportDetail, StockImportDetailDto>();
        CreateMap<GetStockImportDetailsInput, StockImportDetailFilterParams>();
        CreateMap<StockImportDetailCreateDto, StockImportDetailCreateParams>();
        CreateMap<StockImportDetailUpdateDto, StockImportDetailUpdateParams>();
        CreateMap<StockImportDetail, StockImportDetailExcelDto>();
        CreateMap<StockImport, StockImportExportDto>();

        CreateMap<SaleOrder, SaleOrderDto>();
        CreateMap<SaleOrderCreateDto, SaleOrderCreateParams>();
        CreateMap<SaleOrderUpdateDto, SaleOrderUpdateParams>();
        CreateMap<GetSaleOrdersInput, SaleOrderFilterParams>();
        CreateMap<SaleOrderDetail, SaleOrderDetailDto>();
        CreateMap<SODetailExtrafeeUpdateInput, SODetailExtrafeeUpdateParams>();
        CreateMap<SaleOrderListExportSAPData, SAPDataDto>();

        CreateMap<PurchaseOrder, PurchaseOrderDto>();
        CreateMap<PurchaseOrderCreateDto, PurchaseOrderCreateParams>();
        CreateMap<PurchaseOrderUpdateDto, PurchaseOrderUpdateParams>();
        CreateMap<GetPurchaseOrdersInput, PurchaseOrderFilterParams>();
        CreateMap<PurchaseOrderDetail, PurchaseOrderDetailDto>();
        CreateMap<PSIApprovalHistory, ApprovalHistoryDto>();
        CreateMap<PSIApprovalHistory, PSIHistoryDto>();

        CreateMap<GetListDetailDPOsInput, PurchaseOrderGetListDetailDPOParams>();
        CreateMap<PurchaseOrderListDetailDPO, PurchaseOrderListDetailDPODto>();
        CreateMap<PurchaseOrderAddedDetailDPODto, PurchaseOrderAddedDetailDPOParams>();

        CreateMap<SaleOrderAddDetailsInput, MaterialStockLockStockInputAddedDetailSO>();
        CreateMap<MaterialStockLockStock, MaterialStockLockStockDto>();

        CreateMap<GetSaleOrderListDetailDPOsInput, SaleOrderGetListDetailDPOParams>();
        CreateMap<GetSaleOrderListDetailGICsInput, SaleOrderGetListDetailGICParams>();
        CreateMap<SaleOrderListDetailDPO, SaleOrderListDetailDPODto>();
        CreateMap<SaleOrderListDetailGIC, SaleOrderListDetailGICDto>();
        CreateMap<StockImportPriority, StockImportPriorityDto>();
        CreateMap<GetStockImportPrioritiesInput, StockImportPriorityFilterParams>();
        CreateMap<StockImportPriorityCreateDto, StockImportPriorityCreateParams>();
        CreateMap<StockImportPriorityUpdateDto, StockImportPriorityUpdateParams>()
;
        CreateMap<SaleOrderDetailUpdateDto, SaleOrderDetailUpdateParams>();
        CreateMap<PurchaseOrderDetailUpdateDto, PurchaseOrderDetailUpdateParams>();
        CreateMap<MaterialStockLockStock, MaterialStockLockStockDto>();
        CreateMap<SaleOrderAddedDetailDPODto, SaleOrderAddedDetailDPOParams>();
        CreateMap<PurchaseOrderLinkedDPO, PurchaseOrderLinkedDPODto>();

        CreateMap<StockImportAllocation, StockImportAllocationDto>();
        CreateMap<PurchaseOrderListQtyImported, PurchaseOrderListQtyImportedDto>();
        CreateMap<ExportStockImportAllocationInput, ExcelStockImportAllocationParams>();
        CreateMap<StockImportAllocationExport, StockImportAllocationExportDto>();

        CreateMap<GetStockImportListsInput, StockImportListParams>();
        CreateMap<GetStockImportConfirmsInput, StockImportConfirmParams>();
        CreateMap<StockImportList, StockImportListDto>();

        CreateMap<SaleOrdersSapImport, SaleOrdersSapImportDto>();
        CreateMap<GetSaleOrdersSapImportsInput, SaleOrderSapImportFilterParams>();
        CreateMap<SaleOrdersSapImportUpdateDto, SaleOrderSapImportUpdateParams>();
        CreateMap<SaleOrdersSapImportCreateDto, SaleOrderSapImportCreateParams>();

        CreateMap<PurchaseOrdersSapImport, PurchaseOrdersSapImportDto>();
        CreateMap<GetPurchaseOrdersSapImportsInput, PurchaseOrdersSapImportFilterParams>();
        CreateMap<PurchaseOrdersSapImportUpdateDto, PurchaseOrdersSapImportUpdateParams>();
        CreateMap<PurchaseOrdersSapImportCreateDto, PurchaseOrderSapImportCreateParams>();

        CreateMap<SaleOrderListModalDPO, SaleOrderListModalDPODto>();
        CreateMap<SaleOrderListModalDelivery, SaleOrderListModalDeliveryDto>();

        CreateMap<PurchaseOrderLockShipment, PurchaseOrderLockShipmentDto>();

        CreateMap<MaterialStockLockShipment, MaterialStockLockShipmentDto>();

        CreateMap<StockManagementList, StockManagementListDto>();
        CreateMap<GetStockManagementsListInput, StockManagementFilterParams>();
        CreateMap<PurchaseOrderListDetail, PurchaseOrderListDetailDto>();
        CreateMap<PurchaseOrderListDetailWarningStock, PurchaseOrderListDetailWarningStockDto>();
        CreateMap<PurchaseOrderListDetailFOC, PurchaseOrderListDetailFOCDto>();
        CreateMap<PurchaseOrderListSAPData, PurchaseOrderListSAPDataDto>();

        CreateMap<GetMaterialGroupsInput, MaterialGroupFilterParams>();
        CreateMap<MaterialGroupCreateDto, MaterialGroupCreateParams>();
        CreateMap<MaterialGroupUpdateDto, MaterialGroupUpdateParams>();

        CreateMap<StockCategory, StockCategoryDto>();
        CreateMap<StockCategoryCreateDto, StockCategoryCreateParams>();
        CreateMap<StockCategoryUpdateDto, StockCategoryUpdateParams>();
        CreateMap<GetStockCategoriesInput, StockCategoryFilterParams>();

        CreateMap<GetSaleOrdersInput, SaleOrderListExportSAPDataParams>();
        CreateMap<SaleOrderListExportSAPData, SaleOrderListExportSAPDataDto>();

        CreateMap<GetStockImportConfirmsInput, StockImportUpdateParams>();
        CreateMap<StockQty, StockQtyDto>();
        CreateMap<StockOfSO, StockOfSODto>();
        CreateMap<Locked, LockedDto>();
        CreateMap<LockShipment, LockShipmentDto>();
        CreateMap<OnOrderStock, OnOrderStockDto>();

        CreateMap<DPOReportDto, DPODataReportDto>();
        CreateMap<MaterialOverallStockReport, DataMaterialOverallStockReportDto>();

        CreateMap<DistributorTarget, DistributorTargetDto>();
        CreateMap<DistributorTarget, DistributorTargetExcelDto>();
        CreateMap<DistributorTargetCreateDto, DistributorTargetCreateParams>();
        CreateMap<GetDistributorTargetsInput, DistributorTargetFilterParams>();
        CreateMap<DistributorTargetExcelDownloadDto, DistributorTargetFilterParams>();
        CreateMap<DistributorTargetUpdateDto, DistributorTargetUpdateParams>();

        CreateMap<DashboardSaleResult, SaleResultBuyerDto>();
        CreateMap<DashboardSaleResult, SaleResultPODto>();
        CreateMap<DashboardSaleResult, SaleResultMaterialGroupDto>();
        CreateMap<DashboardSaleResult, SaleResultBaseDto>();
        CreateMap<DashboardApprovalItem, ApprovalDashboardItemDto>();

        CreateMap<GetInventoryReportsInput, ExcelInventoryReportFilterParams>();
        CreateMap<InventoryReport, InventoryReportDto>();

        CreateMap<GetPurchaseInvoicesInput, ExcelPurchaseInvoiceParams>();

        CreateMap<HistoryTracking, HistoryTrackingDto>();
        CreateMap<AssetHistoryTracking, HistoryTrackingDto>();
        CreateMap<AddMoreItemHistory, AddMoreItemHistoryDto>();

        CreateMap<GetGKRsInput, GKRFilterParams>();
        CreateMap<DPO, GKRDto>();
        CreateMap<DPODetail, GKRDetailDto>();
        CreateMap<DPOLockStockEtaEtdModel, GKRDetailLockShipmentEtaEtdDto>();
        CreateMap<DPODetail, GKRDetailDto>();

        CreateMap<SpoBatchRequest, SpoBatchRequestDto>();

        CreateMap<SpoBatchRequestDetail, SpoBatchRequestDetailDto>();
        CreateMap<GetSpoBatchRequestsInput, SpoBatchRequestFilterParams>();
        CreateMap<SpoBatchRequestCreateDto, SpoBatchRequestCreateParams>();
        CreateMap<SpoBatchRequestUpdateDto, SpoBatchRequestUpdateParams>();
        CreateMap<GetAllocationInvoicesInput, ExcelAllocationInvoiceParams>();

        CreateMap<SaleReportInput, SaleReportFillterParams>();
        CreateMap<SaleReportByCustomer, SaleReportByCustomerDto>();

        CreateMap<SaleReportByCustomerR05, SaleReportByCustomerR05Dto>();

        CreateMap<CfgDiscountRatio, CfgDiscountRatioDto>();
        CreateMap<GetCfgDiscountRatiosInput, CfgDiscountRatioFilterParams>();
        CreateMap<CfgDiscountRatioUpdateDto, CfgDiscountRatioUpdateParams>();

        #region Asset
        CreateMap<Asset, AssetDto>();
        CreateMap<AssetCreateDto, AssetCreateParams>();
        CreateMap<GetAssetsInput, AssetFilterParams>();
        CreateMap<SearchAssetInput, AssetFilterParams>();
        #endregion

        #region AssetRequest
        CreateMap<AssetRequest, AssetRequestDto>();
        CreateMap<AssetRequestCreateDto, AssetRequestCreateParams>();
        CreateMap<AssetRequestUpdateDto, AssetRequestUpdateParams>();
        CreateMap<AssetRequestUpdateExtendDto, AssetRequestUpdateParams>();
        CreateMap<AssetRequestApprovalHistory, AssetRequestApprovalHistoryDto>();
        CreateMap<AssetRequestApprovalRoute, AssetRequestApprovalRouteDto>();
        #endregion

        CreateMap<AssetRequestDetail, AssetRequestDetailDto>();
        CreateMap<GetAssetRequestsInput, AssetRequestFilterParams>().ForMember(dest => dest.RequestTypes, opt => opt.Ignore())
            .ForMember(dest => dest.Statuses, opt => opt.Ignore());
        CreateMap<AssetRequestDetailCreateDto, AssetRequestDetailCreateParams>();
        CreateMap<AssetRequestDetailUpdateParams, AssetRequestDetailCreateParams>();
    }
}
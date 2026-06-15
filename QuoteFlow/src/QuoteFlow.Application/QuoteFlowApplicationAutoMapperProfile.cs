using AutoMapper;
using QuoteFlow.AddMoreItemHistories;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalRoutes;

using QuoteFlow.Attachments;
using QuoteFlow.Buyers;
using QuoteFlow.Buyers.ParameterObjects;
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
using QuoteFlow.HistoryTrackings;
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
using QuoteFlow.SaleOrders;
using QuoteFlow.SaleOrders.ParameterObjects;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.SaleOrders.SaleOrderDetails.ParameterObjects;
using QuoteFlow.SaleOrdersSapImports;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.SalesAssignments;
using QuoteFlow.SalesAssignments.ParameterObjects;
using QuoteFlow.Shared.Excels;
using QuoteFlow.StockCategories;
using QuoteFlow.StockCategories.ParameterObjects;
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

       
        //.ForMember(dest => dest.ClassSuggestion, opt => opt.MapFrom(src => src.Class_Suggestion))
        // .ForMember(dest => dest.KeyAccountClassCode, opt => opt.MapFrom(src => src.KeyAccount_Class_Code))
        //  .ForMember(dest => dest.KeyAccountClassName, opt => opt.MapFrom(src => src.KeyAccount_Class_Name))
        //   .ForMember(dest => dest.KeyAccountTypeName, opt => opt.MapFrom(src => src.KeyAccount_Type_Name))
        //    .ForMember(dest => dest.KeyAccountTypeCode, opt => opt.MapFrom(src => src.KeyAccount_Type_Code))
        //    .ForMember(dest => dest.SalePIC, opt => opt.MapFrom(src => src.MEVNSalePIC));

       
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
    

        CreateMap<WorkflowApprover, WorkflowApproverDto>();
        CreateMap<GetWorkflowConfigurationsInput, WorkflowFilterParams>();

        CreateMap<WorkflowConfiguration, WorkflowConfigurationDto>();

        CreateMap<CustomerPIC, CustomerPICDto>()
        .ForMember(dest => dest.PICPhone, opt => opt.MapFrom(src => src.PIC_Phone))
        .ForMember(dest => dest.PICEmail, opt => opt.MapFrom(src => src.PIC_Email))
        .ForMember(dest => dest.PICJobTitle, opt => opt.MapFrom(src => src.PIC_JobTitle));

        CreateMap<Attachment, AttachmentDto>();
      
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

      
        CreateMap<SaleOrder, SaleOrderDto>();
        CreateMap<SaleOrderCreateDto, SaleOrderCreateParams>();
        CreateMap<SaleOrderUpdateDto, SaleOrderUpdateParams>();
        CreateMap<GetSaleOrdersInput, SaleOrderFilterParams>();
        CreateMap<SaleOrderDetail, SaleOrderDetailDto>();
        CreateMap<SODetailExtrafeeUpdateInput, SODetailExtrafeeUpdateParams>();
        CreateMap<SaleOrderListExportSAPData, SAPDataDto>();

      

        CreateMap<SaleOrderAddDetailsInput, MaterialStockLockStockInputAddedDetailSO>();
        CreateMap<MaterialStockLockStock, MaterialStockLockStockDto>();

        CreateMap<GetSaleOrderListDetailDPOsInput, SaleOrderGetListDetailDPOParams>();
        CreateMap<GetSaleOrderListDetailGICsInput, SaleOrderGetListDetailGICParams>();
        CreateMap<SaleOrderListDetailDPO, SaleOrderListDetailDPODto>();
        CreateMap<SaleOrderListDetailGIC, SaleOrderListDetailGICDto>();
      
;
        CreateMap<SaleOrderDetailUpdateDto, SaleOrderDetailUpdateParams>();

        CreateMap<MaterialStockLockStock, MaterialStockLockStockDto>();
        CreateMap<SaleOrderAddedDetailDPODto, SaleOrderAddedDetailDPOParams>();
       

      

        CreateMap<SaleOrdersSapImport, SaleOrdersSapImportDto>();
        CreateMap<GetSaleOrdersSapImportsInput, SaleOrderSapImportFilterParams>();
        CreateMap<SaleOrdersSapImportUpdateDto, SaleOrderSapImportUpdateParams>();
        CreateMap<SaleOrdersSapImportCreateDto, SaleOrderSapImportCreateParams>();

      

        CreateMap<SaleOrderListModalDPO, SaleOrderListModalDPODto>();
        CreateMap<SaleOrderListModalDelivery, SaleOrderListModalDeliveryDto>();

       

        CreateMap<MaterialStockLockShipment, MaterialStockLockShipmentDto>();

        CreateMap<StockManagementList, StockManagementListDto>();
        CreateMap<GetStockManagementsListInput, StockManagementFilterParams>();
    
        CreateMap<GetMaterialGroupsInput, MaterialGroupFilterParams>();
        CreateMap<MaterialGroupCreateDto, MaterialGroupCreateParams>();
        CreateMap<MaterialGroupUpdateDto, MaterialGroupUpdateParams>();

        CreateMap<StockCategory, StockCategoryDto>();
        CreateMap<StockCategoryCreateDto, StockCategoryCreateParams>();
        CreateMap<StockCategoryUpdateDto, StockCategoryUpdateParams>();
        CreateMap<GetStockCategoriesInput, StockCategoryFilterParams>();

        CreateMap<GetSaleOrdersInput, SaleOrderListExportSAPDataParams>();
        CreateMap<SaleOrderListExportSAPData, SaleOrderListExportSAPDataDto>();

     
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

      

        CreateMap<HistoryTracking, HistoryTrackingDto>();
        CreateMap<AddMoreItemHistory, AddMoreItemHistoryDto>();

       

        CreateMap<SaleReportInput, SaleReportFillterParams>();
        CreateMap<SaleReportByCustomer, SaleReportByCustomerDto>();

        CreateMap<SaleReportByCustomerR05, SaleReportByCustomerR05Dto>();

        CreateMap<CfgDiscountRatio, CfgDiscountRatioDto>();
        CreateMap<GetCfgDiscountRatiosInput, CfgDiscountRatioFilterParams>();
        CreateMap<CfgDiscountRatioUpdateDto, CfgDiscountRatioUpdateParams>();

 
    }
}
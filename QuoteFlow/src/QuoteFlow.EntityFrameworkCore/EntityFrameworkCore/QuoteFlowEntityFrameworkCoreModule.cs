using QuoteFlow.AssetRequestDetails;
using QuoteFlow.AssetRequests;
using QuoteFlow.Assets;
using Dapper;
using QuoteFlow.AddMoreItemHistories;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalRoutes;
using QuoteFlow.Attachments;
using QuoteFlow.Buyers;
using QuoteFlow.Cargos;
using QuoteFlow.Cargos.CargoDatas;
using QuoteFlow.CfgDiscountRatios;
using QuoteFlow.CustomerPICs;
using QuoteFlow.Customers;
using QuoteFlow.Dapper;
using QuoteFlow.DistributorTargets;
using QuoteFlow.DPOs;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.EntityFrameworkCore.Interceptors;
using QuoteFlow.HistoryTrackings;
using QuoteFlow.KeyAccountEvaluations;
using QuoteFlow.KeyAccounts;
using QuoteFlow.MaterialGroupBuyers;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.Materials.MaterialHistories;
using QuoteFlow.Materials.MaterialStocks;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.MaterialStockUploads;
using QuoteFlow.Messages;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PSIs;
using QuoteFlow.PSIs.PSIDetails;
using QuoteFlow.PurchaseOrderDetails;
using QuoteFlow.PurchaseOrderLockShipments;
using QuoteFlow.PurchaseOrders;
using QuoteFlow.PurchaseOrders.PurchaseOrderDetails;
using QuoteFlow.PurchaseOrdersSapImports;
using QuoteFlow.SaleOrders;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.SaleOrdersSapImports;
using QuoteFlow.SpecialInputPrices;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;
using QuoteFlow.SpoBatchRequests;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using QuoteFlow.StockCategories;
using QuoteFlow.StockImportAllocations;
using QuoteFlow.StockImportDetails;
using QuoteFlow.StockImportPriorities;
using QuoteFlow.StockImports;
using QuoteFlow.StockTracingDetails;
using QuoteFlow.StockTracings;
using QuoteFlow.SupplierBUs;
using QuoteFlow.SystemCategories;
using QuoteFlow.SystemConfigurations;
using QuoteFlow.WorkflowApprovers;
using QuoteFlow.WorkflowConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Gdpr;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Studio;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;

namespace QuoteFlow.EntityFrameworkCore;

[DependsOn(
    typeof(QuoteFlowDomainModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpIdentityProEntityFrameworkCoreModule),
    typeof(AbpOpenIddictProEntityFrameworkCoreModule),
    typeof(LanguageManagementEntityFrameworkCoreModule),
    typeof(FileManagementEntityFrameworkCoreModule),
    typeof(TextTemplateManagementEntityFrameworkCoreModule),
    typeof(AbpGdprEntityFrameworkCoreModule),
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule)
    )]
public class QuoteFlowEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {

        QuoteFlowEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<QuoteFlowDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
            options.AddRepository<Buyer, Buyers.EfCoreBuyerRepository>();

            options.AddRepository<KeyAccount, KeyAccounts.EfCoreKeyAccountRepository>();

            options.AddRepository<SystemCategory, SystemCategories.EfCoreSystemCategoryRepository>();

            options.AddRepository<StockTracing, StockTracings.EfCoreStockTracingRepository>();

            options.AddRepository<StockTracingDetail, StockTracingDetails.EfCoreStockTracingDetailRepository>();

            options.AddRepository<Material, Materials.EfCoreMaterialRepository>();

            options.AddRepository<PSI, EFCorePSIRepository>();

            options.AddRepository<KeyAccountEvaluation, KeyAccountEvaluations.EfCoreKeyAccountEvaluationRepository>();

            options.AddRepository<PriceOffer, PriceOffers.EfCorePriceOfferRepository>();

            options.AddRepository<PriceOfferCustomer, EfCorePriceOfferCustomerRepository>();

            options.AddRepository<PriceOfferDetail, EfCorePriceOfferDetailRepository>();

            options.AddRepository<PSIDetail, EfCorePSIDetailRepository>();

            options.AddRepository<Cargo, Cargos.EfCoreCargoRepository>();

            options.AddRepository<CargoData, EfCoreCargoDataRepository>();

            options.AddRepository<Customer, Customers.EfCoreCustomerRepository>();

            options.AddRepository<MaterialHistory, EfCoreMaterialHistoryRepository>();

            options.AddRepository<MaterialStock, EfCoreMaterialStockRepository>();

            options.AddRepository<StockCategory, StockCategories.EfCoreStockCategoryRepository>();

            options.AddRepository<ApprovalHistory, ApprovalHistories.EfCoreApprovalHistoryRepository>();

            options.AddRepository<AddMoreItemHistory, AddMoreItemHistories.EFCoreAddMoreItemHistoryRepository>();

            options.AddRepository<MaterialApprovalRequest, EfCoreMaterialApprovalRequestRepository>();

            options.AddRepository<MaterialApprovalRequestDetail, EfCoreMaterialApprovalRequestDetailRepository>();

            options.AddRepository<ApprovalRoute, ApprovalRoutes.EfCoreApprovalRouteRepository>();

            options.AddRepository<DPO, DPOs.EfCoreDPORepository>();

            options.AddRepository<DPODetail, EfCoreDPODetailRepository>();

            options.AddRepository<MaterialStockUploadDetail, MaterialStockUploadDetails.EfCoreMaterialStockUploadDetailRepository>();

            options.AddRepository<SpecialInputPrice, SpecialInputPrices.EfCoreSpecialInputPriceRepository>();

            options.AddRepository<SpecialInputPriceDetail, EfCoreSpecialInputPriceDetailRepository>();

            options.AddRepository<WorkflowApprover, WorkflowApprovers.EfCoreWorkflowApproverRepository>();

            options.AddRepository<WorkflowConfiguration, WorkflowConfigurations.EfCoreWorkflowConfigurationRepository>();

            options.AddRepository<CustomerPIC, CustomerPICs.EfCoreCustomerPICRepository>();

            options.AddRepository<Attachment, Attachments.EfCoreAttachmentRepository>();

            options.AddRepository<Message, Messages.EfCoreMessageRepository>();

            options.AddRepository<MaterialStockUpload, MaterialStockUploads.EfCoreMaterialStockUploadRepository>();

            options.AddRepository<SystemConfiguration, SystemConfigurations.EfCoreSystemConfigurationRepository>();

            options.AddRepository<SupplierBU, SupplierBUs.EfCoreSupplierBURepository>();

            options.AddRepository<MaterialGroupBuyer, MaterialGroupBuyers.EfCoreMaterialGroupBuyerRepository>();

            options.AddRepository<SaleOrder, SaleOrders.EfCoreSaleOrderRepository>();

            options.AddRepository<SaleOrderDetail, EfCoreSaleOrderDetailRepository>();

            options.AddRepository<PurchaseOrder, PurchaseOrders.EfCorePurchaseOrderRepository>();

            options.AddRepository<PurchaseOrderDetail, EfCorePurchaseOrderDetailRepository>();

            options.AddRepository<StockImport, StockImports.EfCoreStockImportRepository>();

            options.AddRepository<StockImportDetail, StockImportDetails.EfCoreStockImportDetailRepository>();

            options.AddRepository<StockImportPriority, StockImportPriorities.EfCoreStockImportPriorityRepository>();

            options.AddRepository<MaterialStockLockStock, EfCoreMaterialStockLockStockRepository>();

            options.AddRepository<StockImportAllocation, StockImportAllocations.EfCoreStockImportAllocationRepository>();

            options.AddRepository<SaleOrdersSapImport, SaleOrdersSapImports.EfCoreSaleOrdersSapImportRepository>();

            options.AddRepository<PurchaseOrdersSapImport, PurchaseOrdersSapImports.EfCorePurchaseOrdersSapImportRepository>();

            options.AddRepository<DistributorTarget, DistributorTargets.EfCoreDistributorTargetRepository>();

            options.AddRepository<PurchaseOrderLockShipment, PurchaseOrderLockShipments.EfCorePurchaseOrderLockShipmentRepository>();

            options.AddRepository<MaterialStockLockShipment, EfCoreMaterialStockLockShipmentRepository>();

            options.AddRepository<HistoryTracking, HistoryTrackings.EfCoreHistoryTrackingRepository>();

            options.AddRepository<SpoBatchRequest, SpoBatchRequests.EfCoreSpoBatchRequestRepository>();

            options.AddRepository<SpoBatchRequestDetail, EfCoreSpoBatchRequestDetailRepository>();

            options.AddRepository<CfgDiscountRatio, CfgDiscountRatios.EfCoreCfgDiscountRatioRepository>();

            options.AddRepository<Asset, Assets.EfCoreAssetRepository>();

            options.AddRepository<AssetRequest, AssetRequests.EfCoreAssetRequestRepository>();

            options.AddRepository<AssetRequestDetail, AssetRequestDetails.EfCoreAssetRequestDetailRepository>();

        });

        if (AbpStudioAnalyzeHelper.IsInAnalyzeMode)
        {
            return;
        }

        context.Services.AddScoped<AuditSaveChangesInterceptor>();
        context.Services.AddScoped<DataTruncationInterceptor>();
        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also QuoteFlowDbContextFactory for EF Core tooling. */

            options.UseSqlServer();

            options.Configure(c =>
            {
                c.UseSqlServer()
                    .AddInterceptors(c.ServiceProvider.GetRequiredService<AuditSaveChangesInterceptor>())
                    .AddInterceptors(c.ServiceProvider.GetRequiredService<DataTruncationInterceptor>())
                    .EnableSensitiveDataLogging();
            });
        });

        SqlMapper.AddTypeHandler(new ExtraPropertyDictionaryTypeHandler());
    }
}
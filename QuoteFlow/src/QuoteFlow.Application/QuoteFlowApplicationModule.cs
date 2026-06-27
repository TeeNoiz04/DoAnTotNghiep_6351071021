using QuoteFlow.Customers;
using QuoteFlow.Customers.Excels.Converters;
using QuoteFlow.Customers.Excels.Validators;
using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.DPOs;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using QuoteFlow.DPOs.Excels.ImportDPO.Converters;
using QuoteFlow.DPOs.Excels.ImportDPO.Validators;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.Materials;
using QuoteFlow.Materials.Excels.MaterialFactory;
using QuoteFlow.Materials.Excels.MaterialNewRegistrations;
using QuoteFlow.Materials.Excels.MaterialSAP;
using QuoteFlow.Materials.Excels.MaterialStatus;
using QuoteFlow.Materials.Excels.MaterialUpdateInventoryPlans;
using QuoteFlow.Materials.Excels.MaterialUpdatePrices;
using QuoteFlow.Materials.Excels.MaterialUpdateWithoutPrices;
using QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
using QuoteFlow.Materials.MaterialImport.MaterialFactory;
using QuoteFlow.Materials.MaterialImport.MaterialSAP;
using QuoteFlow.Materials.MaterialImport.MaterialStatus;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.MaterialStockUploadDetails.ParameterObjects;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.Excels.PriceOfferAddMoreItems.Converters;
using QuoteFlow.PriceOffers.Excels.PriceOfferAddMoreItems.Validators;
using QuoteFlow.PriceOffers.Excels.PriceOfferAPs.Converters;
using QuoteFlow.PriceOffers.Excels.PriceOfferAPs.Validators;
using QuoteFlow.PriceOffers.Excels.PriceOfferDSs.Converters;
using QuoteFlow.PriceOffers.Excels.PriceOfferDSs.Validators;
using QuoteFlow.PriceOffers.Excels.PriceOfferNBs.Converters;
using QuoteFlow.PriceOffers.Excels.PriceOfferNBs.Validators;
using QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Converters;
using QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Validators;
using QuoteFlow.PriceOffers.Excels.PriceOfferUpdateLandingCosts.Converters;
using QuoteFlow.PriceOffers.Excels.PriceOfferUpdateLandingCosts.Validators;
using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.RequesterContexts;
using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrders.Excel.Conventer;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Flagging;

using QuoteFlow.StockManagements.Excels.StockInventory.Converters;
using QuoteFlow.StockManagements.Excels.StockInventory.Validators;
using QuoteFlow.StockManagements.Excels.StockTransfer.Converters;
using QuoteFlow.StockManagements.Excels.StockTransfer.Validators;
using QuoteFlow.StockTracingDetails.ParameterObjects;
using QuoteFlow.StockTracings;
using QuoteFlow.StockTracings.Excels;
using QuoteFlow.SpoBatchRequests.Excel;
using QuoteFlow.SpoBatchRequests.Excel.Converters;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.ParameterObject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using Volo.Abp.Account;
using Volo.Abp.AuditLogging;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Gdpr;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Quartz;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.FileManagement;
using static QuoteFlow.ServiceKeys;

namespace QuoteFlow;

[DependsOn(
    typeof(QuoteFlowDomainModule),
    typeof(QuoteFlowApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountPublicApplicationModule),
    typeof(AbpAccountAdminApplicationModule),
    typeof(AbpAuditLoggingApplicationModule),
    typeof(TextTemplateManagementApplicationModule),
    typeof(AbpOpenIddictProApplicationModule),
    typeof(LanguageManagementApplicationModule),
    typeof(FileManagementApplicationModule),
    typeof(AbpGdprApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
[DependsOn(typeof(AbpQuartzModule))]
[DependsOn(typeof(AbpBackgroundWorkersQuartzModule))]
public class QuoteFlowApplicationModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        base.PreConfigureServices(context);

        var configuration = context.Services.GetConfiguration();

        PreConfigure<AbpQuartzOptions>(options =>
        {
            options.Configurator = configure =>
            {
                configure.UsePersistentStore(storeOptions =>
                {
                    storeOptions.UseProperties = true;
                    storeOptions.UseClustering();
                    storeOptions.RetryInterval = TimeSpan.FromSeconds(15);
                    storeOptions.UseNewtonsoftJsonSerializer();
                    storeOptions.UseSqlServer(configuration.GetConnectionString("Default")!);
                });
            };
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<QuoteFlowApplicationModule>();
        });

        context.Services.AddScoped<IRequesterContext, RequesterContext>();
        context.Services.AddScoped<IEffectiveUserContext, EffectiveUserContext>();

        // Register flagging services
        context.Services.AddScoped<IFlaggingService<DPO, DPOFlagsDto>, DPOFlaggingService>();

        AddExcelValidation(context);
    }

    private static void AddExcelValidation(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var priceOfferPP = ExcelValidatorKeys.PriceOffers.PP;
        var priceOfferAP = ExcelValidatorKeys.PriceOffers.AP;
        var priceOfferDS = ExcelValidatorKeys.PriceOffers.DS;
        var priceOfferNB = ExcelValidatorKeys.PriceOffers.NB;
        var priceOfferAddMoreItem = ExcelValidatorKeys.PriceOffers.DT;
        var priceOfferUpdateLandingCost = ExcelValidatorKeys.PriceOffers.ULC;
        var materialUpdatePrice = ExcelValidatorKeys.Materials.UP;
        var materialNewRegistration = ExcelValidatorKeys.Materials.NR;
        var materialUpdateWithoutPrice = ExcelValidatorKeys.Materials.WUP;
        var materialUpdateInventoryPlan = ExcelValidatorKeys.Materials.UIP;

        var stockTracingDelivery = ExcelValidatorKeys.StocTracings.STD;
        var stockTracingInventory = ExcelValidatorKeys.StocTracings.STI;
        var stockTracingReceipt = ExcelValidatorKeys.StocTracings.STR;

        var materialSAP = ExcelValidatorKeys.Materials.SAP;
        var materialFactory = ExcelValidatorKeys.Materials.FAC;
        var materialStatus = ExcelValidatorKeys.Materials.STA;

        var stockManagementTransfer = ExcelValidatorKeys.StockManagement.TR;
        var stockManagementInventory = ExcelValidatorKeys.StockManagement.IN;

        var gicWarranty = ExcelValidatorKeys.GIC.WR;
        var gicSponsor = ExcelValidatorKeys.GIC.SP;
        var gicInternal = ExcelValidatorKeys.GIC.IN;
        var gicWriteOff = ExcelValidatorKeys.GIC.WO;

        var pSI_FA = ExcelValidatorKeys.PSI.FA;
        var pSI_LVS = ExcelValidatorKeys.PSI.LVS;

        var cargo = ExcelValidatorKeys.Cargos.CA;
        var asset = ExcelValidatorKeys.Assets.AS;
        var assetAudit = ExcelValidatorKeys.Assets.ASA;
        var assetUpdate = ExcelValidatorKeys.Assets.ASU;

        var supplierBU = ExcelValidatorKeys.SupplierBU.SBU;
        var customer = ExcelValidatorKeys.Customer.CU;
        var supplierBUUpdate = ExcelValidatorKeys.SupplierBU.SBUU;
        var customerUpdate = ExcelValidatorKeys.Customer.CUU;

        var dpo = ExcelValidatorKeys.DPOs.DPO;

        var gkr = ExcelValidatorKeys.GKRs.GKR;

        var stockImport = ExcelValidatorKeys.StokImport.SI;
        var special = ExcelValidatorKeys.Special.SIP;
        var stockImportPriority = ExcelValidatorKeys.StokImportPriority.SIP;

        var so = ExcelValidatorKeys.SaleOrders.SO;

        var so_WR = ExcelValidatorKeys.SaleOrders.WR;
        var so_IU = ExcelValidatorKeys.SaleOrders.IU;
        var so_IUC = ExcelValidatorKeys.SaleOrders.IUC;
        var so_FOC = ExcelValidatorKeys.SaleOrders.FOC;
        var so_WO = ExcelValidatorKeys.SaleOrders.WO;
        var po = ExcelValidatorKeys.PurchaseOrders.PO;

        var br = ExcelValidatorKeys.PriceOffers.BR;

        // Row Validators
        services.AddKeyedScoped<IExcelRowValidator<PriceOfferDetailImportDto>, PriceOfferPPDetailRowValidator>(priceOfferPP);
        services.AddKeyedScoped<IExcelRowValidator<PriceOfferDetailImportDto>, PriceOfferAPDetailRowValidator>(priceOfferAP);
        services.AddKeyedScoped<IExcelRowValidator<PriceOfferDetailImportDto>, PriceOfferDSDetailRowValidator>(priceOfferDS);
        services.AddKeyedScoped<IExcelRowValidator<PriceOfferDetailImportDto>, PriceOfferNBDetailRowValidator>(priceOfferNB);
        services.AddKeyedScoped<IExcelRowValidator<PriceOfferCustomerImportDto>, PriceOfferPPCustomerRowValidator>(priceOfferPP);
        services.AddKeyedScoped<IExcelRowValidator<PriceOfferDetailImportDto>, PriceOfferAddMoreItemRowValidator>(priceOfferAddMoreItem);
        services.AddKeyedScoped<IExcelRowValidator<PriceOfferUpdateLandingCostImportDto>, PriceOfferUpdateLandingCostRowValidator>(priceOfferUpdateLandingCost);
        services.AddKeyedScoped<IExcelRowValidator<MaterialUpdatePriceImportDto>, MaterialUpdatePriceRowValidator>(materialUpdatePrice);
        services.AddKeyedScoped<IExcelRowValidator<MaterialNewRegistrationImportDto>, MaterialNewRegistrationRowValidator>(materialNewRegistration);
        services.AddKeyedScoped<IExcelRowValidator<MaterialUpdateWithoutPriceImportDto>, MaterialUpdateWithoutPriceRowValidator>(materialUpdateWithoutPrice);
        services.AddKeyedScoped<IExcelRowValidator<MaterialUpdateInventoryPlanImportDto>, MaterialUpdateInventoryPlanRowValidator>(materialUpdateInventoryPlan);

        services.AddKeyedScoped<IExcelRowValidator<MaterialStockUploadDetailImportTransferDto>, StockTransferRowValidation>(stockManagementTransfer);
        services.AddKeyedScoped<IExcelRowValidator<MaterialStockUploadDetailImportInventoryDto>, StockInventoryRowValidation>(stockManagementInventory);

        services.AddKeyedScoped<IExcelRowValidator<StockTracingDeliveryImportDto>, StockTracingDeliveryRowValidator>(stockTracingDelivery);
        services.AddKeyedScoped<IExcelRowValidator<StockTracingInventoryImportDto>, StockTracingInventoryRowValidator>(stockTracingInventory);
        services.AddKeyedScoped<IExcelRowValidator<StockTracingReceiptImportDto>, StockTracingReceiptRowValidator>(stockTracingReceipt);

        services.AddKeyedScoped<IExcelRowValidator<CustomerImportDto>, ImportCustomerRowValidator>(customer);

        services.AddKeyedScoped<IExcelRowValidator<MaterialSAPUpdateExcelDto>, MaterialSAPRowValidatior>(materialSAP);
        services.AddKeyedScoped<IExcelRowValidator<MaterialFactoryUpdateExcelDto>, MaterialFactoryRowValidatior>(materialFactory);
        services.AddKeyedScoped<IExcelRowValidator<MaterialStatusUpdateExcelDto>, MaterialStatusRowValidator>(materialStatus);

       
        services.AddKeyedScoped<IExcelRowValidator<ImportDPODetailDto>, ImportDPODetailRowValidator>(dpo);

        services.AddKeyedScoped<IExcelRowValidator<SaleOrderExcelDto>, SaleOrderExcelRowValidation>(so);
        services.AddKeyedScoped<IExcelRowValidator<SpoBatchRequestDetailImportDto>, SpoBatchRequestDetailRowValidator>(br);
     
      
        // Sub-list validators
        services.AddKeyedScoped<IExcelValidator<PriceOfferDetailImportDto>>(priceOfferPP, (provider, _) =>
        {
            var config = new PriceOfferPPDetailValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<PriceOfferDetailImportDto>>(priceOfferPP);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<PriceOfferDetailImportDto>>>();
            return new BaseExcelValidator<PriceOfferDetailImportDto>(config, rowValidator, logger);
        });

        services.AddKeyedScoped<IExcelValidator<PriceOfferDetailImportDto>>(priceOfferAP, (provider, _) =>
        {
            var config = new PriceOfferAPDetailValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<PriceOfferDetailImportDto>>(priceOfferAP);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<PriceOfferDetailImportDto>>>();
            return new BaseExcelValidator<PriceOfferDetailImportDto>(config, rowValidator, logger);
        });

        services.AddKeyedScoped<IExcelValidator<PriceOfferDetailImportDto>>(priceOfferDS, (provider, _) =>
        {
            var config = new PriceOfferDSDetailValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<PriceOfferDetailImportDto>>(priceOfferDS);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<PriceOfferDetailImportDto>>>();
            return new BaseExcelValidator<PriceOfferDetailImportDto>(config, rowValidator, logger);
        });

        services.AddKeyedScoped<IExcelValidator<PriceOfferDetailImportDto>>(priceOfferNB, (provider, _) =>
        {
            var config = new PriceOfferNBDetailValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<PriceOfferDetailImportDto>>(priceOfferNB);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<PriceOfferDetailImportDto>>>();
            return new BaseExcelValidator<PriceOfferDetailImportDto>(config, rowValidator, logger);
        });

      

        services.AddKeyedScoped<IExcelValidator<PriceOfferDetailImportDto>>(priceOfferAddMoreItem, (provider, _) =>
        {
            var config = new PriceOfferAddMoreItemValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<PriceOfferDetailImportDto>>(priceOfferAddMoreItem);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<PriceOfferDetailImportDto>>>();
            return new PriceOfferAddMoreItemValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<PriceOfferUpdateLandingCostImportDto>>(priceOfferUpdateLandingCost, (provider, _) =>
        {
            var config = new PriceOfferUpdateLandingCostValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<PriceOfferUpdateLandingCostImportDto>>(priceOfferUpdateLandingCost);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<PriceOfferUpdateLandingCostImportDto>>>();
            return new PriceOfferUpdateLandingCostValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<PriceOfferCustomerImportDto>>(priceOfferPP, (provider, _) =>
        {
            var config = new PriceOfferPPCustomerValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<PriceOfferCustomerImportDto>>(priceOfferPP);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<PriceOfferCustomerImportDto>>>();
            var customerRepository = provider.GetRequiredService<ICustomerRepository>();
            return new PriceOfferPPCustomerValidator(config, rowValidator, logger, customerRepository);
        });

        //services.AddKeyedScoped<IExcelValidator<MaterialNewRegistrationImportDto>>(materialNewRegistration, (provider, _) =>
        //{
        //    var config = new MaterialNewRegistrationValidationConfig();
        //    var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialNewRegistrationImportDto>>(materialNewRegistration);
        //    var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialNewRegistrationImportDto>>>();
        //    return new BaseExcelValidator<MaterialNewRegistrationImportDto>(config, rowValidator, logger);
        //});

        services.AddKeyedScoped<IExcelValidator<MaterialNewRegistrationImportDto>>(materialNewRegistration, (provider, _) =>
        {
            var config = new MaterialNewRegistrationValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialNewRegistrationImportDto>>(materialNewRegistration);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialNewRegistrationImportDto>>>();
            return new MaterialNewRegistrationValidator(config, rowValidator, logger, provider);
        });
        services.AddKeyedScoped<IExcelValidator<MaterialUpdatePriceImportDto>>(materialUpdatePrice, (provider, _) =>
        {
            var config = new MaterialUpdatePriceValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialUpdatePriceImportDto>>(materialUpdatePrice);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialUpdatePriceImportDto>>>();
            return new MaterialUpdatePriceValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<MaterialUpdateWithoutPriceImportDto>>(materialUpdateWithoutPrice, (provider, _) =>
        {
            var config = new MaterialUpdateWithoutPriceValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialUpdateWithoutPriceImportDto>>(materialUpdateWithoutPrice);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialUpdateWithoutPriceImportDto>>>();
            return new MaterialUpdateWhitoutPriceValidatior(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<MaterialUpdateInventoryPlanImportDto>>(materialUpdateInventoryPlan, (provider, _) =>
        {
            var config = new MaterialUpdateInventoryPlanValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialUpdateInventoryPlanImportDto>>(materialUpdateInventoryPlan);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialUpdateInventoryPlanImportDto>>>();
            return new BaseExcelValidator<MaterialUpdateInventoryPlanImportDto>(config, rowValidator, logger);
        });



        //services.AddKeyedScoped<IExcelValidator<StockTracingDeliveryImportDto>>(stockTracingDelivery, (provider, _) =>
        //{
        //    var config = new StockTracingDeliveryConfig();
        //    var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<StockTracingDeliveryImportDto>>(stockTracingDelivery);
        //    var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<StockTracingDeliveryImportDto>>>();
        //    return new BaseExcelValidator<StockTracingDeliveryImportDto>(config, rowValidator, logger);
        //});

        services.AddKeyedScoped<IExcelValidator<StockTracingInventoryImportDto>>(stockTracingInventory, (provider, _) =>
        {
            var config = new StockTracingInventoryConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<StockTracingInventoryImportDto>>(stockTracingInventory);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<StockTracingInventoryImportDto>>>();
            return new BaseExcelValidator<StockTracingInventoryImportDto>(config, rowValidator, logger);
        });

        //services.AddKeyedScoped<IExcelValidator<StockTracingReceiptImportDto>>(stockTracingReceipt, (provider, _) =>
        //{
        //    var config = new StockTracingReceiptConfig();
        //    var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<StockTracingReceiptImportDto>>(stockTracingReceipt);
        //    var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<StockTracingReceiptImportDto>>>();
        //    return new BaseExcelValidator<StockTracingReceiptImportDto>(config, rowValidator, logger);
        //});
      

        services.AddKeyedScoped<IExcelValidator<StockTracingDeliveryImportDto>>(stockTracingDelivery, (provider, _) =>
        {
            var config = new StockTracingDeliveryConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<StockTracingDeliveryImportDto>>(stockTracingDelivery);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<StockTracingDeliveryImportDto>>>();
            return new StockTracingDeliveryValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<StockTracingReceiptImportDto>>(stockTracingReceipt, (provider, _) =>
        {
            var config = new StockTracingReceiptConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<StockTracingReceiptImportDto>>(stockTracingReceipt);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<StockTracingReceiptImportDto>>>();
            return new StockTracingReceiptValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<StockTracingInventoryImportDto>>(stockTracingInventory, (provider, _) =>
        {
            var config = new StockTracingInventoryConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<StockTracingInventoryImportDto>>(stockTracingInventory);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<StockTracingInventoryImportDto>>>();
            return new StockTracingInventoryValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<MaterialSAPUpdateExcelDto>>(materialSAP, (provider, _) =>
        {
            var config = new MaterialSAPConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialSAPUpdateExcelDto>>(materialSAP);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialSAPUpdateExcelDto>>>();
            return new MaterialSAPValidator(config, rowValidator, logger, provider);
        });
        services.AddKeyedScoped<IExcelValidator<MaterialUpdateInventoryPlanImportDto>>(materialUpdateInventoryPlan, (provider, _) =>
        {
            var config = new MaterialUpdateInventoryPlanValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialUpdateInventoryPlanImportDto>>(materialUpdateInventoryPlan);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialUpdateInventoryPlanImportDto>>>();
            return new MaterialUpdateInventoryPlanValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<MaterialFactoryUpdateExcelDto>>(materialFactory, (provider, _) =>
        {
            var config = new MaterialFactoryConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialFactoryUpdateExcelDto>>(materialFactory);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialFactoryUpdateExcelDto>>>();
            return new MaterialFactoryValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<MaterialStatusUpdateExcelDto>>(materialStatus, (provider, _) =>
        {
            var config = new MaterialStatusConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialStatusUpdateExcelDto>>(materialStatus);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialStatusUpdateExcelDto>>>();
            return new MaterialStatusValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<ImportDPODetailDto>>(dpo, (provider, _) =>
        {
            var config = new ImportDPODetailValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<ImportDPODetailDto>>(dpo);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<ImportDPODetailDto>>>();
            var priceOfferRepository = provider.GetRequiredService<IPriceOfferRepository>();
            return new ImportDPODetailValidator(config, rowValidator, priceOfferRepository, logger, provider);
        });

     

        // Full File Validators
        services.AddKeyedScoped<IExcelValidator<PriceOfferImportDto>>(priceOfferPP, (provider, _) =>
        {
            var offerCustomerValidator = provider.GetRequiredKeyedService<IExcelValidator<PriceOfferCustomerImportDto>>(priceOfferPP);
            var offerDetailValidator = provider.GetRequiredKeyedService<IExcelValidator<PriceOfferDetailImportDto>>(priceOfferPP);
            return new PriceOfferPPValidator(
                offerDetailValidator,
                offerCustomerValidator,
                provider
            );
        });

        services.AddKeyedScoped<IExcelValidator<PriceOfferImportDto>>(priceOfferAP, (provider, _) =>
        {
            var offerDetailValidator = provider.GetRequiredKeyedService<IExcelValidator<PriceOfferDetailImportDto>>(priceOfferAP);
            return new PriceOfferAPValidator(
                offerDetailValidator,
                provider

            );
        });

        services.AddKeyedScoped<IExcelValidator<PriceOfferImportDto>>(priceOfferDS, (provider, _) =>
        {
            var offerDetailValidator = provider.GetRequiredKeyedService<IExcelValidator<PriceOfferDetailImportDto>>(priceOfferDS);
            return new PriceOfferDSValidator(
                offerDetailValidator,
                provider

            );
        });

        services.AddKeyedScoped<IExcelValidator<PriceOfferImportDto>>(priceOfferNB, (provider, _) =>
        {
            var offerDetailValidator = provider.GetRequiredKeyedService<IExcelValidator<PriceOfferDetailImportDto>>(priceOfferNB);
            return new PriceOfferNBValidator(
                offerDetailValidator,
                provider
            );
        });

      

        services.AddKeyedScoped<IExcelValidator<ImportDPODto>>(dpo, (provider, _) =>
        {
            var detailValidator = provider.GetRequiredKeyedService<IExcelValidator<ImportDPODetailDto>>(dpo);
            return new ImportDPOValidator(
                detailValidator,
                provider
            );
        });
       
        services.AddKeyedScoped<IExcelValidator<MaterialStockUploadDetailImportTransferDto>>(stockManagementTransfer, (provider, _) =>
        {
            var config = new StockTransferValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialStockUploadDetailImportTransferDto>>(stockManagementTransfer);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialStockUploadDetailImportTransferDto>>>();
            return new StockTransferValidator(config, rowValidator, logger, provider);
        });

        services.AddKeyedScoped<IExcelValidator<MaterialStockUploadDetailImportInventoryDto>>(stockManagementInventory, (provider, _) =>
        {
            var config = new StockInventoryValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<MaterialStockUploadDetailImportInventoryDto>>(stockManagementInventory);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<MaterialStockUploadDetailImportInventoryDto>>>();
            return new StockInventoryValidator(config, rowValidator, logger, provider);
        });
       
        services.AddKeyedScoped<IExcelValidator<CustomerImportDto>>(customer, (provider, _) =>
        {
            var config = new ImportCustomerValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<CustomerImportDto>>(customer);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<CustomerImportDto>>>();
            return new ImportCustomerValidator(config, rowValidator, logger, provider);
        });
        services.AddKeyedScoped<IExcelValidator<SaleOrderExcelDto>>(so, (provider, _) =>
        {
            var config = new SaleOrderExcelConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<SaleOrderExcelDto>>(so);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<SaleOrderExcelDto>>>();
            return new SaleOrderExcelValidator(config, rowValidator, logger, provider);
        });
        services.AddKeyedScoped<IExcelValidator<SpoBatchRequestDetailImportDto>>(br, (provider, _) =>
        {
            var config = new SpoBatchRequestDetailValidationConfig();
            var rowValidator = provider.GetRequiredKeyedService<IExcelRowValidator<SpoBatchRequestDetailImportDto>>(br);
            var logger = provider.GetRequiredService<ILogger<BaseExcelValidator<SpoBatchRequestDetailImportDto>>>();
            return new SpoBatchRequestDetailValidator(config, rowValidator, logger, provider);
        });


        //----------------------------------------------------------------

        // Excel DTO Converters
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferCustomerImportDto, PriceOfferCustomerCreateParams>, PriceOfferPPCustomerExcelDtoConverter>(priceOfferPP);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>, PriceOfferPPDetailExcelDtoConverter>(priceOfferPP);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>, PriceOfferPPExcelDtoConverter>(priceOfferPP);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>, PriceOfferAPDetailExcelDtoConverter>(priceOfferAP);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>, PriceOfferAPExcelDtoConverter>(priceOfferAP);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>, PriceOfferDSDetailExcelDtoConverter>(priceOfferDS);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>, PriceOfferDSExcelDtoConverter>(priceOfferDS);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>, PriceOfferNBDetailExcelDtoConverter>(priceOfferNB);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>, PriceOfferNBExcelDtoConverter>(priceOfferNB);

      
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>, PriceOfferAddMoreItemExcelDtoConverter>(priceOfferAddMoreItem);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>, PriceOfferAddMoreItemExcelDtoConverter>(priceOfferAddMoreItem);
        services.AddKeyedScoped<IExcelDtoConverter<PriceOfferUpdateLandingCostImportDto, PriceOfferDetailUpdateLandingCostParams>, PriceOfferUpdateLandingCostExcelDtoConverter>(priceOfferUpdateLandingCost);
        services.AddKeyedScoped<IExcelDtoConverter<MaterialNewRegistrationImportDto, MaterialApprovalRequestDetailCreateParams>, MaterialNewRegistrationExcelDtoConverter>(materialNewRegistration);
        services.AddKeyedScoped<IExcelDtoConverter<MaterialUpdateWithoutPriceImportDto, ExcelMaterialUpdateWithoutPrriceParams>, MaterialUpdateWhitoutPriceExcelDtoConverter>(materialUpdateWithoutPrice);
        services.AddKeyedScoped<IExcelDtoConverter<MaterialUpdatePriceImportDto, ExcelMaterialUpdatePriceParams>, MaterialUpdatePriceExcelDtoConverter>(materialUpdatePrice);
        services.AddKeyedScoped<IExcelDtoConverter<MaterialUpdateInventoryPlanImportDto, ExcelMaterialUpdateInventoryPlanUpdateParams>, MaterialUpdateInventoryPlanConverter>(materialUpdateInventoryPlan);
        services.AddKeyedScoped<IExcelDtoConverter<MaterialSAPUpdateExcelDto, ExcelMaterialUpdateParams>, MaterialSAPExcelDtoConverter>(materialSAP);
        services.AddKeyedScoped<IExcelDtoConverter<MaterialFactoryUpdateExcelDto, ExcelMaterialFactoryUpdateParams>, MaterialFactoryExcelDtoConverter>(materialFactory);
        services.AddKeyedScoped<IExcelDtoConverter<MaterialStatusUpdateExcelDto, ExcelMaterialStatusUpdateParams>, MaterialStatusExcelDtoConverter>(materialStatus);
        services.AddKeyedScoped<IExcelDtoConverter<MaterialStockUploadDetailImportTransferDto, MaterialStockUploadDetailCreateParams>, StockTransferExcelDtoConverter>(stockManagementTransfer);
        services.AddKeyedScoped<IExcelDtoConverter<MaterialStockUploadDetailImportInventoryDto, MaterialStockUploadDetailCreateParams>, StockInventoryExcelDtoConverter>(stockManagementInventory);
       
        services.AddKeyedScoped<IExcelDtoConverter<CustomerImportDto, CustomerCreateParams>, CustomerImportConverter>(customer);
        services.AddKeyedScoped<IExcelDtoConverter<CustomerImportDto, CustomerUpdateParams>, CustomerImportUpdateConverter>(customerUpdate);

        services.AddKeyedScoped<IExcelDtoConverter<StockTracingDeliveryImportDto, StockTracingDetailCreateParams>, StockTracingDeliveryExcelDtoConverter>(stockTracingDelivery);
        services.AddKeyedScoped<IExcelDtoConverter<StockTracingInventoryImportDto, StockTracingDetailCreateParams>, StockTracingInventoryExcelDtoConverter>(stockTracingInventory);
        services.AddKeyedScoped<IExcelDtoConverter<StockTracingReceiptImportDto, StockTracingDetailCreateParams>, StockTracingReceiptExcelDtoConverter>(stockTracingReceipt);
      
        // DPO Excel Converters
        services.AddKeyedScoped<IExcelDtoConverter<ImportDPODto, DPOCreateParams>, ImportDPOExcelDtoConverter>(dpo);
        services.AddKeyedScoped<IExcelDtoConverter<ImportDPODetailDto, DPODetailCreateParams>, ImportDPODetailExcelDtoConverter>(dpo);

        // GKR Excel Converters
      
        services.AddKeyedScoped<IExcelDtoConverter<SaleOrderExcelDto, SaleOrderSapImportCreateParams>, SaleOrderExcelConventer>(so);
        // SPO Batch Request Excel Converter
        services.AddKeyedScoped<IExcelDtoConverter<SpoBatchRequestDetailImportDto, SpoBatchRequestDetailCreateParams>, SpoBatchRequestDetailExcelDtoConverter>(br);
         // Factories
        services.AddScoped<IExcelImportFactory, ExcelImportFactory>();
    }
}

public static class ServiceKeys
{
    public static class ExcelValidatorKeys
    {
        public static class PriceOffers
        {
            public const string PP = "PriceOffer_PP";
            public const string DS = "PriceOffer_DS";
            public const string AP = "PriceOffer_AP";
            public const string NB = "PriceOffer_NB";
            public const string DT = "PriceOfferAddMoreItemDetail";
            public const string ULC = "PriceOfferUpdateLandingCost";
            public const string BR = "BarchRequest";
        }

        public static class Materials
        {
            public const string UP = "MaterialUpdatePrice";
            public const string NR = "MaterialNewRegistration";
            public const string WUP = "MaterialUpdateWithoutPrice";
            public const string UIP = "MaterialUpdateInventoryPlan";

            public const string SAP = "MaterialSAP";
            public const string FAC = "MaterialFactory";
            public const string STA = "MaterialStatus";
        }

        public static class StocTracings
        {
            public const string STD = "StockTracingDelivery";
            public const string STI = "StockTracingInventory";
            public const string STR = "StockTracingReceipt";
        }
        public static class Cargos
        {
            public const string CA = "Cargo";
        }

        public static class Assets
        {
            public const string AS = "Asset";
            public const string ASU = "AssetUpdate";
            public const string ASA = "AssetAudit";
        }
        public static class SupplierBU
        {
            public const string SBU = "SupplierBU";
            public const string SBUU = "SupplierBUUpdate";
        }

        public static class Customer
        {
            public const string CU = "Customer";
            public const string CUU = "CustomerUpdate";
        }

        public static class StockManagement
        {
            public const string TR = "MaterialStockUploadDetailImportTransfer";
            public const string IN = "MaterialStockUploadDetailImportInventory";
        }

        public static class GIC
        {
            public const string WR = "GICWarranty";
            public const string SP = "GICSponsor";
            public const string IN = "GICInternal";
            public const string WO = "GICWriteOff";
        }

        public static class DPOs
        {
            public const string DPO = "DPO";
        }

        public static class GKRs
        {
            public const string GKR = "GKR";
        }

        public static class PSI
        {
            public const string FA = "PSI_FA";
            public const string LVS = "PSI_LVS";
        }

        public static class StokImport
        {
            public const string SI = "StockImport";
        }

        public static class Special
        {
            public const string SIP = "SpecialInputPrice";
        }

        public static class StokImportPriority
        {
            public const string SIP = "StockImportPriority";
        }

        public static class SaleOrders
        {
            public const string SO = "SaleOrders";
            public const string WR = "SaleOrderGICWarranty";
            public const string IU = "SaleOrderGICInternalUse";
            public const string IUC = "SaleOrderGICInternalUseChange";
            public const string WO = "SaleOrderGICWriteOff";
            public const string FOC = "SaleOrderGICFOC";
        }
        public static class PurchaseOrders
        {
            public const string PO = "PurchaseOrders";
        }

    }
}
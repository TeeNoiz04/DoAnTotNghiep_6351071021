using QuoteFlow.AddMoreItemHistories;
using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalRoutes;
using QuoteFlow.AssetRequestDetails;
using QuoteFlow.AssetRequests;
using QuoteFlow.Assets;
using QuoteFlow.Attachments;
using QuoteFlow.Buyers;
using QuoteFlow.CargoDatas;
using QuoteFlow.Cargos;
using QuoteFlow.Cargos.CargoDatas;
using QuoteFlow.CfgDiscountRatios;
using QuoteFlow.CustomerPICs;
using QuoteFlow.Customers;
using QuoteFlow.DistributorTargets;
using QuoteFlow.DPOs;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.DpoGkrUsages;
using QuoteFlow.Extensions;
using QuoteFlow.HistoryTrackings;
using QuoteFlow.KeyAccountEvaluations;
using QuoteFlow.KeyAccounts;
using QuoteFlow.MaterialGroupBuyers;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Materials.MaterialApprovalRequests;
using QuoteFlow.Materials.MaterialGroups;
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
using QuoteFlow.PurchaseOrdersSapImports;
using QuoteFlow.SaleOrderDetails;
using QuoteFlow.SaleOrders;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.SaleOrdersSapImports;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared.Consts;
using QuoteFlow.SpecialInputPrices;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;
using QuoteFlow.SpoBatchRequestDetails;
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
using QuoteFlow.Suppliers;
using QuoteFlow.SystemCategories;
using QuoteFlow.SystemConfigurations;
using QuoteFlow.WorkflowApprovers;
using QuoteFlow.WorkflowConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Gdpr;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;

namespace QuoteFlow.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ConnectionStringName("Default")]
public class QuoteFlowDbContext :
    AbpDbContext<QuoteFlowDbContext>,
    IIdentityProDbContext
{
    public DbSet<AssetRequestDetail> AssetRequestDetails { get; set; } = null!;
    public DbSet<AssetRequest> AssetRequests { get; set; } = null!;
    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<CfgDiscountRatio> CfgDiscountRatios { get; set; } = null!;
    public DbSet<SpoBatchRequestDetail> SpoBatchRequestDetails { get; set; } = null!;
    public DbSet<SpoBatchRequest> SpoBatchRequests { get; set; } = null!;
    public DbSet<HistoryTracking> HistoryTrackings { get; set; } = null!;
    public DbSet<MaterialHistoryTracking> MaterialHistoryTrackings { get; set; } = null!;
    public DbSet<StockHistoryTracking> StockHistoryTrackings { get; set; } = null!;
    public DbSet<MaterialStockLockShipment> MaterialStockLockShipments { get; set; } = null!;
    public DbSet<PurchaseOrderLockShipment> PurchaseOrderLockShipments { get; set; } = null!;
    public DbSet<PurchaseOrdersSapImport> PurchaseOrdersSapImports { get; set; } = null!;
    public DbSet<SaleOrdersSapImport> SaleOrdersSapImports { get; set; } = null!;
    public DbSet<StockImportAllocation> StockImportAllocations { get; set; } = null!;
    public DbSet<DistributorTarget> DistributorTargets { get; set; } = null!;
    public DbSet<MaterialStockLockStock> MaterialStockLockStocks { get; set; } = null!;
    public DbSet<StockImportPriority> StockImportPriorities { get; set; } = null!;
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = null!;
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
    public DbSet<SaleOrderDetail> SaleOrderDetails { get; set; } = null!;
    public DbSet<SaleOrder> SaleOrders { get; set; } = null!;
    public DbSet<StockImportDetail> StockImportDetails { get; set; } = null!;
    public DbSet<StockImport> StockImports { get; set; } = null!;
    public DbSet<MaterialGroupBuyer> MaterialGroupBuyers { get; set; } = null!;
    public DbSet<MaterialStockUpload> MaterialStockUploads { get; set; } = null!;
    public DbSet<SystemConfiguration> SystemConfigurations { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<CustomerPIC> CustomerPICs { get; set; } = null!;
    public DbSet<MaterialStockUploadDetail> MaterialStockUploadDetails { get; set; } = null!;
    public DbSet<WorkflowConfiguration> WorkflowConfigurations { get; set; } = null!;
    public DbSet<WorkflowApprover> WorkflowApprovers { get; set; } = null!;
    public DbSet<SpecialInputPriceDetail> SpecialInputPriceDetails { get; set; } = null!;
    public DbSet<SpecialInputPrice> SpecialInputPrices { get; set; } = null!;
    public DbSet<DPODetail> DPODetails { get; set; } = null!;
    public DbSet<DPO> DPOs { get; set; } = null!;
    public DbSet<ApprovalRoute> ApprovalRoutes { get; set; } = null!;
    public DbSet<MaterialApprovalRequestDetail> MaterialApprovalRequestDetails { get; set; } = null!;
    public DbSet<MaterialApprovalRequest> MaterialApprovalRequests { get; set; } = null!;
    public DbSet<ApprovalHistory> ApprovalHistories { get; set; } = null!;
    public DbSet<AddMoreItemHistory> AddMoreItemHistories { get; set; } = null!;
    public DbSet<StockCategory> StockCategories { get; set; } = null!;
    public DbSet<MaterialStock> MaterialStocks { get; set; } = null!;
    public DbSet<MaterialHistory> MaterialHistories { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<CargoData> CargoDatas { get; set; } = null!;
    public DbSet<Cargo> Cargos { get; set; } = null!;
    public DbSet<PSIDetail> PSIDetails { get; set; } = null!;
    public DbSet<PriceOfferDetail> PriceOfferDetails { get; set; } = null!;
    public DbSet<PriceOfferCustomer> PriceOfferCustomers { get; set; } = null!;
    public DbSet<PriceOffer> PriceOffers { get; set; } = null!;
    public DbSet<KeyAccountEvaluation> KeyAccountEvaluations { get; set; } = null!;
    public DbSet<Material> Materials { get; set; } = null!;
    public DbSet<StockTracingDetail> StockTracingDetails { get; set; } = null!;
    public DbSet<StockTracing> StockTracings { get; set; } = null!;
    public DbSet<SystemCategory> SystemCategories { get; set; } = null!;
    public DbSet<KeyAccount> KeyAccounts { get; set; } = null!;
    public DbSet<Buyer> Buyers { get; set; } = null!;
    public DbSet<SalesAssignment> SalesAssignments { get; set; } = null!;
    public DbSet<MaterialGroup> MaterialGroups { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;

    public DbSet<SupplierBU> SupplierBUs { get; set; } = null!;

    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext 
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    #endregion

    public QuoteFlowDbContext(DbContextOptions<QuoteFlowDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureOpenIddictPro();
        builder.ConfigureIdentityPro();
        builder.ConfigureFeatureManagement();
        builder.ConfigureLanguageManagement();
        builder.ConfigureFileManagement();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureGdpr();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(QuoteFlowConsts.DbTablePrefix + "YourEntities", QuoteFlowConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        if (builder.IsHostDatabase())
        {
            builder.Entity<KeyAccount>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "KeyAccount", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.ConfigureObjectHasApprovalRoute();

                b.Property(x => x.BuyerId).HasColumnName(nameof(KeyAccount.BuyerId)).IsRequired();
                b.Property(x => x.BuyerShortName).HasColumnName(nameof(KeyAccount.BuyerShortName)).HasMaxLength(KeyAccountConsts.BuyerShortNameMaxLength);

                b.Property(x => x.KeyAccountCode).HasColumnName(nameof(KeyAccount.KeyAccountCode)).IsRequired().HasMaxLength(KeyAccountConsts.KeyAccountCodeMaxLength);
                b.Property(x => x.KeyAccountShortName).HasColumnName(nameof(KeyAccount.KeyAccountShortName)).HasMaxLength(KeyAccountConsts.KeyAccountShortNameMaxLength);
                b.Property(x => x.KeyAccountName).HasColumnName(nameof(KeyAccount.KeyAccountName)).HasMaxLength(KeyAccountConsts.KeyAccountNameMaxLength);

                b.Property(x => x.KeyAccountType).HasColumnName(nameof(KeyAccount.KeyAccountType)).HasMaxLength(SystemCategoryConsts.DescriptionMaxLength);
                b.Property(x => x.KeyAccountClassBuyer).HasColumnName(nameof(KeyAccount.KeyAccountClassBuyer)).HasMaxLength(SystemCategoryConsts.DescriptionMaxLength);
                b.Property(x => x.KeyAccountClass).HasColumnName(nameof(KeyAccount.KeyAccountClass)).HasMaxLength(SystemCategoryConsts.DescriptionMaxLength);

                b.Property(x => x.MaterialType).HasColumnName(nameof(KeyAccount.MaterialType)).HasMaxLength(SystemCategoryConsts.DescriptionMaxLength);

                b.Property(x => x.TargetEndUsers).HasColumnName(nameof(KeyAccount.TargetEndUsers)).HasMaxLength(KeyAccountConsts.TargetEndUsersMaxLength);
                b.Property(x => x.Industry).HasColumnName(nameof(KeyAccount.Industry)).HasMaxLength(KeyAccountConsts.IndustryMaxLength);
                b.Property(x => x.RegistrationYear).HasColumnName(nameof(KeyAccount.RegistrationYear));

                b.Property(x => x.CustomerTaxCode).HasColumnName(nameof(KeyAccount.CustomerTaxCode)).IsRequired().HasMaxLength(KeyAccountConsts.CustomerTaxCodeMaxLength);
                b.Property(x => x.CustomerName).HasColumnName(nameof(KeyAccount.CustomerName)).HasMaxLength(KeyAccountConsts.CustomerNameMaxLength);
                b.Property(x => x.CustomerAddress).HasColumnName(nameof(KeyAccount.CustomerAddress)).HasMaxLength(KeyAccountConsts.CustomerAddressMaxLength);
                b.Property(x => x.CustomerProvince).HasColumnName(nameof(KeyAccount.CustomerProvince)).HasMaxLength(KeyAccountConsts.CustomerProvinceMaxLength);
                b.Property(x => x.CustomerWebsite).HasColumnName(nameof(KeyAccount.CustomerWebsite)).HasMaxLength(KeyAccountConsts.CustomerWebsiteMaxLength);
                b.Property(x => x.CustomerPhone).HasColumnName(nameof(KeyAccount.CustomerPhone)).HasMaxLength(QuoteFlowSharedConsts.PhoneMaxLength);

                b.Property(x => x.CurrentSaleRoute).HasColumnName(nameof(KeyAccount.CurrentSaleRoute)).HasMaxLength(KeyAccountConsts.CurrentSaleRouteMaxLength);
                b.Property(x => x.CustomerCountry).HasColumnName(nameof(KeyAccount.CustomerCountry)).HasMaxLength(KeyAccountConsts.CustomerCountryMaxLength);

                b.Property(x => x.CustomerLocation).HasColumnName(nameof(KeyAccount.CustomerLocation));

                b.Property(x => x.LastRegisteredProjectCode).HasColumnName(nameof(KeyAccount.LastRegisteredProjectCode)).HasMaxLength(KeyAccountConsts.LastRegisteredProjectCodeMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(KeyAccount.Note)).HasMaxLength(QuoteFlowSharedConsts.NoteMaxLength);
                b.Property(x => x.IsDeactive).HasColumnName(nameof(KeyAccount.IsDeactive));
                b.Property(x => x.Status).HasColumnName(nameof(KeyAccount.Status)).HasMaxLength(KeyAccountConsts.StatusMaxLength);

                b.HasOne(x => x.Buyer).WithMany().HasForeignKey(x => x.BuyerId).OnDelete(DeleteBehavior.Restrict);

                b.HasMany(x => x.CustomerPICs).WithOne().HasForeignKey(x => x.KeyAccountId).OnDelete(DeleteBehavior.Restrict);
                b.HasMany(x => x.KeyAccountEvaluation).WithOne().HasForeignKey(x => x.KeyAccountId).OnDelete(DeleteBehavior.Restrict);
                b.HasMany(x => x.ApprovalHistories).WithOne().HasForeignKey(x => x.KeyAccountId).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.ApprovalRoutes).WithOne().HasForeignKey(x => x.KeyAccountId).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.Attachments).WithOne().HasForeignKey(x => x.KeyAccountId).OnDelete(DeleteBehavior.Cascade);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SystemCategory>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SystemCategories", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.ParentId).HasColumnName(nameof(SystemCategory.ParentId));
                b.Property(x => x.Code).HasColumnName(nameof(SystemCategory.Code)).IsRequired().HasMaxLength(SystemCategoryConsts.CodeMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(SystemCategory.Description)).IsRequired().HasMaxLength(SystemCategoryConsts.DescriptionMaxLength);
                b.Property(x => x.Value).HasColumnName(nameof(SystemCategory.Value)).HasPrecision(18, 2);
                b.Property(x => x.CategoryType).HasColumnName(nameof(SystemCategory.CategoryType)).IsRequired().HasMaxLength(SystemCategoryConsts.CategoryTypeMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(SystemCategory.Note)).HasMaxLength(QuoteFlowSharedConsts.NoteMaxLength);
                b.Property(x => x.IsDeactive).HasColumnName(nameof(SystemCategory.IsDeactive));
                b.Property(x => x.SortOrder).HasColumnName(nameof(SystemCategory.SortOrder));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<StockTracing>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "StockTracings", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.FileName).HasColumnName(nameof(StockTracing.FileName)).HasMaxLength(StockTracingConsts.FileNameMaxLength);
                b.Property(x => x.ReportType).HasColumnName(nameof(StockTracing.ReportType)).HasConversion<EnumToStringConverter<ReportType>>();
                b.Property(x => x.FromDate).HasColumnName(nameof(StockTracing.FromDate));
                b.Property(x => x.ToDate).HasColumnName(nameof(StockTracing.ToDate));
                b.Property(x => x.Note).HasColumnName(nameof(StockTracing.Note)).HasMaxLength(QuoteFlowSharedConsts.NoteMaxLength);

                b.HasMany(x => x.Details).WithOne(d => d.StockTracing).HasForeignKey(d => d.StockTracingId).OnDelete(DeleteBehavior.Cascade);
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<StockTracingDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "StockTracingDetails", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.StockTracingId).HasColumnName(nameof(StockTracingDetail.StockTracingId)).IsRequired();
                b.Property(x => x.ReportType).HasColumnName(nameof(StockTracingDetail.ReportType)).HasConversion<EnumToStringConverter<ReportType>>();
                b.Property(x => x.RowNo).HasColumnName(nameof(StockTracingDetail.RowNo));
                b.Property(x => x.PackingListCode).HasColumnName(nameof(StockTracingDetail.PackingListCode)).HasMaxLength(StockTracingDetailConsts.PackingListCodeMaxLength);
                b.Property(x => x.CheckListCode).HasColumnName(nameof(StockTracingDetail.CheckListCode)).HasMaxLength(StockTracingDetailConsts.CheckListCodeMaxLength);
                b.Property(x => x.DateEntered).HasColumnName(nameof(StockTracingDetail.DateEntered));
                b.Property(x => x.Stock).HasColumnName(nameof(StockTracingDetail.Stock)).HasMaxLength(StockTracingDetailConsts.StockMaxLength);
                b.Property(x => x.BU).HasColumnName(nameof(StockTracingDetail.BU)).HasMaxLength(StockTracingDetailConsts.BUMaxLength);
                b.Property(x => x.Customer).HasColumnName(nameof(StockTracingDetail.Customer)).HasMaxLength(StockTracingDetailConsts.CustomerMaxLength);
                b.Property(x => x.Category).HasColumnName(nameof(StockTracingDetail.Category)).HasMaxLength(StockTracingDetailConsts.CategoryMaxLength);
                b.Property(x => x.GIV).HasColumnName(nameof(StockTracingDetail.GIV)).HasMaxLength(StockTracingDetailConsts.GIVMaxLength);
                b.Property(x => x.Invoice).HasColumnName(nameof(StockTracingDetail.Invoice)).HasMaxLength(StockTracingDetailConsts.InvoiceMaxLength);
                b.Property(x => x.SKUCode).HasColumnName(nameof(StockTracingDetail.SKUCode)).HasMaxLength(StockTracingDetailConsts.SKUCodeMaxLength);
                b.Property(x => x.SKUName).HasColumnName(nameof(StockTracingDetail.SKUName)).HasMaxLength(StockTracingDetailConsts.SKUNameMaxLength);
                b.Property(x => x.Quality).HasColumnName(nameof(StockTracingDetail.Quality)).HasMaxLength(StockTracingDetailConsts.QualityMaxLength);
                b.Property(x => x.Warranty).HasColumnName(nameof(StockTracingDetail.Warranty)).HasMaxLength(StockTracingDetailConsts.WarrantyMaxLength);
                b.Property(x => x.Unit).HasColumnName(nameof(StockTracingDetail.Unit)).HasMaxLength(StockTracingDetailConsts.UnitMaxLength);
                b.Property(x => x.Series).HasColumnName(nameof(StockTracingDetail.Series)).HasMaxLength(StockTracingDetailConsts.SeriesMaxLength);
                b.Property(x => x.OriginCode).HasColumnName(nameof(StockTracingDetail.OriginCode)).HasMaxLength(StockTracingDetailConsts.OriginCodeMaxLength);
                b.Property(x => x.ProductionDate).HasColumnName(nameof(StockTracingDetail.ProductionDate));
                b.Property(x => x.Location).HasColumnName(nameof(StockTracingDetail.Location)).HasMaxLength(StockTracingDetailConsts.LocationMaxLength);
                b.Property(x => x.GolfaCode).HasColumnName(nameof(StockTracingDetail.GolfaCode)).HasMaxLength(StockTracingDetailConsts.GolfaCodeMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(StockTracingDetail.Note)).HasMaxLength(QuoteFlowSharedConsts.NoteMaxLength);
            });
        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<KeyAccountEvaluation>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "KeyAccount_Evaluations", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.KeyAccountId).HasColumnName("KeyAccount_Id").IsRequired();
                b.Property(x => x.EvaluationType).HasColumnName(nameof(KeyAccountEvaluation.EvaluationType)).IsRequired().HasMaxLength(KeyAccountEvaluationConsts.EvaluationTypeMaxLength);
                b.Property(x => x.EvaluationId).HasColumnName(nameof(KeyAccountEvaluation.EvaluationId)).IsRequired();
                b.Property(x => x.BuyerInfo1).HasColumnName("Buyer_Info1").HasMaxLength(KeyAccountEvaluationConsts.BuyerInfo1MaxLength);
                b.Property(x => x.BuyerInfo2).HasColumnName("Buyer_Info2").HasMaxLength(KeyAccountEvaluationConsts.BuyerInfo2MaxLength);
                b.Property(x => x.MEVNInfo1).HasColumnName("MEVN_Info1").HasMaxLength(KeyAccountEvaluationConsts.MEVNInfo1MaxLength);
                b.Property(x => x.MEVNInfo2).HasColumnName("MEVN_Info2").HasMaxLength(KeyAccountEvaluationConsts.MEVNInfo2MaxLength);
                b.Property(x => x.CompetitorInfo1).HasColumnName("Competitor_Info1").HasMaxLength(KeyAccountEvaluationConsts.CompetitorInfo1MaxLength);
                b.Property(x => x.CompetitorInfo2).HasColumnName("Competitor_Info2").HasMaxLength(KeyAccountEvaluationConsts.CompetitorInfo2MaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(KeyAccountEvaluation.Note)).HasMaxLength(QuoteFlowSharedConsts.NoteMaxLength);
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PriceOffer>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "PriceOffer", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.ConfigureObjectHasApprovalRoute();

                b.Property(x => x.PriceOfferCode).HasColumnName("PriceOffer_Code").IsRequired().HasMaxLength(PriceOfferConsts.PriceOfferCodeMaxLength);
                b.Property(x => x.BuyerId).HasColumnName(nameof(PriceOffer.BuyerId));
                b.Property(x => x.BuyerCode).HasColumnName(nameof(PriceOffer.BuyerCode)).HasMaxLength(PriceOfferConsts.BuyerCodeMaxLength);
                b.Property(x => x.BuyerTypeId).HasColumnName(nameof(PriceOffer.BuyerTypeId));
                b.Property(x => x.MaterialType).HasColumnName(nameof(PriceOffer.MaterialType)).IsRequired().HasMaxLength(PriceOfferConsts.MaterialTypeMaxLength);
                b.Property(x => x.LocationId).HasColumnName(nameof(PriceOffer.LocationId));
                b.Property(x => x.LocationOld).HasColumnName("Location_Old").HasMaxLength(PriceOfferConsts.LocationOldMaxLength);
                b.Property(x => x.ProjectName).HasColumnName(nameof(PriceOffer.ProjectName)).HasMaxLength(PriceOfferConsts.ProjectNameMaxLength);
                b.Property(x => x.ProjectTypeId).HasColumnName(nameof(PriceOffer.ProjectTypeId));
                b.Property(x => x.EUIndustryId).HasColumnName(nameof(PriceOffer.EUIndustryId));
                b.Property(x => x.Application).HasColumnName(nameof(PriceOffer.Application)).HasMaxLength(PriceOfferConsts.ApplicationMaxLength);
                b.Property(x => x.Country).HasColumnName(nameof(PriceOffer.Country)).HasMaxLength(PriceOfferConsts.CountryMaxLength);
                b.Property(x => x.Province).HasColumnName(nameof(PriceOffer.Province)).HasMaxLength(PriceOfferConsts.ProvinceMaxLength);
                b.Property(x => x.DetailedAddress).HasColumnName(nameof(PriceOffer.DetailedAddress)).HasMaxLength(PriceOfferConsts.DetailedAddressMaxLength);
                b.Property(x => x.CompetitorBrand).HasColumnName(nameof(PriceOffer.CompetitorBrand)).HasMaxLength(PriceOfferConsts.CompetitorBrandMaxLength);
                b.Property(x => x.PriceGapWithCompetitor).HasColumnName(nameof(PriceOffer.PriceGapWithCompetitor)).HasMaxLength(PriceOfferConsts.PriceGapWithCompetitorMaxLength);
                b.Property(x => x.DecisionRight).HasColumnName(nameof(PriceOffer.DecisionRight)).HasMaxLength(PriceOfferConsts.DecisionRightMaxLength);
                b.Property(x => x.POPlannedDate).HasColumnName(nameof(PriceOffer.POPlannedDate));
                b.Property(x => x.DeliveryDate).HasColumnName(nameof(PriceOffer.DeliveryDate));
                b.Property(x => x.UpcomingPotentialProjects).HasColumnName(nameof(PriceOffer.UpcomingPotentialProjects)).HasMaxLength(PriceOfferConsts.UpcomingPotentialProjectsMaxLength);
                b.Property(x => x.OtherPJInformation).HasColumnName(nameof(PriceOffer.OtherPJInformation)).HasMaxLength(PriceOfferConsts.OtherPJInformationMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(PriceOffer.FileName)).HasMaxLength(PriceOfferConsts.FileNameMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(PriceOffer.Note)).HasMaxLength(QuoteFlowSharedConsts.NoteMaxLength);
                b.Property(x => x.CloseDate).HasColumnName(nameof(PriceOffer.CloseDate));
                b.Property(x => x.TotalMEVNOfferAmount).HasColumnName(nameof(PriceOffer.TotalMEVNOfferAmount)).HasPrecision(18, 2);
                b.Property(x => x.AccountNo).HasColumnName(nameof(PriceOffer.AccountNo)).HasMaxLength(PriceOfferConsts.AccountNoMaxLength);
                b.Property(x => x.KeyAccountId).HasColumnName(nameof(PriceOffer.KeyAccountId));
                b.Property(x => x.KeyAccountTypeId).HasColumnName(nameof(PriceOffer.KeyAccountTypeId));
                b.Property(x => x.KeyAccountClassId).HasColumnName(nameof(PriceOffer.KeyAccountClassId));
                b.Property(x => x.ProjectResultStatus).HasColumnName(nameof(PriceOffer.ProjectResultStatus)).HasMaxLength(PriceOfferConsts.ProjectResultStatusMaxLength);
                b.Property(x => x.ProjectResultNote).HasColumnName(nameof(PriceOffer.ProjectResultNote)).HasMaxLength(PriceOfferConsts.ProjectResultNoteMaxLength);
                b.Property(x => x.ProjectResultSubmittedAt).HasColumnName(nameof(PriceOffer.ProjectResultSubmittedAt));
                b.Property(x => x.ProjectResultSubmitterId).HasColumnName(nameof(PriceOffer.ProjectResultSubmitterId));
                b.Property(x => x.ProjectResultSubmitterUsername).HasColumnName(nameof(PriceOffer.ProjectResultSubmitterUsername)).HasMaxLength(PriceOfferConsts.ProjectResultSubmitterUsernameMaxLength); b.Property(x => x.ProjectResultSubmitterFullName).HasColumnName(nameof(PriceOffer.ProjectResultSubmitterFullName)).HasMaxLength(PriceOfferConsts.ProjectResultSubmitterFullNameMaxLength);
                b.Property(x => x.ApprovalStatus).HasColumnName(nameof(PriceOffer.ApprovalStatus)).HasMaxLength(PriceOfferConsts.ProjectResultStatusMaxLength);
                b.Property(x => x.TotalStandardAmount).HasColumnName(nameof(PriceOffer.TotalStandardAmount)).HasPrecision(18, 2);
                b.Property(x => x.TotalRequestedAmount).HasColumnName(nameof(PriceOffer.TotalRequestedAmount)).HasPrecision(18, 2);
                b.Property(x => x.TotalPriceToCustomer).HasColumnName(nameof(PriceOffer.TotalPriceToCustomer)).HasPrecision(18, 2);
                b.Property(x => x.DiscountRatio).HasColumnName("SPO_DiscountRatio").HasPrecision(18, 5);
                b.Property(x => x.DiscountRatioConfigured).HasColumnName("SPO_DiscountRatio_CFG").HasPrecision(18, 5);
                b.Property(x => x.TotalMarginIssues).HasColumnName("MarginIssue");
                b.Property(x => x.SpecialInputPriceId).HasColumnName(nameof(PriceOffer.SpecialInputPriceId));
                b.Property(x => x.SpecialInputPriceAssignmentNote).HasColumnName(nameof(PriceOffer.SpecialInputPriceAssignmentNote)).HasMaxLength(QuoteFlowSharedConsts.NoteMaxLength);
                b.Property(x => x.SpecialInputPriceAccountName).HasColumnName(nameof(PriceOffer.SpecialInputPriceAccountName)).HasMaxLength(SpecialInputPriceConsts.AccountNameMaxLength);
                b.Property(x => x.SpecialInputPriceAssignedTime).HasColumnName(nameof(PriceOffer.SpecialInputPriceAssignedTime));
                b.Property(x => x.SpecialInputPriceAssignerId).HasColumnName(nameof(PriceOffer.SpecialInputPriceAssignerId));
                b.Property(x => x.SpecialInputPriceAssignerUsername).HasColumnName(nameof(PriceOffer.SpecialInputPriceAssignerUsername)).HasMaxLength(ExtendedAuditedObjectConsts.MaxCreatorUsernameLength);
                b.Property(x => x.SpecialInputPriceAssignerFullName).HasColumnName(nameof(PriceOffer.SpecialInputPriceAssignerFullName)).HasMaxLength(ExtendedAuditedObjectConsts.MaxCreatorNameLength);
                b.Property(x => x.InitialTotalMEVNOfferAmount).HasColumnName(nameof(PriceOffer.InitialTotalMEVNOfferAmount)).HasPrecision(18, 2);
                b.Property(x => x.HasDPOUsed).HasColumnName(nameof(PriceOffer.HasDPOUsed)).HasDefaultValue(false);

                // Denormalization fields
                b.Property(x => x.BuyerTypeDescription).HasColumnName(nameof(PriceOffer.BuyerTypeDescription)).HasMaxLength(PriceOfferConsts.BuyerTypeDescriptionMaxLength);
                b.Property(x => x.ProjectTypeDescription).HasColumnName(nameof(PriceOffer.ProjectTypeDescription)).HasMaxLength(PriceOfferConsts.ProjectTypeDescriptionMaxLength);
                b.Property(x => x.EUIndustryDescription).HasColumnName(nameof(PriceOffer.EUIndustryDescription)).HasMaxLength(PriceOfferConsts.EUIndustryDescriptionMaxLength);
                b.Property(x => x.KeyAccountClassDescription).HasColumnName(nameof(PriceOffer.KeyAccountClassDescription)).HasMaxLength(PriceOfferConsts.KeyAccountClassDescriptionMaxLength);
                b.Property(x => x.KeyAccountTypeDescription).HasColumnName(nameof(PriceOffer.KeyAccountTypeDescription)).HasMaxLength(PriceOfferConsts.KeyAccountTypeDescriptionMaxLength);
                b.Property(x => x.LocationDescription).HasColumnName(nameof(PriceOffer.LocationDescription)).HasMaxLength(PriceOfferConsts.LocationDescriptionMaxLength);

                b.HasOne(x => x.Buyer).WithMany().HasForeignKey(x => x.BuyerId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.KeyAccount).WithMany().HasForeignKey(x => x.KeyAccountId).OnDelete(DeleteBehavior.Restrict);
                b.HasMany(x => x.Customers).WithOne(y => y.PriceOffer).HasForeignKey(x => x.PriceOfferId).OnDelete(DeleteBehavior.Restrict);
                b.HasMany(x => x.Details).WithOne(y => y.PriceOffer).HasForeignKey(x => x.PriceOfferId).OnDelete(DeleteBehavior.Restrict);
                b.HasMany(x => x.ApprovalHistories).WithOne().HasForeignKey(x => x.PriceOfferId).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.ApprovalRoutes).WithOne().HasForeignKey(x => x.PriceOfferId).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.Messages).WithOne().HasForeignKey(x => x.PriceOfferId).OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(x => x.PriceOfferCode).IsUnique().HasDatabaseName("IX_PriceOffer_Code");
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PriceOfferCustomer>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "PriceOffer_Customer", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.PriceOfferId).HasColumnName(nameof(PriceOfferCustomer.PriceOfferId)).IsRequired();
                b.Property(x => x.SaleChannel).HasColumnName(nameof(PriceOfferCustomer.SaleChannel)).IsRequired().HasMaxLength(PriceOfferCustomerConsts.SaleChannelMaxLength);
                b.Property(x => x.SaleChannelNumber).HasColumnName(nameof(PriceOfferCustomer.SaleChannelNumber)).IsRequired();
                b.Property(x => x.CustomerId).HasColumnName(nameof(PriceOfferCustomer.CustomerId)).IsRequired();
                b.Property(x => x.CustomerTaxCode).HasColumnName(nameof(PriceOfferCustomer.CustomerTaxCode)).HasMaxLength(PriceOfferCustomerConsts.CustomerTaxCodeMaxLength);
                b.Property(x => x.CustomerName).HasColumnName(nameof(PriceOfferCustomer.CustomerName)).HasMaxLength(PriceOfferCustomerConsts.CustomerNameMaxLength);
                b.Property(x => x.CustomerAddress).HasColumnName(nameof(PriceOfferCustomer.CustomerAddress)).HasMaxLength(PriceOfferCustomerConsts.CustomerAddressMaxLength);
                b.Property(x => x.CustomerNationality).HasColumnName(nameof(PriceOfferCustomer.CustomerNationality)).HasMaxLength(PriceOfferCustomerConsts.CustomerNationalityMaxLength);
                b.Property(x => x.CustomerType).HasColumnName(nameof(PriceOfferCustomer.CustomerType)).HasMaxLength(PriceOfferCustomerConsts.CustomerTypeMaxLength);
                b.Property(x => x.CustomerIndustry).HasColumnName(nameof(PriceOfferCustomer.CustomerIndustry)).HasMaxLength(PriceOfferCustomerConsts.CustomerIndustryMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(PriceOfferCustomer.Note)).HasMaxLength(PriceOfferCustomerConsts.NoteMaxLength);
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PriceOfferDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "PriceOfferDetail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.RowNo).HasColumnName(nameof(PriceOfferDetail.RowNo)).IsRequired();
                b.Property(x => x.PriceOfferId).HasColumnName(nameof(PriceOfferDetail.PriceOfferId)).IsRequired();
                b.Property(x => x.GolfaCode).HasColumnName(nameof(PriceOfferDetail.GolfaCode)).IsRequired().HasMaxLength(PriceOfferDetailConsts.GolfaCodeMaxLength);
                b.Property(x => x.ModelName).HasColumnName(nameof(PriceOfferDetail.ModelName)).IsRequired().HasMaxLength(PriceOfferDetailConsts.ModelNameMaxLength);
                b.Property(x => x.SpecialSpec1).HasColumnName(nameof(PriceOfferDetail.SpecialSpec1)).HasMaxLength(PriceOfferDetailConsts.SpecialSpec1MaxLength);
                b.Property(x => x.SpecialSpec2).HasColumnName(nameof(PriceOfferDetail.SpecialSpec2)).HasMaxLength(PriceOfferDetailConsts.SpecialSpec2MaxLength);
                b.Property(x => x.DpoUsed).HasColumnName(nameof(PriceOfferDetail.DpoUsed)).HasPrecision(18, 2);
                b.Property(x => x.Qty).HasColumnName(nameof(PriceOfferDetail.Qty)).IsRequired().HasPrecision(18, 2);
                b.Property(x => x.StandardPrice).HasColumnName(nameof(PriceOfferDetail.StandardPrice)).IsRequired().HasPrecision(18, 2);
                b.Property(x => x.StandardAmount).HasColumnName(nameof(PriceOfferDetail.StandardAmount)).IsRequired().HasPrecision(18, 2);
                b.Property(x => x.BuyerPrice).HasColumnName(nameof(PriceOfferDetail.BuyerPrice)).HasPrecision(18, 2);
                b.Property(x => x.RequestedAmount).HasColumnName(nameof(PriceOfferDetail.RequestedAmount)).HasPrecision(18, 2);
                b.Property(x => x.RequestedDiscountRatio).HasColumnName(nameof(PriceOfferDetail.RequestedDiscountRatio)).HasPrecision(18, 5);
                b.Property(x => x.PriceToCustomer).HasColumnName(nameof(PriceOfferDetail.PriceToCustomer)).HasPrecision(18, 2);
                b.Property(x => x.MEVNOfferPrice).HasColumnName(nameof(PriceOfferDetail.MEVNOfferPrice)).IsRequired().HasPrecision(18, 2);
                b.Property(x => x.CompetitorBrand).HasColumnName(nameof(PriceOfferDetail.CompetitorBrand)).HasMaxLength(PriceOfferDetailConsts.CompetitorBrandMaxLength);
                b.Property(x => x.CompetitorModel).HasColumnName(nameof(PriceOfferDetail.CompetitorModel)).HasMaxLength(PriceOfferDetailConsts.CompetitorModelMaxLength);
                b.Property(x => x.CompetitorPrice).HasColumnName(nameof(PriceOfferDetail.CompetitorPrice)).HasPrecision(18, 2);
                b.Property(x => x.LandingCost).HasColumnName(nameof(PriceOfferDetail.LandingCost)).HasPrecision(18, 2);
                b.Property(x => x.InputPrice).HasColumnName(nameof(PriceOfferDetail.InputPrice)).HasPrecision(18, 2);
                b.Property(x => x.InputCurrency).HasColumnName(nameof(PriceOfferDetail.InputCurrency)).HasMaxLength(PriceOfferDetailConsts.InputCurrencyMaxLength);
                b.Property(x => x.ManagerMargin).HasColumnName(nameof(PriceOfferDetail.ManagerMargin)).HasPrecision(18, 2);
                b.Property(x => x.PriceOfferDetailMargin).HasColumnName(nameof(PriceOfferDetail.PriceOfferDetailMargin)).HasPrecision(18, 2);
                b.Property(x => x.AccountCode).HasColumnName(nameof(PriceOfferDetail.AccountCode)).HasMaxLength(PriceOfferDetailConsts.AccountCodeMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(PriceOfferDetail.Note)).HasMaxLength(PriceOfferDetailConsts.NoteMaxLength);
                b.Property(x => x.ImportGuid).HasColumnName(nameof(PriceOfferDetail.ImportGuid)).IsRequired();
                b.Property(x => x.Status).HasColumnName(nameof(PriceOfferDetail.Status)).HasMaxLength(QuoteFlowSharedConsts.StatusMaxLength);
                b.Property(x => x.MaxSalesOfferPrice).HasColumnName(nameof(PriceOfferDetail.MaxSalesOfferPrice)).HasPrecision(18, 2);
                b.Property(x => x.MaxMangerOfferPrice).HasColumnName(nameof(PriceOfferDetail.MaxMangerOfferPrice)).HasPrecision(18, 2);
                b.Property(x => x.ActualDiscountRatio).HasColumnName(nameof(PriceOfferDetail.ActualDiscountRatio)).HasPrecision(18, 5);

                b.HasMany(x => x.ApprovalHistories).WithOne().HasForeignKey(x => x.PriceOfferDetailId).OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(x => x.GolfaCode).HasDatabaseName("IX_PriceOfferDetail_GolfaCode");
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SalesAssignment>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SaleTeam", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.SaleUserName).HasColumnName(nameof(SalesAssignment.SaleUserName)).IsRequired().HasMaxLength(SalesAssignmentConsts.SaleUserNameMaxLength);
                b.Property(x => x.SaleFullName).HasColumnName(nameof(SalesAssignment.SaleFullName)).HasMaxLength(SalesAssignmentConsts.SaleFullNameMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(SalesAssignment.MaterialType)).IsRequired().HasMaxLength(SalesAssignmentConsts.MaterialTypeMaxLength);
                b.Property(x => x.LocationId).HasColumnName(nameof(SalesAssignment.LocationId)).IsRequired();
                b.Property(x => x.BuyerId).HasColumnName(nameof(SalesAssignment.BuyerId)).IsRequired();
                b.Property(x => x.BuyerShortName).HasColumnName(nameof(SalesAssignment.BuyerShortName)).HasMaxLength(SalesAssignmentConsts.BuyerShortNameMaxLength);
                b.Property(x => x.BuyerTypeId).HasColumnName(nameof(SalesAssignment.BuyerTypeId)).IsRequired();
                b.Property(x => x.Note).HasColumnName(nameof(SalesAssignment.Note)).HasMaxLength(QuoteFlowSharedConsts.NoteMaxLength);

                b.HasOne(x => x.Buyer).WithMany().HasForeignKey(x => x.BuyerId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.BuyerType).WithMany().HasForeignKey(x => x.BuyerTypeId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Location).WithMany().HasForeignKey(x => x.LocationId).OnDelete(DeleteBehavior.Restrict);
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PSI>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "PSI", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.ConfigureObjectHasApprovalRoute();

                b.Property(x => x.PSICode).HasColumnName("PSI_Code").IsRequired().HasMaxLength(PSIConsts.PSI_CodeMaxLength);
                b.Property(x => x.FY).HasColumnName(nameof(PSI.FY));
                b.Property(x => x.FileName).HasColumnName(nameof(PSI.FileName)).HasMaxLength(PSIConsts.FileNameMaxLength);
                b.Property(x => x.ImportType).HasColumnName(nameof(PSI.ImportType)).HasMaxLength(PSIConsts.ImportTypeMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(PSI.Note)).HasMaxLength(PSIConsts.NoteMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(PSI.Status)).HasMaxLength(PSIConsts.StatusMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(PSI.MaterialType)).HasMaxLength(PSIConsts.StatusMaxLength);
                b.HasMany(x => x.ApprovalHistories).WithOne().HasForeignKey(y => y.PSI_Id).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.ApprovalRoutes).WithOne().HasForeignKey(y => y.PSI_Id).OnDelete(DeleteBehavior.Cascade);
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Cargo>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Cargo", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.FileName).HasColumnName(nameof(Cargo.FileName)).HasMaxLength(CargoConsts.FileNameMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(Cargo.Note));
                b.Property(x => x.SupplierCode).HasColumnName(nameof(Cargo.SupplierCode)).HasMaxLength(CargoConsts.SupplierCodeMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(Cargo.MaterialType)).HasMaxLength(CargoConsts.MaterialTypeMaxLength);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<CargoData>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "CargoData", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.CargoId).HasColumnName(nameof(CargoData.CargoId));
                b.Property(x => x.PODetailId).HasColumnName(nameof(CargoData.PODetailId));
                b.Property(x => x.PODetailCode).HasColumnName(nameof(CargoData.PODetailCode)).HasMaxLength(CargoDataConsts.PODetailCodeMaxLength);
                b.Property(x => x.GolfaCode).HasColumnName(nameof(CargoData.GolfaCode)).IsRequired().HasMaxLength(CargoDataConsts.GolfaCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(CargoData.Model)).IsRequired().HasMaxLength(CargoDataConsts.ModelMaxLength);
                b.Property(x => x.PORef).HasColumnName(nameof(CargoData.PORef)).HasMaxLength(CargoDataConsts.PORefMaxLength);
                b.Property(x => x.InvoiceNo).HasColumnName(nameof(CargoData.InvoiceNo)).HasMaxLength(CargoDataConsts.InvoiceNoMaxLength);
                b.Property(x => x.SRNo).HasColumnName(nameof(CargoData.SRNo)).HasMaxLength(CargoDataConsts.SRNoMaxLength);
                b.Property(x => x.Classification).HasColumnName(nameof(CargoData.Classification)).HasMaxLength(CargoDataConsts.ClassificationMaxLength);
                b.Property(x => x.Product).HasColumnName(nameof(CargoData.Product)).HasMaxLength(CargoDataConsts.ProductMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(CargoData.MaterialType)).HasMaxLength(CargoDataConsts.MaterialTypeMaxLength);
                b.Property(x => x.Spec1).HasColumnName(nameof(CargoData.Spec1)).HasMaxLength(CargoDataConsts.Spec1MaxLength);
                b.Property(x => x.Spec2).HasColumnName(nameof(CargoData.Spec2)).HasMaxLength(CargoDataConsts.Spec2MaxLength);
                b.Property(x => x.Spec3).HasColumnName(nameof(CargoData.Spec3)).HasMaxLength(CargoDataConsts.Spec3MaxLength);
                b.Property(x => x.OrderQty).HasColumnName(nameof(CargoData.OrderQty)).HasMaxLength(CargoDataConsts.OrderQtyMaxLength);
                b.Property(x => x.ExWorkQty).HasColumnName(nameof(CargoData.ExWorkQty)).HasMaxLength(CargoDataConsts.ExWorkQtyMaxLength);
                b.Property(x => x.NonExWorkQty).HasColumnName(nameof(CargoData.NonExWorkQty)).HasMaxLength(CargoDataConsts.NonExWorkQtyMaxLength);
                b.Property(x => x.InSTCH).HasColumnName(nameof(CargoData.InSTCH)).HasMaxLength(CargoDataConsts.InSTCHMaxLength);
                b.Property(x => x.Shipped).HasColumnName(nameof(CargoData.Shipped)).HasMaxLength(CargoDataConsts.ShippedMaxLength);
                b.Property(x => x.WaitForShip).HasColumnName(nameof(CargoData.WaitForShip)).HasMaxLength(CargoDataConsts.WaitForShipMaxLength);
                b.Property(x => x.ShipDate).HasColumnName(nameof(CargoData.ShipDate));
                b.Property(x => x.OrderDate).HasColumnName(nameof(CargoData.OrderDate));
                b.Property(x => x.InSTCHDate).HasColumnName(nameof(CargoData.InSTCHDate));
                b.Property(x => x.ShipmentMethod).HasColumnName(nameof(CargoData.ShipmentMethod)).HasMaxLength(CargoDataConsts.ShipmentMethodMaxLength);
                b.Property(x => x.ETA1).HasColumnName(nameof(CargoData.ETA1));
                b.Property(x => x.ETA2).HasColumnName(nameof(CargoData.ETA2));
                b.Property(x => x.MEVNRequest).HasColumnName(nameof(CargoData.MEVNRequest));
                b.Property(x => x.STCReply).HasColumnName(nameof(CargoData.STCReply));
                b.Property(x => x.EU).HasColumnName(nameof(CargoData.EU)).HasMaxLength(CargoDataConsts.EUMaxLength);
                b.Property(x => x.MEVNAddedRequest).HasColumnName(nameof(CargoData.MEVNAddedRequest)).HasMaxLength(CargoDataConsts.MEVNAddedRequestMaxLength);
                b.Property(x => x.NPD).HasColumnName(nameof(CargoData.NPD)).HasMaxLength(CargoDataConsts.NPDMaxLength);
                b.Property(x => x.PlannedShipment).HasColumnName(nameof(CargoData.PlannedShipment)).HasMaxLength(CargoDataConsts.PlannedShipmentMaxLength);
                b.Property(x => x.SODate).HasColumnName(nameof(CargoData.SODate)).HasMaxLength(CargoDataConsts.SODateMaxLength);
                b.Property(x => x.CellMarker).HasColumnName(nameof(CargoData.CellMarker)).HasMaxLength(CargoDataConsts.CellMarkerMaxLength);
                b.Property(x => x.ShipmentForm).HasColumnName(nameof(CargoData.ShipmentForm)).HasMaxLength(CargoDataConsts.ShipmentFormMaxLength);
                b.Property(x => x.MachineNumber).HasColumnName(nameof(CargoData.MachineNumber)).HasMaxLength(CargoDataConsts.MachineNumberMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(CargoData.FileName)).HasMaxLength(CargoDataConsts.FileNameMaxLength);
                b.Property(x => x.SupplierCode).HasColumnName(nameof(CargoData.SupplierCode)).HasMaxLength(CargoDataConsts.SupplierCodeMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(CargoData.Note)).HasMaxLength(CargoDataConsts.NoteMaxLength);

            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PSIDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "PSI_Detail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.PSIId).HasColumnName("PSI_Id").IsRequired();
                b.Property(x => x.MaterialGroup).HasColumnName(nameof(PSIDetail.MaterialGroup)).HasMaxLength(PSIDetailConsts.MaterialGroupMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(PSIDetail.Description)).HasMaxLength(PSIDetailConsts.DescriptionMaxLength);
                b.Property(x => x.FY).HasColumnName(nameof(PSIDetail.FY));
                b.Property(x => x.Month).HasColumnName(nameof(PSIDetail.Month));
                b.Property(x => x.Plan).HasColumnName(nameof(PSIDetail.Plan));
                b.Property(x => x.Note).HasColumnName(nameof(PSIDetail.Note)).HasMaxLength(PSIDetailConsts.NoteMaxLength);
                b.Property(x => x.ImportGuid).HasColumnName(nameof(PSIDetail.ImportGuid));
            });
        }

        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialHistory>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialHistory", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.MaterialId).HasColumnName(nameof(MaterialHistory.MaterialId)).IsRequired();
                b.Property(x => x.Action).HasColumnName(nameof(MaterialHistory.Action)).HasMaxLength(MaterialHistoryConsts.ActionMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(MaterialHistory.Note)).HasMaxLength(MaterialHistoryConsts.NoteMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialStock>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialStock", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.MaterialId).HasColumnName(nameof(MaterialStock.MaterialId)).IsRequired();
                b.Property(x => x.StockCategoryId).HasColumnName(nameof(MaterialStock.StockCategoryId)).IsRequired();
                b.Property(x => x.GolfaCode).HasColumnName(nameof(MaterialStock.GolfaCode)).IsRequired().HasMaxLength(MaterialStockConsts.GolfaCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(MaterialStock.Model)).IsRequired().HasMaxLength(MaterialStockConsts.ModelMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(MaterialStock.Qty));
                b.Property(x => x.Locked).HasColumnName(nameof(MaterialStock.Locked));
                b.Property(x => x.LockStockKeeping).HasColumnName(nameof(MaterialStock.LockStockKeeping));
                b.Property(x => x.LockStockSO).HasColumnName(nameof(MaterialStock.LockStockSO));
                b.Property(x => x.Available_Qty).HasColumnName(nameof(MaterialStock.Available_Qty));
                b.Property(x => x.Note).HasColumnName(nameof(MaterialStock.Note)).HasMaxLength(MaterialStockConsts.NoteMaxLength);

                b.HasOne(x => x.StockCategory).WithMany().HasForeignKey(y => y.StockCategoryId);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<StockCategory>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "StockCategory", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.StockCode).HasColumnName(nameof(StockCategory.StockCode)).IsRequired().HasMaxLength(StockCategoryConsts.StockCodeMaxLength);
                b.Property(x => x.StockName).HasColumnName(nameof(StockCategory.StockName)).IsRequired().HasMaxLength(StockCategoryConsts.StockNameMaxLength);
                b.Property(x => x.SAPCode).HasColumnName(nameof(StockCategory.SAPCode)).HasMaxLength(StockCategoryConsts.SAPCodeMaxLength);
                b.Property(x => x.MainStock).HasColumnName(nameof(StockCategory.MainStock));
                b.Property(x => x.FOC).HasColumnName(nameof(StockCategory.FOC));
                b.Property(x => x.DamagedStock).HasColumnName(nameof(StockCategory.DamagedStock));
                b.Property(x => x.SortOrder).HasColumnName(nameof(StockCategory.SortOrder));
                b.Property(x => x.IsDeactive).HasColumnName(nameof(StockCategory.IsDeactive));
                b.Property(x => x.Note).HasColumnName(nameof(StockCategory.Note)).HasMaxLength(StockCategoryConsts.NoteMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Buyer>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Buyer", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.BuyerTypeId).HasColumnName(nameof(Buyer.BuyerTypeId)).IsRequired();
                b.Property(x => x.BuyerTypeCode).HasColumnName(nameof(Buyer.BuyerTypeCode)).IsRequired().HasMaxLength(BuyerConsts.BuyerCodeMaxLength);
                b.Property(x => x.BuyerCode).HasColumnName(nameof(Buyer.BuyerCode)).IsRequired().HasMaxLength(BuyerConsts.BuyerCodeMaxLength);
                b.Property(x => x.ShortName).HasColumnName(nameof(Buyer.ShortName)).HasMaxLength(BuyerConsts.ShortNameMaxLength);
                b.Property(x => x.FullName).HasColumnName(nameof(Buyer.FullName)).HasMaxLength(BuyerConsts.FullNameMaxLength);
                b.Property(x => x.TaxCode).HasColumnName(nameof(Buyer.TaxCode)).HasMaxLength(BuyerConsts.TaxCodeMaxLength);
                b.Property(x => x.Address).HasColumnName(nameof(Buyer.Address));
                b.Property(x => x.ContactPerson).HasColumnName(nameof(Buyer.ContactPerson)).HasMaxLength(BuyerConsts.ContactPersonMaxLength);
                b.Property(x => x.ContactEmail).HasColumnName(nameof(Buyer.ContactEmail)).HasMaxLength(BuyerConsts.ContactEmailMaxLength);
                b.Property(x => x.ContactPhoneNumber).HasColumnName(nameof(Buyer.ContactPhoneNumber)).HasMaxLength(BuyerConsts.ContactPhoneNumberMaxLength);
                b.Property(x => x.PaymentTermCode).HasColumnName(nameof(Buyer.PaymentTermCode)).HasMaxLength(BuyerConsts.PaymentTermCodeMaxLength);
                b.Property(x => x.PaymentTermDescription).HasColumnName(nameof(Buyer.PaymentTermDescription)).HasMaxLength(BuyerConsts.PaymentTermDescriptionMaxLength);
                b.Property(x => x.Deactive).HasColumnName(nameof(Buyer.Deactive));
                b.Property(x => x.Note).HasColumnName(nameof(Buyer.Note)).HasMaxLength(BuyerConsts.NoteMaxLength);
                b.Property(x => x.CreditLimit).HasColumnName(nameof(Buyer.CreditLimit)).HasPrecision(18, 2);
                b.Property(x => x.CreditExposure).HasColumnName(nameof(Buyer.CreditExposure)).HasPrecision(18, 2);
                b.Property(x => x.AvailableCredit).HasColumnName(nameof(Buyer.AvailableCredit)).HasPrecision(18, 2);
                b.Property(x => x.AppliedPrice).HasColumnName(nameof(Buyer.AppliedPrice));

                b.HasOne(x => x.BuyerType).WithMany().HasForeignKey(x => x.BuyerTypeId).OnDelete(DeleteBehavior.Restrict);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<ApprovalHistory>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "ApprovalHistories", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.EntityType).HasColumnName(nameof(ApprovalHistory.EntityType)).HasMaxLength(ApprovalHistoryConsts.EntityTypeMaxLength);
                b.Property(x => x.ApproverRoleCode).HasColumnName(nameof(ApprovalHistory.ApproverRoleCode)).HasMaxLength(ApprovalHistoryConsts.ApproverRoleCodeMaxLength);
                b.Property(x => x.ApproverRoleName).HasColumnName(nameof(ApprovalHistory.ApproverRoleName)).HasMaxLength(ApprovalHistoryConsts.ApproverRoleNameMaxLength);
                b.Property(x => x.ApproverUsername).HasColumnName(nameof(ApprovalHistory.ApproverUsername)).HasMaxLength(ApprovalHistoryConsts.ApproverUsernameMaxLength);
                b.Property(x => x.ApproverFullName).HasColumnName(nameof(ApprovalHistory.ApproverFullName)).HasMaxLength(ApprovalHistoryConsts.ApproverFullNameMaxLength);
                b.Property(x => x.Action).HasColumnName(nameof(ApprovalHistory.Action)).HasMaxLength(ApprovalHistoryConsts.ApproverRoleCodeMaxLength).IsRequired();
                b.Property(x => x.ActionDate).HasColumnName(nameof(ApprovalHistory.ActionDate)).IsRequired();
                b.Property(x => x.Note).HasColumnName(nameof(ApprovalHistory.Note)).HasMaxLength(ApprovalHistoryConsts.NoteMaxLength);
                b.Property(x => x.IsLastApprovalInCurrentWorkflow).HasColumnName(nameof(ApprovalHistory.IsLastApprovalInCurrentWorkflow)).IsRequired();

                b.HasDiscriminator(x => x.EntityType)
                    .HasValue<ApprovalHistory>(EntityTypes.ApprovalHistory)
                    .HasValue<MaterialApprovalRequestHistory>(EntityTypes.MaterialApprovalRequest)
                    .HasValue<PriceOfferApprovalHistory>(EntityTypes.PriceOffer)
                    .HasValue<PriceOfferDetailApprovalHistory>(EntityTypes.PriceOfferDetail)
                    .HasValue<KeyAccountApprovalHistory>(EntityTypes.KeyAccount)
                    .HasValue<DPOApprovalHistory>(EntityTypes.DPO)
                    .HasValue<GKRApprovalHistory>(EntityTypes.GKR)
                    .HasValue<GICApprovalHistory>(EntityTypes.GIC)
                    .HasValue<SOHistory>(EntityTypes.SO)
                    .HasValue<GKRDetailApprovalHistory>(EntityTypes.GKRDetail)
                    .HasValue<GICDetailApprovalHistory>(EntityTypes.GICDetail)
                    .HasValue<DPODetailApprovalHistory>(EntityTypes.DPODetail)
                    .HasValue<PSIApprovalHistory>(EntityTypes.PSI)
                    .HasValue<AssetRequestApprovalHistory>(EntityTypes.AssetRequest);

                builder.Entity<MaterialApprovalRequestHistory>(b =>
                {
                    b.HasBaseType<ApprovalHistory>();
                    b.Property(x => x.MaterialApprovalRequestId).HasColumnName(nameof(MaterialApprovalRequestHistory.MaterialApprovalRequestId)).IsRequired();
                });
                builder.Entity<PriceOfferApprovalHistory>(b =>
                {
                    b.HasBaseType<ApprovalHistory>();
                    b.Property(x => x.PriceOfferId).HasColumnName(nameof(PriceOfferApprovalHistory.PriceOfferId)).IsRequired();
                    b.Property(x => x.ImportGuid).HasColumnName(nameof(PriceOfferApprovalHistory.ImportGuid));
                });
                builder.Entity<PriceOfferDetailApprovalHistory>(b =>
                {
                    b.HasBaseType<ApprovalHistory>();
                    b.Property(x => x.PriceOfferDetailId).HasColumnName(nameof(PriceOfferDetailApprovalHistory.PriceOfferDetailId)).IsRequired();
                    b.Property(x => x.ImportGuid).HasColumnName(nameof(PriceOfferDetailApprovalHistory.ImportGuid));
                });
                builder.Entity<KeyAccountApprovalHistory>(b =>
                {
                    b.HasBaseType<ApprovalHistory>();
                    b.Property(x => x.KeyAccountId).HasColumnName(nameof(KeyAccountApprovalHistory.KeyAccountId)).IsRequired();
                });
                builder.Entity<DPOApprovalHistory>(b =>
                {
                    b.HasBaseType<ApprovalHistory>();
                    b.Property(x => x.DPOId).HasColumnName(nameof(DPOApprovalHistory.DPOId)).IsRequired();
                });
                builder.Entity<DPODetailApprovalHistory>(b =>
                {
                    b.HasBaseType<ApprovalHistory>();
                    b.Property(x => x.DPODetailId).HasColumnName(nameof(DPODetailApprovalHistory.DPODetailId)).IsRequired();
                });
                builder.Entity<PSIApprovalHistory>(b =>
                {
                    b.HasBaseType<ApprovalHistory>();
                    b.Property(x => x.PSI_Id).HasColumnName(nameof(PSIApprovalHistory.PSI_Id)).IsRequired();
                });
                builder.Entity<GICApprovalHistory>(b =>
                {
                    b.HasBaseType<DPOApprovalHistory>();
                });
                builder.Entity<GKRApprovalHistory>(b =>
                {
                    b.HasBaseType<DPOApprovalHistory>();
                });
                builder.Entity<GKRDetailApprovalHistory>(b =>
                {
                    b.HasBaseType<DPODetailApprovalHistory>();
                });
                builder.Entity<GICDetailApprovalHistory>(b =>
                {
                    b.HasBaseType<DPODetailApprovalHistory>();
                });
                builder.Entity<SOHistory>(b =>
                {
                    b.HasBaseType<ApprovalHistory>();
                    b.Property(x => x.SOId).HasColumnName(nameof(SOHistory.SOId)).IsRequired();
                });
                builder.Entity<AssetRequestApprovalHistory>(b =>
                {
                    b.HasBaseType<ApprovalHistory>();
                    b.Property(x => x.AssetRequestId).HasColumnName(nameof(AssetRequestApprovalHistory.AssetRequestId)).IsRequired();
                });
            });

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<AddMoreItemHistory>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "AddMoreItemHistories", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                // Configure properties
                b.Property(x => x.MaterialCode).HasColumnName(nameof(AddMoreItemHistory.MaterialCode));
                b.Property(x => x.Model).HasColumnName(nameof(AddMoreItemHistory.Model));
                b.Property(x => x.Spec1).HasColumnName(nameof(AddMoreItemHistory.Spec1));
                b.Property(x => x.Spec2).HasColumnName(nameof(AddMoreItemHistory.Spec2));
                b.Property(x => x.Qty).HasColumnName(nameof(AddMoreItemHistory.Qty));
                b.Property(x => x.StandardPriceToDist).HasColumnName(nameof(AddMoreItemHistory.StandardPriceToDist)).HasPrecision(18, 2);
                b.Property(x => x.StandardAmount).HasColumnName(nameof(AddMoreItemHistory.StandardAmount)).HasPrecision(18, 2);
                b.Property(x => x.DistRequestedPrice).HasColumnName(nameof(AddMoreItemHistory.DistRequestedPrice)).HasPrecision(18, 2);
                b.Property(x => x.RequestedAmount).HasColumnName(nameof(AddMoreItemHistory.RequestedAmount)).HasPrecision(18, 2);
                b.Property(x => x.RequestedDiscount).HasColumnName(nameof(AddMoreItemHistory.RequestedDiscount)).HasPrecision(18, 2);
                b.Property(x => x.PriceToCustomer).HasColumnName(nameof(AddMoreItemHistory.PriceToCustomer)).HasPrecision(18, 2);
                b.Property(x => x.PriceOffer).HasColumnName(nameof(AddMoreItemHistory.PriceOffer)).HasPrecision(18, 2);
                b.Property(x => x.CometiorBrand).HasColumnName(nameof(AddMoreItemHistory.CometiorBrand));
                b.Property(x => x.CompetiorModel).HasColumnName(nameof(AddMoreItemHistory.CompetiorModel));
                b.Property(x => x.CompetiorPrice).HasColumnName(nameof(AddMoreItemHistory.CompetiorPrice)).HasPrecision(18, 2);
                b.Property(x => x.ImportGuid).HasColumnName(nameof(AddMoreItemHistory.ImportGuid));
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<ApprovalRoute>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "ApprovalRoute", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.EntityType).HasColumnName(nameof(ApprovalRoute.EntityType)).HasMaxLength(ApprovalRouteConsts.EntityTypeMaxLength);
                b.Property(x => x.InstanceId).HasColumnName(nameof(ApprovalRoute.InstanceId));
                b.Property(x => x.StepSequence).HasColumnName(nameof(ApprovalRoute.StepSequence)).IsRequired();
                b.Property(x => x.Approver).HasColumnName(nameof(ApprovalRoute.Approver)).HasMaxLength(ApprovalRouteConsts.ApproverMaxLength);
                b.Property(x => x.ApproverRoleCode).HasColumnName(nameof(ApprovalRoute.ApproverRoleCode)).IsRequired().HasMaxLength(ApprovalRouteConsts.ApproverRoleCodeMaxLength);
                b.Property(x => x.ApproverRoleName).HasColumnName(nameof(ApprovalRoute.ApproverRoleName)).IsRequired().HasMaxLength(ApprovalRouteConsts.ApproverRoleNameMaxLength);
                b.Property(x => x.ApprovalDate).HasColumnName(nameof(ApprovalRoute.ApprovalDate));
                b.Property(x => x.Notes).HasColumnName(nameof(ApprovalRoute.Notes)).HasMaxLength(ApprovalRouteConsts.NotesMaxLength);
                b.Property(x => x.IsApproved).HasColumnName(nameof(ApprovalRoute.IsApproved)).IsRequired();

                b.HasDiscriminator(x => x.EntityType)
                     .HasValue<ApprovalRoute>(EntityTypes.ApprovalRoute)
                     .HasValue<MaterialApprovalRequestRoute>(EntityTypes.MaterialApprovalRequest)
                     .HasValue<PriceOfferApprovalRoute>(EntityTypes.PriceOffer)
                     .HasValue<KeyAccountApprovalRoute>(EntityTypes.KeyAccount)
                     .HasValue<PSIApprovalRoute>(EntityTypes.PSI)
                     .HasValue<GKRApprovalRoute>(EntityTypes.GKR)
                     .HasValue<AssetRequestApprovalRoute>(EntityTypes.AssetRequest);

                builder.Entity<MaterialApprovalRequestRoute>(b =>
                {
                    b.HasBaseType<ApprovalRoute>();
                    b.Property(x => x.MaterialApprovalRequestId).HasColumnName(nameof(MaterialApprovalRequestRoute.MaterialApprovalRequestId)).IsRequired();
                });

                builder.Entity<PriceOfferApprovalRoute>(b =>
                {
                    b.HasBaseType<ApprovalRoute>();
                    b.Property(x => x.PriceOfferId).HasColumnName(nameof(PriceOfferApprovalRoute.PriceOfferId)).IsRequired();
                });

                builder.Entity<PriceOfferApprovalRoute>(b =>
                {
                    b.HasBaseType<ApprovalRoute>();
                    b.Property(x => x.PriceOfferId).HasColumnName(nameof(PriceOfferApprovalRoute.PriceOfferId)).IsRequired();
                });
                builder.Entity<KeyAccountApprovalRoute>(b =>
                {
                    b.HasBaseType<ApprovalRoute>();
                    b.Property(x => x.KeyAccountId).HasColumnName(nameof(KeyAccountApprovalHistory.KeyAccountId)).IsRequired();
                });
                builder.Entity<PSIApprovalRoute>(b =>
                {
                    b.HasBaseType<ApprovalRoute>();
                    b.Property(x => x.PSI_Id).HasColumnName(nameof(PSIApprovalRoute.PSI_Id)).IsRequired();
                });
                builder.Entity<GKRApprovalRoute>(b =>
                {
                    b.HasBaseType<ApprovalRoute>();
                    b.Property(x => x.GkrId).HasColumnName(nameof(GKRApprovalRoute.GkrId)).IsRequired();
                });
                builder.Entity<AssetRequestApprovalRoute>(b =>
                {
                    b.HasBaseType<ApprovalRoute>();
                    b.Property(x => x.AssetRequestId).HasColumnName(nameof(AssetRequestApprovalRoute.AssetRequestId)).IsRequired();
                });
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Attachment>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Attachments", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.RequestPart).HasColumnName(nameof(Attachment.RequestPart)).HasMaxLength(AttachmentConsts.RequestPartMaxLength);
                b.Property(x => x.AttachCode).HasColumnName(nameof(Attachment.AttachCode)).HasMaxLength(AttachmentConsts.AttachCodeMaxLength);
                b.Property(x => x.AttachName).HasColumnName(nameof(Attachment.AttachName)).HasMaxLength(AttachmentConsts.AttachNameMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(Attachment.FileName)).IsRequired().HasMaxLength(AttachmentConsts.FileNameMaxLength);
                b.Property(x => x.FileNameDB).HasColumnName(nameof(Attachment.FileNameDB)).IsRequired().HasMaxLength(AttachmentConsts.FileNameDBMaxLength);
                b.Property(x => x.FilePath).HasColumnName(nameof(Attachment.FilePath)).HasMaxLength(AttachmentConsts.FilePathMaxLength);
                b.Property(x => x.OfflineAttachment).HasColumnName(nameof(Attachment.OfflineAttachment)).IsRequired();
                b.Property(x => x.Description).HasColumnName(nameof(Attachment.Description)).HasMaxLength(AttachmentConsts.DescriptionMaxLength);

                b.HasDiscriminator(x => x.AttachName)
                    .HasValue<KeyAccountAttachment>(EntityTypes.KeyAccount)
                    .HasValue<PriceOfferAttachment>(EntityTypes.PriceOffer);

                builder.Entity<KeyAccountAttachment>(b =>
                {
                    b.HasBaseType<Attachment>();
                    b.Property(x => x.KeyAccountId).HasColumnName(nameof(KeyAccountAttachment.KeyAccountId)).IsRequired();
                });
                builder.Entity<PriceOfferAttachment>(b =>
                {
                    b.HasBaseType<Attachment>();
                    b.Property(x => x.PriceOfferId).HasColumnName(nameof(PriceOfferAttachment.PriceOfferId)).IsRequired();
                });
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Material>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Materials", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.GolfaCode).HasColumnName(nameof(Material.GolfaCode)).IsRequired().HasMaxLength(MaterialConsts.GolfaCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(Material.Model)).IsRequired().HasMaxLength(MaterialConsts.ModelMaxLength);
                b.Property(x => x.ValidFrom).HasColumnName(nameof(Material.ValidFrom));
                b.Property(x => x.ValidTo).HasColumnName(nameof(Material.ValidTo));
                b.Property(x => x.SAP_Code).HasColumnName(nameof(Material.SAP_Code)).HasMaxLength(MaterialConsts.SAP_CodeMaxLength);
                b.Property(x => x.Spec1).HasColumnName(nameof(Material.Spec1)).HasMaxLength(MaterialConsts.Spec1MaxLength);
                b.Property(x => x.Spec2).HasColumnName(nameof(Material.Spec2)).HasMaxLength(MaterialConsts.Spec2MaxLength);
                b.Property(x => x.Spec3).HasColumnName(nameof(Material.Spec3)).HasMaxLength(MaterialConsts.Spec3MaxLength);
                b.Property(x => x.Spec4).HasColumnName(nameof(Material.Spec4)).HasMaxLength(MaterialConsts.Spec4MaxLength);
                b.Property(x => x.Description_EN).HasColumnName(nameof(Material.Description_EN)).HasMaxLength(MaterialConsts.Description_ENMaxLength);
                b.Property(x => x.Description_VN).HasColumnName(nameof(Material.Description_VN)).HasMaxLength(MaterialConsts.Description_VNMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(Material.MaterialType)).HasMaxLength(MaterialConsts.MaterialTypeMaxLength);
                b.Property(x => x.Unit).HasColumnName(nameof(Material.Unit)).HasMaxLength(MaterialConsts.UnitMaxLength);
                b.Property(x => x.Material_SEC_Classification).HasColumnName(nameof(Material.Material_SEC_Classification)).HasMaxLength(MaterialConsts.Material_SEC_ClassificationMaxLength);
                //b.Property(x => x.Material_Group).HasColumnName(nameof(Material.Material_Group));
                b.Property(x => x.Material_Group).HasColumnName(nameof(Material.Material_Group)).HasMaxLength(MaterialConsts.MaterialGroupMaxLength);
                b.Property(x => x.SAPMatGroup).HasColumnName(nameof(Material.SAPMatGroup)).HasMaxLength(MaterialConsts.SAPMatGroupMaxLength);
                b.Property(x => x.Product_Hierarchy).HasColumnName(nameof(Material.Product_Hierarchy)).HasMaxLength(MaterialConsts.Product_HierarchyMaxLength);
                b.Property(x => x.ProductHierarchyDescription).HasColumnName(nameof(Material.ProductHierarchyDescription)).HasMaxLength(MaterialConsts.ProductHierarchyDescriptionMaxLength);
                b.Property(x => x.CountryOfOrigin).HasColumnName(nameof(Material.CountryOfOrigin)).HasMaxLength(MaterialConsts.CountryOfOriginMaxLength);
                b.Property(x => x.ReferenceLeadTime).HasColumnName(nameof(Material.ReferenceLeadTime));
                b.Property(x => x.WarrantyTime).HasColumnName(nameof(Material.WarrantyTime));
                b.Property(x => x.InventoryCategory).HasColumnName(nameof(Material.InventoryCategory)).HasMaxLength(MaterialConsts.InventoryCategoryMaxLength);
                b.Property(x => x.Maxlot).HasColumnName(nameof(Material.Maxlot));
                b.Property(x => x.StockWarning).HasColumnName(nameof(Material.StockWarning));
                b.Property(x => x.VAT).HasColumnName(nameof(Material.VAT)).HasPrecision(18, 2);
                b.Property(x => x.HS_Code).HasColumnName(nameof(Material.HS_Code)).HasMaxLength(MaterialConsts.HS_CodeMaxLength);
                b.Property(x => x.SupplierBUId).HasColumnName(nameof(Material.SupplierBUId));
                b.Property(x => x.SupplierBUCode).HasColumnName(nameof(Material.SupplierBUCode));
                b.Property(x => x.Factory_Text).HasColumnName(nameof(Material.Factory_Text));
                b.Property(x => x.Input_Price).HasColumnName(nameof(Material.Input_Price)).HasPrecision(18, 2);
                b.Property(x => x.InputCurrency).HasColumnName(nameof(Material.InputCurrency)).HasMaxLength(MaterialConsts.InputCurrencyMaxLength);
                b.Property(x => x.INCOTERMS).HasColumnName(nameof(Material.INCOTERMS)).HasMaxLength(MaterialConsts.INCOTERMSMaxLength);
                b.Property(x => x.EPA).HasColumnName(nameof(Material.EPA)).IsRequired();
                b.Property(x => x.ImportDuty).HasColumnName(nameof(Material.ImportDuty)).HasPrecision(18, 2);
                b.Property(x => x.AppliedExchangeRate).HasColumnName(nameof(Material.AppliedExchangeRate)).HasPrecision(18, 2);
                b.Property(x => x.LandedCost).HasColumnName(nameof(Material.LandedCost)).HasPrecision(18, 2);
                b.Property(x => x.MaxSalesOfferPrice).HasColumnName(nameof(Material.MaxSalesOfferPrice)).HasPrecision(18, 2);
                b.Property(x => x.MaxMangerOfferPrice).HasColumnName(nameof(Material.MaxMangerOfferPrice)).HasPrecision(18, 2);
                b.Property(x => x.Standard_Price).HasColumnName(nameof(Material.Standard_Price)).IsRequired().HasPrecision(18, 2);
                b.Property(x => x.SellingPrice1).HasColumnName(nameof(Material.SellingPrice1)).HasPrecision(18, 2);
                b.Property(x => x.SellingPrice2).HasColumnName(nameof(Material.SellingPrice2)).HasPrecision(18, 2);
                b.Property(x => x.SellingPrice3).HasColumnName(nameof(Material.SellingPrice3)).HasPrecision(18, 2);
                b.Property(x => x.SellingPrice4).HasColumnName(nameof(Material.SellingPrice4)).HasPrecision(18, 2);
                b.Property(x => x.SellingPrice5).HasColumnName(nameof(Material.SellingPrice5)).HasPrecision(18, 2);
                b.Property(x => x.MaterialStatus).HasColumnName(nameof(Material.MaterialStatus)).IsRequired().HasMaxLength(MaterialConsts.MaterialStatusMaxLength);
                //b.Property(x => x.DestinationDate).HasColumnName(nameof(Material.DestinationDate));
                b.Property(x => x.RegistrationDate).HasColumnName(nameof(Material.RegistrationDate));
                //b.Property(x => x.IndeactiveDate).HasColumnName(nameof(Material.IndeactiveDate));
                //b.Property(x => x.Description_Group).HasColumnName(nameof(Material.Description_Group)).HasMaxLength(MaterialConsts.Description_GroupMaxLength);
                //b.Property(x => x.Origin).HasColumnName(nameof(Material.Origin)).HasMaxLength(MaterialConsts.OriginMaxLength);
                //b.Property(x => x.Kind).HasColumnName(nameof(Material.Kind)).HasMaxLength(MaterialConsts.KindMaxLength);
                //b.Property(x => x.Factory).HasColumnName(nameof(Material.Factory));
                //b.Property(x => x.Vendor).HasColumnName(nameof(Material.Vendor));
                //b.Property(x => x.LeadTime).HasColumnName(nameof(Material.LeadTime));
                //b.Property(x => x.RefExchangeRate).HasColumnName(nameof(Material.RefExchangeRate)).HasPrecision(18, 2);
                b.Property(x => x.Note).HasColumnName(nameof(Material.Note)).HasMaxLength(MaterialConsts.NoteMaxLength);
                //b.Property(x => x.Source).HasColumnName(nameof(Material.Source)).HasMaxLength(MaterialConsts.SourceMaxLength);
                //b.Property(x => x.Reason).HasColumnName(nameof(Material.Reason)).HasMaxLength(MaterialConsts.ReasonMaxLength);
                //b.Property(x => x.FinalDPOAcceptanceDate).HasColumnName(nameof(Material.FinalDPOAcceptanceDate));
                b.Property(x => x.SupplierCode).HasColumnName(nameof(Material.SupplierCode)).HasMaxLength(MaterialConsts.SupplierCodeMaxLength);
                b.Property(x => x.MaterialClass).HasColumnName(nameof(Material.MaterialClass)).HasMaxLength(MaterialConsts.MaterialClassMaxLength);

                b.Property(x => x.CargoNote).HasColumnName(nameof(Material.CargoNote)).HasMaxLength(MaterialConsts.CargoNoteMaxLength);
                b.Property(x => x.Weight).HasColumnName(nameof(Material.Weight)).HasMaxLength(MaterialConsts.WeightMaxLength);
                b.Property(x => x.Size).HasColumnName(nameof(Material.Size)).HasMaxLength(MaterialConsts.SizeMaxLength);
                b.Property(x => x.QRCode).HasColumnName(nameof(Material.QRCode)).HasMaxLength(MaterialConsts.QRCodeMaxLength);

                b.HasMany(x => x.MaterialStock).WithOne(y => y.Material).HasForeignKey(y => y.MaterialId);

            });

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialApprovalRequest>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialApprovalRequest", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.ConfigureObjectHasApprovalRoute();

                b.Property(x => x.ImportType).HasColumnName(nameof(MaterialApprovalRequest.ImportType)).IsRequired().HasMaxLength(MaterialApprovalRequestConsts.ImportTypeMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(MaterialApprovalRequest.FileName)).HasMaxLength(MaterialApprovalRequestConsts.FileNameMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(MaterialApprovalRequest.Note)).HasMaxLength(MaterialApprovalRequestConsts.NoteMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(MaterialApprovalRequest.Status)).HasMaxLength(MaterialApprovalRequestConsts.StatusMaxLength);
                b.Property(x => x.RequestNo).HasColumnName(nameof(MaterialApprovalRequest.RequestNo)).IsRequired().HasMaxLength(MaterialApprovalRequestConsts.RequestNoMaxLength);
                b.HasMany(x => x.MaterialApprovalDetails).WithOne().HasForeignKey(y => y.MaterialApprovalId).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.MaterialHistories).WithOne().HasForeignKey(y => y.MaterialApprovalRequestId).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.MaterialRoutes).WithOne().HasForeignKey(y => y.MaterialApprovalRequestId).OnDelete(DeleteBehavior.Cascade);

            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialApprovalRequestDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialApprovalRequestDetail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.MaterialApprovalId).HasColumnName(nameof(MaterialApprovalRequestDetail.MaterialApprovalId)).IsRequired();
                b.Property(x => x.GolfaCode).HasColumnName(nameof(MaterialApprovalRequestDetail.GolfaCode)).IsRequired().HasMaxLength(MaterialApprovalRequestDetailConsts.GolfaCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(MaterialApprovalRequestDetail.Model)).IsRequired().HasMaxLength(MaterialApprovalRequestDetailConsts.ModelMaxLength);
                b.Property(x => x.ValidFrom).HasColumnName(nameof(MaterialApprovalRequestDetail.ValidFrom));
                b.Property(x => x.ValidTo).HasColumnName(nameof(MaterialApprovalRequestDetail.ValidTo));
                b.Property(x => x.SAP_Code).HasColumnName(nameof(MaterialApprovalRequestDetail.SAP_Code)).HasMaxLength(MaterialApprovalRequestDetailConsts.SAP_CodeMaxLength);
                b.Property(x => x.Spec1).HasColumnName(nameof(MaterialApprovalRequestDetail.Spec1)).HasMaxLength(MaterialApprovalRequestDetailConsts.Spec1MaxLength);
                b.Property(x => x.Spec2).HasColumnName(nameof(MaterialApprovalRequestDetail.Spec2)).HasMaxLength(MaterialApprovalRequestDetailConsts.Spec2MaxLength);
                b.Property(x => x.Spec3).HasColumnName(nameof(MaterialApprovalRequestDetail.Spec3)).HasMaxLength(MaterialApprovalRequestDetailConsts.Spec3MaxLength);
                b.Property(x => x.Spec4).HasColumnName(nameof(MaterialApprovalRequestDetail.Spec4)).HasMaxLength(MaterialApprovalRequestDetailConsts.Spec4MaxLength);
                b.Property(x => x.Description_EN).HasColumnName(nameof(MaterialApprovalRequestDetail.Description_EN)).HasMaxLength(MaterialApprovalRequestDetailConsts.Description_ENMaxLength);
                b.Property(x => x.Description_VN).HasColumnName(nameof(MaterialApprovalRequestDetail.Description_VN)).HasMaxLength(MaterialApprovalRequestDetailConsts.Description_VNMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(MaterialApprovalRequestDetail.MaterialType)).HasMaxLength(MaterialApprovalRequestDetailConsts.MaterialTypeMaxLength);
                b.Property(x => x.Unit).HasColumnName(nameof(MaterialApprovalRequestDetail.Unit)).HasMaxLength(MaterialApprovalRequestDetailConsts.UnitMaxLength);
                b.Property(x => x.Material_SEC_Classification).HasColumnName(nameof(MaterialApprovalRequestDetail.Material_SEC_Classification)).HasMaxLength(MaterialApprovalRequestDetailConsts.Material_SEC_ClassificationMaxLength);
                //b.Property(x => x.Material_Group).HasColumnName(nameof(MaterialApprovalRequestDetail.Material_Group));
                b.Property(x => x.Material_Group).HasColumnName(nameof(Material.Material_Group)).HasMaxLength(MaterialApprovalRequestDetailConsts.MaterialGroupMaxLength);
                b.Property(x => x.SAPMatGroup).HasColumnName(nameof(MaterialApprovalRequestDetail.SAPMatGroup)).HasMaxLength(MaterialApprovalRequestDetailConsts.SAPMatGroupMaxLength);
                b.Property(x => x.Product_Hierarchy).HasColumnName(nameof(MaterialApprovalRequestDetail.Product_Hierarchy)).HasMaxLength(MaterialApprovalRequestDetailConsts.Product_HierarchyMaxLength);
                b.Property(x => x.ProductHierarchyDescription).HasColumnName(nameof(MaterialApprovalRequestDetail.ProductHierarchyDescription)).HasMaxLength(MaterialApprovalRequestDetailConsts.ProductHierarchyDescriptionMaxLength);
                b.Property(x => x.CountryOfOrigin).HasColumnName(nameof(MaterialApprovalRequestDetail.CountryOfOrigin)).HasMaxLength(MaterialApprovalRequestDetailConsts.CountryOfOriginMaxLength);
                b.Property(x => x.ReferenceLeadTime).HasColumnName(nameof(MaterialApprovalRequestDetail.ReferenceLeadTime));
                b.Property(x => x.WarrantyTime).HasColumnName(nameof(MaterialApprovalRequestDetail.WarrantyTime));
                b.Property(x => x.InventoryCategory).HasColumnName(nameof(MaterialApprovalRequestDetail.InventoryCategory)).HasMaxLength(MaterialApprovalRequestDetailConsts.InventoryCategoryMaxLength);
                b.Property(x => x.Maxlot).HasColumnName(nameof(MaterialApprovalRequestDetail.Maxlot));
                b.Property(x => x.StockWarning).HasColumnName(nameof(MaterialApprovalRequestDetail.StockWarning));
                b.Property(x => x.VAT).HasColumnName(nameof(MaterialApprovalRequestDetail.VAT));
                b.Property(x => x.HS_Code).HasColumnName(nameof(MaterialApprovalRequestDetail.HS_Code)).HasMaxLength(MaterialApprovalRequestDetailConsts.HS_CodeMaxLength);
                b.Property(x => x.SupplierBUId).HasColumnName(nameof(MaterialApprovalRequestDetail.SupplierBUId));
                b.Property(x => x.SupplierBUCode).HasColumnName(nameof(MaterialApprovalRequestDetail.SupplierBUCode)).HasMaxLength(MaterialApprovalRequestDetailConsts.SupplierBUCodeMaxLength);
                b.Property(x => x.Factory_Text).HasColumnName(nameof(MaterialApprovalRequestDetail.Factory_Text)).HasMaxLength(MaterialApprovalRequestDetailConsts.Factory_TextMaxLength);
                b.Property(x => x.Input_Price).HasColumnName(nameof(MaterialApprovalRequestDetail.Input_Price));
                b.Property(x => x.InputCurrency).HasColumnName(nameof(MaterialApprovalRequestDetail.InputCurrency)).HasMaxLength(MaterialApprovalRequestDetailConsts.InputCurrencyMaxLength);
                b.Property(x => x.INCOTERMS).HasColumnName(nameof(MaterialApprovalRequestDetail.INCOTERMS)).HasMaxLength(MaterialApprovalRequestDetailConsts.INCOTERMSMaxLength);
                b.Property(x => x.EPA).HasColumnName(nameof(MaterialApprovalRequestDetail.EPA));
                b.Property(x => x.ImportDuty).HasColumnName(nameof(MaterialApprovalRequestDetail.ImportDuty));
                b.Property(x => x.AppliedExchangeRate).HasColumnName(nameof(MaterialApprovalRequestDetail.AppliedExchangeRate));
                b.Property(x => x.LandedCost).HasColumnName(nameof(MaterialApprovalRequestDetail.LandedCost));
                b.Property(x => x.MaxSalesOfferPrice).HasColumnName(nameof(MaterialApprovalRequestDetail.MaxSalesOfferPrice));
                b.Property(x => x.MaxMangerOfferPrice).HasColumnName(nameof(MaterialApprovalRequestDetail.MaxMangerOfferPrice));
                b.Property(x => x.Standard_Price).HasColumnName(nameof(MaterialApprovalRequestDetail.Standard_Price));
                b.Property(x => x.SellingPrice1).HasColumnName(nameof(MaterialApprovalRequestDetail.SellingPrice1));
                b.Property(x => x.SellingPrice2).HasColumnName(nameof(MaterialApprovalRequestDetail.SellingPrice2));
                b.Property(x => x.SellingPrice3).HasColumnName(nameof(MaterialApprovalRequestDetail.SellingPrice3));
                b.Property(x => x.SellingPrice4).HasColumnName(nameof(MaterialApprovalRequestDetail.SellingPrice4));
                b.Property(x => x.SellingPrice5).HasColumnName(nameof(MaterialApprovalRequestDetail.SellingPrice5));
                b.Property(x => x.MaterialStatus).HasColumnName(nameof(MaterialApprovalRequestDetail.MaterialStatus)).HasMaxLength(MaterialApprovalRequestDetailConsts.MaterialStatusMaxLength);
                //b.Property(x => x.DestinationDate).HasColumnName(nameof(MaterialApprovalRequestDetail.DestinationDate));
                b.Property(x => x.RegistrationDate).HasColumnName(nameof(MaterialApprovalRequestDetail.RegistrationDate));
                //b.Property(x => x.IndeactiveDate).HasColumnName(nameof(MaterialApprovalRequestDetail.IndeactiveDate));
                //b.Property(x => x.Description_Group).HasColumnName(nameof(MaterialApprovalRequestDetail.Description_Group)).HasMaxLength(MaterialApprovalRequestDetailConsts.Description_GroupMaxLength);
                //b.Property(x => x.Origin).HasColumnName(nameof(MaterialApprovalRequestDetail.Origin)).HasMaxLength(MaterialApprovalRequestDetailConsts.OriginMaxLength);
                //b.Property(x => x.Kind).HasColumnName(nameof(MaterialApprovalRequestDetail.Kind)).HasMaxLength(MaterialApprovalRequestDetailConsts.KindMaxLength);
                //b.Property(x => x.Factory).HasColumnName(nameof(MaterialApprovalRequestDetail.Factory));
                //b.Property(x => x.Vendor).HasColumnName(nameof(MaterialApprovalRequestDetail.Vendor));
                //b.Property(x => x.LeadTime).HasColumnName(nameof(MaterialApprovalRequestDetail.LeadTime));
                //b.Property(x => x.RefExchangeRate).HasColumnName(nameof(MaterialApprovalRequestDetail.RefExchangeRate));
                b.Property(x => x.Note).HasColumnName(nameof(MaterialApprovalRequestDetail.Note)).HasMaxLength(MaterialApprovalRequestDetailConsts.NoteMaxLength);
                b.Property(x => x.Source).HasColumnName(nameof(MaterialApprovalRequestDetail.Source)).HasMaxLength(MaterialApprovalRequestDetailConsts.SourceMaxLength);
                b.Property(x => x.Reason).HasColumnName(nameof(MaterialApprovalRequestDetail.Reason)).HasMaxLength(MaterialApprovalRequestDetailConsts.ReasonMaxLength);
                b.Property(x => x.FinalDPOAcceptanceDate).HasColumnName(nameof(MaterialApprovalRequestDetail.FinalDPOAcceptanceDate));
                b.Property(x => x.SupplierCode).HasColumnName(nameof(MaterialApprovalRequestDetail.SupplierCode)).HasMaxLength(MaterialApprovalRequestDetailConsts.SupplierCodeMaxLength);
                b.Property(x => x.MaterialClass).HasColumnName(nameof(Material.MaterialClass)).HasMaxLength(MaterialApprovalRequestDetailConsts.MaterialClassMaxLength);
                b.Property(x => x.FactoryRefDoc).HasColumnName(nameof(MaterialApprovalRequestDetail.FactoryRefDoc)).HasMaxLength(MaterialApprovalRequestDetailConsts.FactoryRefDocMaxLength);
                b.Property(x => x.Action).HasColumnName(nameof(MaterialApprovalRequestDetail.Action)).HasMaxLength(MaterialApprovalRequestDetailConsts.ActionMaxLength);
                b.Property(x => x.ActionDate).HasColumnName(nameof(MaterialApprovalRequestDetail.ActionDate));

                b.Property(x => x.CargoNote).HasColumnName(nameof(MaterialApprovalRequestDetail.CargoNote)).HasMaxLength(MaterialApprovalRequestDetailConsts.CargoNoteMaxLength);
                b.Property(x => x.Weight).HasColumnName(nameof(MaterialApprovalRequestDetail.Weight)).HasMaxLength(MaterialApprovalRequestDetailConsts.WeightMaxLength);
                b.Property(x => x.Size).HasColumnName(nameof(MaterialApprovalRequestDetail.Size)).HasMaxLength(MaterialApprovalRequestDetailConsts.SizeMaxLength);
                b.Property(x => x.QRCode).HasColumnName(nameof(MaterialApprovalRequestDetail.QRCode)).HasMaxLength(MaterialApprovalRequestDetailConsts.QRCodeMaxLength);

                //b.HasOne(x => x.MaterialGroupCategory).WithMany().HasForeignKey(y => y.Material_Group).OnDelete(DeleteBehavior.Restrict);
                //b.HasOne(x => x.InputCurrencyCategory).WithMany().HasForeignKey(y => y.InputCurrency).OnDelete(DeleteBehavior.Restrict);
            });

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Customer>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Customer", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.TaxCode).HasColumnName(nameof(Customer.TaxCode)).IsRequired().HasMaxLength(CustomerConsts.TaxCodeMaxLength);
                b.Property(x => x.CustomerName).HasColumnName(nameof(Customer.CustomerName)).HasMaxLength(CustomerConsts.CustomerNameMaxLength);
                b.Property(x => x.CustomerShortName).HasColumnName(nameof(Customer.CustomerShortName)).HasMaxLength(CustomerConsts.CustomerShortNameMaxLength);
                b.Property(x => x.CustomerType).HasColumnName(nameof(Customer.CustomerType)).HasMaxLength(CustomerConsts.CustomerTypeMaxLength);
                b.Property(x => x.Address).HasColumnName(nameof(Customer.Address)).HasMaxLength(CustomerConsts.AddressMaxLength);
                b.Property(x => x.Website).HasColumnName(nameof(Customer.Website)).HasMaxLength(CustomerConsts.WebsiteMaxLength);
                b.Property(x => x.Phone).HasColumnName(nameof(Customer.Phone)).HasMaxLength(CustomerConsts.PhoneMaxLength);
                b.Property(x => x.Country).HasColumnName(nameof(Customer.Country));
                b.Property(x => x.Note).HasColumnName(nameof(Customer.Note));
                b.Property(x => x.Province).HasColumnName(nameof(Customer.Province)).HasMaxLength(CustomerConsts.ProvinceMaxLength);
                b.Property(x => x.CustomerIndustry).HasColumnName(nameof(Customer.CustomerIndustry)).HasMaxLength(CustomerConsts.CustomerIndustryMaxLength);
                b.Property(x => x.IsDeactive).HasColumnName(nameof(Customer.IsDeactive));
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<DPO>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "DPO", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.ConfigureObjectHasApprovalRoute();

                b.Property(x => x.DPONo).HasColumnName(nameof(DPO.DPONo)).HasMaxLength(DPOConsts.DPONoMaxLength);
                b.Property(x => x.DPOType).HasColumnName(nameof(DPO.DPOType)).HasMaxLength(DPOConsts.DPOTypeMaxLength);
                b.Property(x => x.GICType).HasColumnName(nameof(DPO.GICType)).HasMaxLength(DPOConsts.DPOTypeMaxLength); ;
                b.Property(x => x.MaterialType).HasColumnName(nameof(DPO.MaterialType)).HasMaxLength(DPOConsts.MaterialTypeMaxLength);
                b.Property(x => x.CostCenter).HasColumnName(nameof(DPO.CostCenter)).HasMaxLength(DPOConsts.CostCenterMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(DPO.Status)).HasMaxLength(DPOConsts.StatusMaxLength);
                b.Property(x => x.BuyerTypeId).HasColumnName(nameof(DPO.BuyerTypeId));
                b.Property(x => x.BuyerId).HasColumnName(nameof(DPO.BuyerId));
                b.Property(x => x.BuyerShortName).HasColumnName(nameof(DPO.BuyerShortName)).HasMaxLength(DPOConsts.BuyerShortNameMaxLength);
                b.Property(x => x.BuyerTypeDescription).HasColumnName(nameof(DPO.BuyerTypeDescription));
                b.Property(x => x.OrderDate).HasColumnName(nameof(DPO.OrderDate));
                b.Property(x => x.TotalAmount).HasColumnName(nameof(DPO.TotalAmount)).HasPrecision(18, 2);
                b.Property(x => x.TotalAmountIncludeExtraFee).HasColumnName(nameof(DPO.TotalAmountIncludeExtraFee)).HasPrecision(18, 2);
                b.Property(x => x.Remark).HasColumnName(nameof(DPO.Remark)).HasMaxLength(DPOConsts.RemarkMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(DPO.FileName)).HasMaxLength(DPOConsts.FileNameMaxLength);
                b.Property(x => x.GICProcess).HasColumnName(nameof(DPO.GICProcess)).HasMaxLength(DPOConsts.GICProcessMaxLength);
                b.Property(x => x.ReferenceDoc).HasColumnName(nameof(DPO.ReferenceDoc)).HasMaxLength(DPOConsts.ReferenceDocMaxLength);
                b.Property(x => x.ReferenceDocDate).HasColumnName(nameof(DPO.ReferenceDocDate));
                b.Property(x => x.LinkedDpoNo).HasColumnName("Gkr_LinkedDpoNo").HasMaxLength(DPOConsts.DPONoMaxLength);
                b.Property(x => x.LinkedDpoId).HasColumnName("Gkr_LinkedDpoId");
                b.Property(x => x.LinkedNote).HasColumnName("Gkr_LinkedNote").HasMaxLength(QuoteFlowSharedConsts.NoteMaxLength);
                b.Property(x => x.ExpirationDate).HasColumnName(nameof(DPO.ExpirationDate));
                b.Property(x => x.Reason).HasColumnName("Gkr_Reason").HasMaxLength(4000);
                b.Property(x => x.SalePicUsername).HasColumnName("Gkr_SalePicUsername").HasMaxLength(500);
                b.Property(x => x.SalePicFullName).HasColumnName("Gkr_SalePicFullName").HasMaxLength(500);
                b.Property(x => x.SalePicTeamId).HasColumnName("Gkr_SalePicTeamId");

                b.HasMany(x => x.ApprovalRoutes).WithOne().HasForeignKey(x => x.GkrId).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.Details).WithOne(y => y.DPO).HasForeignKey(y => y.DPOId).OnDelete(DeleteBehavior.Restrict);
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<DPODetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "DPODetail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.DPOId).HasColumnName(nameof(DPODetail.DPOId)).IsRequired();
                b.Property(x => x.Status).HasColumnName(nameof(DPODetail.Status)).HasMaxLength(DPODetailConsts.StatusMaxLength);
                b.Property(x => x.RowNo).HasColumnName(nameof(DPODetail.RowNo));
                b.Property(x => x.GolfaCode).HasColumnName(nameof(DPODetail.GolfaCode)).IsRequired().HasMaxLength(DPODetailConsts.GolfaCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(DPODetail.Model)).HasMaxLength(DPODetailConsts.ModelMaxLength);
                b.Property(x => x.Spec1).HasColumnName(nameof(DPODetail.Spec1)).HasMaxLength(DPODetailConsts.Spec1MaxLength);
                b.Property(x => x.Spec2).HasColumnName(nameof(DPODetail.Spec2)).HasMaxLength(DPODetailConsts.Spec2MaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(DPODetail.Qty));
                b.Property(x => x.UnitPrice).HasColumnName(nameof(DPODetail.UnitPrice)).HasPrecision(18, 2);
                b.Property(x => x.LandedCost).HasColumnName(nameof(DPODetail.LandedCost)).HasPrecision(18, 2);
                b.Property(x => x.Amount).HasColumnName(nameof(DPODetail.Amount)).HasPrecision(18, 2);
                b.Property(x => x.AmountIncludeExtraFee).HasColumnName(nameof(DPODetail.AmountIncludeExtraFee)).HasPrecision(18, 2);
                b.Property(x => x.RequestedETA).HasColumnName(nameof(DPODetail.RequestedETA));
                b.Property(x => x.SPOId).HasColumnName(nameof(DPODetail.SPOId));
                b.Property(x => x.SPOCode).HasColumnName(nameof(DPODetail.SPOCode)).HasMaxLength(DPODetailConsts.SPOCodeMaxLength);
                b.Property(x => x.CustomerId).HasColumnName(nameof(DPODetail.CustomerId));
                b.Property(x => x.CustomerTaxCode).HasColumnName(nameof(DPODetail.CustomerTaxCode)).HasMaxLength(DPODetailConsts.CustomerTaxCodeMaxLength);
                b.Property(x => x.CustomerName).HasColumnName(nameof(DPODetail.CustomerName)).HasMaxLength(DPODetailConsts.CustomerNameMaxLength);
                b.Property(x => x.CustomerType).HasColumnName(nameof(DPODetail.CustomerType)).HasMaxLength(DPODetailConsts.CustomerTypeMaxLength);
                b.Property(x => x.CustomerIndustry).HasColumnName(nameof(DPODetail.CustomerIndustry)).HasMaxLength(DPODetailConsts.CustomerIndustryMaxLength);
                b.Property(x => x.LockStock).HasColumnName(nameof(DPODetail.LockStock));
                b.Property(x => x.LockStockSO).HasColumnName(nameof(DPODetail.LockStockSO));
                b.Property(x => x.LockShipment).HasColumnName(nameof(DPODetail.LockShipment));
                b.Property(x => x.Delivered).HasColumnName(nameof(DPODetail.Delivered));
                b.Property(x => x.NeedDelivery).HasColumnName(nameof(DPODetail.NeedDelivery));
                b.Property(x => x.Note).HasColumnName(nameof(DPODetail.Note)).HasMaxLength(DPODetailConsts.NoteMaxLength);
                b.Property(x => x.ConfirmNoted).HasColumnName(nameof(DPODetail.ConfirmNoted)).HasMaxLength(DPODetailConsts.NoteMaxLength);
                b.Property(x => x.OrderReason).HasColumnName(nameof(DPODetail.OrderReason));
                b.Property(x => x.AccountNo).HasColumnName(nameof(DPODetail.AccountNo)).HasMaxLength(DPODetailConsts.AccountNoMaxLength);
                b.Property(x => x.Extrafee).HasColumnName(nameof(DPODetail.Extrafee)).HasPrecision(18, 2);
                b.Property(x => x.DamagedProduct).HasColumnName(nameof(DPODetail.DamagedProduct)).HasMaxLength(DPODetailConsts.DamagedProductMaxLength);
                b.Property(x => x.ProductSerialNo).HasColumnName(nameof(DPODetail.ProductSerialNo)).HasMaxLength(DPODetailConsts.ProductSerialNoMaxLength);
                b.Property(x => x.MEVNSellingInvoiceNo).HasColumnName(nameof(DPODetail.MEVNSellingInvoiceNo)).HasMaxLength(DPODetailConsts.MEVNSellingInvoiceNoMaxLength);
                b.Property(x => x.ExtrafeeUsedInSO).HasColumnName("Extrafee_Used_InSO").HasPrecision(18, 2);
                b.Property(x => x.ExtrafeeAvailable).HasColumnName("Extrafee_Available").HasPrecision(18, 2);
                b.Property(x => x.ExtrafeeNote).HasColumnName("Extrafee_Note").HasMaxLength(DPODetailConsts.NoteMaxLength);
                b.Property(x => x.DPOUsed).HasColumnName("Gkr_DpoUsed").HasPrecision(18, 2);
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<DpoGkrUsage>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "DpoGkrUsage", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.GkrId).HasColumnName(nameof(DpoGkrUsage.GkrId)).IsRequired();
                b.Property(x => x.DpoId).HasColumnName(nameof(DpoGkrUsage.DpoId)).IsRequired();
                b.Property(x => x.GkrNo).HasColumnName(nameof(DpoGkrUsage.GkrNo)).HasMaxLength(DPOConsts.DPONoMaxLength).IsRequired();
                b.Property(x => x.DpoNo).HasColumnName(nameof(DpoGkrUsage.DpoNo)).HasMaxLength(DPOConsts.DPONoMaxLength).IsRequired();
                b.Property(x => x.GkrDetailId).HasColumnName(nameof(DpoGkrUsage.GkrDetailId)).IsRequired();
                b.Property(x => x.DpoDetailId).HasColumnName(nameof(DpoGkrUsage.DpoDetailId)).IsRequired();
                b.Property(x => x.GolfaCode).HasColumnName(nameof(DpoGkrUsage.GolfaCode)).HasMaxLength(DPODetailConsts.GolfaCodeMaxLength).IsRequired();
                b.Property(x => x.Model).HasColumnName(nameof(DpoGkrUsage.Model)).HasMaxLength(DPODetailConsts.ModelMaxLength).IsRequired();
                b.Property(x => x.GkrQty).HasColumnName(nameof(DpoGkrUsage.GkrQty)).HasPrecision(18, 2).IsRequired();
                b.Property(x => x.DpoQty).HasColumnName(nameof(DpoGkrUsage.DpoQty)).HasPrecision(18, 2).IsRequired();
                b.Property(x => x.GkrLockStockQty).HasColumnName(nameof(DpoGkrUsage.GkrLockStockQty)).HasPrecision(18, 2).IsRequired();
                b.Property(x => x.DpoLockStockQty).HasColumnName(nameof(DpoGkrUsage.DpoLockStockQty)).HasPrecision(18, 2).IsRequired();
                b.Property(x => x.GkrLockShipmentQty).HasColumnName(nameof(DpoGkrUsage.GkrLockShipmentQty)).HasPrecision(18, 2).IsRequired();
                b.Property(x => x.DpoLockShipmentQty).HasColumnName(nameof(DpoGkrUsage.DpoLockShipmentQty)).HasPrecision(18, 2).IsRequired();

                b.HasIndex(x => x.GkrId).HasDatabaseName("IX_DpoGkrUsage_GkrId");
                b.HasIndex(x => x.DpoId).HasDatabaseName("IX_DpoGkrUsage_DpoId");
                b.HasIndex(x => x.GkrDetailId).HasDatabaseName("IX_DpoGkrUsage_GkrDetailId");
                b.HasIndex(x => x.DpoDetailId).HasDatabaseName("IX_DpoGkrUsage_DpoDetailId");
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialGroup>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialGroups", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.Code).HasColumnName(nameof(MaterialGroup.Code)).IsRequired().HasMaxLength(MaterialGroupConsts.CodeMaxLength);
                b.Property(x => x.Name).HasColumnName(nameof(MaterialGroup.Name)).IsRequired().HasMaxLength(MaterialGroupConsts.NameMaxLength);
                b.Property(x => x.Parent).HasColumnName(nameof(MaterialGroup.Parent));
                b.Property(x => x.SortOrder).HasColumnName(nameof(MaterialGroup.SortOrder)).IsRequired();
                b.Property(x => x.Note).HasColumnName(nameof(MaterialGroup.Note)).HasMaxLength(MaterialGroupConsts.NoteMaxLength);
                b.Property(x => x.IsDeActive).HasColumnName(nameof(MaterialGroup.IsDeActive)).IsRequired();
                b.Property(x => x.MaterialType).HasColumnName(nameof(MaterialGroup.MaterialType)).HasMaxLength(MaterialGroupConsts.MaterialTypeMaxLength);
                b.Property(x => x.MaterialGroupPSI).HasColumnName(nameof(MaterialGroup.MaterialGroupPSI)).HasMaxLength(MaterialGroupConsts.MaterialGroupPSIMaxLength);
                b.Property(x => x.AllowKeyAccount).HasColumnName(nameof(MaterialGroup.AllowKeyAccount)).IsRequired();
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialStockUploadDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialStockUploadDetail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.RequestId).HasColumnName(nameof(MaterialStockUploadDetail.RequestId)).IsRequired();
                b.Property(x => x.MaterialCode).HasColumnName(nameof(MaterialStockUploadDetail.MaterialCode)).IsRequired().HasMaxLength(MaterialStockUploadDetailConsts.MaterialCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(MaterialStockUploadDetail.Model)).HasMaxLength(MaterialStockUploadDetailConsts.ModelMaxLength);
                b.Property(x => x.Storage).HasColumnName(nameof(MaterialStockUploadDetail.Storage)).HasMaxLength(MaterialStockUploadDetailConsts.StorageMaxLength);
                b.Property(x => x.StorageDestination).HasColumnName(nameof(MaterialStockUploadDetail.StorageDestination)).HasMaxLength(MaterialStockUploadDetailConsts.StorageDestinationMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(MaterialStockUploadDetail.Qty));
                b.Property(x => x.RefDoc).HasColumnName(nameof(MaterialStockUploadDetail.RefDoc)).HasMaxLength(MaterialStockUploadDetailConsts.RefDocMaxLength);
                b.Property(x => x.Remark).HasColumnName(nameof(MaterialStockUploadDetail.Remark));
                b.Property(x => x.StorageSrc_Id).HasColumnName(nameof(MaterialStockUploadDetail.StorageSrc_Id));
                b.Property(x => x.StorageDesc_Id).HasColumnName(nameof(MaterialStockUploadDetail.StorageDesc_Id));
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SpecialInputPrice>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SpecialInputPrice", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.AccountNo).HasColumnName(nameof(SpecialInputPrice.AccountNo)).IsRequired().HasMaxLength(SpecialInputPriceConsts.AccountNoMaxLength);
                b.Property(x => x.AccountName).HasColumnName(nameof(SpecialInputPrice.AccountName)).IsRequired().HasMaxLength(SpecialInputPriceConsts.AccountNameMaxLength);
                b.Property(x => x.ProjectName).HasColumnName(nameof(SpecialInputPrice.ProjectName)).HasMaxLength(SpecialInputPriceConsts.ProjectNameMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(SpecialInputPrice.MaterialType)).HasMaxLength(SpecialInputPriceConsts.MaterialTypeMaxLength);
                b.Property(x => x.SupplierId).HasColumnName(nameof(SpecialInputPrice.SupplierId));
                b.Property(x => x.SupplierBUId).HasColumnName(nameof(SpecialInputPrice.SupplierBUId));
                b.Property(x => x.Currency).HasColumnName(nameof(SpecialInputPrice.Currency)).HasMaxLength(SpecialInputPriceConsts.CurrencyMaxLength);
                b.Property(x => x.ValidFrom).HasColumnName(nameof(SpecialInputPrice.ValidFrom));
                b.Property(x => x.ValidTo).HasColumnName(nameof(SpecialInputPrice.ValidTo));
                b.Property(x => x.Status).HasColumnName(nameof(SpecialInputPrice.Status)).IsRequired().HasMaxLength(SpecialInputPriceConsts.StatusMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(SpecialInputPrice.Note)).HasMaxLength(SpecialInputPriceConsts.NoteMaxLength);

                b.HasMany(x => x.Details).WithOne(y => y.SpecialInputPrice).HasForeignKey(x => x.SpecialInputPriceId);
                b.HasOne(x => x.Supplier).WithMany().HasForeignKey(y => y.SupplierId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.SupplierBU).WithMany().HasForeignKey(y => y.SupplierBUId).OnDelete(DeleteBehavior.Restrict);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SpecialInputPriceDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SpecialInputPriceDetail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.SpecialInputPriceId).HasColumnName(nameof(SpecialInputPriceDetail.SpecialInputPriceId));
                b.Property(x => x.AccountNo).HasColumnName(nameof(SpecialInputPriceDetail.AccountNo)).HasMaxLength(SpecialInputPriceDetailConsts.MaterialCodeMaxLength);
                b.Property(x => x.MaterialCode).HasColumnName(nameof(SpecialInputPriceDetail.MaterialCode)).HasMaxLength(SpecialInputPriceDetailConsts.MaterialCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(SpecialInputPriceDetail.Model)).HasMaxLength(SpecialInputPriceDetailConsts.ModelMaxLength);
                b.Property(x => x.Spec1).HasColumnName(nameof(SpecialInputPriceDetail.Spec1)).HasMaxLength(SpecialInputPriceDetailConsts.Spec1MaxLength);
                b.Property(x => x.LimitQty).HasColumnName(nameof(SpecialInputPriceDetail.LimitQty));
                b.Property(x => x.InputPrice).HasColumnName(nameof(SpecialInputPriceDetail.InputPrice));
                b.Property(x => x.LandedCost).HasColumnName(nameof(SpecialInputPriceDetail.LandedCost));
                b.Property(x => x.Used).HasColumnName(nameof(SpecialInputPriceDetail.Used));
                b.Property(x => x.Note).HasColumnName(nameof(SpecialInputPriceDetail.Note)).HasMaxLength(SpecialInputPriceDetailConsts.NoteMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<WorkflowApprover>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Workflow_Approver", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.WFId).HasColumnName(nameof(WorkflowApprover.WFId)).IsRequired();
                b.Property(x => x.Approver).HasColumnName(nameof(WorkflowApprover.Approver)).IsRequired().HasMaxLength(WorkflowApproverConsts.ApproverMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(WorkflowApprover.Note)).HasMaxLength(WorkflowApproverConsts.NoteMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<WorkflowConfiguration>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Workflow_Configuration", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.WorkflowType).HasColumnName("WF_Type").IsRequired().HasMaxLength(WorkflowConfigurationConsts.WorkflowTypeMaxLength);
                b.Property(x => x.WorkflowLevel).HasColumnName("WF_Level").IsRequired();
                b.Property(x => x.WorkflowRole).HasColumnName("WF_Role").IsRequired().HasMaxLength(WorkflowConfigurationConsts.WorkflowRoleMaxLength);
                b.Property(x => x.Condition).HasColumnName(nameof(WorkflowConfiguration.Condition)).HasMaxLength(WorkflowConfigurationConsts.ConditionMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(WorkflowConfiguration.Note)).HasMaxLength(WorkflowConfigurationConsts.NoteMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<CustomerPIC>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Customer_PIC", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.KeyAccountId).HasColumnName(nameof(CustomerPIC.KeyAccountId)).IsRequired();
                b.Property(x => x.PICName).HasColumnName(nameof(CustomerPIC.PICName)).HasMaxLength(CustomerPICConsts.PICNameMaxLength);
                b.Property(x => x.PIC_Phone).HasColumnName(nameof(CustomerPIC.PIC_Phone)).HasMaxLength(CustomerPICConsts.PIC_PhoneMaxLength);
                b.Property(x => x.PIC_Email).HasColumnName(nameof(CustomerPIC.PIC_Email)).HasMaxLength(CustomerPICConsts.PIC_EmailMaxLength);
                b.Property(x => x.PIC_JobTitle).HasColumnName(nameof(CustomerPIC.PIC_JobTitle)).HasMaxLength(CustomerPICConsts.PIC_JobTitleMaxLength);
                b.Property(x => x.Remark).HasColumnName(nameof(CustomerPIC.Remark)).HasMaxLength(CustomerPICConsts.RemarkMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Supplier>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Supplier", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.SupplierCode).HasColumnName(nameof(Supplier.SupplierCode)).IsRequired().HasMaxLength(SupplierConsts.SupplierCodeMaxLength);
                b.Property(x => x.SAPCode).HasColumnName(nameof(Supplier.SAPCode)).HasMaxLength(SupplierConsts.SAPCodeMaxLength);
                b.Property(x => x.ShortName).HasColumnName(nameof(Supplier.ShortName)).IsRequired().HasMaxLength(SupplierConsts.ShortNameMaxLength);
                b.Property(x => x.FullName).HasColumnName(nameof(Supplier.FullName)).IsRequired().HasMaxLength(SupplierConsts.FullNameMaxLength);
                b.Property(x => x.TaxCode).HasColumnName(nameof(Supplier.TaxCode)).HasMaxLength(SupplierConsts.TaxCodeMaxLength);
                b.Property(x => x.Address).HasColumnName(nameof(Supplier.Address)).HasMaxLength(SupplierConsts.AddressMaxLength);
                b.Property(x => x.IsDeactive).HasColumnName(nameof(Supplier.IsDeactive));
            });
        }
        if (builder.IsHostDatabase())
        {

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Message>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Discussion", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.UserName).HasColumnName(nameof(Message.UserName)).IsRequired().HasMaxLength(MessageConsts.UserNameMaxLength);
                b.Property(x => x.FullName).HasColumnName(nameof(Message.FullName)).IsRequired().HasMaxLength(MessageConsts.FullNameMaxLength);
                b.Property(x => x.SendTo).HasColumnName(nameof(Message.SendTo)).IsRequired().HasMaxLength(MessageConsts.SendToMaxLength);
                b.Property(x => x.Comment).HasColumnName(nameof(Message.Comment)).IsRequired();

                b.HasDiscriminator<string>("EntityType")
                    .HasValue<PriceOfferMessage>(EntityTypes.PriceOffer)
                    .HasValue<DPOMessage>(EntityTypes.DPO);

                builder.Entity<PriceOfferMessage>(b =>
                {
                    b.HasBaseType<Message>();
                    b.Property(x => x.PriceOfferId).HasColumnName(nameof(PriceOfferMessage.PriceOfferId)).IsRequired();
                });

                builder.Entity<DPOMessage>(b =>
                {
                    b.HasBaseType<Message>();
                    b.Property(x => x.DPOId).HasColumnName(nameof(DPOMessage.DPOId)).IsRequired();
                });
            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialStockUpload>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialStockUpload", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.RequestNo).HasColumnName(nameof(MaterialStockUpload.RequestNo)).HasMaxLength(MaterialStockUploadConsts.RequestNoMaxLength);
                b.Property(x => x.ImportType).HasColumnName(nameof(MaterialStockUpload.ImportType)).HasMaxLength(MaterialStockUploadConsts.ImportTypeMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(MaterialStockUpload.FileName)).HasMaxLength(MaterialStockUploadConsts.FilNameMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(MaterialStockUpload.Note)).HasMaxLength(MaterialStockUploadConsts.NoteMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(MaterialStockUpload.Status)).HasMaxLength(MaterialStockUploadConsts.StatusMaxLength);
                b.HasMany(x => x.MaterialStockUploadDetails).WithOne().HasForeignKey(y => y.RequestId).OnDelete(DeleteBehavior.Restrict);

            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SystemConfiguration>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SystemConfiguration", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.CfgKey).HasColumnName(nameof(SystemConfiguration.CfgKey)).IsRequired().HasMaxLength(SystemConfigurationConsts.CfgKeyMaxLength);
                b.Property(x => x.CfgValue).HasColumnName(nameof(SystemConfiguration.CfgValue)).IsRequired().HasMaxLength(SystemConfigurationConsts.CfgValueMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(SystemConfiguration.Description)).HasMaxLength(SystemConfigurationConsts.DescriptionMaxLength);
                b.Property(x => x.IsSystemCfg).HasColumnName(nameof(SystemConfiguration.IsSystemCfg));
                b.Property(x => x.CfgType).HasColumnName(nameof(SystemConfiguration.CfgType)).IsRequired().HasMaxLength(SystemConfigurationConsts.CfgKeyMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SupplierBU>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Supplier_BU", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.SupplierBUCode).HasColumnName(nameof(SupplierBU.SupplierBUCode)).IsRequired().HasMaxLength(SupplierBUConsts.SupplierBUCodeMaxLength);
                b.Property(x => x.SupplierBURemarks).HasColumnName(nameof(SupplierBU.SupplierBURemarks)).HasMaxLength(SupplierBUConsts.SupplierBURemarksMaxLength);
                b.Property(x => x.OrderMethod).HasColumnName(nameof(SupplierBU.OrderMethod)).HasMaxLength(SupplierBUConsts.OrderMethodMaxLength);
                b.Property(x => x.POTemplate).HasColumnName(nameof(SupplierBU.POTemplate)).HasMaxLength(SupplierBUConsts.POTemplateMaxLength);
                b.Property(x => x.Contact).HasColumnName(nameof(SupplierBU.Contact)).HasMaxLength(SupplierBUConsts.ContactMaxLength);
                b.Property(x => x.Email).HasColumnName(nameof(SupplierBU.Email)).HasMaxLength(SupplierBUConsts.EmailMaxLength);
                b.Property(x => x.INCOTerm).HasColumnName(nameof(SupplierBU.INCOTerm)).HasMaxLength(SupplierBUConsts.INCOTermMaxLength);
                b.Property(x => x.PaymentTermCode).HasColumnName(nameof(SupplierBU.PaymentTermCode)).HasMaxLength(SupplierBUConsts.PaymentTermCodeMaxLength);
                b.Property(x => x.PaymentDescription).HasColumnName(nameof(SupplierBU.PaymentDescription)).HasMaxLength(SupplierBUConsts.PaymentDescriptionMaxLength);
                b.Property(x => x.Currency).HasColumnName(nameof(SupplierBU.Currency)).HasMaxLength(SupplierBUConsts.CurrencyMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(SupplierBU.MaterialType)).HasMaxLength(SupplierBUConsts.MaterialTypeMaxLength);
                b.Property(x => x.SupplierId).HasColumnName(nameof(SupplierBU.SupplierId));
                b.Property(x => x.SupplierCode).HasColumnName(nameof(SupplierBU.SupplierCode)).HasMaxLength(SupplierBUConsts.SupplierCodeMaxLength);
                b.Property(x => x.SupplierShortName).HasColumnName(nameof(SupplierBU.SupplierShortName)).HasMaxLength(SupplierBUConsts.SupplierShortNameMaxLength);
                b.Property(x => x.SupplierAddress).HasColumnName(nameof(SupplierBU.SupplierAddress)).HasMaxLength(SupplierBUConsts.SupplierAddressMaxLength);
                b.Property(x => x.SortOrder).HasColumnName(nameof(SupplierBU.SortOrder)).IsRequired();
                b.Property(x => x.FASCMVendorCode).HasColumnName(nameof(SupplierBU.FASCMVendorCode)).HasMaxLength(SupplierBUConsts.FASCMVendorCodeMaxLength);
                b.Property(x => x.FASCMBuyerCode).HasColumnName(nameof(SupplierBU.FASCMBuyerCode)).HasMaxLength(SupplierBUConsts.FASCMBuyerCodeMaxLength);
                b.Property(x => x.FASCMConsigneeCode).HasColumnName(nameof(SupplierBU.FASCMConsigneeCode)).HasMaxLength(SupplierBUConsts.FASCMConsigneeCodeMaxLength);
                b.Property(x => x.FASCMSectionCode).HasColumnName(nameof(SupplierBU.FASCMSectionCode)).HasMaxLength(SupplierBUConsts.FASCMSectionCodeMaxLength);
                b.Property(x => x.FASCMPaymentTerm).HasColumnName(nameof(SupplierBU.FASCMPaymentTerm)).HasMaxLength(SupplierBUConsts.FASCMPaymentTermMaxLength);
                b.Property(x => x.FASCMFreightMethod).HasColumnName(nameof(SupplierBU.FASCMFreightMethod)).HasMaxLength(SupplierBUConsts.FASCMFreightMethodMaxLength);
                b.Property(x => x.FASCMDeliveryTerms).HasColumnName(nameof(SupplierBU.FASCMDeliveryTerms)).HasMaxLength(SupplierBUConsts.FASCMDeliveryTermsMaxLength);
                b.Property(x => x.FASCMPlaceOfDeliveryTerms).HasColumnName(nameof(SupplierBU.FASCMPlaceOfDeliveryTerms)).HasMaxLength(SupplierBUConsts.FASCMPlaceOfDeliveryTermsMaxLength);
                b.Property(x => x.FASCMShippingMarkCode).HasColumnName(nameof(SupplierBU.FASCMShippingMarkCode)).HasMaxLength(SupplierBUConsts.FASCMShippingMarkCodeMaxLength);
                b.Property(x => x.IsDeactive).HasColumnName(nameof(SupplierBU.IsDeactive));

                b.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialGroupBuyer>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialGroupBuyer", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.MaterialGroupId).HasColumnName(nameof(MaterialGroupBuyer.MaterialGroupId));
                b.Property(x => x.MaterialGroupCode).HasColumnName(nameof(MaterialGroupBuyer.MaterialGroupCode)).HasMaxLength(MaterialGroupBuyerConsts.MaterialGroupCodeMaxLength);
                b.Property(x => x.BuyerId).HasColumnName(nameof(MaterialGroupBuyer.BuyerId)).IsRequired();
                b.Property(x => x.BuyerShortName).HasColumnName(nameof(MaterialGroupBuyer.BuyerShortName)).HasMaxLength(MaterialGroupBuyerConsts.BuyerShortNameMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(MaterialGroupBuyer.Note));

                b.HasOne(x => x.MaterialGroup).WithMany().HasForeignKey(x => x.MaterialGroupId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.MaterialGroup).WithMany().HasForeignKey(x => x.MaterialGroupId).OnDelete(DeleteBehavior.Restrict);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SaleOrder>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SaleOrder", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.SONo).HasColumnName(nameof(SaleOrder.SONo)).IsRequired().HasMaxLength(SaleOrderConsts.SONoMaxLength);
                b.Property(x => x.SOSAPNo).HasColumnName("SAPSONo").HasMaxLength(SaleOrderConsts.SOSAPNoMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(SaleOrder.MaterialType)).HasMaxLength(SaleOrderConsts.MaterialTypeMaxLength);
                b.Property(x => x.BuyerType).HasColumnName(nameof(SaleOrder.BuyerType)).HasMaxLength(SaleOrderConsts.MaterialTypeMaxLength);
                b.Property(x => x.BuyerId).HasColumnName(nameof(SaleOrder.BuyerId));
                b.Property(x => x.BuyerCode).HasColumnName(nameof(SaleOrder.BuyerCode)).HasMaxLength(SaleOrderConsts.BuyerCodeMaxLength);
                b.Property(x => x.BuyerName).HasColumnName(nameof(SaleOrder.BuyerName)).HasMaxLength(SaleOrderConsts.BuyerNameMaxLength);
                b.Property(x => x.OrderDate).HasColumnName(nameof(SaleOrder.OrderDate));
                b.Property(x => x.StatusCode).HasColumnName(nameof(SaleOrder.StatusCode)).HasMaxLength(SaleOrderConsts.StatusCodeMaxLength);
                b.Property(x => x.StockCategoryId).HasColumnName(nameof(SaleOrder.StockCategoryId));
                b.Property(x => x.SO_VAT).HasColumnName(nameof(SaleOrder.SO_VAT));
                b.Property(x => x.Note).HasColumnName(nameof(SaleOrder.Note)).HasMaxLength(SaleOrderConsts.NoteMaxLength);
                b.Property(x => x.IsDeleted).HasColumnName(nameof(SaleOrder.IsDeleted));
                b.Property(x => x.SAPBillingNo).HasColumnName(nameof(SaleOrder.SAPBillingNo)).HasMaxLength(SaleOrderConsts.SOSAPNoMaxLength);
                b.Property(x => x.SAPDONo).HasColumnName(nameof(SaleOrder.SAPDONo)).HasMaxLength(SaleOrderConsts.SOSAPNoMaxLength);
                b.Property(x => x.SAPDeliveryDate).HasColumnName(nameof(SaleOrder.SAPDeliveryDate));
                b.Property(x => x.SAPInvoice).HasColumnName(nameof(SaleOrder.SAPInvoice)).HasMaxLength(SaleOrderConsts.SOSAPNoMaxLength);
                b.Property(x => x.SAPInvoiceDate).HasColumnName(nameof(SaleOrder.SAPInvoiceDate)).HasMaxLength(SaleOrderConsts.SOSAPNoMaxLength);
                b.Property(x => x.DeliveryConfirmed).HasColumnName(nameof(SaleOrder.DeliveryConfirmed));
                b.Property(x => x.SOType).HasColumnName(nameof(SaleOrder.SOType)).HasMaxLength(SaleOrderConsts.SOTypeMaxLength);
                b.Property(x => x.GICType).HasColumnName(nameof(SaleOrder.GICType)).HasMaxLength(SaleOrderConsts.GICTypeMaxLength);
                b.Property(x => x.GICProcess).HasColumnName(nameof(SaleOrder.GICProcess)).HasMaxLength(SaleOrderConsts.GICProcessMaxLength);

                b.Property(x => x.GICGivNo).HasColumnName(nameof(SaleOrder.GICGivNo)).HasMaxLength(SaleOrderConsts.SONoMaxLength);
                b.Property(x => x.GICGivDate).HasColumnName(nameof(SaleOrder.GICGivDate));
                b.Property(x => x.CompletelyClosed).HasColumnName(nameof(SaleOrder.CompletelyClosed));

                b.HasMany(x => x.SaleOrderDetails).WithOne().HasForeignKey(y => y.SaleOrderId);
                b.HasMany(x => x.SOHistories).WithOne().HasForeignKey(y => y.SOId).OnDelete(DeleteBehavior.Cascade);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SaleOrderDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SaleOrderDetail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.SaleOrderId).HasColumnName(nameof(SaleOrderDetail.SaleOrderId)).IsRequired();
                b.Property(x => x.DPODetailId).HasColumnName(nameof(SaleOrderDetail.DPODetailId));
                b.Property(x => x.StatusCode).HasColumnName(nameof(SaleOrderDetail.StatusCode)).HasMaxLength(SaleOrderDetailConsts.StatusCodeMaxLength);
                b.Property(x => x.GolfaCode).HasColumnName(nameof(SaleOrderDetail.GolfaCode)).HasMaxLength(SaleOrderDetailConsts.GolfaCodeMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(SaleOrderDetail.Qty));
                b.Property(x => x.Price).HasColumnName(nameof(SaleOrderDetail.Price));
                b.Property(x => x.Amount).HasColumnName(nameof(SaleOrderDetail.Amount));
                b.Property(x => x.VAT).HasColumnName(nameof(SaleOrderDetail.VAT));
                b.Property(x => x.Extrafee).HasColumnName(nameof(SaleOrderDetail.Extrafee));
                b.Property(x => x.AmountIncludeExtrafee).HasColumnName(nameof(SaleOrderDetail.AmountIncludeExtrafee));
                b.Property(x => x.StockCategoryId).HasColumnName(nameof(SaleOrderDetail.StockCategoryId));
                b.Property(x => x.Note).HasColumnName(nameof(SaleOrderDetail.Note)).HasMaxLength(SaleOrderDetailConsts.NoteMaxLength);
                b.Property(x => x.Extrafee_Note).HasColumnName(nameof(SaleOrderDetail.Extrafee_Note)).HasMaxLength(SaleOrderDetailConsts.MaxExtrafee_NoteLength);
                b.Property(x => x.LockStockId).HasColumnName(nameof(SaleOrderDetail.LockStockId));
                b.Property(x => x.IsDeleted).HasColumnName(nameof(SaleOrder.IsDeleted));

                b.Property(x => x.SAPLandingCost).HasColumnName(nameof(SaleOrderDetail.SAPLandingCost));

                b.Property(x => x.SAPAmountLandingCost).HasColumnName(nameof(SaleOrderDetail.SAPAmountLandingCost));

                b.Property(x => x.GICPorNo).HasColumnName(nameof(SaleOrderDetail.GICPorNo)).HasMaxLength(128);

                b.Property(x => x.GICPrNo).HasColumnName(nameof(SaleOrderDetail.GICPrNo)).HasMaxLength(128);

                b.Property(x => x.GICSalePIC).HasColumnName(nameof(SaleOrderDetail.GICSalePIC)).HasMaxLength(400);

                b.Property(x => x.GICLocation).HasColumnName(nameof(SaleOrderDetail.GICLocation)).HasMaxLength(400);

                b.Property(x => x.GICReservationNo).HasColumnName(nameof(SaleOrderDetail.GICReservationNo)).HasMaxLength(128);

                b.Property(x => x.GICGivNo).HasColumnName(nameof(SaleOrderDetail.GICGivNo)).HasMaxLength(128);
                b.Property(x => x.GICGivDate).HasColumnName(nameof(SaleOrderDetail.GICGivDate));
                b.Property(x => x.ChangeNote).HasColumnName(nameof(SaleOrderDetail.ChangeNote));
                b.Property(x => x.Disposed).HasColumnName(nameof(SaleOrdersSapImport.Disposed));
                b.HasOne(x => x.DPODetail).WithMany().HasForeignKey(y => y.DPODetailId);
                b.HasOne(x => x.StockCategory).WithMany().HasForeignKey(y => y.StockCategoryId);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PurchaseOrder>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "PurchaseOrder", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.PONo).HasColumnName(nameof(PurchaseOrder.PONo)).IsRequired().HasMaxLength(PurchaseOrderConsts.PONoMaxLength);
                b.Property(x => x.PODate).HasColumnName(nameof(PurchaseOrder.PODate));
                b.Property(x => x.POSAPNo).HasColumnName(nameof(PurchaseOrder.POSAPNo)).HasMaxLength(PurchaseOrderConsts.POSAPNoMaxLength);
                b.Property(x => x.POSAPDate).HasColumnName(nameof(PurchaseOrder.POSAPDate));
                b.Property(x => x.StatusCode).HasColumnName(nameof(PurchaseOrder.StatusCode)).HasMaxLength(PurchaseOrderConsts.StatusCodeMaxLength);
                b.Property(x => x.CreateSource).HasColumnName(nameof(PurchaseOrder.CreateSource)).HasMaxLength(PurchaseOrderConsts.CreateSourceMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(PurchaseOrder.MaterialType)).HasMaxLength(PurchaseOrderConsts.MaterialTypeMaxLength);
                b.Property(x => x.SupplierBUId).HasColumnName(nameof(PurchaseOrder.SupplierBUId));
                b.Property(x => x.SupplierBUCode).HasColumnName(nameof(PurchaseOrder.SupplierBUCode)).HasMaxLength(PurchaseOrderConsts.SupplierBUCodeMaxLength);
                b.Property(x => x.SupplierId).HasColumnName(nameof(PurchaseOrder.SupplierId));
                b.Property(x => x.SupplierCode).HasColumnName(nameof(PurchaseOrder.SupplierCode)).HasMaxLength(PurchaseOrderConsts.SupplierBUCodeMaxLength);
                b.Property(x => x.Currency).HasColumnName(nameof(PurchaseOrder.Currency)).IsRequired().HasMaxLength(PurchaseOrderConsts.CurrencyMaxLength);
                b.Property(x => x.EPA).HasColumnName(nameof(PurchaseOrder.EPA)).IsRequired();
                b.Property(x => x.SendToSupplier).HasColumnName(nameof(PurchaseOrder.SendToSupplier)).IsRequired();
                b.Property(x => x.OurRef).HasColumnName(nameof(PurchaseOrder.OurRef)).HasMaxLength(PurchaseOrderConsts.OurRefMaxLength);
                b.Property(x => x.IsDeleted).HasColumnName(nameof(SaleOrder.IsDeleted));
                b.HasOne(x => x.SupplierBU).WithMany().HasForeignKey(y => y.SupplierBUId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Supplier).WithMany().HasForeignKey(y => y.SupplierId);
                b.HasMany(x => x.PurchaseOrderDetails).WithOne(y => y.PurchaseOrder).HasForeignKey(y => y.PurchaseOrderId);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PurchaseOrderDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "PurchaseOrderDetail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.PurchaseOrderId).HasColumnName(nameof(PurchaseOrderDetail.PurchaseOrderId));
                b.Property(x => x.GolfaCode).HasColumnName(nameof(PurchaseOrderDetail.GolfaCode)).IsRequired().HasMaxLength(PurchaseOrderDetailConsts.GolfaCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(PurchaseOrderDetail.Model)).HasMaxLength(PurchaseOrderDetailConsts.ModelMaxLength);
                b.Property(x => x.StatusCode).HasColumnName(nameof(PurchaseOrderDetail.StatusCode)).HasMaxLength(PurchaseOrderDetailConsts.StatusCodeMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(PurchaseOrderDetail.Qty));
                b.Property(x => x.Price).HasColumnName(nameof(PurchaseOrderDetail.Price));
                b.Property(x => x.Amount).HasColumnName(nameof(PurchaseOrderDetail.Amount));
                b.Property(x => x.AmountVND).HasColumnName(nameof(PurchaseOrderDetail.AmountVND));
                b.Property(x => x.Note).HasColumnName(nameof(PurchaseOrderDetail.Note)).HasMaxLength(PurchaseOrderDetailConsts.NoteMaxLength);
                b.Property(x => x.ProjectCode).HasColumnName(nameof(PurchaseOrderDetail.ProjectCode)).HasMaxLength(PurchaseOrderDetailConsts.ProjectCodeMaxLength);
                b.Property(x => x.AccountNo).HasColumnName(nameof(PurchaseOrderDetail.AccountNo)).HasMaxLength(PurchaseOrderDetailConsts.AccountNoMaxLength);
                b.Property(x => x.QtyImported).HasColumnName(nameof(PurchaseOrderDetail.QtyImported));
                b.Property(x => x.QtyLocked).HasColumnName(nameof(PurchaseOrderDetail.QtyLocked));
                b.Property(x => x.QtyAvailable).HasColumnName(nameof(PurchaseOrderDetail.QtyAvailable));
                b.Property(x => x.LeadTime).HasColumnName(nameof(PurchaseOrderDetail.LeadTime));
                b.Property(x => x.STCReply).HasColumnName("Cargo_STCReply");
                b.Property(x => x.MachineNumber).HasColumnName("Cargo_MachineNumber").HasMaxLength(CargoDataConsts.MachineNumberMaxLength);
                b.Property(x => x.MEVNAddedRequest).HasColumnName("Cargo_MEVNAddedRequest").HasMaxLength(CargoDataConsts.MEVNAddedRequestMaxLength);
                b.Property(x => x.MEVNRequest).HasColumnName("Cargo_MEVNRequest").HasMaxLength(CargoDataConsts.MEVNAddedRequestMaxLength);
                b.Property(x => x.Maxlot).HasColumnName(nameof(PurchaseOrderDetail.Maxlot));
                b.Property(x => x.IsDeleted).HasColumnName(nameof(SaleOrder.IsDeleted));
                b.Property(x => x.PODetailCode).HasColumnName(nameof(PurchaseOrderDetail.PODetailCode)).HasMaxLength(PurchaseOrderDetailConsts.PODetalCodeMaxLength);
                b.Property(x => x.Urgent).HasColumnName(nameof(PurchaseOrderDetail.Urgent));

                b.Property(x => x.RequestETA).HasColumnName(nameof(PurchaseOrderDetail.RequestETA));
                b.Property(x => x.Customer).HasColumnName(nameof(PurchaseOrderDetail.Customer)).HasMaxLength(PurchaseOrderDetailConsts.Customer);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<StockImport>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "StockImport", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.InvoiceNo).HasColumnName(nameof(StockImport.InvoiceNo)).IsRequired().HasMaxLength(StockImportConsts.InvoiceNoMaxLength);
                b.Property(x => x.InvoiceType).HasColumnName(nameof(StockImport.InvoiceType)).HasMaxLength(StockImportConsts.InvoiceTypeMaxLength);
                b.Property(x => x.DeliveryTerm).HasColumnName(nameof(StockImport.DeliveryTerm)).HasMaxLength(StockImportConsts.DeliveryTermMaxLength);
                b.Property(x => x.SupplierId).HasColumnName(nameof(StockImport.SupplierId));
                b.Property(x => x.SupplierCode).HasColumnName(nameof(StockImport.SupplierCode)).HasMaxLength(StockImportConsts.SupplierCodeMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(StockImport.FileName)).IsRequired().HasMaxLength(StockImportConsts.FileNameMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(StockImport.Status)).HasMaxLength(StockImportConsts.StatusMaxLength);
                b.Property(x => x.InvoiceDate).HasColumnName(nameof(StockImport.InvoiceDate));
                b.Property(x => x.StockDate).HasColumnName(nameof(StockImport.StockDate));
                b.Property(x => x.ShipmentMethod).HasColumnName(nameof(StockImport.ShipmentMethod)).HasMaxLength(StockImportConsts.ShipmentMethodMaxLength);
                b.Property(x => x.ETD).HasColumnName(nameof(StockImport.ETD));
                b.Property(x => x.ETA).HasColumnName(nameof(StockImport.ETA));
                b.Property(x => x.BillNo).HasColumnName(nameof(StockImport.BillNo)).HasMaxLength(StockImportConsts.BillNoMaxLength);
                b.Property(x => x.CDNo).HasColumnName(nameof(StockImport.CDNo)).HasMaxLength(StockImportConsts.CDNoMaxLength);
                b.Property(x => x.CDDate).HasColumnName(nameof(StockImport.CDDate));
                b.Property(x => x.ATD).HasColumnName(nameof(StockImport.ATD));
                b.Property(x => x.ATA).HasColumnName(nameof(StockImport.ATA));
                b.Property(x => x.ReceivingReportDate).HasColumnName(nameof(StockImport.ReceivingReportDate));
                b.Property(x => x.WHArrivalDate).HasColumnName(nameof(StockImport.WHArrivalDate));
                b.Property(x => x.Note).HasColumnName(nameof(StockImport.Note)).HasMaxLength(StockImportConsts.NoteMaxLength);
                b.Property(x => x.StockCodeConfirmed).HasColumnName(nameof(StockImport.StockCodeConfirmed)).HasMaxLength(StockImportConsts.InvoiceNoMaxLength);

                b.Property(x => x.StockNameConfirmed).HasColumnName(nameof(StockImport.StockNameConfirmed)).HasMaxLength(StockImportConsts.ShipmentMethodMaxLength);
                b.Property(x => x.TotalQty).HasColumnName(nameof(StockImport.TotalQty));
                b.Property(x => x.TotalAmount).HasColumnName(nameof(StockImport.TotalAmount));
                b.Property(x => x.ConfirmNote).HasColumnName(nameof(StockImport.ConfirmNote));

                // Relationship: StockImport -> StockImportDetails (1:N)
                b.HasMany(x => x.Details).WithOne().HasForeignKey(x => x.StockImportId).OnDelete(DeleteBehavior.Cascade);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<StockImportDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "StockImportDetails", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.StockImportId).HasColumnName(nameof(StockImportDetail.StockImportId)).IsRequired();
                b.Property(x => x.InvoiceNo).HasColumnName(nameof(StockImportDetail.InvoiceNo)).IsRequired().HasMaxLength(StockImportDetailConsts.InvoiceNoMaxLength);
                b.Property(x => x.ItemModel).HasColumnName(nameof(StockImportDetail.ItemModel)).HasMaxLength(StockImportDetailConsts.ItemModelMaxLength);
                b.Property(x => x.MaterialCode).HasColumnName(nameof(StockImportDetail.MaterialCode)).HasMaxLength(StockImportDetailConsts.MaterialCodeMaxLength);
                b.Property(x => x.Unit).HasColumnName(nameof(StockImportDetail.Unit)).HasMaxLength(StockImportDetailConsts.UnitMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(StockImportDetail.Qty));
                b.Property(x => x.Price).HasColumnName(nameof(StockImportDetail.Price));
                b.Property(x => x.Amount).HasColumnName(nameof(StockImportDetail.Amount));
                b.Property(x => x.GensanchiNM).HasColumnName(nameof(StockImportDetail.GensanchiNM)).HasMaxLength(StockImportDetailConsts.GensanchiNMMaxLength);
                b.Property(x => x.ETA).HasColumnName(nameof(StockImportDetail.ETA));
                b.Property(x => x.ETD).HasColumnName(nameof(StockImportDetail.ETD));
                b.Property(x => x.ShipmentMethod).HasColumnName(nameof(StockImportDetail.ShipmentMethod)).HasMaxLength(StockImportDetailConsts.ShipmentMethodMaxLength);
                b.Property(x => x.BillNo).HasColumnName(nameof(StockImportDetail.BillNo)).HasMaxLength(StockImportDetailConsts.BillNoMaxLength);
                b.Property(x => x.MachineNumber).HasColumnName(nameof(StockImportDetail.MachineNumber)).HasMaxLength(StockImportDetailConsts.MachineNumberMaxLength);
                b.Property(x => x.PONo).HasColumnName(nameof(StockImportDetail.PONo)).IsRequired().HasMaxLength(StockImportDetailConsts.PONoMaxLength);
                b.Property(x => x.CDNo).HasColumnName(nameof(StockImportDetail.CDNo)).HasMaxLength(StockImportDetailConsts.CDNoMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(StockImportDetail.Note)).HasMaxLength(StockImportDetailConsts.NoteMaxLength);
                b.Property(x => x.DeliveryTerm).HasColumnName(nameof(StockImportDetail.DeliveryTerm)).HasMaxLength(StockImportDetailConsts.DeliveryTermMaxLength);
                b.Property(x => x.Origin).HasColumnName(nameof(StockImportDetail.Origin)).HasMaxLength(StockImportDetailConsts.OriginMaxLength);
                b.Property(x => x.InvoiceDate).HasColumnName(nameof(StockImportDetail.InvoiceDate));

            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialStockLockStock>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialStock_LockStock", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.GolfaCode).HasColumnName(nameof(MaterialStockLockStock.GolfaCode)).IsRequired().HasMaxLength(MaterialStockLockStockConsts.GolfaCodeMaxLength);
                b.Property(x => x.DPODetailId).HasColumnName("DPODetail_Id");
                b.Property(x => x.StockCategoryId).HasColumnName(nameof(MaterialStockLockStock.StockCategoryId));
                b.Property(x => x.Qty).HasColumnName(nameof(MaterialStockLockStock.Qty));
                b.Property(x => x.Note).HasColumnName(nameof(MaterialStockLockStock.Note));
                b.Property(x => x.ReleasedLock).HasColumnName(nameof(MaterialStockLockStock.ReleasedLock));

                b.HasOne(x => x.StockCategory).WithMany().HasForeignKey(y => y.StockCategoryId);

            });
        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<StockImportPriority>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "StockImport_Priority", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.DPONo).HasColumnName(nameof(StockImportPriority.DPONo)).IsRequired().HasMaxLength(StockImportPriorityConsts.DPONoMaxLength);
                b.Property(x => x.PONo).HasColumnName(nameof(StockImportPriority.PONo)).IsRequired().HasMaxLength(StockImportPriorityConsts.PONoMaxLength);
                b.Property(x => x.MaterialCode).HasColumnName(nameof(StockImportPriority.MaterialCode)).IsRequired().HasMaxLength(StockImportPriorityConsts.MaterialCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(StockImportPriority.Model)).HasMaxLength(StockImportPriorityConsts.ModelMaxLength);
                b.Property(x => x.StatusCode).HasColumnName(nameof(StockImportPriority.StatusCode)).HasMaxLength(StockImportPriorityConsts.StatusCodeMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(StockImportPriority.Qty)).IsRequired();
                b.Property(x => x.Priority).HasColumnName(nameof(StockImportPriority.Priority));
                b.Property(x => x.QtyUsed).HasColumnName(nameof(StockImportPriority.QtyUsed));
                b.Property(x => x.QtyAvailable).HasColumnName(nameof(StockImportPriority.QtyAvailable));
                b.Property(x => x.Note).HasColumnName(nameof(StockImportPriority.Note));
                b.Property(x => x.ImportGuid).HasColumnName(nameof(StockImportPriority.ImportGuid));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialStockLockStock>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialStock_LockStock", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.GolfaCode).HasColumnName(nameof(MaterialStockLockStock.GolfaCode)).IsRequired().HasMaxLength(MaterialStockLockStockConsts.GolfaCodeMaxLength);
                b.Property(x => x.DPODetailId).HasColumnName("DPODetail_Id");
                b.Property(x => x.StockCategoryId).HasColumnName(nameof(MaterialStockLockStock.StockCategoryId));
                b.Property(x => x.Qty).HasColumnName(nameof(MaterialStockLockStock.Qty));
                b.Property(x => x.Note).HasColumnName(nameof(MaterialStockLockStock.Note)).HasMaxLength(MaterialStockLockStockConsts.NoteMaxLength);
                b.Property(x => x.ReleasedLock).HasColumnName(nameof(MaterialStockLockStock.ReleasedLock));

                b.HasOne(x => x.StockCategory).WithMany().HasForeignKey(y => y.StockCategoryId).OnDelete(DeleteBehavior.Restrict);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<DistributorTarget>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "BuyerTarget", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.BuyerTypeId).HasColumnName(nameof(DistributorTarget.BuyerTypeId));
                b.Property(x => x.BuyerId).HasColumnName(nameof(DistributorTarget.BuyerId)).IsRequired();
                b.Property(x => x.BuyerCode).HasColumnName(nameof(DistributorTarget.BuyerCode)).HasMaxLength(DistributorTargetConsts.BuyerCodeMaxLength);
                b.Property(x => x.BuyerName).HasColumnName(nameof(DistributorTarget.BuyerName)).HasMaxLength(DistributorTargetConsts.BuyerNameMaxLength);
                b.Property(x => x.MaterialType).HasColumnName(nameof(DistributorTarget.MaterialType)).IsRequired().HasMaxLength(DistributorTargetConsts.MaterialTypeMaxLength);
                b.Property(x => x.FinanceYear).HasColumnName(nameof(DistributorTarget.FinanceYear));
                b.Property(x => x.FirstFYTarget).HasColumnName(nameof(DistributorTarget.FirstFYTarget));
                b.Property(x => x.SecondFYTarget).HasColumnName(nameof(DistributorTarget.SecondFYTarget));
                b.Property(x => x.Note).HasColumnName(nameof(DistributorTarget.Note));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<StockImportAllocation>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "StockImport_Allocation", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.StockImportId).HasColumnName(nameof(StockImportAllocation.StockImportId));
                b.Property(x => x.StockImportDetail_Id).HasColumnName(nameof(StockImportAllocation.StockImportDetail_Id)).IsRequired();
                b.Property(x => x.InvoiceNo).HasColumnName(nameof(StockImportAllocation.InvoiceNo)).HasMaxLength(StockImportAllocationConsts.InvoiceNoMaxLength);
                b.Property(x => x.PODetailId).HasColumnName(nameof(StockImportAllocation.PODetailId));
                b.Property(x => x.PONo).HasColumnName(nameof(StockImportAllocation.PONo)).HasMaxLength(StockImportAllocationConsts.PONoMaxLength);
                b.Property(x => x.DPODetailId).HasColumnName(nameof(StockImportAllocation.DPODetailId));
                b.Property(x => x.DPONo).HasColumnName(nameof(StockImportAllocation.DPONo)).HasMaxLength(StockImportAllocationConsts.DPONoMaxLength);
                b.Property(x => x.MaterialCode).HasColumnName(nameof(StockImportAllocation.MaterialCode)).HasMaxLength(StockImportAllocationConsts.MaterialCodeMaxLength);
                b.Property(x => x.Qty_Import).HasColumnName(nameof(StockImportAllocation.Qty_Import));
                b.Property(x => x.Price).HasColumnName(nameof(StockImportAllocation.Price));
                b.Property(x => x.Qty_Requested).HasColumnName(nameof(StockImportAllocation.Qty_Requested));
                b.Property(x => x.Qty_Import_ForAllocation).HasColumnName(nameof(StockImportAllocation.Qty_Import_ForAllocation));
                b.Property(x => x.Qty_Allocation).HasColumnName(nameof(StockImportAllocation.Qty_Allocation));
                b.Property(x => x.Allocation_Order).HasColumnName(nameof(StockImportAllocation.Allocation_Order));
                b.Property(x => x.AllocationStep).HasColumnName(nameof(StockImportAllocation.AllocationStep)).HasMaxLength(StockImportAllocationConsts.AllocationStepMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(StockImportAllocation.Note)).HasMaxLength(StockImportAllocationConsts.NoteMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SaleOrdersSapImport>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SaleOrdersSAPImport", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.MaterialCode).HasColumnName("GolfaCode").HasMaxLength(SaleOrdersSapImportConsts.SONoMaxLength);
                b.Property(x => x.ModelName).HasColumnName(nameof(SaleOrdersSapImport.ModelName)).HasMaxLength(MaterialConsts.ModelMaxLength);
                b.Property(x => x.SOType).HasColumnName(nameof(SaleOrdersSapImport.SOType)).HasMaxLength(SaleOrdersSapImportConsts.SONoMaxLength);

                b.Property(x => x.SONo).HasColumnName(nameof(SaleOrdersSapImport.SONo)).HasMaxLength(SaleOrdersSapImportConsts.SONoMaxLength);
                //b.Property(x => x.DONo).HasColumnName(nameof(SaleOrdersSapImport.DONo)).HasMaxLength(SaleOrdersSapImportConsts.DONoMaxLength);
                //b.Property(x => x.DODate).HasColumnName(nameof(SaleOrdersSapImport.DODate));
                //b.Property(x => x.DONote).HasColumnName(nameof(SaleOrdersSapImport.DONote)).HasMaxLength(SaleOrdersSapImportConsts.DONoteMaxLength);
                b.Property(x => x.SOSAPNo).HasColumnName(nameof(SaleOrdersSapImport.SOSAPNo)).HasMaxLength(SaleOrdersSapImportConsts.SOSAPNoMaxLength);
                b.Property(x => x.DOSAPNo).HasColumnName(nameof(SaleOrdersSapImport.DOSAPNo)).HasMaxLength(SaleOrdersSapImportConsts.DOSAPNoMaxLength);
                b.Property(x => x.BillingNo).HasColumnName(nameof(SaleOrdersSapImport.BillingNo)).HasMaxLength(SaleOrdersSapImportConsts.BillingNoMaxLength);
                b.Property(x => x.InvoiceNo).HasColumnName(nameof(SaleOrdersSapImport.InvoiceNo)).HasMaxLength(SaleOrdersSapImportConsts.InvoiceNoMaxLength);
                b.Property(x => x.InvoiceDate).HasColumnName(nameof(SaleOrdersSapImport.InvoiceDate));
                b.Property(x => x.Note).HasColumnName(nameof(SaleOrdersSapImport.Note)).HasMaxLength(SaleOrdersSapImportConsts.NoteMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(SaleOrdersSapImport.FileName)).HasMaxLength(SaleOrdersSapImportConsts.FileNameMaxLength);
                b.Property(x => x.ImportKey).HasColumnName(nameof(SaleOrdersSapImport.ImportKey));
                b.Property(x => x.IsDeleted).HasColumnName(nameof(SaleOrdersSapImport.IsDeleted));

                b.Property(x => x.GICLandingCost).HasColumnName(nameof(SaleOrdersSapImport.GICLandingCost));

                b.Property(x => x.GICAmountLandingCost).HasColumnName(nameof(SaleOrdersSapImport.GICAmountLandingCost));

                b.Property(x => x.GICPORNo).HasColumnName(nameof(SaleOrdersSapImport.GICPORNo)).HasMaxLength(SaleOrdersSapImportConsts.GICPORNoMaxLength);

                b.Property(x => x.GICPRNo).HasColumnName(nameof(SaleOrdersSapImport.GICPRNo)).HasMaxLength(SaleOrdersSapImportConsts.GICPRNoMaxLength);

                b.Property(x => x.GICGivNo).HasColumnName(nameof(SaleOrdersSapImport.GICGivNo)).HasMaxLength(SaleOrdersSapImportConsts.GICGivNoMaxLength);

                b.Property(x => x.GICGivDate).HasColumnName(nameof(SaleOrdersSapImport.GICGivDate));

                b.Property(x => x.GICSalesPIC).HasColumnName(nameof(SaleOrdersSapImport.GICSalesPIC)).HasMaxLength(SaleOrdersSapImportConsts.GICSalesPICMaxLength);

                b.Property(x => x.GICLocation).HasColumnName(nameof(SaleOrdersSapImport.GICLocation)).HasMaxLength(SaleOrdersSapImportConsts.GICLocationMaxLength);

                b.Property(x => x.GICReservationNo).HasColumnName(nameof(SaleOrdersSapImport.GICReservationNo)).HasMaxLength(SaleOrdersSapImportConsts.GICReservationNoMaxLength);

                b.Property(x => x.GICAssetClass).HasColumnName(nameof(SaleOrdersSapImport.GICAssetClass)).HasMaxLength(SaleOrdersSapImportConsts.GICAssetClassMaxLength);

                b.Property(x => x.GICMainAssetCode).HasColumnName(nameof(SaleOrdersSapImport.GICMainAssetCode)).HasMaxLength(SaleOrdersSapImportConsts.GICMainAssetCodeMaxLength);

                b.Property(x => x.GICSubAssetCode).HasColumnName(nameof(SaleOrdersSapImport.GICSubAssetCode)).HasMaxLength(SaleOrdersSapImportConsts.GICSubAssetCodeMaxLength);

                b.Property(x => x.GICAssetName).HasColumnName(nameof(SaleOrdersSapImport.GICAssetName)).HasMaxLength(SaleOrdersSapImportConsts.GICAssetNameMaxLength);

                b.Property(x => x.GICNo).HasColumnName(nameof(SaleOrdersSapImport.GICNo)).HasMaxLength(SaleOrdersSapImportConsts.GICAssetNameMaxLength);

                b.Property(x => x.Disposed).HasColumnName(nameof(SaleOrdersSapImport.Disposed)).HasMaxLength(SaleOrdersSapImportConsts.GICAssetNameMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PurchaseOrdersSapImport>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "PurchaseOrdersSAPImport", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.PONo).HasColumnName(nameof(PurchaseOrdersSapImport.PONo)).HasMaxLength(PurchaseOrdersSapImportConsts.PONoMaxLength);
                b.Property(x => x.POSAPNo).HasColumnName(nameof(PurchaseOrdersSapImport.POSAPNo)).HasMaxLength(PurchaseOrdersSapImportConsts.POSAPNoMaxLength);
                b.Property(x => x.POSAPDate).HasColumnName(nameof(PurchaseOrdersSapImport.POSAPDate));
                b.Property(x => x.ImportKey).HasColumnName(nameof(PurchaseOrdersSapImport.ImportKey));
                b.Property(x => x.Note).HasColumnName(nameof(SaleOrdersSapImport.Note)).HasMaxLength(SaleOrdersSapImportConsts.NoteMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(SaleOrdersSapImport.FileName)).HasMaxLength(SaleOrdersSapImportConsts.FileNameMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PurchaseOrderLockShipment>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "PurchaseOrderLockShipment", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();

                b.Property(x => x.PODetailId).HasColumnName(nameof(PurchaseOrderLockShipment.PODetailId)).IsRequired();
                b.Property(x => x.DPODetailId).HasColumnName(nameof(PurchaseOrderLockShipment.DPODetailId)).IsRequired();
                b.Property(x => x.MaterialCode).HasColumnName(nameof(PurchaseOrderLockShipment.MaterialCode)).HasMaxLength(PurchaseOrderLockShipmentConsts.MaterialCodeMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(PurchaseOrderLockShipment.Qty));
                b.Property(x => x.QtyDisposed).HasColumnName(nameof(PurchaseOrderLockShipment.QtyDisposed));
                b.Property(x => x.QtyNeed).HasColumnName(nameof(PurchaseOrderLockShipment.QtyNeed));
                b.Property(x => x.Note).HasColumnName(nameof(PurchaseOrderLockShipment.Note)).HasMaxLength(PurchaseOrderLockShipmentConsts.NoteMaxLength);
                b.Property(x => x.PONo).HasColumnName(nameof(PurchaseOrderLockShipment.PONo)).HasMaxLength(PurchaseOrderLockShipmentConsts.PONoMaxLength);
                b.Property(x => x.DPONo).HasColumnName(nameof(PurchaseOrderLockShipment.DPONo)).HasMaxLength(PurchaseOrderLockShipmentConsts.DPONoMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<MaterialStockLockShipment>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "MaterialStock_LockShipment", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.ConfigureCustomExtendedAuditing();
                b.Property(x => x.GolfaCode).HasColumnName(nameof(MaterialStockLockShipment.GolfaCode)).IsRequired().HasMaxLength(MaterialStockLockShipmentConsts.GolfaCodeMaxLength);
                b.Property(x => x.LockOnOrder).HasColumnName(nameof(MaterialStockLockShipment.LockOnOrder));
                b.Property(x => x.StockOnOrder).HasColumnName(nameof(MaterialStockLockShipment.StockOnOrder));
                b.Property(x => x.Note).HasColumnName(nameof(MaterialStockLockShipment.Note)).HasMaxLength(MaterialStockLockShipmentConsts.NoteMaxLength);
            });

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<HistoryTracking>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "HistoryTracking", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.TrackingType).HasColumnName(nameof(HistoryTracking.TrackingType)).IsRequired().HasMaxLength(HistoryTrackingConsts.TrackingTypeMaxLength);
                b.Property(x => x.Action).HasColumnName(nameof(HistoryTracking.Action)).IsRequired().HasMaxLength(HistoryTrackingConsts.ActionMaxLength);
                b.Property(x => x.ObjectId).HasColumnName(nameof(HistoryTracking.ObjectId)).HasMaxLength(HistoryTrackingConsts.ObjectIdMaxLength);
                b.Property(x => x.GolfaCode).HasColumnName(nameof(HistoryTracking.GolfaCode)).HasMaxLength(HistoryTrackingConsts.GolfaCodeMaxLength);
                b.Property(x => x.Model).HasColumnName(nameof(HistoryTracking.Model)).HasMaxLength(HistoryTrackingConsts.ModelMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(HistoryTracking.Qty)).HasPrecision(18, 2);
                b.Property(x => x.PreviousValue).HasColumnName("Previous_Value").HasPrecision(18, 2);
                b.Property(x => x.NextValue).HasColumnName("Next_Value").HasPrecision(18, 2);
                b.Property(x => x.StockId).HasColumnName(nameof(HistoryTracking.StockId));
                b.Property(x => x.StockName).HasColumnName(nameof(HistoryTracking.StockName)).HasMaxLength(HistoryTrackingConsts.StockNameMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(HistoryTracking.Note)).HasMaxLength(HistoryTrackingConsts.NoteMaxLength);

                b.HasDiscriminator(x => x.TrackingType)
                    .HasValue<HistoryTracking>("General")
                    .HasValue<MaterialHistoryTracking>("Material")
                    .HasValue<StockHistoryTracking>("STOCK")
                    .HasValue<AssetHistoryTracking>(EntityTypes.AssetRequest);

                builder.Entity<AssetHistoryTracking>(b =>
                {
                    b.HasBaseType<HistoryTracking>();
                    b.Property(x => x.AssetId).HasColumnName(nameof(AssetHistoryTracking.AssetId));
                    b.HasIndex(x => x.AssetId);
                });
                builder.Entity<MaterialHistoryTracking>(b =>
                {
                    b.HasBaseType<HistoryTracking>();
                });

                builder.Entity<StockHistoryTracking>(b =>
                {
                    b.HasBaseType<HistoryTracking>();
                });
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SpoBatchRequest>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SPO_BatchRequest", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.RequestNo).HasColumnName(nameof(SpoBatchRequest.RequestNo)).IsRequired().HasMaxLength(SpoBatchRequestConsts.RequestNoMaxLength);
                b.Property(x => x.ImportType).HasColumnName(nameof(SpoBatchRequest.ImportType)).IsRequired().HasMaxLength(SpoBatchRequestConsts.ImportTypeMaxLength);
                b.Property(x => x.FileName).HasColumnName(nameof(SpoBatchRequest.FileName)).HasMaxLength(SpoBatchRequestConsts.FileNameMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(SpoBatchRequest.Note)).HasMaxLength(SpoBatchRequestConsts.NoteMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(SpoBatchRequest.Status)).HasMaxLength(SpoBatchRequestConsts.StatusMaxLength);

                b.HasMany(x => x.SpoBatchRequestDetails).WithOne().HasForeignKey(y => y.RequestId);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SpoBatchRequestDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "SPO_BatchRequest_Detail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.RequestId).HasColumnName(nameof(SpoBatchRequestDetail.RequestId)).IsRequired();
                b.Property(x => x.SPOCode).HasColumnName(nameof(SpoBatchRequestDetail.SPOCode)).HasMaxLength(SpoBatchRequestDetailConsts.SPOCodeMaxLength);
                b.Property(x => x.GolfaCode).HasColumnName(nameof(SpoBatchRequestDetail.GolfaCode)).HasMaxLength(SpoBatchRequestDetailConsts.GolfaCodeMaxLength);
                b.Property(x => x.Action).HasColumnName(nameof(SpoBatchRequestDetail.Action)).HasMaxLength(SpoBatchRequestDetailConsts.ActionMaxLength);
                b.Property(x => x.ActionDate).HasColumnName(nameof(SpoBatchRequestDetail.ActionDate));
                b.Property(x => x.Note).HasColumnName(nameof(SpoBatchRequestDetail.Note)).HasMaxLength(SpoBatchRequestDetailConsts.NoteMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<CfgDiscountRatio>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "CFG_DiscountRatio", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Approval_Type).HasColumnName(nameof(CfgDiscountRatio.Approval_Type)).HasMaxLength(CfgDiscountRatioConsts.Approval_TypeMaxLength);
                b.Property(x => x.Product_Type).HasColumnName(nameof(CfgDiscountRatio.Product_Type)).HasMaxLength(CfgDiscountRatioConsts.Product_TypeMaxLength);
                b.Property(x => x.AccountClassify).HasColumnName(nameof(CfgDiscountRatio.AccountClassify)).HasMaxLength(CfgDiscountRatioConsts.AccountClassifyMaxLength);
                b.Property(x => x.Value_Min).HasColumnName(nameof(CfgDiscountRatio.Value_Min));
                b.Property(x => x.Value_Max).HasColumnName(nameof(CfgDiscountRatio.Value_Max));
                b.Property(x => x.DiscountRatio).HasColumnName(nameof(CfgDiscountRatio.DiscountRatio));
                b.Property(x => x.Note).HasColumnName(nameof(CfgDiscountRatio.Note));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Asset>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "Assets", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.AssetName).HasColumnName(nameof(Asset.AssetName)).IsRequired().HasMaxLength(AssetConsts.AssetNameMaxLength);
                //b.Property(x => x.Title).HasColumnName(nameof(Asset.Title)).HasMaxLength(AssetConsts.TitleMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(Asset.Description)).HasMaxLength(AssetConsts.DescriptionMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(Asset.Qty)).HasColumnType("int");
                b.Property(x => x.AssetClass).HasColumnName(nameof(Asset.AssetClass)).HasMaxLength(AssetConsts.AssetClassMaxLength);
                b.Property(x => x.AssetType).HasColumnName(nameof(Asset.AssetType)).HasMaxLength(AssetConsts.AssetTypeMaxLength);
                b.Property(x => x.WarehouseId).HasColumnName(nameof(Asset.WarehouseId));
                b.Property(x => x.WarehouseName).HasColumnName(nameof(Asset.WarehouseName)).HasMaxLength(AssetConsts.WarehouseNameMaxLength);
                b.Property(x => x.SalePIC).HasColumnName(nameof(Asset.SalePIC)).HasMaxLength(AssetConsts.SalePICMaxLength);
                b.Property(x => x.CodeMain).HasColumnName(nameof(Asset.CodeMain)).HasMaxLength(AssetConsts.CodeMainMaxLength);
                b.Property(x => x.CodeSub).HasColumnName(nameof(Asset.CodeSub)).HasMaxLength(AssetConsts.CodeSubMaxLength);
                b.Property(x => x.CodeMain_AF).HasColumnName(nameof(Asset.CodeMain_AF)).HasMaxLength(AssetConsts.CodeMain_AFMaxLength);
                b.Property(x => x.CodeSub_AF).HasColumnName(nameof(Asset.CodeSub_AF)).HasMaxLength(AssetConsts.CodeSub_AFMaxLength);
                b.Property(x => x.NumberOfComponent).HasColumnName(nameof(Asset.NumberOfComponent));
                b.Property(x => x.POR).HasColumnName(nameof(Asset.POR)).HasMaxLength(AssetConsts.PORMaxLength);
                b.Property(x => x.PR).HasColumnName(nameof(Asset.PR)).HasMaxLength(AssetConsts.PRMaxLength);
                b.Property(x => x.GIV).HasColumnName(nameof(Asset.GIV)).HasMaxLength(AssetConsts.GIVMaxLength);
                b.Property(x => x.MaterialCode).HasColumnName(nameof(Asset.MaterialCode)).HasMaxLength(AssetConsts.MaterialCodeMaxLength);
                b.Property(x => x.ModelName).HasColumnName(nameof(Asset.ModelName)).HasMaxLength(AssetConsts.ModelNameMaxLength);
                b.Property(x => x.Unit).HasColumnName(nameof(Asset.Unit)).HasMaxLength(AssetConsts.UnitMaxLength);

                b.Property(x => x.Price).HasColumnName(nameof(Asset.Price)).HasColumnType("decimal(18,4)");
                b.Property(x => x.InvoicePrice).HasColumnName(nameof(Asset.InvoicePrice)).HasColumnType("decimal(18,4)"); // Added
                b.Property(x => x.Amount).HasColumnName(nameof(Asset.Amount)).HasColumnType("decimal(18,4)");

                b.Property(x => x.Division).HasColumnName(nameof(Asset.Division)).HasMaxLength(AssetConsts.DivisionMaxLength);
                b.Property(x => x.Department).HasColumnName(nameof(Asset.Department)).HasMaxLength(AssetConsts.DepartmentMaxLength);
                b.Property(x => x.SectionSAP).HasColumnName(nameof(Asset.SectionSAP)).HasMaxLength(AssetConsts.SectionSAPMaxLength);
                b.Property(x => x.REG).HasColumnName(nameof(Asset.REG)).HasMaxLength(AssetConsts.REGMaxLength);
                b.Property(x => x.Source).HasColumnName(nameof(Asset.Source)).HasMaxLength(AssetConsts.SourceMaxLength);
                b.Property(x => x.RequestNo).HasColumnName(nameof(Asset.RequestNo)).HasMaxLength(AssetConsts.RequestNoMaxLength);

                b.Property(x => x.Section).HasColumnName(nameof(Asset.Section)).HasMaxLength(AssetConsts.SectionMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(Asset.Status)).HasMaxLength(AssetConsts.StatusMaxLength);
                b.Property(x => x.LendingInformation).HasColumnName(nameof(Asset.LendingInformation)).HasMaxLength(AssetConsts.LendingInformationMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(Asset.Note));
                b.Property(x => x.IsPendingApproval).HasColumnName(nameof(Asset.IsPendingApproval));

                b.HasMany(x => x.HistoryTrackings).WithOne().HasForeignKey(x => x.AssetId).OnDelete(DeleteBehavior.Cascade);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<AssetRequest>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "AssetRequest", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.RequestNo).HasColumnName(nameof(AssetRequest.RequestNo)).IsRequired().HasMaxLength(AssetRequestConsts.RequestNoMaxLength);
                b.Property(x => x.Title).HasColumnName(nameof(AssetRequest.Title)).HasMaxLength(AssetRequestConsts.TitleMaxLength);
                b.Property(x => x.RequestType).HasColumnName(nameof(AssetRequest.RequestType)).HasMaxLength(AssetRequestConsts.RequestTypeMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(AssetRequest.Status)).HasMaxLength(AssetRequestConsts.StatusMaxLength);
                b.Property(x => x.WarehouseSrcId).HasColumnName(nameof(AssetRequest.WarehouseSrcId));
                b.Property(x => x.WarehouseSrcName).HasColumnName(nameof(AssetRequest.WarehouseSrcName)).HasMaxLength(AssetRequestConsts.WarehouseSrcNameMaxLength);
                b.Property(x => x.WarehouseDestId).HasColumnName(nameof(AssetRequest.WarehouseDestId));
                b.Property(x => x.WarehouseDestName).HasColumnName(nameof(AssetRequest.WarehouseDestName)).HasMaxLength(AssetRequestConsts.WarehouseDestNameMaxLength);
                b.Property(x => x.PIC_Src).HasColumnName(nameof(AssetRequest.PIC_Src)).HasMaxLength(AssetRequestConsts.PIC_SrcMaxLength);
                b.Property(x => x.PIC_Dest).HasColumnName(nameof(AssetRequest.PIC_Dest)).HasMaxLength(AssetRequestConsts.PIC_DestMaxLength);
                b.Property(x => x.Lending_CustomerTaxCode).HasColumnName(nameof(AssetRequest.Lending_CustomerTaxCode)).HasMaxLength(AssetRequestConsts.Lending_CustomerTaxCodeMaxLength);
                b.Property(x => x.Lending_ReturnDate).HasColumnName(nameof(AssetRequest.Lending_ReturnDate));
                b.Property(x => x.Lending_ActualReturnDate).HasColumnName(nameof(AssetRequest.Lending_ActualReturnDate));
                b.Property(x => x.RequestOwner).HasColumnName(nameof(AssetRequest.RequestOwner)).HasMaxLength(AssetRequestConsts.RequestOwnerMaxLength);
                b.Property(x => x.SubmittedDate).HasColumnName(nameof(AssetRequest.SubmittedDate));
                b.Property(x => x.Note).HasColumnName(nameof(AssetRequest.Note));
                b.Property(x => x.CurrentApprovalRouteInstanceId).HasColumnName(nameof(AssetRequest.CurrentApprovalRouteInstanceId));
                b.Property(x => x.CurrentApprovalStepSequence).HasColumnName(nameof(AssetRequest.CurrentApprovalStepSequence));
                b.Property(x => x.CurrentApproverRoleName).HasColumnName(nameof(AssetRequest.CurrentApproverRoleName)).HasMaxLength(AssetRequestConsts.CurrentApproverRoleNameMaxLength);
                b.Property(x => x.CurrentApproverRoleCode).HasColumnName(nameof(AssetRequest.CurrentApproverRoleCode)).HasMaxLength(AssetRequestConsts.CurrentApproverRoleCodeMaxLength);
                b.Property(x => x.CurrentApprovalRound).HasColumnName(nameof(AssetRequest.CurrentApprovalRound)).HasMaxLength(AssetRequestConsts.CurrentApprovalRoundMaxLength);
                b.Property(x => x.Lending_CustomerName).HasColumnName(nameof(AssetRequest.Lending_CustomerName)).HasMaxLength(AssetRequestConsts.Lending_CustomerNameMaxLength);
                b.Property(x => x.AgreementNo).HasColumnName(nameof(AssetRequest.AgreementNo)).HasMaxLength(AssetRequestConsts.AgreementNoMaxLength);
                b.Property(x => x.LendingInvoiceNo).HasColumnName(nameof(AssetRequest.LendingInvoiceNo)).HasMaxLength(AssetRequestConsts.LendingInvoiceNoMaxLength);
                b.Property(x => x.ReturnInvoiceNo).HasColumnName(nameof(AssetRequest.ReturnInvoiceNo)).HasMaxLength(AssetRequestConsts.ReturnInvoiceNoMaxLength);
                b.Property(x => x.Lending_Target).HasColumnName(nameof(AssetRequest.Lending_Target)).HasMaxLength(AssetRequestConsts.Lending_Target);
                b.Property(x => x.ExtensionDoc).HasColumnName(nameof(AssetRequest.ExtensionDoc)).HasMaxLength(AssetRequestConsts.ExtensionDoc);
                b.Property(x => x.DeliveryNote).HasColumnName(nameof(AssetRequest.DeliveryNote)).HasMaxLength(AssetRequestConsts.DeliveryNote);
                b.Property(x => x.Audit_FromDate).HasColumnName(nameof(AssetRequest.Audit_FromDate));
                b.Property(x => x.Audit_ToDate).HasColumnName(nameof(AssetRequest.Audit_ToDate));

                b.HasMany(x => x.ApprovalRoutes).WithOne().HasForeignKey(x => x.AssetRequestId).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.ApprovalHistories).WithOne().HasForeignKey(x => x.AssetRequestId).OnDelete(DeleteBehavior.Cascade);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<AssetRequestDetail>(b =>
            {
                b.ToTable(QuoteFlowConsts.DbTablePrefix + "AssetRequestDetail", QuoteFlowConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.RequestId).HasColumnName(nameof(AssetRequestDetail.RequestId)).IsRequired();
                b.Property(x => x.AssetId).HasColumnName(nameof(AssetRequestDetail.AssetId));
                b.Property(x => x.AssetName).HasColumnName(nameof(AssetRequestDetail.AssetName)).IsRequired().HasMaxLength(AssetRequestDetailConsts.AssetNameMaxLength);
                b.Property(x => x.IsDeleted).HasColumnName(nameof(AssetRequestDetail.IsDeleted)).IsRequired();
                b.Property(x => x.Status).HasColumnName(nameof(AssetRequestDetail.Status)).HasMaxLength(AssetRequestDetailConsts.StatusMaxLength);
                b.Property(x => x.AuditNote).HasColumnName(nameof(AssetRequestDetail.AuditNote)).HasMaxLength(AssetRequestDetailConsts.NoteMaxLength);
                b.Property(x => x.AuditResult).HasColumnName(nameof(AssetRequestDetail.AuditResult)).HasMaxLength(AssetRequestDetailConsts.AuditResultMaxLength);
                b.Property(x => x.Note).HasColumnName(nameof(AssetRequestDetail.Note)).HasMaxLength(AssetRequestDetailConsts.NoteMaxLength);
                b.Property(x => x.Counted_Quantity).HasColumnName(nameof(AssetRequestDetail.Counted_Quantity));
                b.Property(x => x.Variance).HasColumnName(nameof(AssetRequestDetail.Variance));
                b.Property(x => x.FAI_PIC).HasColumnName(nameof(AssetRequestDetail.FAI_PIC)).HasMaxLength(AssetRequestDetailConsts.FAI_PICMaxLength);
                b.Property(x => x.FAP_PIC).HasColumnName(nameof(AssetRequestDetail.FAP_PIC)).HasMaxLength(AssetRequestDetailConsts.FAP_PICMaxLength);
                b.Property(x => x.IA_PIC).HasColumnName(nameof(AssetRequestDetail.IA_PIC)).HasMaxLength(AssetRequestDetailConsts.IA_PICMaxLength);
                b.Property(x => x.AF_PIC).HasColumnName(nameof(AssetRequestDetail.AF_PIC)).HasMaxLength(AssetRequestDetailConsts.AF_PICMaxLength);
                //
                b.Property(x => x.AssetClass).HasColumnName(nameof(AssetRequestDetail.AssetClass)).HasMaxLength(AssetRequestDetailConsts.AssetClassMaxLength);
                b.Property(x => x.AssetType).HasColumnName(nameof(AssetRequestDetail.AssetType)).HasMaxLength(AssetRequestDetailConsts.AssetTypeMaxLength);
                b.Property(x => x.WarehouseId).HasColumnName(nameof(AssetRequestDetail.WarehouseId));
                b.Property(x => x.WarehouseName).HasColumnName(nameof(AssetRequestDetail.WarehouseName)).HasMaxLength(AssetRequestDetailConsts.WarehouseNameMaxLength);
                b.Property(x => x.SalePIC).HasColumnName(nameof(AssetRequestDetail.SalePIC)).HasMaxLength(AssetRequestDetailConsts.SalePICMaxLength);
                b.Property(x => x.CodeMain).HasColumnName(nameof(AssetRequestDetail.CodeMain)).HasMaxLength(AssetRequestDetailConsts.CodeMainMaxLength);
                b.Property(x => x.CodeSub).HasColumnName(nameof(AssetRequestDetail.CodeSub)).HasMaxLength(AssetRequestDetailConsts.CodeSubMaxLength);
                b.Property(x => x.CodeMain_AF).HasColumnName(nameof(AssetRequestDetail.CodeMain_AF)).HasMaxLength(AssetRequestDetailConsts.CodeMain_AFMaxLength);
                b.Property(x => x.CodeSub_AF).HasColumnName(nameof(AssetRequestDetail.CodeSub_AF)).HasMaxLength(AssetRequestDetailConsts.CodeSub_AFMaxLength);
                b.Property(x => x.NumberOfComponent).HasColumnName(nameof(AssetRequestDetail.NumberOfComponent));
                b.Property(x => x.POR).HasColumnName(nameof(AssetRequestDetail.POR)).HasMaxLength(AssetRequestDetailConsts.PORMaxLength);
                b.Property(x => x.PR).HasColumnName(nameof(AssetRequestDetail.PR)).HasMaxLength(AssetRequestDetailConsts.PRMaxLength);
                b.Property(x => x.GIV).HasColumnName(nameof(AssetRequestDetail.GIV)).HasMaxLength(AssetRequestDetailConsts.GIVMaxLength);
                b.Property(x => x.MaterialCode).HasColumnName(nameof(AssetRequestDetail.MaterialCode)).HasMaxLength(AssetRequestDetailConsts.MaterialCodeMaxLength);
                b.Property(x => x.ModelName).HasColumnName(nameof(AssetRequestDetail.ModelName)).HasMaxLength(AssetRequestDetailConsts.ModelNameMaxLength);
                b.Property(x => x.Unit).HasColumnName(nameof(AssetRequestDetail.Unit)).HasMaxLength(AssetRequestDetailConsts.UnitMaxLength);
                b.Property(x => x.Price).HasColumnName(nameof(AssetRequestDetail.Price)).HasColumnType("decimal(18,4)");
                b.Property(x => x.InvoicePrice).HasColumnName(nameof(AssetRequestDetail.InvoicePrice)).HasColumnType("decimal(18,4)");
                b.Property(x => x.Amount).HasColumnName(nameof(AssetRequestDetail.Amount)).HasColumnType("decimal(18,4)");
                b.Property(x => x.Division).HasColumnName(nameof(AssetRequestDetail.Division)).HasMaxLength(AssetRequestDetailConsts.DivisionMaxLength);
                b.Property(x => x.Department).HasColumnName(nameof(AssetRequestDetail.Department)).HasMaxLength(AssetRequestDetailConsts.DepartmentMaxLength);
                b.Property(x => x.Section).HasColumnName(nameof(AssetRequestDetail.Section)).HasMaxLength(AssetRequestDetailConsts.SectionMaxLength);
                b.Property(x => x.REG).HasColumnName(nameof(AssetRequestDetail.REG)).HasMaxLength(AssetRequestDetailConsts.REGMaxLength);
                b.Property(x => x.Source).HasColumnName(nameof(AssetRequestDetail.Source)).HasMaxLength(AssetRequestDetailConsts.SourceMaxLength);
                b.Property(x => x.SectionSAP).HasColumnName(nameof(AssetRequestDetail.SectionSAP)).HasMaxLength(AssetRequestDetailConsts.SectionSAPMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(AssetRequestDetail.Description)).HasMaxLength(AssetRequestDetailConsts.DescriptionMaxLength);
                b.Property(x => x.Qty).HasColumnName(nameof(AssetRequestDetail.Qty));
                b.Property(x => x.AssetNote).HasColumnName(nameof(AssetRequestDetail.AssetNote));
                b.HasOne<AssetRequest>(x => x.Request)
                    .WithMany(x => x.Details)
                    .HasForeignKey(x => x.RequestId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne<Asset>(x => x.Asset)
                     .WithMany()
                     .HasForeignKey(x => x.AssetId)
                     .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
using QuoteFlow.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace QuoteFlow.Permissions;

public class QuoteFlowPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(QuoteFlowPermissions.GroupName);

        var dashboardGroup = myGroup.AddPermission(QuoteFlowPermissions.Dashboard.Default, L("Permission:Dashboard"));
        dashboardGroup.AddChild(QuoteFlowPermissions.Dashboard.SalesResultBasedOnPlan, L("Permission:Dashboard.SalesResultBasedOnPlan"));
        dashboardGroup.AddChild(QuoteFlowPermissions.Dashboard.SalesByMaterialGroup, L("Permission:Dashboard.SalesByMaterialGroup"));
        dashboardGroup.AddChild(QuoteFlowPermissions.Dashboard.SalesByBuyer, L("Permission:Dashboard.SalesByBuyer"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(QuoteFlowPermissions.MyPermission1, L("Permission:MyPermission1"));
        var generalPermission = myGroup.AddPermission(QuoteFlowPermissions.General.Default, L("Permission:General"));
        generalPermission.AddChild(QuoteFlowPermissions.General.FullAccessToSalesDimensions, L("Permission:FullAccessToSalesDimensions"));

        // Material Management
        var materialPermission = myGroup.AddPermission(QuoteFlowPermissions.Materials.Default, L("Permission:MaterialManagements"));
        materialPermission.AddChild(QuoteFlowPermissions.Materials.MaterialData, L("Permission:MaterialDatas"));
        materialPermission.AddChild(QuoteFlowPermissions.Materials.UploadMaterialData, L("Permission:UploadMaterialData"));
        materialPermission.AddChild(QuoteFlowPermissions.Materials.ViewPurchaseArea, L("Permission:ViewPurchaseArea"));
        materialPermission.AddChild(QuoteFlowPermissions.Materials.ViewStrategicPrice, L("Permission:ViewStrategicPrice"));
        materialPermission.AddChild(QuoteFlowPermissions.Materials.ExportMaterialMasterData, L("Permission:ExportMaterialMasterData"));

        var materialUploads = materialPermission.AddChild(QuoteFlowPermissions.Materials.Uploads.UploadDefault, L("Permission:Uploads"));
        materialUploads.AddChild(QuoteFlowPermissions.Materials.Uploads.NewMaterial, L("Permission:NewMaterial"));
        materialUploads.AddChild(QuoteFlowPermissions.Materials.Uploads.UpdatePrice, L("Permission:UpdatePrice"));
        materialUploads.AddChild(QuoteFlowPermissions.Materials.Uploads.UpdateMaterialWithoutPrice, L("Permission:UpdateMaterialWithoutPrice"));
        materialUploads.AddChild(QuoteFlowPermissions.Materials.Uploads.MaterialStatus, L("Permission:MaterialStatus"));
        materialUploads.AddChild(QuoteFlowPermissions.Materials.Uploads.Leadtime, L("Permission:Leadtime"));
        materialUploads.AddChild(QuoteFlowPermissions.Materials.Uploads.SapCode, L("Permission:SapCode"));
        materialUploads.AddChild(QuoteFlowPermissions.Materials.Uploads.InventoryPlanning, L("Permission:InventoryPlanning"));

        // Stock Management
        var materialStockPermission = myGroup.AddPermission(QuoteFlowPermissions.MaterialStocks.Default, L("Permission:StockManagements"));
        materialStockPermission.AddChild(QuoteFlowPermissions.MaterialStocks.MaterialStock, L("Permission:MaterialStock"));
        materialStockPermission.AddChild(QuoteFlowPermissions.MaterialStocks.UploadMaterialStock, L("Permission:UploadMaterialStock"));

        var materialStockUploads = materialStockPermission.AddChild(QuoteFlowPermissions.MaterialStocks.Uploads.UploadDefault, L("Permission:Uploads"));
        materialStockUploads.AddChild(QuoteFlowPermissions.MaterialStocks.Uploads.StockInventory, L("Permission:StockInventory"));
        materialStockUploads.AddChild(QuoteFlowPermissions.MaterialStocks.Uploads.StockTransfer, L("Permission:StockTransfer"));

        // Price Offer
        var priceOfferPermission = myGroup.AddPermission(QuoteFlowPermissions.PriceOffers.Default, L("Permission:PriceOffers"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.PriceOfferList, L("Permission:PriceOfferList"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.BatchRequest, L("Permission:BatchRequest"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.ApplySpecialInputPrice, L("Permission:ApplySpecialInputPrice"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.Close, L("Permission:Close"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.Cancel, L("Permission:Cancel"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.ExportAllDetails, L("Permission:ExportAllDetails"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.ConfirmProjectResult, L("Permission:ConfirmProjectResult"));

        var priceOfferUploads = priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.Uploads.UploadDefault, L("Permission:Uploads"));
        priceOfferUploads.AddChild(QuoteFlowPermissions.PriceOffers.Uploads.PriceOfferAP, L("Permission:PriceOfferAP"));
        priceOfferUploads.AddChild(QuoteFlowPermissions.PriceOffers.Uploads.PriceOfferDS, L("Permission:PriceOfferDS"));
        priceOfferUploads.AddChild(QuoteFlowPermissions.PriceOffers.Uploads.PriceOfferPP, L("Permission:PriceOfferPP"));
        priceOfferUploads.AddChild(QuoteFlowPermissions.PriceOffers.Uploads.PriceOfferNB, L("Permission:PriceOfferNB"));
        priceOfferUploads.AddChild(QuoteFlowPermissions.PriceOffers.Uploads.AddMoreItems, L("Permission:AddMoreItems"));
        priceOfferUploads.AddChild(QuoteFlowPermissions.PriceOffers.Uploads.ChangeItemProperties, L("Permission:ChangeItemProperties"));

        // MovingOrders
        var movingOrdersPermission = myGroup.AddPermission(QuoteFlowPermissions.MovingOrders.Default, L("Permission:MovingOrders"));

        // DPOs
        var dpoPermission = movingOrdersPermission.AddChild(QuoteFlowPermissions.MovingOrders.DPOs.DPODefault, L("Permission:DPOs"));
        dpoPermission.AddChild(QuoteFlowPermissions.MovingOrders.DPOs.Import, L("Permission:Import"));
        dpoPermission.AddChild(QuoteFlowPermissions.MovingOrders.DPOs.Delete, L("Permission:Delete"));
        dpoPermission.AddChild(QuoteFlowPermissions.MovingOrders.DPOs.CancelItems, L("Permission:CancelItems"));
        dpoPermission.AddChild(QuoteFlowPermissions.MovingOrders.DPOs.AddExtraFee, L("Permission:AddExtraFee"));
        dpoPermission.AddChild(QuoteFlowPermissions.MovingOrders.DPOs.ConfirmNote, L("Permission:ConfirmNote"));
        dpoPermission.AddChild(QuoteFlowPermissions.MovingOrders.DPOs.LockStock, L("Permission:LockStock"));
        dpoPermission.AddChild(QuoteFlowPermissions.MovingOrders.DPOs.LockOnOrderStock, L("Permission:LockOnOrderStock"));
        dpoPermission.AddChild(QuoteFlowPermissions.MovingOrders.DPOs.ConfirmReject, L("Permission:ConfirmReject"));

       
        //Sale Order
        var saleOrderPermission = myGroup.AddPermission(QuoteFlowPermissions.SaleOrders.Default, L("Permission:SaleOrders"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.Edit, L("Permission:Edit"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.Create, L("Permission:Create"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.Delete, L("Permission:Delete"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.DeleteItem, L("Permission:DeleteItem"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.ConfirmDelivery, L("Permission:ConfirmDelivery"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.Reopen, L("Permission:Reopen"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.AdjustDetailExtraFee, L("Permission:AdjustDetailExtraFee"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.EditSAPInfo, L("Permission:EditSAPInfo"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.ImportInternalUseChange, L("Permission:ImportInternalUseChange"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.ImportSAPSO, L("Permission:ImportSAPSO"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.ExportSAPData, L("Permission:ExportSAPData"));

        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.SAPLandingCost, L("Permission:SAPLandingCost"));

     
        //Stock Tracing
        var stockTracingPermission = myGroup.AddPermission(QuoteFlowPermissions.StockTracings.Default, L("Permission:StockTracings"));
        stockTracingPermission.AddChild(QuoteFlowPermissions.StockTracings.ImportData, L("Permission:ImportData"));
        stockTracingPermission.AddChild(QuoteFlowPermissions.StockTracings.Searching, L("Permission:Searching"));

        // Report
        //var reportsPermission = myGroup.AddPermission(QuoteFlowPermissions.Reports.Default, L("Permission:Reports"));
        //reportsPermission.AddChild(QuoteFlowPermissions.Reports.R25DPOReceivedByMaterialType, L("Permission:R25DPOReceivedByMaterialType"));
        //reportsPermission.AddChild(QuoteFlowPermissions.Reports.R24DPOProcessing, L("Permission:R24DPOProcessing"));
        //reportsPermission.AddChild(QuoteFlowPermissions.Reports.R21OverallStock, L("Permission:R21OverallStock"));
        //reportsPermission.AddChild(QuoteFlowPermissions.Reports.R15Inventory, L("Permission:R15Inventory"));
        //reportsPermission.AddChild(QuoteFlowPermissions.Reports.CustomerSaleReportGeneral, L("Permission:CustomerSaleReportGeneral"));
        //reportsPermission.AddChild(QuoteFlowPermissions.Reports.CustomerSaleReportDetail, L("Permission:CustomerSaleReportDetail"));

        // Master Data
        var masterDataPermission = myGroup.AddPermission(QuoteFlowPermissions.MasterDatas.Default, L("Permission:MasterDatas"));

        var storageLocationPermission = masterDataPermission.AddChild(QuoteFlowPermissions.MasterDatas.StorageLocation, L("Permission:StorageLocation"));
        storageLocationPermission.AddChild(QuoteFlowPermissions.MasterDatas.ViewStorageLocation, L("Permission:View"));
        storageLocationPermission.AddChild(QuoteFlowPermissions.MasterDatas.CreateStorageLocation, L("Permission:Create"));
        storageLocationPermission.AddChild(QuoteFlowPermissions.MasterDatas.EditStorageLocation, L("Permission:Edit"));
        storageLocationPermission.AddChild(QuoteFlowPermissions.MasterDatas.DeleteStorageLocation, L("Permission:Delete"));

        var currencyPermission = masterDataPermission.AddChild(QuoteFlowPermissions.MasterDatas.Currency, L("Permission:Currency"));
        currencyPermission.AddChild(QuoteFlowPermissions.MasterDatas.ViewCurrency, L("Permission:View"));
        currencyPermission.AddChild(QuoteFlowPermissions.MasterDatas.CreateCurrency, L("Permission:Create"));
        currencyPermission.AddChild(QuoteFlowPermissions.MasterDatas.EditCurrency, L("Permission:Edit"));
        currencyPermission.AddChild(QuoteFlowPermissions.MasterDatas.DeleteCurrency, L("Permission:Delete"));

        var materialGroupPermission = masterDataPermission.AddChild(QuoteFlowPermissions.MasterDatas.MaterialGroup, L("Permission:MaterialGroup"));
        materialGroupPermission.AddChild(QuoteFlowPermissions.MasterDatas.ViewMaterialGroup, L("Permission:View"));
        materialGroupPermission.AddChild(QuoteFlowPermissions.MasterDatas.CreateMaterialGroup, L("Permission:Create"));
        materialGroupPermission.AddChild(QuoteFlowPermissions.MasterDatas.EditMaterialGroup, L("Permission:Edit"));
        materialGroupPermission.AddChild(QuoteFlowPermissions.MasterDatas.DeleteMaterialGroup, L("Permission:Delete"));

        var buyerTypePermission = masterDataPermission.AddChild(QuoteFlowPermissions.MasterDatas.BuyerType, L("Permission:BuyerType"));
        buyerTypePermission.AddChild(QuoteFlowPermissions.MasterDatas.ViewBuyerType, L("Permission:View"));
        buyerTypePermission.AddChild(QuoteFlowPermissions.MasterDatas.CreateBuyerType, L("Permission:Create"));
        buyerTypePermission.AddChild(QuoteFlowPermissions.MasterDatas.EditBuyerType, L("Permission:Edit"));
        buyerTypePermission.AddChild(QuoteFlowPermissions.MasterDatas.DeleteBuyerType, L("Permission:Delete"));

        var buyerPermission = masterDataPermission.AddChild(QuoteFlowPermissions.MasterDatas.Buyer, L("Permission:Buyer"));
        buyerPermission.AddChild(QuoteFlowPermissions.MasterDatas.ViewBuyer, L("Permission:View"));
        buyerPermission.AddChild(QuoteFlowPermissions.MasterDatas.CreateBuyer, L("Permission:Create"));
        buyerPermission.AddChild(QuoteFlowPermissions.MasterDatas.EditBuyer, L("Permission:Edit"));
        buyerPermission.AddChild(QuoteFlowPermissions.MasterDatas.DeleteBuyer, L("Permission:Delete"));
        buyerPermission.AddChild(QuoteFlowPermissions.MasterDatas.AddMaterialGroup, L("Permission:AddMaterialGroup"));

        var customerPermission = masterDataPermission.AddChild(QuoteFlowPermissions.MasterDatas.Customer, L("Permission:Customer"));
        customerPermission.AddChild(QuoteFlowPermissions.MasterDatas.ViewCustomer, L("Permission:View"));
        customerPermission.AddChild(QuoteFlowPermissions.MasterDatas.CreateCustomer, L("Permission:Create"));
        customerPermission.AddChild(QuoteFlowPermissions.MasterDatas.EditCustomer, L("Permission:Edit"));
        customerPermission.AddChild(QuoteFlowPermissions.MasterDatas.DeleteCustomer, L("Permission:Delete"));

        var supplierPermission = masterDataPermission.AddChild(QuoteFlowPermissions.MasterDatas.Supplier, L("Permission:Supplier"));
        supplierPermission.AddChild(QuoteFlowPermissions.MasterDatas.ViewSupplier, L("Permission:View"));
        supplierPermission.AddChild(QuoteFlowPermissions.MasterDatas.CreateSupplier, L("Permission:Create"));
        supplierPermission.AddChild(QuoteFlowPermissions.MasterDatas.EditSupplier, L("Permission:Edit"));
        supplierPermission.AddChild(QuoteFlowPermissions.MasterDatas.DeleteSupplier, L("Permission:Delete"));

        var supplierBUPermission = masterDataPermission.AddChild(QuoteFlowPermissions.MasterDatas.SupplierBU, L("Permission:SupplierBU"));
        supplierBUPermission.AddChild(QuoteFlowPermissions.MasterDatas.ViewSupplierBU, L("Permission:View"));
        supplierBUPermission.AddChild(QuoteFlowPermissions.MasterDatas.ImportSupplierBU, L("Permission:Import"));
        supplierBUPermission.AddChild(QuoteFlowPermissions.MasterDatas.DeleteSupplierBU, L("Permission:Delete"));

        //FA Admin
        var faAdminPermission = myGroup.AddPermission(QuoteFlowPermissions.FAAdmins.Default, L("Permission:FAAdmins"));

        var saleTeamPermission = faAdminPermission.AddChild(QuoteFlowPermissions.FAAdmins.SaleTeam, L("Permission:SaleTeam"));
        saleTeamPermission.AddChild(QuoteFlowPermissions.FAAdmins.ViewSaleTeam, L("Permission:View"));
        saleTeamPermission.AddChild(QuoteFlowPermissions.FAAdmins.CreateSaleTeam, L("Permission:Create"));
        saleTeamPermission.AddChild(QuoteFlowPermissions.FAAdmins.EditSaleTeam, L("Permission:Edit"));
        saleTeamPermission.AddChild(QuoteFlowPermissions.FAAdmins.DeleteSaleTeam, L("Permission:Delete"));

        var systemConfigurationPermission = faAdminPermission.AddChild(QuoteFlowPermissions.FAAdmins.SystemConfiguration, L("Permission:SystemConfiguration"));
        systemConfigurationPermission.AddChild(QuoteFlowPermissions.FAAdmins.ViewSystemConfiguration, L("Permission:View"));
        systemConfigurationPermission.AddChild(QuoteFlowPermissions.FAAdmins.EditSystemConfiguration, L("Permission:Edit"));

        var buyerTargetPermission = faAdminPermission.AddChild(QuoteFlowPermissions.FAAdmins.BuyerTarget, L("Permission:BuyerTarget"));
        buyerTargetPermission.AddChild(QuoteFlowPermissions.FAAdmins.ViewBuyerTarget, L("Permission:View"));
        buyerTargetPermission.AddChild(QuoteFlowPermissions.FAAdmins.CreateBuyerTarget, L("Permission:Create"));
        buyerTargetPermission.AddChild(QuoteFlowPermissions.FAAdmins.EditBuyerTarget, L("Permission:Edit"));
        buyerTargetPermission.AddChild(QuoteFlowPermissions.FAAdmins.DeleteBuyerTarget, L("Permission:Delete"));
        var cfgDiscountRatioPermission = faAdminPermission.AddChild(QuoteFlowPermissions.FAAdmins.CfgDiscountRatio, L("Permission:CfgDiscountRatio"));
        cfgDiscountRatioPermission.AddChild(QuoteFlowPermissions.FAAdmins.ViewCfgDiscountRatio, L("Permission:ViewCfgDiscountRatio"));
        cfgDiscountRatioPermission.AddChild(QuoteFlowPermissions.FAAdmins.EditCfgDiscountRatio, L("Permission:EditCfgDiscountRatio"));
        var workflowConfiguration = myGroup.AddPermission(QuoteFlowPermissions.WorkflowConfigurations.Default, L("Permission:WorkflowConfiguration"));
        workflowConfiguration.AddChild(QuoteFlowPermissions.WorkflowConfigurations.View, L("Permission:View"));
        workflowConfiguration.AddChild(QuoteFlowPermissions.WorkflowConfigurations.Edit, L("Permission:Edit"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<QuoteFlowResource>(name);
    }
}
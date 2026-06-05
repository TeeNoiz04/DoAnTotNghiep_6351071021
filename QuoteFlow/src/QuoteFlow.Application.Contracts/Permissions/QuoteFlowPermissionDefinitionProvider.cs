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
        dashboardGroup.AddChild(QuoteFlowPermissions.Dashboard.POResultBasedOnPlan, L("Permission:Dashboard.POResultBasedOnPlan"));
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

        //Key Account
        var keyAccountPermission = myGroup.AddPermission(QuoteFlowPermissions.KeyAccounts.Default, L("Permission:KeyAccounts"));
        keyAccountPermission.AddChild(QuoteFlowPermissions.KeyAccounts.KeyAccountData, L("Permission:KeyAccountData"));
        keyAccountPermission.AddChild(QuoteFlowPermissions.KeyAccounts.ClassAdjustment, L("Permission:ClassAdjustment"));

        // Price Offer
        var priceOfferPermission = myGroup.AddPermission(QuoteFlowPermissions.PriceOffers.Default, L("Permission:PriceOffers"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.PriceOfferList, L("Permission:PriceOfferList"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.BatchRequest, L("Permission:BatchRequest"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.GeneralReport, L("Menu:PriceOffers:GeneralReport"));
        priceOfferPermission.AddChild(QuoteFlowPermissions.PriceOffers.DetailedReport, L("Menu:PriceOffers:DetailReport"));
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

        // GICs
        var gicPermission = movingOrdersPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.GICDefault, L("Permission:GICs"));

        // GICs.InternalUse
        var gicInternalUsePermission = gicPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.InternalUse.InternalUseDefault, L("Permission:InternalUse"));
        gicInternalUsePermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.InternalUse.Import, L("Permission:Import"));
        gicInternalUsePermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.InternalUse.Delete, L("Permission:Delete"));
        gicInternalUsePermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.InternalUse.CancelItems, L("Permission:CancelItems"));
        gicInternalUsePermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.InternalUse.AddExtraFee, L("Permission:AddExtraFee"));
        gicInternalUsePermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.InternalUse.ConfirmNote, L("Permission:ConfirmNote"));
        gicInternalUsePermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.InternalUse.LockStock, L("Permission:LockStock"));
        gicInternalUsePermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.InternalUse.LockOnOrderStock, L("Permission:LockOnOrderStock"));

        // GICs.GivingFOC
        var gicGivingFOCPermission = gicPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.GivingFOC.GivingFOCDefault, L("Permission:GivingFOC"));
        gicGivingFOCPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.GivingFOC.Import, L("Permission:Import"));
        gicGivingFOCPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.GivingFOC.Delete, L("Permission:Delete"));
        gicGivingFOCPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.GivingFOC.CancelItems, L("Permission:CancelItems"));
        gicGivingFOCPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.GivingFOC.AddExtraFee, L("Permission:AddExtraFee"));
        gicGivingFOCPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.GivingFOC.ConfirmNote, L("Permission:ConfirmNote"));
        gicGivingFOCPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.GivingFOC.LockStock, L("Permission:LockStock"));
        gicGivingFOCPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.GivingFOC.LockOnOrderStock, L("Permission:LockOnOrderStock"));

        // GICs.Warranty
        var gicWarrantyPermission = gicPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.Warranty.WarrantyDefault, L("Permission:Warranty"));
        gicWarrantyPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.Warranty.Import, L("Permission:Import"));
        gicWarrantyPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.Warranty.Delete, L("Permission:Delete"));
        gicWarrantyPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.Warranty.CancelItems, L("Permission:CancelItems"));
        gicWarrantyPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.Warranty.AddExtraFee, L("Permission:AddExtraFee"));
        gicWarrantyPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.Warranty.ConfirmNote, L("Permission:ConfirmNote"));
        gicWarrantyPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.Warranty.LockStock, L("Permission:LockStock"));
        gicWarrantyPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.Warranty.LockOnOrderStock, L("Permission:LockOnOrderStock"));

        // GICs.WriteOff
        var gicWriteOffPermission = gicPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.WriteOff.WriteOffDefault, L("Permission:WriteOff"));
        gicWriteOffPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.WriteOff.Import, L("Permission:Import"));
        gicWriteOffPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.WriteOff.Delete, L("Permission:Delete"));
        gicWriteOffPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.WriteOff.CancelItems, L("Permission:CancelItems"));
        gicWriteOffPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.WriteOff.AddExtraFee, L("Permission:AddExtraFee"));
        gicWriteOffPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.WriteOff.ConfirmNote, L("Permission:ConfirmNote"));
        gicWriteOffPermission.AddChild(QuoteFlowPermissions.MovingOrders.GICs.WriteOff.LockStock, L("Permission:LockStock"));

        // GKRs
        var gkrPermission = movingOrdersPermission.AddChild(QuoteFlowPermissions.MovingOrders.GKRs.GKRDefault, L("Permission:GKRs"));
        gkrPermission.AddChild(QuoteFlowPermissions.MovingOrders.GKRs.Import, L("Permission:Import"));
        gkrPermission.AddChild(QuoteFlowPermissions.MovingOrders.GKRs.Delete, L("Permission:Delete"));
        gkrPermission.AddChild(QuoteFlowPermissions.MovingOrders.GKRs.CancelItems, L("Permission:CancelItems"));
        gkrPermission.AddChild(QuoteFlowPermissions.MovingOrders.GKRs.AddExtraFee, L("Permission:AddExtraFee"));
        gkrPermission.AddChild(QuoteFlowPermissions.MovingOrders.GKRs.ConfirmNote, L("Permission:ConfirmNote"));
        gkrPermission.AddChild(QuoteFlowPermissions.MovingOrders.GKRs.LockStock, L("Permission:LockStock"));
        gkrPermission.AddChild(QuoteFlowPermissions.MovingOrders.GKRs.LockOnOrderStock, L("Permission:LockOnOrderStock"));

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
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.ExportReportData, L("Permission:ExportReportData"));
        saleOrderPermission.AddChild(QuoteFlowPermissions.SaleOrders.SAPLandingCost, L("Permission:SAPLandingCost"));

        //Special Input Price
        var specialInputPricePermission = myGroup.AddPermission(QuoteFlowPermissions.SpecialInputPrice.Default, L("Permission:SpecialInputPrice"));
        specialInputPricePermission.AddChild(QuoteFlowPermissions.SpecialInputPrice.Create, L("Permission:Create"));
        specialInputPricePermission.AddChild(QuoteFlowPermissions.SpecialInputPrice.Edit, L("Permission:Edit"));
        specialInputPricePermission.AddChild(QuoteFlowPermissions.SpecialInputPrice.Delete, L("Permission:Delete"));
        specialInputPricePermission.AddChild(QuoteFlowPermissions.SpecialInputPrice.Import, L("Permission:Import"));

        //Purchase Order
        var purchaseOrderPermission = myGroup.AddPermission(QuoteFlowPermissions.PurchaseOrders.Default, L("Permission:PurchaseOrders"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.Edit, L("Permission:Edit"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.Create, L("Permission:Create"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.Delete, L("Permission:Delete"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.DeleteItem, L("Permission:DeleteItem"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.ImportSAPPO, L("Permission:ImportSAPPO"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.ExportPOSAP, L("Permission:ExportPOSAP"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.ExportListPO, L("Permission:ExportListPO"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.PODataReport, L("Permission:PODataReport"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.ExportStandard, L("Permission:ExportStandard"));
        purchaseOrderPermission.AddChild(QuoteFlowPermissions.PurchaseOrders.ExportFASCM, L("Permission:ExportFASCM"));

        //Cargo
        var cargoDataPermission = myGroup.AddPermission(QuoteFlowPermissions.CargoDatas.Default, L("Permission:CargoDatas"));
        cargoDataPermission.AddChild(QuoteFlowPermissions.CargoDatas.ImportData, L("Menu:Cargo:CargoImport"));
        cargoDataPermission.AddChild(QuoteFlowPermissions.CargoDatas.CargoReport, L("Permission:CargoReport"));
        cargoDataPermission.AddChild(QuoteFlowPermissions.CargoDatas.Delete, L("Permission:Delete"));

        // Invoice
        var invoicePermission = myGroup.AddPermission(QuoteFlowPermissions.Shipments.Default, L("Permission:Shipments"));

        // Allocation Priority
        var allocationPriority = invoicePermission.AddChild(QuoteFlowPermissions.Shipments.AllocationPriority.AllocationPriorityDefault, L("Permission:AllocationPriority"));
        allocationPriority.AddChild(QuoteFlowPermissions.Shipments.AllocationPriority.Import, L("Permission:Import"));
        allocationPriority.AddChild(QuoteFlowPermissions.Shipments.AllocationPriority.Delete, L("Permission:Delete"));

        // Supplier Shipment
        var supplierShipments = invoicePermission.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.SupplierShipmentDefault, L("Permission:SupplierShipments"));
        supplierShipments.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.ViewDetail, L("Permission:ViewDetail"));
        supplierShipments.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.Import, L("Permission:Import"));
        supplierShipments.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.Confirm, L("Permission:Confirm"));
        supplierShipments.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.Delete, L("Permission:Delete"));
        supplierShipments.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.ExportPurchaseInvoice, L("Permission:ExportPurchaseInvoice"));
        supplierShipments.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.ExportInvoiceSAPData, L("Permission:ExportInvoiceSAPData"));
        supplierShipments.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.ExportInvoiceAllocation, L("Permission:ExportInvoiceAllocation"));
        var allocationDetails = supplierShipments.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.AllocationDetails.AllocationDetailDefault, L("Permission:AllocationDetails"));
        allocationDetails.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.AllocationDetails.View, L("Permission:View"));
        allocationDetails.AddChild(QuoteFlowPermissions.Shipments.SupplierShipments.AllocationDetails.Export, L("Permission:Export"));

        //PSI
        var psiPermission = myGroup.AddPermission(QuoteFlowPermissions.PSIs.Default, L("Permission:PSIs"));
        psiPermission.AddChild(QuoteFlowPermissions.PSIs.ImportData, L("Menu:PSI:List"));
        psiPermission.AddChild(QuoteFlowPermissions.PSIs.PSIReport, L("Permission:PSIReport"));

        //Stock Tracing
        var stockTracingPermission = myGroup.AddPermission(QuoteFlowPermissions.StockTracings.Default, L("Permission:StockTracings"));
        stockTracingPermission.AddChild(QuoteFlowPermissions.StockTracings.ImportData, L("Permission:ImportData"));
        stockTracingPermission.AddChild(QuoteFlowPermissions.StockTracings.Searching, L("Permission:Searching"));

        // Report
        var reportsPermission = myGroup.AddPermission(QuoteFlowPermissions.Reports.Default, L("Permission:Reports"));
        reportsPermission.AddChild(QuoteFlowPermissions.Reports.R25DPOReceivedByMaterialType, L("Permission:R25DPOReceivedByMaterialType"));
        reportsPermission.AddChild(QuoteFlowPermissions.Reports.R24DPOProcessing, L("Permission:R24DPOProcessing"));
        reportsPermission.AddChild(QuoteFlowPermissions.Reports.R21OverallStock, L("Permission:R21OverallStock"));
        reportsPermission.AddChild(QuoteFlowPermissions.Reports.R15Inventory, L("Permission:R15Inventory"));
        //reportsPermission.AddChild(QuoteFlowPermissions.Reports.R06Sale, L("Permission:R06Sale"));
        //reportsPermission.AddChild(QuoteFlowPermissions.Reports.R05Sale, L("Permission:R05Sale"));
        reportsPermission.AddChild(QuoteFlowPermissions.Reports.CustomerSaleReportGeneral, L("Permission:CustomerSaleReportGeneral"));
        reportsPermission.AddChild(QuoteFlowPermissions.Reports.CustomerSaleReportDetail, L("Permission:CustomerSaleReportDetail"));

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
        //cfgDiscountRatioPermission.AddChild(QuoteFlowPermissions.FAAdmins.CreateCfgDiscountRatio, L("Permission:CreateCfgDiscountRatio"));
        cfgDiscountRatioPermission.AddChild(QuoteFlowPermissions.FAAdmins.EditCfgDiscountRatio, L("Permission:EditCfgDiscountRatio"));
        //cfgDiscountRatioPermission.AddChild(QuoteFlowPermissions.FAAdmins.DeleteCfgDiscountRatio, L("Permission:DeleteCfgDiscountRatio"));

        var workflowConfiguration = myGroup.AddPermission(QuoteFlowPermissions.WorkflowConfigurations.Default, L("Permission:WorkflowConfiguration"));
        workflowConfiguration.AddChild(QuoteFlowPermissions.WorkflowConfigurations.View, L("Permission:View"));
        workflowConfiguration.AddChild(QuoteFlowPermissions.WorkflowConfigurations.Edit, L("Permission:Edit"));

        //var cfgDiscountRatioPermission = myGroup.AddPermission(QuoteFlowPermissions.CfgDiscountRatios.Default, L("Permission:CfgDiscountRatios"));
        //cfgDiscountRatioPermission.AddChild(QuoteFlowPermissions.CfgDiscountRatios.Create, L("Permission:Create"));
        //cfgDiscountRatioPermission.AddChild(QuoteFlowPermissions.CfgDiscountRatios.Edit, L("Permission:Edit"));
        //cfgDiscountRatioPermission.AddChild(QuoteFlowPermissions.CfgDiscountRatios.Delete, L("Permission:Delete"));


        // FATAs
        var assetPermission = myGroup.AddPermission(QuoteFlowPermissions.Assets.Default, L("Permission:Assets"));
        var fataCategoryPermission = assetPermission.AddChild(QuoteFlowPermissions.Assets.FATACategory, L("Permission:FATACategory"));
        fataCategoryPermission.AddChild(QuoteFlowPermissions.Assets.ImportCreate, L("Permission:ImportCreate"));
        fataCategoryPermission.AddChild(QuoteFlowPermissions.Assets.ImportUpdate, L("Permission:ImportUpdate"));
        //fataCategoryPermission.AddChild(QuoteFlowPermissions.Assets.Delete, L("Permission:Delete"));
        fataCategoryPermission.AddChild(QuoteFlowPermissions.Assets.ViewLandingPrice, L("Permission:ViewLandingPrice"));
        fataCategoryPermission.AddChild(QuoteFlowPermissions.Assets.ExportReport, L("Permission:ExportReport"));
        
        // 0. Asset Approve Request
        var assetApprovePermission = assetPermission.AddChild(QuoteFlowPermissions.Assets.FATAApproveRequests, L("Permission:AssetApproveRequests"));

        //// 1. Asset Request
        //var assetRequestPermission = myGroup.AddPermission(QuoteFlowPermissions.Assets.Default, L("Permission:AssetRequests"));
        //assetRequestPermission.AddChild(QuoteFlowPermissions.Assets.ViewList, L("Permission:ViewList"));
        //assetRequestPermission.AddChild(QuoteFlowPermissions.AssetRequests.ViewSale, L("Permission:ViewSale"));

       
        // 2. PIC Transfer
        var picTransferPermission = assetPermission.AddChild(QuoteFlowPermissions.Assets.PICTransfer, L("Permission:PICTransfer"));
        picTransferPermission.AddChild(QuoteFlowPermissions.Assets.CreatePICTransfer, L("Permission:Create"));
        picTransferPermission.AddChild(QuoteFlowPermissions.Assets.ViewFullPICPICTransfer, L("Permission:ViewFullPIC"));
        //picTransferPermission.AddChild(QuoteFlowPermissions.Assets.EditPICTransfer, L("Permission:Edit"));
        //picTransferPermission.AddChild(QuoteFlowPermissions.Assets.DeletePICTransfer, L("Permission:Delete"));

        // 3. Asset Lending
        var assetLendingPermission = assetPermission.AddChild(QuoteFlowPermissions.Assets.FATALending, L("Permission:AssetLending"));
        assetLendingPermission.AddChild(QuoteFlowPermissions.Assets.CreateFATALending, L("Permission:Create"));
        assetLendingPermission.AddChild(QuoteFlowPermissions.Assets.ExtendFATALending, L("Permission:Extend"));
        //assetLendingPermission.AddChild(QuoteFlowPermissions.Assets.Edit, L("Permission:Edit"));
        //assetLendingPermission.AddChild(QuoteFlowPermissions.Assets.Delete, L("Permission:Delete"));

        // 4. Asset Liquidation
        var assetLiquidationPermission = assetPermission.AddChild(QuoteFlowPermissions.Assets.FATALiquidation, L("Permission:AssetLiquidation"));
        assetLiquidationPermission.AddChild(QuoteFlowPermissions.Assets.CreateFATALiquidation, L("Permission:Create"));
        //assetLiquidationPermission.AddChild(QuoteFlowPermissions.Assets.Edit, L("Permission:Edit"));
        //assetLiquidationPermission.AddChild(QuoteFlowPermissions.Assets.Delete, L("Permission:Delete"));

        // 5. Asset Audit
        var assetAuditPermission = assetPermission.AddChild(QuoteFlowPermissions.Assets.FATAAudit, L("Permission:AssetAudit"));
        assetAuditPermission.AddChild(QuoteFlowPermissions.Assets.CreateFATAAudit, L("Permission:Create"));
        assetAuditPermission.AddChild(QuoteFlowPermissions.Assets.ExportFATAAudit, L("Permission:Export"));
        assetAuditPermission.AddChild(QuoteFlowPermissions.Assets.ImportFATAAudit, L("Permission:Import"));

        // 6. Asset StockTransfer
        var assetStockTransferPermission = assetPermission.AddChild(QuoteFlowPermissions.Assets.FATAStockTransfer, L("Permission:AssetStockTransfer"));
        assetStockTransferPermission.AddChild(QuoteFlowPermissions.Assets.CreateFATAStockTransfer, L("Permission:Create"));
        assetStockTransferPermission.AddChild(QuoteFlowPermissions.Assets.ViewFullPICFATAStockTransfer, L("Permission:ViewFullPIC"));
        //assetStockTransferPermission.AddChild(QuoteFlowPermissions.Assets.EditFATAStockTransfer, L("Permission:Edit"));
        //assetStockTransferPermission.AddChild(QuoteFlowPermissions.Assets.DeleteFATAStockTransfer, L("Permission:Delete"));

        // 7. Asset Reports
        //var assetReportsPermission = assetPermission.AddChild(QuoteFlowPermissions.Assets.FATAReports, L("Permission:AssetReports"));

        //var assetRequestDetailPermission = myGroup.AddPermission(QuoteFlowPermissions.AssetRequestDetails.Default, L("Permission:AssetRequestDetails"));
        //assetRequestDetailPermission.AddChild(QuoteFlowPermissions.AssetRequestDetails.Create, L("Permission:Create"));
        //assetRequestDetailPermission.AddChild(QuoteFlowPermissions.AssetRequestDetails.Edit, L("Permission:Edit"));
        //assetRequestDetailPermission.AddChild(QuoteFlowPermissions.AssetRequestDetails.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<QuoteFlowResource>(name);
    }
}
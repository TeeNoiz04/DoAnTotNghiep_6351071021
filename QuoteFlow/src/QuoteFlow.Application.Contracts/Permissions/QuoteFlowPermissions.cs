namespace QuoteFlow.Permissions;

public static class QuoteFlowPermissions
{
    public const string GroupName = "QuoteFlow";

    public static class Dashboard
    {
        public const string Default = GroupName + ".Dashboard";
        public const string SalesResultBasedOnPlan = Default + ".SalesResultBasedOnPlan";
        public const string POResultBasedOnPlan = Default + ".POResultBasedOnPlan";
        public const string SalesByMaterialGroup = Default + ".SalesByMaterialGroup";
        public const string SalesByBuyer = Default + ".SalesByBuyer";
    }

    public static class General
    {
        public const string Default = GroupName + ".General";
        public const string FullAccessToSalesDimensions = Default + ".FullBuyerAccess";
    }

    public static class Materials
    {
        // Menu
        public const string Default = GroupName + ".MaterialManagements";
        public const string MaterialData = Default + ".MaterialData";
        public const string UploadMaterialData = Default + ".UploadMaterialData";

        // Smaller views
        public const string ViewPurchaseArea = Default + ".ViewPurchaseArea";
        public const string ViewStrategicPrice = Default + ".ViewStrategicPrice";
        public const string ExportMaterialMasterData = Default + ".ExportMaterialMasterData";

        // Uploads
        public static class Uploads
        {
            public const string UploadDefault = Default + ".Uploads";
            public const string NewMaterial = UploadDefault + ".NewMaterial";
            public const string UpdatePrice = UploadDefault + ".UpdatePrice";
            public const string UpdateMaterialWithoutPrice = UploadDefault + ".UpdateMaterialWithoutPrice";
            public const string MaterialStatus = UploadDefault + ".MaterialStatus";
            public const string Leadtime = UploadDefault + ".Leadtime";
            public const string SapCode = UploadDefault + ".SapCode";
            public const string InventoryPlanning = UploadDefault + ".InventoryPlanning";
        }
    }

    public static class MaterialStocks
    {
        // Menu
        public const string Default = GroupName + ".StockManagements";
        public const string MaterialStock = Default + ".MaterialStock";
        public const string UploadMaterialStock = Default + ".UploadMaterialStock";

        // Uploads
        public static class Uploads
        {
            public const string UploadDefault = Default + ".Uploads";
            public const string StockInventory = UploadDefault + ".StockInventory";
            public const string StockTransfer = UploadDefault + ".StockTransfer";
        }
    }

    public static class KeyAccounts
    {
        // Menu
        public const string Default = GroupName + ".KeyAccounts";
        public const string KeyAccountData = Default + ".KeyAccountData";
        public const string ClassAdjustment = Default + ".ClassAdjustment";
    }

    public static class PriceOffers
    {
        // Menu
        public const string Default = GroupName + ".PriceOffers";
        public const string PriceOfferList = Default + ".PriceOfferList";
        public const string BatchRequest = Default + ".BatchRequest";
        public const string GeneralReport = Default + ".GeneralReport";
        public const string DetailedReport = Default + ".DetailedReport";

        // Features
        public const string ApplySpecialInputPrice = Default + ".ApplySpecialInputPrice";
        public const string Close = Default + ".Close";
        public const string Cancel = Default + ".Cancel";
        public const string ExportAllDetails = Default + ".ExportAllDetails";
        public const string ConfirmProjectResult = Default + ".ConfirmProjectResult";

        // Uploads
        public static class Uploads
        {
            public const string UploadDefault = Default + ".Uploads";
            public const string PriceOfferAP = UploadDefault + ".PriceOfferAP";
            public const string PriceOfferDS = UploadDefault + ".PriceOfferDS";
            public const string PriceOfferPP = UploadDefault + ".PriceOfferPP";
            public const string PriceOfferNB = UploadDefault + ".PriceOfferNB";
            public const string AddMoreItems = UploadDefault + ".AddMoreItems";
            public const string ChangeItemProperties = Default + ".ChangeItemProperties";
        }
    }

    public static class MovingOrders
    {
        public const string Default = GroupName + ".MovingOrders";

        public static class DPOs
        {
            public const string DPODefault = Default + ".DPOs";
            public const string Import = Default + ".Import";
            public const string Delete = Default + ".Delete";
            public const string CancelItems = Default + ".CancelItems";
            public const string AddExtraFee = Default + ".AddExtraFee";
            public const string ConfirmNote = Default + ".ConfirmNote";
            public const string LockStock = Default + ".LockStock";
            public const string LockOnOrderStock = Default + ".LockOnOrderStock";
            public const string ConfirmReject = Default + ".ConfirmReject";
        }

        public static class GICs
        {
            public const string GICDefault = Default + ".GICs";
            public static class InternalUse
            {
                public const string InternalUseDefault = GICDefault + ".InternalUse";
                public const string Import = InternalUseDefault + ".Import";
                public const string Delete = InternalUseDefault + ".Delete";
                public const string CancelItems = InternalUseDefault + ".CancelItems";
                public const string AddExtraFee = InternalUseDefault + ".AddExtraFee";
                public const string ConfirmNote = InternalUseDefault + ".ConfirmNote";
                public const string LockStock = InternalUseDefault + ".LockStock";
                public const string LockOnOrderStock = InternalUseDefault + ".LockOnOrderStock";
            }

            public static class GivingFOC
            {
                public const string GivingFOCDefault = GICDefault + ".GivingFOC";
                public const string Import = GivingFOCDefault + ".Import";
                public const string Delete = GivingFOCDefault + ".Delete";
                public const string CancelItems = GivingFOCDefault + ".CancelItems";
                public const string AddExtraFee = GivingFOCDefault + ".AddExtraFee";
                public const string ConfirmNote = GivingFOCDefault + ".ConfirmNote";
                public const string LockStock = GivingFOCDefault + ".LockStock";
                public const string LockOnOrderStock = GivingFOCDefault + ".LockOnOrderStock";
            }

            public static class Warranty
            {
                public const string WarrantyDefault = GICDefault + ".Warranty";
                public const string Import = WarrantyDefault + ".Import";
                public const string Delete = WarrantyDefault + ".Delete";
                public const string CancelItems = WarrantyDefault + ".CancelItems";
                public const string AddExtraFee = WarrantyDefault + ".AddExtraFee";
                public const string ConfirmNote = WarrantyDefault + ".ConfirmNote";
                public const string LockStock = WarrantyDefault + ".LockStock";
                public const string LockOnOrderStock = WarrantyDefault + ".LockOnOrderStock";
            }

            public static class WriteOff
            {
                public const string WriteOffDefault = GICDefault + ".WriteOff";
                public const string Import = WriteOffDefault + ".Import";
                public const string Delete = WriteOffDefault + ".Delete";
                public const string CancelItems = WriteOffDefault + ".CancelItems";
                public const string AddExtraFee = WriteOffDefault + ".AddExtraFee";
                public const string ConfirmNote = WriteOffDefault + ".ConfirmNote";
                public const string LockStock = WriteOffDefault + ".LockStock";
            }
        }

        public static class GKRs
        {
            public const string GKRDefault = Default + ".GKRs";
            public const string Import = GKRDefault + ".Import";
            public const string Delete = GKRDefault + ".Delete";
            public const string CancelItems = GKRDefault + ".CancelItems";
            public const string AddExtraFee = GKRDefault + ".AddExtraFee";
            public const string ConfirmNote = GKRDefault + ".ConfirmNote";
            public const string LockStock = GKRDefault + ".LockStock";
            public const string LockOnOrderStock = GKRDefault + ".LockOnOrderStock";
        }
    }

    public static class SaleOrders
    {
        public const string Default = GroupName + ".SaleOrders";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
        public const string DeleteItem = Default + ".DeleteItem";
        public const string ConfirmDelivery = Default + ".ConfirmDelivery";
        public const string Reopen = Default + ".Reopen";
        public const string AdjustDetailExtraFee = Default + ".AdjustDetailExtraFee";
        public const string EditSAPInfo = Default + ".EditSAPInfo";
        public const string ImportInternalUseChange = Default + ".ImportInternalUseChange";
        public const string ImportSAPSO = Default + ".ImportSAPSO";
        public const string ExportSAPData = Default + ".ExportSAPData";
        public const string ExportReportData = Default + ".ExportReportData";
        public const string SAPLandingCost = Default + ".SAPLandingCost";
    }

    public static class SpecialInputPrice
    {
        public const string Default = GroupName + ".SpecialInputPrice";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Import = Default + ".Import";
    }

    public static class PurchaseOrders
    {
        public const string Default = GroupName + ".PurchaseOrders";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
        public const string DeleteItem = Default + ".DeleteItem";
        public const string ImportSAPPO = Default + ".ImportSAPPO";

        // Single export
        public const string ExportStandard = Default + ".ExportStandard";
        public const string ExportFASCM = Default + ".ExportFASCM";

        // List export
        public const string ExportPOSAP = Default + ".ExportPOSAP";
        public const string ExportListPO = Default + ".ExportListPO";
        public const string PODataReport = Default + ".PODataReport";
    }

    public static class CargoDatas
    {
        public const string Default = GroupName + ".CargoDatas";
        public const string ImportData = Default + ".ImportData";
        public const string CargoReport = Default + ".CargoReport";
        public const string Delete = Default + ".Delete";
    }

    public static class Shipments
    {
        public const string Default = GroupName + ".Shipments";

        public static class AllocationPriority
        {
            public const string AllocationPriorityDefault = Default + ".AllocationPriority";
            public const string Import = AllocationPriorityDefault + ".Import";
            public const string Delete = AllocationPriorityDefault + ".Delete";
        }

        public static class SupplierShipments
        {
            public const string SupplierShipmentDefault = Default + ".SupplierShipments";
            public const string ViewDetail = SupplierShipmentDefault + ".ViewDetail";
            public const string Import = SupplierShipmentDefault + ".Import";
            public const string Confirm = SupplierShipmentDefault + ".Confirm";
            public const string Delete = SupplierShipmentDefault + ".Delete";
            public const string ExportPurchaseInvoice = SupplierShipmentDefault + ".ExportPurchaseInvoice";
            public const string ExportInvoiceSAPData = SupplierShipmentDefault + ".ExportInvoiceSAPData";
            public const string ExportInvoiceAllocation = SupplierShipmentDefault + ".ExportInvoiceAllocation";

            // eye icon
            public static class AllocationDetails
            {
                public const string AllocationDetailDefault = SupplierShipmentDefault + ".AllocationDetails";
                public const string View = AllocationDetailDefault + ".View";
                public const string Export = AllocationDetailDefault + ".Export";
            }
        }
    }

    // PSI
    public static class PSIs
    {
        public const string Default = GroupName + ".PSIs";
        public const string ImportData = Default + ".ImportData";
        public const string PSIReport = Default + ".PSIReport";
    }

    // Stock Tracing
    public static class StockTracings
    {
        public const string Default = GroupName + ".StockTracings";
        public const string ImportData = Default + ".ImportData";
        public const string Searching = Default + ".Searching";
    }

    // Reports
    public static class Reports
    {
        public const string Default = GroupName + ".Reports";

        public const string R25DPOReceivedByMaterialType = Default + ".R25DPOReceivedByMaterialType"; // R25
        public const string R24DPOProcessing = Default + ".R24DPOProcessing"; // R24
        public const string R21OverallStock = Default + ".R21OverallStock";
        public const string R15Inventory = Default + ".R15Inventory";
        public const string R06Sale = Default + ".R06Sale"; // R06
        public const string R05Sale = Default + ".R05Sale"; // R05

        // Not do yet
        public const string CustomerSaleReportGeneral = Default + ".CustomerSaleReportGeneral";
        public const string CustomerSaleReportDetail = Default + ".CustomerSaleReportDetail";
    }

    public static class MasterDatas
    {
        public const string Default = GroupName + ".MasterDatas";

        public const string StorageLocation = Default + ".StorageLocation";
        public const string ViewStorageLocation = StorageLocation + ".ViewStorageLocation";
        public const string CreateStorageLocation = StorageLocation + ".CreateStorageLocation";
        public const string EditStorageLocation = StorageLocation + ".EditStorageLocation";
        public const string DeleteStorageLocation = StorageLocation + ".DeleteStorageLocation";

        public const string Currency = Default + ".Currency";
        public const string ViewCurrency = Currency + ".ViewCurrency";
        public const string CreateCurrency = Currency + ".CreateCurrency";
        public const string EditCurrency = Currency + ".EditCurrency";
        public const string DeleteCurrency = Currency + ".DeleteCurrency";

        public const string MaterialGroup = Default + ".MaterialGroup";
        public const string ViewMaterialGroup = MaterialGroup + ".ViewMaterialGroup";
        public const string CreateMaterialGroup = MaterialGroup + ".CreateMaterialGroup";
        public const string EditMaterialGroup = MaterialGroup + ".EditMaterialGroup";
        public const string DeleteMaterialGroup = MaterialGroup + ".DeleteMaterialGroup";

        public const string BuyerType = Default + ".BuyerType";
        public const string ViewBuyerType = BuyerType + ".ViewBuyerType";
        public const string CreateBuyerType = BuyerType + ".CreateBuyerType";
        public const string EditBuyerType = BuyerType + ".EditBuyerType";
        public const string DeleteBuyerType = BuyerType + ".DeleteBuyerType";

        public const string Buyer = Default + ".Buyer";
        public const string ViewBuyer = Buyer + ".ViewBuyer";
        public const string CreateBuyer = Buyer + ".CreateBuyer";
        public const string EditBuyer = Buyer + ".EditBuyer";
        public const string DeleteBuyer = Buyer + ".DeleteBuyer";
        public const string AddMaterialGroup = Buyer + ".AddMaterialGroup";

        public const string Customer = Default + ".Customer";
        public const string ViewCustomer = Customer + ".ViewCustomer";
        public const string CreateCustomer = Customer + ".CreateCustomer";
        public const string EditCustomer = Customer + ".EditCustomer";
        public const string DeleteCustomer = Customer + ".DeleteCustomer";

        public const string Supplier = Default + ".Supplier";
        public const string ViewSupplier = Supplier + ".ViewSupplier";
        public const string CreateSupplier = Supplier + ".CreateSupplier";
        public const string EditSupplier = Supplier + ".EditSupplier";
        public const string DeleteSupplier = Supplier + ".DeleteSupplier";

        public const string SupplierBU = Default + ".SupplierBU";
        public const string ViewSupplierBU = SupplierBU + ".ViewSupplierBU";
        public const string ImportSupplierBU = SupplierBU + ".ImportSupplierBU";
        public const string DeleteSupplierBU = SupplierBU + ".DeleteSupplierBU";
    }

    public static class FAAdmins
    {
        public const string Default = GroupName + ".FAAdmins";

        public const string SaleTeam = Default + ".SaleTeam";
        public const string ViewSaleTeam = SaleTeam + ".ViewSaleTeam";
        public const string CreateSaleTeam = SaleTeam + ".CreateSaleTeam";
        public const string EditSaleTeam = SaleTeam + ".EditSaleTeam";
        public const string DeleteSaleTeam = SaleTeam + ".DeleteSaleTeam";

        public const string SystemConfiguration = Default + ".SystemConfiguration";
        public const string ViewSystemConfiguration = SystemConfiguration + ".ViewSystemConfiguration";
        public const string EditSystemConfiguration = SystemConfiguration + ".EditSystemConfiguration";

        public const string BuyerTarget = Default + ".BuyerTarget";
        public const string ViewBuyerTarget = BuyerTarget + ".ViewBuyerTarget";
        public const string CreateBuyerTarget = BuyerTarget + ".CreateBuyerTarget";
        public const string EditBuyerTarget = BuyerTarget + ".EditBuyerTarget";
        public const string DeleteBuyerTarget = BuyerTarget + ".DeleteBuyerTarget";

        public const string CfgDiscountRatio = Default + ".CfgDiscountRatio";
        public const string ViewCfgDiscountRatio = CfgDiscountRatio + ".ViewCfgDiscountRatio";
        public const string CreateCfgDiscountRatio = CfgDiscountRatio + ".CreateCfgDiscountRatio";
        public const string EditCfgDiscountRatio = CfgDiscountRatio + ".EditCfgDiscountRatio";
        public const string DeleteCfgDiscountRatio = CfgDiscountRatio + ".DeleteCfgDiscountRatio";
    }

    public static class WorkflowConfigurations
    {
        public const string Default = GroupName + ".WorkflowConfiguration";
        public const string View = Default + ".View";
        public const string Edit = Default + ".Edit";
    }

    //public static class CfgDiscountRatios
    //{
    //    public const string Default = GroupName + ".CfgDiscountRatios";
    //    public const string Edit = Default + ".Edit";
    //    public const string Create = Default + ".Create";
    //    public const string Delete = Default + ".Delete";
    //}

    public static class Assets
    {
        public const string Default = GroupName + ".Assets";
        // FATA Category
        public const string FATACategory = Default + ".FATACategory";
        public const string ImportUpdate = FATACategory + ".ImportUpdate";
        public const string ImportCreate = FATACategory + ".ImportCreate";
        public const string ViewLandingPrice = FATACategory + ".ViewLandingPrice";
        public const string ExportReport = FATACategory + ".ExportReport";

        // FATA Approve
        public const string FATAApproveRequests = Default + ".AssetApproveRequests";

        // PIC transfer 
        public const string PICTransfer = Default + ".PICTransfer";   
        public const string CreatePICTransfer = PICTransfer + ".Create";
        public const string ViewFullPICPICTransfer = PICTransfer + ".ViewFullPIC";
        //public const string EditPICTransfer = PICTransfer + ".Edit";
        //public const string DeletePICTransfer = PICTransfer + ".Delete";

        // FATA Lending
        public const string FATALending = Default + ".AssetLending";
        public const string CreateFATALending = FATALending + ".Create";
        public const string ExtendFATALending = FATALending + ".Extend";
        //public const string EditFATALending = FATALending + ".Edit";
        //public const string DeleteFATALending = FATALending + ".Delete";

        // FATA Liquidation
        public const string FATALiquidation = Default + ".AssetLiquidation";
        public const string CreateFATALiquidation = FATALiquidation + ".Create";
        //public const string EditFATALiquidation = FATALiquidation + ".Edit";        
        //public const string DeleteFATALiquidation = FATALiquidation + ".Delete";

        //FATA StockTransfer
        public const string FATAStockTransfer = Default + ".AssetStockTransfer";
        public const string CreateFATAStockTransfer = FATAStockTransfer + ".Create";
        public const string ViewFullPICFATAStockTransfer = FATAStockTransfer + ".ViewFullPIC";
        //public const string EditFATAStockTransfer = FATAStockTransfer + ".Edit";
        //public const string DeleteFATAStockTransfer = FATAStockTransfer + ".Delete";

        //FATA Audit
        public const string FATAAudit = Default + ".AssetAudit";
        public const string CreateFATAAudit = FATAAudit + ".Create";
        public const string ExportFATAAudit = FATAAudit + ".Export";
        public const string ImportFATAAudit = FATAAudit + ".Import";


        //// FATA Reports
        //public const string FATAReports = GroupName + ".AssetReports";

        // FATA RequestDetails
        public const string FATARequestDetails = GroupName + ".AssetRequestDetails";
        public const string CreateFATARequestDetails = FATARequestDetails + ".Create";
        //public const string EditFATARequestDetails = FATARequestDetails + ".Edit";
        //public const string DeleteFATARequestDetails = FATARequestDetails + ".Delete";

    }
    //public static class AssetApproveRequests
    //{
    //    public const string Default = GroupName + ".AssetApproveRequests";
    //}
    //public static class AssetRequests
    //{
    //    public const string Default = GroupName + ".AssetRequests";
    //    public const string ViewList = Default + ".ViewList";   //view landing price
    //    public const string ViewSale = Default + ".ViewSale";

    //}
    //public static class PICTransfer
    //{
    //    public const string Default = GroupName + ".PICTransfer";
    //    public const string Edit = Default + ".Edit";
    //    public const string Create = Default + ".Create";
    //    public const string Delete = Default + ".Delete";
    //}
    //public static class AssetLending
    //{
    //    public const string Default = GroupName + ".AssetLending";
    //    public const string Edit = Default + ".Edit";
    //    public const string Create = Default + ".Create";
    //    public const string Delete = Default + ".Delete";
    //    public const string Extended = Default + ".Extended";
    //}
    //public static class AssetLiquidation
    //{
    //    public const string Default = GroupName + ".AssetLiquidation";
    //    public const string Edit = Default + ".Edit";
    //    public const string Create = Default + ".Create";
    //    public const string Delete = Default + ".Delete";
    //}

    //public static class AssetStockTransfer
    //{
    //    public const string Default = GroupName + ".AssetStockTransfer";
    //    public const string Edit = Default + ".Edit";
    //    public const string Create = Default + ".Create";
    //    public const string Delete = Default + ".Delete";
    //}
    //public static class AssetAudit
    //{
    //    public const string Default = GroupName + ".AssetAudit";
    //    public const string Create = Default + ".Create";
    //    public const string Export = Default + ".Export";
    //    public const string Import = Default + ".Import";
    //}
    //public static class AssetReports
    //{
    //    public const string Default = GroupName + ".AssetReports";
    //}
    //public static class AssetRequestDetails
    //{
    //    public const string Default = GroupName + ".AssetRequestDetails";
    //    public const string Edit = Default + ".Edit";
    //    public const string Create = Default + ".Create";
    //    public const string Delete = Default + ".Delete";
    //}
}
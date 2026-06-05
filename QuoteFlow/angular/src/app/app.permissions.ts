export class AppPermissions {
  static GroupName = 'QuoteFlow';

  static readonly Dashboard = (() => {
    const Default = AppPermissions.GroupName + '.Dashboard';
    return {
      Default,
      SalesResultBasedOnPlan: Default + '.SalesResultBasedOnPlan',
      POResultBasedOnPlan: Default + '.POResultBasedOnPlan',
      SalesByMaterialGroup: Default + '.SalesByMaterialGroup',
      SalesByBuyer: Default + '.SalesByBuyer',
    };
  })();

  static readonly Materials = (() => {
    const Default = AppPermissions.GroupName + '.MaterialManagements';
    return {
      Default,
      MaterialData: Default + '.MaterialData',
      UploadMaterialData: Default + '.UploadMaterialData',

      ViewPurchaseArea: Default + '.ViewPurchaseArea',
      ViewStrategicPrice: Default + '.ViewStrategicPrice',
      ExportMaterialMasterData: Default + '.ExportMaterialMasterData',

      Uploads: (() => {
        const UploadDefault = Default + '.Uploads';
        return {
          UploadDefault,
          NewMaterial: UploadDefault + '.NewMaterial',
          UpdatePrice: UploadDefault + '.UpdatePrice',
          UpdateMaterialWithoutPrice: UploadDefault + '.UpdateMaterialWithoutPrice',
          MaterialStatus: UploadDefault + '.MaterialStatus',
          Leadtime: UploadDefault + '.Leadtime',
          SapCode: UploadDefault + '.SapCode',
          InventoryPlanning: UploadDefault + '.InventoryPlanning',
        };
      })(),
    };
  })();

  static readonly MaterialStocks = (() => {
    const Default = AppPermissions.GroupName + '.StockManagements';

    return {
      Default,
      MaterialStock: Default + '.MaterialStock',
      UploadMaterialStock: Default + '.UploadMaterialStock',
      Uploads: (() => {
        const UploadDefault = Default + '.Uploads';
        return {
          UploadDefault,
          StockInventory: UploadDefault + '.StockInventory',
          StockTransfer: UploadDefault + '.StockTransfer',
        };
      })(),
    };
  })();

  static readonly KeyAccounts = (() => {
    const Default = AppPermissions.GroupName + '.KeyAccounts';

    return {
      Default,
      KeyAccountData: Default + '.KeyAccountData',
      ClassAdjustment: Default + '.ClassAdjustment',
    };
  })();

  static readonly PriceOffers = (() => {
    const Default = AppPermissions.GroupName + '.PriceOffers';

    return {
      Default: Default,
      PriceOfferList: Default + '.PriceOfferList',
      GeneralReport: Default + '.GeneralReport',
      DetailedReport: Default + '.DetailedReport',
      BatchRequest: Default + '.BatchRequest',
      ApplySpecialInputPrice: Default + '.ApplySpecialInputPrice',
      Close: Default + '.Close',
      Cancel: Default + '.Cancel',
      ExportAllDetails: Default + '.ExportAllDetails',

      Uploads: (() => {
        const UploadDefault = Default + '.Uploads';

        return {
          UploadDefault,
          PriceOfferAP: UploadDefault + '.PriceOfferAP',
          PriceOfferDS: UploadDefault + '.PriceOfferDS',
          PriceOfferPP: UploadDefault + '.PriceOfferPP',
          PriceOfferNB: UploadDefault + '.PriceOfferNB',
          AddMoreItems: UploadDefault + '.AddMoreItems',
          ChangeItemProperties: Default + '.ChangeItemProperties',
        };
      })(),
    };
  })();

  static readonly MovingOrders = (() => {
    const Default = AppPermissions.GroupName + '.MovingOrders';

    return {
      Default,

      DPOs: {
        DPODefault: Default + '.DPOs',
        Import: Default + '.Import',
        Delete: Default + '.Delete',
        CancelItems: Default + '.CancelItems',
        AddExtraFee: Default + '.AddExtraFee',
        ConfirmNote: Default + '.ConfirmNote',
        LockStock: Default + '.LockStock',
        LockOnOrderStock: Default + '.LockOnOrderStock',
        ConfirmReject: Default + '.ConfirmReject',
      },

      GICs: {
        GICDefault: Default + '.GICs',

        InternalUse: {
          InternalUseDefault: Default + '.GICs.InternalUse',
          Import: Default + '.GICs.InternalUse.Import',
          Delete: Default + '.GICs.InternalUse.Delete',
          CancelItems: Default + '.GICs.InternalUse.CancelItems',
          AddExtraFee: Default + '.GICs.InternalUse.AddExtraFee',
          ConfirmNote: Default + '.GICs.InternalUse.ConfirmNote',
          LockStock: Default + '.GICs.InternalUse.LockStock',
          LockOnOrderStock: Default + '.GICs.InternalUse.LockOnOrderStock',
        },

        GivingFOC: {
          GivingFOCDefault: Default + '.GICs.GivingFOC',
          Import: Default + '.GICs.GivingFOC.Import',
          Delete: Default + '.GICs.GivingFOC.Delete',
          CancelItems: Default + '.GICs.GivingFOC.CancelItems',
          AddExtraFee: Default + '.GICs.GivingFOC.AddExtraFee',
          ConfirmNote: Default + '.GICs.GivingFOC.ConfirmNote',
          LockStock: Default + '.GICs.GivingFOC.LockStock',
          LockOnOrderStock: Default + '.GICs.GivingFOC.LockOnOrderStock',
        },

        Warranty: {
          WarrantyDefault: Default + '.GICs.Warranty',
          Import: Default + '.GICs.Warranty.Import',
          Delete: Default + '.GICs.Warranty.Delete',
          CancelItems: Default + '.GICs.Warranty.CancelItems',
          AddExtraFee: Default + '.GICs.Warranty.AddExtraFee',
          ConfirmNote: Default + '.GICs.Warranty.ConfirmNote',
          LockStock: Default + '.GICs.Warranty.LockStock',
          LockOnOrderStock: Default + '.GICs.Warranty.LockOnOrderStock',
        },

        WriteOff: {
          WriteOffDefault: Default + '.GICs.WriteOff',
          Import: Default + '.GICs.WriteOff.Import',
          Delete: Default + '.GICs.WriteOff.Delete',
          CancelItems: Default + '.GICs.WriteOff.CancelItems',
          AddExtraFee: Default + '.GICs.WriteOff.AddExtraFee',
          ConfirmNote: Default + '.GICs.WriteOff.ConfirmNote',
          LockStock: Default + '.GICs.WriteOff.LockStock',
        },
      },

      GKRs: {
        GKRDefault: Default + '.GKRs',
        Import: Default + '.GKRs.Import',
        Delete: Default + '.GKRs.Delete',
        CancelItems: Default + '.GKRs.CancelItems',
        AddExtraFee: Default + '.GKRs.AddExtraFee',
        ConfirmNote: Default + '.GKRs.ConfirmNote',
        LockStock: Default + '.GKRs.LockStock',
        LockOnOrderStock: Default + '.GKRs.LockOnOrderStock',
      },
    };
  })();

  static readonly SaleOrders = (() => {
    const Default = `${AppPermissions.GroupName}.SaleOrders`;
    return {
      Default,
      Edit: `${Default}.Edit`,
      Create: `${Default}.Create`,
      Delete: `${Default}.Delete`,
      DeleteItem: `${Default}.DeleteItem`,
      ConfirmDelivery: `${Default}.ConfirmDelivery`,
      Reopen: `${Default}.Reopen`,
      AdjustDetailExtraFee: `${Default}.AdjustDetailExtraFee`,
      EditSAPInfo: `${Default}.EditSAPInfo`,
      ImportInternalUseChange: `${Default}.ImportInternalUseChange`,
      ImportSAPSO: `${Default}.ImportSAPSO`,
      ExportSAPData: `${Default}.ExportSAPData`,
      ExportReportData: `${Default}.ExportReportData`,
    };
  })();

  static readonly SpecialInputPrice = (() => {
    const Default = AppPermissions.GroupName + '.SpecialInputPrice';
    return {
      Default,
      Create: Default + '.Create',
      Edit: Default + '.Edit',
      Delete: Default + '.Delete',
      Import: Default + '.Import',
      Export: Default + '.Export',
    };
  })();

  static readonly PurchaseOrders = (() => {
    const Default = AppPermissions.GroupName + '.PurchaseOrders';
    return {
      Default,
      Edit: Default + '.Edit',
      Create: Default + '.Create',
      Delete: Default + '.Delete',
      DeleteItem: Default + '.DeleteItem',
      ImportSAPPO: Default + '.ImportSAPPO',
      ExportStandard: Default + '.ExportStandard',
      ExportFASCM: Default + '.ExportFASCM',
      ExportPOSAP: Default + '.ExportPOSAP',
      ExportListPO: Default + '.ExportListPO',
      PODataReport: Default + '.PODataReport',
    };
  })();

  static readonly CargoDatas = (() => {
    const Default = AppPermissions.GroupName + '.CargoDatas';
    return {
      Default,
      ImportData: Default + '.ImportData',
      CargoReport: Default + '.CargoReport',
      Delete: Default + '.Delete',
    };
  })();

  static readonly Shipments = (() => {
    const Default = AppPermissions.GroupName + '.Shipments';

    return {
      Default,

      AllocationPriority: {
        AllocationPriorityDefault: Default + '.AllocationPriority',
        Import: Default + '.AllocationPriority.Import',
        Delete: Default + '.AllocationPriority.Delete',
      },

      SupplierShipments: {
        SupplierShipmentDefault: Default + '.SupplierShipments',
        ViewDetail: Default + '.SupplierShipments.ViewDetail',
        Import: Default + '.SupplierShipments.Import',
        Confirm: Default + '.SupplierShipments.Confirm',
        Delete: Default + '.SupplierShipments.Delete',
        ExportPurchaseInvoice: Default + '.SupplierShipments.ExportPurchaseInvoice',
        ExportInvoiceSAPData: Default + '.SupplierShipments.ExportInvoiceSAPData',
        ExportInvoiceAllocation: Default + '.SupplierShipments.ExportInvoiceAllocation',

        AllocationDetails: {
          AllocationDetailDefault: Default + '.SupplierShipments.AllocationDetails',
          View: Default + '.SupplierShipments.AllocationDetails.View',
          Export: Default + '.SupplierShipments.AllocationDetails.Export',
        },
      },
    };
  })();

  static readonly PSIs = (() => {
    const Default = AppPermissions.GroupName + '.PSIs';
    return {
      Default,
      ImportData: Default + '.ImportData',
      PSIReport: Default + '.PSIReport',
    };
  })();

  static readonly StockTracings = (() => {
    const Default = AppPermissions.GroupName + '.StockTracings';
    return {
      Default,
      ImportData: Default + '.ImportData',
      Searching: Default + '.Searching',
    };
  })();

  static readonly Reports = (() => {
    const Default = AppPermissions.GroupName + '.Reports';

    return {
      Default,
      R25DPOReceivedByMaterialType: Default + '.R25DPOReceivedByMaterialType', // R25
      R24DPOProcessing: Default + '.R24DPOProcessing', // R24
      R21OverallStock: Default + '.R21OverallStock',
      R15Inventory: Default + '.R15Inventory',
      R06Sale: Default + '.R06Sale',
      R05Sale: Default + '.R05Sale',
      CustomerSaleReportGeneral: Default + '.CustomerSaleReportGeneral',
      CustomerSaleReportDetail: Default + '.CustomerSaleReportDetail',
    };
  })();

  static readonly MasterDatas = (() => {
    const Default = AppPermissions.GroupName + '.MasterDatas';

    const StorageLocation = Default + '.StorageLocation';
    const Currency = Default + '.Currency';
    const MaterialGroup = Default + '.MaterialGroup';
    const BuyerType = Default + '.BuyerType';
    const Buyer = Default + '.Buyer';
    const Customer = Default + '.Customer';
    const Supplier = Default + '.Supplier';
    const SupplierBU = Default + '.SupplierBU';

    return {
      Default,

      StorageLocation,
      ViewStorageLocation: StorageLocation + '.ViewStorageLocation',
      CreateStorageLocation: StorageLocation + '.CreateStorageLocation',
      EditStorageLocation: StorageLocation + '.EditStorageLocation',
      DeleteStorageLocation: StorageLocation + '.DeleteStorageLocation',

      Currency,
      ViewCurrency: Currency + '.ViewCurrency',
      CreateCurrency: Currency + '.CreateCurrency',
      EditCurrency: Currency + '.EditCurrency',
      DeleteCurrency: Currency + '.DeleteCurrency',

      MaterialGroup,
      ViewMaterialGroup: MaterialGroup + '.ViewMaterialGroup',
      CreateMaterialGroup: MaterialGroup + '.CreateMaterialGroup',
      EditMaterialGroup: MaterialGroup + '.EditMaterialGroup',
      DeleteMaterialGroup: MaterialGroup + '.DeleteMaterialGroup',

      BuyerType,
      ViewBuyerType: BuyerType + '.ViewBuyerType',
      CreateBuyerType: BuyerType + '.CreateBuyerType',
      EditBuyerType: BuyerType + '.EditBuyerType',
      DeleteBuyerType: BuyerType + '.DeleteBuyerType',

      Buyer,
      ViewBuyer: Buyer + '.ViewBuyer',
      CreateBuyer: Buyer + '.CreateBuyer',
      EditBuyer: Buyer + '.EditBuyer',
      DeleteBuyer: Buyer + '.DeleteBuyer',
      AddMaterialGroup: Buyer + '.AddMaterialGroup',

      Customer,
      ViewCustomer: Customer + '.ViewCustomer',
      CreateCustomer: Customer + '.CreateCustomer',
      EditCustomer: Customer + '.EditCustomer',
      DeleteCustomer: Customer + '.DeleteCustomer',

      Supplier,
      ViewSupplier: Supplier + '.ViewSupplier',
      CreateSupplier: Supplier + '.CreateSupplier',
      EditSupplier: Supplier + '.EditSupplier',
      DeleteSupplier: Supplier + '.DeleteSupplier',

      SupplierBU,
      ViewSupplierBU: SupplierBU + '.ViewSupplierBU',
      ImportSupplierBU: SupplierBU + '.ImportSupplierBU',
      DeleteSupplierBU: SupplierBU + '.DeleteSupplierBU',
    };
  })();

  static readonly FAAdmins = (() => {
    const Default = AppPermissions.GroupName + '.FAAdmins';

    const SaleTeam = Default + '.SaleTeam';
    const SystemConfiguration = Default + '.SystemConfiguration';
    const BuyerTarget = Default + '.BuyerTarget';
    const CfgDiscountRatio = Default + '.CfgDiscountRatio';

    return {
      Default,

      SaleTeam,
      ViewSaleTeam: SaleTeam + '.ViewSaleTeam',
      CreateSaleTeam: SaleTeam + '.CreateSaleTeam',
      EditSaleTeam: SaleTeam + '.EditSaleTeam',
      DeleteSaleTeam: SaleTeam + '.DeleteSaleTeam',

      SystemConfiguration,
      ViewSystemConfiguration: SystemConfiguration + '.ViewSystemConfiguration',
      EditSystemConfiguration: SystemConfiguration + '.EditSystemConfiguration',

      BuyerTarget,
      ViewBuyerTarget: BuyerTarget + '.ViewBuyerTarget',
      CreateBuyerTarget: BuyerTarget + '.CreateBuyerTarget',
      EditBuyerTarget: BuyerTarget + '.EditBuyerTarget',
      DeleteBuyerTarget: BuyerTarget + '.DeleteBuyerTarget',

      CfgDiscountRatio,
      ViewCfgDiscountRatio: CfgDiscountRatio + '.ViewCfgDiscountRatio',
      CreateCfgDiscountRatio: CfgDiscountRatio + '.CreateCfgDiscountRatio',
      EditCfgDiscountRatio: CfgDiscountRatio + '.EditCfgDiscountRatio',
      DeleteCfgDiscountRatio: CfgDiscountRatio + '.DeleteCfgDiscountRatio',
    };
  })();

  static readonly WorkflowConfigurations = (() => {
    const Default = AppPermissions.GroupName + '.WorkflowConfiguration';
    return {
      Default: Default,
      View: Default + '.View',
      Edit: Default + '.Edit',
    };
  })();

  static readonly Assets = (() => {
    const Default = `${AppPermissions.GroupName}.Assets`;
    return {
      Default,
      // ImportCreate: `${Default}.ImportCreate`,
      // ImportUpdate: `${Default}.ImportUpdate`,
      // ViewLandingPrice: `${Default}.ViewLandingPrice`,
    };
  })();
  static readonly FATACategory = (() => {
    const Default = `${AppPermissions.GroupName}.Assets.FATACategory`;
    return {
      Default,
      ImportCreate: `${Default}.ImportCreate`,
      ImportUpdate: `${Default}.ImportUpdate`,
      ViewLandingPrice: `${Default}.ViewLandingPrice`,
      ExportReport: `${Default}.ExportReport`,
    };
  })();
  // static readonly AssetRequests = (() => {
  //   //transfer
  //   const Default = `${AppPermissions.GroupName}.AssetRequests`;
  //   return {
  //     Default,
  //     // Create: `${Default}.Create`,
  //     // Edit: `${Default}.Edit`,
  //     // Delete: `${Default}.Delete`,
  //     ViewList: `${Default}.ViewList`, //see full request
  //   };
  // })();

  static readonly AssetStockTransferRequests = (() => {
    //transfer
    const Default = `${AppPermissions.GroupName}.Assets.AssetStockTransfer`;
    return {
      Default,
      Create: `${Default}.Create`,
      Edit: `${Default}.Edit`,
      Delete: `${Default}.Delete`,
    };
  })();

  static readonly AssetApproveRequests = (() => {
    //transfer
    const Default = `${AppPermissions.GroupName}.Assets.AssetApproveRequests`;
    return {
      Default,
    };
  })();
  static readonly AssetRequestDetails = (() => {
    const Default = `${AppPermissions.GroupName}.Assets.AssetRequestDetails`;
    return {
      Default,
      Edit: `${Default}.Edit`,
      Create: `${Default}.Create`,
      Delete: `${Default}.Delete`,
    };
  })();
  static readonly PICTransfer = (() => {
    const Default = `${AppPermissions.GroupName}.Assets.PICTransfer`;
    return {
      Default,
      Create: `${Default}.Create`,
      Edit: `${Default}.Edit`,
      Delete: `${Default}.Delete`,
    };
  })();

  static readonly AssetLending = (() => {
    const Default = `${AppPermissions.GroupName}.Assets.AssetLending`;
    return {
      Default,
      Create: `${Default}.Create`,
      Edit: `${Default}.Edit`,
      Delete: `${Default}.Delete`,
    };
  })();

  static readonly AssetLiquidation = (() => {
    const Default = `${AppPermissions.GroupName}.Assets.AssetLiquidation`;
    return {
      Default,
      Create: `${Default}.Create`,
      Edit: `${Default}.Edit`,
      Delete: `${Default}.Delete`,
    };
  })();

  static readonly AssetAudit = (() => {
    const Default = `${AppPermissions.GroupName}.Assets.AssetAudit`;
    return {
      Default,
      Create: `${Default}.Create`,
      Export: `${Default}.Export`,
      Import: `${Default}.Import`,
    };
  })();

  static readonly AssetReports = (() => {
    const Default = `${AppPermissions.GroupName}.Assets.AssetReports`;
    return {
      Default,
    };
  })();
}

export class AppRoutes {
  static NEW = 'new';
  static EDIT = 'edit';
  static VIEW = 'view';
  static APPROVAL = 'approval';
  static readonly DETAILS_WITH_ID = (id: string) => `${id}`;
  static readonly HOME = {
    BASE: '',
    ORDER: 1,
    TITLE: 'Home',
  };
  static readonly DASHBOARD = {
    BASE: 'dashboard',
    ORDER: 1,
    TITLE: 'Dashboard',
    OVERVIEW: {
      BASE: 'base',
      ORDER: 1,
      TITLE: 'Overview | Dashboard',
    },
    APPROVAL: {
      BASE: 'approval',
      ORDER: 2,
      TITLE: 'Approval Dashboard | Dashboard',
    },
  };
  static readonly MATERIAL_STOCK = {
    BASE: 'materials',
    ORDER: 2,
    TITLE: 'Material Stock',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'Material Stock List | Material Stock',
    },
    MY_APPROVALS: {
      BASE: 'my-approvals',
      ORDER: 2,
      TITLE: 'My Approvals | Material Stock',
    },
    IMPORT_MATERIAL: {
      BASE: 'import-material',
      ORDER: 3,
      TITLE: 'Import Material | Material Stock',
    },
    IMPORT_MY_APPROVALS: {
      BASE: 'import-material-my-approvals',
      ORDER: 4,
      TITLE: 'Import Material - My Approvals | Material Stock',
    },
    DETAILS: {
      BASE: 'import-details',
      ORDER: 5,
      TITLE: 'Import Material Details | Material Stock',
    },
  };
  static readonly ASSET_MANAGEMENT = {
    BASE: 'asset-management',
    ORDER: 3.5,
    TITLE: 'Asset Management',

    ASSET_CATALOG: {
      BASE: 'asset-catalog',
      ORDER: 1,
      TITLE: 'Asset Catalog | Asset Management',
    },

    INTERNAL_WAREHOUSE_TRANSFER: {
      BASE: 'internal-warehouse-transfer',
      ORDER: 2,
      TITLE: 'Internal Warehouse Transfer | Asset Management',
      DETAILS: {
        BASE: 'details',
        ORDER: 2,
        TITLE: 'Stock Transfer Details',
      },
    },

    ASSET_APPROVAL: {
      BASE: 'asset-approval',
      ORDER: 10,
      TITLE: 'Asset Approval | Asset Management',
    },

    PIC_TRANSFER: {
      BASE: 'pic-transfer',
      ORDER: 3,
      TITLE: 'PIC Transfer | Asset Management',
      LIST: {
        BASE: 'list',
        ORDER: 1,
        TITLE: 'PIC Transfer List | Asset Management',
      },
      DETAILS: {
        BASE: 'details',
        ORDER: 2,
        TITLE: 'PIC Transfer Details | Asset Management',
      },
    },

    ASSET_LENDING: {
      BASE: 'asset-lending',
      ORDER: 4,
      TITLE: 'Asset Lending | Asset Management',
      LIST: {
        BASE: 'list',
        ORDER: 1,
        TITLE: 'Asset Lending List | Asset Management',
      },
      DETAILS: {
        BASE: 'details',
        ORDER: 2,
        TITLE: 'Asset Lending Details | Asset Management',
      },
    },

    ASSET_LIQUIDATION: {
      BASE: 'asset-liquidation',
      ORDER: 5,
      TITLE: 'Asset Liquidation | Asset Management',
      LIST: {
        BASE: 'list',
        ORDER: 1,
        TITLE: 'Asset Liquidation List | Asset Management',
      },
      DETAILS: {
        BASE: 'details',
        ORDER: 2,
        TITLE: 'Asset Liquidation Details | Asset Management',
      },
    },

    ASSET_AUDIT: {
      BASE: 'asset-audit',
      ORDER: 6,
      TITLE: 'FATA Inventory Checking | Asset Management',
      DETAILS: {
        BASE: 'details',
        ORDER: 2,
        TITLE: 'Stock Transfer Details',
      },
    },

    ASSET_REPORTS: {
      BASE: 'asset-reports',
      ORDER: 7,
      TITLE: 'Asset Reports | Asset Management',
    },
  };
  static readonly STOCK_MANAGEMENT = {
    BASE: 'stock-management',
    ORDER: 4,
    TITLE: 'Stock Management',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'Material Stock | Stock Management',
    },
    IMPORT_STOCK: {
      BASE: 'import-stock',
      ORDER: 2,
      TITLE: 'Upload Data | Stock Management',
    },
    REPORT_STOCK: {
      BASE: 'report-stock',
      ORDER: 3,
      TITLE: 'Overall Stock Report (R21) | Stock Management',
    },
    REAL_TIME_STOCK: {
      BASE: 'real-time-stock',
      ORDER: 4,
      TITLE: 'Real Time Stock | Stock Management',
    },
    DETAILS: {
      BASE: 'import-details',
      ORDER: 5,
      TITLE: 'Import Stock Details | Stock Management',
    },
  };
  static readonly KEY_ACCOUNTS = {
    BASE: 'key-accounts',
    ORDER: 5,
    TITLE: 'Key Accounts',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'Key Accounts List | Key Accounts',
    },
    MY_APPROVALS: {
      BASE: 'my-approvals',
      ORDER: 2,
      TITLE: 'My Approvals | Key Accounts',
    },
    CLASS_ADJUSTMENT: {
      BASE: 'class-adjustment',
      ORDER: 5,
      TITLE: 'Class Adjustment| Key Accounts',
    },
    GENERAL_REPORT: {
      BASE: 'general-report',
      ORDER: 3,
      TITLE: 'General Report| Key Accounts',
    },
    DETAILED_REPORT: {
      BASE: 'detailed-report',
      ORDER: 4,
      TITLE: 'Detailed Report| Key Accounts',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 6,
      TITLE: 'Key Account Details | Key Accounts',
    },
  };
  static readonly SPECIAL_PRICE_OFFERS = {
    BASE: 'price-offer',
    ORDER: 6,
    TITLE: 'Special Price Offers',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'Price Offers List | Special Price Offers',
    },
    MY_APPROVALS: {
      BASE: 'my-approvals',
      ORDER: 2,
      TITLE: 'My Approvals | Special Price Offers',
    },
    BATCH_REQUEST: {
      BASE: 'batch-request',
      ORDER: 3,
      TITLE: 'SPO Batch Request Management | Special Price Offers',
    },
    GENERAL_REPORT: {
      BASE: 'general-report',
      ORDER: 4,
      TITLE: 'General Report | Special Price Offers',
    },
    SPO_DETAILED_REPORT: {
      BASE: 'spo-detailed-report',
      ORDER: 5,
      TITLE: 'SPO Detailed Report(R04) | Special Price Offers',
    },
    DETAILED_REPORT: {
      BASE: 'detailed-report',
      ORDER: 5,
      TITLE: 'Detailed Report | Special Price Offers',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 6,
      TITLE: 'Price Offer Details | Special Price Offers',
    },
    DETAILS_BATCH_REQUEST: {
      BASE: 'batch-request-details',
      ORDER: 7,
      TITLE: 'SPO Batch Request Details | Special Price Offers',
    },
  };
  static readonly WORKFLOW_CONFIGURATION = {
    BASE: 'workflow-configuration',
    ORDER: 5,
    TITLE: 'Workflow Configuration',
    PRICE_OFFER_WORKFLOW: {
      BASE: 'price-offer-workflow',
      ORDER: 1,
      TITLE: 'Price Offer Workflow Configuration',
    },
    PSI_WORKFLOW: {
      BASE: 'psi-workflow',
      ORDER: 2,
      TITLE: 'PSI Workflow Configuration',
    },
    MATERIAL_STOCK_WORKFLOW: {
      BASE: 'material-stock-workflow',
      ORDER: 3,
      TITLE: 'Material Stock Workflow Configuration',
    },
    KEY_ACCOUNT_WORKFLOW: {
      BASE: 'kay-account-workflow',
      ORDER: 4,
      TITLE: 'Key Account Workflow Configuration',
    },
    GKR_WORKFLOW: {
      BASE: 'gkr-workflow',
      ORDER: 5,
      TITLE: 'GKR Workflow Configuration',
    },
    ASSET_MANAGEMENT: {
      BASE: 'asset-management',
      ORDER: 6,
      TITLE: 'FATA Management',
    },
  };
  static readonly SALE_ORDERS_MANAGEMENT = {
    BASE: 'sale-orders',
    ORDER: 8,
    LIST: {
      BASE: 'list',
      TITLE: '::SaleOrders',
      ORDER: 1,
    },
    NEW: {
      BASE: 'new',
      TITLE: '::NewSaleOrder',
      ORDER: 2,
    },
    DETAILS: {
      BASE: 'details',
      TITLE: '::SaleOrderDetails',
      ORDER: 3,
    },
  };
  static readonly SALE_ORDERS_GIC_MANAGEMENT = {
    BASE: 'sale-orders-gic',
    ORDER: 9,
    LIST: {
      BASE: 'list',
      TITLE: '::SaleOrdersGIC',
      ORDER: 1,
    },
    NEW: {
      BASE: 'new',
      TITLE: '::NewSaleOrderGIC',
      ORDER: 2,
    },
    DETAILS: {
      BASE: 'details',
      TITLE: '::SaleOrderGICDetails',
      ORDER: 3,
    },
  };
  static readonly PURCHASE_ORDERS_MANAGEMENT = {
    BASE: 'purchase-orders',
    ORDER: 10,
    TITLE: 'Purchase Orders Management',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'Purchase Orders List | Purchase Orders Management',
    },
    NEW: {
      BASE: 'new',
      ORDER: 2,
      TITLE: 'New Purchase Order | Purchase Orders Management',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 3,
      TITLE: 'Purchase Order Details | Purchase Orders Management',
    },
  };
  static readonly CARGO_DATA = {
    BASE: 'cargo-data',
    ORDER: 11,
    TITLE: 'Cargo Data',
    IMPORT: {
      BASE: 'import-cargo',
      ORDER: 1,
      TITLE: 'Import Data | Cargo Data',
    },
    REPORT: {
      BASE: 'report-cargo',
      ORDER: 2,
      TITLE: 'Cargo Report | Cargo Data',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 3,
      TITLE: 'Cargo Details | Cargo Data',
    },
  };
  static readonly IMPORT_ALLOCATION = {
    BASE: 'import-allocation',
    ORDER: 12,
    TITLE: 'Import Allocation',
    ALLOCATION_PRIORITY: {
      BASE: 'allocation-priority',
      ORDER: 1,
      TITLE: 'Material Stock List | Import Allocation',
    },
    IMPORT_LIST: {
      BASE: 'import-from-vendor-factory',
      ORDER: 2,
      TITLE: 'Import Material | Import Allocation',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 3,
      TITLE: 'Details | Import Allocation',
    },
    INVOICE_DETAILS: {
      BASE: 'invoice-details',
      ORDER: 3,
      TITLE: 'Invoice Details | Import Allocation',
    },
  };
  static readonly PSI = {
    BASE: 'psi',
    ORDER: 13,
    TITLE: 'PSI',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'PSI | PSI',
    },
    MY_APPROVALS: {
      BASE: 'my-approvals',
      ORDER: 2,
      TITLE: 'My Approvals | PSI',
    },
    PSI_REPORT: {
      BASE: 'psi-report',
      ORDER: 3,
      TITLE: 'PSI Report | PSI',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 6,
      TITLE: 'PSI Details | PSI',
    },
  };
  static readonly STOCK_TRACING = {
    BASE: 'stock-tracing',
    ORDER: 14,
    TITLE: 'Stock Tracing',
    IMPORT: {
      BASE: 'import-stock-tracing',
      ORDER: 1,
      TITLE: 'Import Data',
    },
    SEARCH: {
      BASE: 'search-stock-tracing',
      ORDER: 2,
      TITLE: 'Searching',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 3,
      TITLE: 'Stock Tracing Details',
    },
  };
  static readonly REPORT = {
    BASE: 'report',
    ORDER: 15,
    TITLE: 'Reports',
    DPO_REPORT: {
      BASE: 'dpo-report',
      ORDER: 1,
      TITLE: 'DPO Report | Reports',
      DPO_BY_MATERIAL_TYPE: {
        BASE: 'DPO-by-material-type',
        ORDER: 1,
        TITLE: 'DPO by Material Type | Reports',
      },
      DPO_BY_MATERIAL_GROUP: {
        BASE: 'DPO-by-material-group',
        ORDER: 2,
        TITLE: 'DPO by Material Group | Reports',
      },
      DPO_BY_DISTRIBUTOR: {
        BASE: 'DPO-by-distributor',
        ORDER: 3,
        TITLE: 'DPO by Distributor | Reports',
      },
      DPO_2023: {
        BASE: 'DPO-report-2023',
        ORDER: 4,
        TITLE: 'DPO Report 2023 | Reports',
      },
      DPO_PROCESSING: {
        BASE: 'DPO-processing-report',
        ORDER: 5,
        TITLE: 'DPO Processing Report | Reports',
      },
    },
    SALES_REPORT: {
      BASE: 'sales-report',
      ORDER: 2,
      TITLE: 'Sales Report | Reports',
      CUSTOMER_SALE_GENERAL: {
        BASE: 'customer-sale-general',
        ORDER: 9,
        TITLE: 'Customer Sale General | Reports',
      },
      CUSTOMER_SALE_DETAIL: {
        BASE: 'customer-sale-detail',
        ORDER: 10,
        TITLE: 'Customer Sale Detail | Reports',
      },
    },
    INVENTORY_REPORT: {
      BASE: 'inventory-report',
      ORDER: 3,
      TITLE: 'Inventory Report | Reports',
      OVERALL_STOCK_REPORT: {
        BASE: 'overall-stock-report',
        ORDER: 2,
        TITLE: 'Overall Stock Report | Reports',
      },
      INVENTORY_REPORT: {
        BASE: 'inventory-report',
        ORDER: 3,
        TITLE: 'Inventory Report | Reports',
      },
    },
    CUSTOMER_REPORT: {
      BASE: 'customer-report',
      ORDER: 4,
      SALE_R05: {
        BASE: 'sale-report-R05',
        ORDER: 4,
        TITLE: 'Sale Report R05 | Reports',
      },
      SALE: {
        BASE: 'sale-report',
        ORDER: 5,
        TITLE: 'Sale Report R06 | Reports',
      },
    },
  };
  static readonly APPLICATION_SETTING = {
    BASE: 'application-setting',
    ORDER: 16,
    TITLE: 'Application Setting',
    FILE_TEMPLATE: {
      BASE: 'file-management',
      ORDER: 1,
      TITLE: 'File template',
    },
    SUPPLIER: {
      BASE: 'supplier',
      ORDER: 5,
      TITLE: 'File template',
    },
    SALE_TEAM: {
      BASE: 'sale-team',
      ORDER: 7,
      TITLE: 'Sale Team',
    },
    BUYER_TYPE: {
      BASE: 'material-group-buyer',
      ORDER: 8,
      TITLE: 'Material Groups of Buyer',
    },
  };
  static readonly APPLICATION_CATEGORIES = {
    BASE: 'master-data',
    ORDER: 17,
    TITLE: 'Application Categories',
    STORAGE_LOCATION: {
      BASE: 'storage-location',
      ORDER: 1,
      TITLE: 'Storage Location',
    },
    CURRENCY: {
      BASE: 'currency',
      ORDER: 2,
      TITLE: 'currency',
    },
    MATERIAL_GROUP: {
      BASE: 'material-group',
      ORDER: 3,
      TITLE: 'Material Group',
    },
    BUYER_TYPE: {
      BASE: 'buyer-type',
      ORDER: 4,
      TITLE: 'Buyer type',
    },
    CUSTOMER: {
      BASE: 'customer',
      ORDER: 6,
      TITLE: 'Customer',
    },
    SUPPLIER: {
      BASE: 'supplier',
      ORDER: 7,
      TITLE: 'File template',
    },
    SUPPLIER_BU: {
      BASE: 'supplier-bu',
      ORDER: 8,
      TITLE: 'Supplier BU',
    },
    BUYER: {
      BASE: 'buyer',
      ORDER: 5,
      TITLE: 'Buyer',
    },
    MATERIAL_SEC_CLASSIFICATION: {
      BASE: 'material-sec-classification',
      ORDER: 9,
      TITLE: 'Material Sec Classification',
    },
    SAP_MAT_GROUP: {
      BASE: 'sap-mat-group',
      ORDER: 10,
      TITLE: 'SAP Material Group',
    },
    INVENTORY_REPORT: {
      BASE: 'inventory-report',
      ORDER: 11,
      TITLE: 'Inventory Report',
    },
  };
  static readonly FA_ADMIN = {
    BASE: 'fa-admin',
    ORDER: 18,
    SALE_TEAM: {
      BASE: 'sale-team',
      ORDER: 1,
      TITLE: 'Sale Team',
    },
    SYSTEM_CONFIGURATION: {
      BASE: 'system-configuration',
      ORDER: 2,
      TITLE: 'System Configuration',
    },
    DISTRIBUTOR_TARGET: {
      BASE: 'buyer-target',
      ORDER: 3,
      TITLE: 'Buyer Target',
    },
    SPO_DISCOUNT: {
      BASE: 'spo-discount',
      ORDER: 4,
      TITLE: 'SPO Discount',
    },
  };
  static readonly SYSTEM_CATEGORY = {
    BASE: 'system-categories',
    ORDER: 19,
    DISTRIBUTOR: {
      BASE: 'buyer-categories',
      ORDER: 1,
      TITLE: 'Buyer Categories | System Categories',
    },
    FACTORY: {
      BASE: 'factories',
      ORDER: 2,
      TITLE: 'Factories | System Categories',
    },
    CURRENCY: {
      BASE: 'currencies',
      ORDER: 3,
      TITLE: 'Currencies | System Categories',
    },
    MATERIAL_GROUP: {
      BASE: 'material-groups',
      ORDER: 4,
      TITLE: 'Material Groups | System Categories',
    },
    MATERIAL_TYPE: {
      BASE: 'material-types',
      ORDER: 5,
      TITLE: 'Material Types | System Categories',
    },
    SUPPLIER: {
      BASE: 'suppliers',
      ORDER: 6,
      TITLE: 'Suppliers | System Categories',
    },
    STOCK_CATEGORY: {
      BASE: 'stock-categories',
      ORDER: 7,
      TITLE: 'Stock Categories | System Categories',
    },
    PROJECT_INPUT_PRICE: {
      BASE: 'project-input-prices',
      ORDER: 8,
      TITLE: 'Project Input Prices | System Categories',
    },
    SALE_OF_DISTRIBUTOR: {
      BASE: 'sales-of-buyers',
      ORDER: 9,
      TITLE: 'Sales of Buyers | System Categories',
    },
    MATERIAL_GROUP_OF_DISTRIBUTOR: {
      BASE: 'material-groups-of-buyers',
      ORDER: 10,
      TITLE: 'Material Groups of Buyers | System Categories',
    },
    SALE_TEAM: {
      BASE: 'sale-teams',
      ORDER: 11,
      TITLE: 'Sale Teams | System Categories',
    },
    BUYER: {
      BASE: 'buyers',
      ORDER: 12,
      TITLE: 'Buyers | System Categories',
    },
  };
  static readonly SYSTEM_CONFIGURATION = {
    BASE: 'system-configuration',
    ORDER: 20,
    TITLE: 'System Configuration',
  };
  static readonly BUYERS = {
    BASE: 'buyers',
    ORDER: 21,
    TITLE: 'Buyers',
  };

  // ── Các static không thay đổi ORDER ───────────────────────
  static readonly PURCHASE_ORDERS = {
    BASE: 'purchase-orders',
    ORDER: 10,
    TITLE: 'Purchase Orders',
  };
  static readonly SALE_ORDERS = {
    BASE: 'sale-orders',
    ORDER: 8,
    TITLE: 'Sale Orders (SO)',
    DPO: {
      BASE: 'sale-orders',
      ORDER: 1,
      TITLE: 'Sale Orders (DPO)',
    },
    GIC: {
      BASE: 'sale-orders-gic',
      ORDER: 2,
      TITLE: 'Sale Orders (GIC)',
    },
  };
  static readonly DPO = {
    BASE: 'dpo',
    ORDER: 1,
    TITLE: 'DPO Management',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'DPO List | DPO Management',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 2,
      TITLE: 'DPO Details | DPO Management',
    },
    REPORT: {
      BASE: 'report',
      ORDER: 3,
      TITLE: 'DPO Received | DPO Management',
    },
  };
  static readonly GIC = {
    BASE: 'gic',
    ORDER: 2,
    TITLE: 'GIC Management',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'GIC List | GIC Management',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 2,
      TITLE: 'GIC Details | GIC Management',
    },
  };
  static readonly GKR = {
    BASE: 'gkr',
    ORDER: 3,
    TITLE: 'GKR Management',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'GKR List | GKR Management',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 2,
      TITLE: 'GKR Details | GKR Management',
    },
    APPROVALS: {
      BASE: 'approvals',
      ORDER: 3,
      TITLE: 'GKR Approvals | GKR Management',
    },
  };
  static readonly SPECIAL_INPUT_PRICE = {
    BASE: 'special-input-prices',
    ORDER: 8,
    TITLE: 'Special Input Prices',
    LIST: {
      BASE: 'list',
      ORDER: 1,
      TITLE: 'Special Input Prices List | Special Input Prices',
    },
    NEW: {
      BASE: 'new',
      ORDER: 2,
      TITLE: 'New Special Input Price | Special Input Prices',
    },
    DETAILS: {
      BASE: 'details',
      ORDER: 3,
      TITLE: 'Special Input Price Detail | Special Input Prices',
    },
  };
  static readonly CUSTOMERS = {
    BASE: 'customers',
    ORDER: 4,
    TITLE: 'Customers',
  };
  static readonly DELIVERY_ORDERS = {
    BASE: 'delivery-orders',
    ORDER: 8,
    TITLE: 'Delivery Orders',
  };
  static readonly CUSTOMER_MANAGEMENT = {
    BASE: 'customer-management',
    ORDER: 14,
    TITLE: 'Customer Management',
  };
}

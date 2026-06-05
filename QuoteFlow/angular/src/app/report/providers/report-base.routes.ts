import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const REPORT_BASE_ROUTES: ABP.Route[] = [
  // --- MENU GỐC: REPORT ---
  {
    iconClass: 'fa-solid fa-comments-dollar',
    name: '::Menu:Report',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Reports.Default}`,
    breadcrumbText: '::Report',
    order: AppRoutes.REPORT.ORDER,
  },

  // --- MENU CHA: DPO REPORT ---
  {
    path: '/',
    iconClass: 'fa-solid fa-file-invoice-dollar',
    name: '::Menu:DPOReport',
    parentName: '::Menu:Report',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Reports.R25DPOReceivedByMaterialType}`,
    breadcrumbText: '::Report',
    order: AppRoutes.REPORT.DPO_REPORT.ORDER,
  },

  // --- MENU CHA: INVENTORY REPORT ---
  {
    path: '/',
    iconClass: 'fa-solid fa-warehouse', // Inventory Report
    name: '::Menu:InventoryReport',
    parentName: '::Menu:Report',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Reports.R15Inventory} || ${AppPermissions.Reports.R21OverallStock}`,
    breadcrumbText: '::Report',
    order: AppRoutes.REPORT.INVENTORY_REPORT.ORDER,
  },

  // --- MENU CHA MỚI: CUSTOMER ---
  // (Nằm ngang hàng với InventoryReport và DPOReport)
  {
    path: '/',
    iconClass: 'fa-solid fa-users', // Icon ví dụ cho Customer
    name: '::Menu:Customer',
    parentName: '::Menu:Report', // Cùng cha với InventoryReport
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Reports.CustomerSaleReportGeneral} || ${AppPermissions.Reports.CustomerSaleReportDetail}`,
    breadcrumbText: '::Report',
    // Bạn nên tạo thêm hằng số order cho Customer trong AppRoutes, tạm thời để cứng hoặc lấy theo thứ tự mong muốn
    order: AppRoutes.REPORT.CUSTOMER_REPORT.ORDER,
  },

  // --- CÁC ITEM CON CỦA INVENTORY REPORT ---
  {
    path: `/${AppRoutes.REPORT.BASE}/${AppRoutes.REPORT.INVENTORY_REPORT.OVERALL_STOCK_REPORT.BASE}`,
    iconClass: 'fa-solid fa-boxes-stacked', // tổng quan kho
    name: '::Menu:Report:OverallStockReport',
    parentName: '::Menu:InventoryReport',
    order: AppRoutes.REPORT.INVENTORY_REPORT.OVERALL_STOCK_REPORT.ORDER,
    requiredPolicy: `${AppPermissions.Reports.R21OverallStock}`,
    layout: eLayoutType.application,
    breadcrumbText: '::Report:OverallStockReport',
  },
  {
    path: `/${AppRoutes.REPORT.BASE}/${AppRoutes.REPORT.INVENTORY_REPORT.INVENTORY_REPORT.BASE}`,
    iconClass: 'fa-solid fa-warehouse', // tồn kho chi tiết, realtime
    name: '::Menu:Report:InventoryReport',
    parentName: '::Menu:InventoryReport',
    // requiredPolicy: `${AppPermissions.Reports.R15Inventory}`,
    order: AppRoutes.REPORT.INVENTORY_REPORT.INVENTORY_REPORT.ORDER,
    layout: eLayoutType.application,
    breadcrumbText: '::Report:InventoryReport',
  },

  // --- CÁC ITEM CON CỦA CUSTOMER (Đã chuyển R05, R06 xuống đây) ---
  {
    path: `/${AppRoutes.REPORT.BASE}/${AppRoutes.REPORT.CUSTOMER_REPORT.SALE_R05.BASE}`,
    iconClass: 'fa-solid fa-flag',
    name: '::Menu:Report:SaleReportR05',
    parentName: '::Menu:Customer',
    requiredPolicy: `${AppPermissions.Reports.CustomerSaleReportGeneral}`,
    order: AppRoutes.REPORT.CUSTOMER_REPORT.SALE_R05.ORDER,
    layout: eLayoutType.application,
    breadcrumbText: '::Report:SaleReportR05',
  },
  {
    path: `/${AppRoutes.REPORT.BASE}/${AppRoutes.REPORT.CUSTOMER_REPORT.SALE.BASE}`,
    iconClass: 'fa-solid fa-flag',
    name: '::Menu:Report:SaleReportR06',
    parentName: '::Menu:Customer',
    requiredPolicy: `${AppPermissions.Reports.CustomerSaleReportDetail}`,
    order: AppRoutes.REPORT.CUSTOMER_REPORT.SALE.ORDER,
    layout: eLayoutType.application,
    breadcrumbText: '::Report:SaleReport',
  },

  // --- CÁC ITEM CON CỦA DPO REPORT ---
  {
    path: `/${AppRoutes.REPORT.BASE}/${AppRoutes.REPORT.DPO_REPORT.DPO_BY_MATERIAL_TYPE.BASE}`,
    iconClass: 'fa-solid fa-layer-group',
    name: '::Menu:Report:DPOByMaterialType',
    parentName: '::Menu:DPOReport',
    order: AppRoutes.REPORT.DPO_REPORT.DPO_BY_MATERIAL_TYPE.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Reports.R25DPOReceivedByMaterialType}`,
    breadcrumbText: '::Report:DPOByMaterialType',
  },
  {
    path: `/${AppRoutes.REPORT.BASE}/${AppRoutes.REPORT.DPO_REPORT.DPO_PROCESSING.BASE}`,
    iconClass: 'fa-solid fa-layer-group',
    name: '::Menu:Report:DPOProcessing',
    parentName: '::Menu:DPOReport',
    order: AppRoutes.REPORT.DPO_REPORT.DPO_PROCESSING.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Reports.R24DPOProcessing}`,
    breadcrumbText: '::Report:DPOProcessing',
  },
];

import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const STOCK_MANAGEMENT_BASE_ROUTES: ABP.Route[] = [
  {
    iconClass: 'fas fa-cube',
    name: '::Menu:StockManagement',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MaterialStocks.Default}`,
    breadcrumbText: '::StockManagement',
    order: AppRoutes.STOCK_MANAGEMENT.ORDER,
  },
  {
    path: `/${AppRoutes.STOCK_MANAGEMENT.BASE}/${AppRoutes.STOCK_MANAGEMENT.LIST.BASE}`,
    iconClass: 'fas fa-list',
    name: '::Menu:StockManagement:List',
    parentName: '::Menu:StockManagement',
    order: AppRoutes.STOCK_MANAGEMENT.LIST.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MaterialStocks.MaterialStock}`,
    breadcrumbText: '::StockManagement:List',
  },
  {
    path: `/${AppRoutes.STOCK_MANAGEMENT.BASE}/${AppRoutes.STOCK_MANAGEMENT.IMPORT_STOCK.BASE}`,
    iconClass: 'fas fa-file-alt',
    name: '::Menu:StockManagement:ImportStock',
    parentName: '::Menu:StockManagement',
    order: AppRoutes.STOCK_MANAGEMENT.IMPORT_STOCK.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MaterialStocks.UploadMaterialStock}`,
    breadcrumbText: '::StockManagement:ImportStock',
  },
  // {
  //   path: `/${AppRoutes.STOCK_MANAGEMENT.BASE}/${AppRoutes.STOCK_MANAGEMENT.REPORT_STOCK.BASE}`,
  //   iconClass: 'fas fa-chart-line',
  //   name: '::Menu:StockManagement:StockWarningReport',
  //   parentName: '::Menu:StockManagement',
  //   order: AppRoutes.STOCK_MANAGEMENT.REPORT_STOCK.ORDER,
  //   layout: eLayoutType.application,
  //   requiredPolicy: `${AppPermissions.MaterialStocks.StockWarningReport}`,
  //   breadcrumbText: '::StockManagement:OverallStockReport',
  // },
  // {
  //   path: `/${AppRoutes.STOCK_MANAGEMENT.BASE}/${AppRoutes.STOCK_MANAGEMENT.REAL_TIME_STOCK.BASE}`,
  //   iconClass: 'fas fa-warehouse',
  //   name: '::Menu:StockManagement:RealTimeStock',
  //   parentName: '::Menu:StockManagement',
  //   order: AppRoutes.STOCK_MANAGEMENT.REAL_TIME_STOCK.ORDER,
  //   layout: eLayoutType.application,
  //   requiredPolicy: `${AppPermissions.MaterialStocks.RealTimeStock}`,
  //   breadcrumbText: '::StockManagement:InventoryReport',
  // },
];

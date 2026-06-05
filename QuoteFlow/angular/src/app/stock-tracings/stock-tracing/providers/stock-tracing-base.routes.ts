import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const STOCK_TRACING_BASE_ROUTES: ABP.Route[] = [
  {
    iconClass: 'fas fa-upload',
    name: '::Menu:StockTracings',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.StockTracings.Default}`,
    breadcrumbText: '::StockTracings',
    order: AppRoutes.STOCK_TRACING.ORDER,
  },
  {
    path: `/${AppRoutes.STOCK_TRACING.BASE}/${AppRoutes.STOCK_TRACING.IMPORT.BASE}`,
    iconClass: 'fas fa-file-alt',
    name: '::Menu:StockTracings:ImportStockTracing',
    parentName: '::Menu:StockTracings',
    order: AppRoutes.STOCK_TRACING.IMPORT.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.StockTracings.ImportData}`,
    breadcrumbText: '::StockTracings',
  },
  {
    path: `/${AppRoutes.STOCK_TRACING.BASE}/${AppRoutes.STOCK_TRACING.SEARCH.BASE}`,
    iconClass: 'fas fa-search',
    name: '::Menu:StockTracings:SearchStockTracing',
    parentName: '::Menu:StockTracings',
    order: AppRoutes.STOCK_TRACING.SEARCH.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.StockTracings.Searching}`,
    breadcrumbText: '::StockTracings',
  },
];

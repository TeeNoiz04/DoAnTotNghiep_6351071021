import { RoutesService, eLayoutType } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';
import { AppPermissions } from './app.permissions';
import { AppRoutes } from './app.routes';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
    {
      iconClass: 'fas fa-chart-line',
      name: '::Menu:Dashboard',
      layout: eLayoutType.application,
      order: AppRoutes.HOME.ORDER,
      requiredPolicy: `${AppPermissions.Dashboard.Default}`,
    },
    {
      path: '/dashboard/base',
      name: 'Overview',
      iconClass: 'fas fa-chart-bar',
      order: AppRoutes.HOME.ORDER + 0.1,
      layout: eLayoutType.application,
      requiredPolicy: `${AppPermissions.Dashboard.Default}`,
      parentName: '::Menu:Dashboard',
    },
    {
      path: '/dashboard/approval',
      name: 'Approval Dashboard',
      iconClass: 'fas fa-tasks',
      order: AppRoutes.HOME.ORDER + 0.2,
      layout: eLayoutType.application,
      requiredPolicy: `${AppPermissions.Dashboard.Default}`,
      parentName: '::Menu:Dashboard',
    },
    {
      iconClass: 'fas fa-box',
      name: '::Menu:MovingOrders',
      layout: eLayoutType.application,
      breadcrumbText: '::MovingOrders',
      requiredPolicy: `${AppPermissions.MovingOrders.Default}`,
      order: 6,
    },
    {
      path: '/purchase-orders/list',
      name: 'Purchase Order (PO)',
      iconClass: 'fas fa-shopping-cart',
      order: AppRoutes.PURCHASE_ORDERS.ORDER,
      layout: eLayoutType.application,
      requiredPolicy: `${AppPermissions.PurchaseOrders.Default}`,
    },
    {
      path: `/${AppRoutes.SALE_ORDERS.DPO.BASE}/list`,
      name: `${AppRoutes.SALE_ORDERS.DPO.TITLE}`,
      iconClass: 'fas fa-clipboard-check',
      order: AppRoutes.SALE_ORDERS.DPO.ORDER,
      layout: eLayoutType.application,
      requiredPolicy: `${AppPermissions.SaleOrders.Default}`,
      parentName: `${AppRoutes.SALE_ORDERS.TITLE}`,
    },
    {
      path: `/${AppRoutes.SALE_ORDERS.GIC.BASE}/list`,
      name: `${AppRoutes.SALE_ORDERS.GIC.TITLE}`,
      iconClass: 'fas fa-file-invoice',
      order: AppRoutes.SALE_ORDERS.GIC.ORDER,
      layout: eLayoutType.application,
      requiredPolicy: `${AppPermissions.SaleOrders.Default}`,
      parentName: `${AppRoutes.SALE_ORDERS.TITLE}`,
    },
    {
      iconClass: 'fas fa-file-signature',
      name: `${AppRoutes.SALE_ORDERS.TITLE}`,
      layout: eLayoutType.application,
      order: AppRoutes.SALE_ORDERS.ORDER,
      requiredPolicy: `${AppPermissions.SaleOrders.Default}`,
    },
  ]);
  routes.remove(['File Templates']);
}

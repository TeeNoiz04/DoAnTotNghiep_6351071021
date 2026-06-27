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
    // {
    //   iconClass: 'fas fa-chart-line',
    //   name: '::Menu:Dashboard',
    //   layout: eLayoutType.application,
    //   order: AppRoutes.HOME.ORDER,
    //   requiredPolicy: `${AppPermissions.Dashboard.Default}`,
    // },
    // {
    //   path: '/dashboard/base',
    //   name: 'Overview',
    //   iconClass: 'fas fa-chart-bar',
    //   order: AppRoutes.HOME.ORDER + 0.1,
    //   layout: eLayoutType.application,
    //   requiredPolicy: `${AppPermissions.Dashboard.Default}`,
    //   parentName: '::Menu:Dashboard',
    // },
    // {
    //   path: '/dashboard/approval',
    //   name: 'Approval Dashboard',
    //   iconClass: 'fas fa-tasks',
    //   order: AppRoutes.HOME.ORDER + 0.2,
    //   layout: eLayoutType.application,
    //   requiredPolicy: `${AppPermissions.Dashboard.Default}`,
    //   parentName: '::Menu:Dashboard',
    // },
    {
      path: `/${AppRoutes.SALE_ORDERS.DPO.BASE}/list`,
      name: `${AppRoutes.SALE_ORDERS.DPO.TITLE}`,
      iconClass: 'fas fa-clipboard-check',
      order: 8,
      layout: eLayoutType.application,
      requiredPolicy: `${AppPermissions.SaleOrders.Default}`,
    },
    {
      // Layout anchor for SO detail routes - no menu entry
      path: `/${AppRoutes.SALE_ORDERS.DPO.BASE}`,
      name: '__so_layout_anchor__',
      invisible: true,
      layout: eLayoutType.application,
      requiredPolicy: `${AppPermissions.SaleOrders.Default}`,
    },
  ]);
  routes.remove(['File Templates']);
}

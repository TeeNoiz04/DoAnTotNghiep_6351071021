import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const DPO_BASE_ROUTES: ABP.Route[] = [
  {
    path: `/${AppRoutes.DPO.BASE}/${AppRoutes.DPO.LIST.BASE}`,
    iconClass: 'fa-solid fa-clipboard-check',
    name: '::Menu:DPOs',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MovingOrders.DPOs.DPODefault}`,
    breadcrumbText: '::DPOManagement',
    order: 7,
  },
  {
    // Layout anchor for detail routes - invisible, no menu entry
    path: `/${AppRoutes.DPO.BASE}`,
    name: '__dpo_layout_anchor__',
    invisible: true,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MovingOrders.DPOs.DPODefault}`,
  },
];

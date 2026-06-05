import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const DPO_BASE_ROUTES: ABP.Route[] = [
  {
    path: `/${AppRoutes.DPO.BASE}/${AppRoutes.DPO.LIST.BASE}`,
    iconClass: 'fa-solid fa-clipboard-check',
    name: '::Menu:DPOs',
    layout: eLayoutType.application,
    parentName: '::Menu:MovingOrders',
    requiredPolicy: `${AppPermissions.MovingOrders.DPOs.DPODefault}`,
    breadcrumbText: '::DPOManagement',
    order: AppRoutes.DPO.ORDER,
  },
];

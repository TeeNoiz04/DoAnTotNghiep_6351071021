import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';

export const BUYER_BASE_ROUTES: ABP.Route[] = [
  {
    path: '/buyers',
    iconClass: 'fa-solid fa-people-group',
    name: '::Menu:BuyerManagement',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewBuyer}`,
    breadcrumbText: '::Buyers',
  },
];

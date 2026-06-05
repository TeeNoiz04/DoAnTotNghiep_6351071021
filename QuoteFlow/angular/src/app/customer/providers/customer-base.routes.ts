import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';

export const CUSTOMER_BASE_ROUTES: ABP.Route[] = [
  {
    path: '/customers',
    iconClass: 'fa-solid fa-comments-dollar',
    name: '::Menu:CustomerManagement',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.MasterDatas.ViewCustomer}`,
    breadcrumbText: '::Customers',
  },
];

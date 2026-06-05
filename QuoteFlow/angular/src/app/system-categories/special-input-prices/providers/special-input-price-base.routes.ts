import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const SPECIAL_INPUT_PRICE_BASE_ROUTES: ABP.Route[] = [
  {
    path: `/${AppRoutes.SPECIAL_INPUT_PRICE.BASE}`,
    iconClass: 'fas fa-dollar-sign',
    name: '::Menu:SystemCategories:SpecialInputPrice',
    order: AppRoutes.SPECIAL_INPUT_PRICE.ORDER,
    layout: eLayoutType.application,
    breadcrumbText: '::SpecialInputPrice',
    requiredPolicy: `${AppPermissions.SpecialInputPrice.Default}`,
  },
];

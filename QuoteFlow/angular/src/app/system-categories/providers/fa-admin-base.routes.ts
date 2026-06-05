import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const FA_ADMIN_BASE_ROUTES: ABP.Route[] = [
  {
    iconClass: 'fas fa-sliders-h', // general application settings
    name: '::Menu:FA_Admins',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.FAAdmins.Default}`,
    breadcrumbText: '::FA_Admins',
    order: AppRoutes.FA_ADMIN.ORDER,
  },
  {
    path: `/${AppRoutes.FA_ADMIN.BASE}/${AppRoutes.FA_ADMIN.SALE_TEAM.BASE}`,
    iconClass: 'fas fa-users-cog', // sales team / management
    name: '::Menu:ApplicationSetting:SaleTeam',
    parentName: '::Menu:FA_Admins',
    order: AppRoutes.FA_ADMIN.SALE_TEAM.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.FAAdmins.ViewSaleTeam}`,
    breadcrumbText: '::FA_Admins',
  },
  {
    path: `/${AppRoutes.FA_ADMIN.BASE}/${AppRoutes.FA_ADMIN.SYSTEM_CONFIGURATION.BASE}`,
    iconClass: 'fas fa-gear', // sales team / management
    name: '::Menu:ApplicationSetting:SystemConfiguration',
    parentName: '::Menu:FA_Admins',
    order: AppRoutes.FA_ADMIN.SYSTEM_CONFIGURATION.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.FAAdmins.ViewSystemConfiguration}`,
    breadcrumbText: '::FA_Admins',
  },

  {
    path: `/${AppRoutes.FA_ADMIN.BASE}/${AppRoutes.FA_ADMIN.DISTRIBUTOR_TARGET.BASE}`,
    iconClass: 'fas fa-bullseye', // buyer target / goal
    name: '::Menu:ApplicationSetting:DistributorTarget',
    parentName: '::Menu:FA_Admins',
    order: AppRoutes.FA_ADMIN.DISTRIBUTOR_TARGET.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.FAAdmins.ViewBuyerTarget}`,
    breadcrumbText: '::FA_Admins',
  },

  {
    path: `/${AppRoutes.FA_ADMIN.BASE}/${AppRoutes.FA_ADMIN.SPO_DISCOUNT.BASE}`,
    iconClass: 'fas fa-percent',
    name: '::Menu:ApplicationSetting:SPODiscountSetting',
    parentName: '::Menu:FA_Admins',
    order: AppRoutes.FA_ADMIN.SPO_DISCOUNT.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.FAAdmins.ViewCfgDiscountRatio}`,
    breadcrumbText: '::FA_Admins',
  },
];

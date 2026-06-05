import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const PSI_BASE_ROUTES: ABP.Route[] = [
  {
    iconClass: 'fas fa-file-alt',
    name: '::Menu:PSI',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.PSIs.Default}`,
    breadcrumbText: '::PSI',
    order: AppRoutes.PSI.ORDER,
  },
  {
    path: `/${AppRoutes.PSI.BASE}/${AppRoutes.PSI.LIST.BASE}`,
    iconClass: 'fas fa-list',
    name: '::Menu:PSI:List',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.PSIs.ImportData}`,
    breadcrumbText: '::PSIList',
    order: AppRoutes.PSI.LIST.ORDER,
    parentName: '::Menu:PSI',
  },
  {
    path: `/${AppRoutes.PSI.BASE}/${AppRoutes.PSI.MY_APPROVALS.BASE}`,
    iconClass: 'fas fa-user-check',
    name: '::Menu:PSI:MyApprovals',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.PSIs.ImportData}`,
    breadcrumbText: '::MyApprovals',
    order: AppRoutes.PSI.MY_APPROVALS.ORDER,
    parentName: '::Menu:PSI',
  },
  {
    path: `/${AppRoutes.PSI.BASE}/${AppRoutes.PSI.PSI_REPORT.BASE}`,
    iconClass: 'fas fa-chart-bar',
    name: '::Menu:PSI:PSISReport',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.PSIs.PSIReport}`,
    breadcrumbText: '::PSISReport',
    order: AppRoutes.PSI.PSI_REPORT.ORDER,
    parentName: '::Menu:PSI',
  },
];

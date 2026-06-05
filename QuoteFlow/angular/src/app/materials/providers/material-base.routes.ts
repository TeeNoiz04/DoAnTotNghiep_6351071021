import { ABP, eLayoutType } from '@abp/ng.core';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';

export const MATERIAL_BASE_ROUTES: ABP.Route[] = [
  {
    iconClass: 'fas fa-boxes',
    name: '::Menu:Materials',
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Materials.Default}`,
    breadcrumbText: '::Materials',
    order: AppRoutes.MATERIAL_STOCK.ORDER,
  },
  {
    path: `/${AppRoutes.MATERIAL_STOCK.BASE}/${AppRoutes.MATERIAL_STOCK.LIST.BASE}`,
    iconClass: 'fas fa-list',
    name: '::Menu:Materials:List',
    parentName: '::Menu:Materials',
    order: AppRoutes.MATERIAL_STOCK.LIST.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Materials.MaterialData}`,
    breadcrumbText: '::Materials:List',
  },
  {
    path: `/${AppRoutes.MATERIAL_STOCK.BASE}/${AppRoutes.MATERIAL_STOCK.IMPORT_MATERIAL.BASE}`,
    iconClass: 'fas fa-file-alt',
    name: '::Menu:Materials:ImportMaterial',
    parentName: '::Menu:Materials',
    order: AppRoutes.MATERIAL_STOCK.IMPORT_MATERIAL.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Materials.UploadMaterialData}`,
    breadcrumbText: '::Materials:ImportMaterial',
  },
  {
    path: `/${AppRoutes.MATERIAL_STOCK.BASE}/${AppRoutes.MATERIAL_STOCK.IMPORT_MY_APPROVALS.BASE}`,
    iconClass: 'fas fa-check-double',
    name: '::Menu:Materials:ImportMyApprovals',
    parentName: '::Menu:Materials',
    order: AppRoutes.MATERIAL_STOCK.IMPORT_MY_APPROVALS.ORDER,
    layout: eLayoutType.application,
    requiredPolicy: `${AppPermissions.Materials.Default}`,
    breadcrumbText: '::Materials:ImportMyApprovals',
  },
];

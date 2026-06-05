import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { ImportMaterialDetailComponent } from './components/import-material/import-material-detail.component';
import { ImportMaterialComponent } from './components/import-material/import-material.component';
import { MaterialManagementComponent } from './components/material-management/material-management.component';
import { MyApprovalsMaterialsComponent } from './components/my-approvals/my-approvals.component';

export const routes: Routes = [
  {
    path: AppRoutes.MATERIAL_STOCK.LIST.BASE,
    component: MaterialManagementComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.MATERIAL_STOCK.LIST.TITLE,
    },
  },
  {
    path: AppRoutes.MATERIAL_STOCK.IMPORT_MATERIAL.BASE,
    component: ImportMaterialComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.MATERIAL_STOCK.IMPORT_MATERIAL.TITLE,
    },
  },
  {
    path: AppRoutes.MATERIAL_STOCK.IMPORT_MY_APPROVALS.BASE,
    component: MyApprovalsMaterialsComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.MATERIAL_STOCK.IMPORT_MY_APPROVALS.TITLE,
    },
  },
  {
    path: `:id/${AppRoutes.MATERIAL_STOCK.DETAILS.BASE}`,
    component: ImportMaterialDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.MATERIAL_STOCK.DETAILS.TITLE,
      requiredPolicy: `${AppPermissions.MaterialStocks.Default}`,
    },
  },
  {
    path: `:id/${AppRoutes.MATERIAL_STOCK.DETAILS.BASE}/${AppRoutes.MATERIAL_STOCK.MY_APPROVALS.BASE}`,
    component: ImportMaterialDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.MATERIAL_STOCK.DETAILS.TITLE,
      requiredPolicy: `${AppPermissions.MaterialStocks.Default}`,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MaterialRoutingModule {}

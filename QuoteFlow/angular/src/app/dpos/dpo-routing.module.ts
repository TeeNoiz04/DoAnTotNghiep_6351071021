import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { DPODetailComponent } from './components/dpo-detail-page.component';
import { DPOComponent } from './components/dpo.component';

const routes: Routes = [
  // {
  //   path: AppRoutes.DPO.REPORT.BASE,
  //   component: DPOReportComponent,
  //   canActivate: [authGuard, permissionGuard],
  //   data: {
  //     title: AppRoutes.DPO.REPORT.TITLE,
  //   },
  // },
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full',
  },
  {
    path: 'list',
    component: DPOComponent,
    canActivate: [authGuard, permissionGuard],
    data: {},
  },
  {
    path: ':id',
    component: DPODetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPolicy: `${AppPermissions.MovingOrders.DPOs.DPODefault}`,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DPORoutingModule {}

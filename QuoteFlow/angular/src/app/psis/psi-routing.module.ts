import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { reportModalGuard } from '@app/report/guards/report-modal.guard';
import { MyApprovalsComponent } from './components/my-approvals/my-approvals.component';
import { PSIReportPlaceholderComponent } from './components/psi-report-placeholder/psi-report-placeholder.component';
import { PSIDetailComponent } from './components/psi/psi-detail.component';
import { PSIComponent } from './components/psi/psi.component';
import { PSIReportComponent } from './components/psi-report/psi-report.component';

export const routes: Routes = [
  {
    path: '',
    component: PSIComponent,
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: `${AppRoutes.PSI.LIST.BASE}`,
    component: PSIComponent,
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: `${AppRoutes.PSI.MY_APPROVALS.BASE}`,
    component: MyApprovalsComponent,
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: `${AppRoutes.PSI.PSI_REPORT.BASE}`,
    // component: PSIReportPlaceholderComponent,
    component: PSIReportComponent,
    canActivate: [reportModalGuard],
    data: {
      title: AppRoutes.PSI.PSI_REPORT.TITLE,
      reportType: 'psi-by-product',
    },
  },
  {
    path: `:id/${AppRoutes.PSI.DETAILS.BASE}`,
    component: PSIDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.PSI.DETAILS.TITLE,
      requiredPolicy: `${AppPermissions.PSIs.Default}`,
    },
  },
  {
    path: `:id/${AppRoutes.PSI.DETAILS.BASE}/${AppRoutes.PSI.MY_APPROVALS.BASE}`,
    component: PSIDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.PSI.DETAILS.TITLE,
      requiredPolicy: `${AppPermissions.PSIs.Default}`,
    },
  },
  {
    path: 'psi-report-detail',
    loadComponent: () =>
      import('./components/psi-report/psi-report.component').then(m => m.PSIReportComponent),
    canActivate: [authGuard, permissionGuard],
    data: {
      title: 'PSI Report - Detail View',
      requiredPolicy: `${AppPermissions.PSIs.Default}`,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PSIRoutingModule {}

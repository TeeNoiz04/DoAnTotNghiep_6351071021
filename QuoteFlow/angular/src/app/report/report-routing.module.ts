import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { DPOReportComponent } from './components/dpo-report/dpo-report.component';
import { ReportStockPlaceholderComponent } from '@app/stock-management/components/placeholder/placeholder.component';
import { reportModalGuard } from './guards/report-modal.guard';
import { StockReportComponent } from '@app/stock-management/components/stock-report/stock-report.component';
import { DPOProcessingReportComponent } from './components/dpo-processing-report/dpo-processing-report.component';
import { SaleReportR06Component } from '@app/stock-management/components/sale-report/sale-report-r06.component';
import { SaleReportR05Component } from '@app/stock-management/components/sale-report-general/sale-report-r05.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: AppRoutes.REPORT.BASE,
    pathMatch: 'full',
  },
  {
    path: AppRoutes.REPORT.DPO_REPORT.DPO_2023.BASE,
    component: DPOReportComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.REPORT.DPO_REPORT.DPO_2023.TITLE,
    },
  },
  {
    path: AppRoutes.REPORT.DPO_REPORT.DPO_PROCESSING.BASE,
    component: DPOProcessingReportComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.REPORT.DPO_REPORT.DPO_PROCESSING.TITLE,
    },
  },
  {
    path: AppRoutes.REPORT.INVENTORY_REPORT.OVERALL_STOCK_REPORT.BASE,
    component: StockReportComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.STOCK_MANAGEMENT.REPORT_STOCK.TITLE,
    },
  },
  {
    path: AppRoutes.REPORT.INVENTORY_REPORT.INVENTORY_REPORT.BASE,
    component: ReportStockPlaceholderComponent,
    canActivate: [reportModalGuard],
    data: {
      title: AppRoutes.REPORT.INVENTORY_REPORT.INVENTORY_REPORT.TITLE,
      reportType: 'inventory',
    },
  },
  {
    path: AppRoutes.REPORT.CUSTOMER_REPORT.SALE.BASE,
    component: SaleReportR06Component,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.REPORT.CUSTOMER_REPORT.SALE.TITLE,
    },
  },
  {
    path: AppRoutes.REPORT.CUSTOMER_REPORT.SALE_R05.BASE,
    component: SaleReportR05Component,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.REPORT.CUSTOMER_REPORT.SALE_R05.TITLE,
      reportType: 'sale report R05',
    },
  },
  {
    path: AppRoutes.REPORT.DPO_REPORT.DPO_BY_MATERIAL_TYPE.BASE,
    component: DPOReportComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.REPORT.DPO_REPORT.DPO_BY_MATERIAL_TYPE.TITLE,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportRoutingModule {}

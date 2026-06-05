import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { ImportStockDetailComponent } from './components/import-stock/import-stock-detail.component';
import { ImportStockComponent } from './components/import-stock/import-stock.component';
import { StockManagementComponent } from './components/stock-management/stock-management.component';
import { StockReportViewService } from './services/stock-report.service';

export const routes: Routes = [
  {
    path: AppRoutes.STOCK_MANAGEMENT.LIST.BASE,
    component: StockManagementComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.STOCK_MANAGEMENT.LIST.TITLE,
    },
  },
  {
    path: AppRoutes.STOCK_MANAGEMENT.IMPORT_STOCK.BASE,
    component: ImportStockComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.STOCK_MANAGEMENT.IMPORT_STOCK.TITLE,
    },
  },

  {
    path: `:id/${AppRoutes.STOCK_MANAGEMENT.DETAILS.BASE}`,
    component: ImportStockDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.STOCK_MANAGEMENT.DETAILS.TITLE,
      requiredPolicy: `${AppPermissions.MaterialStocks.Default}`,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [StockReportViewService],
})
export class StockManagementRoutingModule {}

import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { StockTracingDetailComponent } from './components/import-stock-tracing/stock-tracing-detail.component';
import { StockTracingComponent } from './components/import-stock-tracing/stock-tracing.component';
import { SearchStockTracingComponent } from './components/search-stock-tracing/search-stock-tracing.component';

export const routes: Routes = [
  {
    path: AppRoutes.STOCK_TRACING.IMPORT.BASE,
    component: StockTracingComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.STOCK_TRACING.IMPORT.TITLE,
    },
  },
  {
    path: AppRoutes.STOCK_TRACING.SEARCH.BASE,
    component: SearchStockTracingComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.STOCK_TRACING.SEARCH.TITLE,
    },
  },
  {
    path: `:id/${AppRoutes.STOCK_TRACING.DETAILS.BASE}`,
    component: StockTracingDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.STOCK_TRACING.DETAILS.TITLE,
      requiredPolicy: `${AppPermissions.StockTracings.Default}`,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class StockTracingRoutingModule {}

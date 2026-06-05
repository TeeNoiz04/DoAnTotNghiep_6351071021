import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { SaleOrdersDetailComponent } from './components/sale-orders-details/sale-orders-details.component';
import { SaleOrdersManagementComponent } from './components/sale-orders-management/sale-orders-management.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: AppRoutes.SALE_ORDERS_MANAGEMENT.LIST.BASE,
    pathMatch: 'full',
  },
  {
    path: AppRoutes.SALE_ORDERS_MANAGEMENT.LIST.BASE,
    component: SaleOrdersManagementComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.SALE_ORDERS_MANAGEMENT.LIST.TITLE,
    },
  },
  {
    path: AppRoutes.SALE_ORDERS_MANAGEMENT.NEW.BASE,
    component: SaleOrdersDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.SALE_ORDERS_MANAGEMENT.NEW.TITLE,
      requiredPolicy: `${AppPermissions.SaleOrders.Default}`,
    },
  },
  {
    path: `:id/${AppRoutes.SALE_ORDERS_MANAGEMENT.DETAILS.BASE}`,
    component: SaleOrdersDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.SALE_ORDERS_MANAGEMENT.DETAILS.TITLE,
      requiredPolicy: `${AppPermissions.SaleOrders.Default}`,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SaleOrdersRoutingModule {}

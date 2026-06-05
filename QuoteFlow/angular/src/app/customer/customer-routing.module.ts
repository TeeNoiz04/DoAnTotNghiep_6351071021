import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { CustomerComponent } from './components/customer.component';

export const routes: Routes = [
  {
    path: '',
    component: CustomerComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: 'Customers',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CustomerRoutingModule {}

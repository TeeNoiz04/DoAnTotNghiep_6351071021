import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { BuyerComponent } from './components/buyer.component';

export const routes: Routes = [
  {
    path: '',
    component: BuyerComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: 'Buyers',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BuyerRoutingModule {}

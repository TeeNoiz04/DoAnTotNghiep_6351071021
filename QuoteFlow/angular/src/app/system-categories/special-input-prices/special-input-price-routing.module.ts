import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { AppRoutes } from '@app/app.routes';
import { SpecialInputPriceDetailComponent } from './components/special-input-price-detail-page.component';
import { SpecialInputPriceComponent } from './components/special-input-price.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: AppRoutes.SPECIAL_INPUT_PRICE.BASE,
    pathMatch: 'full',
  },
  {
    path: `${AppRoutes.SPECIAL_INPUT_PRICE.BASE}`,
    component: SpecialInputPriceComponent,
    data: {
      title: AppRoutes.SPECIAL_INPUT_PRICE.TITLE,
    },
  },
  {
    path: `${AppRoutes.SPECIAL_INPUT_PRICE.BASE}/details/:id`,
    component: SpecialInputPriceDetailComponent,
    data: {
      title: 'Special Input Price Details | Special Input Prices',
      requiredPolicy: `${AppPermissions.SpecialInputPrice.Default}`,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SpecialInputPriceRoutingModule {}

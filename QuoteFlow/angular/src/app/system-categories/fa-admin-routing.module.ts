import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { SaleBuyerComponent } from './sale-of-buyer/components/sale-buyer.component';
import { DistributorTargetComponent } from './distributor-target/components/distributor-target.component';
import { SpecialInputPriceComponent } from './special-input-prices/components/special-input-price.component';
import { SpecialInputPriceDetailComponent } from './special-input-prices/components/special-input-price-detail-page.component';
import { PriceOfferWorkflowComponent } from './workflow-configuration/components/price-offer-workflow/price-offer-workflow.component';
import { MaterialStockWorkflowComponent } from './workflow-configuration/components/material-stock-workflow/material-stock-workflow.component';
import { PSIWorkflowComponent } from './workflow-configuration/components/psi-workflow/psi-workflow.component';
import { KeyAccountWorkflowComponent } from './workflow-configuration/components/key-account-workflow/key-account-workflow.component';
import { SpoDiscountComponent } from './spo-discount/components/spo-discount.component';

export const routes: Routes = [
  {
    path: `${AppRoutes.FA_ADMIN.SALE_TEAM.BASE}`,
    component: SaleBuyerComponent,
    data: {
      title: AppRoutes.FA_ADMIN.SALE_TEAM.TITLE,
    },
  },
  {
    path: `${AppRoutes.FA_ADMIN.DISTRIBUTOR_TARGET.BASE}`,
    component: DistributorTargetComponent,
    data: {
      title: AppRoutes.FA_ADMIN.DISTRIBUTOR_TARGET.TITLE,
    },
  },
  {
    path: `${AppRoutes.FA_ADMIN.SPO_DISCOUNT.BASE}`,
    component: SpoDiscountComponent,
    data: {
      title: AppRoutes.FA_ADMIN.SPO_DISCOUNT.TITLE,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FAAdminRoutingModule {}

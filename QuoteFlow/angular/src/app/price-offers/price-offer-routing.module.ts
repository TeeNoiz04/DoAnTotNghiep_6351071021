import { authGuard, permissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { MyApprovalsPriceOffersComponent } from './components/my-approvals/my-approvals.component';

import { AppPermissions } from '@app/app.permissions';
import { ReportPlaceholderComponent } from '@app/report/components/placeholder/report-placeholder.component';
import { reportModalGuard } from '@app/report/guards/report-modal.guard';
import { ImportBatchRequestDetailComponent } from './components/batch-request/components/import-batch-request-detail.component';
import { ImportBatchRequestComponent } from './components/batch-request/components/import-batch-request.component';
import { PriceOfferDetailComponent } from './components/special-price-offer/price-offer-detail.component';
import { PriceOfferComponent } from './components/special-price-offer/price-offer.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: AppRoutes.SPECIAL_PRICE_OFFERS.LIST.BASE,
    pathMatch: 'full',
  },
  {
    path: `${AppRoutes.SPECIAL_PRICE_OFFERS.LIST.BASE}`,
    component: PriceOfferComponent,
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: `${AppRoutes.SPECIAL_PRICE_OFFERS.MY_APPROVALS.BASE}`,
    component: MyApprovalsPriceOffersComponent,
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: `${AppRoutes.SPECIAL_PRICE_OFFERS.BATCH_REQUEST.BASE}`,
    component: ImportBatchRequestComponent,
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: `${AppRoutes.SPECIAL_PRICE_OFFERS.GENERAL_REPORT.BASE}`,
    canActivate: [reportModalGuard],
    component: ReportPlaceholderComponent,
  },
  {
    path: `${AppRoutes.SPECIAL_PRICE_OFFERS.SPO_DETAILED_REPORT.BASE}`,
    canActivate: [reportModalGuard],
    component: ReportPlaceholderComponent,
  },

  {
    path: `:id/${AppRoutes.SPECIAL_PRICE_OFFERS.DETAILS.BASE}`,
    component: PriceOfferDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.SPECIAL_PRICE_OFFERS.DETAILS.TITLE,
      requiredPolicy: `${AppPermissions.PriceOffers.Default}`,
    },
  },

  {
    path: `:id/${AppRoutes.SPECIAL_PRICE_OFFERS.DETAILS_BATCH_REQUEST.BASE}`,
    component: ImportBatchRequestDetailComponent,
    canActivate: [authGuard, permissionGuard],
    data: {
      title: AppRoutes.SPECIAL_PRICE_OFFERS.DETAILS_BATCH_REQUEST.TITLE,
      requiredPolicy: `${AppPermissions.PriceOffers.Default}`,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PriceOfferRoutingModule {}

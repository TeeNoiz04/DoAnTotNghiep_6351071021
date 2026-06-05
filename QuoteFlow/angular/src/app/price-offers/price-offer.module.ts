import { NgModule } from '@angular/core';
import { PriceOfferComponent } from './components/special-price-offer/price-offer.component';
import { PriceOfferRoutingModule } from './price-offer-routing.module';
import { MyApprovalsPriceOffersComponent } from './components/my-approvals/my-approvals.component';

@NgModule({
  declarations: [],
  imports: [PriceOfferComponent, MyApprovalsPriceOffersComponent, PriceOfferRoutingModule],
})
export class PriceOfferModule {}

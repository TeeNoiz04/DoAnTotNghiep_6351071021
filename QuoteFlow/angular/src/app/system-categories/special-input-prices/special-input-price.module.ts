import { CommonModule } from '@angular/common'; // Always include CommonModule for feature modules
import { NgModule } from '@angular/core';
import { SpecialInputPriceDetailComponent } from './components/special-input-price-detail-page.component';
import { SpecialInputPriceComponent } from './components/special-input-price.component';
import { SpecialInputPriceRoutingModule } from './special-input-price-routing.module';

@NgModule({
  declarations: [
    // Add any other components declared in this module
  ],
  imports: [
    CommonModule,
    SpecialInputPriceRoutingModule,
    SpecialInputPriceComponent,
    SpecialInputPriceDetailComponent,
  ],
})
export class SpecialInputPriceModule {}

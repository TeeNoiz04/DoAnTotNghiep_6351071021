import { NgModule } from '@angular/core';
import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerComponent } from './components/customer.component';

@NgModule({
  declarations: [],
  imports: [CustomerComponent, CustomerRoutingModule],
})
export class CustomerModule {}

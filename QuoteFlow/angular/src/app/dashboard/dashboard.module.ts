import { NgModule } from '@angular/core';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { AuditLoggingModule } from '@volo/abp.ng.audit-logging';
import { PageModule } from '@abp/ng.components/page';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { HostDashboardComponent } from './host-dashboard/host-dashboard.component';
import { DateRangePickerModule } from '@volo/abp.commercial.ng.ui';

@NgModule({
  imports: [
    DashboardRoutingModule,
    NgbDatepickerModule,
    AuditLoggingModule,
    PageModule,
    DateRangePickerModule,
    DashboardComponent,
    HostDashboardComponent,
  ],
})
export class DashboardModule {}

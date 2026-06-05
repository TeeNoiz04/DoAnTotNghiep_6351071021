import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';

import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { NgSelectModule } from '@ng-select/ng-select';
import { CustomerDetailViewService } from '../services/customer-detail.service';
import { CustomerFilterService } from '../services/customer-filter.service';
import { CustomerViewService } from '../services/customer.service';
import { CustomerDetailModalComponent } from './customer-detail.component';
import { CustomerFilterComponent } from './customer-filter/customer-filter.component';
import {
  AbstractCustomerComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './customer.abstract.component';
import { ImportCustomerModalComponent } from './import-customer-modal/import-customer-modal.component';
import { ResultImportCustomerComponent } from './result-import-customer/result-import-customer.component';
import { ErrorDisplayComponent } from '@app/shared/components';

@Component({
  selector: 'app-customer',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbDropdownModule,
    NgSelectModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    CustomerDetailModalComponent,
    CustomerFilterComponent,
    ErrorDisplayComponent,
    AuditInfoColumnComponent,
    ...ChildComponentDependencies,
    ImportCustomerModalComponent,
    ResultImportCustomerComponent,
    NgbTooltip,
  ],
  providers: [
    ListService,
    CustomerViewService,
    CustomerDetailViewService,
    CustomerFilterService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './customer.component.html',
  styleUrls: [`customer.component.scss`],
})
export class CustomerComponent extends AbstractCustomerComponent {}

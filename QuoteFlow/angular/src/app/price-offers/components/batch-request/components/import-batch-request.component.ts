import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ErrorDisplayComponent } from '@app/shared/components/error-display/error-display.component';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
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

import {
  AbstractImportBatchRequestComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './import-batch-request.abstract.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { BatchRequestManagementViewService } from '../services/batch-request-management.service';
import { ImportBatchRequestViewService } from '../services/import-batch-request.service';
import { ImportBatchRequestModalComponent } from './import-batch-request/import-batch-request-modal/import-batch-request-modal.component';
import { ResultImportBatchRequestComponent } from './import-batch-request/result-import-batch-request/result-import-batch-requet.component';
import { ImportBatchRequestFilterComponent } from './import-batch-request/import-batch-request-filter/import-batch-request-filter.component';

@Component({
  selector: 'app-import-batch-request',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ImportBatchRequestFilterComponent,
    ImportBatchRequestModalComponent,
    ResultImportBatchRequestComponent,
    MatCheckboxModule,
    UsernamePipe,
    StatusLabelComponent,
    ErrorDisplayComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    BatchRequestManagementViewService,
    ImportBatchRequestViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './import-batch-request.component.html',
  styleUrls: [`import-batch-request.component.scss`],
})
export class ImportBatchRequestComponent extends AbstractImportBatchRequestComponent {}

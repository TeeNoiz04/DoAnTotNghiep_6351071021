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
import {
  AbstractMyApprovalsMaterialsComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './my-approvals.abstract.component';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { MyApprovalsFilterComponent } from './my-approvals-filter/my-approvals-filter.component';
import { MaterialViewService } from '../../services/material.service';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';

@Component({
  selector: 'app-my-approvals-materials',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.Default,
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
    StatusLabelComponent,
    MyApprovalsFilterComponent,
    ApproversModalComponent,
    HistoryModalComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    MaterialViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './my-approvals.component.html',
  styleUrls: ['./my-approvals.component.scss'],
})
export class MyApprovalsMaterialsComponent extends AbstractMyApprovalsMaterialsComponent {}

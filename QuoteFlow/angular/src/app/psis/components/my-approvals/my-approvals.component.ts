import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { PSIDetailViewService } from '@app/psis/services/psi-detail.service';
import { PSIViewService } from '@app/psis/services/psi.service';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbCollapseModule, NgbDropdownModule, NgbTooltip, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { MyApprovalsFilterComponent } from './my-approvals-filter/my-approvals-filter.component';
import {
  AbstractMyApprovalsComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './my-approvals.abstract.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';

@Component({
  selector: 'app-my-approvals',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    MyApprovalsFilterComponent,
    StatusLabelComponent,
    HistoryModalComponent,
    NgbTooltipModule,
    ApproversModalComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [ListService, PSIViewService],
  templateUrl: './my-approvals.component.html',
  styleUrls: [`my-approvals.component.scss`],
})
export class MyApprovalsComponent extends AbstractMyApprovalsComponent {}

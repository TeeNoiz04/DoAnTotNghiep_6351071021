import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { PSIViewService } from '@app/psis/services/psi.service';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbCollapseModule, NgbDropdownModule, NgbTooltip, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { PSIFilterComponent } from './psi-filter/psi-filter.component';
import { PSIImportModalComponent } from './psi-import-modal/psi-import-modal.component';
import { AbstractPSIComponent, ChildComponentDependencies, ChildTabDependencies } from './psi.abstract.component';
import { ResultImportPSIComponent } from './result-import-psi/result-import-psi.component';
import { ErrorDisplayComponent } from '@app/shared/components';
import { ApprovalCommentModalComponent } from '@app/shared/components/approval-comment/approval-comment.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';

@Component({
  selector: 'app-psi',
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
    PSIFilterComponent,
    PSIImportModalComponent,
    StatusLabelComponent,
    HistoryModalComponent,
    NgbTooltipModule,
    ErrorDisplayComponent,
    ApproversModalComponent,
    ResultImportPSIComponent,
    ApprovalCommentModalComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [ListService, PSIViewService],
  templateUrl: './psi.component.html',
  styleUrls: ['./psi.component.scss'],
})
export class PSIComponent extends AbstractPSIComponent {}

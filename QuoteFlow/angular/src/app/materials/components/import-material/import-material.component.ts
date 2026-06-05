import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ApprovalCommentModalComponent } from '@app/shared/components/approval-comment/approval-comment.component';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { ErrorDisplayComponent } from '@app/shared/components/error-display/error-display.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
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
import { MaterialService } from '@proxy/materials';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ImportMaterialViewService } from '../../services/import-material.service';
import { ImportMaterialFilterComponent } from './import-material-filter/import-material-filter.component';
import { ImportMaterialModalComponent } from './import-material-modal/import-material-modal.component';
import {
  AbstractImportMaterialComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './import-material.abstract.component';
import { ResultImportMaterialComponent } from './result-import-material/result-import-material.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { WarningDisplayComponent } from '@app/shared/components/warning-display/warning-display.component';

@Component({
  selector: 'app-import-material',
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
    ImportMaterialFilterComponent,
    ImportMaterialModalComponent,
    StatusLabelComponent,
    ApproversModalComponent,
    ApprovalCommentModalComponent,
    HistoryModalComponent,
    ResultImportMaterialComponent,
    UsernamePipe,
    ErrorDisplayComponent,
    AuditInfoColumnComponent,
    EscCloseModalDirective,
    NgbTooltip,
    ...ChildComponentDependencies,
    WarningDisplayComponent,
  ],
  providers: [
    ListService,
    MaterialService,
    ImportMaterialViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './import-material.component.html',
  styleUrls: ['./import-material.component.scss'],
  // styles: `
  //   ::ng-deep.datatable-row-detail {
  //     background: transparent !important;
  //   }
  // `,
})
export class ImportMaterialComponent extends AbstractImportMaterialComponent {}

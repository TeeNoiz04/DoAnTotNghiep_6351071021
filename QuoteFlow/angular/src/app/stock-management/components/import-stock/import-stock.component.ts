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
import { StockManagementService } from '@proxy/stock-managements';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ImportStockViewService } from '../../services/import-stock.service';
import { ImportStockFilterComponent } from './import-stock-filter/import-stock-filter.component';
import { ImportStockModalComponent } from './import-stock-modal/import-stock-modal.component';
import {
  AbstractImportStockComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './import-stock.abstract.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { ResultImportStockComponent } from './result-import-stock/result-import-stock.component';

@Component({
  selector: 'app-import-stock',
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
    ImportStockFilterComponent,
    ImportStockModalComponent,
    ResultImportStockComponent,
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
    StockManagementService,
    ImportStockViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './import-stock.component.html',
  styleUrls: [`./import-stock.component.scss`],
})
export class ImportStockComponent extends AbstractImportStockComponent {}

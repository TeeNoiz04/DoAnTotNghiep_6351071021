import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  NgbDateAdapter,
  NgbTimeAdapter,
  NgbCollapseModule,
  NgbDatepickerModule,
  NgbTimepickerModule,
  NgbDropdownModule,
  NgbTooltipModule,
  NgbTooltip,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ListService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, DateAdapter, TimeAdapter } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';

import {
  AbstractFactoryCategoryComponent,
  ChildTabDependencies,
  ChildComponentDependencies,
} from './factory-category.abstract.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { FactoryViewService } from '../services/factory.service';
import { FactoryDetailViewService } from '../services/factory-detail.service';
import { FactoryDetailModalComponent } from './factory-category-detail.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { ImportSupplierBUModalComponent } from './import-supplier-bu-modal/import-supplier-bu-modal.component';
import { ResultImportSupplierBUComponent } from './result-supplier-bu-priority/result-import-supplier-bu.component';
import { ErrorDisplayComponent } from '@app/shared/components';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-factory-category',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    NgbTooltipModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    FactoryDetailModalComponent,
    NgSelectModule,
    ImportSupplierBUModalComponent,
    ResultImportSupplierBUComponent,
    ErrorDisplayComponent,
    TrimDirective,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    FactoryViewService,
    FactoryDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './factory-category.component.html',
  styleUrls: [`factory-category.component.scss`],
})
export class FactoryCategoryComponent extends AbstractFactoryCategoryComponent {}

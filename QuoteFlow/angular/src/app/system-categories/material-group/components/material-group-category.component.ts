import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  NgbDateAdapter,
  NgbTimeAdapter,
  NgbCollapseModule,
  NgbDatepickerModule,
  NgbTimepickerModule,
  NgbDropdownModule,
  NgbTooltip,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ListService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, DateAdapter, TimeAdapter } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { MaterialGroupCategoryDetailModalComponent } from './material-group-category-detail.component';
import {
  AbstractMaterialGroupCategoryComponent,
  ChildTabDependencies,
  ChildComponentDependencies,
} from './material-group-category.abstract.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { MaterialGroupDetailViewService } from '../services/material-group-category-detail.service';
import { MaterialGroupViewService } from '../services/material-group-category.service';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-material-group-category',
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
    TrimDirective,
    MaterialGroupCategoryDetailModalComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    MaterialGroupViewService,
    MaterialGroupDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './material-group-category.component.html',
  styleUrls: [`material-group-category.component.scss`],
})
export class MaterialGroupCategoryComponent extends AbstractMaterialGroupCategoryComponent {}

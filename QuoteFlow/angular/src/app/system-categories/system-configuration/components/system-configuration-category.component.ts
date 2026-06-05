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
import { SystemConfigurationCategoryDetailModalComponent } from './system-configuration-category-detail.component';
import {
  AbstractSystemConfigurationCategoryComponent,
  ChildTabDependencies,
  ChildComponentDependencies,
} from './system-configuration-category.abstract.component';
import { SystemConfigurationViewService } from '../services/system-configuration-category.service';
import { SystemConfigurationDetailViewService } from '../services/system-configuration-category-detail.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-system-configuration-category',
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
    NgSelectModule,
    TrimDirective,
    SystemConfigurationCategoryDetailModalComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    SystemConfigurationViewService,
    SystemConfigurationDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './system-configuration-category.component.html',
  styleUrls: [`system-configuration-category.component.scss`],
})
export class SystemConfigurationCategoryComponent extends AbstractSystemConfigurationCategoryComponent {}

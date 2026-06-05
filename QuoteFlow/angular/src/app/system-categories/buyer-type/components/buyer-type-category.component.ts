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
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { SystemCategoryDetailViewService } from '@app/system-categories/system-category/services/system-category-detail.service';
import { SystemCategoryViewService } from '@app/system-categories/system-category/services/system-category.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { BuyerTypeCategoryDetailModalComponent } from './buyer-type-category-detail.component';
import {
  AbstractBuyerTypeCategoryComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './buyer-type-category.abstract.component';

@Component({
  selector: 'app-buyer-type-category',
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
    BuyerTypeCategoryDetailModalComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    SystemCategoryViewService,
    SystemCategoryDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './buyer-type-category.component.html',
  styleUrls: [`buyer-type-category.component.scss`],
})
export class BuyerTypeCategoryComponent extends AbstractBuyerTypeCategoryComponent {}

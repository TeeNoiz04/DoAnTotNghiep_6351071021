import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { SystemCategoryViewService } from '@app/system-categories/system-category/services/system-category.service';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { VendorCategoryDetailViewService } from '../service/vendor-category-detail.service';
import { VendorCategoryViewService } from '../service/vendor-category.service';
import { VendorCategoryDetailModalComponent } from './vendor-category-detail.component';
import {
  AbstractVendorCategoryComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './vendor-category.abstract.component';

@Component({
  selector: 'app-vendor-category',
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
    VendorCategoryDetailModalComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    SystemCategoryViewService,
    VendorCategoryDetailViewService,
    VendorCategoryViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './vendor-category.component.html',
  styleUrls: [`vendor-category.component.scss`],
})
export class VendorCategoryComponent extends AbstractVendorCategoryComponent {}

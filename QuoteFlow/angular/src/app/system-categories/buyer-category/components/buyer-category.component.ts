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
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';

import { BuyerCategoryDetailViewService } from '../services/buyer-category-detail.service';
import { BuyerCategoryViewService } from '../services/buyer-category.service';
import { BuyerCategoryDetailModalComponent } from './buyer-category-detail.component';
import {
  AbstractBuyerComponent as AbstractBuyerCategoryComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './buyer-category.abstract.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-buyer',
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
    BuyerCategoryDetailModalComponent,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    BuyerCategoryViewService,
    BuyerCategoryDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './buyer-category.component.html',
  styles: `
    ::ng-deep.datatable-row-detail {
      background: transparent !important;
    }
  `,
})
export class BuyerCategoryComponent extends AbstractBuyerCategoryComponent {}

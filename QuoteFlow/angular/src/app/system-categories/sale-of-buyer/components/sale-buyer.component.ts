import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbNavModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule, ValidationGroupDirective } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';

import { ReactiveFormsModule } from '@angular/forms';
import { TrimDirective } from '@app/shared/directives/trim.directive';
import { NgSelectModule } from '@ng-select/ng-select';
import { SaleBuyerDetailViewService } from '../services/sale-buyer-detail.service';
import { SaleBuyerViewService } from '../services/sale-buyer.service';
import { SaleBuyerDetailModalComponent } from './sale-buyer-detail.component';
import {
  AbstractSaleBuyerCategoryComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './sale-buyer.abstract.component';

@Component({
  selector: 'app-sale-buyer',
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
    SaleBuyerDetailModalComponent,
    NgSelectModule,
    CommercialUiModule,
    ReactiveFormsModule,
    NgbNavModule,
    TrimDirective,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    SaleBuyerViewService,
    SaleBuyerDetailViewService,
    ValidationGroupDirective,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './sale-buyer.component.html',
  styleUrls: [`sale-buyer.component.scss`],
})
export class SaleBuyerComponent extends AbstractSaleBuyerCategoryComponent {}

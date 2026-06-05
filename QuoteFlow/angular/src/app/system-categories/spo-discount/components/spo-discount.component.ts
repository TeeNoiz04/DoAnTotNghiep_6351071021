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
import {
  AbstractSpoDiscountComponent,
  ChildTabDependencies,
  ChildComponentDependencies,
} from './spo-discount.abstract.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { SpoDiscountViewService } from '../services/spo-discount.service';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { SpoDiscountDetailModalComponent } from './spo-discount-detail.component';
import { SPODiscountDetailViewService } from '../services/spo-discount-detail.service';

@Component({
  selector: 'app-spo-discount',
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
    AuditInfoColumnComponent,
    SpoDiscountDetailModalComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    SpoDiscountViewService,
    SPODiscountDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './spo-discount.component.html',
  styleUrls: [`spo-discount.component.scss`],
})
export class SpoDiscountComponent extends AbstractSpoDiscountComponent {}

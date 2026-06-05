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
import { NgSelectModule } from '@ng-select/ng-select';
import { BuyerDetailViewService } from '../services/buyer-detail.service';
import { BuyerFilterService } from '../services/buyer-filter.service';
import { BuyerViewService } from '../services/buyer.service';
import { MaterialGroupBuyerDetailViewService } from '../services/material-group-buyer-detail.service';
import { BuyerDetailModalComponent } from './buyer-detail.component';
import { BuyerFilterComponent } from './buyer-filter/buyer-filter.component';
import { BuyerImportModalComponent } from './buyer-import-modal/buyer-import-modal.component';
import { AbstractBuyerComponent, ChildComponentDependencies, ChildTabDependencies } from './buyer.abstract.component';
import { MaterialGroupBuyerModalComponent } from './material-group-buyer-modal/material-group-buyer-modal.component';

@Component({
  selector: 'app-customer',
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
    BuyerDetailModalComponent,
    BuyerImportModalComponent,
    BuyerFilterComponent,
    NgSelectModule,
    NgbTooltip,
    MaterialGroupBuyerModalComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    BuyerViewService,
    BuyerDetailViewService,
    BuyerFilterService,
    MaterialGroupBuyerDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './buyer.component.html',
  styleUrls: [`buyer.component.scss`],
})
export class BuyerComponent extends AbstractBuyerComponent {}

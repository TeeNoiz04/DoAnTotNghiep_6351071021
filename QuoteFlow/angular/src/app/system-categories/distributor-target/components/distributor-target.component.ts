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
import { NgSelectModule } from '@ng-select/ng-select';
import { DistributorTargetDetailViewService } from '../services/distributor-target-detail.service';
import { DistributorTargetViewService } from '../services/distributor-target.service';
import { DistributorTargetDetailModalComponent } from './distributor-target-detail.component';
import {
  AbstractDistributorTargetComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './distributor-target.abstract.component';

@Component({
  selector: 'app-distributor-target',
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
    DistributorTargetDetailModalComponent,
    NgSelectModule,
    CommercialUiModule,
    ReactiveFormsModule,
    NgbNavModule,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    DistributorTargetViewService,
    DistributorTargetDetailViewService,
    ValidationGroupDirective,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './distributor-target.component.html',
  styleUrls: [`distributor-target.component.scss`],
})
export class DistributorTargetComponent extends AbstractDistributorTargetComponent {}

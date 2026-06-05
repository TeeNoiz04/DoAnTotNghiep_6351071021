import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ListService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import {
  AbstractPriceOfferWorkflowComponent,
  ChildTabDependencies,
  ChildComponentDependencies,
} from './price-offer-workflow.abstract.component';
import { WorkflowConfigurationViewService } from '../../services/workflow-configuration.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { WorkflowConfigurationDetailModalComponent } from '../workflow-configuration-detail.component';
import { WorkflowConfigurationDetailViewService } from '../../services/workflow-configuration-detail.service';

@Component({
  selector: 'app-price-offer-workflow',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    NgSelectModule,
    WorkflowConfigurationDetailModalComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [ListService, WorkflowConfigurationViewService, WorkflowConfigurationDetailViewService],
  templateUrl: './price-offer-workflow.component.html',
  styleUrls: [`price-offer-workflow.component.scss`],
})
export class PriceOfferWorkflowComponent extends AbstractPriceOfferWorkflowComponent {}

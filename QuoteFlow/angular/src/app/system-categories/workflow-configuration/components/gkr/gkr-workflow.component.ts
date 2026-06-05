import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ListService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import {
  AbstractGKRWorkflowComponent,
  ChildTabDependencies,
  ChildComponentDependencies,
} from './gkr-workflow.abstract.component';
import { WorkflowConfigurationViewService } from '../../services/workflow-configuration.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { WorkflowConfigurationDetailModalComponent } from '../workflow-configuration-detail.component';
import { WorkflowConfigurationDetailViewService } from '../../services/workflow-configuration-detail.service';

@Component({
  selector: 'app-gkr-workflow',
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
  templateUrl: './gkr-workflow.component.html',
  styleUrls: [`gkr-workflow.component.scss`],
})
export class GKRWorkflowComponent extends AbstractGKRWorkflowComponent {}

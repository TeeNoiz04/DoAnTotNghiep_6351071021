import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ListService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import {
  AbstractKeyAccountWorkflowComponent,
  ChildTabDependencies,
  ChildComponentDependencies,
} from './key-account-workflow.abstract.component';
import { WorkflowConfigurationViewService } from '../../services/workflow-configuration.service';
import { WorkflowConfigurationDetailModalComponent } from '../workflow-configuration-detail.component';
import { WorkflowConfigurationDetailViewService } from '../../services/workflow-configuration-detail.service';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-key-account-workflow',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    NgSelectModule,
    ThemeSharedModule,
    CommercialUiModule,
    WorkflowConfigurationDetailModalComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [ListService, WorkflowConfigurationViewService, WorkflowConfigurationDetailViewService],
  templateUrl: './key-account-workflow.component.html',
  styleUrls: [`key-account-workflow.component.scss`],
})
export class KeyAccountWorkflowComponent extends AbstractKeyAccountWorkflowComponent {}

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ListService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import {
  AbstractPSIWorkflowComponent,
  ChildTabDependencies,
  ChildComponentDependencies,
} from './psi-workflow.abstract.component';
import { WorkflowConfigurationViewService } from '../../services/workflow-configuration.service';
import { WorkflowConfigurationDetailModalComponent } from '../workflow-configuration-detail.component';
import { WorkflowConfigurationDetailViewService } from '../../services/workflow-configuration-detail.service';

@Component({
  selector: 'app-psi-workflow',
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
    WorkflowConfigurationDetailModalComponent,
    ...ChildComponentDependencies,
  ],
  providers: [ListService, WorkflowConfigurationViewService, WorkflowConfigurationDetailViewService],
  templateUrl: './psi-workflow.component.html',
  styles: `
    ::ng-deep.datatable-row-detail {
      background: transparent !important;
    }
    ::ng-deep .text-right {
      text-align: right !important;
    }
  `,
})
export class PSIWorkflowComponent extends AbstractPSIWorkflowComponent {}

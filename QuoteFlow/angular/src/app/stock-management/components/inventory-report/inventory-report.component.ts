import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import {
  AbstractInventoryReportComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './inventory-report.abstract.component';
import { InventoryReportViewService } from '../../services/inventory-report.service';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';

@Component({
  selector: 'app-inventory-report',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    NgxValidateCoreModule,
    NgxDatatableModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    ExpandablePanelV2Component,
    ...ChildComponentDependencies,
  ],
  providers: [ListService, InventoryReportViewService],
  templateUrl: './inventory-report.component.html',
  styleUrls: ['./inventory-report.component.scss'],
})
export class InventoryReportComponent extends AbstractInventoryReportComponent {}

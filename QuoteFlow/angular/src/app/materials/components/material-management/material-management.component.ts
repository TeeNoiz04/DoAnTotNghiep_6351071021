import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, inject, OnInit, ViewChild } from '@angular/core';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
} from '@ng-bootstrap/ng-bootstrap';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import {
  AbstractMaterialManagementComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './material-management.abstract.component';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { MaterialViewService } from '../../services/material.service';
import { MaterialDetailViewService } from '../../services/material-detail.service';
import { MaterialFilterService } from '../../services/material-filter.service';
import { MaterialManagementFilterComponent } from './components/material-management-filter/material-management-filter.component';
import { TableEdgeScrollerComponent } from '@app/shared/components/table-edge-scroller/table-edge-scroller.component';
import { MaterialHistoryModalComponent } from '../material-history-detail-modal/material-history-modal.component';
import { MaterialDto } from '@proxy/materials';

@Component({
  selector: 'app-material-management',
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
    MatCheckboxModule,
    StatusLabelComponent,
    MaterialManagementFilterComponent,
    TableEdgeScrollerComponent,
    MaterialHistoryModalComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    MaterialViewService,
    MaterialDetailViewService,
    MaterialFilterService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './material-management.component.html',
  styleUrls: ['./material-management.component.scss'],
})
export class MaterialManagementComponent extends AbstractMaterialManagementComponent implements OnInit {
  @ViewChild(MaterialHistoryModalComponent) materialHistoryModal!: MaterialHistoryModalComponent;
  protected readonly filterService = inject(MaterialFilterService);

  override ngOnInit() {
    super.ngOnInit();

    // Initialize the filter service for material management
    this.filterService.initializeForType('management', {
      buildForm: true,
      syncFromQuery: true,
      autoSearch: false,
    });

    // Hook to query parameters
    this.filterService.hookToQuery();
  }

  // Override data access to use new filter service
  get data() {
    return this.filterService.data;
  }

  // Override search and clear methods
  onSearch() {
    this.filterService.search();
  }

  clearFilters() {
    this.filterService.clearFilters();
  }

  // Navigation method for Smart Back Button integration
  onViewDetails(id: string) {
    this.filterService.navigateToDetail(id, 'details');
  }

  // Export method
  exportToExcel() {
    this.filterService.exportToExcel();
  }

  onViewMaterialHistoryDetail(row: MaterialDto) {
    this.materialHistoryModal.open(row);
  }
}

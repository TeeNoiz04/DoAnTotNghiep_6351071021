import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { PriceOfferDetailViewService } from '@app/price-offers/services/price-offer-detail.service';
import { PriceOfferViewService } from '@app/price-offers/services/price-offer.service';
import { PriceOfferMyApprovalsFilterService } from '@app/price-offers/services/price-offer-my-approvals-filter.service';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
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
import { MyApprovalsFilterComponent } from './my-approvals-filter/my-approvals-filter.component';
import {
  AbstractMyApprovalsPriceOffersComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './my-approvals.abstract.component';
import { DataTableComponent } from '@app/shared/components/data-table/data-table.component';
import { HeaderTableComponent } from '@app/shared/components/data-table/header/header.component';
import { ColumnComponent } from '@app/shared/components/data-table/column/column.component';
@Component({
  selector: 'app-my-approvals',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
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
    StatusLabelComponent,
    MyApprovalsFilterComponent,
    HistoryModalComponent,
    UsernamePipe,
    ApproversModalComponent,
    AuditInfoColumnComponent,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    PriceOfferViewService,
    PriceOfferDetailViewService,
    PriceOfferMyApprovalsFilterService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './my-approvals.component.html',
  styleUrls: [`my-approvals.component.scss`],
})
export class MyApprovalsPriceOffersComponent extends AbstractMyApprovalsPriceOffersComponent implements OnInit {
  protected readonly filterService = inject(PriceOfferMyApprovalsFilterService);

  override ngOnInit() {
    super.ngOnInit();

    // Initialize the filter service
    this.filterService.initialize({
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
}

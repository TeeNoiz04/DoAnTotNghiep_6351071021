import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService, TrackByService } from '@abp/ng.core';
import {
  Confirmation,
  ConfirmationService,
  DateAdapter,
  ThemeSharedModule,
  TimeAdapter,
  ToasterService,
} from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DPOExtendedService } from '@app/proxy-custom/dpos/dpo-extended.service';
import { DashboardService, DPOStatusSummaryDto } from '@app/proxy/dashboards';
import { DPODto, ImportDPODto } from '@app/proxy/dpos/models';
import { LookupService } from '@app/proxy/general-lookups';
import { ExcelValidationResult } from '@app/proxy/shared/excels/models';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { ErrorDisplayComponent } from '@app/shared/components/error-display/error-display.component';
import { StatusNavBarComponent, StatusTab } from '@app/shared/components/status-nav-bar';
import { TableEdgeScrollerComponent } from '@app/shared/components/table-edge-scroller/table-edge-scroller.component';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { RequestStatusEnum, StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbModal,
  NgbNavModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { filter, finalize, switchMap, takeUntil } from 'rxjs';
import { DPODetailViewService } from '../services/dpo-detail.service';
import { DpoFilterService } from '../services/dpo-filter.service';
import { DPOViewService } from '../services/dpo.service';
import { DpoFilterComponent } from './dpo-filter/dpo-filter.component';
import { AbstractDPOComponent, ChildComponentDependencies, ChildTabDependencies } from './dpo.abstract.component';
import { ImportDPOModalComponent } from './import-dpo-modal/import-dpo-modal.component';
import { ResultImportDpoComponent } from './result-import-dpo/result-import-dpo.component';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';

@Component({
  selector: 'app-dpo',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CommonModule,
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbDropdownModule,
    NgbNavModule,
    NgbTooltipModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    FormsModule,
    ReactiveFormsModule,
    StatusNavBarComponent,
    RouterModule,
    NgSelectModule,
    StatusLabelComponent,
    ImportDPOModalComponent,
    ResultImportDpoComponent,
    ErrorDisplayComponent,
    DpoFilterComponent,
    UsernamePipe,
    TableEdgeScrollerComponent,
    AuditInfoColumnComponent,
    HistoryModalComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    DPOViewService,
    DPODetailViewService,
    LookupService,
    LoadingService,
    DPOExtendedService,
    DpoFilterService,
    DashboardService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './dpo.component.html',
  styleUrls: ['./dpo.component.scss'],
  standalone: true,
})
export class DPOComponent extends AbstractDPOComponent implements OnInit {
  protected readonly modal = inject(NgbModal);
  protected readonly lookupService = inject(LookupService);
  protected readonly loadingService = inject(LoadingService);
  protected readonly dpoService = inject(DPOExtendedService);
  protected readonly dashboardService = inject(DashboardService);
  protected readonly toast = inject(ToasterService);
  protected readonly confirmationService = inject(ConfirmationService);
  public readonly track = inject(TrackByService);
  protected readonly fb = inject(FormBuilder);
  protected readonly filterService = inject(DpoFilterService);

  @ViewChild('importDPO') importDPO: ImportDPOModalComponent | undefined;

  // Import workflow state
  importBusy = false;
  submitBusy = false;
  showResultImportDPO = false;
  resultImport: ExcelValidationResult<ImportDPODto> | undefined;
  hasFullBuyerAccess = false;

  // Status tabs configuration
  activeStatusTab = 'all';
  statusTabs: StatusTab[] = [
    { id: 'all', label: 'All', status: null, count: 0 },
    { id: 'submitted', label: 'Submitted', status: RequestStatusEnum.SUBMITTED, count: 0 },
    { id: 'confirmed', label: 'Confirmed', status: RequestStatusEnum.CONFIRMED, count: 0 },
    { id: 'locked_stock', label: 'Locked Stock', status: RequestStatusEnum.LOCKED_STOCK, count: 0 },
    { id: 'in_progress', label: 'In Progress', status: RequestStatusEnum.IN_PROGRESS, count: 0 },
    { id: 'closed', label: 'Closed', status: RequestStatusEnum.CLOSED, count: 0 },
    { id: 'cancelled', label: 'Cancelled', status: RequestStatusEnum.CANCELLED, count: 0 },
    { id: 'rejected', label: 'Rejected', status: RequestStatusEnum.REJECTED, count: 0 },
  ];

  override ngOnInit() {
    super.ngOnInit();

    // Initialize the new filter service
    this.filterService.initialize({
      buildForm: true,
      syncFromQuery: true,
      autoSearch: false,
    });

    // Hook to query parameters
    this.filterService.hookToQuery();
    this.activeStatusTab = this.statusTabs.find(tab => tab.status === this.filterService.filters.status)?.id || 'all';
    // Load status counts for badges
    this.loadStatusCounts();

    // Refresh counts every 5 minutes to keep badges up to date
    setInterval(() => this.loadStatusCounts(), 5 * 60 * 1000);
  }

  // Override data access to use new filter service
  get data() {
    return this.filterService.data;
  }

  // Override search method
  onSearch() {
    this.filterService.search();
  }

  // Override load method
  override onLoad() {
    this.filterService.search();
  }

  // Navigation method for Smart Back Button integration
  onViewDetails(id: string) {
    this.filterService.navigateToDetail(id, 'details');
  }

  // Handle status tab changes
  onStatusTabChange(tabId: string) {
    this.activeStatusTab = tabId;
    const selectedTab = this.statusTabs.find(tab => tab.id === tabId);

    // Update the filter service with the selected status
    this.filterService.filters = {
      ...this.filterService.filters,
      status: selectedTab?.status || null,
    };

    // Update query params and trigger search
    this.filterService.syncFormWithFilters();
    this.filterService.updateQueryParams(true);
    this.filterService.hookToQueryWithCurrentFilters();
    this.loadStatusCounts();
  }

  // Load status counts for badge notifications
  protected loadStatusCounts() {
    this.dashboardService
      .getDPOStatusSummary({ ...this.filterService.filters, status: null })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (counts: DPOStatusSummaryDto) => {
          this.updateStatusTabCounts(counts);
        },
        error: error => {
          console.error('Failed to load status counts:', error);
          // Keep default counts (0) if API fails
        },
      });
  }

  // Public method to refresh status counts (can be called externally)
  refreshStatusCounts() {
    this.loadStatusCounts();
  }

  // Update status tab counts from API response
  private updateStatusTabCounts(counts: DPOStatusSummaryDto) {
    // Map tab IDs to API response properties
    const statusMapping: Record<string, keyof DPOStatusSummaryDto> = {
      submitted: 'submitted',
      confirmed: 'confirmed',
      locked_stock: 'lockedStock',
      in_progress: 'inProgress',
      closed: 'closed',
      cancelled: 'cancelled',
    };

    this.statusTabs = this.statusTabs.map(tab => ({
      ...tab,
      count:
        tab.id === 'all'
          ? Object.values(counts).reduce((sum, count) => sum + count, 0) // Total for 'all' tab
          : counts[statusMapping[tab.id]] || 0,
    }));
  }

  // Import workflow methods
  onImportVerify() {
    const file = this.importDPO?.fileImport;
    const isValidForm = this.importDPO?.importForm.valid;
    const importInfo = this.importDPO?.importForm.getRawValue();

    if (file && file instanceof File && isValidForm && importInfo) {
      this.loadingService.show();
      this.importBusy = true;

      const input = {
        materialType: importInfo.materialType,
        buyerId: importInfo.buyerId,
        buyerTypeId: importInfo.buyerTypeId,
        confirmDate: importInfo.confirmDate || '',
      };

      this.dpoService
        .validateAndParseDPOManual(file, input)
        .pipe(
          finalize(() => {
            this.importBusy = false;
            this.loadingService.hide();
            this.clearFile();
          }),
        )
        .subscribe({
          next: response => {
            this.resultImport = response;
            this.showResultImportDPO = true;
          },
          error: () => {
            this.resultImport = undefined;
            this.toast.error('Failed to process the file. Please check the file format and try again.');
          },
        });
    } else {
      this.resultImport = undefined;
      if (!file) {
        this.toast.error('Please select a file before submitting.');
      } else {
        this.toast.error('Please fill in all required fields.');
        this.importDPO?.importForm.markAllAsTouched();
      }
    }
  }

  onSubmitImport() {
    if (!this.resultImport) {
      this.toast.error('No data to import.');
      return;
    }

    this.performImport(false);
  }

  private performImport(force: boolean) {
    if (!this.resultImport) {
      return;
    }

    this.loadingService.show();
    this.submitBusy = true;

    this.dpoService
      .importDPO(this.resultImport, force)
      .pipe(
        finalize(() => {
          this.submitBusy = false;
          this.loadingService.hide();
        }),
      )
      .subscribe({
        next: () => {
          this.toast.success('DPO imported successfully.');
          this.resultImport = undefined;
          this.showResultImportDPO = false;
          this.showImportModal = false;
          this.filterService.hookToQuery();
          this.loadStatusCounts(); // Refresh badge counts after import
        },
        error: error => {
          this.handleImportError(error);
        },
      });
  }

  private handleImportError(error: any) {
    const errorCode = error?.error?.error?.code;
    const errorMessage = error?.error?.error?.message;

    if (errorCode === 'QuoteFlow:10603007') {
      const confirmation = this.confirmationService.warn(
        errorMessage || 'Price mismatch detected for items without SPO Code. Do you want to proceed?',
        'Confirm Import',
        {
          yesText: 'Yes, Proceed',
          cancelText: 'Cancel',
          dismissible: true,
          hideCancelBtn: false,
          hideYesBtn: false,
        },
      );

      confirmation.subscribe(result => {
        if (result === 'confirm') {
          this.performImport(true);
        }
      });
    } else {
      this.toast.error('Failed to import DPO. Please try again.');
    }
  }

  onBackFromResult() {
    this.showResultImportDPO = false;
    this.showImportModal = true;
  }

  override clearFilters() {
    super.clearFilters();
    this.filterService.clearFilters();
  }

  // Override delete to refresh badge counts after deletion
  override delete(record: DPODto) {
    // Store original delete method to add our refresh logic
    const originalDelete = this.service.delete.bind(this.service);

    // Replace service delete temporarily to add our refresh logic
    this.service.delete = (deletingRecord: DPODto) => {
      this.confirmationService
        .warn('::DeleteConfirmationMessage', '::AreYouSure', {
          messageLocalizationParams: [deletingRecord.dpoNo],
        })
        .pipe(
          filter(status => status === Confirmation.Status.confirm),
          switchMap(() => this.dpoService.delete(deletingRecord.id)),
        )
        .subscribe({
          next: () => {
            this.list.get(); // Refresh the list
            this.loadStatusCounts(); // Refresh badge counts after deletion
          },
        });
    };

    // Call the modified delete method
    this.service.delete(record);

    // Restore original method
    this.service.delete = originalDelete;
  }

  private clearFile() {
    // Clear the file from the import form to prevent errors
    if (this.importDPO) {
      this.importDPO.resetFile();
    }
  }
}
